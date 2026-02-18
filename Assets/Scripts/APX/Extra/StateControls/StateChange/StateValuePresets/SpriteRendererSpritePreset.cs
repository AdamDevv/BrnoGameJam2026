using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class SpriteRendererSpritePreset : AStateValuePreset<SpriteRenderer, Sprite>
    {
#if UNITY_EDITOR
        public override string FieldName => "Sprite";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as SpriteRenderer); }

        public override void ApplyTo(SpriteRenderer targetObject)
        {
            targetObject.sprite = PresetState;
        }
    }
}
