using System;
using APX.Extra.OdinExtensions.Attributes;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace APX.Extra.OdinExtensions.Editor.Drawers
{
    /// <summary>
    /// Odin drawer for <see cref="T:Sirenix.OdinInspector.ColorPaletteAttribute" />.
    /// </summary>
    [DrawerPriority(DrawerPriorityLevel.AttributePriority)]
    public sealed class ColoredRectAttributeDrawer : OdinAttributeDrawer<ColoredRectAttribute>
    {
        private ValueResolver<Color> colorResolver;

        protected override void Initialize()
        {
            base.Initialize();
            colorResolver = ValueResolver.Get<Color>(Property, Attribute.GetColor, Attribute.Color);
        }

        /// <summary>Not yet documented.</summary>
        protected override void DrawPropertyLayout(GUIContent label)
        {
            GUILayout.Space(Attribute.SpaceBefore);
            
            InspectorProperty property = Property;
            ColoredRectAttribute attribute = Attribute;
            if (attribute.GetColor == null)
            {
                Draw(label, attribute.Color);
            }
            else
            {
                if (colorResolver.HasError)
                {
                    SirenixEditorGUI.ErrorMessageBox(colorResolver.ErrorMessage);
                    CallNextDrawer(label);
                }
                else
                {
                    Draw(label, colorResolver.GetValue());
                }
            }
            GUILayout.Space(Attribute.SpaceAfter);
        }


        private void Draw(GUIContent label, Color color)
        {
            Rect rect;
            Rect rect2;
            float width = Attribute.Width <= 0 ? 16 : Attribute.Width;
            switch (Attribute.Mode)
            {
                case ColoredRectAttribute.DisplayMode.Left:
                    rect = EditorGUILayout.BeginHorizontal();

                    rect2 = GetRect(width, rect.height);
                    rect2.y = rect.y;
                    SirenixEditorGUI.DrawSolidRect(rect2, color, false);

                    EditorGUILayout.BeginVertical();
                    CallNextDrawer(label);
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.EndHorizontal();
                    break;

                case ColoredRectAttribute.DisplayMode.Right:
                    rect = EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.BeginVertical();
                    CallNextDrawer(label);
                    EditorGUILayout.EndVertical();

                    rect2 = GetRect(width, rect.height);
                    rect2.y = rect.y;
                    SirenixEditorGUI.DrawSolidRect(rect2, color, false);

                    EditorGUILayout.EndHorizontal();
                    break;

                case ColoredRectAttribute.DisplayMode.Top:
                    rect = GetRect();
                    SirenixEditorGUI.DrawSolidRect(rect, color, false);

                    CallNextDrawer(label);
                    break;

                case ColoredRectAttribute.DisplayMode.Bottom:
                    CallNextDrawer(label);

                    rect = GetRect();
                    SirenixEditorGUI.DrawSolidRect(rect, color, false);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Rect GetRect(float width = 0, float height = 0)
        {
            if (Attribute.Width != 0) width = Attribute.Width;
            if (Attribute.Height != 0) height = Attribute.Height;
            Rect rect;
            if (width == 0)
            {
                if (height == 0)
                {
                    return EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false));
                }
                else
                {
                    rect = EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false, GUILayout.Height(height)));
                    rect.height = height;
                    return rect;
                }
            }
            else
            {
                if (height == 0)
                {
                    return EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false, GUILayout.Width(width)));
                }
                else
                {
                    rect = EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false, GUILayout.Width(width), GUILayout.Height(height)));
                    rect.height = height;
                    return rect;
                }
            }
        }
    }
}
