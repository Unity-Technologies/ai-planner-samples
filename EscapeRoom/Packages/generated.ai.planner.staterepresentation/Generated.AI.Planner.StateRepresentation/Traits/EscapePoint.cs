using System;
using Unity.Entities;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using Generated.AI.Planner.StateRepresentation.Enums;

namespace Generated.AI.Planner.StateRepresentation
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
