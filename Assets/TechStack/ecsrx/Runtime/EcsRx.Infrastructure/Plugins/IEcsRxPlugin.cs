using System;
using System.Collections.Generic;
using EcsRx.Infrastructure.Dependencies;
using EcsRx.Systems;

namespace EcsRx.Infrastructure.Plugins
{
    public interface IEcsRxPlugin
    {
        string Name { get; }
        Version Version { get; }

        void SetupDependencies(IDependencyContainer container);
        void UnsetupDependencies(IDependencyContainer container);
        IEnumerable<ISystem> GetSystemsForRegistration(IDependencyContainer container);
    }
}