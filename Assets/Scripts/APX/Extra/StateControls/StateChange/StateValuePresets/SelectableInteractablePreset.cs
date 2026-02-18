using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class SelectableInteractablePreset : AStateValuePreset<Selectable, bool>
    {
#if UNITY_EDITOR
        public override string FieldName => "Interactable";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as Selectable); }

        public override void ApplyTo(Selectable targetObject)
        {
            if (targetObject != null)
            {
                targetObject.interactable = PresetState;
            }
        }
    }
}
