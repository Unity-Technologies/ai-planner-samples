using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct EscapePointData : ITraitData, IEquatable<EscapePointData>
    {

        public bool Equals(EscapePointData other)
        {
            return true;
        }

        public override string ToString()
        {
            return $"EscapePoint";
        }
    }
}
