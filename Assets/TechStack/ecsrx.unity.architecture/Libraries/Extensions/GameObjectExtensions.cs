using EcsRx.Entities;
using EcsRx.Unity.Extensions;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Zenject;
using UnityEngine;

namespace EcsRx.Unity.Framework
{
    public static class GameObjectExtensions
    {
        public static IEntity GetEntity(this GameObject go, bool create = false, int collectionId = 0)
        {
            var entityView = go.GetComponent<EntityView>();
            if (entityView == null)
            {
                if (create)
                {
                    var pool = EcsRxApplicationBehaviour.Instance.EntityDatabase.GetCollection(collectionId);
                    var entity = pool.CreateEntity();
                    go.LinkEntity(entity, pool);
                    return entity;
                }

                return null;
            }

            return entityView.Entity;
        }

        public static bool HasEntity(this GameObject go)
        {
            return go.GetComponent<EntityView>();
        }
    }
}