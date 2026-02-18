using APX.Extra.OdinExtensions.Attributes;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace APX.Extra.OdinExtensions.Editor.Drawers
{

    [DrawerPriority(0.5, 0.0, 0.0)]
    public sealed class GUIBackgroundColorAttributeDrawer : OdinAttributeDrawer<GUIBackgroundColorAttribute>
    {
        private ValueResolver<Color> _colorResolver;

        protected override void Initialize()
        {
            _colorResolver = ValueResolver.Get(Property, Attribute.GetColor, Attribute.Color);
        }

        /// <summary>Draws the property.</summary>
        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (_colorResolver.HasError)
            {
                SirenixEditorGUI.ErrorMessageBox(_colorResolver.ErrorMessage);
                CallNextDrawer(label);
            }
            else
            {
                var previousColor = GUI.backgroundColor;
                GUI.backgroundColor = _colorResolver.GetValue();
                CallNextDrawer(label);
                GUI.backgroundColor = previousColor;
            }
        }
    }
}
