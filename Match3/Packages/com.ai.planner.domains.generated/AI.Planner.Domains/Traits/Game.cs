using System;
using Unity.Entities;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using AI.Planner.Domains.Enums;

namespace AI.Planner.Domains
{
    [Serializable]
    public struct Game : ITrait, IEquatable<Game>
    {
        public const string FieldMoveCount = "MoveCount";
        public const string FieldScore = "Score";
        public System.Int64 MoveCount;
        public System.Int64 Score;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(MoveCount):
                    MoveCount = (System.Int64)value;
                    break;
                case nameof(Score):
                    Score = (System.Int64)value;
                    break;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Game.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(MoveCount):
                    return MoveCount;
                case nameof(Score):
                    return Score;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Game.");
            }
        }

        public bool Equals(Game other)
        {
            return MoveCount == other.MoveCount && Score == other.Score;
        }

        public override string ToString()
        {
            return $"Game: {MoveCount} {Score}";
        }
    }
}
