using System;
using Unity.Entities;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using AI.Planner.Domains.Enums;

namespace AI.Planner.Domains
{
    [Serializable]
    public struct Waypoint : ITrait, IEquatable<Waypoint>
    {
        public const string FieldLeft = "Left";
        public const string FieldRight = "Right";
        public const string FieldUp = "Up";
        public const string FieldDown = "Down";
        public const string FieldOccupied = "Occupied";
        public const string FieldVisited = "Visited";
        public Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId Left;
        public Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId Right;
        public Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId Up;
        public Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId Down;
        public System.Boolean Occupied;
        public System.Int64 Visited;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(Left):
                    Left = (Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId)value;
                    break;
                case nameof(Right):
                    Right = (Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId)value;
                    break;
                case nameof(Up):
                    Up = (Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId)value;
                    break;
                case nameof(Down):
                    Down = (Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId)value;
                    break;
                case nameof(Occupied):
                    Occupied = (System.Boolean)value;
                    break;
                case nameof(Visited):
                    Visited = (System.Int64)value;
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
                case nameof(Visited):
                    return Visited;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Waypoint.");
            }
        }

        public bool Equals(Waypoint other)
        {
            return Left == other.Left && Right == other.Right && Up == other.Up && Down == other.Down && Occupied == other.Occupied && Visited == other.Visited;
        }

        public override string ToString()
        {
            return $"Waypoint: {Left} {Right} {Up} {Down} {Occupied} {Visited}";
        }
    }
}
