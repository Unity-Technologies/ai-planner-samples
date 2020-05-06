using System;
using Unity.AI.Planner;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using Unity.AI.Planner.Jobs;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.Match3Plan;

namespace Generated.AI.Planner.Plans.Match3Plan
{
    public struct ActionScheduler :
        ITraitBasedActionScheduler<TraitBasedObject, StateEntityKey, StateData, StateDataContext, StateManager, ActionKey>
    {
        public static readonly Guid SwapRightGuid = Guid.NewGuid();
        public static readonly Guid SwapUpGuid = Guid.NewGuid();

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
            public EntityCommandBuffer SwapRightECB;
            public EntityCommandBuffer SwapUpECB;

            public void Execute()
            {
                // Playback entity changes and output state transition info
                var entityManager = ExclusiveEntityTransaction;

                SwapRightECB.Playback(entityManager);
                for (int i = 0; i < UnexpandedStates.Length; i++)
                {
                    var stateEntity = UnexpandedStates[i].Entity;
                    var SwapRightRefs = entityManager.GetBuffer<SwapRightFixupReference>(stateEntity);
                    for (int j = 0; j < SwapRightRefs.Length; j++)
                        CreatedStateInfo.Enqueue(SwapRightRefs[j].TransitionInfo);
                    entityManager.RemoveComponent(stateEntity, typeof(SwapRightFixupReference));
                }

                SwapUpECB.Playback(entityManager);
                for (int i = 0; i < UnexpandedStates.Length; i++)
                {
                    var stateEntity = UnexpandedStates[i].Entity;
                    var SwapUpRefs = entityManager.GetBuffer<SwapUpFixupReference>(stateEntity);
                    for (int j = 0; j < SwapUpRefs.Length; j++)
                        CreatedStateInfo.Enqueue(SwapUpRefs[j].TransitionInfo);
                    entityManager.RemoveComponent(stateEntity, typeof(SwapUpFixupReference));
                }
            }
        }

        public JobHandle Schedule(JobHandle inputDeps)
        {
            var entityManager = StateManager.EntityManager;
            var SwapRightDataContext = StateManager.GetStateDataContext();
            var SwapRightECB = StateManager.GetEntityCommandBuffer();
            SwapRightDataContext.EntityCommandBuffer = SwapRightECB.ToConcurrent();
            var SwapUpDataContext = StateManager.GetStateDataContext();
            var SwapUpECB = StateManager.GetEntityCommandBuffer();
            SwapUpDataContext.EntityCommandBuffer = SwapUpECB.ToConcurrent();

            var allActionJobs = new NativeArray<JobHandle>(3, Allocator.TempJob)
            {
                [0] = new SwapRight(SwapRightGuid, UnexpandedStates, SwapRightDataContext).Schedule(UnexpandedStates, 0, inputDeps),
                [1] = new SwapUp(SwapUpGuid, UnexpandedStates, SwapUpDataContext).Schedule(UnexpandedStates, 0, inputDeps),
                [2] = entityManager.ExclusiveEntityTransactionDependency
            };

            var allActionJobsHandle = JobHandle.CombineDependencies(allActionJobs);
            allActionJobs.Dispose();

            // Playback entity changes and output state transition info
            var playbackJob = new PlaybackECB()
            {
                ExclusiveEntityTransaction = StateManager.ExclusiveEntityTransaction,
                UnexpandedStates = UnexpandedStates,
                CreatedStateInfo = m_CreatedStateInfo,
                SwapRightECB = SwapRightECB,
                SwapUpECB = SwapUpECB,
            };

            var playbackJobHandle = playbackJob.Schedule(allActionJobsHandle);
            entityManager.ExclusiveEntityTransactionDependency = playbackJobHandle;

            return playbackJobHandle;
        }
    }
}
