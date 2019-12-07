using System;
using System.Collections;
#if PLANNER_DOMAIN_GENERATED
using AI.Planner.Domains;
#endif
using Unity.AI.Planner;
using Unity.AI.Planner.Controller;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace VacuumGame
{
    enum ControllerType
    {
        Random,
        Planner
    }
    
    public class VacuumRobot : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField]
        ControllerType m_ControlledBy;
#pragma warning restore 0649

        GameObject m_Target;
        IDecisionController m_PlannerController;
        Coroutine m_NavigateTo;

        public IEnumerator NavigateTo(GameObject target)
        {
            m_Target = target;

            while (m_Target != null && !IsTargetReachable())
            {
                transform.position = Vector3.Lerp(transform.position, m_Target.transform.position, 0.1f);
                transform.LookAt(m_Target.transform.position);
                yield return null;
            }
            transform.position = m_Target.transform.position;

            m_NavigateTo = null;
        }

        public IEnumerator Collect(GameObject dirt)
        {
            DestroyImmediate(dirt);
            
            yield return new WaitForSeconds(0.6f);
        }

        void Start()
        {
            m_PlannerController = GetComponent<IDecisionController>();
        }

        void Update()
        {
            UpdateControl();
        }

        bool IsTargetReachable()
        {
            return Vector3.Distance(transform.position, m_Target.transform.position) < 0.1f;
        }

        void UpdateControl()
        {
            if (m_ControlledBy == ControllerType.Random)
            {
                if (m_NavigateTo == default)
                {
                    var dirt = GameObject.FindWithTag("Dirt");

                    if (dirt != null)
                    {
                        m_Target = dirt;
                        
                        if (IsTargetReachable())
                        {
                            Collect(dirt);
                        }
                        else
                        {
                            m_NavigateTo = StartCoroutine(NavigateTo(dirt));
                        }
                    }
                }
            }
            else
            {
                m_PlannerController.AutoUpdate = true;
            }
        }
    }
}
