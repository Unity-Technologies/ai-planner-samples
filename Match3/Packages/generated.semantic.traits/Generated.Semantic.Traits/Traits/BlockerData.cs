using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct BlockerData : ITraitData, IEquatable<BlockerData>
    {
        public System.Int32 Life;

        public bool Equals(BlockerData other)
        {
            return Life.Equals(other.Life);
        }

        public override string ToString()
        {
            return $"Blocker: {Life}";
        }
    }
}
