using UnityEngine;
using UnityEngine.Scripting;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class RendererEnabledPreset : AStateValuePreset<Renderer, bool>
    {
#if UNITY_EDITOR
        public override string FieldName => "Enabled";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as Renderer); }

        public override void ApplyTo(Renderer targetObject)
        {
            if (targetObject) targetObject.enabled = PresetState;
        }
    }
}
