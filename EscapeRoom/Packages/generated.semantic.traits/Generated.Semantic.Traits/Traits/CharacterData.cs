using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct CharacterData : ITraitData, IEquatable<CharacterData>
    {
        public Unity.Entities.Entity Waypoint;
        public System.Int32 ID;

        public bool Equals(CharacterData other)
        {
            return Waypoint.Equals(other.Waypoint) && ID.Equals(other.ID);
        }

        public override string ToString()
        {
            return $"Character: {Waypoint} {ID}";
        }
    }
}
