using System;
using Generated.Semantic.Traits;
using Generated.Semantic.Traits.Enums;
using Unity.Collections;
using Unity.Entities;
using Unity.Semantic.Traits.Queries;
using UnityEngine;

namespace CustomQuery
{
    [Serializable]
    [QueryEditor("Cover direction", "Facing direction [m_Facing]", typeof(Cover))]
    struct CoverDirectionFilter : IQueryFilter 
    {
        [SerializeField]
        Direction m_Facing;
        
        public void Validate(EntityManager entityManager, NativeArray<Entity> entities, Unity.Collections.LowLevel.Unsafe.UnsafeBitArray entitiesValid)
        {

            for (var i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                if (entitiesValid.IsSet(i))
                {
                    var cover = entityManager.GetComponentData<CoverData>(entity);
                    entitiesValid.Set(i, m_Facing == cover.Direction);
                }
            }
        }
    }
}