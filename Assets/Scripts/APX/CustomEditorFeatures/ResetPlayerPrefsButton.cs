#if UNITY_EDITOR

using APX.Extra.EditorUtils.ToolbarExtensions;
using APX.Extra.OdinExtensions;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace APX.CustomEditorFeatures
{
    internal static class ResetPlayerPrefsButton
    {
        private static EditorIcon GameManagementIcon => FontAwesomeEditorIcons.FloppyDiskCircleXmarkSolid;

        [InitializeOnLoadMethod]
        private static void InitializeToolbar()
        {
            ToolbarExtender.AddToLeftToolbar(OnToolbarGUI, 0);
        }

        private static void OnToolbarGUI()
        {
            GUILayout.Space(2);

            if (GUILayout.Button(new GUIContent(null, GameManagementIcon.Highlighted, "Delete Player Prefs"), ToolbarStyles.ToolbarButton,
                    GUILayout.Width(30)))
            {
                PlayerPrefs.DeleteAll();
                Debug.Log("Player Prefs have been reset.");
            }

            GUILayout.Space(2);
        }
    }
}

#endif