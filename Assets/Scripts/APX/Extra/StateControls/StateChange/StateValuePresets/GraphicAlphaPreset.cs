using APX.Extra.Misc;
using UnityEngine.Scripting;
using UnityEngine.UI;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class GraphicAlphaPreset : AStateValuePreset<Graphic, float>
    {
#if UNITY_EDITOR
        public override string FieldName => "Alpha";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as Graphic); }

        public override void ApplyTo(Graphic targetObject)
        {
            if (targetObject != null) targetObject.color = targetObject.color.WithA(PresetState);
        }
    }
}
