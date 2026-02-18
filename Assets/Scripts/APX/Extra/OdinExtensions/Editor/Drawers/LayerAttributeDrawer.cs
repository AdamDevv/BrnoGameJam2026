using APX.Extra.OdinExtensions.Attributes;
using UnityEditor;
using UnityEngine;

namespace APX.Extra.OdinExtensions.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(LayerAttribute))]
    public class LayerAttributeEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) { property.intValue = EditorGUI.LayerField(position, label, property.intValue); }
    }
}
