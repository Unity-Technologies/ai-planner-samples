using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct ActivationLockData : ITraitData, IEquatable<ActivationLockData>
    {
        public Generated.Semantic.Traits.Enums.ActivationType ActivationA;
        public Generated.Semantic.Traits.Enums.ActivationType ActivationB;

        public bool Equals(ActivationLockData other)
        {
            return ActivationA.Equals(other.ActivationA) && ActivationB.Equals(other.ActivationB);
        }

        public override string ToString()
        {
            return $"ActivationLock: {ActivationA} {ActivationB}";
        }
    }
}
