using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class StatePresetApply : AStateValuePreset<StatePresetBehaviour>
    {
#if UNITY_EDITOR
        [OnInspectorGUI]
        private void MyInspectorGUI() { GUILayout.Label(FieldName); }

        public override string FieldName => "Apply State";

#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as StatePresetBehaviour); }

        public override void ApplyTo(StatePresetBehaviour targetObject)
        {
            if (targetObject != null) targetObject.Apply();
        }
    }
}
