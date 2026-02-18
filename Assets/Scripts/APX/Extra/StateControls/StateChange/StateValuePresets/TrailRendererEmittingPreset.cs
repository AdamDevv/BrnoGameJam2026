using UnityEngine;
using UnityEngine.Scripting;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class TrailRendererEmittingPreset : AStateValuePreset<TrailRenderer, bool>
    {
#if UNITY_EDITOR
        public override string FieldName => "Emitting";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as TrailRenderer); }

        public override void ApplyTo(TrailRenderer targetObject)
        {
            if (targetObject) targetObject.emitting = PresetState;
        }
    }
}
