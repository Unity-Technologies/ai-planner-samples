using System;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.CoverTactic;
using Unity.AI.Planner.Traits;
using Unity.Burst;
using UnityEngine;
using Location = Unity.AI.Planner.Traits.Location;

namespace AI.Tactics
{
    [BurstCompile]
    public struct PlayOrderComparer : IParameterComparer<StateData>
    {
        public StateData StateData { get; set; }

        public int Compare(int a, int b)
        {
            Agent agentA = StateData.GetTraitOnObjectAtIndex<Agent>(a);
            Agent agentB = StateData.GetTraitOnObjectAtIndex<Agent>(b);

            return agentA.Timeline.CompareTo(agentB.Timeline);
        }
    }

    public struct ClosestLocation : IParameterComparerWithReference<StateData, Location>
    {
        public StateData StateData { get; set; }
        public Location ReferenceTrait { get; set; }

        public int Compare(int a, int b)
        {
            Location locationA = StateData.GetTraitOnObjectAtIndex<Location>(a);
            Location locationB = StateData.GetTraitOnObjectAtIndex<Location>(b);

            return Vector3.Distance(locationB.Position, ReferenceTrait.Position).CompareTo(Vector3.Distance(locationA.Position, ReferenceTrait.Position));
        }
    }

}
