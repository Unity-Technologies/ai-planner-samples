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
    [AddComponentMenu("Semantic/Traits/Waypoint (Trait)")]
    [RequireComponent(typeof(SemanticObject))]
    public partial class Waypoint : MonoBehaviour, ITrait
    {
        public UnityEngine.GameObject Left
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<WaypointData>(m_Entity))
                {
                    var data = m_EntityManager.GetComponentData<WaypointData>(m_Entity);
                    if (data.Left != default)
                        m_p0 = m_EntityManager.GetComponentObject<Transform>(data.Left).gameObject;
                }

                return m_p0;
            }
            set
            {
                WaypointData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<WaypointData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<WaypointData>(m_Entity);
                Entity entity = default;
                if (value != null)
                {
                    var semanticObject = value.GetComponent<SemanticObject>();
                    if (semanticObject)
                        entity = semanticObject.Entity;
                }
                m_p0 = value;
                data.Left = entity;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public UnityEngine.GameObject Right
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<WaypointData>(m_Entity))
                {
                    var data = m_EntityManager.GetComponentData<WaypointData>(m_Entity);
                    if (data.Right != default)
                        m_p1 = m_EntityManager.GetComponentObject<Transform>(data.Right).gameObject;
                }

                return m_p1;
            }
            set
            {
                WaypointData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<WaypointData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<WaypointData>(m_Entity);
                Entity entity = default;
                if (value != null)
                {
                    var semanticObject = value.GetComponent<SemanticObject>();
                    if (semanticObject)
                        entity = semanticObject.Entity;
                }
                m_p1 = value;
                data.Right = entity;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public UnityEngine.GameObject Up
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<WaypointData>(m_Entity))
                {
                    var data = m_EntityManager.GetComponentData<WaypointData>(m_Entity);
                    if (data.Up != default)
                        m_p2 = m_EntityManager.GetComponentObject<Transform>(data.Up).gameObject;
                }

                return m_p2;
            }
            set
            {
                WaypointData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<WaypointData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<WaypointData>(m_Entity);
                Entity entity = default;
                if (value != null)
                {
                    var semanticObject = value.GetComponent<SemanticObject>();
                    if (semanticObject)
                        entity = semanticObject.Entity;
                }
                m_p2 = value;
                data.Up = entity;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public UnityEngine.GameObject Down
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<WaypointData>(m_Entity))
                {
                    var data = m_EntityManager.GetComponentData<WaypointData>(m_Entity);
                    if (data.Down != default)
                        m_p3 = m_EntityManager.GetComponentObject<Transform>(data.Down).gameObject;
                }

                return m_p3;
            }
            set
            {
                WaypointData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<WaypointData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<WaypointData>(m_Entity);
                Entity entity = default;
                if (value != null)
                {
                    var semanticObject = value.GetComponent<SemanticObject>();
                    if (semanticObject)
                        entity = semanticObject.Entity;
                }
                m_p3 = value;
                data.Down = entity;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public System.Boolean Occupied
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<WaypointData>(m_Entity))
                {
                    m_p4 = m_EntityManager.GetComponentData<WaypointData>(m_Entity).Occupied;
                }

                return m_p4;
            }
            set
            {
                WaypointData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<WaypointData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<WaypointData>(m_Entity);
                data.Occupied = m_p4 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public System.Int32 StepsToEnd
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<WaypointData>(m_Entity))
                {
                    m_p5 = m_EntityManager.GetComponentData<WaypointData>(m_Entity).StepsToEnd;
                }

                return m_p5;
            }
            set
            {
                WaypointData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<WaypointData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<WaypointData>(m_Entity);
                data.StepsToEnd = m_p5 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public WaypointData Data
        {
            get => m_EntityManager != default && m_EntityManager.HasComponent<WaypointData>(m_Entity) ?
                m_EntityManager.GetComponentData<WaypointData>(m_Entity) : GetData();
            set
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<WaypointData>(m_Entity))
                    m_EntityManager.SetComponentData(m_Entity, value);
            }
        }

        #pragma warning disable 649
        [SerializeField]
        [InspectorName("Left")]
        UnityEngine.GameObject m_p0 = default;
        [SerializeField]
        [InspectorName("Right")]
        UnityEngine.GameObject m_p1 = default;
        [SerializeField]
        [InspectorName("Up")]
        UnityEngine.GameObject m_p2 = default;
        [SerializeField]
        [InspectorName("Down")]
        UnityEngine.GameObject m_p3 = default;
        [SerializeField]
        [InspectorName("Occupied")]
        System.Boolean m_p4 = false;
        [SerializeField]
        [InspectorName("StepsToEnd")]
        System.Int32 m_p5 = 0;
        #pragma warning restore 649

        EntityManager m_EntityManager;
        World m_World;
        Entity m_Entity;

        WaypointData GetData()
        {
            WaypointData data = default;
            if (m_p0)
            {
                var semanticObject = m_p0.GetComponent<SemanticObject>();
                if (semanticObject)
                    data.Left = semanticObject.Entity;
            }
            if (m_p1)
            {
                var semanticObject = m_p1.GetComponent<SemanticObject>();
                if (semanticObject)
                    data.Right = semanticObject.Entity;
            }
            if (m_p2)
            {
                var semanticObject = m_p2.GetComponent<SemanticObject>();
                if (semanticObject)
                    data.Up = semanticObject.Entity;
            }
            if (m_p3)
            {
                var semanticObject = m_p3.GetComponent<SemanticObject>();
                if (semanticObject)
                    data.Down = semanticObject.Entity;
            }
            data.Occupied = m_p4;
            data.StepsToEnd = m_p5;

            return data;
        }

        IEnumerator UpdateRelations()
        {
            yield return null; // Wait one frame for all game objects to be converted to entities
            Left = m_p0;
            Right = m_p1;
            Up = m_p2;
            Down = m_p3;
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

            if (!destinationManager.HasComponent(entity, typeof(WaypointData)))
            {
                destinationManager.AddComponentData(entity, GetData());
                StartCoroutine(UpdateRelations());
            }
        }

        void OnDestroy()
        {
            if (m_World != default && m_World.IsCreated)
            {
                m_EntityManager.RemoveComponent<WaypointData>(m_Entity);
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
            TraitGizmos.DrawGizmoForTrait(nameof(WaypointData), gameObject,Data);
        }
#endif
    }
}
