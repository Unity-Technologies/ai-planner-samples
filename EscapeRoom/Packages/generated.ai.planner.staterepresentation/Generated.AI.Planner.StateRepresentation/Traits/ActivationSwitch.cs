using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.Traits;
using Generated.Semantic.Traits.Enums;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct ActivationSwitch : ITrait, IBufferElementData, IEquatable<ActivationSwitch>
    {
        public const string FieldType = "Type";
        public Generated.Semantic.Traits.Enums.ActivationType Type;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(Type):
                    Type = (Generated.Semantic.Traits.Enums.ActivationType)Enum.ToObject(typeof(Generated.Semantic.Traits.Enums.ActivationType), value);
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
            return $"ActivationSwitch\n  Type: {Type}";
        }
    }
}
