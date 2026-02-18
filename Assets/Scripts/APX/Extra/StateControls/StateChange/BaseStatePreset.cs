using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace APX.Extra.StateControls.StateChange
{
    [System.Serializable]
    public class BaseStatePreset : AStatePreset
    {
        [PropertyOrder(10)]
        [ValidateInput("ValidatePresets", ContinuousValidationCheck = true)]
        [ListDrawerSettings(OnTitleBarGUI = "DrawApplyButton")]
        public List<ObjectStatePreset> ObjectPresets = new List<ObjectStatePreset>();

        public override void Apply()
        {
            foreach (var objectPreset in ObjectPresets)
            {
                if (objectPreset == null)
                {
                    Debug.LogError("Object preset is null!");
                    continue;
                }
                objectPreset.Apply();
            }
        }

#if UNITY_EDITOR
        public void DrawApplyButton()
        {
            if (Sirenix.Utilities.Editor.SirenixEditorGUI.ToolbarButton("APPLY"))
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
