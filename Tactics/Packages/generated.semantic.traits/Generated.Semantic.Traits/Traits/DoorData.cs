using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct DoorData : ITraitData, IEquatable<DoorData>
    {
        public System.Boolean Open;

        public bool Equals(DoorData other)
        {
            return Open.Equals(other.Open);
        }

        public override string ToString()
        {
            return $"Door: {Open}";
        }
    }
}
