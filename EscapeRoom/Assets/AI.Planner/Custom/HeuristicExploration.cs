using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.Escape;
using Unity.AI.Planner;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace AI.Planner.Custom.Escape
{
    public struct HeuristicExploration : ICustomCumulativeRewardEstimator<StateData>
    {
        public BoundedValue Evaluate(StateData stateData)
        {
            float estimatedValue = 50f; // the reward for reaching the goal

            var characterIndices = new NativeList<int>(3, Allocator.Temp);

            var characterFilter = new NativeArray<ComponentType>(1, Allocator.Temp){ [0] = ComponentType.ReadWrite<Character>()};
            stateData.GetTraitBasedObjectIndices(characterIndices, characterFilter);
            characterFilter.Dispose();

            for (int i = 0; i < characterIndices.Length; i++)
            {
                var character = stateData.GetTraitOnObjectAtIndex<Character>(characterIndices[i]);
                var waypoint = stateData.GetTraitOnObject<Waypoint>(stateData.GetTraitBasedObject(character.Waypoint));
                estimatedValue += -0.1f * waypoint.StepsToEnd; // move cost for each step
            }


            characterIndices.Dispose();

            return new BoundedValue(estimatedValue - 100, estimatedValue - 5, estimatedValue);
        }
    }
}
