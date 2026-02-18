using APX.Extra.StateControls.Toggle;
using Sirenix.OdinInspector;
using UnityEngine;

namespace APX.Extra.StateControls.StateChange
{
    [System.Serializable]
    public struct StateAction
    {
        public StateBehaviour Behaviour;

        [ShowIf("IsActivation")]
        public GameObject TargetObject;
        [ShowIf("IsToggle")]
        public AToggleableBehaviour TargetToggle;
        [ShowIf("Behaviour", StateBehaviour.AnimateState)]
        public Animator TargetAnimator;
        [ShowIf("Behaviour", StateBehaviour.AnimateState)]
        public string AnimationState;

        public void Apply()
        {
            switch (Behaviour)
            {
                case StateBehaviour.None:
                    break;
                case StateBehaviour.Activate:
                    if (TargetObject != null) TargetObject.SetActive(true);
                    break;
                case StateBehaviour.Deactivate:
                    if (TargetObject != null) TargetObject.SetActive(false);
                    break;
                case StateBehaviour.ToggleOn:
                    if (TargetToggle != null) TargetToggle.State = true;
                    break;
                case StateBehaviour.ToggleOff:
                    if (TargetToggle != null) TargetToggle.State = false;
                    break;
                case StateBehaviour.AnimateState:
                    if (TargetAnimator != null) TargetAnimator.Play(AnimationState);
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }

#if UNITY_EDITOR
        public bool IsActivation() { return Behaviour == StateBehaviour.Activate || Behaviour == StateBehaviour.Deactivate; }
        public bool IsToggle() { return Behaviour == StateBehaviour.ToggleOn || Behaviour == StateBehaviour.ToggleOff; }
#endif
    }
}
