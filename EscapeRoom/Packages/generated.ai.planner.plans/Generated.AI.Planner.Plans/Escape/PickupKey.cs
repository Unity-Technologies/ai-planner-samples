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
    struct PickupKey : IJobParallelForDefer
    {
        public Guid ActionGuid;
        
        const int k_CharacterIndex = 0;
        const int k_ItemIndex = 1;
        const int k_MaxArguments = 2;

        public static readonly string[] parameterNames = {
            "Character",
            "Item",
        };

        [ReadOnly] NativeArray<StateEntityKey> m_StatesToExpand;
        StateDataContext m_StateDataContext;

        // local allocations
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> CharacterFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> CharacterObjectIndices;
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> ItemFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> ItemObjectIndices;

        [NativeDisableContainerSafetyRestriction] NativeList<ActionKey> ArgumentPermutations;
        [NativeDisableContainerSafetyRestriction] NativeList<PickupKeyFixupReference> TransitionInfo;

        bool LocalContainersInitialized => ArgumentPermutations.IsCreated;

        internal PickupKey(Guid guid, NativeList<StateEntityKey> statesToExpand, StateDataContext stateDataContext)
        {
            ActionGuid = guid;
            m_StatesToExpand = statesToExpand.AsDeferredJobArray();
            m_StateDataContext = stateDataContext;
            CharacterFilter = default;
            CharacterObjectIndices = default;
            ItemFilter = default;
            ItemObjectIndices = default;
            ArgumentPermutations = default;
            TransitionInfo = default;
        }

        void InitializeLocalContainers()
        {
            CharacterFilter = new NativeArray<ComponentType>(2, Allocator.Temp){[0] = ComponentType.ReadWrite<Character>(),[1] = ComponentType.ReadWrite<Carrier>(),  };
            CharacterObjectIndices = new NativeList<int>(2, Allocator.Temp);
            ItemFilter = new NativeArray<ComponentType>(3, Allocator.Temp){[0] = ComponentType.ReadWrite<Item>(),[1] = ComponentType.ReadWrite<Position>(),[2] = ComponentType.ReadWrite<Carriable>(),  };
            ItemObjectIndices = new NativeList<int>(2, Allocator.Temp);

            ArgumentPermutations = new NativeList<ActionKey>(4, Allocator.Temp);
            TransitionInfo = new NativeList<PickupKeyFixupReference>(ArgumentPermutations.Length, Allocator.Temp);
        }

        public static int GetIndexForParameterName(string parameterName)
        {
            
            if (string.Equals(parameterName, "Character", StringComparison.OrdinalIgnoreCase))
                 return k_CharacterIndex;
            if (string.Equals(parameterName, "Item", StringComparison.OrdinalIgnoreCase))
                 return k_ItemIndex;

            return -1;
        }

        void GenerateArgumentPermutations(StateData stateData, NativeList<ActionKey> argumentPermutations)
        {
            CharacterObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(CharacterObjectIndices, CharacterFilter);
            
            ItemObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(ItemObjectIndices, ItemFilter);
            
            var CharacterBuffer = stateData.CharacterBuffer;
            var CarriableBuffer = stateData.CarriableBuffer;
            var CarrierBuffer = stateData.CarrierBuffer;
            var PositionBuffer = stateData.PositionBuffer;
            
            

            for (int i0 = 0; i0 < CharacterObjectIndices.Length; i0++)
            {
                var CharacterIndex = CharacterObjectIndices[i0];
                var CharacterObject = stateData.TraitBasedObjects[CharacterIndex];
                
                
                
                if (!(CarrierBuffer[CharacterObject.CarrierIndex].Carried == TraitBasedObjectId.None))
                    continue;
                
                
            
            

            for (int i1 = 0; i1 < ItemObjectIndices.Length; i1++)
            {
                var ItemIndex = ItemObjectIndices[i1];
                var ItemObject = stateData.TraitBasedObjects[ItemIndex];
                
                if (!(CharacterBuffer[CharacterObject.CharacterIndex].Waypoint == PositionBuffer[ItemObject.PositionIndex].Waypoint))
                    continue;
                
                if (!(CarriableBuffer[ItemObject.CarriableIndex].CarriedBy == TraitBasedObjectId.None))
                    continue;
                
                
                

                var actionKey = new ActionKey(k_MaxArguments) {
                                                        ActionGuid = ActionGuid,
                                                       [k_CharacterIndex] = CharacterIndex,
                                                       [k_ItemIndex] = ItemIndex,
                                                    };
                argumentPermutations.Add(actionKey);
            
            }
            
            }
        }

        StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> ApplyEffects(ActionKey action, StateEntityKey originalStateEntityKey)
        {
            var originalState = m_StateDataContext.GetStateData(originalStateEntityKey);
            var originalStateObjectBuffer = originalState.TraitBasedObjects;
            var originalItemObject = originalStateObjectBuffer[action[k_ItemIndex]];
            var originalCharacterObject = originalStateObjectBuffer[action[k_CharacterIndex]];

            var newState = m_StateDataContext.CopyStateData(originalState);
            var newCarriableBuffer = newState.CarriableBuffer;
            var newCarrierBuffer = newState.CarrierBuffer;
            {
                    var @Carriable = newCarriableBuffer[originalItemObject.CarriableIndex];
                    @Carriable.@CarriedBy = originalState.GetTraitBasedObjectId(originalCharacterObject);
                    newCarriableBuffer[originalItemObject.CarriableIndex] = @Carriable;
            }
            {
                    var @Carrier = newCarrierBuffer[originalCharacterObject.CarrierIndex];
                    @Carrier.@Carried = originalState.GetTraitBasedObjectId(originalItemObject);
                    newCarrierBuffer[originalCharacterObject.CarrierIndex] = @Carrier;
            }

            

            var reward = Reward(originalState, action, newState);
            var StateTransitionInfo = new StateTransitionInfo { Probability = 1f, TransitionUtilityValue = reward };
            var resultingStateKey = m_StateDataContext.GetStateDataKey(newState);

            return new StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo>(originalStateEntityKey, action, resultingStateKey, StateTransitionInfo);
        }

        float Reward(StateData originalState, ActionKey action, StateData newState)
        {
            var reward = 1f;

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
                TransitionInfo.Add(new PickupKeyFixupReference { TransitionInfo = ApplyEffects(ArgumentPermutations[i], stateEntityKey) });
            }

            // fixups
            var stateEntity = stateEntityKey.Entity;
            var fixupBuffer = m_StateDataContext.EntityCommandBuffer.AddBuffer<PickupKeyFixupReference>(jobIndex, stateEntity);
            fixupBuffer.CopyFrom(TransitionInfo);
        }

        
        public static T GetCharacterTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_CharacterIndex]);
        }
        
        public static T GetItemTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_ItemIndex]);
        }
        
    }

    public struct PickupKeyFixupReference : IBufferElementData
    {
        internal StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> TransitionInfo;
    }
}


