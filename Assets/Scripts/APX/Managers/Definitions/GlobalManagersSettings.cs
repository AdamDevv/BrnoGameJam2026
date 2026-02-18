using System.Collections.Generic;
using APX.Managers.GameObjects;
using APX.Util;
using Sirenix.Utilities;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace APX.Managers.Definitions
{
    [GlobalConfig(SettingsConstants.SETTINGS_ASSET_PATH)]
    public class GlobalManagersSettings : GlobalConfig<GlobalManagersSettings>
    {
        [SerializeField] private ASingleton[] _Managers;


        public ASingleton[] Managers => _Managers;

#if UNITY_EDITOR
        private const string SETTINGS_PATH = "Project/APX/Settings/Global Managers";
        private static readonly IEnumerable<string> KEYWORDS = new[] { "Managers", "GlobalManagers" };

        [MenuItem("APX/Settings/Global Managers")]
        public static void OpenLoaderSettings()
        {
            SettingsService.OpenProjectSettings(SETTINGS_PATH);
        }

        [SettingsProvider]
        protected static SettingsProvider RegisterSettingsProvider()
        {
            return Instance == null ? null : AssetSettingsProvider.CreateProviderFromObject(SETTINGS_PATH, Instance, KEYWORDS);
        }
#endif
    }
}
