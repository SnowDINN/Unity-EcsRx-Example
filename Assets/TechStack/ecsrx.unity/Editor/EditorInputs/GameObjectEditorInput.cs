#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace EcsRx.Unity.EditorCode.EditorInputs
{
    public class GameObjectEditorInput : SimpleEditorInput<GameObject>
    {
        protected override GameObject CreateTypeUI(string label, GameObject value)
        { return (GameObject)EditorGUILayout.ObjectField(label, value, typeof(GameObject), true); }
    }
}
#endif