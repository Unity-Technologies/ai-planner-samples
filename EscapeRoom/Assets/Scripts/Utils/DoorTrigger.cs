using System;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    bool m_Open;

    public void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, (m_Open)?90:0, 0), 0.1f);
    }

    void OnTriggerEnter(Collider other)
    {
        m_Open = true;
    }
    
    void OnTriggerExit(Collider other)
    {
        m_Open = false;
    }

}
