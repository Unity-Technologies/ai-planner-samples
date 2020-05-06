using System;
using Unity.Entities;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using Generated.AI.Planner.StateRepresentation.Enums;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct Coordinate : ITrait, IEquatable<Coordinate>
    {
        public const string FieldX = "X";
        public const string FieldY = "Y";
        public System.Int64 X;
        public System.Int64 Y;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(X):
                    X = (System.Int64)value;
                    break;
                case nameof(Y):
                    Y = (System.Int64)value;
                    break;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Coordinate.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(X):
                    return X;
                case nameof(Y):
                    return Y;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Coordinate.");
            }
        }

        public bool Equals(Coordinate other)
        {
            return X == other.X && Y == other.Y;
        }

        public override string ToString()
        {
            return $"Coordinate\n  X: {X}\n  Y: {Y}";
        }
    }
}
