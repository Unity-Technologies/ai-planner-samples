using System;
using Unity.Entities;
using Unity.AI.Planner.DomainLanguage.TraitBased;

namespace Generated.AI.Planner.StateRepresentation
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
