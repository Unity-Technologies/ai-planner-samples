using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.AI.Planner;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using Unity.Burst;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.Clean;

namespace Generated.AI.Planner.Plans.Clean
{
    [BurstCompile]
    struct Navigate : IJobParallelForDefer
    {
        public Guid ActionGuid;
        
        const int k_MoverIndex = 0;
        const int k_DestinationIndex = 1;
        const int k_MaxArguments = 2;

        public static readonly string[] parameterNames = {
            "Mover",
            "Destination",
        };

        [ReadOnly] NativeArray<StateEntityKey> m_StatesToExpand;
        StateDataContext m_StateDataContext;

        internal Navigate(Guid guid, NativeList<StateEntityKey> statesToExpand, StateDataContext stateDataContext)
        {
            ActionGuid = guid;
            m_StatesToExpand = statesToExpand.AsDeferredJobArray();
            m_StateDataContext = stateDataContext;
        }

        public static int GetIndexForParameterName(string parameterName)
        {
            
            if (string.Equals(parameterName, "Mover", StringComparison.OrdinalIgnoreCase))
                 return k_MoverIndex;
            if (string.Equals(parameterName, "Destination", StringComparison.OrdinalIgnoreCase))
                 return k_DestinationIndex;

            return -1;
        }

        void GenerateArgumentPermutations(StateData stateData, NativeList<ActionKey> argumentPermutations)
        {
            var MoverFilter = new NativeArray<ComponentType>(2, Allocator.Temp){[0] = ComponentType.ReadWrite<Generated.AI.Planner.StateRepresentation.Moveable>(),[1] = ComponentType.ReadWrite<Unity.AI.Planner.DomainLanguage.TraitBased.Location>(),  };
            var DestinationFilter = new NativeArray<ComponentType>(2, Allocator.Temp){[0] = ComponentType.ReadWrite<Unity.AI.Planner.DomainLanguage.TraitBased.Location>(), [1] = ComponentType.Exclude<Generated.AI.Planner.StateRepresentation.Moveable>(),  };
            var MoverObjectIndices = new NativeList<int>(2, Allocator.Temp);
            stateData.GetTraitBasedObjectIndices(MoverObjectIndices, MoverFilter);
            
            var DestinationObjectIndices = new NativeList<int>(2, Allocator.Temp);
            stateData.GetTraitBasedObjectIndices(DestinationObjectIndices, DestinationFilter);
            
            var LocationBuffer = stateData.LocationBuffer;
            
            

            for (int i0 = 0; i0 < MoverObjectIndices.Length; i0++)
            {
                var MoverIndex = MoverObjectIndices[i0];
                var MoverObject = stateData.TraitBasedObjects[MoverIndex];
                
                
                
            
            

            for (int i1 = 0; i1 < DestinationObjectIndices.Length; i1++)
            {
                var DestinationIndex = DestinationObjectIndices[i1];
                var DestinationObject = stateData.TraitBasedObjects[DestinationIndex];
                
                if (!(LocationBuffer[MoverObject.LocationIndex].Position != LocationBuffer[DestinationObject.LocationIndex].Position))
                    continue;
                
                

                var actionKey = new ActionKey(k_MaxArguments) {
                                                        ActionGuid = ActionGuid,
                                                       [k_MoverIndex] = MoverIndex,
                                                       [k_DestinationIndex] = DestinationIndex,
                                                    };
                argumentPermutations.Add(actionKey);
            
            }
            
            }
            MoverObjectIndices.Dispose();
            DestinationObjectIndices.Dispose();
            MoverFilter.Dispose();
            DestinationFilter.Dispose();
        }

        StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> ApplyEffects(ActionKey action, StateEntityKey originalStateEntityKey)
        {
            var originalState = m_StateDataContext.GetStateData(originalStateEntityKey);
            var originalStateObjectBuffer = originalState.TraitBasedObjects;
            var originalMoverObject = originalStateObjectBuffer[action[k_MoverIndex]];
            var originalDestinationObject = originalStateObjectBuffer[action[k_DestinationIndex]];

            var newState = m_StateDataContext.CopyStateData(originalState);
            var newLocationBuffer = newState.LocationBuffer;
            {
                    var @Location = newLocationBuffer[originalMoverObject.LocationIndex];
                    @Location.Position = newLocationBuffer[originalDestinationObject.LocationIndex].Position;
                    newLocationBuffer[originalMoverObject.LocationIndex] = @Location;
            }

            

            var reward = Reward(originalState, action, newState);
            var StateTransitionInfo = new StateTransitionInfo { Probability = 1f, TransitionUtilityValue = reward };
            var resultingStateKey = m_StateDataContext.GetStateDataKey(newState);

            return new StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo>(originalStateEntityKey, action, resultingStateKey, StateTransitionInfo);
        }

        float Reward(StateData originalState, ActionKey action, StateData newState)
        {
            var reward = 0f;
            {
                var param0 = originalState.GetTraitOnObjectAtIndex<Unity.AI.Planner.DomainLanguage.TraitBased.Location>(action[0]);
                var param1 = originalState.GetTraitOnObjectAtIndex<Unity.AI.Planner.DomainLanguage.TraitBased.Location>(action[1]);
                reward -= new global::Unity.AI.Planner.Navigation.LocationDistance().RewardModifier( param0, param1);
            }

            return reward;
        }

        public void Execute(int jobIndex)
        {
            m_StateDataContext.JobIndex = jobIndex;

            var stateEntityKey = m_StatesToExpand[jobIndex];
            var stateData = m_StateDataContext.GetStateData(stateEntityKey);

            var argumentPermutations = new NativeList<ActionKey>(4, Allocator.Temp);
            GenerateArgumentPermutations(stateData, argumentPermutations);

            var transitionInfo = new NativeArray<NavigateFixupReference>(argumentPermutations.Length, Allocator.Temp);
            for (var i = 0; i < argumentPermutations.Length; i++)
            {
                transitionInfo[i] = new NavigateFixupReference { TransitionInfo = ApplyEffects(argumentPermutations[i], stateEntityKey) };
            }

            // fixups
            var stateEntity = stateEntityKey.Entity;
            var fixupBuffer = m_StateDataContext.EntityCommandBuffer.AddBuffer<NavigateFixupReference>(jobIndex, stateEntity);
            fixupBuffer.CopyFrom(transitionInfo);

            transitionInfo.Dispose();
            argumentPermutations.Dispose();
        }

        
        public static T GetMoverTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_MoverIndex]);
        }
        
        public static T GetDestinationTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_DestinationIndex]);
        }
        
    }

    public struct NavigateFixupReference : IBufferElementData
    {
        internal StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> TransitionInfo;
    }
}


