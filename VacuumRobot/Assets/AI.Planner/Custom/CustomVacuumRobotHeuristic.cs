using Unity.AI.Planner;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using Unity.Collections;
using Unity.Mathematics;
#if PLANNER_DOMAIN_GENERATED
using AI.Planner.Domains;
#endif

namespace AI.Planner.Actions.Clean
{
#if PLANNER_DOMAIN_GENERATED
    public struct CustomVacuumRobotHeuristic : ICustomHeuristic<StateData>
    {
        public BoundedValue Evaluate(StateData stateData)
        {
            var dirtIndices = new NativeList<int>(stateData.TraitBasedObjects.Length, Allocator.Temp);
            stateData.GetTraitBasedObjectIndices(dirtIndices, typeof(Dirt), typeof(Location));
            
            if (dirtIndices.Length == 0) // no dirt remaining
                return new BoundedValue(0,0,0);

            var numberOfDirtPiles = dirtIndices.Length;
            float maxDistance = float.MinValue;
            float minDistance = float.MaxValue;
            float totalDistances = 0;
            for (int i = 0; i < numberOfDirtPiles; i++)
            {
                var dirtPosition = stateData.GetTraitOnObjectAtIndex<Location>(dirtIndices[i]).Position;
                for (int j = i + 1; j < numberOfDirtPiles; j++)
                {
                    var otherDirtPosition = stateData.GetTraitOnObjectAtIndex<Location>(dirtIndices[j]).Position;
                    
                    var distance = (dirtPosition - otherDirtPosition).magnitude;
                    
                    totalDistances += distance;
                    maxDistance = math.max(maxDistance, distance);
                    minDistance = math.min(minDistance, distance);
                }
            }
            
            float collectAllReward = numberOfDirtPiles * 10; // reward for collecting dirt
            float bestCaseDistances = (numberOfDirtPiles - 1) * minDistance; // can also assume robot is already at a dirt pile
            float worstCaseDistances = numberOfDirtPiles * maxDistance; 
            float avgCaseDistances = numberOfDirtPiles * totalDistances / (numberOfDirtPiles * (numberOfDirtPiles - 1.0f) / 2.0f );
            
            dirtIndices.Dispose();
          
            return new BoundedValue(collectAllReward - worstCaseDistances, collectAllReward - avgCaseDistances, collectAllReward + bestCaseDistances);
        }
    }
#endif
}
