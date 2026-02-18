using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class GameObjectActivePreset : AStateValuePreset<GameObject, bool>
    {
#if UNITY_EDITOR
        public override string FieldName => "Active";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as GameObject); }

        public override void ApplyTo(GameObject targetObject)
        {
            if (targetObject != null) targetObject.SetActive(PresetState);
        }
    }
}
