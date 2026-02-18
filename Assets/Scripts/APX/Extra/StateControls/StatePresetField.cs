using System;
using System.Collections.Generic;
using APX.Extra.StateControls.StateChange;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

namespace APX.Extra.StateControls
{
    [Serializable]
    public class StatePresetField
    {
        [FormerlySerializedAs("ObjectPresets")]
        [SerializeField]
        [PropertyOrder(10)]
        [ValidateInput("ValidatePresets", ContinuousValidationCheck = true)]
        [ListDrawerSettings(ShowFoldout = false, OnTitleBarGUI = "DrawApplyButton")]
        private List<ObjectStatePreset> _ObjectPresets = new List<ObjectStatePreset>();
        public List<ObjectStatePreset> ObjectPresets => _ObjectPresets;

        public void Apply()
        {
            foreach (var objectPreset in _ObjectPresets)
            {
                if (objectPreset == null)
                {
                    Debug.LogError("Object preset is null!");
                    continue;
                }
                objectPreset.Apply();
            }
        }

        public StatePresetField() { }
        public StatePresetField(List<ObjectStatePreset> objectPresets)
        {
            _ObjectPresets = objectPresets;
        }

#if UNITY_EDITOR
        public void DrawApplyButton()
        {
            if (SirenixEditorGUI.ToolbarButton("APPLY"))
            {
                Apply();
            }
        }

        [UsedImplicitly]
        private bool ValidatePresets(List<ObjectStatePreset> presets, ref string errorMessage, ref InfoMessageType? messageType)
        {
            if (StateControlEditorUtils.CheckForObjectPresetsWarnings(presets, out var message))
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
