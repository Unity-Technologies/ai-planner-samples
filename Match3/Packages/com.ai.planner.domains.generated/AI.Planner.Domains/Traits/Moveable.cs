using System;
using Unity.Entities;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using AI.Planner.Domains.Enums;

namespace AI.Planner.Domains
{
    [Serializable]
    public struct Moveable : ITrait, IEquatable<Moveable>
    {

        public void SetField(string fieldName, object value)
        {
        }

        public object GetField(string fieldName)
        {
            throw new ArgumentException("No fields exist on trait Moveable.");
        }

        public bool Equals(Moveable other)
        {
            return true;
        }

        public override string ToString()
        {
            return $"Moveable";
        }
    }
}
