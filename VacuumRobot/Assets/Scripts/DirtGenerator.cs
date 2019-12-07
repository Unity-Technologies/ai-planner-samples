using System;
using UnityEngine;

namespace VacuumGame
{
    public class DirtGenerator : MonoBehaviour
    {
        [Serializable]
        struct Range
        {
#pragma warning disable 0649
            public float min;
            public float max;
#pragma warning restore 0649
            public float Random => UnityEngine.Random.Range(min, max);
        }
        
        [SerializeField]
        GameObject m_DirtPrefab = default;

        [SerializeField]
        float m_DirtSpawnFrequency = default;

        [SerializeField]
        Range m_RangeX = default;
        
        [SerializeField]
        Range m_RangeZ = default;
        
        float m_LastSpawnTime;
        
        public void Update()
        {
            if (Time.realtimeSinceStartup < m_LastSpawnTime + m_DirtSpawnFrequency)
                return;

            Instantiate(m_DirtPrefab, new Vector3(m_RangeX.Random, 0, m_RangeZ.Random), Quaternion.identity, transform);
            m_LastSpawnTime = Time.realtimeSinceStartup;
        }
    }
}