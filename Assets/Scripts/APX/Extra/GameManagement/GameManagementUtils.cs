#if UNITY_EDITOR

using System.Reflection;
using APX.Extra.GameManagement.Runtime;
using APX.Extra.OdinExtensions;

namespace APX.Extra.GameManagement
{
    public static class GameManagementUtils
    {
        public static bool TryConvertToPanel(object obj, out DynamicManagementPanel panel)
        {
            panel = null;
            var attribute = obj?.GetType().GetCustomAttribute<DynamicManagementGlobalObjectAttribute>(true);
            if (attribute == null) return false;

            panel = ConvertToPanel(obj, attribute);
            return true;
        }

        public static DynamicManagementPanel ConvertToPanel(object obj, DynamicManagementGlobalObjectAttribute attribute)
        {
            var icon = EditorIconsUtility.GetIcon(attribute.IconName, attribute.IconBundle);
            return new DynamicManagementPanel(obj, attribute.PanelPath, icon.Highlighted, attribute.Priority);
        }
    }
}

#endif