using APX.Extra.StateControls.Toggle;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class ToggleableStatePreset : AStateValuePreset<AToggleableBehaviour, bool>
    {
#if UNITY_EDITOR
        public override string FieldName => "Set State";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as AToggleableBehaviour); }

        public override void ApplyTo(AToggleableBehaviour targetObject)
        {
            if (targetObject != null) targetObject.State = PresetState;
        }
    }
}
