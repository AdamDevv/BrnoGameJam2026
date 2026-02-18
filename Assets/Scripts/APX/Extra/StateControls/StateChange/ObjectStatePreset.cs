using System.Collections.Generic;
using APX.Extra.OdinExtensions.Attributes;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace APX.Extra.StateControls.StateChange
{
    [System.Serializable]
    public class ObjectStatePreset : AStatePreset
    {
        [FormerlySerializedAs("_target")]
        [Required]
        [SerializeField]
        [DontValidate]
        [InlineButton("Apply")]
        [EnhancedObjectDrawer]
        private Object _Target;

        [FormerlySerializedAs("ValuePresets")]
        [SerializeReference]
        [HideReferenceObjectPicker]
        [ValidateInput("ValidateValuePresets", ContinuousValidationCheck = true)]
        [ValueDropdown("ValueDropdown", DrawDropdownForListElements = false)]
        internal List<AStateValuePreset> _ValuePresets = new List<AStateValuePreset>();

        public Object Target
        {
            get => _Target;
            set => _Target = value;
        }

        public List<AStateValuePreset> ValuePresets => _ValuePresets;

        public ObjectStatePreset() { }

        public ObjectStatePreset(Object target)
        {
            _Target = target;
        }

        public ObjectStatePreset(Object target, List<AStateValuePreset> valuePresets)
        {
            _Target = target;
            _ValuePresets = valuePresets;
        }

        public override void Apply()
        {
            if (_ValuePresets != null)
            {
                foreach (var valuePreset in _ValuePresets)
                {
                    if (valuePreset == null)
                    {
                        Debug.LogError("Name: " + _Target.name + ", Value preset is null!");
                        continue;
                    }
                    valuePreset.ApplyTo(_Target);

                }
#if UNITY_EDITOR
                EditorUtility.SetDirty(_Target);
#endif
            }
        }

#if UNITY_EDITOR
        private List<ValueDropdownItem<AStateValuePreset>> ValueDropdown()
        {
            return StateControlEditorUtils.GetValuePresetDropdown(Target, _ValuePresets);
        }

        [UsedImplicitly]
        private bool ValidateValuePresets(List<AStateValuePreset> presets, ref string errorMessage, ref InfoMessageType? messageType)
        {
            if (StateControlEditorUtils.CheckForValuePresetsWarnings(_Target, presets, out var message))
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
