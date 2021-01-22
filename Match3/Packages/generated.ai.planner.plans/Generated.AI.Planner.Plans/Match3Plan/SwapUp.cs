using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.AI.Planner;
using Unity.AI.Planner.Traits;
using Unity.Burst;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.Match3Plan;
using Generated.Semantic.Traits.Enums;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Generated.AI.Planner.Plans.Match3Plan
{
    [BurstCompile]
    struct SwapUp : IJobParallelForDefer
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

        // local allocations
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> GameFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> GameObjectIndices;
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> SourceFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> SourceObjectIndices;
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> TargetFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> TargetObjectIndices;

        [NativeDisableContainerSafetyRestriction] NativeList<ActionKey> ArgumentPermutations;
        [NativeDisableContainerSafetyRestriction] NativeList<SwapUpFixupReference> TransitionInfo;

        bool LocalContainersInitialized => ArgumentPermutations.IsCreated;

        internal SwapUp(Guid guid, NativeList<StateEntityKey> statesToExpand, StateDataContext stateDataContext)
        {
            ActionGuid = guid;
            m_StatesToExpand = statesToExpand.AsDeferredJobArray();
            m_StateDataContext = stateDataContext;
            GameFilter = default;
            GameObjectIndices = default;
            SourceFilter = default;
            SourceObjectIndices = default;
            TargetFilter = default;
            TargetObjectIndices = default;
            ArgumentPermutations = default;
            TransitionInfo = default;
        }

        void InitializeLocalContainers()
        {
            GameFilter = new NativeArray<ComponentType>(1, Allocator.Temp){[0] = ComponentType.ReadWrite<Game>(),  };
            GameObjectIndices = new NativeList<int>(2, Allocator.Temp);
            SourceFilter = new NativeArray<ComponentType>(3, Allocator.Temp){[0] = ComponentType.ReadWrite<Cell>(),[1] = ComponentType.ReadWrite<Coordinate>(), [2] = ComponentType.Exclude<Blocker>(),  };
            SourceObjectIndices = new NativeList<int>(2, Allocator.Temp);
            TargetFilter = new NativeArray<ComponentType>(3, Allocator.Temp){[0] = ComponentType.ReadWrite<Cell>(),[1] = ComponentType.ReadWrite<Coordinate>(), [2] = ComponentType.Exclude<Blocker>(),  };
            TargetObjectIndices = new NativeList<int>(2, Allocator.Temp);

            ArgumentPermutations = new NativeList<ActionKey>(4, Allocator.Temp);
            TransitionInfo = new NativeList<SwapUpFixupReference>(ArgumentPermutations.Length, Allocator.Temp);
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
            GameObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(GameObjectIndices, GameFilter);
            
            SourceObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(SourceObjectIndices, SourceFilter);
            
            TargetObjectIndices.Clear();
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
                
                if (!(CellBuffer[SourceObject.CellIndex].Top == stateData.GetTraitBasedObjectId(TargetIndex)))
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
                TransitionInfo.Add(new SwapUpFixupReference { TransitionInfo = ApplyEffects(ArgumentPermutations[i], stateEntityKey) });
            }

            // fixups
            var stateEntity = stateEntityKey.Entity;
            var fixupBuffer = m_StateDataContext.EntityCommandBuffer.AddBuffer<SwapUpFixupReference>(jobIndex, stateEntity);
            fixupBuffer.CopyFrom(TransitionInfo);
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

    public struct SwapUpFixupReference : IBufferElementData
    {
        internal StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> TransitionInfo;
    }
}


