using EcsRx.Infrastructure.Extensions;
using EcsRx.Zenject;
using System.Collections;
using UnityEngine;

namespace EcsRx.Unity.Framework
{
    [DefaultExecutionOrder(-20000)]
    public abstract class EcsRxUnityFrameworkApplicationBehaviour : EcsRxApplicationBehaviour
    {
        protected override void BindSystems()
        {
            base.BindSystems();

            Container.BindApplicableSystems(
                "EcsRx.Unity.Framework.Systems",
                "EcsRx.Unity.Framework.ViewResolvers");
        }

        protected override void LoadModules()
        {
            base.LoadModules();

            Container.LoadModule<Modules.Tool.Module>();
            Container.LoadModule<Modules.Plugin.Module>();
        }

        protected override IEnumerator ApplicationStartedAsync()
        {
            yield return Container.InitializeModules();
            ApplicationStarted();
        }

        protected override void ApplicationStarted()
        {
            Started = true;
        }

        protected virtual void OnDestroy()
        {
            if (Container.HasBinding<Modules.Plugin.IPluginLoader>())
            {
                var pluginLoader = Container.Resolve<Modules.Plugin.IPluginLoader>();
                pluginLoader.UnloadAll();
            }

            Container.UnloadModules();
            StopAndUnbindAllSystems();
        }
    }
}