using System;
using System.Collections.Generic;
using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Infrastructure.Plugins;
using EcsRx.Systems;
using UnityEngine;
using Zenject;

namespace Anonymous.Plugins.Example
{
    public class Plugin : IEcsRxPlugin
    {
        public const string PluginName = "Example";
        public const string SettingsName = "Anonymous" + PluginName + "PluginExampleSettings";

        public const string SettingsPath =
            "Anonymous/" + Settings.Organization + "/Plugins/Example/" + PluginName + "/Settings";

        public const string SystemNamespace = "Anonymous" + ".Plugins." + PluginName + ".Systems";

        public string Name => PluginName;
        public Version Version => new(0, 1, 0);

        public void SetupDependencies(IDependencyContainer container)
        {
            installSettings(container);
            container.BindApplicableSystems(SystemNamespace);
        }

        public void UnsetupDependencies(IDependencyContainer container)
        {
            container.UnbindApplicableSystems(SystemNamespace);
            uninstallSettings(container);
        }

        public IEnumerable<ISystem> GetSystemsForRegistration(IDependencyContainer container)
        {
            return container.ResolveApplicableSystems(SystemNamespace);
        }

        private void installSettings(IDependencyContainer container)
        {
            var installer = Resources.Load(SettingsName);
            if (installer != null)
            {
                var _installer = installer as Installer;
                if (_installer != null)
                {
                    if (container.NativeContainer is DiContainer nativeContainer)
                        nativeContainer.BindInstance(_installer.Settings).IfNotBound();
                }
            }
        }

        private void uninstallSettings(IDependencyContainer container)
        {
            if (container.NativeContainer is DiContainer nativeContainer)
                if (nativeContainer.HasBinding<Settings>())
                    nativeContainer.Unbind<Settings>();
        }
    }
}