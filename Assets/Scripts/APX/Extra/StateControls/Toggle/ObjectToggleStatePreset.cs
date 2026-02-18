using System.Collections.Generic;
using APX.Extra.OdinExtensions.Attributes;
using APX.Extra.StateControls.StateChange;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Serialization;

namespace APX.Extra.StateControls.Toggle
{
    [System.Serializable]
    public class ObjectToggleStatePreset : IToggleable
    {
        [FormerlySerializedAs("_target")]
        [Required]
        [SerializeField]
        [DontValidate]
        [EnhancedObjectDrawer]
        private Object _Target;
        public Object Target { get => _Target; set => _Target = value; }

        [FormerlySerializedAs("OnStateValuePresets")]
        [PropertySpace(4)]
        [Preserve]
        [SerializeReference]
        [HideReferenceObjectPicker] 
        [ValidateInput("ValidateValuePresets", ContinuousValidationCheck = true)]
        [ValueDropdown("OnValuesDropdown", DrawDropdownForListElements = false)]
        public List<AStateValuePreset> _OnStateValuePresets = new List<AStateValuePreset>();

        [FormerlySerializedAs("OffStateValuePresets")]
        [Preserve]
        [SerializeReference]
        [HideReferenceObjectPicker] 
        [ValidateInput("ValidateValuePresets", ContinuousValidationCheck = true)]
        [ValueDropdown("OffValuesDropdown", DrawDropdownForListElements = false)]
        public List<AStateValuePreset> _OffStateValuePresets = new List<AStateValuePreset>();
        
        [ShowInInspector]
        [InlineButton("UpdateState", "Update State")]
        public virtual bool State
        {
            get => _state;
            set => SetState(value);
        }
        protected bool _state;

        public ObjectToggleStatePreset() { }
        public ObjectToggleStatePreset(Object target) { _Target = target; }

        public virtual void UpdateState()
        {
            if (_Target != null)
            {
                var valuePresets = _state ? _OnStateValuePresets : _OffStateValuePresets;
                if (valuePresets != null)
                {
                    for (var index = 0; index < valuePresets.Count; index++)
                    {
                        var valuePreset = valuePresets[index];
                        if (valuePreset == null)
                        {
                            Debug.LogError($"[{GetType().Name}] Value preset in '{_Target.name}' at index {index} is null!", _Target);
                            continue;
                        }

                        valuePreset.ApplyTo(_Target);
                    }
#if UNITY_EDITOR
                    EditorUtility.SetDirty(_Target);
#endif
                }
            }
        }

        protected virtual void SetState(bool state)
        {
            _state = state;
            UpdateState();
        }

#if UNITY_EDITOR
        private IList<ValueDropdownItem<AStateValuePreset>> OnValuesDropdown()
        {
            return StateControlEditorUtils.GetValuePresetDropdown(Target, _OnStateValuePresets);
        }

        private IList<ValueDropdownItem<AStateValuePreset>> OffValuesDropdown()
        {
            return StateControlEditorUtils.GetValuePresetDropdown(Target, _OffStateValuePresets);
        }
        
        [UsedImplicitly]
        private bool ValidateValuePresets(List<AStateValuePreset> valuePresets, ref string errorMessage, ref InfoMessageType? messageType)
        {
            if (StateControlEditorUtils.CheckUnusablePresetsWarnings(_Target, valuePresets, out var message))
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
