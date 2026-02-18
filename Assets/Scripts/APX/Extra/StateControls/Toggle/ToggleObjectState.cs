using Sirenix.OdinInspector;
using UnityEngine;

namespace APX.Extra.StateControls.Toggle
{
    public class ToggleObjectState : MonoBehaviour, IToggleable
    {
#region Public Fields
        [SerializeField, InlineProperty, HideLabel, HideReferenceObjectPicker]
        private ObjectToggleStatePreset _objectToggleStatePreset = new ObjectToggleStatePreset();

        public bool State { get => _objectToggleStatePreset.State; set => _objectToggleStatePreset.State = value; }
#endregion
    }
}
