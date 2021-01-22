using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct CarrierData : ITraitData, IEquatable<CarrierData>
    {
        public Unity.Entities.Entity Carried;

        public bool Equals(CarrierData other)
        {
            return Carried.Equals(other.Carried);
        }

        public override string ToString()
        {
            return $"Carrier: {Carried}";
        }
    }
}
