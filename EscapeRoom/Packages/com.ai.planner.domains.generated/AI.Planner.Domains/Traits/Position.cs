using System;
using Unity.Entities;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using AI.Planner.Domains.Enums;

namespace AI.Planner.Domains
{
    [Serializable]
    public struct Position : ITrait, IEquatable<Position>
    {
        public const string FieldWaypoint = "Waypoint";
        public Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId Waypoint;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(Waypoint):
                    Waypoint = (Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId)value;
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
            return $"Position: {Waypoint}";
        }
    }
}
