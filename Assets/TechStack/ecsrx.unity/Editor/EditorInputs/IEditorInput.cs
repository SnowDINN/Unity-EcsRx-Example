using System;

namespace EcsRx.Unity.EditorCode.EditorInputs
{
    public interface IEditorInput
    {
        bool HandlesType(Type type);
        object CreateUI(string label, object value);
    }
}