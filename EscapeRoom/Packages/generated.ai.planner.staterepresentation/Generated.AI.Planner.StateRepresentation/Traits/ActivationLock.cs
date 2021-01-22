using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.Traits;
using Generated.Semantic.Traits.Enums;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct ActivationLock : ITrait, IBufferElementData, IEquatable<ActivationLock>
    {
        public const string FieldActivationA = "ActivationA";
        public const string FieldActivationB = "ActivationB";
        public Generated.Semantic.Traits.Enums.ActivationType ActivationA;
        public Generated.Semantic.Traits.Enums.ActivationType ActivationB;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(ActivationA):
                    ActivationA = (Generated.Semantic.Traits.Enums.ActivationType)Enum.ToObject(typeof(Generated.Semantic.Traits.Enums.ActivationType), value);
                    break;
                case nameof(ActivationB):
                    ActivationB = (Generated.Semantic.Traits.Enums.ActivationType)Enum.ToObject(typeof(Generated.Semantic.Traits.Enums.ActivationType), value);
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
