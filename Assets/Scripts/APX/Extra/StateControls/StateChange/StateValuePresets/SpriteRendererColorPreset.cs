using UnityEngine;
using UnityEngine.Scripting;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class SpriteRendererColorPreset : AStateValuePreset<SpriteRenderer, Color>
    {
#if UNITY_EDITOR
        public override string FieldName => "Color";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as SpriteRenderer); }

        public override void ApplyTo(SpriteRenderer targetObject)
        {
            if (targetObject) targetObject.color = PresetState;
        }
    }
}
