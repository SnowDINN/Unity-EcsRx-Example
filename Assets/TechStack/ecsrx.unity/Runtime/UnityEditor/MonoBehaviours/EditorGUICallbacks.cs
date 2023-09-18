using EcsRx.Collections;
using EcsRx.Collections.Database;
using EcsRx.Executor;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.MonoBehaviours
{
    public class EditorGUICallbacks : MonoBehaviour
    {
        public delegate void DrawGizmos();
        public static event DrawGizmos OnDrawGizmosEventCallback;
        public delegate void DrawGizmosSelected();
        public static event DrawGizmos OnDrawGizmosSelectedEventCallback;
        public delegate void GUI();
        public static event GUI OnGUIEventCallback;
        public delegate void PreRender();
        public static event PreRender OnPreRenderEventCallback;
        public delegate void PostRender();
        public static event PostRender OnPostRenderEventCallback;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            OnDrawGizmosEventCallback?.Invoke();
        }

        private void OnDrawGizmosSelected()
        {
            OnDrawGizmosSelectedEventCallback?.Invoke();
        }

        private void OnGUI()
        {
            OnGUIEventCallback?.Invoke();
        }

        private void OnPreRender()
        {
            OnPreRenderEventCallback?.Invoke();
        }

        private void OnPostRender()
        {
            OnPostRenderEventCallback?.Invoke();
        }
#endif
    }
}