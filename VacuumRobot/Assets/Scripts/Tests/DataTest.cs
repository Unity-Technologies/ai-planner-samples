using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Unity.AI.Planner.DataTests
{
    /*
     * Major jobs:
     *     - (not ECS) Pick some entities
     *     - Parallel action jobs (readonly state data access, queue ECB changes)
     *     - ECB playback
     *     - Read new data, decide to keep, queue deletions on ECB
     *     - ECB playback to delete
     *     - Read surviving entities, evaluate some function
     *
     * Just do a simple addition example
     *     - Pick numbers not yet expanded
     *     - Expand in parallel
     *     - ECB playback
     *     - read new entities, queue deletions
     *     - ECB playback to delete
     *     - Read surviving entities, output values
     *
     *    add 1,3,7
     *
     *     1
     *     1,2,4,8
     *     2 -> 3,5,9
     *     4 -> 5,7,11
     *     8 -> 9,11,15
     */

    struct IntData : IBufferElementData
    {
        public int Value;
    }

    struct BoolData : IBufferElementData
    {
        public bool Value;
    }

    struct GeneratedEntityData : IBufferElementData
    {
        public Entity GeneratedEntity;
    }

    class TestJobComponentSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps) => inputDeps;
    }

    [BurstCompile]
    struct ChooseNumbers : IJob
    {
        [ReadOnly] public NativeHashMap<int, Entity> EntityMap;
        [ReadOnly] public int NumEntitiesToChoose;

        [WriteOnly] public NativeList<Entity> ChosenEntities;

        public void Execute()
        {
            ChosenEntities.Clear();
            var keys = EntityMap.GetKeyArray(Allocator.Temp);
            keys.Sort();
            var numEntities = keys.Length;

            for (int i = 1; i <= Math.Min(NumEntitiesToChoose, numEntities); i++)
            {
                ChosenEntities.Add(EntityMap[keys[numEntities - i]]);
            }
        }
    }

    [BurstCompile]
    struct AddNumbers : IJobParallelForDefer
    {
        [ReadOnly,NativeDisableContainerSafetyRestriction]
        public BufferFromEntity<IntData> IntBufferData;

        [ReadOnly,NativeDisableContainerSafetyRestriction]
        public BufferFromEntity<BoolData> BoolBufferData;

        [ReadOnly] public NativeArray<Entity> ChosenEntities;
        [ReadOnly] public NativeArray<int> NumbersToAdd;

        [WriteOnly] public EntityCommandBuffer.ParallelWriter ECB;

        public void Execute(int index)
        {
            var chosenEntity = ChosenEntities[index];
            var chosenEntityValue = IntBufferData[chosenEntity][0].Value;

            var chosenBoolBuffer = BoolBufferData[chosenEntity];
            if (chosenBoolBuffer[0].Value)
                return;

            var updatedBuffer = ECB.SetBuffer<BoolData>(index, chosenEntity);
            updatedBuffer.Add(new BoolData { Value = true });

            var generatedBuffer = ECB.AddBuffer<GeneratedEntityData>(index, chosenEntity);

            for (int i = 0; i < NumbersToAdd.Length; i++)
            {
                var numToAdd = NumbersToAdd[i];

                var newEntity = ECB.CreateEntity(index);
                var intBuffer = ECB.AddBuffer<IntData>(index, newEntity);
                intBuffer.Add(new IntData { Value = chosenEntityValue + numToAdd });
                var boolBuffer = ECB.AddBuffer<BoolData>(index, newEntity);
                boolBuffer.Add(new BoolData { Value = false });

                generatedBuffer.Add(new GeneratedEntityData{ GeneratedEntity = newEntity });
            }
        }
    }

    struct PlaybackCreateECB : IJob
    {
        [ReadOnly] public NativeList<Entity> ParentEntities;

        public ExclusiveEntityTransaction ExclusiveEntityTransaction;
        public EntityCommandBuffer ECB;
        public NativeList<Entity> CreatedEntities; // Now realized after ECB playback (not temp values)

        public void Execute()
        {
            CreatedEntities.Clear();
            ECB.Playback(ExclusiveEntityTransaction);

            for (int i = 0; i < ParentEntities.Length; i++)
            {
                var entity = ParentEntities[i];
                if (!ExclusiveEntityTransaction.HasComponent(entity, typeof(GeneratedEntityData))) // occurs if earlier job fails
                    continue;

                var reference = ExclusiveEntityTransaction.GetBuffer<GeneratedEntityData>(entity);
                for (int j = 0; j < reference.Length; j++)
                {
                    CreatedEntities.Add(reference[j].GeneratedEntity);
                }
                ExclusiveEntityTransaction.RemoveComponent(entity, typeof(GeneratedEntityData));
            }
        }
    }

    [BurstCompile]
    struct AppendNewNumbersToSet : IJobParallelForDefer
    {

        [ReadOnly] public NativeArray<Entity> EntitiesToAdd;

        [ReadOnly,NativeDisableContainerSafetyRestriction]
        public BufferFromEntity<IntData> IntBufferData;

        [ReadOnly,NativeDisableContainerSafetyRestriction]
        public BufferFromEntity<BoolData> BoolBufferData;


        [WriteOnly] public NativeHashMap<int, Entity>.ParallelWriter IntToEntityLookup;
        [WriteOnly] public EntityCommandBuffer.ParallelWriter ECB;

        public void Execute(int index)
        {
            if (EntitiesToAdd.Length == 0)
                return;

            var newEntity = EntitiesToAdd[index];
            var value = IntBufferData[newEntity][0].Value;

            if (!IntToEntityLookup.TryAdd(value, newEntity))
                ECB.DestroyEntity(index, newEntity);
        }
    }

    struct PlaybackDestroyECB : IJob
    {
        public ExclusiveEntityTransaction ExclusiveEntityTransaction;
        public EntityCommandBuffer ECB;

        public void Execute()
        {
            ECB.Playback(ExclusiveEntityTransaction);
        }
    }


    [TestFixture]
    public class DataTest
    {
        [Test]
        public void TestJobs()
        {
            const int k_NumIterations = 3;
            const int k_NumEntitiesToChoose = 3;
            var numsToAdd = new NativeArray<int>(3, Allocator.Persistent){ [0] = 1, [1] = 3, [2] = 7};

            // setup
            var world = new World("Test World");
            var system = new TestJobComponentSystem();
            world.AddSystem(system);
            var entityManager = world.EntityManager;

            // initial entity
            var firstEntity = entityManager.CreateEntity(typeof(IntData), typeof(BoolData));
            var firstEntityBuffer = entityManager.GetBuffer<IntData>(firstEntity);
            firstEntityBuffer.Add(new IntData{ Value = 0 });
            var boolBuffer = entityManager.GetBuffer<BoolData>(firstEntity);
            boolBuffer.Add(new BoolData{ Value = false });

            // entity data
            var intBufferFromEntity = system.GetBufferFromEntity<IntData>(isReadOnly:true);        // happens before exclusivity
            var boolBufferFromEntity = system.GetBufferFromEntity<BoolData>(isReadOnly:true);
            var exclusiveEntityTransaction = entityManager.BeginExclusiveEntityTransaction();

            // job data containers
            var valToEntityLookup = new NativeHashMap<int, Entity>(100, Allocator.Persistent) { { 0, firstEntity } };
            var commandBuffers = new List<EntityCommandBuffer>();
            var chosenEntities = new NativeList<Entity>(Allocator.Persistent);
            var createdEntities = new NativeList<Entity>(Allocator.Persistent);

            JobHandle jobHandle = default;
            for (int i = 0; i < k_NumIterations; i++)
            {
                // ecbs and other containers
                var createECB = new EntityCommandBuffer(Allocator.TempJob);
                var destroyECB = new EntityCommandBuffer(Allocator.TempJob);
                commandBuffers.Add(createECB);
                commandBuffers.Add(destroyECB);


                // schedule job sequence
                jobHandle = new ChooseNumbers
                {
                    EntityMap = valToEntityLookup,
                    NumEntitiesToChoose = k_NumEntitiesToChoose,

                    ChosenEntities = chosenEntities
                }.Schedule(jobHandle);

                jobHandle = new AddNumbers
                {
                    ChosenEntities = chosenEntities.AsDeferredJobArray(),
                    IntBufferData = intBufferFromEntity,
                    BoolBufferData = boolBufferFromEntity,
                    NumbersToAdd = numsToAdd,

                    ECB = createECB.AsParallelWriter(),
                }.Schedule(chosenEntities, default, jobHandle);

                jobHandle = new PlaybackCreateECB
                {
                    ParentEntities = chosenEntities,

                    ExclusiveEntityTransaction = exclusiveEntityTransaction,
                    ECB = createECB,
                    CreatedEntities = createdEntities,
                }.Schedule(jobHandle);

                jobHandle = new AppendNewNumbersToSet
                {
                    EntitiesToAdd = createdEntities.AsDeferredJobArray(),
                    IntBufferData = intBufferFromEntity,
                    BoolBufferData = boolBufferFromEntity,

                    IntToEntityLookup = valToEntityLookup.AsParallelWriter(),
                    ECB = destroyECB.AsParallelWriter(),
                }.Schedule(createdEntities, default, jobHandle);

                jobHandle = new PlaybackDestroyECB
                {
                    ECB = destroyECB,
                    ExclusiveEntityTransaction = exclusiveEntityTransaction,
                }.Schedule(jobHandle);
            }

            // Complete job
            jobHandle.Complete();
            entityManager.EndExclusiveEntityTransaction();

            // Output results
            var keyValueArrays = valToEntityLookup.GetKeyValueArrays(Allocator.TempJob);
            for (int i = 0; i < keyValueArrays.Length; i++)
            {
                Debug.Log($"{keyValueArrays.Keys[i]}: {keyValueArrays.Values[i]}");
            }
            keyValueArrays.Dispose();

            // Cleanup
            foreach (var ecb in commandBuffers)
            {
                ecb.Dispose();
            }
            valToEntityLookup.Dispose();
            createdEntities.Dispose();
            chosenEntities.Dispose();
            world.Dispose();
            numsToAdd.Dispose();
        }

    }
}
