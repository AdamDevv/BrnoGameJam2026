using UnityEngine;
using UnityEngine.UI;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    public class CanvasScalerReferenceResolutionPreset : AStateValuePreset<CanvasScaler, Vector2>
    {
#if UNITY_EDITOR
        public override string FieldName => "Reference Resolution";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as CanvasScaler); }

        public override void ApplyTo(CanvasScaler targetObject)
        {
            if (targetObject != null)
            {
                targetObject.referenceResolution = PresetState;
            }
        }
    }
}