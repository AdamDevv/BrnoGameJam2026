#if UNITY_EDITOR
using System;
using APX.Extra.OdinExtensions;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace APX.Extra.DataStructures.RectTransformSnapshots
{
    public class RectTransformSnapshotDrawer : OdinValueDrawer<RectTransformSnapshot>, IDisposable
    {
        private IRectTransformProvider _targetProvider;
        private RectTransform _tempRectTransform;
        private Editor _editor;
        private static GUIStyle ButtonStyle => new(SirenixGUIStyles.Button) {padding = new RectOffset(4, 4, 4, 4)};

        protected override void Initialize()
        {
            Property.Tree.OnUndoRedoPerformed += UpdateInternalRectTransformData;

            _tempRectTransform = new GameObject($"RECT_TRANSFORM_PRESET")
                .AddComponent<RectTransform>();
            _tempRectTransform.gameObject.hideFlags = HideFlags.DontSave | HideFlags.HideInHierarchy;

            if (Property.TryGetParentObject<MonoBehaviour>(out var instance))
            {
                var currentTransform = instance.transform;
                var parent = currentTransform.parent;
                if (parent == null)
                    parent = currentTransform;

                if (!PrefabUtility.IsPartOfPrefabAsset(parent))
                    _tempRectTransform.SetParent(parent);

                if (instance is IRectTransformProvider rectTransformApplier)
                    _targetProvider = rectTransformApplier;
            }

            UpdateInternalRectTransformData();

            var rectTransformEditorType = Type.GetType("UnityEditor.RectTransformEditor, UnityEditor");
            _editor = Editor.CreateEditor(_tempRectTransform, rectTransformEditorType);
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (Selection.activeGameObject.TryGetComponent(out RectTransform rectTransform))
            {
                if (GUILayout.Button(
                        GUIHelper.TempContent(FontAwesomeEditorIcons.CopyRegular.Highlighted, "Copy into preset"),
                        ButtonStyle, GUILayout.Width(45), GUILayout.Height(22)))
                {
                    CopyFromRectTransform(rectTransform);
                }
            }

            if (GUILayout.Button(
                    GUIHelper.TempContent(FontAwesomeEditorIcons.WandMagicSparklesRegular.Highlighted, "Apply"),
                    ButtonStyle, GUILayout.Width(45), GUILayout.Height(22)))
            {
                if (Property.TryGetParentObject<MonoBehaviour>(out var instance))
                {
                    Undo.RegisterCompleteObjectUndo(instance.GetComponent<RectTransform>(),
                        "Applied RectTransform preset");

                    var targetTransform = _targetProvider != null ? _targetProvider.RectTransform : instance.GetComponent<RectTransform>();
                    if (rectTransform != null)
                        ValueEntry.SmartValue.ApplyTo(targetTransform);
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            SirenixEditorGUI.HorizontalLineSeparator();
            SirenixEditorGUI.BeginIndentedHorizontal();
            GUILayout.Space(8);
            EditorGUILayout.BeginVertical();
            EditorGUI.BeginChangeCheck();
            _editor.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck())
                CopyFromRectTransform(_tempRectTransform);
            EditorGUILayout.EndVertical();
            SirenixEditorGUI.EndIndentedHorizontal();
        }

        private void CopyFromRectTransform(RectTransform rectTransform)
        {
            Property.Parent.RecordForUndo(null, true);
            ValueEntry.SmartValue.FromRectTransform(rectTransform);
            Property.Parent.ValueEntry.ApplyChanges();
            Property.MarkSerializationRootDirty();
            ValueEntry.SmartValue.ApplyTo(_tempRectTransform);
        }

        private void UpdateInternalRectTransformData()
        {
            ValueEntry.SmartValue.ApplyTo(_tempRectTransform);
        }

        public void Dispose()
        {
            Property.Tree.OnUndoRedoPerformed -= UpdateInternalRectTransformData;

            Object.DestroyImmediate(_editor);
            if (_tempRectTransform)
                Object.DestroyImmediate(_tempRectTransform.gameObject);
        }
    }
}
#endif
