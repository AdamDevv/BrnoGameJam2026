using System.Linq;
using APGame.Managers;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace APGame.LevelFeatures.TreeBranch
{
    public class TreeBranchOne : MonoBehaviour
    {
        [SerializeField] private int _Index;

        public bool IsInPlace { get; private set; }

        private bool _isDetached;
        private int _clickCount;
        private Vector3 _startRot;

        private void Start()
        {
            _startRot = transform.eulerAngles;
        }

        private void OnMouseDown()
        {
            if (!GameManager.Instance.IsInputEnabled) return;
            if (_isDetached) return;

            transform.DOKill();

            transform.eulerAngles = _startRot;

            if (_clickCount < 0)
            {
                transform.DOShakeRotation(0.5f, new Vector3(0, 0, 6 + _clickCount * 5), randomness: 1, randomnessMode: ShakeRandomnessMode.Harmonic);
                _clickCount++;
                return;
            }

            var slot = FindObjectsByType<TreeBranchOneSlot>(FindObjectsSortMode.None).First(slot => slot.Index == _Index);

            _isDetached = true;

            UniTask.Void(async cancellationToken => {
                const float animationDuration = 0.8f;
                const Ease animationEase = Ease.OutBack;
                Sequence sequence = DOTween.Sequence()
                    .Append(transform.DOMove(slot.transform.position, animationDuration).SetEase(animationEase, 0.1f))
                    .Join(transform.DORotate(Vector3.zero, animationDuration).SetEase(animationEase, 0.1f))
                    .Join(transform.DOScale(Vector3.one * 1.3f, animationDuration).SetEase(animationEase, 0.1f));

                await sequence.ToUniTask(cancellationToken: cancellationToken);

                transform.parent = ClockManager.Instance.Clock.transform;
                IsInPlace = true;
            }, destroyCancellationToken);
        }
    }
}
