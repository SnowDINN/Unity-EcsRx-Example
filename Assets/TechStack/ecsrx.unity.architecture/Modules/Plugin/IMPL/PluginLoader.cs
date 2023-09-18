using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Extensions;
using EcsRx.Infrastructure.Plugins;
using EcsRx.Zenject;

namespace EcsRx.Unity.Framework.Modules.Plugin
{
    public class PluginLoader : IPluginLoader
    {
        private readonly Dictionary<Type, IEcsRxPlugin> plugins = new();
        public bool Ready { get; private set; }

        public IEnumerator Initialize()
        {
            yield return null;
            Ready = true;
        }

        public void Shutdown()
        {
            plugins.ForEachRun(kv => { Unload(kv.Value, false); });

            plugins.Clear();
        }

        public T Load<T>() where T : IEcsRxPlugin, new()
        {
            IEcsRxPlugin plugin = new T();

            var container = EcsRxApplicationBehaviour.Instance.Container;
            var systemExecutor = EcsRxApplicationBehaviour.Instance.SystemExecutor;
            plugin.SetupDependencies(container);
            var systems = plugin.GetSystemsForRegistration(container);
            foreach (var system in systems) systemExecutor.AddSystem(system);

            plugins.Add(typeof(T), plugin);
            return (T)plugin;
        }

        public void Unload<T>() where T : IEcsRxPlugin
        {
            var type = typeof(T);
            if (plugins.ContainsKey(type))
            {
                var plugin = plugins.FirstOrDefault(x => x.Value.GetType() == type).Value;
                Unload(plugin);
            }
        }

        public void Unload(IEcsRxPlugin plugin, bool remove = true)
        {
            if (plugin != null)
            {
                var container = EcsRxApplicationBehaviour.Instance.Container;
                var systemExecutor = EcsRxApplicationBehaviour.Instance.SystemExecutor;
                plugin.GetSystemsForRegistration(container)
                    .ForEachRun(x => systemExecutor.RemoveSystem(x));
                plugin.UnsetupDependencies(container);
                if (remove)
                    plugins.Remove(plugin.GetType());
            }
        }

        public void UnloadAll()
        {
            var pluginsCopy = plugins.ToList();
            pluginsCopy.ForEachRun(plugin => { Unload(plugin.Value); });
        }
    }
}