using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class ImageSpritePreset : AStateValuePreset<Image, Sprite>
    {
#if UNITY_EDITOR
        public override string FieldName => "Sprite";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as Image); }

        public override void ApplyTo(Image targetObject)
        {
            if (targetObject != null) targetObject.sprite = PresetState;
        }
    }
}
