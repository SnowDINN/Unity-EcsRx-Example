using EcsRx.Collections;
using EcsRx.Collections.Database;
using EcsRx.Executor;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.MonoBehaviours
{
    public class ActiveSystemsViewer : MonoBehaviour
    {
        [Inject]
        public ISystemExecutor SystemExecutor { get; private set; }

        [Inject]
        public IEntityDatabase EntityDatabase { get; private set; }

        [Inject]
        public IObservableGroupManager ObservableGroupManager { get; private set; }
    }
}