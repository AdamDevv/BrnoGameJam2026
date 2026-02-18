#if UNITY_EDITOR

using System.IO;
using APX.Extra.EditorUtils.ToolbarExtensions;
using APX.Extra.OdinExtensions;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace APX.CustomEditorFeatures.PlayRootSceneFeature
{
    public static class PlayRootScene
    {
        private static readonly string PROJECT_PREFIX = $"Proj{Animator.StringToHash(Application.dataPath)}_";
        private const string EDITOR_PREF_PREVIOUS_SCENE = "SceneAutoLoader.PreviousScene";
        private static readonly string EDITOR_PREF_PREFIX = PROJECT_PREFIX + EDITOR_PREF_PREVIOUS_SCENE;

        private static string PreviousScene
        {
            get => EditorPrefs.GetString(EDITOR_PREF_PREFIX, SceneManager.GetActiveScene().path);
            set => EditorPrefs.SetString(EDITOR_PREF_PREFIX, value);
        }

        [InitializeOnLoadMethod]
        private static void InitializeToolbar()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
            ToolbarExtender.AddToLeftPlayButtons(PlayRootSceneToolbarGUI, 0);
        }

        private static void PlayRootSceneToolbarGUI()
        {
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);

            if (GUILayout.Button(GUIHelper.TempContent(CustomEditorIcons.RootMaster.Active, "Play Root scene"),
                    ToolbarStyles.ToolbarButtonBiggerIcon, GUILayout.Width(30)))
            {
                TryPlayMasterScene();
            }

            EditorGUI.EndDisabledGroup();

            GUILayout.Space(2);
        }

        public static bool TryPlayMasterScene()
        {
            if (!LoaderSettings.IsRootSceneSet)
            {
                SelectMasterScene();
                return false;
            }

            PreviousScene = SceneManager.GetActiveScene().path;
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                if (!EditorSceneManager.OpenScene(LoaderSettings.RootScene).IsValid())
                {
                    bool dialogResult = EditorUtility.DisplayDialog(
                        title: "Master scene not found",
                        message: $"Scene not found:\n{LoaderSettings.RootScene}\n\nWould you like to choose a different master scene?",
                        ok: "Yes",
                        cancel: "No"
                    );

                    if (dialogResult)
                    {
                        SelectMasterScene();
                    }

                    return false;
                }
            }
            else
            {
                // User cancelled the save operation, cancel play as well.
                return false;
            }

            // Start play mode
            EditorApplication.isPlaying = true;

            return true;
        }

        private static void SelectMasterScene()
        {
            var path = EditorUtility.OpenFilePanel("Select Root Scene", Application.dataPath, "unity").Replace(Application.dataPath, "Assets");
            if (!string.IsNullOrEmpty(path) && AssetDatabase.LoadAssetAtPath<SceneAsset>(path) != null)
            {
                LoaderSettings.RootScene = path;
            }
        }

        private static void OnPlayModeChanged(PlayModeStateChange playModeStateChange)
        {
            if (!EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (!string.IsNullOrEmpty(PreviousScene))
            {
                EditorApplication.delayCall += ReloadPreviousSceneOnDelayCall;
            }
        }

        #region Previous scene handling

        private static void ReloadPreviousSceneOnDelayCall()
        {
            EditorApplication.delayCall -= ReloadPreviousSceneOnDelayCall;

            if (!File.Exists(PreviousScene))
            {
                PreviousScene = null;
                return;
            }

            if (!EditorApplication.isPlaying)
            {
                EditorSceneManager.OpenScene(PreviousScene);
                PreviousScene = null;
            }
            else
            {
                EditorApplication.update += ReloadPreviousSceneOncePossible;
            }
        }

        private static void ReloadPreviousSceneOncePossible()
        {
            if (EditorApplication.isPlaying) return;

            if (File.Exists(PreviousScene))
                EditorSceneManager.OpenScene(PreviousScene);
            EditorApplication.update -= ReloadPreviousSceneOncePossible;
            PreviousScene = null;
        }

        #endregion
    }
}


#endif
