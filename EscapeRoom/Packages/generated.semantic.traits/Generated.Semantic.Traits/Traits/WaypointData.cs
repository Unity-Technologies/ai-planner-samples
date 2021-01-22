using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct WaypointData : ITraitData, IEquatable<WaypointData>
    {
        public Unity.Entities.Entity Left;
        public Unity.Entities.Entity Right;
        public Unity.Entities.Entity Up;
        public Unity.Entities.Entity Down;
        public System.Boolean Occupied;
        public System.Int32 StepsToEnd;

        public bool Equals(WaypointData other)
        {
            return Left.Equals(other.Left) && Right.Equals(other.Right) && Up.Equals(other.Up) && Down.Equals(other.Down) && Occupied.Equals(other.Occupied) && StepsToEnd.Equals(other.StepsToEnd);
        }

        public override string ToString()
        {
            return $"Waypoint: {Left} {Right} {Up} {Down} {Occupied} {StepsToEnd}";
        }
    }
}
