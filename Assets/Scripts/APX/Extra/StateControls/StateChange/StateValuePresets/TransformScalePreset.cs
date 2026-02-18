using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class TransformScalePreset : AStateValuePreset<Transform, Vector3>
    {
#if UNITY_EDITOR
        public override string FieldName => "Scale";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as Transform); }

        public override void ApplyTo(Transform targetObject)
        {
            if (targetObject != null)
            {
                targetObject.localScale = PresetState;
            }
        }
    }
}
