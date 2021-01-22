using System;
using System.Collections;
using Generated.Semantic.Traits;
using UnityEngine;

namespace Match3
{
    [Serializable]
    public class Player : MonoBehaviour
    {
        public Match3Grid Grid { get; set; }

        // Used implicitly via Decision Controller
        public IEnumerator SwapCells(GameObject cell1, GameObject cell2)
        {
            var coords1 = cell1.GetComponent<Coordinate>();
            var coords2 = cell2.GetComponent<Coordinate>();

            Grid.SwapGems((int)coords1.X, (int)coords1.Y, (int)coords2.X, (int)coords2.Y);

            while (!Grid.ReadyToPlay())
            {
                yield return null;
            }
        }
    }
}
