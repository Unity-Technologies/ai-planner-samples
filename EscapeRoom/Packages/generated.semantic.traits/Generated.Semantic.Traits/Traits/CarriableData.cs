using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct CarriableData : ITraitData, IEquatable<CarriableData>
    {
        public Unity.Entities.Entity CarriedBy;

        public bool Equals(CarriableData other)
        {
            return CarriedBy.Equals(other.CarriedBy);
        }

        public override string ToString()
        {
            return $"Carriable: {CarriedBy}";
        }
    }
}
