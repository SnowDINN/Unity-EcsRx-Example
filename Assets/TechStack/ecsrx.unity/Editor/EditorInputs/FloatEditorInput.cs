#if UNITY_EDITOR
using UnityEditor;

namespace EcsRx.Unity.EditorCode.EditorInputs
{
    public class FloatEditorInput : SimpleEditorInput<float>
    {
        protected override float CreateTypeUI(string label, float value)
        { return EditorGUILayout.FloatField(label, value); }
    }
}
#endif