using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct DirtData : ITraitData, IEquatable<DirtData>
    {

        public bool Equals(DirtData other)
        {
            return true;
        }

        public override string ToString()
        {
            return $"Dirt";
        }
    }
}
