using Sirenix.OdinInspector;
using UnityEngine;

namespace APX.Extra.StateControls.Toggle
{
    public class ToggleAnimator : AToggleableBehaviour
    {
#region Public Fields
        [Space(5)]
        public Animator TargetAnimator;

        public bool RestartAnimWhenTurnedOn;
        [ShowIf("RestartAnimWhenTurnedOn")]
        public string StateName;
#endregion


#region Public Methods
        public override void UpdateState(bool immediate)
        {
            if (TargetAnimator != null) TargetAnimator.enabled = _state;
        }

        public override void SetState(bool state, bool immediate = false)
        {
            var restart = RestartAnimWhenTurnedOn && _state == false && state == true;
            base.SetState(state, immediate);
            if (restart) TargetAnimator.Play(StateName);
        }
#endregion
    }
}
