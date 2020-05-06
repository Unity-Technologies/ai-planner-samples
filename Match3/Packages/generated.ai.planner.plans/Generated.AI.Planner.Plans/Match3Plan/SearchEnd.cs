using Unity.AI.Planner;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.Match3Plan;
using Generated.AI.Planner.StateRepresentation.Enums;

namespace Generated.AI.Planner.Plans.Match3Plan
{
    public struct SearchEnd
    {
        public bool IsTerminal(StateData stateData)
        {
            var GameFilter = new NativeArray<ComponentType>(1, Allocator.Temp){[0] = ComponentType.ReadWrite<Game>(),  };
            var GameObjectIndices = new NativeList<int>(2, Allocator.Temp);
            stateData.GetTraitBasedObjectIndices(GameObjectIndices, GameFilter);
            var GameBuffer = stateData.GameBuffer;
            for (int i0 = 0; i0 < GameObjectIndices.Length; i0++)
            {
                var GameIndex = GameObjectIndices[i0];
                var GameObject = stateData.TraitBasedObjects[GameIndex];
            
                
                if (!(GameBuffer[GameObject.GameIndex].MoveCount > 10))
                    continue;
                GameObjectIndices.Dispose();
                GameFilter.Dispose();
                return true;
            }
            GameObjectIndices.Dispose();
            GameFilter.Dispose();

            return false;
        }

        public float TerminalReward(StateData stateData)
        {
            var reward = 0f;

            return reward;
        }
    }
}
