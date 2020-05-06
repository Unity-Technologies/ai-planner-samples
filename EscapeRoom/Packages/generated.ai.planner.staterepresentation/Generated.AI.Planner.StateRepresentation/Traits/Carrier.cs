using System;
using Unity.Entities;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using Generated.AI.Planner.StateRepresentation.Enums;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct Carrier : ITrait, IEquatable<Carrier>
    {
        public const string FieldCarried = "Carried";
        public Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId Carried;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(Carried):
                    Carried = (Unity.AI.Planner.DomainLanguage.TraitBased.TraitBasedObjectId)value;
                    break;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Carrier.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(Carried):
                    return Carried;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Carrier.");
            }
        }

        public bool Equals(Carrier other)
        {
            return Carried == other.Carried;
        }

        public override string ToString()
        {
            return $"Carrier\n  Carried: {Carried}";
        }
    }
}
