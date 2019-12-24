#if PLANNER_DOMAINS_GENERATED
using AI.Planner.Domains;
#endif
using System;
using UnityEngine;
using Unity.AI.Planner.DomainLanguage.TraitBased;

namespace AI.Planner.Actions.Match3Plan
{
#if PLANNER_DOMAINS_GENERATED
    public struct CustomSwapEffect : ICustomActionEffect<StateData>
    {
        public void ApplyCustomActionEffectsToState(StateData originalState, ActionKey action, StateData newState)
        {
            var id1 = newState.GetTraitBasedObjectId(action[Match3Utility.Cell1Index]);
            var obj1 = newState.GetTraitBasedObject(id1);
            var cell1 = newState.GetTraitOnObject<Cell>(obj1);

            var id2 = newState.GetTraitBasedObjectId(action[Match3Utility.Cell2Index]);
            var obj2 = newState.GetTraitBasedObject(id2);
            var cell2 = newState.GetTraitOnObject<Cell>(obj2);

            Match3Utility.SwapCellAndUpdateBoard(action, newState, cell1, cell2);
        }
    }
#endif
}
