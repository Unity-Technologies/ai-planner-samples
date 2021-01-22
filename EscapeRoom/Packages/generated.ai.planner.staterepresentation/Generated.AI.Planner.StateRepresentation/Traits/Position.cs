using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.Traits;
using Generated.Semantic.Traits.Enums;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct Position : ITrait, IBufferElementData, IEquatable<Position>
    {
        public const string FieldWaypoint = "Waypoint";
        public Unity.AI.Planner.Traits.TraitBasedObjectId Waypoint;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(Waypoint):
                    Waypoint = (Unity.AI.Planner.Traits.TraitBasedObjectId)value;
                    break;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Position.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(Waypoint):
                    return Waypoint;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Position.");
            }
        }

        public bool Equals(Position other)
        {
            return Waypoint == other.Waypoint;
        }

        public override string ToString()
        {
            return $"Position\n  Waypoint: {Waypoint}";
        }
    }
}
