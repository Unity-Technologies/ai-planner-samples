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
    [AddComponentMenu("Semantic/Traits/ActivationSwitch (Trait)")]
    [RequireComponent(typeof(SemanticObject))]
    public partial class ActivationSwitch : MonoBehaviour, ITrait
    {
        public Generated.Semantic.Traits.Enums.ActivationType Type
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<ActivationSwitchData>(m_Entity))
                {
                    m_p0 = m_EntityManager.GetComponentData<ActivationSwitchData>(m_Entity).Type;
                }

                return m_p0;
            }
            set
            {
                ActivationSwitchData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<ActivationSwitchData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<ActivationSwitchData>(m_Entity);
                data.Type = m_p0 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public ActivationSwitchData Data
        {
            get => m_EntityManager != default && m_EntityManager.HasComponent<ActivationSwitchData>(m_Entity) ?
                m_EntityManager.GetComponentData<ActivationSwitchData>(m_Entity) : GetData();
            set
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<ActivationSwitchData>(m_Entity))
                    m_EntityManager.SetComponentData(m_Entity, value);
            }
        }

        #pragma warning disable 649
        [SerializeField]
        [InspectorName("Type")]
        Generated.Semantic.Traits.Enums.ActivationType m_p0 = (Generated.Semantic.Traits.Enums.ActivationType)0;
        #pragma warning restore 649

        EntityManager m_EntityManager;
        World m_World;
        Entity m_Entity;

        ActivationSwitchData GetData()
        {
            ActivationSwitchData data = default;
            data.Type = m_p0;

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

            if (!destinationManager.HasComponent(entity, typeof(ActivationSwitchData)))
            {
                destinationManager.AddComponentData(entity, GetData());
            }
        }

        void OnDestroy()
        {
            if (m_World != default && m_World.IsCreated)
            {
                m_EntityManager.RemoveComponent<ActivationSwitchData>(m_Entity);
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
            TraitGizmos.DrawGizmoForTrait(nameof(ActivationSwitchData), gameObject,Data);
        }
#endif
    }
}
