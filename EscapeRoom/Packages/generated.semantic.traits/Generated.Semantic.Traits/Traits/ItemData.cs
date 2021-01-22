using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct ItemData : ITraitData, IEquatable<ItemData>
    {
        public Generated.Semantic.Traits.Enums.ItemType Type;

        public bool Equals(ItemData other)
        {
            return Type.Equals(other.Type);
        }

        public override string ToString()
        {
            return $"Item: {Type}";
        }
    }
}
