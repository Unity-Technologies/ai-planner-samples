using UnityEngine;
using UnityEngine.UI;

public class Match3UI : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Text m_UIText;
	
    [SerializeField]
    Text m_UITextGoal;

    [SerializeField]
    Match3Grid m_Grid;
#pragma warning restore 0649

    public void Start()
    {
	    m_Grid.gameDataUpdated = UpdateUI;
    }
    
    void UpdateUI()
    {
	    m_UIText.text = m_Grid.MoveCount.ToString();
	    m_UITextGoal.text = m_Grid.Goals[0].GemCount.ToString();
    }

}
