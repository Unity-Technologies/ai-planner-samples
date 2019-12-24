using System;
using Unity.Entities;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using AI.Planner.Domains.Enums;

namespace AI.Planner.Domains
{
    [Serializable]
    public struct ActivationSwitch : ITrait, IEquatable<ActivationSwitch>
    {
        public const string FieldType = "Type";
        public AI.Planner.Domains.Enums.ActivationType Type;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(Type):
                    Type = (AI.Planner.Domains.Enums.ActivationType)Enum.ToObject(typeof(AI.Planner.Domains.Enums.ActivationType), value);
                    break;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait ActivationSwitch.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(Type):
                    return Type;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait ActivationSwitch.");
            }
        }

        public bool Equals(ActivationSwitch other)
        {
            return Type == other.Type;
        }

        public override string ToString()
        {
            return $"ActivationSwitch: {Type}";
        }
    }
}
