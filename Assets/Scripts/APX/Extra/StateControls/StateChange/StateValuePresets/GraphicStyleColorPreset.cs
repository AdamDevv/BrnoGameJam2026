using APX.Extra.Presets;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class GraphicStyleColorPreset : AStateValuePreset<Graphic, ColorStylePreset>
    {
#if UNITY_EDITOR
        public override string FieldName => "Color Style";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as Graphic); }

        public override void ApplyTo(Graphic targetObject)
        {
            if (targetObject != null) targetObject.color = PresetState.Color;
        }
    }
}
