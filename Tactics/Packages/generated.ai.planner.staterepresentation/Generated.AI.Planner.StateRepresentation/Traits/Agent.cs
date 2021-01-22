using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.Traits;
using Generated.Semantic.Traits.Enums;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct Agent : ITrait, IBufferElementData, IEquatable<Agent>
    {
        public const string FieldTimeline = "Timeline";
        public const string FieldSafe = "Safe";
        public const string FieldHasWeapon = "HasWeapon";
        public System.Int32 Timeline;
        public System.Boolean Safe;
        public System.Boolean HasWeapon;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(Timeline):
                    Timeline = (System.Int32)value;
                    break;
                case nameof(Safe):
                    Safe = (System.Boolean)value;
                    break;
                case nameof(HasWeapon):
                    HasWeapon = (System.Boolean)value;
                    break;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Agent.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(Timeline):
                    return Timeline;
                case nameof(Safe):
                    return Safe;
                case nameof(HasWeapon):
                    return HasWeapon;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Agent.");
            }
        }

        public bool Equals(Agent other)
        {
            return Timeline == other.Timeline && Safe == other.Safe && HasWeapon == other.HasWeapon;
        }

        public override string ToString()
        {
            return $"Agent\n  Timeline: {Timeline}\n  Safe: {Safe}\n  HasWeapon: {HasWeapon}";
        }
    }
}
