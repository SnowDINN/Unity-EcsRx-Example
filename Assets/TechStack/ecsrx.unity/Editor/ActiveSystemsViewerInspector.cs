#if UNITY_EDITOR
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Plugins.Views.Components;
using EcsRx.Systems;
using EcsRx.Unity.Extensions;
using EcsRx.Unity.EditorCode.Extensions;
using EcsRx.Unity.EditorCode.UIAspects;
using EcsRx.Unity.MonoBehaviours;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace EcsRx.Unity.EditorCode
{
    [CustomEditor(typeof(ActiveSystemsViewer))]
    public class ActiveSystemsViewerInspector : global::UnityEditor.Editor
    {
        public class EntityInfo
        {
            public bool ShowComponents { get; set; }
            public IEntity Entity { get; set; }
            public bool Touched { get; set; }
        }

        public class SystemInfo
        {
            public bool ShowEntities { get; set; }
            public ISystem System { get; set; }
            public bool Touched { get; set; }

            public List<EntityInfo> EntityInfos { get; set; }

            public SystemInfo()
            {
                EntityInfos = new List<EntityInfo>();
            }
        }

        Dictionary<string, SystemInfo> systemInfoDict = new Dictionary<string, SystemInfo>();

        public override void OnInspectorGUI()
        {
            var activeSystemsViewer = (ActiveSystemsViewer)target;
            if (activeSystemsViewer == null) { return; }

            var systemExecutor = activeSystemsViewer.SystemExecutor;
            if (systemExecutor == null)
            {
                EditorGUILayout.LabelField("System Executor Inactive");
                return;
            }

            if (systemExecutor.Systems.Count() == 0)
            {
                EditorGUILayout.LabelField("There is no systems");
                return;
            }

            #region SYSTEM REFRESH
            foreach (var systemInfoKeyPair in systemInfoDict)
            {
                systemInfoKeyPair.Value.Touched = false;
            }

            var orderedSystems = systemExecutor.Systems.OrderBy(x => x.GetType().Name);
            foreach (var system in orderedSystems)
            {
                SystemInfo systemInfo;
                if (systemInfoDict.TryGetValue(system.GetType().Name, out systemInfo) == false)
                {
                    systemInfo = new SystemInfo();
                    systemInfoDict.Add(system.GetType().Name, systemInfo);
                }

                systemInfo.System = system;
                systemInfo.Touched = true;
            }

            List<string> removeSystemInfos = new List<string>();
            foreach (var systemInfoKeyPair in systemInfoDict)
            {
                if (systemInfoKeyPair.Value.Touched == false)
                {
                    removeSystemInfos.Add(systemInfoKeyPair.Key);
                }
            }

            foreach (var removeSystemInfo in removeSystemInfos)
            {
                systemInfoDict.Remove(removeSystemInfo);
            }
            #endregion

            foreach (var systemInfoKeyPair in systemInfoDict)
            {
                var key = systemInfoKeyPair.Key;
                var systemInfo = systemInfoKeyPair.Value;
                EditorGUILayout.BeginVertical();
                {
                    var system = systemInfo.System;

                    EditorGUILayout.BeginVertical(EditorExtensions.DefaultBoxStyle);
                    {
                        var affinities = system.GetGroupAffinities();
                        var observableGroup = activeSystemsViewer.ObservableGroupManager.GetObservableGroup(system.Group, affinities);

                        this.WithHorizontalLayout(() =>
                        {
                            if (observableGroup.Count > 0)
                            {
                                if (systemInfo.ShowEntities)
                                {
                                    if (this.WithIconButton("V")) { systemInfo.ShowEntities = false; }
                                }
                                else
                                {
                                    if (this.WithIconButton(">")) { systemInfo.ShowEntities = true; }
                                }

                                this.WithLabel(key + $"({observableGroup.Count})");
                            }
                            else
                            {
                                this.WithLabel(key);
                            }
                        });

                        if (systemInfo.ShowEntities)
                        {
                            var entityInfos = systemInfo.EntityInfos;
                            foreach (var entityInfo in entityInfos)
                            {
                                entityInfo.Touched = false;
                            }

                            foreach (var entity in observableGroup)
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

                            foreach (var entityInfo in entityInfos)
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
#endif