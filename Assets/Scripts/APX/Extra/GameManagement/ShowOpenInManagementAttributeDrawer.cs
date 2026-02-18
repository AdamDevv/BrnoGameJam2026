#if UNITY_EDITOR

using APX.Extra.GameManagement.Runtime;
using APX.Extra.OdinExtensions;
using JetBrains.Annotations;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace APX.Extra.GameManagement
{
    [UsedImplicitly]
    [DrawerPriority(DrawerPriorityLevel.WrapperPriority)]
    [AllowGUIEnabledForReadonly]
    public class ShowOpenInManagementAttributeDrawer<TField> : OdinAttributeDrawer<ShowOpenInManagementAttribute, TField> where TField : ScriptableObject
    {
        private ValueResolver<bool> _showIfResolver;

        protected override void Initialize()
        {
            if(!string.IsNullOrEmpty(Attribute.ShowIf))
                _showIfResolver = ValueResolver.Get<bool>(Property, Attribute.ShowIf);
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            ValueResolver.DrawErrors(_showIfResolver);

            if (_showIfResolver == null || _showIfResolver.HasError || _showIfResolver.GetValue())
            {
                EditorGUILayout.BeginHorizontal();
                CallNextDrawer(label);

                EditorGUI.BeginDisabledGroup(ValueEntry.SmartValue == null);
                var rect = GUILayoutUtility.GetRect(18, 18, SirenixGUIStyles.Button, GUILayoutOptions.ExpandWidth(false).Width(18));
                if (SirenixEditorGUI.IconButton(rect, FontAwesomeEditorIcons.BookArrowUpSolid, "Open in Game Management"))
                {
                    var window = GameManagementWindow.GetWindow();
                    if (window.TrySelectPanel<ISelectorManagementPanel>(out var panel, p => p.CanSelect(ValueEntry.SmartValue)))
                    {
                        panel.Select(ValueEntry.SmartValue);
                    }
                }
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                CallNextDrawer(label);
            }
        }
    }
}

#endif