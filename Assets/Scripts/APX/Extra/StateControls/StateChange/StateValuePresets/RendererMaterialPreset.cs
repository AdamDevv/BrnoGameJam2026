using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class RendererMaterialPreset : AStateValuePreset<Renderer, Material>
    {
#if UNITY_EDITOR
        public override string FieldName => "Material";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as Renderer); }

        public override void ApplyTo(Renderer targetObject)
        {
            targetObject.material = PresetState;
        }
    }
}
