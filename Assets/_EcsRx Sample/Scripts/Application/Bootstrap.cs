using Anonymous.Plugins.Example;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Unity.Framework;
using EcsRx.Unity.Framework.Modules.Plugin;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Anonymous
{
    public class Bootstrap : EcsRxUnityFrameworkApplicationBehaviour
    {
        private Settings settings;

        protected override void ApplicationStarted()
        {
            base.ApplicationStarted();

            var pluginLoader = Container.Resolve<IPluginLoader>();
            pluginLoader.Load<Plugin>();

            if (Application.isMobilePlatform)
                Screen.sleepTimeout = SleepTimeout.NeverSleep;

            settings = Container.Resolve<Settings>();
            Debug.Log($"<color=white>{settings.Name}</color> <color=green>Application Started</color>");
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (Container.NativeContainer is DiContainer nativeContainer)
                if (nativeContainer.HasBinding<Settings>())
                    nativeContainer.Unbind<Settings>();

            Debug.Log($"<color=white>{settings.Name}</color> <color=green>Application Shutdowned</color>");

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void installSettings(string settingsName)
        {
            var installer = Resources.Load(settingsName);
            if (installer != null)
            {
                var _installer = installer as Installer;
                if (_installer != null)
                    if (Container.NativeContainer is DiContainer nativeContainer)
                        nativeContainer.BindInstance(_installer.Settings).IfNotBound();
            }
        }

        protected override void BindSystems()
        {
            base.BindSystems();
            installSettings(Settings.SettingsOrganization);
        }

        protected override void LoadModules()
        {
            base.LoadModules();
            Container.LoadModule<Anonymous.Libraries.Example.Module>();
        }
    }
}