using System;
using UnityEngine;

public class SmartCover : MonoBehaviour
{
    [SerializeField]
    Vector3 m_ActivatePosition = default;
    
    [SerializeField]
    Vector3 m_ActivateRotation = default;

    bool m_Activate = false;

    public void OnTriggerEnter(Collider collider)
    {
        m_Activate = true;
    }

    public void Update()
    {
        if (!m_Activate)
            return;
        
        transform.position = Vector3.Lerp(transform.position, m_ActivatePosition, 0.3f);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(m_ActivateRotation), 0.3f);
    }
}

