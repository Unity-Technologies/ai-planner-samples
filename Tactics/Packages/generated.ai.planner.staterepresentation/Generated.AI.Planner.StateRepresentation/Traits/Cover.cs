using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.Traits;
using Generated.Semantic.Traits.Enums;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct Cover : ITrait, IBufferElementData, IEquatable<Cover>
    {
        public const string FieldSpotTaken = "SpotTaken";
        public const string FieldDirection = "Direction";
        public System.Boolean SpotTaken;
        public Generated.Semantic.Traits.Enums.Direction Direction;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(SpotTaken):
                    SpotTaken = (System.Boolean)value;
                    break;
                case nameof(Direction):
                    Direction = (Generated.Semantic.Traits.Enums.Direction)Enum.ToObject(typeof(Generated.Semantic.Traits.Enums.Direction), value);
                    break;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Cover.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(SpotTaken):
                    return SpotTaken;
                case nameof(Direction):
                    return Direction;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Cover.");
            }
        }

        public bool Equals(Cover other)
        {
            return SpotTaken == other.SpotTaken && Direction == other.Direction;
        }

        public override string ToString()
        {
            return $"Cover\n  SpotTaken: {SpotTaken}\n  Direction: {Direction}";
        }
    }
}
