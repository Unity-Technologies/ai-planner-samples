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
    [AddComponentMenu("Semantic/Traits/Carriable (Trait)")]
    [RequireComponent(typeof(SemanticObject))]
    public partial class Carriable : MonoBehaviour, ITrait
    {
        public UnityEngine.GameObject CarriedBy
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<CarriableData>(m_Entity))
                {
                    var data = m_EntityManager.GetComponentData<CarriableData>(m_Entity);
                    if (data.CarriedBy != default)
                        m_p0 = m_EntityManager.GetComponentObject<Transform>(data.CarriedBy).gameObject;
                }

                return m_p0;
            }
            set
            {
                CarriableData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<CarriableData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<CarriableData>(m_Entity);
                Entity entity = default;
                if (value != null)
                {
                    var semanticObject = value.GetComponent<SemanticObject>();
                    if (semanticObject)
                        entity = semanticObject.Entity;
                }
                m_p0 = value;
                data.CarriedBy = entity;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public CarriableData Data
        {
            get => m_EntityManager != default && m_EntityManager.HasComponent<CarriableData>(m_Entity) ?
                m_EntityManager.GetComponentData<CarriableData>(m_Entity) : GetData();
            set
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<CarriableData>(m_Entity))
                    m_EntityManager.SetComponentData(m_Entity, value);
            }
        }

        #pragma warning disable 649
        [SerializeField]
        [InspectorName("CarriedBy")]
        UnityEngine.GameObject m_p0 = default;
        #pragma warning restore 649

        EntityManager m_EntityManager;
        World m_World;
        Entity m_Entity;

        CarriableData GetData()
        {
            CarriableData data = default;
            if (m_p0)
            {
                var semanticObject = m_p0.GetComponent<SemanticObject>();
                if (semanticObject)
                    data.CarriedBy = semanticObject.Entity;
            }

            return data;
        }

        IEnumerator UpdateRelations()
        {
            yield return null; // Wait one frame for all game objects to be converted to entities
            CarriedBy = m_p0;
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

            if (!destinationManager.HasComponent(entity, typeof(CarriableData)))
            {
                destinationManager.AddComponentData(entity, GetData());
                StartCoroutine(UpdateRelations());
            }
        }

        void OnDestroy()
        {
            if (m_World != default && m_World.IsCreated)
            {
                m_EntityManager.RemoveComponent<CarriableData>(m_Entity);
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
            TraitGizmos.DrawGizmoForTrait(nameof(CarriableData), gameObject,Data);
        }
#endif
    }
}
