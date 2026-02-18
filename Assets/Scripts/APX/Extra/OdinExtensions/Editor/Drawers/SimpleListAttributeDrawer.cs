using APX.Extra.OdinExtensions.Attributes;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace APX.Extra.OdinExtensions.Editor.Drawers
{
    public class SimpleListAttributeDrawer : OdinAttributeDrawer<SimpleListAttribute>
    {
        private readonly GUIStyle _listItemStyle = new(GUIStyle.none)
        {
            padding = new RectOffset(5, 5, 3, 3)
        };

        protected override bool CanDrawAttributeProperty(InspectorProperty property)
        {
            return property.ChildResolver is IOrderedCollectionResolver;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            SirenixEditorGUI.BeginIndentedVertical(SirenixGUIStyles.PropertyMargin);
            SirenixEditorGUI.BeginVerticalList();
            foreach (var child in Property.Children)
            {
                SirenixEditorGUI.BeginListItem(true, _listItemStyle);
                child.Draw(null);
                SirenixEditorGUI.EndListItem();
            }
            SirenixEditorGUI.EndVerticalList();
            SirenixEditorGUI.EndIndentedVertical();
        }
    }
}
