using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.Traits;
using Generated.Semantic.Traits.Enums;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct Waypoint : ITrait, IBufferElementData, IEquatable<Waypoint>
    {
        public const string FieldLeft = "Left";
        public const string FieldRight = "Right";
        public const string FieldUp = "Up";
        public const string FieldDown = "Down";
        public const string FieldOccupied = "Occupied";
        public const string FieldStepsToEnd = "StepsToEnd";
        public Unity.AI.Planner.Traits.TraitBasedObjectId Left;
        public Unity.AI.Planner.Traits.TraitBasedObjectId Right;
        public Unity.AI.Planner.Traits.TraitBasedObjectId Up;
        public Unity.AI.Planner.Traits.TraitBasedObjectId Down;
        public System.Boolean Occupied;
        public System.Int32 StepsToEnd;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(Left):
                    Left = (Unity.AI.Planner.Traits.TraitBasedObjectId)value;
                    break;
                case nameof(Right):
                    Right = (Unity.AI.Planner.Traits.TraitBasedObjectId)value;
                    break;
                case nameof(Up):
                    Up = (Unity.AI.Planner.Traits.TraitBasedObjectId)value;
                    break;
                case nameof(Down):
                    Down = (Unity.AI.Planner.Traits.TraitBasedObjectId)value;
                    break;
                case nameof(Occupied):
                    Occupied = (System.Boolean)value;
                    break;
                case nameof(StepsToEnd):
                    StepsToEnd = (System.Int32)value;
                    break;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Waypoint.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(Left):
                    return Left;
                case nameof(Right):
                    return Right;
                case nameof(Up):
                    return Up;
                case nameof(Down):
                    return Down;
                case nameof(Occupied):
                    return Occupied;
                case nameof(StepsToEnd):
                    return StepsToEnd;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Waypoint.");
            }
        }

        public bool Equals(Waypoint other)
        {
            return Left == other.Left && Right == other.Right && Up == other.Up && Down == other.Down && Occupied == other.Occupied && StepsToEnd == other.StepsToEnd;
        }

        public override string ToString()
        {
            return $"Waypoint\n  Left: {Left}\n  Right: {Right}\n  Up: {Up}\n  Down: {Down}\n  Occupied: {Occupied}\n  StepsToEnd: {StepsToEnd}";
        }
    }
}
