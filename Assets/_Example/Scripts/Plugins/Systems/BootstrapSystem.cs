using EcsRx.Events;
using EcsRx.Groups;
using EcsRx.Groups.Observable;
using EcsRx.Systems;
using UniRx;
using UnityEngine;

namespace Anonymous.Plugins.Example.Systems
{
    public class BootstrapSystem : IManualSystem
    {
        private readonly IEventSystem eventSystem;
        private readonly Settings settings;

        private readonly CompositeDisposable subscriptions = new();

        public BootstrapSystem(IEventSystem eventSystem,
            Settings settings)
        {
            this.eventSystem = eventSystem;
            this.settings = settings;
        }

        public IGroup Group => new EmptyGroup();

        public void StartSystem(IObservableGroup observableGroup)
        {
            Debug.Log($"<color=green>Bootstrap System</color> <color=white>{settings.Name}</color>");
        }

        public void StopSystem(IObservableGroup observableGroup)
        {
            subscriptions.Dispose();
            Debug.Log($"<color=green>Unload Bootstrap System</color> <color=white>{settings.Name}</color>");
        }
    }
}