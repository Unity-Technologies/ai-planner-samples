using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.Traits;
using Generated.Semantic.Traits.Enums;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct Blocker : ITrait, IBufferElementData, IEquatable<Blocker>
    {
        public const string FieldLife = "Life";
        public System.Int32 Life;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(Life):
                    Life = (System.Int32)value;
                    break;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Blocker.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(Life):
                    return Life;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Blocker.");
            }
        }

        public bool Equals(Blocker other)
        {
            return Life == other.Life;
        }

        public override string ToString()
        {
            return $"Blocker\n  Life: {Life}";
        }
    }
}
