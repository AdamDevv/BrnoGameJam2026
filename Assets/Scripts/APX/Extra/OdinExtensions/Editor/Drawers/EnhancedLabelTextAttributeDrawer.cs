using APX.Extra.OdinExtensions.Attributes;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace APX.Extra.OdinExtensions.Editor.Drawers
{
    [DrawerPriority(DrawerPriorityLevel.SuperPriority)]
    public class EnhancedLabelTextAttributeDrawer : OdinAttributeDrawer<EnhancedLabelTextAttribute>
    {
        private ValueResolver<string> textProvider;
        private ValueResolver<Texture> textureProvider;
        private GUIContent overrideLabel;

        protected override void Initialize()
        {
            textProvider = ValueResolver.GetForString(Property, Attribute.Text);
            textureProvider = ValueResolver.Get<Texture>(Property, Attribute.Texture);
            overrideLabel = new GUIContent();
        }

        /// <summary>Draws the attribute.</summary>
        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (textProvider.HasError)
            {
                SirenixEditorGUI.ErrorMessageBox(textProvider.ErrorMessage);
                CallNextDrawer(label);
            }
            else
            {
                string name = textProvider.GetValue();
                GUIContent label1;
                if (name == null)
                {
                    label1 = (GUIContent) null;
                }
                else
                {
                    if (Attribute.NicifyText)
                        name = ObjectNames.NicifyVariableName(name);
                    overrideLabel.text = name;
                    label1 = overrideLabel;
                }

                var texture = textureProvider.GetValue();
                if (!texture)
                {
                    SirenixEditorGUI.ErrorMessageBox(textureProvider.ErrorMessage);
                }
                else if (label1 != null)
                {
                    label1.image = texture;
                }
                CallNextDrawer(label1);
            }
        }
    }
}
