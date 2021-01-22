using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct AgentData : ITraitData, IEquatable<AgentData>
    {
        public System.Int32 Timeline;
        public System.Boolean Safe;
        public System.Boolean HasWeapon;

        public bool Equals(AgentData other)
        {
            return Timeline.Equals(other.Timeline) && Safe.Equals(other.Safe) && HasWeapon.Equals(other.HasWeapon);
        }

        public override string ToString()
        {
            return $"Agent: {Timeline} {Safe} {HasWeapon}";
        }
    }
}
