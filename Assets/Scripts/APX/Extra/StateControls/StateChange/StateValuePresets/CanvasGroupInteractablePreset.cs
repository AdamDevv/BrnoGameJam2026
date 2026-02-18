using UnityEngine;
using UnityEngine.Scripting;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    public class CanvasGroupInteractablePreset : AStateValuePreset<CanvasGroup, bool>
    {
        [Preserve]
#if UNITY_EDITOR
        public override string FieldName => "Interactable";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as CanvasGroup); }

        public override void ApplyTo(CanvasGroup targetObject)
        {
            if (targetObject != null) targetObject.interactable = PresetState;
        }
    }
}
