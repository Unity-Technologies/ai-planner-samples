using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct RobotData : ITraitData, IEquatable<RobotData>
    {

        public bool Equals(RobotData other)
        {
            return true;
        }

        public override string ToString()
        {
            return $"Robot";
        }
    }
}
