using System;
using Unity.Entities;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using AI.Planner.Domains.Enums;

namespace AI.Planner.Domains
{
    [Serializable]
    public struct Cell : ITrait, IEquatable<Cell>
    {
        public const string FieldType = "Type";
        public const string FieldLeft = "Left";
        public const string FieldRight = "Right";
        public const string FieldTop = "Top";
        public const string FieldBottom = "Bottom";
        public AI.Planner.Domains.Enums.CellType Type;
        public Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId Left;
        public Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId Right;
        public Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId Top;
        public Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId Bottom;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(Type):
                    Type = (AI.Planner.Domains.Enums.CellType)Enum.ToObject(typeof(AI.Planner.Domains.Enums.CellType), value);
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
            return $"Cell: {Type} {Left} {Right} {Top} {Bottom}";
        }
    }
}
