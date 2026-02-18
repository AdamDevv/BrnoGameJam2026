using UnityEngine;
using UnityEngine.Scripting;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    public class CanvasGroupBlockRaycastPreset : AStateValuePreset<CanvasGroup, bool>
    {
        [Preserve]
#if UNITY_EDITOR
        public override string FieldName => "Block Raycasts";
#endif

        public override void ApplyTo(object targetObject)
        {
            ApplyTo(targetObject as CanvasGroup);
        }

        public override void ApplyTo(CanvasGroup targetObject)
        {
            if (targetObject != null) targetObject.blocksRaycasts = PresetState;
        }
    }
}
