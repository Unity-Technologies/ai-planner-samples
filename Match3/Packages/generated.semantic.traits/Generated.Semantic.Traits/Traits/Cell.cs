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
    [AddComponentMenu("Semantic/Traits/Cell (Trait)")]
    [RequireComponent(typeof(SemanticObject))]
    public partial class Cell : MonoBehaviour, ITrait
    {
        public Generated.Semantic.Traits.Enums.CellType Type
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<CellData>(m_Entity))
                {
                    m_p0 = m_EntityManager.GetComponentData<CellData>(m_Entity).Type;
                }

                return m_p0;
            }
            set
            {
                CellData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<CellData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<CellData>(m_Entity);
                data.Type = m_p0 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public UnityEngine.GameObject Left
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<CellData>(m_Entity))
                {
                    var data = m_EntityManager.GetComponentData<CellData>(m_Entity);
                    if (data.Left != default)
                        m_p1 = m_EntityManager.GetComponentObject<Transform>(data.Left).gameObject;
                }

                return m_p1;
            }
            set
            {
                CellData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<CellData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<CellData>(m_Entity);
                Entity entity = default;
                if (value != null)
                {
                    var semanticObject = value.GetComponent<SemanticObject>();
                    if (semanticObject)
                        entity = semanticObject.Entity;
                }
                m_p1 = value;
                data.Left = entity;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public UnityEngine.GameObject Right
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<CellData>(m_Entity))
                {
                    var data = m_EntityManager.GetComponentData<CellData>(m_Entity);
                    if (data.Right != default)
                        m_p2 = m_EntityManager.GetComponentObject<Transform>(data.Right).gameObject;
                }

                return m_p2;
            }
            set
            {
                CellData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<CellData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<CellData>(m_Entity);
                Entity entity = default;
                if (value != null)
                {
                    var semanticObject = value.GetComponent<SemanticObject>();
                    if (semanticObject)
                        entity = semanticObject.Entity;
                }
                m_p2 = value;
                data.Right = entity;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public UnityEngine.GameObject Top
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<CellData>(m_Entity))
                {
                    var data = m_EntityManager.GetComponentData<CellData>(m_Entity);
                    if (data.Top != default)
                        m_p3 = m_EntityManager.GetComponentObject<Transform>(data.Top).gameObject;
                }

                return m_p3;
            }
            set
            {
                CellData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<CellData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<CellData>(m_Entity);
                Entity entity = default;
                if (value != null)
                {
                    var semanticObject = value.GetComponent<SemanticObject>();
                    if (semanticObject)
                        entity = semanticObject.Entity;
                }
                m_p3 = value;
                data.Top = entity;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public UnityEngine.GameObject Bottom
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<CellData>(m_Entity))
                {
                    var data = m_EntityManager.GetComponentData<CellData>(m_Entity);
                    if (data.Bottom != default)
                        m_p4 = m_EntityManager.GetComponentObject<Transform>(data.Bottom).gameObject;
                }

                return m_p4;
            }
            set
            {
                CellData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<CellData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<CellData>(m_Entity);
                Entity entity = default;
                if (value != null)
                {
                    var semanticObject = value.GetComponent<SemanticObject>();
                    if (semanticObject)
                        entity = semanticObject.Entity;
                }
                m_p4 = value;
                data.Bottom = entity;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public CellData Data
        {
            get => m_EntityManager != default && m_EntityManager.HasComponent<CellData>(m_Entity) ?
                m_EntityManager.GetComponentData<CellData>(m_Entity) : GetData();
            set
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<CellData>(m_Entity))
                    m_EntityManager.SetComponentData(m_Entity, value);
            }
        }

        #pragma warning disable 649
        [SerializeField]
        [InspectorName("Type")]
        Generated.Semantic.Traits.Enums.CellType m_p0 = (Generated.Semantic.Traits.Enums.CellType)0;
        [SerializeField]
        [InspectorName("Left")]
        UnityEngine.GameObject m_p1 = default;
        [SerializeField]
        [InspectorName("Right")]
        UnityEngine.GameObject m_p2 = default;
        [SerializeField]
        [InspectorName("Top")]
        UnityEngine.GameObject m_p3 = default;
        [SerializeField]
        [InspectorName("Bottom")]
        UnityEngine.GameObject m_p4 = default;
        #pragma warning restore 649

        EntityManager m_EntityManager;
        World m_World;
        Entity m_Entity;

        CellData GetData()
        {
            CellData data = default;
            data.Type = m_p0;
            if (m_p1)
            {
                var semanticObject = m_p1.GetComponent<SemanticObject>();
                if (semanticObject)
                    data.Left = semanticObject.Entity;
            }
            if (m_p2)
            {
                var semanticObject = m_p2.GetComponent<SemanticObject>();
                if (semanticObject)
                    data.Right = semanticObject.Entity;
            }
            if (m_p3)
            {
                var semanticObject = m_p3.GetComponent<SemanticObject>();
                if (semanticObject)
                    data.Top = semanticObject.Entity;
            }
            if (m_p4)
            {
                var semanticObject = m_p4.GetComponent<SemanticObject>();
                if (semanticObject)
                    data.Bottom = semanticObject.Entity;
            }

            return data;
        }

        IEnumerator UpdateRelations()
        {
            yield return null; // Wait one frame for all game objects to be converted to entities
            Left = m_p1;
            Right = m_p2;
            Top = m_p3;
            Bottom = m_p4;
            yield break;
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

            if (!destinationManager.HasComponent(entity, typeof(CellData)))
            {
                destinationManager.AddComponentData(entity, GetData());
                StartCoroutine(UpdateRelations());
            }
        }

        void OnDestroy()
        {
            if (m_World != default && m_World.IsCreated)
            {
                m_EntityManager.RemoveComponent<CellData>(m_Entity);
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
            TraitGizmos.DrawGizmoForTrait(nameof(CellData), gameObject,Data);
        }
#endif
    }
}
