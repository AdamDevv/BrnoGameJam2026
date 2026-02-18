using APX.Extra.OdinExtensions.Attributes;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace APX.Extra.OdinExtensions.Editor.Drawers
{
    [DrawerPriority(DrawerPriorityLevel.WrapperPriority)]
    public class ShowGetComponentAttributeDrawer<T> : OdinAttributeDrawer<ShowGetComponentAttribute, T> where T : Component
    {
        private Component _parentComponent;
        private bool _isValid;

        protected override void Initialize()
        {
            base.Initialize();
            _isValid = typeof(Component).IsAssignableFrom(Property.Info.TypeOfOwner);
            _parentComponent = Property.SerializationRoot.ValueEntry.WeakSmartValue as Component;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (_isValid)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    CallNextDrawer(label);
                    var rect = GUILayoutUtility.GetRect(18, 18, SirenixGUIStyles.Button, GUILayoutOptions.ExpandWidth(false).Width(18));
                    if (SirenixEditorGUI.IconButton(rect, FontAwesomeEditorIcons.MagnifyingGlassSolid, "Get component"))
                    {
                        if (ShowGetComponentUtils.TrySearchComponent<T>(_parentComponent, Attribute.TargetPlace, out var newValue))
                        {
                            ValueEntry.SmartValue = newValue;
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                CallNextDrawer(label);
            }
        }
    }
}
