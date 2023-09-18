#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace EcsRx.Unity.EditorCode.EditorInputs
{
    public class RectEditorInput : SimpleEditorInput<Rect>
    {
        protected override Rect CreateTypeUI(string label, Rect value)
        { return EditorGUILayout.RectField(label, value); }
    }
}
#endif