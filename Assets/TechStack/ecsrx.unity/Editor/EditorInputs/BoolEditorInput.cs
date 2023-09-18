#if UNITY_EDITOR
using UnityEditor;

namespace EcsRx.Unity.EditorCode.EditorInputs
{
    public class BoolEditorInput : SimpleEditorInput<bool>
    {
        protected override bool CreateTypeUI(string label, bool value)
        { return EditorGUILayout.Toggle(label, value); }
    }
}
#endif