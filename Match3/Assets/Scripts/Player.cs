using System;
using System.Collections;
using UnityEngine;

namespace Match3
{
    public enum ControllerType
    {
        Player = 0,
        Planner
    }
    
    [Serializable]
    public class Player : MonoBehaviour
    {
        [SerializeField]
        ControllerType m_controller = ControllerType.Player;
        
        GemObject m_SelectedGem;
        
        public Match3Grid Grid { get; set; }
        
        protected void Update()
        {
            if ( Grid.Goals[0].GemCount <= 0)
                return;
            
            if (m_controller == ControllerType.Player)
            {
                if (m_SelectedGem != null)
                {
                    if (Input.GetMouseButton(0))
                    {
                        m_SelectedGem.Highlight(true);
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        var mousePosition = Input.mousePosition;
                        mousePosition.z = Camera.main.transform.position.y;
                        var targetPos = Camera.main.ScreenToWorldPoint(mousePosition);
                        Grid.TryToSwapSelectedCell(m_SelectedGem, targetPos);
                        m_SelectedGem = null;
                    }
                }
		
                Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {	
                    var hitGameObject = hit.collider.gameObject;
                    GemObject gem = hitGameObject.GetComponent<GemObject>();

                    if (Input.GetMouseButton(0))
                    {
                        if (m_SelectedGem == null && !gem.Destroyed)
                        {
                            m_SelectedGem = gem;
                            var gemPos = m_SelectedGem.transform.position;
                            m_SelectedGem.transform.position = new Vector3(gemPos.x, 0.25f, gemPos.z);
                        }
                    }
                    else
                    {
                        if (m_SelectedGem != null)
                        {
                            Grid.ResetGemPosition(m_SelectedGem);
                        }
				
                        m_SelectedGem = null;
                        gem.Highlight(true);
                    }
                }
            }
        }
        
#if PLANNER_DOMAIN_GENERATED
        // Used implicitly via Decision Controller
        public IEnumerator SwapCells(AI.Planner.Domains.Coordinate cell1, AI.Planner.Domains.Coordinate cell2)
        {
            Grid.SwapGems((int)cell1.X, (int)cell1.Y, (int)cell2.X, (int)cell2.Y);

            while (!Grid.ReadyToAct())
            {
                yield return null;
            }
        }
#endif
    }
}
