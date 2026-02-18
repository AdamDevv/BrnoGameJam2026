using UnityEngine.Scripting;
using UnityEngine.UI;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class ImageFillAmountPreset : AStateValuePreset<Image, float>
    {
#if UNITY_EDITOR
        public override string FieldName => "Fill Amount";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as Image); }

        public override void ApplyTo(Image targetObject)
        {
            if (targetObject) targetObject.fillAmount = PresetState;
        }
    }
}
