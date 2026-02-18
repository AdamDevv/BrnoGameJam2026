using APX.Extra.OdinExtensions.Attributes;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace APX.Extra.OdinExtensions.Editor.Drawers
{
    [DrawerPriority(DrawerPriorityLevel.WrapperPriority)]
    public class PasswordFieldAttributeDrawer : OdinAttributeDrawer<PasswordFieldAttribute, string>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            ValueEntry.SmartValue = EditorGUILayout.PasswordField(label, ValueEntry.SmartValue);
        }
    }
}
