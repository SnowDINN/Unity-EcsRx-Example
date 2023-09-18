using System.Collections;
using UnityEngine;

namespace Anonymous.Libraries.Example
{
    public class ExampleModule : IExampleModule
    {
        public bool Ready { get; set; }

        public IEnumerator Initialize()
        {
            yield return null;

            Debug.Log($"<color=green>Module Loaded</color>");
            Ready = true;
        }

        public void Shutdown()
        {
            Debug.Log($"<color=green>Module UnLoaded</color>");
            Ready = false;
        }

        public void LoadModule()
        {

        }
    }
}