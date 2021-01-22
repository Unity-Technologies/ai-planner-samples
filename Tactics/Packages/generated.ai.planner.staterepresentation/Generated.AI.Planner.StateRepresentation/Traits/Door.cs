using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.Traits;
using Generated.Semantic.Traits.Enums;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct Door : ITrait, IBufferElementData, IEquatable<Door>
    {
        public const string FieldOpen = "Open";
        public System.Boolean Open;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(Open):
                    Open = (System.Boolean)value;
                    break;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Door.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(Open):
                    return Open;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Door.");
            }
        }

        public bool Equals(Door other)
        {
            return Open == other.Open;
        }

        public override string ToString()
        {
            return $"Door\n  Open: {Open}";
        }
    }
}
