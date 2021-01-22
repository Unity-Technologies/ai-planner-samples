using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.AI.Planner;
using Unity.AI.Planner.Traits;
using Unity.AI.Planner.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using PlanningAgent = Unity.AI.Planner.Traits.PlanningAgent;

namespace Generated.AI.Planner.StateRepresentation.Escape
{
    // Plans don't share key types to enforce a specific state representation
    public struct StateEntityKey : IEquatable<StateEntityKey>, IStateKey
    {
        public Entity Entity;
        public int HashCode;

        public bool Equals(StateEntityKey other) => Entity == other.Entity;

        public bool Equals(IStateKey other) => other is StateEntityKey key && Equals(key);

        public override int GetHashCode() => HashCode;

        public override string ToString() => $"StateEntityKey ({Entity} {HashCode})";

        public string Label => $"State{Entity}";
    }

    public static class TraitArrayIndex<T> where T : struct, ITrait
    {
        public static readonly int Index = -1;

        static TraitArrayIndex()
        {
            var typeIndex = TypeManager.GetTypeIndex<T>();
            if (typeIndex == TypeManager.GetTypeIndex<Character>())
                Index = 0;
            else if (typeIndex == TypeManager.GetTypeIndex<Waypoint>())
                Index = 1;
            else if (typeIndex == TypeManager.GetTypeIndex<ActivationLock>())
                Index = 2;
            else if (typeIndex == TypeManager.GetTypeIndex<KeyLock>())
                Index = 3;
            else if (typeIndex == TypeManager.GetTypeIndex<Carrier>())
                Index = 4;
            else if (typeIndex == TypeManager.GetTypeIndex<Item>())
                Index = 5;
            else if (typeIndex == TypeManager.GetTypeIndex<Position>())
                Index = 6;
            else if (typeIndex == TypeManager.GetTypeIndex<Carriable>())
                Index = 7;
            else if (typeIndex == TypeManager.GetTypeIndex<ActivationSwitch>())
                Index = 8;
            else if (typeIndex == TypeManager.GetTypeIndex<EscapePoint>())
                Index = 9;
            else if (typeIndex == TypeManager.GetTypeIndex<PlanningAgent>())
                Index = 10;
        }
    }

    [StructLayout(LayoutKind.Sequential, Size=12)]
    public struct TraitBasedObject : ITraitBasedObject, IEquatable<TraitBasedObject>
    {
        public int Length => 11;

        public byte this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return CharacterIndex;
                    case 1:
                        return WaypointIndex;
                    case 2:
                        return ActivationLockIndex;
                    case 3:
                        return KeyLockIndex;
                    case 4:
                        return CarrierIndex;
                    case 5:
                        return ItemIndex;
                    case 6:
                        return PositionIndex;
                    case 7:
                        return CarriableIndex;
                    case 8:
                        return ActivationSwitchIndex;
                    case 9:
                        return EscapePointIndex;
                    case 10:
                        return PlanningAgentIndex;
                }

                return Unset;
            }
            set
            {
                switch (i)
                {
                    case 0:
                        CharacterIndex = value;
                        break;
                    case 1:
                        WaypointIndex = value;
                        break;
                    case 2:
                        ActivationLockIndex = value;
                        break;
                    case 3:
                        KeyLockIndex = value;
                        break;
                    case 4:
                        CarrierIndex = value;
                        break;
                    case 5:
                        ItemIndex = value;
                        break;
                    case 6:
                        PositionIndex = value;
                        break;
                    case 7:
                        CarriableIndex = value;
                        break;
                    case 8:
                        ActivationSwitchIndex = value;
                        break;
                    case 9:
                        EscapePointIndex = value;
                        break;
                    case 10:
                        PlanningAgentIndex = value;
                        break;
                }
            }
        }

        public static readonly byte Unset = Byte.MaxValue;

        public static TraitBasedObject Default => new TraitBasedObject
        {
            CharacterIndex = Unset,
            WaypointIndex = Unset,
            ActivationLockIndex = Unset,
            KeyLockIndex = Unset,
            CarrierIndex = Unset,
            ItemIndex = Unset,
            PositionIndex = Unset,
            CarriableIndex = Unset,
            ActivationSwitchIndex = Unset,
            EscapePointIndex = Unset,
            PlanningAgentIndex = Unset,
        };


        public byte CharacterIndex;
        public byte WaypointIndex;
        public byte ActivationLockIndex;
        public byte KeyLockIndex;
        public byte CarrierIndex;
        public byte ItemIndex;
        public byte PositionIndex;
        public byte CarriableIndex;
        public byte ActivationSwitchIndex;
        public byte EscapePointIndex;
        public byte PlanningAgentIndex;


        static readonly int s_CharacterTypeIndex = TypeManager.GetTypeIndex<Character>();
        static readonly int s_WaypointTypeIndex = TypeManager.GetTypeIndex<Waypoint>();
        static readonly int s_ActivationLockTypeIndex = TypeManager.GetTypeIndex<ActivationLock>();
        static readonly int s_KeyLockTypeIndex = TypeManager.GetTypeIndex<KeyLock>();
        static readonly int s_CarrierTypeIndex = TypeManager.GetTypeIndex<Carrier>();
        static readonly int s_ItemTypeIndex = TypeManager.GetTypeIndex<Item>();
        static readonly int s_PositionTypeIndex = TypeManager.GetTypeIndex<Position>();
        static readonly int s_CarriableTypeIndex = TypeManager.GetTypeIndex<Carriable>();
        static readonly int s_ActivationSwitchTypeIndex = TypeManager.GetTypeIndex<ActivationSwitch>();
        static readonly int s_EscapePointTypeIndex = TypeManager.GetTypeIndex<EscapePoint>();
        static readonly int s_PlanningAgentTypeIndex = TypeManager.GetTypeIndex<PlanningAgent>();

        public bool HasSameTraits(TraitBasedObject other)
        {
            for (var i = 0; i < Length; i++)
            {
                var traitIndex = this[i];
                var otherTraitIndex = other[i];
                if (traitIndex == Unset && otherTraitIndex != Unset || traitIndex != Unset && otherTraitIndex == Unset)
                    return false;
            }
            return true;
        }

        public bool HasTraitSubset(TraitBasedObject traitSubset)
        {
            for (var i = 0; i < Length; i++)
            {
                var requiredTrait = traitSubset[i];
                if (requiredTrait != Unset && this[i] == Unset)
                    return false;
            }
            return true;
        }

        // todo - replace with more efficient subset check
        public bool MatchesTraitFilter(NativeArray<ComponentType> componentTypes)
        {
            for (int i = 0; i < componentTypes.Length; i++)
            {
                var t = componentTypes[i];
                if (t == default || t.TypeIndex == 0)
                {
                    // This seems to be necessary for Burst compilation; Doesn't happen with non-Burst compilation
                }
                else if (t.TypeIndex == s_CharacterTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ CharacterIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_WaypointTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ WaypointIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_ActivationLockTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ ActivationLockIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_KeyLockTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ KeyLockIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_CarrierTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ CarrierIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_ItemTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ ItemIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_PositionTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ PositionIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_CarriableTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ CarriableIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_ActivationSwitchTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ ActivationSwitchIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_EscapePointTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ EscapePointIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_PlanningAgentTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ PlanningAgentIndex == Unset)
                        return false;
                }
                else
                    throw new ArgumentException($"Incorrect trait type used in object query: {t}");
            }

            return true;
        }

        public bool MatchesTraitFilter(ComponentType[] componentTypes)
        {
            for (int i = 0; i < componentTypes.Length; i++)
            {
                var t = componentTypes[i];
                if (t == default || t == null || t.TypeIndex == 0)
                {
                    // This seems to be necessary for Burst compilation; Doesn't happen with non-Burst compilation
                }
                else if (t.TypeIndex == s_CharacterTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ CharacterIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_WaypointTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ WaypointIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_ActivationLockTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ ActivationLockIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_KeyLockTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ KeyLockIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_CarrierTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ CarrierIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_ItemTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ ItemIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_PositionTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ PositionIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_CarriableTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ CarriableIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_ActivationSwitchTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ ActivationSwitchIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_EscapePointTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ EscapePointIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_PlanningAgentTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ PlanningAgentIndex == Unset)
                        return false;
                }
                else
                    throw new ArgumentException($"Incorrect trait type used in object query: {t}");
            }

            return true;
        }

        public bool Equals(TraitBasedObject other)
        {

                return CharacterIndex == other.CharacterIndex && WaypointIndex == other.WaypointIndex && ActivationLockIndex == other.ActivationLockIndex && KeyLockIndex == other.KeyLockIndex && CarrierIndex == other.CarrierIndex && ItemIndex == other.ItemIndex && PositionIndex == other.PositionIndex && CarriableIndex == other.CarriableIndex && ActivationSwitchIndex == other.ActivationSwitchIndex && EscapePointIndex == other.EscapePointIndex && PlanningAgentIndex == other.PlanningAgentIndex;
        }

        public override bool Equals(object obj)
        {
            return obj is TraitBasedObject other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {

                    var hashCode = CharacterIndex.GetHashCode();
                    
                     hashCode = (hashCode * 397) ^ WaypointIndex.GetHashCode();
                     hashCode = (hashCode * 397) ^ ActivationLockIndex.GetHashCode();
                     hashCode = (hashCode * 397) ^ KeyLockIndex.GetHashCode();
                     hashCode = (hashCode * 397) ^ CarrierIndex.GetHashCode();
                     hashCode = (hashCode * 397) ^ ItemIndex.GetHashCode();
                     hashCode = (hashCode * 397) ^ PositionIndex.GetHashCode();
                     hashCode = (hashCode * 397) ^ CarriableIndex.GetHashCode();
                     hashCode = (hashCode * 397) ^ ActivationSwitchIndex.GetHashCode();
                     hashCode = (hashCode * 397) ^ EscapePointIndex.GetHashCode();
                     hashCode = (hashCode * 397) ^ PlanningAgentIndex.GetHashCode();
                return hashCode;
            }
        }
    }

    public struct StateData : ITraitBasedStateData<TraitBasedObject, StateData>
    {
        public Entity StateEntity;
        public DynamicBuffer<TraitBasedObject> TraitBasedObjects;
        public DynamicBuffer<TraitBasedObjectId> TraitBasedObjectIds;

        public DynamicBuffer<Character> CharacterBuffer;
        public DynamicBuffer<Waypoint> WaypointBuffer;
        public DynamicBuffer<ActivationLock> ActivationLockBuffer;
        public DynamicBuffer<KeyLock> KeyLockBuffer;
        public DynamicBuffer<Carrier> CarrierBuffer;
        public DynamicBuffer<Item> ItemBuffer;
        public DynamicBuffer<Position> PositionBuffer;
        public DynamicBuffer<Carriable> CarriableBuffer;
        public DynamicBuffer<ActivationSwitch> ActivationSwitchBuffer;
        public DynamicBuffer<EscapePoint> EscapePointBuffer;
        public DynamicBuffer<PlanningAgent> PlanningAgentBuffer;

        static readonly int s_CharacterTypeIndex = TypeManager.GetTypeIndex<Character>();
        static readonly int s_WaypointTypeIndex = TypeManager.GetTypeIndex<Waypoint>();
        static readonly int s_ActivationLockTypeIndex = TypeManager.GetTypeIndex<ActivationLock>();
        static readonly int s_KeyLockTypeIndex = TypeManager.GetTypeIndex<KeyLock>();
        static readonly int s_CarrierTypeIndex = TypeManager.GetTypeIndex<Carrier>();
        static readonly int s_ItemTypeIndex = TypeManager.GetTypeIndex<Item>();
        static readonly int s_PositionTypeIndex = TypeManager.GetTypeIndex<Position>();
        static readonly int s_CarriableTypeIndex = TypeManager.GetTypeIndex<Carriable>();
        static readonly int s_ActivationSwitchTypeIndex = TypeManager.GetTypeIndex<ActivationSwitch>();
        static readonly int s_EscapePointTypeIndex = TypeManager.GetTypeIndex<EscapePoint>();
        static readonly int s_PlanningAgentTypeIndex = TypeManager.GetTypeIndex<PlanningAgent>();

        public StateData(ExclusiveEntityTransaction transaction, Entity stateEntity)
        {
            StateEntity = stateEntity;
            TraitBasedObjects = transaction.GetBuffer<TraitBasedObject>(stateEntity);
            TraitBasedObjectIds = transaction.GetBuffer<TraitBasedObjectId>(stateEntity);

            CharacterBuffer = transaction.GetBuffer<Character>(stateEntity);
            WaypointBuffer = transaction.GetBuffer<Waypoint>(stateEntity);
            ActivationLockBuffer = transaction.GetBuffer<ActivationLock>(stateEntity);
            KeyLockBuffer = transaction.GetBuffer<KeyLock>(stateEntity);
            CarrierBuffer = transaction.GetBuffer<Carrier>(stateEntity);
            ItemBuffer = transaction.GetBuffer<Item>(stateEntity);
            PositionBuffer = transaction.GetBuffer<Position>(stateEntity);
            CarriableBuffer = transaction.GetBuffer<Carriable>(stateEntity);
            ActivationSwitchBuffer = transaction.GetBuffer<ActivationSwitch>(stateEntity);
            EscapePointBuffer = transaction.GetBuffer<EscapePoint>(stateEntity);
            PlanningAgentBuffer = transaction.GetBuffer<PlanningAgent>(stateEntity);
        }

        public StateData(int jobIndex, EntityCommandBuffer.ParallelWriter entityCommandBuffer, Entity stateEntity)
        {
            StateEntity = stateEntity;
            TraitBasedObjects = entityCommandBuffer.AddBuffer<TraitBasedObject>(jobIndex, stateEntity);
            TraitBasedObjectIds = entityCommandBuffer.AddBuffer<TraitBasedObjectId>(jobIndex, stateEntity);

            CharacterBuffer = entityCommandBuffer.AddBuffer<Character>(jobIndex, stateEntity);
            WaypointBuffer = entityCommandBuffer.AddBuffer<Waypoint>(jobIndex, stateEntity);
            ActivationLockBuffer = entityCommandBuffer.AddBuffer<ActivationLock>(jobIndex, stateEntity);
            KeyLockBuffer = entityCommandBuffer.AddBuffer<KeyLock>(jobIndex, stateEntity);
            CarrierBuffer = entityCommandBuffer.AddBuffer<Carrier>(jobIndex, stateEntity);
            ItemBuffer = entityCommandBuffer.AddBuffer<Item>(jobIndex, stateEntity);
            PositionBuffer = entityCommandBuffer.AddBuffer<Position>(jobIndex, stateEntity);
            CarriableBuffer = entityCommandBuffer.AddBuffer<Carriable>(jobIndex, stateEntity);
            ActivationSwitchBuffer = entityCommandBuffer.AddBuffer<ActivationSwitch>(jobIndex, stateEntity);
            EscapePointBuffer = entityCommandBuffer.AddBuffer<EscapePoint>(jobIndex, stateEntity);
            PlanningAgentBuffer = entityCommandBuffer.AddBuffer<PlanningAgent>(jobIndex, stateEntity);
        }

        public StateData Copy(int jobIndex, EntityCommandBuffer.ParallelWriter entityCommandBuffer)
        {
            var stateEntity = entityCommandBuffer.Instantiate(jobIndex, StateEntity);
            var traitBasedObjects = entityCommandBuffer.SetBuffer<TraitBasedObject>(jobIndex, stateEntity);
            traitBasedObjects.CopyFrom(TraitBasedObjects.AsNativeArray());
            var traitBasedObjectIds = entityCommandBuffer.SetBuffer<TraitBasedObjectId>(jobIndex, stateEntity);
            traitBasedObjectIds.CopyFrom(TraitBasedObjectIds.AsNativeArray());

            var Characters = entityCommandBuffer.SetBuffer<Character>(jobIndex, stateEntity);
            Characters.CopyFrom(CharacterBuffer.AsNativeArray());
            var Waypoints = entityCommandBuffer.SetBuffer<Waypoint>(jobIndex, stateEntity);
            Waypoints.CopyFrom(WaypointBuffer.AsNativeArray());
            var ActivationLocks = entityCommandBuffer.SetBuffer<ActivationLock>(jobIndex, stateEntity);
            ActivationLocks.CopyFrom(ActivationLockBuffer.AsNativeArray());
            var KeyLocks = entityCommandBuffer.SetBuffer<KeyLock>(jobIndex, stateEntity);
            KeyLocks.CopyFrom(KeyLockBuffer.AsNativeArray());
            var Carriers = entityCommandBuffer.SetBuffer<Carrier>(jobIndex, stateEntity);
            Carriers.CopyFrom(CarrierBuffer.AsNativeArray());
            var Items = entityCommandBuffer.SetBuffer<Item>(jobIndex, stateEntity);
            Items.CopyFrom(ItemBuffer.AsNativeArray());
            var Positions = entityCommandBuffer.SetBuffer<Position>(jobIndex, stateEntity);
            Positions.CopyFrom(PositionBuffer.AsNativeArray());
            var Carriables = entityCommandBuffer.SetBuffer<Carriable>(jobIndex, stateEntity);
            Carriables.CopyFrom(CarriableBuffer.AsNativeArray());
            var ActivationSwitchs = entityCommandBuffer.SetBuffer<ActivationSwitch>(jobIndex, stateEntity);
            ActivationSwitchs.CopyFrom(ActivationSwitchBuffer.AsNativeArray());
            var EscapePoints = entityCommandBuffer.SetBuffer<EscapePoint>(jobIndex, stateEntity);
            EscapePoints.CopyFrom(EscapePointBuffer.AsNativeArray());
            var PlanningAgents = entityCommandBuffer.SetBuffer<PlanningAgent>(jobIndex, stateEntity);
            PlanningAgents.CopyFrom(PlanningAgentBuffer.AsNativeArray());

            return new StateData
            {
                StateEntity = stateEntity,
                TraitBasedObjects = traitBasedObjects,
                TraitBasedObjectIds = traitBasedObjectIds,

                CharacterBuffer = Characters,
                WaypointBuffer = Waypoints,
                ActivationLockBuffer = ActivationLocks,
                KeyLockBuffer = KeyLocks,
                CarrierBuffer = Carriers,
                ItemBuffer = Items,
                PositionBuffer = Positions,
                CarriableBuffer = Carriables,
                ActivationSwitchBuffer = ActivationSwitchs,
                EscapePointBuffer = EscapePoints,
                PlanningAgentBuffer = PlanningAgents,
            };
        }

        public void AddObject(NativeArray<ComponentType> types, out TraitBasedObject traitBasedObject, TraitBasedObjectId objectId, FixedString64 name = default)
        {
            traitBasedObject = TraitBasedObject.Default;
#if DEBUG
            objectId.Name.CopyFrom(name);
#endif

            for (int i = 0; i < types.Length; i++)
            {
                var t = types[i];
                if (t.TypeIndex == s_CharacterTypeIndex)
                {
                    CharacterBuffer.Add(default);
                    traitBasedObject.CharacterIndex = (byte) (CharacterBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_WaypointTypeIndex)
                {
                    WaypointBuffer.Add(default);
                    traitBasedObject.WaypointIndex = (byte) (WaypointBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_ActivationLockTypeIndex)
                {
                    ActivationLockBuffer.Add(default);
                    traitBasedObject.ActivationLockIndex = (byte) (ActivationLockBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_KeyLockTypeIndex)
                {
                    KeyLockBuffer.Add(default);
                    traitBasedObject.KeyLockIndex = (byte) (KeyLockBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_CarrierTypeIndex)
                {
                    CarrierBuffer.Add(default);
                    traitBasedObject.CarrierIndex = (byte) (CarrierBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_ItemTypeIndex)
                {
                    ItemBuffer.Add(default);
                    traitBasedObject.ItemIndex = (byte) (ItemBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_PositionTypeIndex)
                {
                    PositionBuffer.Add(default);
                    traitBasedObject.PositionIndex = (byte) (PositionBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_CarriableTypeIndex)
                {
                    CarriableBuffer.Add(default);
                    traitBasedObject.CarriableIndex = (byte) (CarriableBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_ActivationSwitchTypeIndex)
                {
                    ActivationSwitchBuffer.Add(default);
                    traitBasedObject.ActivationSwitchIndex = (byte) (ActivationSwitchBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_EscapePointTypeIndex)
                {
                    EscapePointBuffer.Add(default);
                    traitBasedObject.EscapePointIndex = (byte) (EscapePointBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_PlanningAgentTypeIndex)
                {
                    PlanningAgentBuffer.Add(default);
                    traitBasedObject.PlanningAgentIndex = (byte) (PlanningAgentBuffer.Length - 1);
                }
            }

            TraitBasedObjectIds.Add(objectId);
            TraitBasedObjects.Add(traitBasedObject);
        }

        public void AddObject(NativeArray<ComponentType> types, out TraitBasedObject traitBasedObject, out TraitBasedObjectId objectId, FixedString64 name = default)
        {
            objectId = new TraitBasedObjectId() { Id = ObjectId.GetNext() };
            AddObject(types, out traitBasedObject, objectId, name);
        }

        public void ConvertAndSetPlannerTrait(Entity sourceEntity, EntityManager sourceEntityManager,
            NativeArray<ComponentType> sourceTraitTypes, IDictionary<Entity, TraitBasedObjectId> entityToObjectId,
            ref TraitBasedObject traitBasedObject)
        {
            unsafe
            {
                foreach (var type in sourceTraitTypes)
                {
                    if (type == typeof(Generated.Semantic.Traits.CharacterData))
                    {
                        var traitData = sourceEntityManager.GetComponentData<Generated.Semantic.Traits.CharacterData>(sourceEntity);
                        var plannerTraitData = new Character();
                        plannerTraitData.ID = traitData.ID;
                        if (entityToObjectId.TryGetValue(traitData.Waypoint, out var Waypoint))
                            plannerTraitData.Waypoint = Waypoint;
                        SetTraitOnObject(plannerTraitData, ref traitBasedObject);
                    }
                    if (type == typeof(Generated.Semantic.Traits.WaypointData))
                    {
                        var traitData = sourceEntityManager.GetComponentData<Generated.Semantic.Traits.WaypointData>(sourceEntity);
                        var plannerTraitData = new Waypoint();
                        plannerTraitData.Occupied = traitData.Occupied;
                        plannerTraitData.StepsToEnd = traitData.StepsToEnd;
                        if (entityToObjectId.TryGetValue(traitData.Left, out var Left))
                            plannerTraitData.Left = Left;
                        if (entityToObjectId.TryGetValue(traitData.Right, out var Right))
                            plannerTraitData.Right = Right;
                        if (entityToObjectId.TryGetValue(traitData.Up, out var Up))
                            plannerTraitData.Up = Up;
                        if (entityToObjectId.TryGetValue(traitData.Down, out var Down))
                            plannerTraitData.Down = Down;
                        SetTraitOnObject(plannerTraitData, ref traitBasedObject);
                    }
                    if (type == typeof(Generated.Semantic.Traits.ActivationLockData))
                    {
                        var traitData = sourceEntityManager.GetComponentData<Generated.Semantic.Traits.ActivationLockData>(sourceEntity);
                        var plannerTraitData = new ActivationLock();
                        UnsafeUtility.CopyStructureToPtr(ref traitData, UnsafeUtility.AddressOf(ref plannerTraitData));
                        SetTraitOnObject(plannerTraitData, ref traitBasedObject);
                    }
                    if (type == typeof(Generated.Semantic.Traits.CarrierData))
                    {
                        var traitData = sourceEntityManager.GetComponentData<Generated.Semantic.Traits.CarrierData>(sourceEntity);
                        var plannerTraitData = new Carrier();
                        if (entityToObjectId.TryGetValue(traitData.Carried, out var Carried))
                            plannerTraitData.Carried = Carried;
                        SetTraitOnObject(plannerTraitData, ref traitBasedObject);
                    }
                    if (type == typeof(Generated.Semantic.Traits.ItemData))
                    {
                        var traitData = sourceEntityManager.GetComponentData<Generated.Semantic.Traits.ItemData>(sourceEntity);
                        var plannerTraitData = new Item();
                        UnsafeUtility.CopyStructureToPtr(ref traitData, UnsafeUtility.AddressOf(ref plannerTraitData));
                        SetTraitOnObject(plannerTraitData, ref traitBasedObject);
                    }
                    if (type == typeof(Generated.Semantic.Traits.PositionData))
                    {
                        var traitData = sourceEntityManager.GetComponentData<Generated.Semantic.Traits.PositionData>(sourceEntity);
                        var plannerTraitData = new Position();
                        if (entityToObjectId.TryGetValue(traitData.Waypoint, out var Waypoint))
                            plannerTraitData.Waypoint = Waypoint;
                        SetTraitOnObject(plannerTraitData, ref traitBasedObject);
                    }
                    if (type == typeof(Generated.Semantic.Traits.CarriableData))
                    {
                        var traitData = sourceEntityManager.GetComponentData<Generated.Semantic.Traits.CarriableData>(sourceEntity);
                        var plannerTraitData = new Carriable();
                        if (entityToObjectId.TryGetValue(traitData.CarriedBy, out var CarriedBy))
                            plannerTraitData.CarriedBy = CarriedBy;
                        SetTraitOnObject(plannerTraitData, ref traitBasedObject);
                    }
                    if (type == typeof(Generated.Semantic.Traits.ActivationSwitchData))
                    {
                        var traitData = sourceEntityManager.GetComponentData<Generated.Semantic.Traits.ActivationSwitchData>(sourceEntity);
                        var plannerTraitData = new ActivationSwitch();
                        UnsafeUtility.CopyStructureToPtr(ref traitData, UnsafeUtility.AddressOf(ref plannerTraitData));
                        SetTraitOnObject(plannerTraitData, ref traitBasedObject);
                    }
                }
            }
        }

        public void SetTraitOnObject(ITrait trait, ref TraitBasedObject traitBasedObject)
        {
            if (trait is Character CharacterTrait)
                SetTraitOnObject(CharacterTrait, ref traitBasedObject);
            else if (trait is Waypoint WaypointTrait)
                SetTraitOnObject(WaypointTrait, ref traitBasedObject);
            else if (trait is ActivationLock ActivationLockTrait)
                SetTraitOnObject(ActivationLockTrait, ref traitBasedObject);
            else if (trait is KeyLock KeyLockTrait)
                SetTraitOnObject(KeyLockTrait, ref traitBasedObject);
            else if (trait is Carrier CarrierTrait)
                SetTraitOnObject(CarrierTrait, ref traitBasedObject);
            else if (trait is Item ItemTrait)
                SetTraitOnObject(ItemTrait, ref traitBasedObject);
            else if (trait is Position PositionTrait)
                SetTraitOnObject(PositionTrait, ref traitBasedObject);
            else if (trait is Carriable CarriableTrait)
                SetTraitOnObject(CarriableTrait, ref traitBasedObject);
            else if (trait is ActivationSwitch ActivationSwitchTrait)
                SetTraitOnObject(ActivationSwitchTrait, ref traitBasedObject);
            else if (trait is EscapePoint EscapePointTrait)
                SetTraitOnObject(EscapePointTrait, ref traitBasedObject);
            else if (trait is PlanningAgent PlanningAgentTrait)
                SetTraitOnObject(PlanningAgentTrait, ref traitBasedObject);
            else 
                throw new ArgumentException($"Trait {trait} of type {trait.GetType()} is not supported in this state representation.");
        }

        public void SetTraitOnObjectAtIndex(ITrait trait, int traitBasedObjectIndex)
        {
            if (trait is Character CharacterTrait)
                SetTraitOnObjectAtIndex(CharacterTrait, traitBasedObjectIndex);
            else if (trait is Waypoint WaypointTrait)
                SetTraitOnObjectAtIndex(WaypointTrait, traitBasedObjectIndex);
            else if (trait is ActivationLock ActivationLockTrait)
                SetTraitOnObjectAtIndex(ActivationLockTrait, traitBasedObjectIndex);
            else if (trait is KeyLock KeyLockTrait)
                SetTraitOnObjectAtIndex(KeyLockTrait, traitBasedObjectIndex);
            else if (trait is Carrier CarrierTrait)
                SetTraitOnObjectAtIndex(CarrierTrait, traitBasedObjectIndex);
            else if (trait is Item ItemTrait)
                SetTraitOnObjectAtIndex(ItemTrait, traitBasedObjectIndex);
            else if (trait is Position PositionTrait)
                SetTraitOnObjectAtIndex(PositionTrait, traitBasedObjectIndex);
            else if (trait is Carriable CarriableTrait)
                SetTraitOnObjectAtIndex(CarriableTrait, traitBasedObjectIndex);
            else if (trait is ActivationSwitch ActivationSwitchTrait)
                SetTraitOnObjectAtIndex(ActivationSwitchTrait, traitBasedObjectIndex);
            else if (trait is EscapePoint EscapePointTrait)
                SetTraitOnObjectAtIndex(EscapePointTrait, traitBasedObjectIndex);
            else if (trait is PlanningAgent PlanningAgentTrait)
                SetTraitOnObjectAtIndex(PlanningAgentTrait, traitBasedObjectIndex);
            else 
                throw new ArgumentException($"Trait {trait} of type {trait.GetType()} is not supported in this state representation.");
        }


        public TTrait GetTraitOnObject<TTrait>(TraitBasedObject traitBasedObject) where TTrait : struct, ITrait
        {
            var traitBasedObjectTraitIndex = TraitArrayIndex<TTrait>.Index;
            if (traitBasedObjectTraitIndex == -1)
                throw new ArgumentException($"Trait {typeof(TTrait)} not supported in this plan");

            var traitBufferIndex = traitBasedObject[traitBasedObjectTraitIndex];
            if (traitBufferIndex == TraitBasedObject.Unset)
                throw new ArgumentException($"Trait of type {typeof(TTrait)} does not exist on object {traitBasedObject}.");

            return GetBuffer<TTrait>()[traitBufferIndex];
        }

        public bool HasTraitOnObject<TTrait>(TraitBasedObject traitBasedObject) where TTrait : struct, ITrait
        {
            var traitBasedObjectTraitIndex = TraitArrayIndex<TTrait>.Index;
            if (traitBasedObjectTraitIndex == -1)
                throw new ArgumentException($"Trait {typeof(TTrait)} not supported in this plan");

            var traitBufferIndex = traitBasedObject[traitBasedObjectTraitIndex];
            return traitBufferIndex != TraitBasedObject.Unset;
        }

        public void SetTraitOnObject<TTrait>(TTrait trait, ref TraitBasedObject traitBasedObject) where TTrait : struct, ITrait
        {
            var objectIndex = GetTraitBasedObjectIndex(traitBasedObject);
            if (objectIndex == -1)
                throw new ArgumentException($"Object {traitBasedObject} does not exist within the state data {this}.");

            var traitIndex = TraitArrayIndex<TTrait>.Index;
            var traitBuffer = GetBuffer<TTrait>();

            var bufferIndex = traitBasedObject[traitIndex];
            if (bufferIndex == TraitBasedObject.Unset)
            {
                traitBuffer.Add(trait);
                traitBasedObject[traitIndex] = (byte) (traitBuffer.Length - 1);

                TraitBasedObjects[objectIndex] = traitBasedObject;
            }
            else
            {
                traitBuffer[bufferIndex] = trait;
            }
        }

        public bool RemoveTraitOnObject<TTrait>(ref TraitBasedObject traitBasedObject) where TTrait : struct, ITrait
        {
            var objectTraitIndex = TraitArrayIndex<TTrait>.Index;
            var traitBuffer = GetBuffer<TTrait>();

            var traitBufferIndex = traitBasedObject[objectTraitIndex];
            if (traitBufferIndex == TraitBasedObject.Unset)
                return false;

            // last index
            var lastBufferIndex = traitBuffer.Length - 1;

            // Swap back and remove
            var lastTrait = traitBuffer[lastBufferIndex];
            traitBuffer[lastBufferIndex] = traitBuffer[traitBufferIndex];
            traitBuffer[traitBufferIndex] = lastTrait;
            traitBuffer.RemoveAt(lastBufferIndex);

            // Update index for object with last trait in buffer
            var numObjects = TraitBasedObjects.Length;
            for (int i = 0; i < numObjects; i++)
            {
                var otherTraitBasedObject = TraitBasedObjects[i];
                if (otherTraitBasedObject[objectTraitIndex] == lastBufferIndex)
                {
                    otherTraitBasedObject[objectTraitIndex] = traitBufferIndex;
                    TraitBasedObjects[i] = otherTraitBasedObject;
                    break;
                }
            }

            // Update traitBasedObject in buffer (ref is to a copy)
            for (int i = 0; i < numObjects; i++)
            {
                if (traitBasedObject.Equals(TraitBasedObjects[i]))
                {
                    traitBasedObject[objectTraitIndex] = TraitBasedObject.Unset;
                    TraitBasedObjects[i] = traitBasedObject;
                    return true;
                }
            }

            throw new ArgumentException($"TraitBasedObject {traitBasedObject} does not exist in the state container {this}.");
        }

        public bool RemoveObject(TraitBasedObject traitBasedObject)
        {
            var objectIndex = GetTraitBasedObjectIndex(traitBasedObject);
            if (objectIndex == -1)
                return false;


            RemoveTraitOnObject<Character>(ref traitBasedObject);
            RemoveTraitOnObject<Waypoint>(ref traitBasedObject);
            RemoveTraitOnObject<ActivationLock>(ref traitBasedObject);
            RemoveTraitOnObject<KeyLock>(ref traitBasedObject);
            RemoveTraitOnObject<Carrier>(ref traitBasedObject);
            RemoveTraitOnObject<Item>(ref traitBasedObject);
            RemoveTraitOnObject<Position>(ref traitBasedObject);
            RemoveTraitOnObject<Carriable>(ref traitBasedObject);
            RemoveTraitOnObject<ActivationSwitch>(ref traitBasedObject);
            RemoveTraitOnObject<EscapePoint>(ref traitBasedObject);
            RemoveTraitOnObject<PlanningAgent>(ref traitBasedObject);

            TraitBasedObjects.RemoveAt(objectIndex);
            TraitBasedObjectIds.RemoveAt(objectIndex);

            return true;
        }


        public TTrait GetTraitOnObjectAtIndex<TTrait>(int traitBasedObjectIndex) where TTrait : struct, ITrait
        {
            var traitBasedObjectTraitIndex = TraitArrayIndex<TTrait>.Index;
            if (traitBasedObjectTraitIndex == -1)
                throw new ArgumentException($"Trait {typeof(TTrait)} not supported in this state representation");

            var traitBasedObject = TraitBasedObjects[traitBasedObjectIndex];
            var traitBufferIndex = traitBasedObject[traitBasedObjectTraitIndex];
            if (traitBufferIndex == TraitBasedObject.Unset)
                throw new Exception($"Trait index for {typeof(TTrait)} is not set for object {traitBasedObject}");

            return GetBuffer<TTrait>()[traitBufferIndex];
        }

        public void SetTraitOnObjectAtIndex<T>(T trait, int traitBasedObjectIndex) where T : struct, ITrait
        {
            var traitBasedObjectTraitIndex = TraitArrayIndex<T>.Index;
            if (traitBasedObjectTraitIndex == -1)
                throw new ArgumentException($"Trait {typeof(T)} not supported in this state representation");

            var traitBasedObject = TraitBasedObjects[traitBasedObjectIndex];
            var traitBufferIndex = traitBasedObject[traitBasedObjectTraitIndex];
            var traitBuffer = GetBuffer<T>();
            if (traitBufferIndex == TraitBasedObject.Unset)
            {
                traitBuffer.Add(trait);
                traitBufferIndex = (byte)(traitBuffer.Length - 1);
                traitBasedObject[traitBasedObjectTraitIndex] = traitBufferIndex;
                TraitBasedObjects[traitBasedObjectIndex] = traitBasedObject;
            }
            else
            {
                traitBuffer[traitBufferIndex] = trait;
            }
        }

        public bool RemoveTraitOnObjectAtIndex<TTrait>(int traitBasedObjectIndex) where TTrait : struct, ITrait
        {
            var objectTraitIndex = TraitArrayIndex<TTrait>.Index;
            var traitBuffer = GetBuffer<TTrait>();

            var traitBasedObject = TraitBasedObjects[traitBasedObjectIndex];
            var traitBufferIndex = traitBasedObject[objectTraitIndex];
            if (traitBufferIndex == TraitBasedObject.Unset)
                return false;

            // last index
            var lastBufferIndex = traitBuffer.Length - 1;

            // Swap back and remove
            var lastTrait = traitBuffer[lastBufferIndex];
            traitBuffer[lastBufferIndex] = traitBuffer[traitBufferIndex];
            traitBuffer[traitBufferIndex] = lastTrait;
            traitBuffer.RemoveAt(lastBufferIndex);

            // Update index for object with last trait in buffer
            var numObjects = TraitBasedObjects.Length;
            for (int i = 0; i < numObjects; i++)
            {
                var otherTraitBasedObject = TraitBasedObjects[i];
                if (otherTraitBasedObject[objectTraitIndex] == lastBufferIndex)
                {
                    otherTraitBasedObject[objectTraitIndex] = traitBufferIndex;
                    TraitBasedObjects[i] = otherTraitBasedObject;
                    break;
                }
            }

            traitBasedObject[objectTraitIndex] = TraitBasedObject.Unset;
            TraitBasedObjects[traitBasedObjectIndex] = traitBasedObject;

            return true;
        }

        public bool RemoveTraitBasedObjectAtIndex(int traitBasedObjectIndex)
        {
            RemoveTraitOnObjectAtIndex<Character>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<Waypoint>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<ActivationLock>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<KeyLock>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<Carrier>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<Item>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<Position>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<Carriable>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<ActivationSwitch>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<EscapePoint>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<PlanningAgent>(traitBasedObjectIndex);

            TraitBasedObjects.RemoveAt(traitBasedObjectIndex);
            TraitBasedObjectIds.RemoveAt(traitBasedObjectIndex);

            return true;
        }


        public NativeArray<int> GetTraitBasedObjectIndices(NativeList<int> traitBasedObjectIndices, NativeArray<ComponentType> traitFilter)
        {
            var numObjects = TraitBasedObjects.Length;
            for (var i = 0; i < numObjects; i++)
            {
                var traitBasedObject = TraitBasedObjects[i];
                if (traitBasedObject.MatchesTraitFilter(traitFilter))
                    traitBasedObjectIndices.Add(i);
            }

            return traitBasedObjectIndices.AsArray();
        }

        public NativeArray<int> GetTraitBasedObjectIndices(NativeList<int> traitBasedObjectIndices, params ComponentType[] traitFilter)
        {
            var numObjects = TraitBasedObjects.Length;
            for (var i = 0; i < numObjects; i++)
            {
                var traitBasedObject = TraitBasedObjects[i];
                if (traitBasedObject.MatchesTraitFilter(traitFilter))
                    traitBasedObjectIndices.Add(i);
            }

            return traitBasedObjectIndices.AsArray();
        }

        public int GetTraitBasedObjectIndex(TraitBasedObject traitBasedObject)
        {
            for (int objectIndex = TraitBasedObjects.Length - 1; objectIndex >= 0; objectIndex--)
            {
                bool match = true;
                var other = TraitBasedObjects[objectIndex];
                for (int i = 0; i < traitBasedObject.Length && match; i++)
                {
                    match &= traitBasedObject[i] == other[i];
                }

                if (match)
                    return objectIndex;
            }

            return -1;
        }

        public int GetTraitBasedObjectIndex(TraitBasedObjectId traitBasedObjectId)
        {
            var objectIndex = -1;
            for (int i = TraitBasedObjectIds.Length - 1; i >= 0; i--)
            {
                if (TraitBasedObjectIds[i].Equals(traitBasedObjectId))
                {
                    objectIndex = i;
                    break;
                }
            }

            return objectIndex;
        }

        public TraitBasedObjectId GetTraitBasedObjectId(TraitBasedObject traitBasedObject)
        {
            var index = GetTraitBasedObjectIndex(traitBasedObject);
            return TraitBasedObjectIds[index];
        }

        public TraitBasedObjectId GetTraitBasedObjectId(int traitBasedObjectIndex)
        {
            return TraitBasedObjectIds[traitBasedObjectIndex];
        }

        public TraitBasedObject GetTraitBasedObject(TraitBasedObjectId traitBasedObject)
        {
            var index = GetTraitBasedObjectIndex(traitBasedObject);
            return TraitBasedObjects[index];
        }


        DynamicBuffer<T> GetBuffer<T>() where T : struct, ITrait
        {
            var index = TraitArrayIndex<T>.Index;
            switch (index)
            {
                case 0:
                    return CharacterBuffer.Reinterpret<T>();
                case 1:
                    return WaypointBuffer.Reinterpret<T>();
                case 2:
                    return ActivationLockBuffer.Reinterpret<T>();
                case 3:
                    return KeyLockBuffer.Reinterpret<T>();
                case 4:
                    return CarrierBuffer.Reinterpret<T>();
                case 5:
                    return ItemBuffer.Reinterpret<T>();
                case 6:
                    return PositionBuffer.Reinterpret<T>();
                case 7:
                    return CarriableBuffer.Reinterpret<T>();
                case 8:
                    return ActivationSwitchBuffer.Reinterpret<T>();
                case 9:
                    return EscapePointBuffer.Reinterpret<T>();
                case 10:
                    return PlanningAgentBuffer.Reinterpret<T>();
            }

            return default;
        }

        public bool Equals(IStateData other) => other is StateData otherData && Equals(otherData);

        public bool Equals(StateData rhsState)
        {
            if (StateEntity == rhsState.StateEntity)
                return true;

            // Easy check is to make sure each state has the same number of trait-based objects
            if (TraitBasedObjects.Length != rhsState.TraitBasedObjects.Length
                || CharacterBuffer.Length != rhsState.CharacterBuffer.Length
                || WaypointBuffer.Length != rhsState.WaypointBuffer.Length
                || ActivationLockBuffer.Length != rhsState.ActivationLockBuffer.Length
                || KeyLockBuffer.Length != rhsState.KeyLockBuffer.Length
                || CarrierBuffer.Length != rhsState.CarrierBuffer.Length
                || ItemBuffer.Length != rhsState.ItemBuffer.Length
                || PositionBuffer.Length != rhsState.PositionBuffer.Length
                || CarriableBuffer.Length != rhsState.CarriableBuffer.Length
                || ActivationSwitchBuffer.Length != rhsState.ActivationSwitchBuffer.Length
                || EscapePointBuffer.Length != rhsState.EscapePointBuffer.Length
                || PlanningAgentBuffer.Length != rhsState.PlanningAgentBuffer.Length)
                return false;

            var objectMap = new ObjectCorrespondence(TraitBasedObjectIds.Length, Allocator.Temp);
            bool statesEqual = TryGetObjectMapping(rhsState, objectMap);
            objectMap.Dispose();

            return statesEqual;
        }

        bool ITraitBasedStateData<TraitBasedObject, StateData>.TryGetObjectMapping(StateData rhsState, ObjectCorrespondence objectMap)
        {
            // Easy check is to make sure each state has the same number of domain objects
            if (TraitBasedObjects.Length != rhsState.TraitBasedObjects.Length
                || CharacterBuffer.Length != rhsState.CharacterBuffer.Length
                || WaypointBuffer.Length != rhsState.WaypointBuffer.Length
                || ActivationLockBuffer.Length != rhsState.ActivationLockBuffer.Length
                || KeyLockBuffer.Length != rhsState.KeyLockBuffer.Length
                || CarrierBuffer.Length != rhsState.CarrierBuffer.Length
                || ItemBuffer.Length != rhsState.ItemBuffer.Length
                || PositionBuffer.Length != rhsState.PositionBuffer.Length
                || CarriableBuffer.Length != rhsState.CarriableBuffer.Length
                || ActivationSwitchBuffer.Length != rhsState.ActivationSwitchBuffer.Length
                || EscapePointBuffer.Length != rhsState.EscapePointBuffer.Length
                || PlanningAgentBuffer.Length != rhsState.PlanningAgentBuffer.Length)
                return false;

            return TryGetObjectMapping(rhsState, objectMap);
        }

        internal bool TryGetObjectMapping(StateData rhsState, ObjectCorrespondence objectMap)
        {
            objectMap.Initialize(TraitBasedObjectIds, rhsState.TraitBasedObjectIds);

            bool statesEqual = true;
            var numObjects = TraitBasedObjects.Length;
            for (int lhsIndex = 0; lhsIndex < numObjects; lhsIndex++)
            {
                var lhsId = TraitBasedObjectIds[lhsIndex].Id;
                if (objectMap.TryGetValue(lhsId, out _)) // already matched
                    continue;

                // todo lhsIndex to start? would require swapping rhs on assignments, though
                bool matchFound = true;

                for (var rhsIndex = 0; rhsIndex < numObjects; rhsIndex++)
                {
                    var rhsId = rhsState.TraitBasedObjectIds[rhsIndex].Id;
                    if (objectMap.ContainsRHS(rhsId)) // skip if already assigned todo optimize this
                        continue;

                    objectMap.BeginNewTraversal();
                    objectMap.Add(lhsId, rhsId);

                    // Traversal comparing all reachable objects
                    matchFound = true;
                    while (objectMap.Next(out var lhsIdToEvaluate, out var rhsIdToEvaluate))
                    {
                        // match objects, queueing as needed
                        var lhsTraitBasedObject = TraitBasedObjects[objectMap.GetLHSIndex(lhsIdToEvaluate)];
                        var rhsTraitBasedObject = rhsState.TraitBasedObjects[objectMap.GetRHSIndex(rhsIdToEvaluate)];

                        if (!ObjectsMatchAttributes(lhsTraitBasedObject, rhsTraitBasedObject, rhsState) ||
                            !CheckRelationsAndQueueObjects(lhsTraitBasedObject, rhsTraitBasedObject, rhsState, objectMap))
                        {
                            objectMap.RevertTraversalChanges();

                            matchFound = false;
                            break;
                        }
                    }

                    if (matchFound)
                        break;
                }

                if (!matchFound)
                {
                    statesEqual = false;
                    break;
                }
            }

            return statesEqual;
        }

        bool ObjectsMatchAttributes(TraitBasedObject traitBasedObjectLHS, TraitBasedObject traitBasedObjectRHS, StateData rhsState)
        {
            if (!traitBasedObjectLHS.HasSameTraits(traitBasedObjectRHS))
                return false;

            if (traitBasedObjectLHS.CharacterIndex != TraitBasedObject.Unset
                && !CharacterTraitAttributesEqual(CharacterBuffer[traitBasedObjectLHS.CharacterIndex], rhsState.CharacterBuffer[traitBasedObjectRHS.CharacterIndex]))
                return false;


            if (traitBasedObjectLHS.WaypointIndex != TraitBasedObject.Unset
                && !WaypointTraitAttributesEqual(WaypointBuffer[traitBasedObjectLHS.WaypointIndex], rhsState.WaypointBuffer[traitBasedObjectRHS.WaypointIndex]))
                return false;


            if (traitBasedObjectLHS.ActivationLockIndex != TraitBasedObject.Unset
                && !ActivationLockTraitAttributesEqual(ActivationLockBuffer[traitBasedObjectLHS.ActivationLockIndex], rhsState.ActivationLockBuffer[traitBasedObjectRHS.ActivationLockIndex]))
                return false;




            if (traitBasedObjectLHS.ItemIndex != TraitBasedObject.Unset
                && !ItemTraitAttributesEqual(ItemBuffer[traitBasedObjectLHS.ItemIndex], rhsState.ItemBuffer[traitBasedObjectRHS.ItemIndex]))
                return false;




            if (traitBasedObjectLHS.ActivationSwitchIndex != TraitBasedObject.Unset
                && !ActivationSwitchTraitAttributesEqual(ActivationSwitchBuffer[traitBasedObjectLHS.ActivationSwitchIndex], rhsState.ActivationSwitchBuffer[traitBasedObjectRHS.ActivationSwitchIndex]))
                return false;




            return true;
        }
        
        bool CharacterTraitAttributesEqual(Character one, Character two)
        {
            return
                    one.ID == two.ID;
        }
        
        bool WaypointTraitAttributesEqual(Waypoint one, Waypoint two)
        {
            return
                    one.Occupied == two.Occupied && 
                    one.StepsToEnd == two.StepsToEnd;
        }
        
        bool ActivationLockTraitAttributesEqual(ActivationLock one, ActivationLock two)
        {
            return
                    one.ActivationA == two.ActivationA && 
                    one.ActivationB == two.ActivationB;
        }
        
        bool ItemTraitAttributesEqual(Item one, Item two)
        {
            return
                    one.Type == two.Type;
        }
        
        bool ActivationSwitchTraitAttributesEqual(ActivationSwitch one, ActivationSwitch two)
        {
            return
                    one.Type == two.Type;
        }
        
        bool CheckRelationsAndQueueObjects(TraitBasedObject traitBasedObjectLHS, TraitBasedObject traitBasedObjectRHS, StateData rhsState, ObjectCorrespondence objectMap)
        {
            // edge walking - for relation properties
            ObjectId lhsRelationId;
            ObjectId rhsRelationId;
            ObjectId rhsAssignedId;
            if (traitBasedObjectLHS.CharacterIndex != TraitBasedObject.Unset)
            {
                // The Ids to match for Character.Waypoint
                lhsRelationId = CharacterBuffer[traitBasedObjectLHS.CharacterIndex].Waypoint.Id;
                rhsRelationId = rhsState.CharacterBuffer[traitBasedObjectRHS.CharacterIndex].Waypoint.Id;

                if (lhsRelationId.Equals(ObjectId.None) ^ rhsRelationId.Equals(ObjectId.None))
                    return false;

                if (objectMap.TryGetValue(lhsRelationId, out rhsAssignedId))
                {
                    if (!rhsRelationId.Equals(rhsAssignedId))
                        return false;
                }
                else
                {
                    objectMap.Add(lhsRelationId, rhsRelationId);
                }
            }

            if (traitBasedObjectLHS.WaypointIndex != TraitBasedObject.Unset)
            {
                // The Ids to match for Waypoint.Left
                lhsRelationId = WaypointBuffer[traitBasedObjectLHS.WaypointIndex].Left.Id;
                rhsRelationId = rhsState.WaypointBuffer[traitBasedObjectRHS.WaypointIndex].Left.Id;

                if (lhsRelationId.Equals(ObjectId.None) ^ rhsRelationId.Equals(ObjectId.None))
                    return false;

                if (objectMap.TryGetValue(lhsRelationId, out rhsAssignedId))
                {
                    if (!rhsRelationId.Equals(rhsAssignedId))
                        return false;
                }
                else
                {
                    objectMap.Add(lhsRelationId, rhsRelationId);
                }

                // The Ids to match for Waypoint.Right
                lhsRelationId = WaypointBuffer[traitBasedObjectLHS.WaypointIndex].Right.Id;
                rhsRelationId = rhsState.WaypointBuffer[traitBasedObjectRHS.WaypointIndex].Right.Id;

                if (lhsRelationId.Equals(ObjectId.None) ^ rhsRelationId.Equals(ObjectId.None))
                    return false;

                if (objectMap.TryGetValue(lhsRelationId, out rhsAssignedId))
                {
                    if (!rhsRelationId.Equals(rhsAssignedId))
                        return false;
                }
                else
                {
                    objectMap.Add(lhsRelationId, rhsRelationId);
                }

                // The Ids to match for Waypoint.Up
                lhsRelationId = WaypointBuffer[traitBasedObjectLHS.WaypointIndex].Up.Id;
                rhsRelationId = rhsState.WaypointBuffer[traitBasedObjectRHS.WaypointIndex].Up.Id;

                if (lhsRelationId.Equals(ObjectId.None) ^ rhsRelationId.Equals(ObjectId.None))
                    return false;

                if (objectMap.TryGetValue(lhsRelationId, out rhsAssignedId))
                {
                    if (!rhsRelationId.Equals(rhsAssignedId))
                        return false;
                }
                else
                {
                    objectMap.Add(lhsRelationId, rhsRelationId);
                }

                // The Ids to match for Waypoint.Down
                lhsRelationId = WaypointBuffer[traitBasedObjectLHS.WaypointIndex].Down.Id;
                rhsRelationId = rhsState.WaypointBuffer[traitBasedObjectRHS.WaypointIndex].Down.Id;

                if (lhsRelationId.Equals(ObjectId.None) ^ rhsRelationId.Equals(ObjectId.None))
                    return false;

                if (objectMap.TryGetValue(lhsRelationId, out rhsAssignedId))
                {
                    if (!rhsRelationId.Equals(rhsAssignedId))
                        return false;
                }
                else
                {
                    objectMap.Add(lhsRelationId, rhsRelationId);
                }
            }

            if (traitBasedObjectLHS.CarrierIndex != TraitBasedObject.Unset)
            {
                // The Ids to match for Carrier.Carried
                lhsRelationId = CarrierBuffer[traitBasedObjectLHS.CarrierIndex].Carried.Id;
                rhsRelationId = rhsState.CarrierBuffer[traitBasedObjectRHS.CarrierIndex].Carried.Id;

                if (lhsRelationId.Equals(ObjectId.None) ^ rhsRelationId.Equals(ObjectId.None))
                    return false;

                if (objectMap.TryGetValue(lhsRelationId, out rhsAssignedId))
                {
                    if (!rhsRelationId.Equals(rhsAssignedId))
                        return false;
                }
                else
                {
                    objectMap.Add(lhsRelationId, rhsRelationId);
                }
            }

            if (traitBasedObjectLHS.PositionIndex != TraitBasedObject.Unset)
            {
                // The Ids to match for Position.Waypoint
                lhsRelationId = PositionBuffer[traitBasedObjectLHS.PositionIndex].Waypoint.Id;
                rhsRelationId = rhsState.PositionBuffer[traitBasedObjectRHS.PositionIndex].Waypoint.Id;

                if (lhsRelationId.Equals(ObjectId.None) ^ rhsRelationId.Equals(ObjectId.None))
                    return false;

                if (objectMap.TryGetValue(lhsRelationId, out rhsAssignedId))
                {
                    if (!rhsRelationId.Equals(rhsAssignedId))
                        return false;
                }
                else
                {
                    objectMap.Add(lhsRelationId, rhsRelationId);
                }
            }

            if (traitBasedObjectLHS.CarriableIndex != TraitBasedObject.Unset)
            {
                // The Ids to match for Carriable.CarriedBy
                lhsRelationId = CarriableBuffer[traitBasedObjectLHS.CarriableIndex].CarriedBy.Id;
                rhsRelationId = rhsState.CarriableBuffer[traitBasedObjectRHS.CarriableIndex].CarriedBy.Id;

                if (lhsRelationId.Equals(ObjectId.None) ^ rhsRelationId.Equals(ObjectId.None))
                    return false;

                if (objectMap.TryGetValue(lhsRelationId, out rhsAssignedId))
                {
                    if (!rhsRelationId.Equals(rhsAssignedId))
                        return false;
                }
                else
                {
                    objectMap.Add(lhsRelationId, rhsRelationId);
                }
            }


            return true;
        }

        public override int GetHashCode()
        {
            // h = 3860031 + (h+y)*2779 + (h*y*2)   // from How to Hash a Set by Richard O’Keefe
            var stateHashValue = 3860031 + (397 + TraitBasedObjectIds.Length) * 2779 + (397 * TraitBasedObjectIds.Length * 2);

            int bufferLength;

            bufferLength = CharacterBuffer.Length;
            for (int i = 0; i < bufferLength; i++)
            {
                var element = CharacterBuffer[i];
                var value = 397
                    ^ element.ID.GetHashCode();
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            bufferLength = WaypointBuffer.Length;
            for (int i = 0; i < bufferLength; i++)
            {
                var element = WaypointBuffer[i];
                var value = 397
                    ^ element.Occupied.GetHashCode()
                    ^ element.StepsToEnd.GetHashCode();
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            bufferLength = ActivationLockBuffer.Length;
            for (int i = 0; i < bufferLength; i++)
            {
                var element = ActivationLockBuffer[i];
                var value = 397
                    ^ (int) element.ActivationA
                    ^ (int) element.ActivationB;
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            bufferLength = KeyLockBuffer.Length;
            for (int i = 0; i < bufferLength; i++)
            {
                var value = 397;
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            bufferLength = CarrierBuffer.Length;
            for (int i = 0; i < bufferLength; i++)
            {
                var value = 397;
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            bufferLength = ItemBuffer.Length;
            for (int i = 0; i < bufferLength; i++)
            {
                var element = ItemBuffer[i];
                var value = 397
                    ^ (int) element.Type;
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            bufferLength = PositionBuffer.Length;
            for (int i = 0; i < bufferLength; i++)
            {
                var value = 397;
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            bufferLength = CarriableBuffer.Length;
            for (int i = 0; i < bufferLength; i++)
            {
                var value = 397;
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            bufferLength = ActivationSwitchBuffer.Length;
            for (int i = 0; i < bufferLength; i++)
            {
                var element = ActivationSwitchBuffer[i];
                var value = 397
                    ^ (int) element.Type;
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            bufferLength = EscapePointBuffer.Length;
            for (int i = 0; i < bufferLength; i++)
            {
                var value = 397;
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            bufferLength = PlanningAgentBuffer.Length;
            for (int i = 0; i < bufferLength; i++)
            {
                var value = 397;
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }

            return stateHashValue;
        }

        public override string ToString()
        {
            if (StateEntity.Equals(default))
                return string.Empty;

            var sb = new StringBuilder();
            var numObjects = TraitBasedObjects.Length;
            for (var traitBasedObjectIndex = 0; traitBasedObjectIndex < numObjects; traitBasedObjectIndex++)
            {
                var traitBasedObject = TraitBasedObjects[traitBasedObjectIndex];
                sb.AppendLine(TraitBasedObjectIds[traitBasedObjectIndex].ToString());

                var i = 0;

                var traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(CharacterBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(WaypointBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(ActivationLockBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(KeyLockBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(CarrierBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(ItemBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(PositionBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(CarriableBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(ActivationSwitchBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(EscapePointBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(PlanningAgentBuffer[traitIndex].ToString());

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }

    public struct StateDataContext : ITraitBasedStateDataContext<TraitBasedObject, StateEntityKey, StateData>
    {
        public bool IsCreated;
        internal EntityCommandBuffer.ParallelWriter EntityCommandBuffer;
        internal EntityArchetype m_StateArchetype;
        internal int JobIndex;

        [ReadOnly,NativeDisableContainerSafetyRestriction] public BufferFromEntity<TraitBasedObject> TraitBasedObjects;
        [ReadOnly,NativeDisableContainerSafetyRestriction] public BufferFromEntity<TraitBasedObjectId> TraitBasedObjectIds;

        [ReadOnly,NativeDisableContainerSafetyRestriction] public BufferFromEntity<Character> CharacterData;
        [ReadOnly,NativeDisableContainerSafetyRestriction] public BufferFromEntity<Waypoint> WaypointData;
        [ReadOnly,NativeDisableContainerSafetyRestriction] public BufferFromEntity<ActivationLock> ActivationLockData;
        [ReadOnly,NativeDisableContainerSafetyRestriction] public BufferFromEntity<KeyLock> KeyLockData;
        [ReadOnly,NativeDisableContainerSafetyRestriction] public BufferFromEntity<Carrier> CarrierData;
        [ReadOnly,NativeDisableContainerSafetyRestriction] public BufferFromEntity<Item> ItemData;
        [ReadOnly,NativeDisableContainerSafetyRestriction] public BufferFromEntity<Position> PositionData;
        [ReadOnly,NativeDisableContainerSafetyRestriction] public BufferFromEntity<Carriable> CarriableData;
        [ReadOnly,NativeDisableContainerSafetyRestriction] public BufferFromEntity<ActivationSwitch> ActivationSwitchData;
        [ReadOnly,NativeDisableContainerSafetyRestriction] public BufferFromEntity<EscapePoint> EscapePointData;
        [ReadOnly,NativeDisableContainerSafetyRestriction] public BufferFromEntity<PlanningAgent> PlanningAgentData;

        [NativeDisableContainerSafetyRestriction,ReadOnly] ObjectCorrespondence m_ObjectCorrespondence;

        public StateDataContext(JobComponentSystem system, EntityArchetype stateArchetype)
        {
            EntityCommandBuffer = default;
            TraitBasedObjects = system.GetBufferFromEntity<TraitBasedObject>(true);
            TraitBasedObjectIds = system.GetBufferFromEntity<TraitBasedObjectId>(true);

            CharacterData = system.GetBufferFromEntity<Character>(true);
            WaypointData = system.GetBufferFromEntity<Waypoint>(true);
            ActivationLockData = system.GetBufferFromEntity<ActivationLock>(true);
            KeyLockData = system.GetBufferFromEntity<KeyLock>(true);
            CarrierData = system.GetBufferFromEntity<Carrier>(true);
            ItemData = system.GetBufferFromEntity<Item>(true);
            PositionData = system.GetBufferFromEntity<Position>(true);
            CarriableData = system.GetBufferFromEntity<Carriable>(true);
            ActivationSwitchData = system.GetBufferFromEntity<ActivationSwitch>(true);
            EscapePointData = system.GetBufferFromEntity<EscapePoint>(true);
            PlanningAgentData = system.GetBufferFromEntity<PlanningAgent>(true);

            m_StateArchetype = stateArchetype;
            JobIndex = 0;
            m_ObjectCorrespondence = default;
            IsCreated = true;
        }

        public StateData GetStateData(StateEntityKey stateKey)
        {
            var stateEntity = stateKey.Entity;

            return new StateData
            {
                StateEntity = stateEntity,
                TraitBasedObjects = TraitBasedObjects[stateEntity],
                TraitBasedObjectIds = TraitBasedObjectIds[stateEntity],

                CharacterBuffer = CharacterData[stateEntity],
                WaypointBuffer = WaypointData[stateEntity],
                ActivationLockBuffer = ActivationLockData[stateEntity],
                KeyLockBuffer = KeyLockData[stateEntity],
                CarrierBuffer = CarrierData[stateEntity],
                ItemBuffer = ItemData[stateEntity],
                PositionBuffer = PositionData[stateEntity],
                CarriableBuffer = CarriableData[stateEntity],
                ActivationSwitchBuffer = ActivationSwitchData[stateEntity],
                EscapePointBuffer = EscapePointData[stateEntity],
                PlanningAgentBuffer = PlanningAgentData[stateEntity],
            };
        }

        public StateData CopyStateData(StateData stateData)
        {
            return stateData.Copy(JobIndex, EntityCommandBuffer);
        }

        public StateEntityKey GetStateDataKey(StateData stateData)
        {
            return new StateEntityKey { Entity = stateData.StateEntity, HashCode = stateData.GetHashCode()};
        }

        public void DestroyState(StateEntityKey stateKey)
        {
            EntityCommandBuffer.DestroyEntity(JobIndex, stateKey.Entity);
        }

        public StateData CreateStateData()
        {
            return new StateData(JobIndex, EntityCommandBuffer, EntityCommandBuffer.CreateEntity(JobIndex, m_StateArchetype));
        }

        public bool Equals(StateData x, StateData y)
        {
            if (x.TraitBasedObjectIds.Length != y.TraitBasedObjectIds.Length)
                return false;

            if (!m_ObjectCorrespondence.IsCreated)
                m_ObjectCorrespondence = new ObjectCorrespondence(x.TraitBasedObjectIds.Length, Allocator.Temp);

            return x.TryGetObjectMapping(y, m_ObjectCorrespondence);
        }

        public int GetHashCode(StateData obj)
        {
            return obj.GetHashCode();
        }
    }

    [DisableAutoCreation, AlwaysUpdateSystem]
    public class StateManager : JobComponentSystem, ITraitBasedStateManager<TraitBasedObject, StateEntityKey, StateData, StateDataContext>
    {
        public new EntityManager EntityManager
        {
            get
            {
                if (!m_EntityTransactionActive)
                    BeginEntityExclusivity();

                return ExclusiveEntityTransaction.EntityManager;
            }
        }

        ExclusiveEntityTransaction m_ExclusiveEntityTransaction;
        public ExclusiveEntityTransaction ExclusiveEntityTransaction
        {
            get
            {
                if (!m_EntityTransactionActive)
                    BeginEntityExclusivity();

                return m_ExclusiveEntityTransaction;
            }
        }

        StateDataContext m_StateDataContext;
        public StateDataContext StateDataContext
        {
            get
            {
                if (m_StateDataContext.IsCreated)
                    return m_StateDataContext;

                m_StateDataContext = new StateDataContext(this, m_StateArchetype);
                return m_StateDataContext;
            }
        }

        public event Action Destroying;

        List<EntityCommandBuffer> m_EntityCommandBuffers;
        EntityArchetype m_StateArchetype;
        bool m_EntityTransactionActive = false;

        protected override void OnCreate()
        {
            m_StateArchetype = base.EntityManager.CreateArchetype(typeof(State), typeof(TraitBasedObject), typeof(TraitBasedObjectId), typeof(HashCode),
                typeof(Character),
                typeof(Waypoint),
                typeof(ActivationLock),
                typeof(KeyLock),
                typeof(Carrier),
                typeof(Item),
                typeof(Position),
                typeof(Carriable),
                typeof(ActivationSwitch),
                typeof(EscapePoint),
                typeof(PlanningAgent));

            m_EntityCommandBuffers = new List<EntityCommandBuffer>();
        }

        protected override void OnDestroy()
        {
            Destroying?.Invoke();
            EndEntityExclusivity();
            ClearECBs();
            base.OnDestroy();
        }

        public EntityCommandBuffer GetEntityCommandBuffer()
        {
            var ecb = new EntityCommandBuffer(Allocator.Persistent);
            m_EntityCommandBuffers.Add(ecb);
            return ecb;
        }

        public StateData CreateStateData()
        {
            var stateEntity = ExclusiveEntityTransaction.CreateEntity(m_StateArchetype);
            return new StateData(ExclusiveEntityTransaction, stateEntity);;
        }

        public StateData GetStateData(StateEntityKey stateKey, bool readWrite = false)
        {
            return !Enabled || !ExclusiveEntityTransaction.Exists(stateKey.Entity) ?
                default : new StateData(ExclusiveEntityTransaction, stateKey.Entity);
        }

        public void DestroyState(StateEntityKey stateKey)
        {
            var stateEntity = stateKey.Entity;
            if (ExclusiveEntityTransaction.Exists(stateEntity))
                ExclusiveEntityTransaction.DestroyEntity(stateEntity);
        }

        public StateEntityKey GetStateDataKey(StateData stateData)
        {
            return new StateEntityKey { Entity = stateData.StateEntity, HashCode = stateData.GetHashCode()};
        }

        public StateData CopyStateData(StateData stateData)
        {
            var copyStateEntity = ExclusiveEntityTransaction.Instantiate(stateData.StateEntity);
            return new StateData(ExclusiveEntityTransaction, copyStateEntity);
        }

        public StateEntityKey CopyState(StateEntityKey stateKey)
        {
            var copyStateEntity = ExclusiveEntityTransaction.Instantiate(stateKey.Entity);
            var stateData = new StateData(ExclusiveEntityTransaction, copyStateEntity);
            return new StateEntityKey { Entity = copyStateEntity, HashCode = stateData.GetHashCode()};
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var jobDependencyHandle = ExclusiveEntityTransaction.EntityManager.ExclusiveEntityTransactionDependency;
            if (jobDependencyHandle.IsCompleted)
            {
                jobDependencyHandle.Complete();
                ClearECBs();
            }

            return inputDeps;
        }

        void ClearECBs()
        {
            foreach (var ecb in m_EntityCommandBuffers)
            {
                ecb.Dispose();
            }
            m_EntityCommandBuffers.Clear();
        }

        public bool Equals(StateData x, StateData y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(StateData obj)
        {
            return obj.GetHashCode();
        }

        void BeginEntityExclusivity()
        {
            m_StateDataContext = new StateDataContext(this, m_StateArchetype);
            m_ExclusiveEntityTransaction = base.EntityManager.BeginExclusiveEntityTransaction();
            m_EntityTransactionActive = true;
        }

        void EndEntityExclusivity()
        {
            base.EntityManager.EndExclusiveEntityTransaction();
            m_EntityTransactionActive = false;
            m_StateDataContext = default;
        }
    }

    struct DestroyStatesJobScheduler : IDestroyStatesScheduler<StateEntityKey, StateData, StateDataContext, StateManager>
    {
        public StateManager StateManager { private get; set; }
        public NativeQueue<StateEntityKey> StatesToDestroy { private get; set; }

        public JobHandle Schedule(JobHandle inputDeps)
        {
            var stateDataContext = StateManager.StateDataContext;
            var ecb = StateManager.GetEntityCommandBuffer();
            stateDataContext.EntityCommandBuffer = ecb.AsParallelWriter();
            var destroyStatesJobHandle = new DestroyStatesJob<StateEntityKey, StateData, StateDataContext>()
            {
                StateDataContext = stateDataContext,
                StatesToDestroy = StatesToDestroy
            }.Schedule(inputDeps);

            var playbackECBJobHandle = new PlaybackSingleECBJob()
            {
                ExclusiveEntityTransaction = StateManager.ExclusiveEntityTransaction,
                EntityCommandBuffer = ecb
            }.Schedule(destroyStatesJobHandle);

            var entityManager = StateManager.ExclusiveEntityTransaction.EntityManager;
            entityManager.ExclusiveEntityTransactionDependency = playbackECBJobHandle;
            return playbackECBJobHandle;
        }
    }
}
