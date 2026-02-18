#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using APX.Extra.EditorUtils;
using APX.Extra.OdinExtensions.Attributes;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;

namespace APX.Extra.GameManagement
{
    [Serializable]
    public abstract class AParentManagementPanel : AManagementPanel
    {
        private List<AManagementPanel> ChildPanels => CurrentWindow.Panels
            .Where(p => p.PanelPath.StartsWith(PanelPath) && p.PanelPath != PanelPath)
            .ToList();

        private OdinMenuTree _menuTree;
        
        [PropertyOrder(-900)]
        [OnInspectorGUI]
        [HorizontalGroup("Data", 280)]
        [EnhancedBoxGroup("Data/Menu")]
        private void DrawMenu()
        {
            if (_menuTree == null)
            {
                var style = OdinMenuStyle.TreeViewStyle.Clone();
                style.DrawFoldoutTriangle = false;
                _menuTree = new OdinMenuTree(false, style);

                foreach (var panel in ChildPanels)
                {
                    if (panel == null) continue;
                    _menuTree.Add(panel.PanelPath.Substring(PanelPath.Length), panel, panel.PanelIcon);
                }

                _menuTree.Config.UseCachedExpandedStates = false;
                _menuTree.EnumerateTree().ForEach(e => e.Toggled = true);
                _menuTree.Selection.SelectionChanged += type =>
                {
                    CurrentWindow.TrySelectMenuItemWithObject(_menuTree.Selection.FirstOrDefault()?.Value);
                };
            }

            _menuTree.Selection.Clear();
            _menuTree.DrawMenuTree();
            _menuTree.HandleKeyboardMenuNavigation();
        }

        [OnInspectorInit]
        private void OnInspectorInit()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        [OnInspectorDispose]
        private void OnInspectorDispose()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            WaitInEditor.ForNextFrame(() => _menuTree = null);
        }
    }
}

#endif