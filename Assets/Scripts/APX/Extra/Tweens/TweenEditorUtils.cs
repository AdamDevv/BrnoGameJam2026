#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using APX.Extra.DataStructures;
using APX.Extra.EditorUtils;
using APX.Extra.Misc;
using APX.Extra.OdinExtensions;
using APX.Extra.OdinExtensions.Attributes;
using APX.Extra.StateControls.StateChange;
using APX.Extra.Tweens.ObjectActions;
using APX.Extra.Tweens.Segments;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace APX.Extra.Tweens
{
    public static class TweenEditorUtils
    {
        private static GUIStyle _buttonStyle;
        private static GUIStyle ButtonStyle => _buttonStyle ??= new GUIStyle(SirenixGUIStyles.Button)
        {
            padding = new RectOffset(4, 4, 4, 4)
        };

        public static bool DetectCycle(this IParentTweenSegment node)
        {
            return DetectCycle(node, out _);
        }

        public static bool DetectCycle(this IParentTweenSegment node, out IEnumerable<IParentTweenSegment> path)
        {
            var visited = new HashSet<IParentTweenSegment>();
            var result = DetectCycle(node, node, visited);
            path = ((IEnumerable<IParentTweenSegment>) result)?.Reverse();
            return result != null;
        }

        private static List<IParentTweenSegment> DetectCycle(IParentTweenSegment node, IParentTweenSegment root, HashSet<IParentTweenSegment> visited)
        {
            visited.Add(node);
            foreach (var child in node.GetChildSegments().OfType<IParentTweenSegment>())
            {
                if (!visited.Contains(child))
                {
                    var cycle = DetectCycle(child, root, visited);
                    if (cycle != null)
                    {
                        cycle.Add(node);
                        return cycle;
                    }
                }
                else if (child == root)
                {
                    return new List<IParentTweenSegment>{node};
                }
            }
            return null;
        }

        public static IEnumerable<ITweenSegment> ConvertToDirectSegments(this IEnumerable<ITweenSegment> segments, IDictionary<TweenObjectReference, Object> targets)
        {
            var result = new List<ITweenSegment>();
            foreach (var segment in segments)
            {
                if (segment is IPresetTweenSegment presetSegment && presetSegment.TryConvertToDirectSegment(out var directSegment, targets))
                {
                    result.Add(directSegment);
                }
            }
            return result;
        }

        public static IEnumerable<ITweenSegment> ConvertToPresetSegments(this IEnumerable<ITweenSegment> segments, IDictionary<Object, TweenObjectReference> references)
        {
            var result = new List<ITweenSegment>();
            foreach (var segment in segments)
            {
                if (segment is IDirectTweenSegment directSegment && directSegment.TryConvertToPresetSegment(out var presetSegment, references))
                {
                    result.Add(presetSegment);
                }
            }
            return result;
        }

        public static IEnumerable<ValueDropdownItem<ITweenSegment>> GetAllSequenceSegments(InspectorProperty property)
        {
            if (property.HasParentObject<ITweenPreset>())
            {
                return ReflectionHelper.GetTypeCacheDerivedClassesOfType<IPresetTweenSegment>()
                    .Select(t => new ValueDropdownItem<ITweenSegment>(t.LabelName, t));
            }

            return ReflectionHelper.GetTypeCacheDerivedClassesOfType<IDirectTweenSegment>()
                .Select(t => new ValueDropdownItem<ITweenSegment>(t.LabelName, t));
        }

        public static IEnumerable<ValueDropdownItem<ATweenObjectAction>> GetTweenActionsDropdown(Type targetType)
        {
            return ReflectionHelper.GetTypeCacheDerivedClassesOfType<ATweenObjectAction>()
                .Where(a => a.IsValidFor(targetType))
                .Select(a => new ValueDropdownItem<ATweenObjectAction>(a.ToString(), a));
        }

        public static IEnumerable<Type> GetSupportedTargetTypes()
        {
            return ReflectionHelper.GetTypeCacheDerivedClassesOfType<ATweenObjectAction>()
                .Select(t => t.TargetType)
                .Concat(ReflectionHelper.GetTypeCacheDerivedClassesOfType<AStateValuePreset>()
                    .Select(t => t.TargetType))
                .Distinct();
        }

        public static void DrawSavePresetToolbarButton(List<ITweenSegment> segments, InspectorProperty property)
        {
            if (SirenixEditorGUI.ToolbarButton(FontAwesomeEditorIcons.FloppyDiskSolid))
            {
                if (property.HasParentObject<ITweenPreset>())
                {
                    WaitInEditor.ForNextFrame(() =>
                    {
                        var result = OdinExtensionUtils.CreateNewInstanceOfType<TweenAnimationPreset>();
                        if (result == null) return;
                        result.Initialize(segments);
                        EditorUtility.SetDirty(result);
                        EditorGUIUtility.PingObject(result);
                    });
                }
                else
                {
                    var window = EditorWindow.GetWindow<TweenPresetCreatorWindow>(false, "Tween Preset Creator");
                    window.Initialize(segments);
                }
            }
        }
          
        public static void DrawControls(ITweenAnimationPlayer player)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (ToolbarButton(FontAwesomeEditorIcons.ForwardSolid, "Play on Repeat"))
            {
                PlayRepeat();
                void PlayRepeat()
                {
                    player.Rewind();
                    var tween = player.Play();
                    tween.SetLoops(0);
                    tween?.OnComplete(PlayRepeat);
                }
            }

            if (ToolbarButton(FontAwesomeEditorIcons.PlaySolid, "Play"))
            {
                player.Play();
            }
            
            GUIHelper.PushGUIEnabled(player.IsPlaying || player.IsPaused);
            if (ToolbarButton(FontAwesomeEditorIcons.PlayPauseSolid, "Pause/Resume"))
            {
                if (player.IsPlaying)
                {
                    player.Pause();
                }
                else
                {
                    player.Resume();
                }
            }
            GUIHelper.PopGUIEnabled();

            if (ToolbarButton(FontAwesomeEditorIcons.StopSolid, "Kill and Rewind"))
            {
                player.Rewind();
                player.Kill();
            }

            if (ToolbarButton(FontAwesomeEditorIcons.SquareXmarkSolid, "Kill"))
            {
                player.Kill();
            }

            GUIHelper.PushGUIEnabled(player.IsInitialized);
            if (ToolbarButton(FontAwesomeEditorIcons.BackwardFastSolid, "Rewind"))
            {
                player.Rewind();
            }
            if (ToolbarButton(FontAwesomeEditorIcons.ForwardFastSolid, "Complete"))
            {
                player.Complete();
            }
            GUIHelper.PopGUIEnabled();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
        
        public static bool ToolbarButton(EditorIcon icon, string tooltip)
        {
            return GUILayout.Button(GUIHelper.TempContent(icon.Highlighted, tooltip), ButtonStyle, GUILayout.Width(45), GUILayout.Height(22));
        }
    }

    public class TweenPresetCreatorWindow : OdinEditorWindow
    {
        [SerializeField]
        [EnhancedTableList(AlwaysExpanded = true)]
        private List<ComponentReference> _References;

        [InlineProperty]
        [SerializeReference]
        [HideReferenceObjectPicker]
        private List<ITweenSegment> _OriginalSequence = new();

        public void Initialize(List<ITweenSegment> segments)
        {
            _OriginalSequence = segments;
            var targets = new TweenTargetCollection();
            foreach (var segment in segments) segment.PopulateTargets(targets);
            _References = targets.Targets
                .Where(t => t.Key != null)
                .Select(t => new ComponentReference(t.Key, t.Value))
                .ToList();
        }

        [Button]
        public void Confirm()
        {
            var convertedSegments = _OriginalSequence
                .ConvertToPresetSegments(_References.ToDictionary(r => r.Value, r => r.Reference))
                .ToList();

            WaitInEditor.ForNextFrame(() =>
            {
                var result = OdinExtensionUtils.CreateNewInstanceOfType<TweenAnimationPreset>();
                if (result == null) return;
                result.Initialize(convertedSegments);
                EditorUtility.SetDirty(result);
                EditorGUIUtility.PingObject(result);
            });
        }
    }

    [Serializable]
    public class ComponentReference
    {
        [EnableGUI]
        [ReadOnly]
        [SerializeField]
        private Object _Value;

        [EnhancedValueDropdown(nameof(GetAllValidReferences), AppendNextDrawer = true)]
        [ValidateInput(nameof(ValidateReference))]
        [EnableIf(nameof(_Value))]
        [ShowCreateNew(OnCreatedNew = nameof(OnReferenceCreated))]
        [SerializeField]
        private TweenObjectReference _Reference;

        [HideInInspector]
        [SerializeField]
        private UType _RequiredType;

        public Object Value => _Value;
        public TweenObjectReference Reference => _Reference;
        public Type RequiredType => _RequiredType?.Type ?? _Value.GetType();

        public ComponentReference(Object value, Type requiredType)
        {
            _Value = value;
            _RequiredType = requiredType ?? value.GetType();
        }

        private IEnumerable GetAllValidReferences()
        {
            return EditorAssetUtils.FindAllAssetsOfType<TweenObjectReference>()
                .Where(r => RequiredType.IsAssignableFrom(r.ValueType))
                .Select(r => new ValueDropdownItem<TweenObjectReference>(r.name, r));
        }

        private bool ValidateReference(TweenObjectReference value, ref string errorMessage, ref InfoMessageType? messageType)
        {
            if (value == null)
                return true;

            if (value.ValueType == null)
            {
                errorMessage = "Value type is not defined!";
                messageType = InfoMessageType.Error;
                return false;
            }

            if (!RequiredType.IsAssignableFrom(value.ValueType))
            {
                errorMessage = "Reference type does not match the required type!";
                messageType = InfoMessageType.Error;
                return false;
            }
            return true;
        }

        private void OnReferenceCreated(TweenObjectReference value)
        {
            value.ValueType = RequiredType;
        }
    }
}
#endif
