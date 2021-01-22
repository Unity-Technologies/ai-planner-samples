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
    [AddComponentMenu("Semantic/Traits/Cover (Trait)")]
    [RequireComponent(typeof(SemanticObject))]
    public partial class Cover : MonoBehaviour, ITrait
    {
        public System.Boolean SpotTaken
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<CoverData>(m_Entity))
                {
                    m_p0 = m_EntityManager.GetComponentData<CoverData>(m_Entity).SpotTaken;
                }

                return m_p0;
            }
            set
            {
                CoverData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<CoverData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<CoverData>(m_Entity);
                data.SpotTaken = m_p0 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public Generated.Semantic.Traits.Enums.Direction Direction
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<CoverData>(m_Entity))
                {
                    m_p2 = m_EntityManager.GetComponentData<CoverData>(m_Entity).Direction;
                }

                return m_p2;
            }
            set
            {
                CoverData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<CoverData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<CoverData>(m_Entity);
                data.Direction = m_p2 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public CoverData Data
        {
            get => m_EntityManager != default && m_EntityManager.HasComponent<CoverData>(m_Entity) ?
                m_EntityManager.GetComponentData<CoverData>(m_Entity) : GetData();
            set
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<CoverData>(m_Entity))
                    m_EntityManager.SetComponentData(m_Entity, value);
            }
        }

        #pragma warning disable 649
        [SerializeField]
        [InspectorName("SpotTaken")]
        System.Boolean m_p0 = false;
        [SerializeField]
        [InspectorName("Direction")]
        Generated.Semantic.Traits.Enums.Direction m_p2 = (Generated.Semantic.Traits.Enums.Direction)0;
        #pragma warning restore 649

        EntityManager m_EntityManager;
        World m_World;
        Entity m_Entity;

        CoverData GetData()
        {
            CoverData data = default;
            data.SpotTaken = m_p0;
            data.Direction = m_p2;

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

            if (!destinationManager.HasComponent(entity, typeof(CoverData)))
            {
                destinationManager.AddComponentData(entity, GetData());
            }
        }

        void OnDestroy()
        {
            if (m_World != default && m_World.IsCreated)
            {
                m_EntityManager.RemoveComponent<CoverData>(m_Entity);
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
            TraitGizmos.DrawGizmoForTrait(nameof(CoverData), gameObject,Data);
        }
#endif
    }
}
