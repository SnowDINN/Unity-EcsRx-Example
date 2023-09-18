#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Components;
using EcsRx.Extensions;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Unity.EditorCode.Extensions;
using EcsRx.Unity.EditorCode.UIAspects;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace EcsRx.Unity.EditorCode
{
    [CustomEditor(typeof(EntityView))]
    public class EntityViewInspector : global::UnityEditor.Editor
    {
        private EntityView _entityView;

        public bool showComponents = true;

        private Type[] componentTypes;

        private readonly IEnumerable<Type> allComponentTypes = AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(s => s.GetTypes())
                                .Where(p => typeof(IComponent).IsAssignableFrom(p) && p.IsClass);


        private void PoolSection()
        {
            this.UseVerticalBoxLayout(() =>
            {
                if (GUILayout.Button("Destroy Entity"))
                {
                    _entityView.EntityCollection.RemoveEntity(_entityView.Entity.Id);
                    Destroy(_entityView.gameObject);
                }

                this.UseVerticalBoxLayout(() =>
                {
                    var id = _entityView.Entity.Id.ToString();
                    this.WithLabelField("Entity Id: ", id);
                });

                this.UseVerticalBoxLayout(() =>
                {
                    this.WithLabelField("EntityCollection: ", _entityView.EntityCollection.Id.ToString());
                });
            });
        }

        private void ComponentListings()
        {
            EditorGUILayout.BeginVertical(EditorExtensions.DefaultBoxStyle);
            this.WithHorizontalLayout(() =>
            {
                this.WithLabel("Components (" + _entityView.Entity.Components.Count() + ")");
                if (this.WithIconButton("▸")) { showComponents = false; }
                if (this.WithIconButton("▾")) { showComponents = true; }
            });

            var componentsToRemove = new List<int>();
            if (showComponents)
            {
                var currentComponents = _entityView.Entity.Components.ToArray();
                for (var i = 0; i < currentComponents.Length; i++)
                {
                    var currentIndex = i;
                    this.UseVerticalBoxLayout(() =>
                    {
                        var componentType = currentComponents[currentIndex].GetType();
                        var typeName = componentType.Name;
                        var typeNamespace = componentType.Namespace;

                        //this.WithVerticalLayout(() =>
                        //{
                            this.WithHorizontalLayout(() =>
                            {
                                //if (this.WithIconButton("-"))
                                //{
                                //    componentsToRemove.Add(currentIndex);
                                //}

                                this.WithLabel(typeName);
                            });

                            EditorGUILayout.LabelField(typeNamespace);
                            EditorGUILayout.Space();
                        //});
                        
                        var component = currentComponents[currentIndex];
                        ComponentUIAspect.ShowComponentProperties(component);
                    });
                }
            }

            EditorGUILayout.EndVertical();

            if (componentsToRemove.Count == 0)
            { return; }

            var activeComponents = _entityView.Entity.Components.ToArray();
            var componentArray = new Type[componentsToRemove.Count];
            for (var i = 0; i < componentsToRemove.Count; i++)
            {
                var component = activeComponents[i];
                componentArray[i] = component.GetType();
            }
            _entityView.Entity.RemoveComponents(componentArray);

            refreshComponentTypes();
        }

        void refreshComponentTypes()
        {
            if (_entityView.Entity == null)
                return;

            componentTypes = allComponentTypes
                .Where(x => !_entityView.Entity.Components.Select(y => y.GetType()).Contains(x))
                .ToArray();
        }

        public override void OnInspectorGUI()
        {
            if (_entityView.Entity == null)
            {
                EditorGUILayout.LabelField("No Entity Assigned");
                return;
            }

            if (componentTypes == null)
                return;

            PoolSection();
            EditorGUILayout.Space();
            ComponentListings();
        }

        public override VisualElement CreateInspectorGUI()
        {
            _entityView = (EntityView)target;

            refreshComponentTypes();

            return base.CreateInspectorGUI();
        }
    }
}
#endif