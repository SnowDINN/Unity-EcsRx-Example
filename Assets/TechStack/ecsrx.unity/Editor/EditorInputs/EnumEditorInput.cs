#if UNITY_EDITOR
using System;
using UnityEditor;

namespace EcsRx.Unity.EditorCode.EditorInputs
{
    public class EnumEditorInput : SimpleEditorInput<Enum>
    {
        public override bool HandlesType(Type type)
        {
            return type.IsEnum;
        }
        
        protected override Enum CreateTypeUI(string label, Enum value)
        { return EditorGUILayout.EnumPopup(label, value); }
    }
}
#endif