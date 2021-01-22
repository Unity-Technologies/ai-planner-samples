using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct CellData : ITraitData, IEquatable<CellData>
    {
        public Generated.Semantic.Traits.Enums.CellType Type;
        public Unity.Entities.Entity Left;
        public Unity.Entities.Entity Right;
        public Unity.Entities.Entity Top;
        public Unity.Entities.Entity Bottom;

        public bool Equals(CellData other)
        {
            return Type.Equals(other.Type) && Left.Equals(other.Left) && Right.Equals(other.Right) && Top.Equals(other.Top) && Bottom.Equals(other.Bottom);
        }

        public override string ToString()
        {
            return $"Cell: {Type} {Left} {Right} {Top} {Bottom}";
        }
    }
}
