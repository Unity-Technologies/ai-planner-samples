using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.Traits;
using Generated.Semantic.Traits.Enums;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct Cell : ITrait, IBufferElementData, IEquatable<Cell>
    {
        public const string FieldType = "Type";
        public const string FieldLeft = "Left";
        public const string FieldRight = "Right";
        public const string FieldTop = "Top";
        public const string FieldBottom = "Bottom";
        public Generated.Semantic.Traits.Enums.CellType Type;
        public Unity.AI.Planner.Traits.TraitBasedObjectId Left;
        public Unity.AI.Planner.Traits.TraitBasedObjectId Right;
        public Unity.AI.Planner.Traits.TraitBasedObjectId Top;
        public Unity.AI.Planner.Traits.TraitBasedObjectId Bottom;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(Type):
                    Type = (Generated.Semantic.Traits.Enums.CellType)Enum.ToObject(typeof(Generated.Semantic.Traits.Enums.CellType), value);
                    break;
                case nameof(Left):
                    Left = (Unity.AI.Planner.Traits.TraitBasedObjectId)value;
                    break;
                case nameof(Right):
                    Right = (Unity.AI.Planner.Traits.TraitBasedObjectId)value;
                    break;
                case nameof(Top):
                    Top = (Unity.AI.Planner.Traits.TraitBasedObjectId)value;
                    break;
                case nameof(Bottom):
                    Bottom = (Unity.AI.Planner.Traits.TraitBasedObjectId)value;
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
