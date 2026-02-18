using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class TransformPositionPreset : AStateValuePreset<Transform, Vector3>
    {
        public bool SetLocal;

#if UNITY_EDITOR
        public override string FieldName => "Position";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as Transform); }

        public override void ApplyTo(Transform targetObject)
        {
            if (targetObject != null)
            {
                if (SetLocal)
                    targetObject.localPosition = PresetState;
                else
                    targetObject.position = PresetState;
            }
        }
    }
}
