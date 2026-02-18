#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using APX.Extra.EditorUtils.SelectionHistory;
using APX.Extra.EditorUtils.ToolbarExtensions;
using APX.Extra.Misc;
using APX.Extra.OdinExtensions;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using SerializationUtility = Sirenix.Serialization.SerializationUtility;

namespace APX.Extra.GameManagement
{
    public class GameManagementWindow : OdinMenuEditorWindow, IHasCustomMenu
    {
        private static IEnumerable<AManagementPanel> _panelTemplates;
        private static IEnumerable<AManagementPanel> PanelTemplates => _panelTemplates ??= ReflectionHelper.GetTypeCacheDerivedClassesOfType<AManagementPanel>()
            .Where(p => p.IsValid)
            .OrderBy(p => p.Priority)
            .ThenBy(p => p.PanelPath);

        private List<AManagementPanel> _panels;
        public List<AManagementPanel> Panels => _panels ??= PanelTemplates.Select(p =>
            {
                var pan = (AManagementPanel) SerializationUtility.CreateCopy(p);
                pan.CurrentWindow = this;
                return pan;
            })
            // .Concat(GetGlobalDataPanels())
            .ToList();

        private HistoryBuffer<AManagementPanel> _panelHistory = new HistoryBuffer<AManagementPanel>();

        private static EditorIcon GameManagementIcon => FontAwesomeEditorIcons.GamepadModernSolid;

        [InitializeOnLoadMethod]
        private static void InitializeToolbar()
        {
            ToolbarExtender.AddToLeftToolbar(OnToolbarGUI, -460);
        }

        private static void OnToolbarGUI()
        {
            GUILayout.Space(2);
            if (GUILayout.Button(new GUIContent(null, GameManagementIcon.Highlighted, "Open Game Management"), ToolbarStyles.ToolbarButton, GUILayout.Width(30)))
            {
                OpenWindow();
            }
            GUILayout.Space(2);
        }

        [MenuItem("APX/Game Management", priority = 2000)]
        public static void OpenWindow()
        {
            var window = GetWindow<GameManagementWindow>();
            window.SetupWindowStyle();
        }

        public static GameManagementWindow GetWindow()
        {
            if (focusedWindow is GameManagementWindow gameManagementWindow)
                return gameManagementWindow;

            var window = GetWindow<GameManagementWindow>();
            window.SetupWindowStyle();
            return window;
        }

        public bool TrySelectPanel<TPanel>(Predicate<TPanel> predicate = null)
            => TrySelectPanel(out _, predicate);

        public bool TrySelectPanel<TPanel>(out TPanel resultPanel, Predicate<TPanel> predicate = null)
        {
            ForceMenuTreeRebuild();
            var selectionPanel = MenuTree.EnumerateTree().FirstOrDefault(i => i.Value is TPanel tPanel && (predicate == null || predicate(tPanel)));
            if (selectionPanel == null || !TrySelectPanel(selectionPanel))
            {
                resultPanel = default;
                return false;
            }

            resultPanel = (TPanel) selectionPanel.Value;
            return true;
        }

        public static bool HasSelectorPanelFor(ScriptableObject value)
        {
            return value != null && PanelTemplates.Any(p => p is ISelectorManagementPanel selectorPanel && selectorPanel.CanSelect(value));
        }

        private bool TrySelectPanel(AManagementPanel panel, bool recordSelection = true)
        {
            var selectionPanel = MenuTree.EnumerateTree().FirstOrDefault(i => i.Value == panel);
            return TrySelectPanel(selectionPanel, recordSelection);
        }

        private bool TrySelectPanel(OdinMenuItem menuItem, bool recordSelection = true)
        {
            if (menuItem == null)
            {
                return false;
            }
            if (!recordSelection) MenuTree.Selection.SelectionChanged -= OnSelectionChanged;
            MenuTree.Selection.Clear();
            MenuTree.Selection.Add(menuItem);
            if (!recordSelection) MenuTree.Selection.SelectionChanged += OnSelectionChanged;

            return true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            SetupWindowStyle();

            _panelHistory.Add(Panels[0]);

            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnFocus()
        {
            HistoryButtonsHandler.AddListener(OnHistoryButtonPressed, -5);
        }

        private void OnLostFocus()
        {
            HistoryButtonsHandler.RemoveListener(OnHistoryButtonPressed);
        }

        private void OnHistoryButtonPressed(HistoryButtonType buttonType)
        {
            switch (buttonType)
            {
                case HistoryButtonType.Backward: SelectPrevious(); break;
                case HistoryButtonType.Forward: SelectNext(); break;
            }
        }
        
        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            EditorApplication.delayCall += ForceMenuTreeRebuild;
        }
        
        private void SetupWindowStyle()
        {
            titleContent = new GUIContent("Game Management", GameManagementIcon.Highlighted);
        }

        // private static IEnumerable<AManagementPanel> GetGlobalDataPanels()
        // {
        //     var globalDataAssets = GlobalData.GetAllRegisteredObjects();
        //     foreach (var asset in globalDataAssets)
        //     {
        //         if (asset == null)
        //             continue;
        //
        //         if (GameManagementUtils.TryConvertToPanel(asset, out var panel))
        //         {
        //             yield return panel;
        //         }
        //
        //         if (asset is ADefinitionsProvider provider)
        //         {
        //             var definitions = provider.GetAllDefinitions();
        //             foreach (var definition in definitions)
        //             {
        //                 if (GameManagementUtils.TryConvertToPanel(definition, out panel))
        //                 {
        //                     yield return panel;
        //                 }
        //             }
        //         }
        //
        //         var fields = asset.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
        //         foreach (var field in fields)
        //         {
        //             var value = field.GetValue(asset);
        //             if (GameManagementUtils.TryConvertToPanel(value, out panel))
        //             {
        //                 yield return panel;
        //             }
        //         }
        //     }
        // }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.DefaultMenuStyle = OdinMenuStyle.TreeViewStyle.Clone();
            tree.DefaultMenuStyle.IconPadding = 5;

            foreach (var panel in Panels)
            {
                tree.Add(panel.PanelPath, panel, panel.PanelIcon);
            }
            return tree;
        }
        
        public void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Open New Management Window"), false, OnOpenNewWindow);
        }
        
        private void OnOpenNewWindow()
        {
            var window = CreateWindow<GameManagementWindow>();
            window.SetupWindowStyle();
        }

        private void OnSelectionChanged(SelectionChangedType selectionChangedType)
        {
            var currentSelection = MenuTree.Selection.FirstOrDefault();
            if (currentSelection != null && currentSelection.Value is AManagementPanel panel)
            {
                _panelHistory.Add(panel);
                panel.OnPanelSelected();
            }
        }

        protected override void DrawMenu()
        {
            GUILayout.Space(1);
            EditorGUILayout.BeginHorizontal(SirenixGUIStyles.ToolbarBackground, GUILayout.Height(20));
            GUILayout.Space(8);

            EditorGUI.BeginDisabledGroup(!_panelHistory.HasPrevious);
            
            var rect = GUILayoutUtility.GetRect(18, 18, SirenixGUIStyles.Button, GUILayoutOptions.ExpandWidth(false).Width(18));
            if (SirenixEditorGUI.IconButton(rect, FontAwesomeEditorIcons.ArrowLeftSolid, "Step back"))
            {
                SelectPrevious();
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.Space(8);
            EditorGUI.BeginDisabledGroup(!_panelHistory.HasNext);
            
            rect = GUILayoutUtility.GetRect(18, 18, SirenixGUIStyles.Button, GUILayoutOptions.ExpandWidth(false).Width(18));
            if (SirenixEditorGUI.IconButton(rect, FontAwesomeEditorIcons.ArrowRightSolid, "Step forward"))
            {
                SelectNext();
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.Space(6);
            
            MenuTree.DrawSearchToolbar(GUIStyle.none);
            EditorGUILayout.EndHorizontal();
            
            base.DrawMenu();
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginHorizontal(SirenixGUIStyles.ToolbarBackground, GUILayout.Height(20));
            rect = GUILayoutUtility.GetRect(18, 18, SirenixGUIStyles.Button, GUILayoutOptions.ExpandWidth(false).Width(18));
            if (SirenixEditorGUI.IconButton(rect, FontAwesomeEditorIcons.SquarePlusSolid, "Expand All"))
            {
                MenuTree.EnumerateTree(m => m.Toggled = true);
            }
            rect = GUILayoutUtility.GetRect(18, 18, SirenixGUIStyles.Button, GUILayoutOptions.ExpandWidth(false).Width(18));
            if (SirenixEditorGUI.IconButton(rect, FontAwesomeEditorIcons.SquareMinusSolid, "Collapse All"))
            {
                MenuTree.EnumerateTree(m => m.Toggled = false);
            }
            EditorGUILayout.EndHorizontal();

            MenuTree.Selection.SelectionChanged -= OnSelectionChanged;
            MenuTree.Selection.SelectionChanged += OnSelectionChanged;
        }

        private void SelectPrevious()
        {
            if (_panelHistory.TryGetPrevious(out var newPanel))
            {
                TrySelectPanel(newPanel, false);
            }
        }

        private void SelectNext()
        {
            if (_panelHistory.TryGetNext(out var newPanel))
            {
                TrySelectPanel(newPanel, false);
            }
        }
    }
}

#endif