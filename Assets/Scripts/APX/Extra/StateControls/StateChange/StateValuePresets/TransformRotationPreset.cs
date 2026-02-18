using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class TransformRotationPreset : AStateValuePreset<Transform, Vector3>
    {
        public bool SetLocal;

#if UNITY_EDITOR
        public override string FieldName => "Rotation";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as Transform); }

        public override void ApplyTo(Transform targetObject)
        {
            if (targetObject != null)
            {
                if (SetLocal)
                    targetObject.localRotation = Quaternion.Euler(PresetState);
                else
                    targetObject.rotation = Quaternion.Euler(PresetState);
            }
        }
    }
}
