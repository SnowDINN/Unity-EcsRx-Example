#if UNITY_EDITOR
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Plugins.Views.Components;
using EcsRx.Unity.Extensions;
using UnityEditor;
using UnityEngine;

namespace EcsRx.Unity.EditorCode.EditorInputs
{
    public class ViewEntityEditorInput : SimpleEditorInput<Entity>
    {
        protected override Entity CreateTypeUI(string label, Entity value)
        {
            GUI.enabled = false;
            if (value.HasComponent<ViewComponent>())
            {
                var view = value.GetGameObject();
                EditorGUILayout.ObjectField(label, view, typeof(GameObject), true);
            }
            else
            {
                EditorGUILayout.IntField(label + " ID", value.Id);
            }
            GUI.enabled = true;
            return value;
        }
    }
}
#endif