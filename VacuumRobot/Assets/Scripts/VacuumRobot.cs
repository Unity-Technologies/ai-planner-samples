using System.Collections;
using Unity.AI.Planner;
using UnityEngine;
using UnityEngine.AI.Planner.Controller;
using UnityEngine.AI.Planner.DomainLanguage.TraitBased;
#if PLANNER_STATEREPRESENTATION_GENERATED
using Generated.AI.Planner.StateRepresentation;
#endif

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

        DecisionController m_Controller;
#pragma warning restore 0649

        GameObject m_Target;

        Coroutine m_NavigateTo;
        bool m_UpdateStateWithWorldQuery;

        public IEnumerator NavigateTo(GameObject target)
        {
            m_Target = target;

            while (m_Target != null && !IsTargetReachable())
            {
                transform.position = Vector3.Lerp(transform.position, m_Target.transform.position, 0.1f);
                transform.LookAt(m_Target.transform.position);
                yield return null;
            }

            if (m_Target != null)
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
            m_Controller = GetComponent<DecisionController>();
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
                            StartCoroutine(Collect(dirt));
                        else
                            m_NavigateTo = StartCoroutine(NavigateTo(dirt));
                    }
                }
            }
            else
            {
                m_Controller.AutoUpdate = true;
                if (m_UpdateStateWithWorldQuery && m_Controller.PlanExecutionStatus != PlanExecutionStatus.ExecutingAction)
                {
                    m_Controller.UpdateStateWithWorldQuery();
                    m_UpdateStateWithWorldQuery = false;
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
#if PLANNER_STATEREPRESENTATION_GENERATED
            var traitComponent = other.gameObject.GetComponent<TraitComponent>();
            if (traitComponent && traitComponent.HasTraitData<Dirt>())
                m_UpdateStateWithWorldQuery = true;
#endif
        }
    }
}
