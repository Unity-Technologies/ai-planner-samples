using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.Traits;
using Generated.Semantic.Traits.Enums;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct Carrier : ITrait, IBufferElementData, IEquatable<Carrier>
    {
        public const string FieldCarried = "Carried";
        public Unity.AI.Planner.Traits.TraitBasedObjectId Carried;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(Carried):
                    Carried = (Unity.AI.Planner.Traits.TraitBasedObjectId)value;
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
