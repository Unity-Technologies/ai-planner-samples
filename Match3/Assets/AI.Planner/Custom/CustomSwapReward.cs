using System;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.Match3Plan;
using Unity.AI.Planner.DomainLanguage.TraitBased;

namespace AI.Planner.Custom.Match3Plan
{
    public struct CustomSwapReward : ICustomActionReward<StateData>
    {
        public const int GoalReward = 8;
        public const int BasicReward = 1;

        public float RewardModifier(StateData originalState, ActionKey action, StateData newState)
        {
            var gameId = newState.GetTraitBasedObjectId(action[Match3Utility.GameIndex]);
            var game = newState.GetTraitBasedObject(gameId);
            var gameTrait = newState.GetTraitOnObject<Game>(game);

            if (gameTrait.Score > 0)
                return gameTrait.Score;

            return -1;
        }
    }
}
