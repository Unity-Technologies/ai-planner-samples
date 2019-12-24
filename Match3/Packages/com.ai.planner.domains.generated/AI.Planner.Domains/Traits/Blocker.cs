using System;
using Unity.Entities;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using AI.Planner.Domains.Enums;

namespace AI.Planner.Domains
{
    [Serializable]
    public struct Blocker : ITrait, IEquatable<Blocker>
    {
        public const string FieldLife = "Life";
        public System.Int64 Life;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(Life):
                    Life = (System.Int64)value;
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
            return $"Blocker: {Life}";
        }
    }
}
