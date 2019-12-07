using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTrigger : MonoBehaviour
{
	bool m_Activated;

	public bool Activated
	{
		get { return m_Activated; }
		set { m_Activated = value; }
	}

	void OnTriggerEnter(Collider other)
	{
		m_Activated = true;
	}
    
	void OnTriggerExit(Collider other)
	{
		m_Activated = false;
	}
}
