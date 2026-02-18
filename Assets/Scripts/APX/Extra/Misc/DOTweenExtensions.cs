using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

namespace APX.Extra.Misc
{
    public static class DOTweenExtensions
    {
        /// <summary>Tweens an Image's alpha color to the given value.
        /// Also stores the image as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param>
        /// <param name="duration">The duration of the tween</param>
        //public static Tweener DOFade(this TMP_Text target, float endValue, float duration) { return DOTween.ToAlpha(() => target.color, x => target.color = x, endValue, duration).SetTarget(target); }

        public static Tweener DOBlendablePunchPosition(
            this Transform target,
            Vector3 punch,
            float duration,
            int vibrato = 10,
            float elasticity = 1f)
        {
            var tweenHelper = new BlendableTweenWorldPositionHelper(target, punch);
            return DOTween.Punch(tweenHelper.GetLerpParam, tweenHelper.SetLerpParam, Vector3.right, duration, vibrato, elasticity)
                .OnComplete(tweenHelper.ResetToZero)
                .SetTarget(target);
        }
        
        public static Tweener DOBlendablePunchLocalPosition(
            this Transform target,
            Vector3 punch,
            float duration,
            int vibrato = 10,
            float elasticity = 1f)
        {
            var tweenHelper = new BlendableTweenLocalPositionHelper(target, punch);
            return DOTween.Punch(tweenHelper.GetLerpParam, tweenHelper.SetLerpParam, Vector3.right, duration, vibrato, elasticity)
                .OnComplete(tweenHelper.ResetToZero)
                .SetTarget(target);
        }
        
        public static Tweener DOBlendablePunchScale(
            this Transform target,
            Vector3 punch,
            float duration,
            int vibrato = 10,
            float elasticity = 1f)
        {
            var tweenHelper = new BlendablePunchScaleHelper(target, punch);
            return DOTween.Punch(tweenHelper.GetLerpParam, tweenHelper.SetLerpParam, Vector3.right, duration, vibrato, elasticity)
                .SetTarget(target);
        }

        public static Tweener DOBlendableShakeLocalPosition(
            this Transform target,
            float duration,
            Vector3 strength,
            int vibrato = 10,
            float randomness = 90f,
            bool fadeOut = true)
        {
            var tweenHelper = new BlendableTweenLocalPositionHelper(target, strength);
            return DOTween.Shake(tweenHelper.GetLerpParam, tweenHelper.SetLerpParam, duration, Vector3.right, vibrato, randomness, fadeOut)
                .OnComplete(tweenHelper.ResetToZero)
                .SetTarget(target);
        }


        /// <summary>
        /// Tweens the position of transform 't' to the position of transform 'to' with an optional offset 'targetPosLocalOffset' in 'duration' seconds.
        /// </summary>
        /// <param name="t">this transform</param>
        /// <param name="to">we move to the position of Transform 'to</param>
        /// <param name="duration">tween duration in seconds</param>
        /// <returns></returns>
        public static Tweener DOMove(this Transform t, Transform to, float duration)
        {
            var tweenHelper = new MoveToTransformHelper(t, to);
            return DOTween.To(tweenHelper.GetCurrentPos, tweenHelper.SetCurrentPos, 1.0f, duration)
                .SetTarget(t);
        }

        /// <summary>
        /// Tweens the position of transform 't' to the position of transform 'to' with an optional offset 'targetPosLocalOffset' in 'duration' seconds.
        /// </summary>
        /// <param name="t">this transform</param>
        /// <param name="to">we move to the position of Transform 'to</param>
        /// <param name="duration">tween duration in seconds</param>
        /// <param name="targetMod">optional modifier to target position</param>
        /// <returns></returns>
        public static Tweener DOMove(this Transform t, Transform to, float duration, Func<Transform, Vector3> targetMod)
        {
            var tweenHelper = new MoveToTransformOffsetHelper(t, to, targetMod);
            return DOTween.To(tweenHelper.GetCurrentPos, tweenHelper.SetCurrentPos, 1.0f, duration)
                .SetTarget(t);
        }

        /// <summary>
        /// Tweens the position of transform 't' to the position of transform 'to' with an optional offset 'targetPosLocalOffset' in 'duration' seconds,
        /// along a parabola with amplitude 'amplitude' in the direction of a normal given by Vector3.Cross(targetPos - t.position, forwardVector).
        /// </summary>
        /// <param name="t">this transform</param>
        /// <param name="to">we move to the position of Transform 'to</param>
        /// <param name="forwardVector">used to determine normal (try passing Vector3.forward)</param>
        /// <param name="amplitude">how bent the parabola is (length in world space)</param>
        /// <param name="duration">tween duration in seconds</param>
        /// <param name="rotate">set whether should rotate in the direction of movement</param>
        /// <param name="targetMod">optional modifier to target position</param>
        /// <returns></returns>
        public static Tweener DOMoveParabola(this Transform t, Transform to, Vector3 forwardVector, float amplitude, float duration, bool rotate = false, Func<Transform, Vector3> targetMod = null)
        {
            var tweenHelper = new MoveToTransformParabolaHelper(t, to, forwardVector, amplitude, rotate, targetMod);
            return DOTween.To(tweenHelper.GetCurrentPos, tweenHelper.SetCurrentPos, 1.0f, duration)
                .SetTarget(t);
        }
        
        /// <summary>
        /// Tweens the position of transform 't' to the position of transform 'to' with an optional offset 'targetPosLocalOffset' in 'duration' seconds,
        /// along a parabola with amplitude 'amplitude' in the direction of the 'normalVector' vector.
        /// </summary>
        /// <param name="t">this transform</param>
        /// <param name="to">we move to the position of Transform 'to</param>
        /// <param name="normalVector">the normal vector of the parabola at the amplitude</param>
        /// <param name="amplitude">how bent the parabola is (length in world space)</param>
        /// <param name="duration">tween duration in seconds</param>
        /// <param name="rotate">set whether should rotate in the direction of movement</param>
        /// /// <param name="targetMod">optional modifier to target position</param>
        /// <returns></returns>
        public static Tweener DOMoveParabolaWithNormal(this Transform t, Transform to, Vector3 normalVector, float amplitude, float duration, bool rotate = false, Func<Transform, Vector3> targetMod = null)
        {
            var tweenHelper = new MoveToTransformParabolaWithNormalHelper(t, to, normalVector, amplitude, rotate, targetMod);
            return DOTween.To(tweenHelper.GetCurrentPos, tweenHelper.SetCurrentPos, 1.0f, duration)
                .SetTarget(t);
        }

        public static EaseFunction ParabolaEase()
        {
            return ((time, duration, overshootOrAmplitude, period) =>
            {
                if (time < 0.5f)
                {
                    time *= 2;
                    return -(time /= duration) * (time - 2.0f);
                }

                time = (time - 0.5f) * 2;
                return 1 - (time /= duration) * time;
            });
        }
        
        public static TweenerCore<string, string, StringOptions> DOText(this TMP_InputField target, string endValue, float duration, bool richTextEnabled = true, ScrambleMode scrambleMode = ScrambleMode.None, string scrambleChars = null)
        {
            var t = DOTween.To(() => target.text, x => target.text = x, endValue, duration);
            t.SetOptions(richTextEnabled, scrambleMode, scrambleChars)
                .SetTarget(target);
            return t;
        }

        public static TweenerCore<float, float, FloatOptions> DOSizeDeltaX(this RectTransform target, float endValue, float duration, bool snapping = false)
        {
            var t = DOTween.To(() => target.sizeDelta.x, x => target.sizeDelta = target.sizeDelta.WithX(x), endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }


        public static TweenerCore<float, float, FloatOptions> DOSizeDeltaY(this RectTransform target, float endValue, float duration, bool snapping = false)
        {
            var t = DOTween.To(() => target.sizeDelta.y, x => target.sizeDelta = target.sizeDelta.WithY(x), endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }

        private abstract class ABlendableTweenPositionHelper
        {
            // DOTween.Punch expects a Vector3 as its parameter, so we'll use the x component of a Vector3
            protected Vector3 CurrentLerpParam = Vector3.zero;

            protected Transform _target;
            public Vector3 PunchVector;

            public ABlendableTweenPositionHelper(Transform target, Vector3 punchVector)
            {
                _target = target;
                PunchVector = punchVector;
            }

            public Vector3 GetLerpParam() { return CurrentLerpParam; }

            public void SetLerpParam(Vector3 lerpParam)
            {
                var delta = lerpParam.x - CurrentLerpParam.x;
                CurrentLerpParam = lerpParam;
                AddPos(delta * PunchVector);
            }

            public void ResetToZero()
            {
                SetLerpParam(Vector3.zero);
            }

            protected abstract void AddPos(Vector3 offset);
        }

        private class BlendableTweenWorldPositionHelper : ABlendableTweenPositionHelper
        {
            public BlendableTweenWorldPositionHelper(Transform target, Vector3 punchVector) : base(target, punchVector)
            {
            }

            protected override void AddPos(Vector3 offset) { _target.position += offset; }
        }

        private class BlendableTweenLocalPositionHelper : ABlendableTweenPositionHelper
        {
            public BlendableTweenLocalPositionHelper(Transform target, Vector3 punchVector) : base(target, punchVector)
            {
            }

            protected override void AddPos(Vector3 offset) { _target.localPosition += offset; }
        }
        
        private class BlendablePunchScaleHelper
        {
            // DOTween.Punch expects a Vector3 as its parameter, so we'll use the x component of a Vector3
            protected Vector3 CurrentLerpParam = Vector3.zero;

            private Transform _target;
            public Vector3 PunchVector;

            public BlendablePunchScaleHelper(Transform target, Vector3 punchVector)
            {
                _target = target;
                PunchVector = punchVector;
            }

            public Vector3 GetLerpParam() { return CurrentLerpParam; }

            public void SetLerpParam(Vector3 lerpParam)
            {
                var delta = lerpParam.x - CurrentLerpParam.x;
                CurrentLerpParam = lerpParam;
                _target.localScale += delta * PunchVector;
            }
        }

        private class MoveToTransformHelper
        {
            protected float CurrentPos;
            public readonly Transform Transform;
            public readonly Transform Target;

            public MoveToTransformHelper(Transform transform, Transform target)
            {
                Transform = transform;
                Target = target;
            }

            public float GetCurrentPos() { return CurrentPos; }

            public virtual void SetCurrentPos(float value)
            {
                var targetPos = Target.position;
                Transform.position = Vector3.Lerp(Transform.position, targetPos, (value - CurrentPos) / (1f - CurrentPos));
                CurrentPos = value;
            }
        }

        private class MoveToTransformOffsetHelper : MoveToTransformHelper
        {
            public Func<Transform, Vector3> TargetMod;

            public MoveToTransformOffsetHelper(Transform transform, Transform target, Func<Transform, Vector3> targetMod = null)
                : base(transform, target)
            {
                TargetMod = targetMod;
            }

            public override void SetCurrentPos(float value)
            {
                var targetPos = TargetMod?.Invoke(Target) ?? Target.position;
                Transform.position = Vector3.Lerp(Transform.position, targetPos, (value - CurrentPos) / (1f - CurrentPos));
                CurrentPos = value;
            }
        }

        private abstract class AMoveToTransformParabolaHelper : MoveToTransformOffsetHelper
        {
            protected float Amplitude { get; private set; }
            protected bool Rotate { get; private set; }
            protected Vector3 StartPosition { get; private set; }

            public AMoveToTransformParabolaHelper(Transform transform, Transform target, float amplitude, bool rotate = false, Func<Transform, Vector3> targetMod = null)
                : base(transform, target, targetMod)
            {
                Amplitude = amplitude;
                Rotate = rotate;
                StartPosition = transform.position;
            }

            protected abstract Vector3 CalculateNormal();
            
            public override void SetCurrentPos(float value)
            {
                var targetPos = TargetMod?.Invoke(Target) ?? Target.position;
                var currentNormal = CalculateNormal();

                var pos = Vector3.Lerp(StartPosition, targetPos, value);
                var oldPosition = Transform.position;
                Transform.position = pos + Amplitude * GetParabolaParam(value) * currentNormal;

                if (Rotate)
                {
                    var directionOfMovement = Transform.position - oldPosition;
                    if (directionOfMovement != Vector3.zero)
                        Transform.rotation = Quaternion.LookRotation(directionOfMovement);
                }
                CurrentPos = value;
            }

            private float GetParabolaParam(float param)
            {
                var tmp = 2.0f * param - 1.0f;
                return 1.0f - tmp * tmp;
            }
        }

        private class MoveToTransformParabolaHelper : AMoveToTransformParabolaHelper
        {
            private Vector3 _forwardVector;

            public MoveToTransformParabolaHelper(Transform transform, Transform target, Vector3 forwardVector, float amplitude, bool rotate = false, Func<Transform, Vector3> targetMod = null)
                : base(transform, target, amplitude, rotate, targetMod)
            {
                _forwardVector = forwardVector;
            }

            protected override Vector3 CalculateNormal()
            {
                var targetPos = TargetMod?.Invoke(Target) ?? Target.position;
                return Vector3.Cross(targetPos - StartPosition, _forwardVector).normalized;
            }
        }

        private class MoveToTransformParabolaWithNormalHelper : AMoveToTransformParabolaHelper
        {
            private Vector3 _normalVector;

            public MoveToTransformParabolaWithNormalHelper(Transform transform, Transform target, Vector3 normalVector, float amplitude, bool rotate = false, Func<Transform, Vector3> targetMod = null)
                : base(transform, target, amplitude, rotate, targetMod)
            {
                _normalVector = normalVector;
            }

            protected override Vector3 CalculateNormal()
            {
                return _normalVector;
            }
        }
    }
}
