using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EcsRx.Unity.Framework.Modules.Plugin
{
    public class Module : IDependencyModule
    {
        readonly List<Type> bindings = new List<Type>()
        {
            typeof(IPluginLoader),
        };

        public void Setup(IDependencyContainer container)
        {
            container.Bind<IPluginLoader, PluginLoader>();
        }

        public IEnumerator Initialize(IDependencyContainer container)
        {
            return bindings.Select(bind => ((IModule)container.Resolve(bind)).Initialize()).GetEnumerator();
        }

        public void Shutdown(IDependencyContainer container)
        {
            foreach (var bind in bindings)
            {
                ((IModule)container.Resolve(bind)).Shutdown();
                container.Unbind(bind);
            }
        }
    }
}
