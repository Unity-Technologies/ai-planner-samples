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
        public const string FieldCarriedBy = "CarriedBy";
        public Unity.AI.Planner.Traits.TraitBasedObjectId CarriedBy;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(CarriedBy):
                    CarriedBy = (Unity.AI.Planner.Traits.TraitBasedObjectId)value;
                    break;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Item.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(CarriedBy):
                    return CarriedBy;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Item.");
            }
        }

        public bool Equals(Item other)
        {
            return CarriedBy == other.CarriedBy;
        }

        public override string ToString()
        {
            return $"Item\n  CarriedBy: {CarriedBy}";
        }
    }
}
