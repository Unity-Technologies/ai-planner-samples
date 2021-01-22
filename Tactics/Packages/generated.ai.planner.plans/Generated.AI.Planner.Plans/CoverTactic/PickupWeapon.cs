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
    struct PickupWeapon : IJobParallelForDefer
    {
        public Guid ActionGuid;
        
        const int k_AgentIndex = 0;
        const int k_ItemIndex = 1;
        const int k_MaxArguments = 2;

        public static readonly string[] parameterNames = {
            "Agent",
            "Item",
        };

        [ReadOnly] NativeArray<StateEntityKey> m_StatesToExpand;
        StateDataContext m_StateDataContext;

        // local allocations
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> AgentFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> AgentObjectIndices;
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> ItemFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> ItemObjectIndices;

        [NativeDisableContainerSafetyRestriction] NativeList<ActionKey> ArgumentPermutations;
        [NativeDisableContainerSafetyRestriction] NativeList<PickupWeaponFixupReference> TransitionInfo;

        bool LocalContainersInitialized => ArgumentPermutations.IsCreated;

        internal PickupWeapon(Guid guid, NativeList<StateEntityKey> statesToExpand, StateDataContext stateDataContext)
        {
            ActionGuid = guid;
            m_StatesToExpand = statesToExpand.AsDeferredJobArray();
            m_StateDataContext = stateDataContext;
            AgentFilter = default;
            AgentObjectIndices = default;
            ItemFilter = default;
            ItemObjectIndices = default;
            ArgumentPermutations = default;
            TransitionInfo = default;
        }

        void InitializeLocalContainers()
        {
            AgentFilter = new NativeArray<ComponentType>(3, Allocator.Temp){[0] = ComponentType.ReadWrite<Agent>(),[1] = ComponentType.ReadWrite<Location>(),[2] = ComponentType.ReadWrite<Moveable>(),  };
            AgentObjectIndices = new NativeList<int>(2, Allocator.Temp);
            ItemFilter = new NativeArray<ComponentType>(2, Allocator.Temp){[0] = ComponentType.ReadWrite<Item>(),[1] = ComponentType.ReadWrite<Location>(),  };
            ItemObjectIndices = new NativeList<int>(2, Allocator.Temp);

            ArgumentPermutations = new NativeList<ActionKey>(4, Allocator.Temp);
            TransitionInfo = new NativeList<PickupWeaponFixupReference>(ArgumentPermutations.Length, Allocator.Temp);
        }

        public static int GetIndexForParameterName(string parameterName)
        {
            
            if (string.Equals(parameterName, "Agent", StringComparison.OrdinalIgnoreCase))
                 return k_AgentIndex;
            if (string.Equals(parameterName, "Item", StringComparison.OrdinalIgnoreCase))
                 return k_ItemIndex;

            return -1;
        }

        void GenerateArgumentPermutations(StateData stateData, NativeList<ActionKey> argumentPermutations)
        {
            AgentObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(AgentObjectIndices, AgentFilter);
            
            var AgentComparer = new global::AI.Tactics.PlayOrderComparer() {StateData = stateData};
            
            AgentObjectIndices.Sort(AgentComparer);
            ItemObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(ItemObjectIndices, ItemFilter);
            
            var AgentBuffer = stateData.AgentBuffer;
            var ItemBuffer = stateData.ItemBuffer;
            
            
            var validAgentCount = 0;

            for (int i0 = 0; i0 < AgentObjectIndices.Length; i0++)
            {
                var AgentIndex = AgentObjectIndices[i0];
                var AgentObject = stateData.TraitBasedObjects[AgentIndex];
                
                if (!(AgentBuffer[AgentObject.AgentIndex].HasWeapon == false))
                    continue;
                
                
                
            
            

            for (int i1 = 0; i1 < ItemObjectIndices.Length; i1++)
            {
                var ItemIndex = ItemObjectIndices[i1];
                var ItemObject = stateData.TraitBasedObjects[ItemIndex];
                
                
                if (!(ItemBuffer[ItemObject.ItemIndex].CarriedBy == TraitBasedObjectId.None))
                    continue;
                
                

                var actionKey = new ActionKey(k_MaxArguments) {
                                                        ActionGuid = ActionGuid,
                                                       [k_AgentIndex] = AgentIndex,
                                                       [k_ItemIndex] = ItemIndex,
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
            var originalAgentObject = originalStateObjectBuffer[action[k_AgentIndex]];
            var originalItemObject = originalStateObjectBuffer[action[k_ItemIndex]];

            var newState = m_StateDataContext.CopyStateData(originalState);
            var newAgentBuffer = newState.AgentBuffer;
            var newLocationBuffer = newState.LocationBuffer;
            var newItemBuffer = newState.ItemBuffer;
            {
                    var @Agent = newAgentBuffer[originalAgentObject.AgentIndex];
                    @Agent.@HasWeapon = true;
                    newAgentBuffer[originalAgentObject.AgentIndex] = @Agent;
            }
            {
                    var @Location = newLocationBuffer[originalAgentObject.LocationIndex];
                    @Location.Position = newLocationBuffer[originalItemObject.LocationIndex].Position;
                    newLocationBuffer[originalAgentObject.LocationIndex] = @Location;
            }
            {
                    var @Item = newItemBuffer[originalItemObject.ItemIndex];
                    @Item.@CarriedBy = originalState.GetTraitBasedObjectId(originalAgentObject);
                    newItemBuffer[originalItemObject.ItemIndex] = @Item;
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
            var reward = 15f;

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
                TransitionInfo.Add(new PickupWeaponFixupReference { TransitionInfo = ApplyEffects(ArgumentPermutations[i], stateEntityKey) });
            }

            // fixups
            var stateEntity = stateEntityKey.Entity;
            var fixupBuffer = m_StateDataContext.EntityCommandBuffer.AddBuffer<PickupWeaponFixupReference>(jobIndex, stateEntity);
            fixupBuffer.CopyFrom(TransitionInfo);
        }

        
        public static T GetAgentTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_AgentIndex]);
        }
        
        public static T GetItemTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_ItemIndex]);
        }
        
    }

    public struct PickupWeaponFixupReference : IBufferElementData
    {
        internal StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> TransitionInfo;
    }
}


