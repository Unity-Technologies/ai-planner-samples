using System;
using Unity.AI.Planner;
using Unity.AI.Planner.Traits;
using Unity.AI.Planner.Jobs;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.CoverTactic;

namespace Generated.AI.Planner.Plans.CoverTactic
{
    public struct ActionScheduler :
        ITraitBasedActionScheduler<TraitBasedObject, StateEntityKey, StateData, StateDataContext, StateManager, ActionKey>
    {
        public static readonly Guid PickupWeaponGuid = Guid.NewGuid();
        public static readonly Guid TakeCoverGuid = Guid.NewGuid();
        public static readonly Guid SkipTurnGuid = Guid.NewGuid();

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
            public EntityCommandBuffer PickupWeaponECB;
            public EntityCommandBuffer TakeCoverECB;
            public EntityCommandBuffer SkipTurnECB;

            public void Execute()
            {
                // Playback entity changes and output state transition info
                var entityManager = ExclusiveEntityTransaction;

                PickupWeaponECB.Playback(entityManager);
                for (int i = 0; i < UnexpandedStates.Length; i++)
                {
                    var stateEntity = UnexpandedStates[i].Entity;
                    var PickupWeaponRefs = entityManager.GetBuffer<PickupWeaponFixupReference>(stateEntity);
                    for (int j = 0; j < PickupWeaponRefs.Length; j++)
                        CreatedStateInfo.Enqueue(PickupWeaponRefs[j].TransitionInfo);
                    entityManager.RemoveComponent(stateEntity, typeof(PickupWeaponFixupReference));
                }

                TakeCoverECB.Playback(entityManager);
                for (int i = 0; i < UnexpandedStates.Length; i++)
                {
                    var stateEntity = UnexpandedStates[i].Entity;
                    var TakeCoverRefs = entityManager.GetBuffer<TakeCoverFixupReference>(stateEntity);
                    for (int j = 0; j < TakeCoverRefs.Length; j++)
                        CreatedStateInfo.Enqueue(TakeCoverRefs[j].TransitionInfo);
                    entityManager.RemoveComponent(stateEntity, typeof(TakeCoverFixupReference));
                }

                SkipTurnECB.Playback(entityManager);
                for (int i = 0; i < UnexpandedStates.Length; i++)
                {
                    var stateEntity = UnexpandedStates[i].Entity;
                    var SkipTurnRefs = entityManager.GetBuffer<SkipTurnFixupReference>(stateEntity);
                    for (int j = 0; j < SkipTurnRefs.Length; j++)
                        CreatedStateInfo.Enqueue(SkipTurnRefs[j].TransitionInfo);
                    entityManager.RemoveComponent(stateEntity, typeof(SkipTurnFixupReference));
                }
            }
        }

        public JobHandle Schedule(JobHandle inputDeps)
        {
            var entityManager = StateManager.ExclusiveEntityTransaction.EntityManager;
            var PickupWeaponDataContext = StateManager.StateDataContext;
            var PickupWeaponECB = StateManager.GetEntityCommandBuffer();
            PickupWeaponDataContext.EntityCommandBuffer = PickupWeaponECB.AsParallelWriter();
            var TakeCoverDataContext = StateManager.StateDataContext;
            var TakeCoverECB = StateManager.GetEntityCommandBuffer();
            TakeCoverDataContext.EntityCommandBuffer = TakeCoverECB.AsParallelWriter();
            var SkipTurnDataContext = StateManager.StateDataContext;
            var SkipTurnECB = StateManager.GetEntityCommandBuffer();
            SkipTurnDataContext.EntityCommandBuffer = SkipTurnECB.AsParallelWriter();

            var allActionJobs = new NativeArray<JobHandle>(4, Allocator.TempJob)
            {
                [0] = new PickupWeapon(PickupWeaponGuid, UnexpandedStates, PickupWeaponDataContext).Schedule(UnexpandedStates, 0, inputDeps),
                [1] = new TakeCover(TakeCoverGuid, UnexpandedStates, TakeCoverDataContext).Schedule(UnexpandedStates, 0, inputDeps),
                [2] = new SkipTurn(SkipTurnGuid, UnexpandedStates, SkipTurnDataContext).Schedule(UnexpandedStates, 0, inputDeps),
                [3] = entityManager.ExclusiveEntityTransactionDependency
            };

            var allActionJobsHandle = JobHandle.CombineDependencies(allActionJobs);
            allActionJobs.Dispose();

            // Playback entity changes and output state transition info
            var playbackJob = new PlaybackECB()
            {
                ExclusiveEntityTransaction = StateManager.ExclusiveEntityTransaction,
                UnexpandedStates = UnexpandedStates,
                CreatedStateInfo = m_CreatedStateInfo,
                PickupWeaponECB = PickupWeaponECB,
                TakeCoverECB = TakeCoverECB,
                SkipTurnECB = SkipTurnECB,
            };

            var playbackJobHandle = playbackJob.Schedule(allActionJobsHandle);
            entityManager.ExclusiveEntityTransactionDependency = playbackJobHandle;

            return playbackJobHandle;
        }
    }
}
