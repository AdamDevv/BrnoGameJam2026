using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using APX.Extra.OdinExtensions;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif

namespace APX.Extra.Misc
{
    [Serializable]
    [InlineProperty]
    public class ScriptableObjectInterfaceField<TInterface> : SerializableInterfaceField<ScriptableObject, TInterface> where TInterface : class
    {
        public ScriptableObjectInterfaceField(TInterface value) : base(value) { }
        public ScriptableObjectInterfaceField(ScriptableObject field) : base(field) { }
    }

    [Serializable]
    [InlineProperty]
    public class SerializableInterfaceField<TInterface> : SerializableInterfaceField<Object, TInterface> where TInterface : class
    {
        public SerializableInterfaceField(TInterface value) : base(value) { }
        public SerializableInterfaceField(Object field) : base(field) { }
    }

    [Serializable]
    [InlineProperty]
    public class SerializableInterfaceField<TBase, TInterface> where TBase : Object where TInterface : class
    {
        [SerializeField]
        internal TBase field;

        public TInterface Value { get => field as TInterface; set => field = value as TBase; }

        public static implicit operator TInterface(SerializableInterfaceField<TBase, TInterface> other) => other.Value;
#if UNITY_2021_3_OR_NEWER
        public static implicit operator SerializableInterfaceField<TBase, TInterface>(TInterface other) => new(other);
#endif

        public SerializableInterfaceField(TInterface value) { Value = value; }

        public SerializableInterfaceField(TBase field) { this.field = field; }
    }

#if UNITY_EDITOR
    [UsedImplicitly]
    public class ScriptableObjectInterfaceFieldDrawer<TInterface> : OdinValueDrawer<ScriptableObjectInterfaceField<TInterface>>
        where TInterface : class
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            SerializableInterfaceFieldHelper.DrawScriptableObjectLayout<ScriptableObject, TInterface>(label,
                () => ValueEntry.SmartValue.field,
                value =>
                {
                    ValueEntry.SmartValue.field = value;
                    ValueEntry.Property.MarkSerializationRootDirty();
                });
        }
    }

    [UsedImplicitly]
    public class SerializableInterfaceFieldDrawer<TInterface> : OdinValueDrawer<SerializableInterfaceField<TInterface>>
        where TInterface : class
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            SerializableInterfaceFieldHelper.DrawObjectLayout<Object, TInterface>(label,
                () => ValueEntry.SmartValue.field,
                value =>
                {
                    ValueEntry.SmartValue.field = value;
                    ValueEntry.Property.MarkSerializationRootDirty();
                });
        }
    }

    [UsedImplicitly]
    public class SerializableInterfaceFieldDrawer<TBase, TInterface> : OdinValueDrawer<SerializableInterfaceField<TBase, TInterface>>
        where TBase : Object
        where TInterface : class
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (typeof(ScriptableObject).IsAssignableFrom(typeof(TBase)))
            {
                SerializableInterfaceFieldHelper.DrawScriptableObjectLayout<ScriptableObject, TInterface>(label,
                    () => ValueEntry.SmartValue.field as ScriptableObject,
                    value =>
                    {
                        ValueEntry.SmartValue.field = value as TBase;
                        ValueEntry.Property.MarkSerializationRootDirty();
                    });
            }
            else
            {
                SerializableInterfaceFieldHelper.DrawObjectLayout<TBase, TInterface>(label,
                    () => ValueEntry.SmartValue.field,
                    value =>
                    {
                        ValueEntry.SmartValue.field = value;
                        ValueEntry.Property.MarkSerializationRootDirty();
                    });
            }
        }
    }

    public static class SerializableInterfaceFieldHelper
    {
        private static void DrawBaseLayout<TBase, TInterface>(GUIContent label, Func<TBase> getValue, Action<TBase> setValue, out Rect fieldRect)
            where TBase : Object
            where TInterface : class
        {
            var field = getValue();
            if (!(field is TInterface) && field != null)
            {
                SirenixEditorGUI.MessageBox("Invalid value!", MessageType.Error);
            }

            EditorGUI.BeginChangeCheck();
            var newValue = (TBase) SirenixEditorFields.UnityObjectField(label, field, typeof(TBase), true);
            if (EditorGUI.EndChangeCheck() && newValue is TInterface)
            {
                setValue(newValue);
            }

            fieldRect = GUILayoutUtility.GetLastRect();
            GUIHelper.PushGUIEnabled(true);
            GUI.Label(fieldRect.HorizontalPadding(0f, 35f), $"({typeof(TInterface).Name})", SirenixGUIStyles.RightAlignedGreyMiniLabel);
            GUIHelper.PopGUIEnabled();
        }

        public static void DrawObjectLayout<TBase, TInterface>(GUIContent label, Func<TBase> getValue, Action<TBase> setValue)
            where TBase : Object
            where TInterface : class
        {
            EditorGUILayout.BeginHorizontal();
            DrawBaseLayout<TBase, TInterface>(label, getValue, setValue, out var rect);
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawScriptableObjectLayout<TBase, TInterface>(GUIContent label, Func<TBase> getValue, Action<TBase> setValue)
            where TBase : ScriptableObject
            where TInterface : class
        {
            EditorGUILayout.BeginHorizontal();
            DrawBaseLayout<TBase, TInterface>(label, getValue, setValue, out var fieldRect);

            var field = getValue();

            var searchRect = GUILayoutUtility.GetRect(18, 18, SirenixGUIStyles.Button, GUILayoutOptions.ExpandWidth(false).Width(18));
            if (SirenixEditorGUI.IconButton(searchRect, EditorIcons.MagnifyingGlass, "Search"))
            {
                var values = GetFilter();
                var selector = new GenericSelector<TBase>("Assets", false, values);
                selector.SetSelection(field);
                selector.EnableSingleClickToSelect();
                selector.SelectionTree.EnumerateTree().AddThumbnailIcons(true);
                selector.SelectionTree.DefaultMenuStyle.Height = 22;
                selector.SelectionTree.Config.DrawSearchToolbar = true;
                selector.SelectionTree.Config.AutoFocusSearchBar = true;

                selector.SelectionConfirmed += selection => { setValue(selection.FirstOrDefault()); };
                var window = selector.ShowInPopup(fieldRect);
                window.OnClose += selector.SelectionTree.Selection.ConfirmSelection;
            }

            if (Event.current.commandName == "ObjectSelectorUpdated" && EditorGUIUtility.GetObjectPickerControlID() == 666)
            {
                setValue((TBase) EditorGUIUtility.GetObjectPickerObject());
            }

            var createNewRect = GUILayoutUtility.GetRect(18, 18, SirenixGUIStyles.Button, GUILayoutOptions.ExpandWidth(false).Width(18));
            if (SirenixEditorGUI.IconButton(createNewRect, FontAwesomeEditorIcons.FileMedicalSolid, "Create new"))
            {
                OdinExtensionUtils.DrawSubtypeDropDownOrCall(typeof(TInterface), CreateInstance);
            }

            EditorGUILayout.EndHorizontal();

            void CreateInstance(Type type)
            {
                var result = OdinExtensionUtils.CreateNewInstanceOfType<TBase>(type);
                if (result != null)
                {
                    setValue(result);
                }
            }

            static IEnumerable<GenericSelectorItem<TBase>> GetFilter()
            {
                var derivedTypes = ReflectionHelper.GetAssignableTypes(typeof(TInterface));
                var sb = new StringBuilder();
                foreach (var type in derivedTypes)
                {
                    if (typeof(TInterface).IsAssignableFrom(type))
                        sb.Append("t:" + type.FullName + " ");
                }

                if (sb.Length == 0)
                    sb.Append("t:");

                var filter = sb.ToString();
                return Enumerable.Prepend(AssetDatabase.FindAssets(filter)
                        .Select(AssetDatabase.GUIDToAssetPath)
                        .Distinct()
                        .SelectMany(x => (!x.EndsWith(".unity",
                                StringComparison.InvariantCultureIgnoreCase)
                                ? AssetDatabase.LoadAllAssetsAtPath(x)
                                : Enumerable.Repeat(AssetDatabase.LoadAssetAtPath(x, typeof(Object)), 1))
                            .Where(obj => obj != null && obj is TInterface)
                            .Select(obj => new
                            {
                                o = obj as TBase,
                                p = x
                            }))
                        .Select(x => new GenericSelectorItem<TBase>(x.p + (AssetDatabase.IsMainAsset(x.o) ? "" : "/" + x.o.name), x.o)), new GenericSelectorItem<TBase>("Null", null));
            }
        }
    }
#endif
}
