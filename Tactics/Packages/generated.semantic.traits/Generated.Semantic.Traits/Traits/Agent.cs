using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Entities;
using UnityEngine;

namespace Generated.Semantic.Traits
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [AddComponentMenu("Semantic/Traits/Agent (Trait)")]
    [RequireComponent(typeof(SemanticObject))]
    public partial class Agent : MonoBehaviour, ITrait
    {
        public System.Int32 Timeline
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<AgentData>(m_Entity))
                {
                    m_p0 = m_EntityManager.GetComponentData<AgentData>(m_Entity).Timeline;
                }

                return m_p0;
            }
            set
            {
                AgentData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<AgentData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<AgentData>(m_Entity);
                data.Timeline = m_p0 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public System.Boolean Safe
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<AgentData>(m_Entity))
                {
                    m_p1 = m_EntityManager.GetComponentData<AgentData>(m_Entity).Safe;
                }

                return m_p1;
            }
            set
            {
                AgentData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<AgentData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<AgentData>(m_Entity);
                data.Safe = m_p1 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public System.Boolean HasWeapon
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<AgentData>(m_Entity))
                {
                    m_p2 = m_EntityManager.GetComponentData<AgentData>(m_Entity).HasWeapon;
                }

                return m_p2;
            }
            set
            {
                AgentData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<AgentData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<AgentData>(m_Entity);
                data.HasWeapon = m_p2 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public AgentData Data
        {
            get => m_EntityManager != default && m_EntityManager.HasComponent<AgentData>(m_Entity) ?
                m_EntityManager.GetComponentData<AgentData>(m_Entity) : GetData();
            set
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<AgentData>(m_Entity))
                    m_EntityManager.SetComponentData(m_Entity, value);
            }
        }

        #pragma warning disable 649
        [SerializeField]
        [InspectorName("Timeline")]
        System.Int32 m_p0 = 0;
        [SerializeField]
        [InspectorName("Safe")]
        System.Boolean m_p1 = false;
        [SerializeField]
        [InspectorName("HasWeapon")]
        System.Boolean m_p2 = false;
        #pragma warning restore 649

        EntityManager m_EntityManager;
        World m_World;
        Entity m_Entity;

        AgentData GetData()
        {
            AgentData data = default;
            data.Timeline = m_p0;
            data.Safe = m_p1;
            data.HasWeapon = m_p2;

            return data;
        }

        
        void OnEnable()
        {
            // Handle the case where this trait is added after conversion
            var semanticObject = GetComponent<SemanticObject>();
            if (semanticObject && !semanticObject.Entity.Equals(default))
                Convert(semanticObject.Entity, semanticObject.EntityManager, null);
        }

        public void Convert(Entity entity, EntityManager destinationManager, GameObjectConversionSystem _)
        {
            m_Entity = entity;
            m_EntityManager = destinationManager;
            m_World = destinationManager.World;

            if (!destinationManager.HasComponent(entity, typeof(AgentData)))
            {
                destinationManager.AddComponentData(entity, GetData());
            }
        }

        void OnDestroy()
        {
            if (m_World != default && m_World.IsCreated)
            {
                m_EntityManager.RemoveComponent<AgentData>(m_Entity);
                if (m_EntityManager.GetComponentCount(m_Entity) == 0)
                    m_EntityManager.DestroyEntity(m_Entity);
            }
        }

        void OnValidate()
        {

            // Commit local fields to backing store
            Data = GetData();
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            TraitGizmos.DrawGizmoForTrait(nameof(AgentData), gameObject,Data);
        }
#endif
    }
}
