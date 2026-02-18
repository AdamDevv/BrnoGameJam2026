using System;
using System.Collections.Generic;
using APX.Extra.Misc;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting.APIUpdating;
using Object = UnityEngine.Object;

namespace APX.Extra.Tweens.Segments
{
    [Serializable]
    public class UnityEventTweenSegment : ACallbackTweenSegment, IDirectTweenSegment
    {
        [SerializeField]
        public UnityEvent _Event = new UnityEvent();

        public UnityEventTweenSegment() { }
        
        public UnityEventTweenSegment(UnityEvent unityEvent)
        {
            _Event = unityEvent;
        }

        protected override TweenCallback GetCallbackAction() => CallbackAction;

        private void CallbackAction()
        {
            _Event?.Invoke();
        }

#if UNITY_EDITOR
        public UnityEventTweenSegment(AdditionOperation operation, float position, Type targetType)
        {
            _Operation = operation;
            _Position = position;
            TargetType = targetType;
        }

        public override string GetSummary()
        {
            return $"{_Operation.ToString().Nicify()} Unity Event [{_Event.GetSummary()}]";
        }

        public override string LabelName => "Unity Event";

        public bool TryConvertToPresetSegment(out IPresetTweenSegment result, IDictionary<Object, TweenObjectReference> references)
        {
            result = null;
            return false;
        }
#endif
    }
}
