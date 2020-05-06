using System;
using UnityEngine;

public class SwitchTrigger : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Transform m_SwitchVisual;

    [SerializeField]
    float m_ActivatedHeight;
#pragma warning restore 0649

    float m_DefaultHeight;

	public bool Activated { get; set; }

    public void Start()
    {
        m_DefaultHeight = m_SwitchVisual.localPosition.y;
    }

    void OnTriggerEnter(Collider other)
	{
		Activated = true;
        
        m_SwitchVisual.localPosition = new Vector3(m_SwitchVisual.localPosition.x, m_ActivatedHeight, m_SwitchVisual.localPosition.z);
	}
    
	void OnTriggerExit(Collider other)
	{
		Activated = false;
        
        m_SwitchVisual.localPosition = new Vector3(m_SwitchVisual.localPosition.x, m_DefaultHeight, m_SwitchVisual.localPosition.z);
	}
}
