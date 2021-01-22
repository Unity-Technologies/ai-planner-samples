using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct KeyLockData : ITraitData, IEquatable<KeyLockData>
    {

        public bool Equals(KeyLockData other)
        {
            return true;
        }

        public override string ToString()
        {
            return $"KeyLock";
        }
    }
}
