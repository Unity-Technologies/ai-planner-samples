using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct GameData : ITraitData, IEquatable<GameData>
    {
        public System.Int32 MoveCount;
        public System.Int32 Score;
        public Generated.Semantic.Traits.Enums.CellType GoalType;
        public System.Int32 GoalCount;

        public bool Equals(GameData other)
        {
            return MoveCount.Equals(other.MoveCount) && Score.Equals(other.Score) && GoalType.Equals(other.GoalType) && GoalCount.Equals(other.GoalCount);
        }

        public override string ToString()
        {
            return $"Game: {MoveCount} {Score} {GoalType} {GoalCount}";
        }
    }
}
