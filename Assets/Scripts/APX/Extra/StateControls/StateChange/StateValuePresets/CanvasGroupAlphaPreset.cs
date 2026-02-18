using UnityEngine;
using UnityEngine.Scripting;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    public class CanvasGroupAlphaPreset : AStateValuePreset<CanvasGroup, float>
    {
        [Preserve]
#if UNITY_EDITOR
        public override string FieldName => "Alpha";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as CanvasGroup); }

        public override void ApplyTo(CanvasGroup targetObject)
        {
            if (targetObject != null) targetObject.alpha = PresetState;
        }
    }
}
