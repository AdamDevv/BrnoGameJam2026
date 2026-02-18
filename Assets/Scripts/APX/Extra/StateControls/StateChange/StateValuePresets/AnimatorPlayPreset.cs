using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class AnimatorPlayPreset : AStateValuePreset<Animator, string>
    {
#if UNITY_EDITOR
        public override string FieldName => "Play";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as Animator); }

        public override void ApplyTo(Animator targetObject)
        {
            if (targetObject != null) targetObject.Play(PresetState);
        }
    }
}
