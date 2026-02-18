using System;
using APX.Extra.Misc;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace APX.Extra.Tweens.ObjectActions
{
    [Serializable]
    public abstract class ATweenObjectAction
    {
        [SerializeField]
        [MinValue(0)]
        protected float _Delay;

        [SerializeField]
        [MinValue(0)]
        protected float _Duration = 1;

        [SerializeField]
        [HideIf(nameof(IsDurationZero))]
        protected EaseBundle _Ease;

        private bool IsDurationZero => Mathf.Approximately(_Duration, 0);

        public float Delay => _Delay;
        public float Duration => _Duration;
        public EaseBundle Ease => _Ease;

        public abstract Type TargetType { get; }

        public abstract bool TryGetTween(Object target, out Tween tween);
        public abstract bool IsValidFor(Object target);
        public abstract bool IsValidFor(Type targetType);

        public ATweenObjectAction GetCopy()
        {
            return JsonUtility.FromJson(JsonUtility.ToJson(this), GetType()) as ATweenObjectAction;
        }

        public override string ToString() => GetType().Name.Replace("TweenAction", "").Nicify();
    }
    
    [Serializable]
    public abstract class ATweenObjectAction<T> : ATweenObjectAction where T : Object
    {
        public override Type TargetType => typeof(T);

        public override bool TryGetTween(Object target, out Tween tween)
        {
            if (!(target is T tTarget))
            {
                tween = null;
                return false;
            }
            tween = GetTween(tTarget);
            if (!Mathf.Approximately(_Duration, 0))
            {
                tween.SetEase(_Ease);
            }
            tween.SetDelay(_Delay);
            return true;
        }

        public override bool IsValidFor(Object target) => target is T;
        public override bool IsValidFor(Type targetType) => typeof(T).IsAssignableFrom(targetType);
        
        protected abstract Tween GetTween(T target);
    }
}
