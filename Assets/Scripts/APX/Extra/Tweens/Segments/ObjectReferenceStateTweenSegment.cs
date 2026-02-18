using System;
using System.Collections.Generic;
using System.Linq;
using APX.Extra.Misc;
using APX.Extra.OdinExtensions.Attributes;
using APX.Extra.StateControls.StateChange;
using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace APX.Extra.Tweens.Segments
{
    [Serializable]
    public class ObjectReferenceStateTweenSegment : ACallbackTweenSegment, IPresetTweenSegment
    {
        [SerializeField]
        [ValidateInput("ValidatePresets")]
        [ListDrawerSettings(ShowFoldout = false)]
        private List<ObjectReferenceStatePreset> _ObjectPresets = new List<ObjectReferenceStatePreset>();

        protected override TweenCallback GetCallbackAction()
        {
            TweenCallback result = null;
            foreach (var objectPreset in _ObjectPresets)
            {
                if (objectPreset == null)
                {
                    Debug.LogError("Object preset is null!");
                    continue;
                }

                if (!objectPreset.Target.TryGetTempValue(out var target))
                    continue;

                TweenCallback applyAction = () => objectPreset.ApplyTo(target);
                if (result == null)
                    result = applyAction;
                else
                    result += applyAction;
            }
            return result;
        }

        public ObjectReferenceStateTweenSegment() { }
        public ObjectReferenceStateTweenSegment(List<ObjectReferenceStatePreset> objectPresets)
        {
            _ObjectPresets = objectPresets;
        }

#if UNITY_EDITOR
        public override void PopulateReferences(HashSet<TweenObjectReference> references)
        {
            base.PopulateReferences(references);
            foreach (var preset in _ObjectPresets) references.Add(preset.Target);
        }

        public bool TryConvertToDirectSegment(out IDirectTweenSegment result, IDictionary<TweenObjectReference, Object> targets)
        {
            var presets = _ObjectPresets
                .Select(o => new ObjectStatePreset(
                    targets.TryGetValue(o.Target, out var target) ? target : null,
                    o.ValuePresets.Select(v => v.GetCopy()).ToList()))
                .ToList();
            result = new ObjectStateTweenSegment(presets);
            return true;
        }

        public override string GetSummary()
        {
            return $"{_Operation.ToString().Nicify()} State Preset";
        }

        public override string LabelName => "State Preset";

        [UsedImplicitly]
        private bool ValidatePresets(List<ObjectReferenceStatePreset> presets, ref string errorMessage, ref InfoMessageType? messageType)
        {
            /*
            if (StateControlEditorUtils.CheckForObjectPresetsWarnings(presets, out var message))
            {
                messageType = InfoMessageType.Warning;
                errorMessage = message;
                return false;
            }
            */
            return true;
        }
#endif
    }

    [Serializable]
    public class ObjectReferenceStatePreset
    {
        [Required]
        [ShowCreateNew]
        [SerializeField]
        private TweenObjectReference _Target;

        [SerializeReference]
        [HideReferenceObjectPicker]
        [ValidateInput("ValidateValuePresets")]
        [ValueDropdown("GetValuePresetDropdown", DrawDropdownForListElements = false)]
        public List<AStateValuePreset> _ValuePresets = new List<AStateValuePreset>();

        public TweenObjectReference Target => _Target;
        public List<AStateValuePreset> ValuePresets => _ValuePresets;

        public void ApplyTo(Object target)
        {
            if (_ValuePresets == null)
                return;

            foreach (var valuePreset in _ValuePresets)
            {
                if (valuePreset == null)
                {
                    Debug.LogError("Name: " + target.name + ", Value preset is null!");
                    continue;
                }
                valuePreset.ApplyTo(target);

            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(target);
#endif
        }

        public ObjectReferenceStatePreset() { }
        public ObjectReferenceStatePreset(TweenObjectReference target, List<AStateValuePreset> valuePresets)
        {
            _Target = target;
            _ValuePresets = valuePresets;
        }

#if UNITY_EDITOR
        [UsedImplicitly]
        private IEnumerable<ValueDropdownItem<AStateValuePreset>> GetValuePresetDropdown()
        {
            if (_Target)
                return StateControlEditorUtils.GetValuePresetDropdown(_Target.ValueType, _ValuePresets);
            return Enumerable.Empty<ValueDropdownItem<AStateValuePreset>>();
        }

        [UsedImplicitly]
        private bool ValidateValuePresets(List<AStateValuePreset> presets, ref string errorMessage, ref InfoMessageType? messageType)
        {
            if (_Target == null)
                return true;

            if (StateControlEditorUtils.CheckForValuePresetsWarnings(_Target.ValueType, presets, out var message))
            {
                messageType = InfoMessageType.Warning;
                errorMessage = message;
                return false;
            }
            return true;
        }
#endif
    }
}
