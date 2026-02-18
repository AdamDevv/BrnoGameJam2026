using Sirenix.OdinInspector;
using UnityEngine;

namespace APX.Extra.StateControls.StateChange
{
    public class StatePresetBehaviour : MonoBehaviour, IStatePreset
    {
        [SerializeField, InlineProperty, HideLabel, HideReferenceObjectPicker]
        private StatePreset _statePreset = new StatePreset();

        public void Apply() { _statePreset?.Apply(); }
    }
}
