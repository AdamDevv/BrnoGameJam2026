using TMPro;
using UnityEngine.Scripting;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class TMPTextTextPreset: AStateValuePreset<TMP_Text, string>
    {
#if UNITY_EDITOR
        public override string FieldName => "Text";
#endif
        
        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as TMP_Text); }
        
        public override void ApplyTo(TMP_Text targetObject)
        {
            if (targetObject != null) targetObject.text = PresetState;
        }
    }
}
