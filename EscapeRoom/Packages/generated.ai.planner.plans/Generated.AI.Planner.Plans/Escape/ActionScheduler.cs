using System;
using Unity.AI.Planner;
using Unity.AI.Planner.Traits;
using Unity.AI.Planner.Jobs;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.Escape;

namespace Generated.AI.Planner.Plans.Escape
{
    public struct ActionScheduler :
        ITraitBasedActionScheduler<TraitBasedObject, StateEntityKey, StateData, StateDataContext, StateManager, ActionKey>
    {
        public static readonly Guid MoveDownGuid = Guid.NewGuid();
        public static readonly Guid MoveLeftGuid = Guid.NewGuid();
        public static readonly Guid MoveRightGuid = Guid.NewGuid();
        public static readonly Guid MoveUpGuid = Guid.NewGuid();
        public static readonly Guid PickupKeyGuid = Guid.NewGuid();
        public static readonly Guid UseDoorLeftGuid = Guid.NewGuid();
        public static readonly Guid UseDoorRightGuid = Guid.NewGuid();
        public static readonly Guid UseGateUpGuid = Guid.NewGuid();

        // Input
        public NativeList<StateEntityKey> UnexpandedStates { get; set; }
        public StateManager StateManager { get; set; }

        // Output
        NativeQueue<StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo>> IActionScheduler<StateEntityKey, StateData, StateDataContext, StateManager, ActionKey>.CreatedStateInfo
        {
            set => m_CreatedStateInfo = value;
        }

        NativeQueue<StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo>> m_CreatedStateInfo;

        struct PlaybackECB : IJob
        {
            public ExclusiveEntityTransaction ExclusiveEntityTransaction;

            [ReadOnly]
            public NativeList<StateEntityKey> UnexpandedStates;
            public NativeQueue<StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo>> CreatedStateInfo;
            public EntityCommandBuffer MoveDownECB;
            public EntityCommandBuffer MoveLeftECB;
            public EntityCommandBuffer MoveRightECB;
            public EntityCommandBuffer MoveUpECB;
            public EntityCommandBuffer PickupKeyECB;
            public EntityCommandBuffer UseDoorLeftECB;
            public EntityCommandBuffer UseDoorRightECB;
            public EntityCommandBuffer UseGateUpECB;

            public void Execute()
            {
                // Playback entity changes and output state transition info
                var entityManager = ExclusiveEntityTransaction;

                MoveDownECB.Playback(entityManager);
                for (int i = 0; i < UnexpandedStates.Length; i++)
                {
                    var stateEntity = UnexpandedStates[i].Entity;
                    var MoveDownRefs = entityManager.GetBuffer<MoveDownFixupReference>(stateEntity);
                    for (int j = 0; j < MoveDownRefs.Length; j++)
                        CreatedStateInfo.Enqueue(MoveDownRefs[j].TransitionInfo);
                    entityManager.RemoveComponent(stateEntity, typeof(MoveDownFixupReference));
                }

                MoveLeftECB.Playback(entityManager);
                for (int i = 0; i < UnexpandedStates.Length; i++)
                {
                    var stateEntity = UnexpandedStates[i].Entity;
                    var MoveLeftRefs = entityManager.GetBuffer<MoveLeftFixupReference>(stateEntity);
                    for (int j = 0; j < MoveLeftRefs.Length; j++)
                        CreatedStateInfo.Enqueue(MoveLeftRefs[j].TransitionInfo);
                    entityManager.RemoveComponent(stateEntity, typeof(MoveLeftFixupReference));
                }

                MoveRightECB.Playback(entityManager);
                for (int i = 0; i < UnexpandedStates.Length; i++)
                {
                    var stateEntity = UnexpandedStates[i].Entity;
                    var MoveRightRefs = entityManager.GetBuffer<MoveRightFixupReference>(stateEntity);
                    for (int j = 0; j < MoveRightRefs.Length; j++)
                        CreatedStateInfo.Enqueue(MoveRightRefs[j].TransitionInfo);
                    entityManager.RemoveComponent(stateEntity, typeof(MoveRightFixupReference));
                }

                MoveUpECB.Playback(entityManager);
                for (int i = 0; i < UnexpandedStates.Length; i++)
                {
                    var stateEntity = UnexpandedStates[i].Entity;
                    var MoveUpRefs = entityManager.GetBuffer<MoveUpFixupReference>(stateEntity);
                    for (int j = 0; j < MoveUpRefs.Length; j++)
                        CreatedStateInfo.Enqueue(MoveUpRefs[j].TransitionInfo);
                    entityManager.RemoveComponent(stateEntity, typeof(MoveUpFixupReference));
                }

                PickupKeyECB.Playback(entityManager);
                for (int i = 0; i < UnexpandedStates.Length; i++)
                {
                    var stateEntity = UnexpandedStates[i].Entity;
                    var PickupKeyRefs = entityManager.GetBuffer<PickupKeyFixupReference>(stateEntity);
                    for (int j = 0; j < PickupKeyRefs.Length; j++)
                        CreatedStateInfo.Enqueue(PickupKeyRefs[j].TransitionInfo);
                    entityManager.RemoveComponent(stateEntity, typeof(PickupKeyFixupReference));
                }

                UseDoorLeftECB.Playback(entityManager);
                for (int i = 0; i < UnexpandedStates.Length; i++)
                {
                    var stateEntity = UnexpandedStates[i].Entity;
                    var UseDoorLeftRefs = entityManager.GetBuffer<UseDoorLeftFixupReference>(stateEntity);
                    for (int j = 0; j < UseDoorLeftRefs.Length; j++)
                        CreatedStateInfo.Enqueue(UseDoorLeftRefs[j].TransitionInfo);
                    entityManager.RemoveComponent(stateEntity, typeof(UseDoorLeftFixupReference));
                }

                UseDoorRightECB.Playback(entityManager);
                for (int i = 0; i < UnexpandedStates.Length; i++)
                {
                    var stateEntity = UnexpandedStates[i].Entity;
                    var UseDoorRightRefs = entityManager.GetBuffer<UseDoorRightFixupReference>(stateEntity);
                    for (int j = 0; j < UseDoorRightRefs.Length; j++)
                        CreatedStateInfo.Enqueue(UseDoorRightRefs[j].TransitionInfo);
                    entityManager.RemoveComponent(stateEntity, typeof(UseDoorRightFixupReference));
                }

                UseGateUpECB.Playback(entityManager);
                for (int i = 0; i < UnexpandedStates.Length; i++)
                {
                    var stateEntity = UnexpandedStates[i].Entity;
                    var UseGateUpRefs = entityManager.GetBuffer<UseGateUpFixupReference>(stateEntity);
                    for (int j = 0; j < UseGateUpRefs.Length; j++)
                        CreatedStateInfo.Enqueue(UseGateUpRefs[j].TransitionInfo);
                    entityManager.RemoveComponent(stateEntity, typeof(UseGateUpFixupReference));
                }
            }
        }

        public JobHandle Schedule(JobHandle inputDeps)
        {
            var entityManager = StateManager.ExclusiveEntityTransaction.EntityManager;
            var MoveDownDataContext = StateManager.StateDataContext;
            var MoveDownECB = StateManager.GetEntityCommandBuffer();
            MoveDownDataContext.EntityCommandBuffer = MoveDownECB.AsParallelWriter();
            var MoveLeftDataContext = StateManager.StateDataContext;
            var MoveLeftECB = StateManager.GetEntityCommandBuffer();
            MoveLeftDataContext.EntityCommandBuffer = MoveLeftECB.AsParallelWriter();
            var MoveRightDataContext = StateManager.StateDataContext;
            var MoveRightECB = StateManager.GetEntityCommandBuffer();
            MoveRightDataContext.EntityCommandBuffer = MoveRightECB.AsParallelWriter();
            var MoveUpDataContext = StateManager.StateDataContext;
            var MoveUpECB = StateManager.GetEntityCommandBuffer();
            MoveUpDataContext.EntityCommandBuffer = MoveUpECB.AsParallelWriter();
            var PickupKeyDataContext = StateManager.StateDataContext;
            var PickupKeyECB = StateManager.GetEntityCommandBuffer();
            PickupKeyDataContext.EntityCommandBuffer = PickupKeyECB.AsParallelWriter();
            var UseDoorLeftDataContext = StateManager.StateDataContext;
            var UseDoorLeftECB = StateManager.GetEntityCommandBuffer();
            UseDoorLeftDataContext.EntityCommandBuffer = UseDoorLeftECB.AsParallelWriter();
            var UseDoorRightDataContext = StateManager.StateDataContext;
            var UseDoorRightECB = StateManager.GetEntityCommandBuffer();
            UseDoorRightDataContext.EntityCommandBuffer = UseDoorRightECB.AsParallelWriter();
            var UseGateUpDataContext = StateManager.StateDataContext;
            var UseGateUpECB = StateManager.GetEntityCommandBuffer();
            UseGateUpDataContext.EntityCommandBuffer = UseGateUpECB.AsParallelWriter();

            var allActionJobs = new NativeArray<JobHandle>(9, Allocator.TempJob)
            {
                [0] = new MoveDown(MoveDownGuid, UnexpandedStates, MoveDownDataContext).Schedule(UnexpandedStates, 0, inputDeps),
                [1] = new MoveLeft(MoveLeftGuid, UnexpandedStates, MoveLeftDataContext).Schedule(UnexpandedStates, 0, inputDeps),
                [2] = new MoveRight(MoveRightGuid, UnexpandedStates, MoveRightDataContext).Schedule(UnexpandedStates, 0, inputDeps),
                [3] = new MoveUp(MoveUpGuid, UnexpandedStates, MoveUpDataContext).Schedule(UnexpandedStates, 0, inputDeps),
                [4] = new PickupKey(PickupKeyGuid, UnexpandedStates, PickupKeyDataContext).Schedule(UnexpandedStates, 0, inputDeps),
                [5] = new UseDoorLeft(UseDoorLeftGuid, UnexpandedStates, UseDoorLeftDataContext).Schedule(UnexpandedStates, 0, inputDeps),
                [6] = new UseDoorRight(UseDoorRightGuid, UnexpandedStates, UseDoorRightDataContext).Schedule(UnexpandedStates, 0, inputDeps),
                [7] = new UseGateUp(UseGateUpGuid, UnexpandedStates, UseGateUpDataContext).Schedule(UnexpandedStates, 0, inputDeps),
                [8] = entityManager.ExclusiveEntityTransactionDependency
            };

            var allActionJobsHandle = JobHandle.CombineDependencies(allActionJobs);
            allActionJobs.Dispose();

            // Playback entity changes and output state transition info
            var playbackJob = new PlaybackECB()
            {
                ExclusiveEntityTransaction = StateManager.ExclusiveEntityTransaction,
                UnexpandedStates = UnexpandedStates,
                CreatedStateInfo = m_CreatedStateInfo,
                MoveDownECB = MoveDownECB,
                MoveLeftECB = MoveLeftECB,
                MoveRightECB = MoveRightECB,
                MoveUpECB = MoveUpECB,
                PickupKeyECB = PickupKeyECB,
                UseDoorLeftECB = UseDoorLeftECB,
                UseDoorRightECB = UseDoorRightECB,
                UseGateUpECB = UseGateUpECB,
            };

            var playbackJobHandle = playbackJob.Schedule(allActionJobsHandle);
            entityManager.ExclusiveEntityTransactionDependency = playbackJobHandle;

            return playbackJobHandle;
        }
    }
}
