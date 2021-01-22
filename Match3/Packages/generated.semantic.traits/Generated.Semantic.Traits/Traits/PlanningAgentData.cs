using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct PlanningAgentData : ITraitData, IEquatable<PlanningAgentData>
    {

        public bool Equals(PlanningAgentData other)
        {
            return true;
        }

        public override string ToString()
        {
            return $"PlanningAgent";
        }
    }
}
