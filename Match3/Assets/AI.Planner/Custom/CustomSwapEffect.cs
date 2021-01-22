using System;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.Match3Plan;
using UnityEngine;
using Unity.AI.Planner.Traits;

namespace AI.Planner.Custom.Match3Plan
{
    public struct CustomSwapEffect : ICustomActionEffect<StateData>
    {
        public void ApplyCustomActionEffectsToState(StateData originalState, ActionKey action, StateData newState)
        {
            var cell1 = newState.GetTraitOnObjectAtIndex<Cell>(action[Match3Utility.Cell1Index]);
            var cell2 = newState.GetTraitOnObjectAtIndex<Cell>(action[Match3Utility.Cell2Index]);

            Match3Utility.SwapCellAndUpdateBoard(action, newState, cell1, cell2);
        }
    }
}
