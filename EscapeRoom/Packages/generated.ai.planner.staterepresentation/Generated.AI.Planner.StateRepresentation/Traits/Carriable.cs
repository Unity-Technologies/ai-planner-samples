using System;
using Unity.Entities;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using Generated.AI.Planner.StateRepresentation.Enums;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct Carriable : ITrait, IEquatable<Carriable>
    {
        public const string FieldCarriedBy = "CarriedBy";
        public Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId CarriedBy;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(CarriedBy):
                    CarriedBy = (Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId)value;
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
