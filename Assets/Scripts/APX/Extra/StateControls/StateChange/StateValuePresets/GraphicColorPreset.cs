using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class GraphicColorPreset : AStateValuePreset<Graphic, Color>
    {
#if UNITY_EDITOR
        public override string FieldName => "Color";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as Graphic); }

        public override void ApplyTo(Graphic targetObject)
        {
            if (targetObject != null) targetObject.color = PresetState;
        }
    }
}
