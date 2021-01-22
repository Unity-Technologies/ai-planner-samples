using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct MoveableData : ITraitData, IEquatable<MoveableData>
    {

        public bool Equals(MoveableData other)
        {
            return true;
        }

        public override string ToString()
        {
            return $"Moveable";
        }
    }
}
