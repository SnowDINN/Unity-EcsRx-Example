using EcsRx.Collections.Database;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.MonoBehaviours
{
    public class EntityDatabaseViewer : MonoBehaviour
    {
         [Inject]
         public IEntityDatabase EntityDatabase { get; private set; }
    }
}