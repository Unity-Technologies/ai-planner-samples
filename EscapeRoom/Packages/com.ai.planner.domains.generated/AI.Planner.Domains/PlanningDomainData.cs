using System;
using System.Text;
using System.Collections.Generic;
using Unity.AI.Planner;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using Unity.AI.Planner.Jobs;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace AI.Planner.Domains
{
    // Domains don't share key types to enforce that planners are domain specific
    public struct StateEntityKey : IEquatable<StateEntityKey>, IStateKey
    {
        public Entity Entity;
        public int HashCode;

        public bool Equals(StateEntityKey other) => Entity == other.Entity;

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
            if (typeIndex == TypeManager.GetTypeIndex<ActivationLock>())
                Index = 0;
            else if (typeIndex == TypeManager.GetTypeIndex<ActivationSwitch>())
                Index = 1;
            else if (typeIndex == TypeManager.GetTypeIndex<Carriable>())
                Index = 2;
            else if (typeIndex == TypeManager.GetTypeIndex<Carrier>())
                Index = 3;
            else if (typeIndex == TypeManager.GetTypeIndex<Character>())
                Index = 4;
            else if (typeIndex == TypeManager.GetTypeIndex<EscapePoint>())
                Index = 5;
            else if (typeIndex == TypeManager.GetTypeIndex<Item>())
                Index = 6;
            else if (typeIndex == TypeManager.GetTypeIndex<KeyLock>())
                Index = 7;
            else if (typeIndex == TypeManager.GetTypeIndex<Position>())
                Index = 8;
            else if (typeIndex == TypeManager.GetTypeIndex<Waypoint>())
                Index = 9;
            else if (typeIndex == TypeManager.GetTypeIndex<Location>())
                Index = 10;
            else if (typeIndex == TypeManager.GetTypeIndex<Moveable>())
                Index = 11;
        }
    }

    public struct TraitBasedObject : ITraitBasedObject
    {
        public int Length => 12;

        public byte this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return ActivationLockIndex;
                    case 1:
                        return ActivationSwitchIndex;
                    case 2:
                        return CarriableIndex;
                    case 3:
                        return CarrierIndex;
                    case 4:
                        return CharacterIndex;
                    case 5:
                        return EscapePointIndex;
                    case 6:
                        return ItemIndex;
                    case 7:
                        return KeyLockIndex;
                    case 8:
                        return PositionIndex;
                    case 9:
                        return WaypointIndex;
                    case 10:
                        return LocationIndex;
                    case 11:
                        return MoveableIndex;
                }

                return Unset;
            }
            set
            {
                switch (i)
                {
                    case 0:
                        ActivationLockIndex = value;
                        break;
                    case 1:
                        ActivationSwitchIndex = value;
                        break;
                    case 2:
                        CarriableIndex = value;
                        break;
                    case 3:
                        CarrierIndex = value;
                        break;
                    case 4:
                        CharacterIndex = value;
                        break;
                    case 5:
                        EscapePointIndex = value;
                        break;
                    case 6:
                        ItemIndex = value;
                        break;
                    case 7:
                        KeyLockIndex = value;
                        break;
                    case 8:
                        PositionIndex = value;
                        break;
                    case 9:
                        WaypointIndex = value;
                        break;
                    case 10:
                        LocationIndex = value;
                        break;
                    case 11:
                        MoveableIndex = value;
                        break;
                }
            }
        }

        public static readonly byte Unset = Byte.MaxValue;

        public static TraitBasedObject Default => new TraitBasedObject
        {
            ActivationLockIndex = Unset,
            ActivationSwitchIndex = Unset,
            CarriableIndex = Unset,
            CarrierIndex = Unset,
            CharacterIndex = Unset,
            EscapePointIndex = Unset,
            ItemIndex = Unset,
            KeyLockIndex = Unset,
            PositionIndex = Unset,
            WaypointIndex = Unset,
            LocationIndex = Unset,
            MoveableIndex = Unset,
        };


        public byte ActivationLockIndex;
        public byte ActivationSwitchIndex;
        public byte CarriableIndex;
        public byte CarrierIndex;
        public byte CharacterIndex;
        public byte EscapePointIndex;
        public byte ItemIndex;
        public byte KeyLockIndex;
        public byte PositionIndex;
        public byte WaypointIndex;
        public byte LocationIndex;
        public byte MoveableIndex;


        static readonly int s_ActivationLockTypeIndex = TypeManager.GetTypeIndex<ActivationLock>();
        static readonly int s_ActivationSwitchTypeIndex = TypeManager.GetTypeIndex<ActivationSwitch>();
        static readonly int s_CarriableTypeIndex = TypeManager.GetTypeIndex<Carriable>();
        static readonly int s_CarrierTypeIndex = TypeManager.GetTypeIndex<Carrier>();
        static readonly int s_CharacterTypeIndex = TypeManager.GetTypeIndex<Character>();
        static readonly int s_EscapePointTypeIndex = TypeManager.GetTypeIndex<EscapePoint>();
        static readonly int s_ItemTypeIndex = TypeManager.GetTypeIndex<Item>();
        static readonly int s_KeyLockTypeIndex = TypeManager.GetTypeIndex<KeyLock>();
        static readonly int s_PositionTypeIndex = TypeManager.GetTypeIndex<Position>();
        static readonly int s_WaypointTypeIndex = TypeManager.GetTypeIndex<Waypoint>();
        static readonly int s_LocationTypeIndex = TypeManager.GetTypeIndex<Location>();
        static readonly int s_MoveableTypeIndex = TypeManager.GetTypeIndex<Moveable>();

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

                if (t.TypeIndex == s_ActivationLockTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ ActivationLockIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_ActivationSwitchTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ ActivationSwitchIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_CarriableTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ CarriableIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_CarrierTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ CarrierIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_CharacterTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ CharacterIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_EscapePointTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ EscapePointIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_ItemTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ ItemIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_KeyLockTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ KeyLockIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_PositionTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ PositionIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_WaypointTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ WaypointIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_LocationTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ LocationIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_MoveableTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ MoveableIndex == Unset)
                        return false;
                }
                else
                    throw new ArgumentException($"Incorrect trait type used in domain object query: {t}");
            }

            return true;
        }

        public bool MatchesTraitFilter(ComponentType[] componentTypes)
        {
            for (int i = 0; i < componentTypes.Length; i++)
            {
                var t = componentTypes[i];

                if (t.TypeIndex == s_ActivationLockTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ ActivationLockIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_ActivationSwitchTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ ActivationSwitchIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_CarriableTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ CarriableIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_CarrierTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ CarrierIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_CharacterTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ CharacterIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_EscapePointTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ EscapePointIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_ItemTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ ItemIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_KeyLockTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ KeyLockIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_PositionTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ PositionIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_WaypointTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ WaypointIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_LocationTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ LocationIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_MoveableTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ MoveableIndex == Unset)
                        return false;
                }
                else
                    throw new ArgumentException($"Incorrect trait type used in domain object query: {t}");
            }

            return true;
        }
    }

    public struct StateData : ITraitBasedStateData<TraitBasedObject, StateData>
    {
        public Entity StateEntity;
        public DynamicBuffer<TraitBasedObject> TraitBasedObjects;
        public DynamicBuffer<TraitBasedObjectId> TraitBasedObjectIds;

        public DynamicBuffer<ActivationLock> ActivationLockBuffer;
        public DynamicBuffer<ActivationSwitch> ActivationSwitchBuffer;
        public DynamicBuffer<Carriable> CarriableBuffer;
        public DynamicBuffer<Carrier> CarrierBuffer;
        public DynamicBuffer<Character> CharacterBuffer;
        public DynamicBuffer<EscapePoint> EscapePointBuffer;
        public DynamicBuffer<Item> ItemBuffer;
        public DynamicBuffer<KeyLock> KeyLockBuffer;
        public DynamicBuffer<Position> PositionBuffer;
        public DynamicBuffer<Waypoint> WaypointBuffer;
        public DynamicBuffer<Location> LocationBuffer;
        public DynamicBuffer<Moveable> MoveableBuffer;

        static readonly int s_ActivationLockTypeIndex = TypeManager.GetTypeIndex<ActivationLock>();
        static readonly int s_ActivationSwitchTypeIndex = TypeManager.GetTypeIndex<ActivationSwitch>();
        static readonly int s_CarriableTypeIndex = TypeManager.GetTypeIndex<Carriable>();
        static readonly int s_CarrierTypeIndex = TypeManager.GetTypeIndex<Carrier>();
        static readonly int s_CharacterTypeIndex = TypeManager.GetTypeIndex<Character>();
        static readonly int s_EscapePointTypeIndex = TypeManager.GetTypeIndex<EscapePoint>();
        static readonly int s_ItemTypeIndex = TypeManager.GetTypeIndex<Item>();
        static readonly int s_KeyLockTypeIndex = TypeManager.GetTypeIndex<KeyLock>();
        static readonly int s_PositionTypeIndex = TypeManager.GetTypeIndex<Position>();
        static readonly int s_WaypointTypeIndex = TypeManager.GetTypeIndex<Waypoint>();
        static readonly int s_LocationTypeIndex = TypeManager.GetTypeIndex<Location>();
        static readonly int s_MoveableTypeIndex = TypeManager.GetTypeIndex<Moveable>();

        public StateData(JobComponentSystem system, Entity stateEntity, bool readWrite = false)
        {
            StateEntity = stateEntity;
            TraitBasedObjects = system.GetBufferFromEntity<TraitBasedObject>(!readWrite)[stateEntity];
            TraitBasedObjectIds = system.GetBufferFromEntity<TraitBasedObjectId>(!readWrite)[stateEntity];

            ActivationLockBuffer = system.GetBufferFromEntity<ActivationLock>(!readWrite)[stateEntity];
            ActivationSwitchBuffer = system.GetBufferFromEntity<ActivationSwitch>(!readWrite)[stateEntity];
            CarriableBuffer = system.GetBufferFromEntity<Carriable>(!readWrite)[stateEntity];
            CarrierBuffer = system.GetBufferFromEntity<Carrier>(!readWrite)[stateEntity];
            CharacterBuffer = system.GetBufferFromEntity<Character>(!readWrite)[stateEntity];
            EscapePointBuffer = system.GetBufferFromEntity<EscapePoint>(!readWrite)[stateEntity];
            ItemBuffer = system.GetBufferFromEntity<Item>(!readWrite)[stateEntity];
            KeyLockBuffer = system.GetBufferFromEntity<KeyLock>(!readWrite)[stateEntity];
            PositionBuffer = system.GetBufferFromEntity<Position>(!readWrite)[stateEntity];
            WaypointBuffer = system.GetBufferFromEntity<Waypoint>(!readWrite)[stateEntity];
            LocationBuffer = system.GetBufferFromEntity<Location>(!readWrite)[stateEntity];
            MoveableBuffer = system.GetBufferFromEntity<Moveable>(!readWrite)[stateEntity];
        }

        public StateData(int jobIndex, EntityCommandBuffer.Concurrent entityCommandBuffer, Entity stateEntity)
        {
            StateEntity = stateEntity;
            TraitBasedObjects = entityCommandBuffer.AddBuffer<TraitBasedObject>(jobIndex, stateEntity);
            TraitBasedObjectIds = entityCommandBuffer.AddBuffer<TraitBasedObjectId>(jobIndex, stateEntity);

            ActivationLockBuffer = entityCommandBuffer.AddBuffer<ActivationLock>(jobIndex, stateEntity);
            ActivationSwitchBuffer = entityCommandBuffer.AddBuffer<ActivationSwitch>(jobIndex, stateEntity);
            CarriableBuffer = entityCommandBuffer.AddBuffer<Carriable>(jobIndex, stateEntity);
            CarrierBuffer = entityCommandBuffer.AddBuffer<Carrier>(jobIndex, stateEntity);
            CharacterBuffer = entityCommandBuffer.AddBuffer<Character>(jobIndex, stateEntity);
            EscapePointBuffer = entityCommandBuffer.AddBuffer<EscapePoint>(jobIndex, stateEntity);
            ItemBuffer = entityCommandBuffer.AddBuffer<Item>(jobIndex, stateEntity);
            KeyLockBuffer = entityCommandBuffer.AddBuffer<KeyLock>(jobIndex, stateEntity);
            PositionBuffer = entityCommandBuffer.AddBuffer<Position>(jobIndex, stateEntity);
            WaypointBuffer = entityCommandBuffer.AddBuffer<Waypoint>(jobIndex, stateEntity);
            LocationBuffer = entityCommandBuffer.AddBuffer<Location>(jobIndex, stateEntity);
            MoveableBuffer = entityCommandBuffer.AddBuffer<Moveable>(jobIndex, stateEntity);
        }

        public StateData Copy(int jobIndex, EntityCommandBuffer.Concurrent entityCommandBuffer)
        {
            var stateEntity = entityCommandBuffer.Instantiate(jobIndex, StateEntity);
            var traitBasedObjects = entityCommandBuffer.SetBuffer<TraitBasedObject>(jobIndex, stateEntity);
            traitBasedObjects.CopyFrom(TraitBasedObjects.AsNativeArray());
            var traitBasedObjectIds = entityCommandBuffer.SetBuffer<TraitBasedObjectId>(jobIndex, stateEntity);
            traitBasedObjectIds.CopyFrom(TraitBasedObjectIds.AsNativeArray());

            var ActivationLocks = entityCommandBuffer.SetBuffer<ActivationLock>(jobIndex, stateEntity);
            ActivationLocks.CopyFrom(ActivationLockBuffer.AsNativeArray());
            var ActivationSwitchs = entityCommandBuffer.SetBuffer<ActivationSwitch>(jobIndex, stateEntity);
            ActivationSwitchs.CopyFrom(ActivationSwitchBuffer.AsNativeArray());
            var Carriables = entityCommandBuffer.SetBuffer<Carriable>(jobIndex, stateEntity);
            Carriables.CopyFrom(CarriableBuffer.AsNativeArray());
            var Carriers = entityCommandBuffer.SetBuffer<Carrier>(jobIndex, stateEntity);
            Carriers.CopyFrom(CarrierBuffer.AsNativeArray());
            var Characters = entityCommandBuffer.SetBuffer<Character>(jobIndex, stateEntity);
            Characters.CopyFrom(CharacterBuffer.AsNativeArray());
            var EscapePoints = entityCommandBuffer.SetBuffer<EscapePoint>(jobIndex, stateEntity);
            EscapePoints.CopyFrom(EscapePointBuffer.AsNativeArray());
            var Items = entityCommandBuffer.SetBuffer<Item>(jobIndex, stateEntity);
            Items.CopyFrom(ItemBuffer.AsNativeArray());
            var KeyLocks = entityCommandBuffer.SetBuffer<KeyLock>(jobIndex, stateEntity);
            KeyLocks.CopyFrom(KeyLockBuffer.AsNativeArray());
            var Positions = entityCommandBuffer.SetBuffer<Position>(jobIndex, stateEntity);
            Positions.CopyFrom(PositionBuffer.AsNativeArray());
            var Waypoints = entityCommandBuffer.SetBuffer<Waypoint>(jobIndex, stateEntity);
            Waypoints.CopyFrom(WaypointBuffer.AsNativeArray());
            var Locations = entityCommandBuffer.SetBuffer<Location>(jobIndex, stateEntity);
            Locations.CopyFrom(LocationBuffer.AsNativeArray());
            var Moveables = entityCommandBuffer.SetBuffer<Moveable>(jobIndex, stateEntity);
            Moveables.CopyFrom(MoveableBuffer.AsNativeArray());

            return new StateData
            {
                StateEntity = stateEntity,
                TraitBasedObjects = traitBasedObjects,
                TraitBasedObjectIds = traitBasedObjectIds,

                ActivationLockBuffer = ActivationLocks,
                ActivationSwitchBuffer = ActivationSwitchs,
                CarriableBuffer = Carriables,
                CarrierBuffer = Carriers,
                CharacterBuffer = Characters,
                EscapePointBuffer = EscapePoints,
                ItemBuffer = Items,
                KeyLockBuffer = KeyLocks,
                PositionBuffer = Positions,
                WaypointBuffer = Waypoints,
                LocationBuffer = Locations,
                MoveableBuffer = Moveables,
            };
        }

        public void AddObject(NativeArray<ComponentType> types, out TraitBasedObject traitBasedObject, TraitBasedObjectId objectId, string name = null)
        {
            traitBasedObject = TraitBasedObject.Default;
#if DEBUG
            if (!string.IsNullOrEmpty(name))
                objectId.Name.CopyFrom(name);
#endif

            for (int i = 0; i < types.Length; i++)
            {
                var t = types[i];
                if (t.TypeIndex == s_ActivationLockTypeIndex)
                {
                    ActivationLockBuffer.Add(default);
                    traitBasedObject.ActivationLockIndex = (byte) (ActivationLockBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_ActivationSwitchTypeIndex)
                {
                    ActivationSwitchBuffer.Add(default);
                    traitBasedObject.ActivationSwitchIndex = (byte) (ActivationSwitchBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_CarriableTypeIndex)
                {
                    CarriableBuffer.Add(default);
                    traitBasedObject.CarriableIndex = (byte) (CarriableBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_CarrierTypeIndex)
                {
                    CarrierBuffer.Add(default);
                    traitBasedObject.CarrierIndex = (byte) (CarrierBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_CharacterTypeIndex)
                {
                    CharacterBuffer.Add(default);
                    traitBasedObject.CharacterIndex = (byte) (CharacterBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_EscapePointTypeIndex)
                {
                    EscapePointBuffer.Add(default);
                    traitBasedObject.EscapePointIndex = (byte) (EscapePointBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_ItemTypeIndex)
                {
                    ItemBuffer.Add(default);
                    traitBasedObject.ItemIndex = (byte) (ItemBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_KeyLockTypeIndex)
                {
                    KeyLockBuffer.Add(default);
                    traitBasedObject.KeyLockIndex = (byte) (KeyLockBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_PositionTypeIndex)
                {
                    PositionBuffer.Add(default);
                    traitBasedObject.PositionIndex = (byte) (PositionBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_WaypointTypeIndex)
                {
                    WaypointBuffer.Add(default);
                    traitBasedObject.WaypointIndex = (byte) (WaypointBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_LocationTypeIndex)
                {
                    LocationBuffer.Add(default);
                    traitBasedObject.LocationIndex = (byte) (LocationBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_MoveableTypeIndex)
                {
                    MoveableBuffer.Add(default);
                    traitBasedObject.MoveableIndex = (byte) (MoveableBuffer.Length - 1);
                }
            }

            TraitBasedObjectIds.Add(objectId);
            TraitBasedObjects.Add(traitBasedObject);
        }

        public void AddObject(NativeArray<ComponentType> types, out TraitBasedObject traitBasedObject, out TraitBasedObjectId objectId, string name = null)
        {
            objectId = new TraitBasedObjectId() { Id = ObjectId.GetNext() };
            AddObject(types, out traitBasedObject, objectId, name);
        }

        public void SetTraitOnObject(ITrait trait, ref TraitBasedObject traitBasedObject)
        {
            if (trait is ActivationLock ActivationLockTrait)
                SetTraitOnObject(ActivationLockTrait, ref traitBasedObject);
            else if (trait is ActivationSwitch ActivationSwitchTrait)
                SetTraitOnObject(ActivationSwitchTrait, ref traitBasedObject);
            else if (trait is Carriable CarriableTrait)
                SetTraitOnObject(CarriableTrait, ref traitBasedObject);
            else if (trait is Carrier CarrierTrait)
                SetTraitOnObject(CarrierTrait, ref traitBasedObject);
            else if (trait is Character CharacterTrait)
                SetTraitOnObject(CharacterTrait, ref traitBasedObject);
            else if (trait is EscapePoint EscapePointTrait)
                SetTraitOnObject(EscapePointTrait, ref traitBasedObject);
            else if (trait is Item ItemTrait)
                SetTraitOnObject(ItemTrait, ref traitBasedObject);
            else if (trait is KeyLock KeyLockTrait)
                SetTraitOnObject(KeyLockTrait, ref traitBasedObject);
            else if (trait is Position PositionTrait)
                SetTraitOnObject(PositionTrait, ref traitBasedObject);
            else if (trait is Waypoint WaypointTrait)
                SetTraitOnObject(WaypointTrait, ref traitBasedObject);
            else if (trait is Location LocationTrait)
                SetTraitOnObject(LocationTrait, ref traitBasedObject);
            else if (trait is Moveable MoveableTrait)
                SetTraitOnObject(MoveableTrait, ref traitBasedObject);
            else 
                throw new ArgumentException($"Trait {trait} of type {trait.GetType()} is not supported in this domain.");
        }


        public TTrait GetTraitOnObject<TTrait>(TraitBasedObject traitBasedObject) where TTrait : struct, ITrait
        {
            var traitBasedObjectTraitIndex = TraitArrayIndex<TTrait>.Index;
            if (traitBasedObjectTraitIndex == -1)
                throw new ArgumentException($"Trait {typeof(TTrait)} not supported in this domain");

            var traitBufferIndex = traitBasedObject[traitBasedObjectTraitIndex];
            if (traitBufferIndex == TraitBasedObject.Unset)
                throw new ArgumentException($"Trait of type {typeof(TTrait)} does not exist on object {traitBasedObject}.");

            return GetBuffer<TTrait>()[traitBufferIndex];
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
            for (int i = 0; i < TraitBasedObjects.Length; i++)
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
            for (int i = 0; i < TraitBasedObjects.Length; i++)
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


            RemoveTraitOnObject<ActivationLock>(ref traitBasedObject);
            RemoveTraitOnObject<ActivationSwitch>(ref traitBasedObject);
            RemoveTraitOnObject<Carriable>(ref traitBasedObject);
            RemoveTraitOnObject<Carrier>(ref traitBasedObject);
            RemoveTraitOnObject<Character>(ref traitBasedObject);
            RemoveTraitOnObject<EscapePoint>(ref traitBasedObject);
            RemoveTraitOnObject<Item>(ref traitBasedObject);
            RemoveTraitOnObject<KeyLock>(ref traitBasedObject);
            RemoveTraitOnObject<Position>(ref traitBasedObject);
            RemoveTraitOnObject<Waypoint>(ref traitBasedObject);
            RemoveTraitOnObject<Location>(ref traitBasedObject);
            RemoveTraitOnObject<Moveable>(ref traitBasedObject);

            TraitBasedObjects.RemoveAt(objectIndex);
            TraitBasedObjectIds.RemoveAt(objectIndex);

            return true;
        }


        public TTrait GetTraitOnObjectAtIndex<TTrait>(int traitBasedObjectIndex) where TTrait : struct, ITrait
        {
            var traitBasedObjectTraitIndex = TraitArrayIndex<TTrait>.Index;
            if (traitBasedObjectTraitIndex == -1)
                throw new ArgumentException($"Trait {typeof(TTrait)} not supported in this domain");

            var traitBasedObject = TraitBasedObjects[traitBasedObjectIndex];
            var traitBufferIndex = traitBasedObject[traitBasedObjectTraitIndex];
            if (traitBufferIndex == TraitBasedObject.Unset)
                throw new Exception($"Trait index for {typeof(TTrait)} is not set for domain object {traitBasedObject}");

            return GetBuffer<TTrait>()[traitBufferIndex];
        }

        public void SetTraitOnObjectAtIndex<T>(T trait, int traitBasedObjectIndex) where T : struct, ITrait
        {
            var traitBasedObjectTraitIndex = TraitArrayIndex<T>.Index;
            if (traitBasedObjectTraitIndex == -1)
                throw new ArgumentException($"Trait {typeof(T)} not supported in this domain");

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
            for (int i = 0; i < TraitBasedObjects.Length; i++)
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
            RemoveTraitOnObjectAtIndex<ActivationLock>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<ActivationSwitch>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<Carriable>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<Carrier>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<Character>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<EscapePoint>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<Item>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<KeyLock>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<Position>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<Waypoint>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<Location>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<Moveable>(traitBasedObjectIndex);

            TraitBasedObjects.RemoveAt(traitBasedObjectIndex);
            TraitBasedObjectIds.RemoveAt(traitBasedObjectIndex);

            return true;
        }


        public NativeArray<int> GetTraitBasedObjectIndices(NativeList<int> traitBasedObjectIndices, NativeArray<ComponentType> traitFilter)
        {
            for (var i = 0; i < TraitBasedObjects.Length; i++)
            {
                var traitBasedObject = TraitBasedObjects[i];
                if (traitBasedObject.MatchesTraitFilter(traitFilter))
                    traitBasedObjectIndices.Add(i);
            }

            return traitBasedObjectIndices.AsArray();
        }

        public NativeArray<int> GetTraitBasedObjectIndices(NativeList<int> traitBasedObjectIndices, params ComponentType[] traitFilter)
        {
            for (var i = 0; i < TraitBasedObjects.Length; i++)
            {
                var traitBasedObject = TraitBasedObjects[i];
                if (traitBasedObject.MatchesTraitFilter(traitFilter))
                    traitBasedObjectIndices.Add(i);
            }

            return traitBasedObjectIndices.AsArray();
        }

        public int GetTraitBasedObjectIndex(TraitBasedObject traitBasedObject)
        {
            for (int objectIndex = 0; objectIndex < TraitBasedObjects.Length; objectIndex++)
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
            for (int i = 0; i < TraitBasedObjectIds.Length; i++)
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
                    return ActivationLockBuffer.Reinterpret<T>();
                case 1:
                    return ActivationSwitchBuffer.Reinterpret<T>();
                case 2:
                    return CarriableBuffer.Reinterpret<T>();
                case 3:
                    return CarrierBuffer.Reinterpret<T>();
                case 4:
                    return CharacterBuffer.Reinterpret<T>();
                case 5:
                    return EscapePointBuffer.Reinterpret<T>();
                case 6:
                    return ItemBuffer.Reinterpret<T>();
                case 7:
                    return KeyLockBuffer.Reinterpret<T>();
                case 8:
                    return PositionBuffer.Reinterpret<T>();
                case 9:
                    return WaypointBuffer.Reinterpret<T>();
                case 10:
                    return LocationBuffer.Reinterpret<T>();
                case 11:
                    return MoveableBuffer.Reinterpret<T>();
            }

            return default;
        }

        public bool Equals(StateData rhsState)
        {
            if (StateEntity == rhsState.StateEntity)
                return true;

            // Easy check is to make sure each state has the same number of domain objects
            if (TraitBasedObjects.Length != rhsState.TraitBasedObjects.Length
                || ActivationLockBuffer.Length != rhsState.ActivationLockBuffer.Length
                || ActivationSwitchBuffer.Length != rhsState.ActivationSwitchBuffer.Length
                || CarriableBuffer.Length != rhsState.CarriableBuffer.Length
                || CarrierBuffer.Length != rhsState.CarrierBuffer.Length
                || CharacterBuffer.Length != rhsState.CharacterBuffer.Length
                || EscapePointBuffer.Length != rhsState.EscapePointBuffer.Length
                || ItemBuffer.Length != rhsState.ItemBuffer.Length
                || KeyLockBuffer.Length != rhsState.KeyLockBuffer.Length
                || PositionBuffer.Length != rhsState.PositionBuffer.Length
                || WaypointBuffer.Length != rhsState.WaypointBuffer.Length
                || LocationBuffer.Length != rhsState.LocationBuffer.Length
                || MoveableBuffer.Length != rhsState.MoveableBuffer.Length)
                return false;

            var objectMap = new ObjectCorrespondence(TraitBasedObjectIds.Length, Allocator.Temp);
            bool statesEqual = TryGetObjectMapping(rhsState, objectMap);
            objectMap.Dispose();

            return statesEqual;
        }

        bool ITraitBasedStateData<TraitBasedObject, StateData>.TryGetObjectMapping(StateData rhsState, ObjectCorrespondence objectMap)
        {
            return TryGetObjectMapping(rhsState, objectMap);
        }

        bool TryGetObjectMapping(StateData rhsState, ObjectCorrespondence objectMap)
        {
            objectMap.Initialize(TraitBasedObjectIds, rhsState.TraitBasedObjectIds);

            bool statesEqual = true;
            for (int lhsIndex = 0; lhsIndex < TraitBasedObjects.Length; lhsIndex++)
            {
                var lhsId = TraitBasedObjectIds[lhsIndex].Id;
                if (objectMap.TryGetValue(lhsId, out _)) // already matched
                    continue;

                // todo lhsIndex to start? would require swapping rhs on assignments, though
                bool matchFound = true;
                for (var rhsIndex = 0; rhsIndex < rhsState.TraitBasedObjects.Length; rhsIndex++)
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

            if (traitBasedObjectLHS.ActivationLockIndex != TraitBasedObject.Unset
                && !ActivationLockTraitAttributesEqual(ActivationLockBuffer[traitBasedObjectLHS.ActivationLockIndex], rhsState.ActivationLockBuffer[traitBasedObjectRHS.ActivationLockIndex]))
                return false;


            if (traitBasedObjectLHS.ActivationSwitchIndex != TraitBasedObject.Unset
                && !ActivationSwitchTraitAttributesEqual(ActivationSwitchBuffer[traitBasedObjectLHS.ActivationSwitchIndex], rhsState.ActivationSwitchBuffer[traitBasedObjectRHS.ActivationSwitchIndex]))
                return false;




            if (traitBasedObjectLHS.CharacterIndex != TraitBasedObject.Unset
                && !CharacterTraitAttributesEqual(CharacterBuffer[traitBasedObjectLHS.CharacterIndex], rhsState.CharacterBuffer[traitBasedObjectRHS.CharacterIndex]))
                return false;



            if (traitBasedObjectLHS.ItemIndex != TraitBasedObject.Unset
                && !ItemTraitAttributesEqual(ItemBuffer[traitBasedObjectLHS.ItemIndex], rhsState.ItemBuffer[traitBasedObjectRHS.ItemIndex]))
                return false;




            if (traitBasedObjectLHS.WaypointIndex != TraitBasedObject.Unset
                && !WaypointTraitAttributesEqual(WaypointBuffer[traitBasedObjectLHS.WaypointIndex], rhsState.WaypointBuffer[traitBasedObjectRHS.WaypointIndex]))
                return false;


            if (traitBasedObjectLHS.LocationIndex != TraitBasedObject.Unset
                && !LocationTraitAttributesEqual(LocationBuffer[traitBasedObjectLHS.LocationIndex], rhsState.LocationBuffer[traitBasedObjectRHS.LocationIndex]))
                return false;



            return true;
        }
        
        bool ActivationLockTraitAttributesEqual(ActivationLock one, ActivationLock two)
        {
            return
                    one.ActivationA == two.ActivationA && 
                    one.ActivationB == two.ActivationB;
        }
        
        bool ActivationSwitchTraitAttributesEqual(ActivationSwitch one, ActivationSwitch two)
        {
            return
                    one.Type == two.Type;
        }
        
        bool CharacterTraitAttributesEqual(Character one, Character two)
        {
            return
                    one.ID == two.ID;
        }
        
        bool ItemTraitAttributesEqual(Item one, Item two)
        {
            return
                    one.Type == two.Type;
        }
        
        bool WaypointTraitAttributesEqual(Waypoint one, Waypoint two)
        {
            return
                    one.Visited == two.Visited;
        }
        
        bool LocationTraitAttributesEqual(Location one, Location two)
        {
            return
                    one.Position == two.Position && 
                    one.Forward == two.Forward;
        }
        
        bool CheckRelationsAndQueueObjects(TraitBasedObject traitBasedObjectLHS, TraitBasedObject traitBasedObjectRHS, StateData rhsState, ObjectCorrespondence objectMap)
        {
            // edge walking - for relation properties
            ObjectId lhsRelationId;
            ObjectId rhsRelationId;
            ObjectId rhsAssignedId;
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


            return true;
        }

        public override int GetHashCode()
        {
            // h = 3860031 + (h+y)*2779 + (h*y*2)   // from How to Hash a Set by Richard O’Keefe
            var stateHashValue = 0;

            var objectIds = TraitBasedObjectIds;
            for (int i = 0; i < objectIds.Length; i++)
            {
                var element = objectIds[i];
                var value = element.GetHashCode();
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }

            for (int i = 0; i < ActivationLockBuffer.Length; i++)
            {
                var element = ActivationLockBuffer[i];
                var value = 397
                    ^ (int) element.ActivationA
                    ^ (int) element.ActivationB;
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            for (int i = 0; i < ActivationSwitchBuffer.Length; i++)
            {
                var element = ActivationSwitchBuffer[i];
                var value = 397
                    ^ (int) element.Type;
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            for (int i = 0; i < CarriableBuffer.Length; i++)
            {
                var element = CarriableBuffer[i];
                var value = 397;
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            for (int i = 0; i < CarrierBuffer.Length; i++)
            {
                var element = CarrierBuffer[i];
                var value = 397;
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            for (int i = 0; i < CharacterBuffer.Length; i++)
            {
                var element = CharacterBuffer[i];
                var value = 397
                    ^ element.ID.GetHashCode();
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            for (int i = 0; i < EscapePointBuffer.Length; i++)
            {
                var element = EscapePointBuffer[i];
                var value = 397;
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            for (int i = 0; i < ItemBuffer.Length; i++)
            {
                var element = ItemBuffer[i];
                var value = 397
                    ^ (int) element.Type;
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            for (int i = 0; i < KeyLockBuffer.Length; i++)
            {
                var element = KeyLockBuffer[i];
                var value = 397;
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            for (int i = 0; i < PositionBuffer.Length; i++)
            {
                var element = PositionBuffer[i];
                var value = 397;
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            for (int i = 0; i < WaypointBuffer.Length; i++)
            {
                var element = WaypointBuffer[i];
                var value = 397
                    ^ element.Visited.GetHashCode();
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            for (int i = 0; i < LocationBuffer.Length; i++)
            {
                var element = LocationBuffer[i];
                var value = 397
                    ^ element.Position.GetHashCode()
                    ^ element.Forward.GetHashCode();
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            for (int i = 0; i < MoveableBuffer.Length; i++)
            {
                var element = MoveableBuffer[i];
                var value = 397;
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }

            return stateHashValue;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var traitBasedObjectIndex = 0; traitBasedObjectIndex < TraitBasedObjects.Length; traitBasedObjectIndex++)
            {
                var traitBasedObject = TraitBasedObjects[traitBasedObjectIndex];
                sb.AppendLine(TraitBasedObjectIds[traitBasedObjectIndex].ToString());

                var i = 0;

                var traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(ActivationLockBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(ActivationSwitchBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(CarriableBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(CarrierBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(CharacterBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(EscapePointBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(ItemBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(KeyLockBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(PositionBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(WaypointBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(LocationBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(MoveableBuffer[traitIndex].ToString());

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }

    public struct StateDataContext : ITraitBasedStateDataContext<TraitBasedObject, StateEntityKey, StateData>
    {
        internal EntityCommandBuffer.Concurrent EntityCommandBuffer;
        internal EntityArchetype m_StateArchetype;
        internal int JobIndex; //todo assign

        [ReadOnly] public BufferFromEntity<TraitBasedObject> TraitBasedObjects;
        [ReadOnly] public BufferFromEntity<TraitBasedObjectId> TraitBasedObjectIds;

        [ReadOnly] public BufferFromEntity<ActivationLock> ActivationLockData;
        [ReadOnly] public BufferFromEntity<ActivationSwitch> ActivationSwitchData;
        [ReadOnly] public BufferFromEntity<Carriable> CarriableData;
        [ReadOnly] public BufferFromEntity<Carrier> CarrierData;
        [ReadOnly] public BufferFromEntity<Character> CharacterData;
        [ReadOnly] public BufferFromEntity<EscapePoint> EscapePointData;
        [ReadOnly] public BufferFromEntity<Item> ItemData;
        [ReadOnly] public BufferFromEntity<KeyLock> KeyLockData;
        [ReadOnly] public BufferFromEntity<Position> PositionData;
        [ReadOnly] public BufferFromEntity<Waypoint> WaypointData;
        [ReadOnly] public BufferFromEntity<Location> LocationData;
        [ReadOnly] public BufferFromEntity<Moveable> MoveableData;

        public StateDataContext(JobComponentSystem system, EntityArchetype stateArchetype)
        {
            EntityCommandBuffer = default;
            TraitBasedObjects = system.GetBufferFromEntity<TraitBasedObject>(true);
            TraitBasedObjectIds = system.GetBufferFromEntity<TraitBasedObjectId>(true);

            ActivationLockData = system.GetBufferFromEntity<ActivationLock>(true);
            ActivationSwitchData = system.GetBufferFromEntity<ActivationSwitch>(true);
            CarriableData = system.GetBufferFromEntity<Carriable>(true);
            CarrierData = system.GetBufferFromEntity<Carrier>(true);
            CharacterData = system.GetBufferFromEntity<Character>(true);
            EscapePointData = system.GetBufferFromEntity<EscapePoint>(true);
            ItemData = system.GetBufferFromEntity<Item>(true);
            KeyLockData = system.GetBufferFromEntity<KeyLock>(true);
            PositionData = system.GetBufferFromEntity<Position>(true);
            WaypointData = system.GetBufferFromEntity<Waypoint>(true);
            LocationData = system.GetBufferFromEntity<Location>(true);
            MoveableData = system.GetBufferFromEntity<Moveable>(true);

            m_StateArchetype = stateArchetype;
            JobIndex = 0; // todo set on all actions
        }

        public StateData GetStateData(StateEntityKey stateKey)
        {
            var stateEntity = stateKey.Entity;

            return new StateData
            {
                StateEntity = stateEntity,
                TraitBasedObjects = TraitBasedObjects[stateEntity],
                TraitBasedObjectIds = TraitBasedObjectIds[stateEntity],

                ActivationLockBuffer = ActivationLockData[stateEntity],
                ActivationSwitchBuffer = ActivationSwitchData[stateEntity],
                CarriableBuffer = CarriableData[stateEntity],
                CarrierBuffer = CarrierData[stateEntity],
                CharacterBuffer = CharacterData[stateEntity],
                EscapePointBuffer = EscapePointData[stateEntity],
                ItemBuffer = ItemData[stateEntity],
                KeyLockBuffer = KeyLockData[stateEntity],
                PositionBuffer = PositionData[stateEntity],
                WaypointBuffer = WaypointData[stateEntity],
                LocationBuffer = LocationData[stateEntity],
                MoveableBuffer = MoveableData[stateEntity],
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
            return x.Equals(y);
        }

        public int GetHashCode(StateData obj)
        {
            return obj.GetHashCode();
        }
    }

    [DisableAutoCreation]
    public class StateManager : JobComponentSystem, ITraitBasedStateManager<TraitBasedObject, StateEntityKey, StateData, StateDataContext>
    {
        public ExclusiveEntityTransaction ExclusiveEntityTransaction;
        public event Action Destroying;

        List<EntityCommandBuffer> m_EntityCommandBuffers;
        EntityArchetype m_StateArchetype;

        protected override void OnCreate()
        {
            m_StateArchetype = EntityManager.CreateArchetype(typeof(State), typeof(TraitBasedObject), typeof(TraitBasedObjectId), typeof(HashCode),
                typeof(ActivationLock),
                typeof(ActivationSwitch),
                typeof(Carriable),
                typeof(Carrier),
                typeof(Character),
                typeof(EscapePoint),
                typeof(Item),
                typeof(KeyLock),
                typeof(Position),
                typeof(Waypoint),
                typeof(Location),
                typeof(Moveable));

            BeginEntityExclusivity();
            m_EntityCommandBuffers = new List<EntityCommandBuffer>();
        }

        protected override void OnDestroy()
        {
            Destroying?.Invoke();
            EndEntityExclusivity();
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
            EndEntityExclusivity();
            var stateEntity = EntityManager.CreateEntity(m_StateArchetype);
            BeginEntityExclusivity();
            return new StateData(this, stateEntity, true);
        }

        public StateData GetStateData(StateEntityKey stateKey, bool readWrite = false)
        {
            return !Enabled ? default : new StateData(this, stateKey.Entity, readWrite);
        }

        public void DestroyState(StateEntityKey stateKey)
        {
            if (EntityManager != null && EntityManager.IsCreated)
            {
                EndEntityExclusivity();
                EntityManager.DestroyEntity(stateKey.Entity);
                BeginEntityExclusivity();
            }
        }

        public StateDataContext GetStateDataContext()
        {
            return new StateDataContext(this, m_StateArchetype);
        }

        public StateEntityKey GetStateDataKey(StateData stateData)
        {
            return new StateEntityKey { Entity = stateData.StateEntity, HashCode = stateData.GetHashCode()};
        }

        public StateData CopyStateData(StateData stateData)
        {
            EndEntityExclusivity();
            var copyStateEntity = EntityManager.Instantiate(stateData.StateEntity);
            BeginEntityExclusivity();
            return new StateData(this, copyStateEntity, true);
        }

        public StateEntityKey CopyState(StateEntityKey stateKey)
        {
            EndEntityExclusivity();
            var copyStateEntity = EntityManager.Instantiate(stateKey.Entity);
            BeginEntityExclusivity();
            var stateData = GetStateData(stateKey);
            return new StateEntityKey { Entity = copyStateEntity, HashCode = stateData.GetHashCode()};
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps) => JobHandle.CombineDependencies(inputDeps, EntityManager.ExclusiveEntityTransactionDependency);

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
            ExclusiveEntityTransaction = EntityManager.BeginExclusiveEntityTransaction();
        }

        void EndEntityExclusivity()
        {
            EntityManager.EndExclusiveEntityTransaction();

            foreach (var ecb in m_EntityCommandBuffers)
            {
                if (ecb.IsCreated)
                    ecb.Dispose();
            }
            m_EntityCommandBuffers.Clear();
        }
    }

    struct DestroyStatesJobScheduler : IDestroyStatesScheduler<StateEntityKey, StateData, StateDataContext, StateManager>
    {
        public StateManager StateManager { private get; set; }
        public NativeQueue<StateEntityKey> StatesToDestroy { private get; set; }

        public JobHandle Schedule(JobHandle inputDeps)
        {
            var entityManager = StateManager.EntityManager;
            inputDeps = JobHandle.CombineDependencies(inputDeps, entityManager.ExclusiveEntityTransactionDependency);

            var stateDataContext = StateManager.GetStateDataContext();
            var ecb = StateManager.GetEntityCommandBuffer();
            stateDataContext.EntityCommandBuffer = ecb.ToConcurrent();
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

            entityManager.ExclusiveEntityTransactionDependency = playbackECBJobHandle;
            return playbackECBJobHandle;
        }
    }
}
