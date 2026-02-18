using System;
using System.Diagnostics;

namespace APX.Extra.OdinExtensions.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    [Conditional("UNITY_EDITOR")]
    public class ScriptIconAttribute : Attribute
    {
        public EditorIconsBundle? Bundle { get; }
        public string IconName { get; }
        public bool Inherit { get; }
        
        public ScriptIconAttribute(string iconName, bool inherit = false)
        {
            if(EditorIconsUtils.TryMatchPrefix(iconName, out var bundle, out var iconNameWithoutPrefix))
            {
                Bundle = bundle;
                IconName = iconNameWithoutPrefix;
            }
            else
            {
                Bundle = null;
                IconName = iconName;
            }
            Inherit = inherit;
        }

        public ScriptIconAttribute(EditorIconsBundle bundle, string iconName, bool inherit = false)
        {
            Bundle = bundle;
            IconName = iconName;
            Inherit = inherit;
        }
        
        public ScriptIconAttribute(FontAwesomeEditorIconType iconType, bool inherit = false) : this(EditorIconsBundle.FontAwesome, iconType.ToString(), inherit)
        {
            
        }
    }
}
