using System;
using System.Collections;
using UnityEngine;
#if PLANNER_STATEREPRESENTATION_GENERATED
using Generated.AI.Planner.StateRepresentation;
#endif

namespace Match3
{
    [Serializable]
    public class Player : MonoBehaviour
    {
        public Match3Grid Grid { get; set; }
  
#if PLANNER_STATEREPRESENTATION_GENERATED
        // Used implicitly via Decision Controller
        public IEnumerator SwapCells(Coordinate cell1, Coordinate cell2)
        {
            Grid.SwapGems((int)cell1.X, (int)cell1.Y, (int)cell2.X, (int)cell2.Y);

            while (!Grid.ReadyToPlay())
            {
                yield return null;
            }
        }
#endif
    }
}
