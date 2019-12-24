using System;
using Unity.Entities;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using AI.Planner.Domains.Enums;

namespace AI.Planner.Domains
{
    [Serializable]
    public struct ActivationLock : ITrait, IEquatable<ActivationLock>
    {
        public const string FieldActivationA = "ActivationA";
        public const string FieldActivationB = "ActivationB";
        public AI.Planner.Domains.Enums.ActivationType ActivationA;
        public AI.Planner.Domains.Enums.ActivationType ActivationB;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(ActivationA):
                    ActivationA = (AI.Planner.Domains.Enums.ActivationType)Enum.ToObject(typeof(AI.Planner.Domains.Enums.ActivationType), value);
                    break;
                case nameof(ActivationB):
                    ActivationB = (AI.Planner.Domains.Enums.ActivationType)Enum.ToObject(typeof(AI.Planner.Domains.Enums.ActivationType), value);
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
            return $"ActivationLock: {ActivationA} {ActivationB}";
        }
    }
}
