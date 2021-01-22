using System;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.Match3Plan;
using Unity.AI.Planner.Traits;

namespace AI.Planner.Custom.Match3Plan
{
    public struct CustomSwapReward : ICustomActionReward<StateData>
    {
        public const int GoalReward = 8;
        public const int BasicReward = 1;

        public float RewardModifier(StateData originalState, ActionKey action, StateData newState)
        {
            var oldScore = originalState.GetTraitOnObjectAtIndex<Game>(action[Match3Utility.GameIndex]).Score;
            var newScore = newState.GetTraitOnObjectAtIndex<Game>(action[Match3Utility.GameIndex]).Score;
            var gain = newScore - oldScore;

            return gain > 0 ? gain : -1;
        }
    }
}
