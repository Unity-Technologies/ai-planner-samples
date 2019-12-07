using Unity.AI.Planner;
using Unity.Collections;
using UnityEngine;

#if PLANNER_DOMAIN_GENERATED
using AI.Planner.Domains;

namespace AI.Planner.Custom.Escape
{
    public struct HeuristicExploration : ICustomHeuristic<StateData>
    {
        public BoundedValue Evaluate(StateData stateData)
        {
            float estimatedValue = 0;
            
            var waypointIndices = new NativeList<int>(stateData.TraitBasedObjects.Length, Allocator.Temp);
            stateData.GetTraitBasedObjectIndices(waypointIndices, typeof(Waypoint));
            
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
#endif