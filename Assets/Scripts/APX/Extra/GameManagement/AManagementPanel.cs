#if UNITY_EDITOR

using System;
using APX.Extra.EditorUtils;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace APX.Extra.GameManagement
{
    [Serializable]
    public abstract class AManagementPanel
    {
        public abstract string PanelPath { get; }
        public abstract Texture PanelIcon { get; }
        public virtual float Priority => 0;

        public bool IsValid => !string.IsNullOrWhiteSpace(PanelPath);

        public string PanelName => PanelPath.Substring(PanelPath.LastIndexOf('/') + 1);

        public GameManagementWindow CurrentWindow { get; internal set; }

        [PropertyOrder(-10000)]
        [OnInspectorGUI("OnTitleGUI", "OnEndGUI")]
        protected virtual void OnTitleGUI()
        {
            GUILayout.Space(4);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(8);
            var titleStyle = new GUIStyle(SirenixGUIStyles.BoldTitle) {fontSize = 25};
            EditorGUILayout.LabelField(GUIHelper.TempContent($" {PanelName}", PanelIcon), titleStyle, GUILayout.Height(30));
            var labelRect = GUILayoutUtility.GetLastRect();
            if (Event.current.OnMouseDown(labelRect, 1))
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Edit Script"), false, () =>
                {
                    if ((this is IObjectManagementPanel assetPanel && assetPanel.SourceObject.GetType().TryGetMonoScript(out var monoScript)) || GetType().TryGetMonoScript(out monoScript))
                    {
                        AssetDatabase.OpenAsset(monoScript);
                    }
                });
                menu.ShowAsContext();
            }

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(8);

            OnBeginGUI();
        }

        protected virtual void OnBeginGUI() { }
        protected virtual void OnEndGUI() { }

        public virtual void OnPanelSelected() { }
    }
}

#endif