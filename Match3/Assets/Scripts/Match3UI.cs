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
    
    [SerializeField]
    GameObject m_SuccessPanel;
#pragma warning restore 0649

    public void Awake()
    {
        m_SuccessPanel.SetActive(false);
        
        m_Grid.MoveCountChanged += UpdateMoveCount;
        m_Grid.GoalCountChanged += UpdateGoal;
    }
    
    public void OnDestroy()
    {
        m_Grid.MoveCountChanged -= UpdateMoveCount;
        m_Grid.GoalCountChanged -= UpdateGoal;
    }

    void UpdateMoveCount(int moveCount)
    {
        m_UIText.text = moveCount.ToString();
    }
    
    void UpdateGoal(int remainingGoal)
    {
        m_UITextGoal.text = remainingGoal.ToString();
        
        if (remainingGoal == 0)
            m_SuccessPanel.SetActive(true);
    }
}
