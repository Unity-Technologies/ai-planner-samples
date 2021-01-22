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
    [AddComponentMenu("Semantic/Traits/Character (Trait)")]
    [RequireComponent(typeof(SemanticObject))]
    public partial class Character : MonoBehaviour, ITrait
    {
        public UnityEngine.GameObject Waypoint
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<CharacterData>(m_Entity))
                {
                    var data = m_EntityManager.GetComponentData<CharacterData>(m_Entity);
                    if (data.Waypoint != default)
                        m_p0 = m_EntityManager.GetComponentObject<Transform>(data.Waypoint).gameObject;
                }

                return m_p0;
            }
            set
            {
                CharacterData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<CharacterData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<CharacterData>(m_Entity);
                Entity entity = default;
                if (value != null)
                {
                    var semanticObject = value.GetComponent<SemanticObject>();
                    if (semanticObject)
                        entity = semanticObject.Entity;
                }
                m_p0 = value;
                data.Waypoint = entity;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public System.Int32 ID
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<CharacterData>(m_Entity))
                {
                    m_p1 = m_EntityManager.GetComponentData<CharacterData>(m_Entity).ID;
                }

                return m_p1;
            }
            set
            {
                CharacterData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<CharacterData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<CharacterData>(m_Entity);
                data.ID = m_p1 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public CharacterData Data
        {
            get => m_EntityManager != default && m_EntityManager.HasComponent<CharacterData>(m_Entity) ?
                m_EntityManager.GetComponentData<CharacterData>(m_Entity) : GetData();
            set
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<CharacterData>(m_Entity))
                    m_EntityManager.SetComponentData(m_Entity, value);
            }
        }

        #pragma warning disable 649
        [SerializeField]
        [InspectorName("Waypoint")]
        UnityEngine.GameObject m_p0 = default;
        [SerializeField]
        [InspectorName("ID")]
        System.Int32 m_p1 = 0;
        #pragma warning restore 649

        EntityManager m_EntityManager;
        World m_World;
        Entity m_Entity;

        CharacterData GetData()
        {
            CharacterData data = default;
            if (m_p0)
            {
                var semanticObject = m_p0.GetComponent<SemanticObject>();
                if (semanticObject)
                    data.Waypoint = semanticObject.Entity;
            }
            data.ID = m_p1;

            return data;
        }

        IEnumerator UpdateRelations()
        {
            yield return null; // Wait one frame for all game objects to be converted to entities
            Waypoint = m_p0;
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

            if (!destinationManager.HasComponent(entity, typeof(CharacterData)))
            {
                destinationManager.AddComponentData(entity, GetData());
                StartCoroutine(UpdateRelations());
            }
        }

        void OnDestroy()
        {
            if (m_World != default && m_World.IsCreated)
            {
                m_EntityManager.RemoveComponent<CharacterData>(m_Entity);
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
            TraitGizmos.DrawGizmoForTrait(nameof(CharacterData), gameObject,Data);
        }
#endif
    }
}
