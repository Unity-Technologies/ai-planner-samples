using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.Escape;
using Unity.AI.Planner;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace AI.Planner.Custom.Escape
{
    public struct HeuristicExploration : ICustomHeuristic<StateData>
    {
        public BoundedValue Evaluate(StateData stateData)
        {
            float estimatedValue = 0;
            
            var waypointIndices = new NativeList<int>(stateData.TraitBasedObjects.Length, Allocator.Temp);
            
            var waypointFilter = new NativeArray<ComponentType>(1, Allocator.Temp){ [0] = ComponentType.ReadWrite<Waypoint>()};
            stateData.GetTraitBasedObjectIndices(waypointIndices, waypointFilter);
            waypointFilter.Dispose();
            
            for (int i = 0; i < waypointIndices.Length; i++)
            {
                var waypoint = stateData.GetTraitOnObjectAtIndex<Waypoint>(waypointIndices[i]);
                if (waypoint.Visited > 0)
                    estimatedValue += Mathf.Max(0, 5 - waypoint.Visited) * 0.1f; // Increase curiosity by encouraging visiting less visited area first
            }
            
            waypointIndices.Dispose();

            return new BoundedValue(-100 - estimatedValue, estimatedValue, 100 + estimatedValue);
        }
    }
}