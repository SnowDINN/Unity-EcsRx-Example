#if UNITY_EDITOR
using UnityEditor;

namespace EcsRx.Unity.EditorCode.EditorInputs
{
    public class IntEditorInput : SimpleEditorInput<int>
    {
        protected override int CreateTypeUI(string label, int value)
        { return EditorGUILayout.IntField(label, value); }
    }
}
#endif