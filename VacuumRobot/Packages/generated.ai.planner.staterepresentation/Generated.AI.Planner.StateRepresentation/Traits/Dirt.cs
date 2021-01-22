using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.Traits;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct Dirt : ITrait, IBufferElementData, IEquatable<Dirt>
    {

        public void SetField(string fieldName, object value)
        {
        }

        public object GetField(string fieldName)
        {
            throw new ArgumentException("No fields exist on trait Dirt.");
        }

        public bool Equals(Dirt other)
        {
            return true;
        }

        public override string ToString()
        {
            return $"Dirt";
        }
    }
}
