using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class SpriteRendererMaskInteractionPreset: AStateValuePreset<SpriteRenderer, SpriteMaskInteraction>
    {
#if UNITY_EDITOR
        public override string FieldName => "Mask Interaction";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as SpriteRenderer); }

        public override void ApplyTo(SpriteRenderer targetObject)
        {
            targetObject.maskInteraction = PresetState;
        }
    }
}
