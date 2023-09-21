using System;
using UnityEngine;
using Zenject;

namespace Anonymous
{
    [Serializable]
    public class Settings
    {
        public const string Organization = "EcsRx Example";
        public const string SettingsOrganization = "Anonymous" + Organization + "Settings";
        public const string SettingsPath = "Anonymous/" + Organization + "/Settings";
        
        public string Name => Organization;
    }

    [CreateAssetMenu(fileName = Settings.SettingsOrganization, menuName = Settings.SettingsPath)]
    public class Installer : ScriptableObjectInstaller<Installer>
    {
#pragma warning disable 0649
        [SerializeField] public Settings Settings;
#pragma warning restore 0649

        public override void InstallBindings()
        {
            Container.BindInstance(Settings).IfNotBound();
        }
    }
}