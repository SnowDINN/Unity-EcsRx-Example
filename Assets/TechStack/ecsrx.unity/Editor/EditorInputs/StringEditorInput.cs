#if UNITY_EDITOR
using UnityEditor;

namespace EcsRx.Unity.EditorCode.EditorInputs
{
    public class StringEditorInput : SimpleEditorInput<string>
    {
        protected override string CreateTypeUI(string label, string value)
        { return EditorGUILayout.TextField(label, value); }
    }
}
#endif