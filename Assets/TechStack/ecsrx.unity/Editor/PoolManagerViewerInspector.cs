#if UNITY_EDITOR
using EcsRx.Collections.Entity;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Plugins.Views.Components;
using EcsRx.Unity.Extensions;
using EcsRx.Unity.EditorCode.Extensions;
using EcsRx.Unity.EditorCode.UIAspects;
using EcsRx.Unity.MonoBehaviours;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace EcsRx.Unity.EditorCode
{
    [CustomEditor(typeof(EntityDatabaseViewer))]
    public class PoolManagerViewerInspector : global::UnityEditor.Editor
    {
        public class EntityInfo
        {
            public bool ShowComponents { get; set; }
            public IEntity Entity { get; set; }
            public bool Touched { get; set; }
        }

        Dictionary<int, List<EntityInfo>> entityInfoDict = new Dictionary<int, List<EntityInfo>>();
        int searchCollectionId = 0;
        int searchEntityId = 0;

        public override void OnInspectorGUI()
        {
            var entityDatabaseViewer = (EntityDatabaseViewer)target;
            var entityDatabase = entityDatabaseViewer.EntityDatabase;

            if (entityDatabase == null)
            {
                EditorGUILayout.LabelField("Entity Database Inactive");
                return;
            }

            EditorGUILayout.TextField("Active Pools");
            searchCollectionId = EditorGUILayout.IntField("Search Collection ID", searchCollectionId);
            searchEntityId = EditorGUILayout.IntField("Search Entity ID", searchEntityId);
            EditorGUILayout.Space();

            foreach (var entityCollection in entityDatabase.Collections)
            {
                List<EntityInfo> entityInfos;
                if (entityInfoDict.TryGetValue(entityCollection.Id, out entityInfos) == false)
                {
                    entityInfos = new List<EntityInfo>();
                    entityInfoDict.Add(entityCollection.Id, entityInfos);
                }

                if (entityCollection.Count == 0)
                {
                    entityInfos.Clear();
                }
                else
                {
                    foreach (var entityInfo in entityInfos)
                    {
                        entityInfo.Touched = false;
                    }

                    var entityLookup = entityCollection as IEntityCollection;
                    foreach (var entity in entityLookup)
                    {
                        bool isNew = true;
                        foreach (var entityInfo in entityInfos)
                        {
                            if (entity.Id == entityInfo.Entity.Id)
                            {
                                entityInfo.Entity = entity;
                                entityInfo.Touched = true;
                                isNew = false;
                                break;
                            }
                        }

                        if (isNew)
                        {
                            entityInfos.Add(new EntityInfo()
                            {
                                Entity = entity,
                                ShowComponents = false,
                                Touched = true
                            });
                        }
                    }

                    List<EntityInfo> removeEntityInfos = new List<EntityInfo>();
                    foreach (var entityInfo in entityInfos)
                    {
                        if (entityInfo.Touched == false)
                        {
                            removeEntityInfos.Add(entityInfo);
                        }
                    }

                    foreach (var removeEntityInfo in removeEntityInfos)
                    {
                        entityInfos.Remove(removeEntityInfo);
                    }
                }
            }

            if (searchEntityId > 0)
            {
                if (!entityDatabase.IsCollection(searchCollectionId))
                {
                    EditorGUILayout.LabelField("There's no collection");
                    return;
                }

                foreach (var entityInfoKeyPair in entityInfoDict)
                {
                    if (entityInfoKeyPair.Key != searchCollectionId)
                        continue;

                    EditorGUILayout.BeginVertical();

                    bool found = false;

                    foreach (var entityInfo in entityInfoKeyPair.Value)
                    {
                        var entity = entityInfo.Entity;
                        if (entity.Id != searchEntityId)
                            continue;

                        found = true;

                        EditorGUILayout.BeginVertical(EditorExtensions.DefaultBoxStyle);

                        this.WithHorizontalLayout(() =>
                        {
                            if (entity.Components.Count() > 0)
                            {
                                if (entityInfo.ShowComponents)
                                {
                                    if (this.WithIconButton("V")) { entityInfo.ShowComponents = false; }
                                }
                                else
                                {
                                    if (this.WithIconButton(">")) { entityInfo.ShowComponents = true; }
                                }

                                string entityName = "";
                                if (entity.HasComponent<ViewComponent>())
                                {
                                    var view = entity.GetGameObject();
                                    if (view != null)
                                    {
                                        entityName += $"[{entity.Id}]({entity.Components.Count()}) {view.name}";
                                    }
                                    else
                                    {
                                        entityName += $"[{entity.Id}]({entity.Components.Count()}) view is destroyed";
                                    }
                                }
                                else
                                {
                                    entityName += $"[{entity.Id}]({entity.Components.Count()}) no view component";
                                }

                                this.WithLabel(entityName);
                            }
                            else
                            {
                                entityInfo.ShowComponents = false;

                                string entityName = "";
                                entityName += $"[{entity.Id}](0) no view component";

                                this.WithLabel(entityName);
                            }
                        });

                        if (entityInfo.ShowComponents)
                        {
                            var currentComponents = entity.Components.ToArray();
                            for (var i = 0; i < currentComponents.Length; i++)
                            {
                                var currentIndex = i;
                                this.UseVerticalBoxLayout(() =>
                                {
                                    var componentType = currentComponents[currentIndex].GetType();
                                    var typeName = componentType.Name;
                                    var typeNamespace = componentType.Namespace;

                                    this.WithVerticalLayout(() =>
                                    {
                                        this.WithHorizontalLayout(() =>
                                        {
                                            this.WithLabel(typeName);
                                        });

                                        EditorGUILayout.LabelField(typeNamespace);
                                        EditorGUILayout.Space();
                                    });

                                    var component = currentComponents[currentIndex];
                                    ComponentUIAspect.ShowComponentProperties(component);
                                });
                            }
                        }

                        EditorGUILayout.EndVertical();

                        break;
                    }

                    if (found == false)
                    {
                        EditorGUILayout.LabelField("There's no entity");
                    }

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                }
            }
            else
            {
                foreach (var entityInfoKeyPair in entityInfoDict)
                {
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField($"EntityCollection {entityInfoKeyPair.Key} / Entities {entityInfoKeyPair.Value.Count}");

                    foreach (var entityInfo in entityInfoKeyPair.Value)
                    {
                        var entity = entityInfo.Entity;

                        EditorGUILayout.BeginVertical(EditorExtensions.DefaultBoxStyle);

                        this.WithHorizontalLayout(() =>
                        {
                            if (entity.Components.Count() > 0)
                            {
                                if (entityInfo.ShowComponents)
                                {
                                    if (this.WithIconButton("V")) { entityInfo.ShowComponents = false; }
                                }
                                else
                                {
                                    if (this.WithIconButton(">")) { entityInfo.ShowComponents = true; }
                                }

                                string entityName = "";
                                if (entity.HasComponent<ViewComponent>())
                                {
                                    var view = entity.GetGameObject();
                                    if (view != null)
                                    {
                                        entityName += $"[{entity.Id}]({entity.Components.Count()}) {view.name}";
                                    }
                                    else
                                    {
                                        entityName += $"[{entity.Id}]({entity.Components.Count()}) view is destroyed";
                                    }
                                }
                                else
                                {
                                    entityName += $"[{entity.Id}]({entity.Components.Count()}) no view component";
                                }

                                this.WithLabel(entityName);
                            }
                            else
                            {
                                entityInfo.ShowComponents = false;

                                string entityName = "";
                                entityName += $"[{entity.Id}](0) no view component";

                                this.WithLabel(entityName);
                            }
                        });

                        if (entityInfo.ShowComponents)
                        {
                            var currentComponents = entity.Components.ToArray();
                            for (var i = 0; i < currentComponents.Length; i++)
                            {
                                var currentIndex = i;
                                this.UseVerticalBoxLayout(() =>
                                {
                                    var componentType = currentComponents[currentIndex].GetType();
                                    var typeName = componentType.Name;
                                    var typeNamespace = componentType.Namespace;

                                    this.WithVerticalLayout(() =>
                                    {
                                        this.WithHorizontalLayout(() =>
                                        {
                                            this.WithLabel(typeName);
                                        });

                                        EditorGUILayout.LabelField(typeNamespace);
                                        EditorGUILayout.Space();
                                    });

                                    var component = currentComponents[currentIndex];
                                    ComponentUIAspect.ShowComponentProperties(component);
                                });
                            }
                        }

                        EditorGUILayout.EndVertical();
                    }

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                }
            }
        }
    }
}
#endif