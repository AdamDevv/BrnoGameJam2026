using APX.Extra.OdinExtensions.Attributes;
using APX.Extra.StateControls.Toggle;
using Sirenix.OdinInspector;
using UnityEngine;

namespace APX.Extra.Tweens
{
    public class TweenToggleState : AToggleableBehaviour
    {
        [SerializeField]
        [HideLabel]
        [ListDrawerSettings(ShowFoldout = false)]
        [EnhancedBoxGroup("Enabled", "@OdinColors.Green")]
        private TweenAnimationField _EnableAnimation;

        [SerializeField]
        [HideLabel]
        [ListDrawerSettings(ShowFoldout = false)]
        [EnhancedBoxGroup("Disabled", "@OdinColors.Orange")]
        private TweenAnimationField _DisableAnimation;
        
        public override void UpdateState(bool immediate = false)
        {
            GetAnim(!_state)?.Kill();
            var anim = GetAnim(_state);
            anim?.Play();
            if(immediate) anim?.Kill(true);
        }

        private ITweenAnimationPlayer GetAnim(bool state) => state ? _EnableAnimation : _DisableAnimation;
    }
}
