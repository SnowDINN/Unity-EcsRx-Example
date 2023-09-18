using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;

namespace EcsRx.Unity.Framework.Modules.Tool
{
    public class Module : IDependencyModule
    {
        private readonly List<Type> bindings = new()
        {
            typeof(IGameObjectTool)
        };

        public void Setup(IDependencyContainer container)
        {
            container.Bind<IGameObjectTool, GameObjectTool>();
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