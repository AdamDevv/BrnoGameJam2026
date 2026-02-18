using APX.Extra.OdinExtensions.Attributes;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace APX.Extra.OdinExtensions.Editor.Drawers
{
    [DrawerPriority(DrawerPriorityLevel.SuperPriority)]
    public class ShowInFieldsOfTypeAttributeDrawer : OdinAttributeDrawer<ShowInFieldsOfTypeAttribute>
    {
        protected override void Initialize()
        {
            var fieldType = Property.Parent?.Info?.TypeOfValue;
            Property.State.Visible = fieldType == null || OdinExtensionUtils.IsAssignable(fieldType, Attribute.FieldTypes, Attribute.IncludeChildTypes);
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (Property.State.Visible)
            {
                CallNextDrawer(label); 
            }
        }
    }
}
