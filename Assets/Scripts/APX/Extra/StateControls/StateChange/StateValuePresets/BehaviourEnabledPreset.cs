using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class BehaviourEnabledPreset : AStateValuePreset<Behaviour, bool>
    {
#if UNITY_EDITOR
        public override string FieldName => "Enabled";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as Behaviour); }

        public override void ApplyTo(Behaviour targetObject)
        {
            if (targetObject != null) targetObject.enabled = PresetState;
        }
    }
}
