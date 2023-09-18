#if UNITY_EDITOR
using UniRx;
using UnityEditor;

namespace EcsRx.Unity.EditorCode.EditorInputs
{
    public class ReactiveVector2EditorInput : SimpleEditorInput<Vector2ReactiveProperty>
    {
        protected override Vector2ReactiveProperty CreateTypeUI(string label, Vector2ReactiveProperty value)
        {
            value.Value = EditorGUILayout.Vector2Field(label, value.Value);
            return null;
        }
    }
}
#endif