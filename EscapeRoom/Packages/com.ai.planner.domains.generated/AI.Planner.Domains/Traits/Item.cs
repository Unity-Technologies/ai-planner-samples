using System;
using Unity.Entities;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using AI.Planner.Domains.Enums;

namespace AI.Planner.Domains
{
    [Serializable]
    public struct Item : ITrait, IEquatable<Item>
    {
        public const string FieldType = "Type";
        public AI.Planner.Domains.Enums.ItemType Type;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(Type):
                    Type = (AI.Planner.Domains.Enums.ItemType)Enum.ToObject(typeof(AI.Planner.Domains.Enums.ItemType), value);
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
            return $"Item: {Type}";
        }
    }
}
