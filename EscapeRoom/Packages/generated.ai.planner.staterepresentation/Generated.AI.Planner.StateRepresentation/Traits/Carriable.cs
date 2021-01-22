using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.Traits;
using Generated.Semantic.Traits.Enums;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct Carriable : ITrait, IBufferElementData, IEquatable<Carriable>
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
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Carriable.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(CarriedBy):
                    return CarriedBy;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Carriable.");
            }
        }

        public bool Equals(Carriable other)
        {
            return CarriedBy == other.CarriedBy;
        }

        public override string ToString()
        {
            return $"Carriable\n  CarriedBy: {CarriedBy}";
        }
    }
}
