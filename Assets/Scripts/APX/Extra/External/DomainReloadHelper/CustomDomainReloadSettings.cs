#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace APX.Extra.External.DomainReloadHelper
{
    public class CustomDomainReloadSettings : ScriptableObject
    {
        [SerializeField]
        private bool _ReloadOnPlayModeExit = true;
        public static bool ReloadOnPlayModeExit => Instance._ReloadOnPlayModeExit;

        private const string SETTINGS_PATH = "Project/APX/Settings/Domain Reload";
        private static IEnumerable<string> Keywords => new[] {"Domain", "Reload", "PlayMode"};

        private static CustomDomainReloadSettings _instance;
        private static CustomDomainReloadSettings Instance => _instance ??= GetOrCreateInstance();

        private static CustomDomainReloadSettings GetOrCreateInstance()
        {
            var guids = AssetDatabase.FindAssets("t:CustomDomainReloadSettings");
            if (guids.Length > 0)
            {
                _instance = AssetDatabase.LoadAssetAtPath<CustomDomainReloadSettings>(AssetDatabase.GUIDToAssetPath(guids[0]));
            }

            if (_instance) return _instance;

            _instance = CreateInstance<CustomDomainReloadSettings>();
            AssetDatabase.CreateAsset(_instance, "Assets/CustomDomainReloadSettings.asset");
            AssetDatabase.SaveAssets();

            return _instance;
        }

        [SettingsProvider]
        private static SettingsProvider RegisterSettingsProvider()
        {
            return Instance == null ? null : AssetSettingsProvider.CreateProviderFromObject(SETTINGS_PATH, Instance, Keywords);
        }
    }
}
#endif
