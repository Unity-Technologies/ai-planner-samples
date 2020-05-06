using System;
using Unity.Entities;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using Generated.AI.Planner.StateRepresentation.Enums;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct ActivationLock : ITrait, IEquatable<ActivationLock>
    {
        public const string FieldActivationA = "ActivationA";
        public const string FieldActivationB = "ActivationB";
        public Generated.AI.Planner.StateRepresentation.Enums.ActivationType ActivationA;
        public Generated.AI.Planner.StateRepresentation.Enums.ActivationType ActivationB;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(ActivationA):
                    ActivationA = (Generated.AI.Planner.StateRepresentation.Enums.ActivationType)Enum.ToObject(typeof(Generated.AI.Planner.StateRepresentation.Enums.ActivationType), value);
                    break;
                case nameof(ActivationB):
                    ActivationB = (Generated.AI.Planner.StateRepresentation.Enums.ActivationType)Enum.ToObject(typeof(Generated.AI.Planner.StateRepresentation.Enums.ActivationType), value);
                    break;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait ActivationLock.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(ActivationA):
                    return ActivationA;
                case nameof(ActivationB):
                    return ActivationB;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait ActivationLock.");
            }
        }

        public bool Equals(ActivationLock other)
        {
            return ActivationA == other.ActivationA && ActivationB == other.ActivationB;
        }

        public override string ToString()
        {
            return $"ActivationLock\n  ActivationA: {ActivationA}\n  ActivationB: {ActivationB}";
        }
    }
}
