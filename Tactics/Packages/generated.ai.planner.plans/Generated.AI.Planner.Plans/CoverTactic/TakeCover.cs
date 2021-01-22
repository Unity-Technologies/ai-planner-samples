using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.AI.Planner;
using Unity.AI.Planner.Traits;
using Unity.Burst;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.CoverTactic;
using Generated.Semantic.Traits.Enums;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Generated.AI.Planner.Plans.CoverTactic
{
    [BurstCompile]
    struct TakeCover : IJobParallelForDefer
    {
        public Guid ActionGuid;
        
        const int k_AgentIndex = 0;
        const int k_CoverIndex = 1;
        const int k_MaxArguments = 2;

        public static readonly string[] parameterNames = {
            "Agent",
            "Cover",
        };

        [ReadOnly] NativeArray<StateEntityKey> m_StatesToExpand;
        StateDataContext m_StateDataContext;

        // local allocations
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> AgentFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> AgentObjectIndices;
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> CoverFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> CoverObjectIndices;

        [NativeDisableContainerSafetyRestriction] NativeList<ActionKey> ArgumentPermutations;
        [NativeDisableContainerSafetyRestriction] NativeList<TakeCoverFixupReference> TransitionInfo;

        bool LocalContainersInitialized => ArgumentPermutations.IsCreated;

        internal TakeCover(Guid guid, NativeList<StateEntityKey> statesToExpand, StateDataContext stateDataContext)
        {
            ActionGuid = guid;
            m_StatesToExpand = statesToExpand.AsDeferredJobArray();
            m_StateDataContext = stateDataContext;
            AgentFilter = default;
            AgentObjectIndices = default;
            CoverFilter = default;
            CoverObjectIndices = default;
            ArgumentPermutations = default;
            TransitionInfo = default;
        }

        void InitializeLocalContainers()
        {
            AgentFilter = new NativeArray<ComponentType>(2, Allocator.Temp){[0] = ComponentType.ReadWrite<Location>(),[1] = ComponentType.ReadWrite<Agent>(),  };
            AgentObjectIndices = new NativeList<int>(2, Allocator.Temp);
            CoverFilter = new NativeArray<ComponentType>(2, Allocator.Temp){[0] = ComponentType.ReadWrite<Cover>(),[1] = ComponentType.ReadWrite<Location>(),  };
            CoverObjectIndices = new NativeList<int>(2, Allocator.Temp);

            ArgumentPermutations = new NativeList<ActionKey>(4, Allocator.Temp);
            TransitionInfo = new NativeList<TakeCoverFixupReference>(ArgumentPermutations.Length, Allocator.Temp);
        }

        public static int GetIndexForParameterName(string parameterName)
        {
            
            if (string.Equals(parameterName, "Agent", StringComparison.OrdinalIgnoreCase))
                 return k_AgentIndex;
            if (string.Equals(parameterName, "Cover", StringComparison.OrdinalIgnoreCase))
                 return k_CoverIndex;

            return -1;
        }

        void GenerateArgumentPermutations(StateData stateData, NativeList<ActionKey> argumentPermutations)
        {
            AgentObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(AgentObjectIndices, AgentFilter);
            
            var AgentComparer = new global::AI.Tactics.PlayOrderComparer() {StateData = stateData};
            
            AgentObjectIndices.Sort(AgentComparer);
            CoverObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(CoverObjectIndices, CoverFilter);
            
            var CoverBuffer = stateData.CoverBuffer;
            var LocationBuffer = stateData.LocationBuffer;
            var AgentBuffer = stateData.AgentBuffer;
            
            
            var validAgentCount = 0;

            for (int i0 = 0; i0 < AgentObjectIndices.Length; i0++)
            {
                var AgentIndex = AgentObjectIndices[i0];
                var AgentObject = stateData.TraitBasedObjects[AgentIndex];
                
                
                
                if (!(AgentBuffer[AgentObject.AgentIndex].Safe == false))
                    continue;
                
                
            
            

            for (int i1 = 0; i1 < CoverObjectIndices.Length; i1++)
            {
                var CoverIndex = CoverObjectIndices[i1];
                var CoverObject = stateData.TraitBasedObjects[CoverIndex];
                
                if (!(CoverBuffer[CoverObject.CoverIndex].SpotTaken == false))
                    continue;
                
                if (!(LocationBuffer[AgentObject.LocationIndex].Position != LocationBuffer[CoverObject.LocationIndex].Position))
                    continue;
                
                
                

                var actionKey = new ActionKey(k_MaxArguments) {
                                                        ActionGuid = ActionGuid,
                                                       [k_AgentIndex] = AgentIndex,
                                                       [k_CoverIndex] = CoverIndex,
                                                    };
                argumentPermutations.Add(actionKey);
            
            }
            
            validAgentCount++;
            if (validAgentCount >= 1)
                break;
            }
        }

        StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> ApplyEffects(ActionKey action, StateEntityKey originalStateEntityKey)
        {
            var originalState = m_StateDataContext.GetStateData(originalStateEntityKey);
            var originalStateObjectBuffer = originalState.TraitBasedObjects;
            var originalCoverObject = originalStateObjectBuffer[action[k_CoverIndex]];
            var originalAgentObject = originalStateObjectBuffer[action[k_AgentIndex]];

            var newState = m_StateDataContext.CopyStateData(originalState);
            var newCoverBuffer = newState.CoverBuffer;
            var newLocationBuffer = newState.LocationBuffer;
            var newAgentBuffer = newState.AgentBuffer;
            {
                    var @Cover = newCoverBuffer[originalCoverObject.CoverIndex];
                    @Cover.@SpotTaken = true;
                    newCoverBuffer[originalCoverObject.CoverIndex] = @Cover;
            }
            {
                    var @Location = newLocationBuffer[originalAgentObject.LocationIndex];
                    @Location.Position = newLocationBuffer[originalCoverObject.LocationIndex].Position;
                    newLocationBuffer[originalAgentObject.LocationIndex] = @Location;
            }
            {
                    var @Agent = newAgentBuffer[originalAgentObject.AgentIndex];
                    @Agent.@Safe = true;
                    newAgentBuffer[originalAgentObject.AgentIndex] = @Agent;
            }
            {
                    var @Agent = newAgentBuffer[originalAgentObject.AgentIndex];
                    @Agent.@Timeline += 1;
                    newAgentBuffer[originalAgentObject.AgentIndex] = @Agent;
            }

            

            var reward = Reward(originalState, action, newState);
            var StateTransitionInfo = new StateTransitionInfo { Probability = 1f, TransitionUtilityValue = reward };
            var resultingStateKey = m_StateDataContext.GetStateDataKey(newState);

            return new StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo>(originalStateEntityKey, action, resultingStateKey, StateTransitionInfo);
        }

        float Reward(StateData originalState, ActionKey action, StateData newState)
        {
            var reward = 25f;
            {
                var param0 = originalState.GetTraitOnObjectAtIndex<Unity.AI.Planner.Traits.Location>(action[0]);
                var param1 = originalState.GetTraitOnObjectAtIndex<Unity.AI.Planner.Traits.Location>(action[1]);
                reward -= new global::Unity.AI.Planner.Navigation.LocationDistance().RewardModifier( param0, param1);
            }

            return reward;
        }

        public void Execute(int jobIndex)
        {
            if (!LocalContainersInitialized)
                InitializeLocalContainers();

            m_StateDataContext.JobIndex = jobIndex;

            var stateEntityKey = m_StatesToExpand[jobIndex];
            var stateData = m_StateDataContext.GetStateData(stateEntityKey);

            ArgumentPermutations.Clear();
            GenerateArgumentPermutations(stateData, ArgumentPermutations);

            TransitionInfo.Clear();
            TransitionInfo.Capacity = math.max(TransitionInfo.Capacity, ArgumentPermutations.Length);
            for (var i = 0; i < ArgumentPermutations.Length; i++)
            {
                TransitionInfo.Add(new TakeCoverFixupReference { TransitionInfo = ApplyEffects(ArgumentPermutations[i], stateEntityKey) });
            }

            // fixups
            var stateEntity = stateEntityKey.Entity;
            var fixupBuffer = m_StateDataContext.EntityCommandBuffer.AddBuffer<TakeCoverFixupReference>(jobIndex, stateEntity);
            fixupBuffer.CopyFrom(TransitionInfo);
        }

        
        public static T GetAgentTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_AgentIndex]);
        }
        
        public static T GetCoverTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_CoverIndex]);
        }
        
    }

    public struct TakeCoverFixupReference : IBufferElementData
    {
        internal StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> TransitionInfo;
    }
}


