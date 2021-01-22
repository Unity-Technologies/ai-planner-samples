using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct CoordinateData : ITraitData, IEquatable<CoordinateData>
    {
        public System.Int32 X;
        public System.Int32 Y;

        public bool Equals(CoordinateData other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override string ToString()
        {
            return $"Coordinate: {X} {Y}";
        }
    }
}
