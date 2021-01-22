using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct CoverData : ITraitData, IEquatable<CoverData>
    {
        public System.Boolean SpotTaken;
        public Generated.Semantic.Traits.Enums.Direction Direction;

        public bool Equals(CoverData other)
        {
            return SpotTaken.Equals(other.SpotTaken) && Direction.Equals(other.Direction);
        }

        public override string ToString()
        {
            return $"Cover: {SpotTaken} {Direction}";
        }
    }
}
