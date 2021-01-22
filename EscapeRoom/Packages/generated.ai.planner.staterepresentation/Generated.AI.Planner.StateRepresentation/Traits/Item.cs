using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.Traits;
using Generated.Semantic.Traits.Enums;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct Item : ITrait, IBufferElementData, IEquatable<Item>
    {
        public const string FieldType = "Type";
        public Generated.Semantic.Traits.Enums.ItemType Type;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(Type):
                    Type = (Generated.Semantic.Traits.Enums.ItemType)Enum.ToObject(typeof(Generated.Semantic.Traits.Enums.ItemType), value);
                    break;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Item.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(Type):
                    return Type;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Item.");
            }
        }

        public bool Equals(Item other)
        {
            return Type == other.Type;
        }

        public override string ToString()
        {
            return $"Item\n  Type: {Type}";
        }
    }
}
