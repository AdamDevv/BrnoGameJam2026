using System;
using System.Linq;
using APX.Extra.OdinExtensions.Attributes;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace APX.Extra.OdinExtensions.Editor.Drawers
{
    public class DisableEnumFlagsDrawerAttributeDrawer<T> : OdinAttributeDrawer<DisableEnumFlagsDrawerAttribute, T> where T : Enum
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var selected = ValueEntry.SmartValue;
            EditorGUILayout.PrefixLabel(label, EditorStyles.label);
            GenericSelector<T>.DrawSelectorDropdown(label, selected.ToString(), rect =>
            {
                var values = (T[]) Enum.GetValues(typeof(T));
                var selector = new GenericSelector<T>("Values", false, x => x.ToString(), values);
                selector.SetSelection(ValueEntry.SmartValue);
                selector.SelectionTree.Config.DrawSearchToolbar = true;
                var window = selector.ShowInPopup(rect);
                selector.SelectionChanged += selection =>
                {
                    ValueEntry.SmartValue = selection.FirstOrDefault();
                    window.Close();
                };
                return selector;
            });
        }
    }
}
