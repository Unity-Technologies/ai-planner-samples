using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.AI.Planner;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using Unity.Burst;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.Match3Plan;
using Generated.AI.Planner.StateRepresentation.Enums;

namespace Generated.AI.Planner.Plans.Match3Plan
{
    [BurstCompile]
    struct SwapRight : IJobParallelForDefer
    {
        public Guid ActionGuid;
        
        const int k_GameIndex = 0;
        const int k_SourceIndex = 1;
        const int k_TargetIndex = 2;
        const int k_MaxArguments = 3;

        public static readonly string[] parameterNames = {
            "Game",
            "Source",
            "Target",
        };

        [ReadOnly] NativeArray<StateEntityKey> m_StatesToExpand;
        StateDataContext m_StateDataContext;

        internal SwapRight(Guid guid, NativeList<StateEntityKey> statesToExpand, StateDataContext stateDataContext)
        {
            ActionGuid = guid;
            m_StatesToExpand = statesToExpand.AsDeferredJobArray();
            m_StateDataContext = stateDataContext;
        }

        public static int GetIndexForParameterName(string parameterName)
        {
            
            if (string.Equals(parameterName, "Game", StringComparison.OrdinalIgnoreCase))
                 return k_GameIndex;
            if (string.Equals(parameterName, "Source", StringComparison.OrdinalIgnoreCase))
                 return k_SourceIndex;
            if (string.Equals(parameterName, "Target", StringComparison.OrdinalIgnoreCase))
                 return k_TargetIndex;

            return -1;
        }

        void GenerateArgumentPermutations(StateData stateData, NativeList<ActionKey> argumentPermutations)
        {
            var GameFilter = new NativeArray<ComponentType>(1, Allocator.Temp){[0] = ComponentType.ReadWrite<Generated.AI.Planner.StateRepresentation.Game>(),  };
            var SourceFilter = new NativeArray<ComponentType>(3, Allocator.Temp){[0] = ComponentType.ReadWrite<Generated.AI.Planner.StateRepresentation.Coordinate>(),[1] = ComponentType.ReadWrite<Generated.AI.Planner.StateRepresentation.Cell>(), [2] = ComponentType.Exclude<Generated.AI.Planner.StateRepresentation.Blocker>(),  };
            var TargetFilter = new NativeArray<ComponentType>(3, Allocator.Temp){[0] = ComponentType.ReadWrite<Generated.AI.Planner.StateRepresentation.Coordinate>(),[1] = ComponentType.ReadWrite<Generated.AI.Planner.StateRepresentation.Cell>(), [2] = ComponentType.Exclude<Generated.AI.Planner.StateRepresentation.Blocker>(),  };
            var GameObjectIndices = new NativeList<int>(2, Allocator.Temp);
            stateData.GetTraitBasedObjectIndices(GameObjectIndices, GameFilter);
            
            var SourceObjectIndices = new NativeList<int>(2, Allocator.Temp);
            stateData.GetTraitBasedObjectIndices(SourceObjectIndices, SourceFilter);
            
            var TargetObjectIndices = new NativeList<int>(2, Allocator.Temp);
            stateData.GetTraitBasedObjectIndices(TargetObjectIndices, TargetFilter);
            
            var CellBuffer = stateData.CellBuffer;
            
            

            for (int i0 = 0; i0 < GameObjectIndices.Length; i0++)
            {
                var GameIndex = GameObjectIndices[i0];
                var GameObject = stateData.TraitBasedObjects[GameIndex];
                
                
                
                
                
                
            
            

            for (int i1 = 0; i1 < SourceObjectIndices.Length; i1++)
            {
                var SourceIndex = SourceObjectIndices[i1];
                var SourceObject = stateData.TraitBasedObjects[SourceIndex];
                
                
                if (!(CellBuffer[SourceObject.CellIndex].Type != CellType.None))
                    continue;
                
                
                
                
            
            

            for (int i2 = 0; i2 < TargetObjectIndices.Length; i2++)
            {
                var TargetIndex = TargetObjectIndices[i2];
                var TargetObject = stateData.TraitBasedObjects[TargetIndex];
                
                if (!(CellBuffer[SourceObject.CellIndex].Right == stateData.GetTraitBasedObjectId(TargetIndex)))
                    continue;
                
                
                if (!(CellBuffer[TargetObject.CellIndex].Type != CellType.None))
                    continue;
                
                
                

                var actionKey = new ActionKey(k_MaxArguments) {
                                                        ActionGuid = ActionGuid,
                                                       [k_GameIndex] = GameIndex,
                                                       [k_SourceIndex] = SourceIndex,
                                                       [k_TargetIndex] = TargetIndex,
                                                    };
                argumentPermutations.Add(actionKey);
            
            }
            
            }
            
            }
            GameObjectIndices.Dispose();
            SourceObjectIndices.Dispose();
            TargetObjectIndices.Dispose();
            GameFilter.Dispose();
            SourceFilter.Dispose();
            TargetFilter.Dispose();
        }

        StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> ApplyEffects(ActionKey action, StateEntityKey originalStateEntityKey)
        {
            var originalState = m_StateDataContext.GetStateData(originalStateEntityKey);
            var originalStateObjectBuffer = originalState.TraitBasedObjects;
            var originalGameObject = originalStateObjectBuffer[action[k_GameIndex]];

            var newState = m_StateDataContext.CopyStateData(originalState);
            var newGameBuffer = newState.GameBuffer;
            {
                    var @Game = newGameBuffer[originalGameObject.GameIndex];
                    @Game.@MoveCount += 1;
                    newGameBuffer[originalGameObject.GameIndex] = @Game;
            }
            {
                    new global::AI.Planner.Custom.Match3Plan.CustomSwapEffect().ApplyCustomActionEffectsToState(originalState, action, newState);
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
                reward += new global::AI.Planner.Custom.Match3Plan.CustomSwapReward().RewardModifier( originalState, action, newState);
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

            var transitionInfo = new NativeArray<SwapRightFixupReference>(argumentPermutations.Length, Allocator.Temp);
            for (var i = 0; i < argumentPermutations.Length; i++)
            {
                transitionInfo[i] = new SwapRightFixupReference { TransitionInfo = ApplyEffects(argumentPermutations[i], stateEntityKey) };
            }

            // fixups
            var stateEntity = stateEntityKey.Entity;
            var fixupBuffer = m_StateDataContext.EntityCommandBuffer.AddBuffer<SwapRightFixupReference>(jobIndex, stateEntity);
            fixupBuffer.CopyFrom(transitionInfo);

            transitionInfo.Dispose();
            argumentPermutations.Dispose();
        }

        
        public static T GetGameTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_GameIndex]);
        }
        
        public static T GetSourceTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_SourceIndex]);
        }
        
        public static T GetTargetTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_TargetIndex]);
        }
        
    }

    public struct SwapRightFixupReference : IBufferElementData
    {
        internal StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> TransitionInfo;
    }
}


