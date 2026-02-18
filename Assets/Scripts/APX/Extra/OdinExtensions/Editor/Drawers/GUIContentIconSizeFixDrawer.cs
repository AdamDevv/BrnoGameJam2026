namespace APX.Extra.OdinExtensions.Editor.Drawers
{
    // Should fix the resizing of icons in the inspector but it breaks a lot of other stuff, like info messages currently

    /*
    [DrawerPriority(-1000)]
    public class GUIContentIconSizeFixDrawer : OdinDrawer
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var prev = EditorGUIUtility.GetIconSize();
            EditorGUIUtility.SetIconSize(new Vector2(12, 12));
            CallNextDrawer(label);
            EditorGUIUtility.SetIconSize(prev);
        }

        public override bool CanDrawProperty(InspectorProperty property)
        {
            var valueEntry = property.ValueEntry;
            return valueEntry != null && !property.IsTreeRoot;
        }
    }
    */
}
