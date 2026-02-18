using APX.Extra.Misc;
using UnityEngine;
using UnityEngine.Scripting;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class SpriteRendererAlphaPreset : AStateValuePreset<SpriteRenderer, float>
    {
#if UNITY_EDITOR
        public override string FieldName => "Alpha";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as SpriteRenderer); }

        public override void ApplyTo(SpriteRenderer targetObject)
        {
            if (targetObject) targetObject.color = targetObject.color.WithA(PresetState);
        }
    }
}
