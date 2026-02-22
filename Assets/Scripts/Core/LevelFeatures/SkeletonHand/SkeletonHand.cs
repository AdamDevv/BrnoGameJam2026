using System;
using APGame.InGame;
using APGame.Managers;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace APGame.LevelFeatures.SkeletonHand
{
    public class SkeletonHand : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _Anim1Curve;

        public bool IsClockHandAttached { get; private set; }

        private bool _isDetached;
        private int _clickCount;
        private Vector3 _startRot;
        private Clock _clock;

        private void Start()
        {
            _clock = ClockManager.Instance.Clock as Clock;
            _startRot = transform.eulerAngles;
        }

        private void OnMouseDown()
        {
            if (!GameManager.Instance.IsInputEnabled) return;
            if (_isDetached) return;

            transform.DOKill();

            transform.eulerAngles = _startRot;

            if (_clickCount < 5)
            {
                transform.DOShakeRotation(0.5f, new Vector3(0, 0, 3 + _clickCount * 5), randomness: 1, randomnessMode: ShakeRandomnessMode.Harmonic);
                _clickCount++;
                return;
            }

            _isDetached = true;

            UniTask.Void(async cancellationToken => {
                const float anim1Duration = 0.5f;
                await DOTween.Sequence()
                    .Append(transform.DOMove(transform.position + Vector3.right * 0.3f + Vector3.up * 0.1f, anim1Duration).SetEase(_Anim1Curve))
                    .Join(transform.DORotate(transform.eulerAngles + Vector3.forward * 20, anim1Duration).SetEase(_Anim1Curve))
                    .ToUniTask(cancellationToken: cancellationToken);


                const float anim2Duration = 1.1f;
                await DOTween.Sequence()
                    .Append(transform.DOMove(transform.position + Vector3.up * 4.8f, anim2Duration).SetEase(Ease.InOutCubic))
                    .Join(transform.DORotate(transform.eulerAngles + Vector3.forward * 20, anim2Duration).SetEase(Ease.InOutCubic))
                    .Join(transform.DOScale(1.8f, anim2Duration).SetEase(Ease.InOutCubic))
                    .ToUniTask(cancellationToken: cancellationToken);

                float n = 0;
                Vector3 startPos = transform.position;
                Quaternion startRot = transform.rotation;
                Vector3 startScale = transform.localScale;
                await DOTween.To(() => n, x => n = x, 1, 1f)
                    .OnUpdate(() => {
                        var targetPosition = _clock.HourHand.transform.position;
                        var targetRotation = _clock.HourHand.transform.rotation;
                        var targetScale = Vector3.one * 0.8f;
                        transform.position = Vector3.Lerp(startPos, targetPosition, n);
                        transform.rotation = Quaternion.Lerp(startRot, targetRotation, n);
                        transform.localScale = Vector3.Lerp(startScale, targetScale, n);
                    })
                    .SetEase(Ease.InOutCubic)
                    .ToUniTask(cancellationToken: cancellationToken);

                Destroy(transform.GetComponent<Rigidbody2D>());
                transform.parent = _clock.HourHand.transform;
                IsClockHandAttached = true;
            }, destroyCancellationToken);
        }
    }
}
