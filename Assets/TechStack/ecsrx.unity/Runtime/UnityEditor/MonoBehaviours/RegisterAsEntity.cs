using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Collections;
using EcsRx.Collections.Database;
using EcsRx.Collections.Entity;
using EcsRx.Components;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Plugins.Views.Components;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Unity.Extensions;
using EcsRx.Zenject;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace EcsRx.Unity.MonoBehaviours
{
    public abstract class RegisterAsEntity : MonoBehaviour, IConvertToEntity
    {
        public IEntityDatabase EntityDatabase { get; private set; }

        public int CollectionId;

        public void RegisterEntity()
        {
            if (!gameObject.activeInHierarchy || !gameObject.activeSelf) { return; }

            IEntityCollection collectionToUse;

            if (CollectionId == 0)
            { collectionToUse = EntityDatabase.GetCollection(); }
            else if (EntityDatabase.Collections.All(x => x.Id != CollectionId))
            { collectionToUse = EntityDatabase.CreateCollection(CollectionId); }
            else
            { collectionToUse = EntityDatabase.GetCollection(CollectionId); }

            var entityView = gameObject.GetComponent<EntityView>();
            if (entityView != null)
            {
                setupEntityComponent(entityView.Entity);
            }
            else
            {
                var createdEntity = collectionToUse.CreateEntity();
                createdEntity.AddComponents(new ViewComponent { View = gameObject });
                setupEntityBinding(createdEntity, collectionToUse);
                setupEntityComponent(createdEntity);
            }

            Destroy(this);
        }

        IEnumerator Start()
        {
            while (EcsRxApplicationBehaviour.Instance == null)
                yield return null;

            while (!EcsRxApplicationBehaviour.Instance.Started)
                yield return null;

            while (EcsRxApplicationBehaviour.Instance.EntityDatabase == null)
                yield return null;

            EntityDatabase = EcsRxApplicationBehaviour.Instance.EntityDatabase;

            RegisterEntity();
        }

        private void setupEntityBinding(IEntity entity, IEntityCollection entityCollection)
        {
            var entityBinding = gameObject.AddComponent<EntityView>();
            entityBinding.Entity = entity;
            entityBinding.EntityCollection = entityCollection;
        }

        private void setupEntityComponent(IEntity entity)
        {
            Convert(entity);
        }

        public virtual void Convert(IEntity entity)
        {
        }
    }
}