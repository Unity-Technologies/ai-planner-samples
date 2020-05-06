using System;
using Unity.Entities;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using Generated.AI.Planner.StateRepresentation.Enums;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct Cell : ITrait, IEquatable<Cell>
    {
        public const string FieldType = "Type";
        public const string FieldLeft = "Left";
        public const string FieldRight = "Right";
        public const string FieldTop = "Top";
        public const string FieldBottom = "Bottom";
        public Generated.AI.Planner.StateRepresentation.Enums.CellType Type;
        public Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId Left;
        public Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId Right;
        public Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId Top;
        public Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId Bottom;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(Type):
                    Type = (Generated.AI.Planner.StateRepresentation.Enums.CellType)Enum.ToObject(typeof(Generated.AI.Planner.StateRepresentation.Enums.CellType), value);
                    break;
                case nameof(Left):
                    Left = (Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId)value;
                    break;
                case nameof(Right):
                    Right = (Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId)value;
                    break;
                case nameof(Top):
                    Top = (Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId)value;
                    break;
                case nameof(Bottom):
                    Bottom = (Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId)value;
                    break;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Cell.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(Type):
                    return Type;
                case nameof(Left):
                    return Left;
                case nameof(Right):
                    return Right;
                case nameof(Top):
                    return Top;
                case nameof(Bottom):
                    return Bottom;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Cell.");
            }
        }

        public bool Equals(Cell other)
        {
            return Type == other.Type && Left == other.Left && Right == other.Right && Top == other.Top && Bottom == other.Bottom;
        }

        public override string ToString()
        {
            return $"Cell\n  Type: {Type}\n  Left: {Left}\n  Right: {Right}\n  Top: {Top}\n  Bottom: {Bottom}";
        }
    }
}
