using System.Collections;

namespace EcsRx.Unity.Framework
{
    public interface IModule
    {
        bool Ready { get; }
        IEnumerator Initialize();
        void Shutdown();
    }
}