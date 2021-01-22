using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.AI.Planner;
using Unity.AI.Planner.Traits;
using Unity.Burst;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.Escape;
using Generated.Semantic.Traits.Enums;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Generated.AI.Planner.Plans.Escape
{
    [BurstCompile]
    struct MoveDown : IJobParallelForDefer
    {
        public Guid ActionGuid;
        
        const int k_CharacterIndex = 0;
        const int k_ToIndex = 1;
        const int k_FromIndex = 2;
        const int k_MaxArguments = 3;

        public static readonly string[] parameterNames = {
            "Character",
            "To",
            "From",
        };

        [ReadOnly] NativeArray<StateEntityKey> m_StatesToExpand;
        StateDataContext m_StateDataContext;

        // local allocations
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> CharacterFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> CharacterObjectIndices;
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> ToFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> ToObjectIndices;
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> FromFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> FromObjectIndices;

        [NativeDisableContainerSafetyRestriction] NativeList<ActionKey> ArgumentPermutations;
        [NativeDisableContainerSafetyRestriction] NativeList<MoveDownFixupReference> TransitionInfo;

        bool LocalContainersInitialized => ArgumentPermutations.IsCreated;

        internal MoveDown(Guid guid, NativeList<StateEntityKey> statesToExpand, StateDataContext stateDataContext)
        {
            ActionGuid = guid;
            m_StatesToExpand = statesToExpand.AsDeferredJobArray();
            m_StateDataContext = stateDataContext;
            CharacterFilter = default;
            CharacterObjectIndices = default;
            ToFilter = default;
            ToObjectIndices = default;
            FromFilter = default;
            FromObjectIndices = default;
            ArgumentPermutations = default;
            TransitionInfo = default;
        }

        void InitializeLocalContainers()
        {
            CharacterFilter = new NativeArray<ComponentType>(1, Allocator.Temp){[0] = ComponentType.ReadWrite<Character>(),  };
            CharacterObjectIndices = new NativeList<int>(2, Allocator.Temp);
            ToFilter = new NativeArray<ComponentType>(3, Allocator.Temp){[0] = ComponentType.ReadWrite<Waypoint>(), [1] = ComponentType.Exclude<ActivationLock>(), [2] = ComponentType.Exclude<KeyLock>(),  };
            ToObjectIndices = new NativeList<int>(2, Allocator.Temp);
            FromFilter = new NativeArray<ComponentType>(1, Allocator.Temp){[0] = ComponentType.ReadWrite<Waypoint>(),  };
            FromObjectIndices = new NativeList<int>(2, Allocator.Temp);

            ArgumentPermutations = new NativeList<ActionKey>(4, Allocator.Temp);
            TransitionInfo = new NativeList<MoveDownFixupReference>(ArgumentPermutations.Length, Allocator.Temp);
        }

        public static int GetIndexForParameterName(string parameterName)
        {
            
            if (string.Equals(parameterName, "Character", StringComparison.OrdinalIgnoreCase))
                 return k_CharacterIndex;
            if (string.Equals(parameterName, "To", StringComparison.OrdinalIgnoreCase))
                 return k_ToIndex;
            if (string.Equals(parameterName, "From", StringComparison.OrdinalIgnoreCase))
                 return k_FromIndex;

            return -1;
        }

        void GenerateArgumentPermutations(StateData stateData, NativeList<ActionKey> argumentPermutations)
        {
            CharacterObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(CharacterObjectIndices, CharacterFilter);
            
            ToObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(ToObjectIndices, ToFilter);
            
            FromObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(FromObjectIndices, FromFilter);
            
            var WaypointBuffer = stateData.WaypointBuffer;
            var CharacterBuffer = stateData.CharacterBuffer;
            
            

            for (int i0 = 0; i0 < CharacterObjectIndices.Length; i0++)
            {
                var CharacterIndex = CharacterObjectIndices[i0];
                var CharacterObject = stateData.TraitBasedObjects[CharacterIndex];
                
                
                
                
                
                
            
            

            for (int i1 = 0; i1 < ToObjectIndices.Length; i1++)
            {
                var ToIndex = ToObjectIndices[i1];
                var ToObject = stateData.TraitBasedObjects[ToIndex];
                
                if (!(WaypointBuffer[ToObject.WaypointIndex].Occupied == false))
                    continue;
                
                if (!(WaypointBuffer[ToObject.WaypointIndex].Up == CharacterBuffer[CharacterObject.CharacterIndex].Waypoint))
                    continue;
                
                
                
                
            
            

            for (int i2 = 0; i2 < FromObjectIndices.Length; i2++)
            {
                var FromIndex = FromObjectIndices[i2];
                var FromObject = stateData.TraitBasedObjects[FromIndex];
                
                
                
                if (!(CharacterBuffer[CharacterObject.CharacterIndex].Waypoint == stateData.GetTraitBasedObjectId(FromIndex)))
                    continue;
                
                
                

                var actionKey = new ActionKey(k_MaxArguments) {
                                                        ActionGuid = ActionGuid,
                                                       [k_CharacterIndex] = CharacterIndex,
                                                       [k_ToIndex] = ToIndex,
                                                       [k_FromIndex] = FromIndex,
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
            var originalCharacterObject = originalStateObjectBuffer[action[k_CharacterIndex]];
            var originalToObject = originalStateObjectBuffer[action[k_ToIndex]];
            var originalFromObject = originalStateObjectBuffer[action[k_FromIndex]];

            var newState = m_StateDataContext.CopyStateData(originalState);
            var newCharacterBuffer = newState.CharacterBuffer;
            var newWaypointBuffer = newState.WaypointBuffer;
            {
                    var @Character = newCharacterBuffer[originalCharacterObject.CharacterIndex];
                    @Character.@Waypoint = originalState.GetTraitBasedObjectId(originalToObject);
                    newCharacterBuffer[originalCharacterObject.CharacterIndex] = @Character;
            }
            {
                    var @Waypoint = newWaypointBuffer[originalToObject.WaypointIndex];
                    @Waypoint.@Occupied = true;
                    newWaypointBuffer[originalToObject.WaypointIndex] = @Waypoint;
            }
            {
                    var @Waypoint = newWaypointBuffer[originalFromObject.WaypointIndex];
                    @Waypoint.@Occupied = false;
                    newWaypointBuffer[originalFromObject.WaypointIndex] = @Waypoint;
            }

            

            var reward = Reward(originalState, action, newState);
            var StateTransitionInfo = new StateTransitionInfo { Probability = 1f, TransitionUtilityValue = reward };
            var resultingStateKey = m_StateDataContext.GetStateDataKey(newState);

            return new StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo>(originalStateEntityKey, action, resultingStateKey, StateTransitionInfo);
        }

        float Reward(StateData originalState, ActionKey action, StateData newState)
        {
            var reward = -0.1f;

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
                TransitionInfo.Add(new MoveDownFixupReference { TransitionInfo = ApplyEffects(ArgumentPermutations[i], stateEntityKey) });
            }

            // fixups
            var stateEntity = stateEntityKey.Entity;
            var fixupBuffer = m_StateDataContext.EntityCommandBuffer.AddBuffer<MoveDownFixupReference>(jobIndex, stateEntity);
            fixupBuffer.CopyFrom(TransitionInfo);
        }

        
        public static T GetCharacterTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_CharacterIndex]);
        }
        
        public static T GetToTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_ToIndex]);
        }
        
        public static T GetFromTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_FromIndex]);
        }
        
    }

    public struct MoveDownFixupReference : IBufferElementData
    {
        internal StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> TransitionInfo;
    }
}


