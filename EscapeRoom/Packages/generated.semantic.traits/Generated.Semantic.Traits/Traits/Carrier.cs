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
    [AddComponentMenu("Semantic/Traits/Carrier (Trait)")]
    [RequireComponent(typeof(SemanticObject))]
    public partial class Carrier : MonoBehaviour, ITrait
    {
        public UnityEngine.GameObject Carried
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<CarrierData>(m_Entity))
                {
                    var data = m_EntityManager.GetComponentData<CarrierData>(m_Entity);
                    if (data.Carried != default)
                        m_p0 = m_EntityManager.GetComponentObject<Transform>(data.Carried).gameObject;
                }

                return m_p0;
            }
            set
            {
                CarrierData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<CarrierData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<CarrierData>(m_Entity);
                Entity entity = default;
                if (value != null)
                {
                    var semanticObject = value.GetComponent<SemanticObject>();
                    if (semanticObject)
                        entity = semanticObject.Entity;
                }
                m_p0 = value;
                data.Carried = entity;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public CarrierData Data
        {
            get => m_EntityManager != default && m_EntityManager.HasComponent<CarrierData>(m_Entity) ?
                m_EntityManager.GetComponentData<CarrierData>(m_Entity) : GetData();
            set
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<CarrierData>(m_Entity))
                    m_EntityManager.SetComponentData(m_Entity, value);
            }
        }

        #pragma warning disable 649
        [SerializeField]
        [InspectorName("Carried")]
        UnityEngine.GameObject m_p0 = default;
        #pragma warning restore 649

        EntityManager m_EntityManager;
        World m_World;
        Entity m_Entity;

        CarrierData GetData()
        {
            CarrierData data = default;
            if (m_p0)
            {
                var semanticObject = m_p0.GetComponent<SemanticObject>();
                if (semanticObject)
                    data.Carried = semanticObject.Entity;
            }

            return data;
        }

        IEnumerator UpdateRelations()
        {
            yield return null; // Wait one frame for all game objects to be converted to entities
            Carried = m_p0;
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

            if (!destinationManager.HasComponent(entity, typeof(CarrierData)))
            {
                destinationManager.AddComponentData(entity, GetData());
                StartCoroutine(UpdateRelations());
            }
        }

        void OnDestroy()
        {
            if (m_World != default && m_World.IsCreated)
            {
                m_EntityManager.RemoveComponent<CarrierData>(m_Entity);
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
            TraitGizmos.DrawGizmoForTrait(nameof(CarrierData), gameObject,Data);
        }
#endif
    }
}
