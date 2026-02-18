#if UNITY_EDITOR

using System;
using System.Diagnostics;
using APX.Extra.OdinExtensions;

namespace APX.Extra.GameManagement.Runtime
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    [Conditional("UNITY_EDITOR")]
    public class DynamicManagementGlobalObjectAttribute : Attribute
    {
        public EditorIconsBundle? IconBundle;
        public string IconName;
        public string PanelPath;
        public int Priority;

        public DynamicManagementGlobalObjectAttribute(string panelPath, string iconName, int priority = 0)
        {
            PanelPath = panelPath;
            Priority = priority;
            IconName = iconName;
        }

        public DynamicManagementGlobalObjectAttribute(string panelPath, EditorIconsBundle bundle, string iconName, int priority = 0) : this(panelPath, iconName, priority)
        {
            IconBundle = bundle;
        }

        public DynamicManagementGlobalObjectAttribute(string panelPath, FontAwesomeEditorIconType iconType, int priority = 0) :
            this(panelPath, EditorIconsBundle.FontAwesome, iconType.ToString(), priority)
        {

        }
    }
}

#endif