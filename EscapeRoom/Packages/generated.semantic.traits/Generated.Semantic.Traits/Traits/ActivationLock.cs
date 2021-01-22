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
    [AddComponentMenu("Semantic/Traits/ActivationLock (Trait)")]
    [RequireComponent(typeof(SemanticObject))]
    public partial class ActivationLock : MonoBehaviour, ITrait
    {
        public Generated.Semantic.Traits.Enums.ActivationType ActivationA
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<ActivationLockData>(m_Entity))
                {
                    m_p0 = m_EntityManager.GetComponentData<ActivationLockData>(m_Entity).ActivationA;
                }

                return m_p0;
            }
            set
            {
                ActivationLockData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<ActivationLockData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<ActivationLockData>(m_Entity);
                data.ActivationA = m_p0 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public Generated.Semantic.Traits.Enums.ActivationType ActivationB
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<ActivationLockData>(m_Entity))
                {
                    m_p1 = m_EntityManager.GetComponentData<ActivationLockData>(m_Entity).ActivationB;
                }

                return m_p1;
            }
            set
            {
                ActivationLockData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<ActivationLockData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<ActivationLockData>(m_Entity);
                data.ActivationB = m_p1 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public ActivationLockData Data
        {
            get => m_EntityManager != default && m_EntityManager.HasComponent<ActivationLockData>(m_Entity) ?
                m_EntityManager.GetComponentData<ActivationLockData>(m_Entity) : GetData();
            set
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<ActivationLockData>(m_Entity))
                    m_EntityManager.SetComponentData(m_Entity, value);
            }
        }

        #pragma warning disable 649
        [SerializeField]
        [InspectorName("ActivationA")]
        Generated.Semantic.Traits.Enums.ActivationType m_p0 = (Generated.Semantic.Traits.Enums.ActivationType)0;
        [SerializeField]
        [InspectorName("ActivationB")]
        Generated.Semantic.Traits.Enums.ActivationType m_p1 = (Generated.Semantic.Traits.Enums.ActivationType)1;
        #pragma warning restore 649

        EntityManager m_EntityManager;
        World m_World;
        Entity m_Entity;

        ActivationLockData GetData()
        {
            ActivationLockData data = default;
            data.ActivationA = m_p0;
            data.ActivationB = m_p1;

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

            if (!destinationManager.HasComponent(entity, typeof(ActivationLockData)))
            {
                destinationManager.AddComponentData(entity, GetData());
            }
        }

        void OnDestroy()
        {
            if (m_World != default && m_World.IsCreated)
            {
                m_EntityManager.RemoveComponent<ActivationLockData>(m_Entity);
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
            TraitGizmos.DrawGizmoForTrait(nameof(ActivationLockData), gameObject,Data);
        }
#endif
    }
}
