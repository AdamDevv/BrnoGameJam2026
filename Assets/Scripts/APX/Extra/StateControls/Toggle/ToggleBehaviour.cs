using UnityEngine;
using UnityEngine.Serialization;

namespace APX.Extra.StateControls.Toggle
{
    public class ToggleBehaviour : AToggleableBehaviour
    {
#region Public Fields
        [FormerlySerializedAs("TargetComponent")]
        [Space(5)]
        public Behaviour TargetBehaviour;
#endregion


#region Public Methods
        public override void UpdateState(bool immediate)
        {
            if (TargetBehaviour != null) TargetBehaviour.enabled = _state;
        }
#endregion
    }
}
