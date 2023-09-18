#if UNITY_EDITOR
using UniRx;
using UnityEditor;

namespace EcsRx.Unity.EditorCode.EditorInputs
{
    public class ReactiveFloatEditorInput : SimpleEditorInput<FloatReactiveProperty>
    {
        protected override FloatReactiveProperty CreateTypeUI(string label, FloatReactiveProperty value)
        {
            value.Value = EditorGUILayout.FloatField(label, value.Value);
            return null;
        }
    }
}
#endif