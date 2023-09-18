using System.Collections;
using EcsRx.Unity.Dependencies;
using UnityEngine;

namespace EcsRx.Unity.Framework.Modules.Tool
{
    public class GameObjectTool : IGameObjectTool
    {
        public bool Ready { get; private set; }

        public IEnumerator Initialize()
        {
            yield return null;
            Ready = true;
        }

        public void Shutdown()
        {
        }

        public GameObject InstantiateWithInit(GameObject prefab, Transform parent = null)
        {
            var instance = Object.Instantiate(prefab);
            instance.name = instance.name.Replace("(Clone)", "");
            if (parent != null)
                instance.transform.SetParent(parent);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localEulerAngles = Vector3.zero;
            instance.transform.localScale = Vector3.one;

            return instance;
        }

        public GameObject InstantiateWithInit(IUnityInstantiator instantiator, GameObject prefab,
            Transform parent = null)
        {
            var instance = instantiator.InstantiatePrefab(prefab);
            instance.name = instance.name.Replace("(Clone)", "");
            if (parent != null)
                instance.transform.SetParent(parent);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localEulerAngles = Vector3.zero;
            instance.transform.localScale = Vector3.one;

            return instance;
        }

        public void SetParentWithInit(GameObject go, Transform parent)
        {
            go.transform.SetParent(parent);
            go.transform.localPosition = Vector3.zero;
            go.transform.localEulerAngles = Vector3.zero;
            go.transform.localScale = Vector3.one;
        }
    }
}