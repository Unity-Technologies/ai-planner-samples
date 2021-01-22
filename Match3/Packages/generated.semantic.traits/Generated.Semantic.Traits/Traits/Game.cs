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
    [AddComponentMenu("Semantic/Traits/Game (Trait)")]
    [RequireComponent(typeof(SemanticObject))]
    public partial class Game : MonoBehaviour, ITrait
    {
        public System.Int32 MoveCount
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<GameData>(m_Entity))
                {
                    m_p0 = m_EntityManager.GetComponentData<GameData>(m_Entity).MoveCount;
                }

                return m_p0;
            }
            set
            {
                GameData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<GameData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<GameData>(m_Entity);
                data.MoveCount = m_p0 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public System.Int32 Score
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<GameData>(m_Entity))
                {
                    m_p1 = m_EntityManager.GetComponentData<GameData>(m_Entity).Score;
                }

                return m_p1;
            }
            set
            {
                GameData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<GameData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<GameData>(m_Entity);
                data.Score = m_p1 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public Generated.Semantic.Traits.Enums.CellType GoalType
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<GameData>(m_Entity))
                {
                    m_p2 = m_EntityManager.GetComponentData<GameData>(m_Entity).GoalType;
                }

                return m_p2;
            }
            set
            {
                GameData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<GameData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<GameData>(m_Entity);
                data.GoalType = m_p2 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public System.Int32 GoalCount
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<GameData>(m_Entity))
                {
                    m_p3 = m_EntityManager.GetComponentData<GameData>(m_Entity).GoalCount;
                }

                return m_p3;
            }
            set
            {
                GameData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<GameData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<GameData>(m_Entity);
                data.GoalCount = m_p3 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public GameData Data
        {
            get => m_EntityManager != default && m_EntityManager.HasComponent<GameData>(m_Entity) ?
                m_EntityManager.GetComponentData<GameData>(m_Entity) : GetData();
            set
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<GameData>(m_Entity))
                    m_EntityManager.SetComponentData(m_Entity, value);
            }
        }

        #pragma warning disable 649
        [SerializeField]
        [InspectorName("MoveCount")]
        System.Int32 m_p0 = 0;
        [SerializeField]
        [InspectorName("Score")]
        System.Int32 m_p1 = 0;
        [SerializeField]
        [InspectorName("GoalType")]
        Generated.Semantic.Traits.Enums.CellType m_p2 = (Generated.Semantic.Traits.Enums.CellType)0;
        [SerializeField]
        [InspectorName("GoalCount")]
        System.Int32 m_p3 = 0;
        #pragma warning restore 649

        EntityManager m_EntityManager;
        World m_World;
        Entity m_Entity;

        GameData GetData()
        {
            GameData data = default;
            data.MoveCount = m_p0;
            data.Score = m_p1;
            data.GoalType = m_p2;
            data.GoalCount = m_p3;

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

            if (!destinationManager.HasComponent(entity, typeof(GameData)))
            {
                destinationManager.AddComponentData(entity, GetData());
            }
        }

        void OnDestroy()
        {
            if (m_World != default && m_World.IsCreated)
            {
                m_EntityManager.RemoveComponent<GameData>(m_Entity);
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
            TraitGizmos.DrawGizmoForTrait(nameof(GameData), gameObject,Data);
        }
#endif
    }
}
