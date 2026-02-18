#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace APX.CustomEditorFeatures
{
    internal static class EditorTabsShortcuts
    {
        private const BindingFlags MAX_BINDING_FLAGS = (BindingFlags) 62;

        private static readonly FieldInfo _dockAreaFieldInfo;
        private static readonly FieldInfo _panesFieldInfo;

        static EditorTabsShortcuts()
        {
            _dockAreaFieldInfo = typeof(EditorWindow).GetField("m_Parent", MAX_BINDING_FLAGS);
            _panesFieldInfo = Type.GetType("UnityEditor.DockArea, UnityEditor")!.GetField("m_Panes", MAX_BINDING_FLAGS);
        }

        [MenuItem("APX/Util/Toggle Selected Lock %e")] // Ctrl + E
        private static void ToggleSelectedTabLock()
        {
            var window = EditorWindow.focusedWindow;

            var isLockedProperty =
                window.GetType().GetProperty("isLocked") ??
                window.GetType().GetProperty("isLocked", BindingFlags.NonPublic | BindingFlags.Instance);

            if (isLockedProperty is null)
            {
                return;
            }

            var isLocked = (bool) isLockedProperty.GetValue(window);
            isLockedProperty.SetValue(window, !isLocked);
        }

        [MenuItem("APX/Util/Select Next Tab %TAB")] // Ctrl + Tab
        private static void SelectNextTab() => SelectNextTabByDir(1);

        [MenuItem("APX/Util/Select Previous Tab %#TAB")] // Ctrl + Shift + Tab
        private static void SelectPreviousTab() => SelectNextTabByDir(-1);

        private static void SelectNextTabByDir(int dir)
        {
            var window = EditorWindow.focusedWindow;

            if (!TryGetTabList(window, out List<EditorWindow> tabs))
            {
                return;
            }

            var selectedTabIndex = tabs.IndexOf(window);

            if (selectedTabIndex == -1)
            {
                return;
            }

            var newTabIndex = (selectedTabIndex + dir + tabs.Count) % tabs.Count;

            tabs[newTabIndex]?.Focus();
        }

        private static bool TryGetTabList(EditorWindow window, out List<EditorWindow> tabs)
        {
            tabs = null;
            var result = true;

            try
            {
                object dockArea = _dockAreaFieldInfo.GetValue(window);
                object panes = _panesFieldInfo.GetValue(dockArea);
                tabs = (List<EditorWindow>) panes;
            }
            catch (Exception)
            {
                result = false;
            }

            if (tabs is null)
            {
                result = false;
            }

            return result;
        }
    }
}

#endif
