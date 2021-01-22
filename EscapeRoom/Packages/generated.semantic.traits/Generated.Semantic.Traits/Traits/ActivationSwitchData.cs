using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct ActivationSwitchData : ITraitData, IEquatable<ActivationSwitchData>
    {
        public Generated.Semantic.Traits.Enums.ActivationType Type;

        public bool Equals(ActivationSwitchData other)
        {
            return Type.Equals(other.Type);
        }

        public override string ToString()
        {
            return $"ActivationSwitch: {Type}";
        }
    }
}
