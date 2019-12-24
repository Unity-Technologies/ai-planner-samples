using System;
using Unity.Entities;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using AI.Planner.Domains.Enums;

namespace AI.Planner.Domains
{
    [Serializable]
    public struct EscapePoint : ITrait, IEquatable<EscapePoint>
    {

        public void SetField(string fieldName, object value)
        {
        }

        public object GetField(string fieldName)
        {
            throw new ArgumentException("No fields exist on trait EscapePoint.");
        }

        public bool Equals(EscapePoint other)
        {
            return true;
        }

        public override string ToString()
        {
            return $"EscapePoint";
        }
    }
}
