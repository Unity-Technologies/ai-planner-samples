using System;
using Unity.Entities;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using Generated.AI.Planner.StateRepresentation.Enums;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct Character : ITrait, IEquatable<Character>
    {
        public const string FieldWaypoint = "Waypoint";
        public const string FieldID = "ID";
        public Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId Waypoint;
        public System.Int64 ID;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(Waypoint):
                    Waypoint = (Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId)value;
                    break;
                case nameof(ID):
                    ID = (System.Int64)value;
                    break;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Character.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(Waypoint):
                    return Waypoint;
                case nameof(ID):
                    return ID;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Character.");
            }
        }

        public bool Equals(Character other)
        {
            return Waypoint == other.Waypoint && ID == other.ID;
        }

        public override string ToString()
        {
            return $"Character\n  Waypoint: {Waypoint}\n  ID: {ID}";
        }
    }
}
