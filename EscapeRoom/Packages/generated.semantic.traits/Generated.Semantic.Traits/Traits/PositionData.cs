using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct PositionData : ITraitData, IEquatable<PositionData>
    {
        public Unity.Entities.Entity Waypoint;

        public bool Equals(PositionData other)
        {
            return Waypoint.Equals(other.Waypoint);
        }

        public override string ToString()
        {
            return $"Position: {Waypoint}";
        }
    }
}
