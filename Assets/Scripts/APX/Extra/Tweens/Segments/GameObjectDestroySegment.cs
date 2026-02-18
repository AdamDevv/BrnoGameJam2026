using System.Collections.Generic;
using APX.Extra.Misc;
using DG.Tweening;
using UnityEngine;

namespace APX.Extra.Tweens.Segments
{
    [System.Serializable]
    public class GameObjectDestroySegment : ACallbackTweenSegment, IDirectTweenSegment
    {
        [SerializeField]
        public GameObject _GameObject;

        public GameObjectDestroySegment() { }

        protected override TweenCallback GetCallbackAction() => CallbackAction;

        private void CallbackAction() { Object.Destroy(_GameObject); }

#if UNITY_EDITOR
        public GameObjectDestroySegment(AdditionOperation operation, float position, System.Type targetType)
        {
            _Operation = operation;
            _Position = position;
            TargetType = targetType;
        }

        public override string GetSummary()
        {
            return _GameObject ? $"{_Operation.ToString().Nicify()} Destroy GO {_GameObject.name} [at {_GameObject.GetPath()}]" : $"{_Operation.ToString().Nicify()} Destroy GO";
        }

        public override string LabelName => "Destroy GameObject";

        public bool TryConvertToPresetSegment(out IPresetTweenSegment result, IDictionary<Object, TweenObjectReference> references)
        {
            result = null;
            return false;
        }
#endif
    }
}
