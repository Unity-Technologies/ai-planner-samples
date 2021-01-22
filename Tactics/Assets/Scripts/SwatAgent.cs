using System;
using System.Collections.Generic;
using Generated.Semantic.Traits;
using UnityEngine;
using UnityEngine.AI;

public class SwatAgent : MonoBehaviour
{
    static readonly int k_Moving = Animator.StringToHash("Moving");
    static readonly int k_Weapon = Animator.StringToHash("Weapon");

    List<Transform> m_Targets = new List<Transform>();

    [SerializeField]
    Animator m_Animator = default;

    [SerializeField]
    SkinnedMeshRenderer m_Mesh = default;
    
    [SerializeField]
    GameObject m_WeaponContainer = default;
    
    [SerializeField]
    Material m_DefaultMaterial;

    NavMeshAgent m_NavMeshAgent = default;

    public void Start()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();

        var agent = GetComponent<Agent>();
        SetWeapon(agent.HasWeapon);
    }

    public void GoTo(GameObject target)
    {
        m_Targets.Add(target.transform);
    }

    public void Update()
    {
        if (m_Targets.Count == 0)
            return;

        var currentTarget = m_Targets[0];

        if (currentTarget == null || Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            if (currentTarget != null)
                transform.forward = currentTarget.forward;

            m_Animator.SetBool(k_Moving, false);
            m_Targets.RemoveAt(0);
        }
        else if (m_NavMeshAgent.destination != currentTarget.transform.position)
        {
            m_Animator.SetBool(k_Moving, true);
            m_NavMeshAgent.destination = currentTarget.transform.position;
        }
    }

    public void SetWeapon(bool value)
    {
        m_WeaponContainer.SetActive(value);
        m_Animator.SetBool(k_Weapon, value);

        if (value)
        {
            // Reset to default material
            m_Mesh.sharedMaterial = m_DefaultMaterial;    
        }
    }
}
