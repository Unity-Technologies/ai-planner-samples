using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OpenBy2Switch : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField]
	List<SwitchTrigger> m_switchA;
	
	[SerializeField]
	List<SwitchTrigger> m_switchB;

	[SerializeField]
	GameObject m_NotificationA;
	
	[SerializeField]
	GameObject m_NotificationB;
	
	[SerializeField]
	GameObject m_GateDoor;
	
	[SerializeField]
	float m_GateDoorOpenPosition;
#pragma warning restore 0649
	
	public void Update()
	{
		bool aActivated = m_switchA.Any(s => s.Activated);
		m_NotificationA.SetActive(aActivated);
		
		bool bActivated = m_switchB.Any(s => s.Activated);
		m_NotificationB.SetActive(bActivated);

		if (aActivated && bActivated)
		{
			m_GateDoor.transform.localPosition = Vector3.Lerp(m_GateDoor.transform.localPosition, new Vector3(0,m_GateDoorOpenPosition, 0), 0.1f);
		}
		else
		{
			m_GateDoor.transform.localPosition = Vector3.zero;
		}
	}
}
