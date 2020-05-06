using System;
using Unity.Entities;
using Unity.AI.Planner.DomainLanguage.TraitBased;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct Robot : ITrait, IEquatable<Robot>
    {

        public void SetField(string fieldName, object value)
        {
        }

        public object GetField(string fieldName)
        {
            throw new ArgumentException("No fields exist on trait Robot.");
        }

        public bool Equals(Robot other)
        {
            return true;
        }

        public override string ToString()
        {
            return $"Robot";
        }
    }
}
