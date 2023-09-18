#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace EcsRx.Unity.EditorCode.EditorInputs
{
    public class ColorEditorInput : SimpleEditorInput<Color>
    {
        protected override Color CreateTypeUI(string label, Color value)
        { return EditorGUILayout.ColorField(label, value); }
    }
}
#endif