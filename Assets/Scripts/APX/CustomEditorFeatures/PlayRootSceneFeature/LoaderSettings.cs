#if UNITY_EDITOR
using System.Collections.Generic;
using APX.Extra.OdinExtensions.Attributes;
using APX.Util;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace APX.CustomEditorFeatures.PlayRootSceneFeature
{
    [GlobalConfig(SettingsConstants.SETTINGS_ASSET_PATH)]
    public class LoaderSettings : GlobalConfig<LoaderSettings>
    {
        [SerializeField]
        [SceneReference(SceneReferenceType.Path, true)]
        private string _RootScene;

        public static bool IsRootSceneSet => !string.IsNullOrEmpty(RootScene);

        public static string RootScene
        {
            get => Instance._RootScene;
            set => Instance._RootScene = value;
        }

        private static string SettingsPath => "Project/APX/Settings/Loader";
        private static IEnumerable<string> Keywords => new[] { "Loader", "Loading", "Game", "RootScene" };

        [MenuItem("APX/Settings/Loader")]
        public static void OpenLoaderSettings()
        {
            SettingsService.OpenProjectSettings(SettingsPath);
        }

        [SettingsProvider]
        protected static SettingsProvider RegisterSettingsProvider()
        {
            return Instance == null ? null : AssetSettingsProvider.CreateProviderFromObject(SettingsPath, Instance, Keywords);
        }
    }
}
#endif
