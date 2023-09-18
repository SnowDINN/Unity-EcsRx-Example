using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Scheduling;
using EcsRx.Unity.Scheduling;
using System.Collections;

namespace EcsRx.Unity.Modules
{
    public class UnityOverrideModule : IDependencyModule 
    {
        public void Setup(IDependencyContainer container)
        {
            container.Unbind<IUpdateScheduler>();
            container.Bind<IUpdateScheduler, UnityUpdateScheduler>(x => x.AsSingleton());
        }

        public IEnumerator Initialize(IDependencyContainer container)
        {
            yield break;
        }

        public void Shutdown(IDependencyContainer container)
        {
        }
    }
}