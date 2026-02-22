using System;
using System.Linq;
using APGame.Managers;
using APX.Extra.Misc;
using APX.Util.OdinAttributes;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Object = System.Object;

namespace APGame.LevelFeatures.StarCreateNumbers
{
    public class StarSystem : MonoBehaviour
    {
        [SerializeField] private int _Number;
        [SerializeField] private Star[] _Stars;

        public bool IsAnyStarActive { get; private set; }
        public bool IsFinished { get; private set; }
        public bool IsInPlace { get; private set; }

        private int _nextStarIndex = 0;

        private void Start()
        {
            
            for (int i = 0; i < _Stars.Length; i++)
            {
                var index = i;
                Star star = _Stars[index];
                star.OnHovered.AddListener(() => OnStarHovered(index));
            }
        }

        private void Update()
        {
            if (_nextStarIndex != 0)
            {
                var lastStar = _Stars[_nextStarIndex - 1];
                Vector3 mousePos = CameraManager.MainCamera.ScreenToWorldPoint(Input.mousePosition).WithZ(0);
                lastStar.Line.End = mousePos - lastStar.transform.position;
            }
        }

        private void OnStarHovered(int index)
        {
            var allStarSystems = FindObjectsByType<StarSystem>(FindObjectsSortMode.None).Where(ss => !ss.IsFinished).ToList();
            var allOtherStarSystems = allStarSystems.Except(new[] { this }).ToArray();

            if (!GameManager.Instance.IsInputEnabled) return;
            if (IsFinished) return;
            if (index < _nextStarIndex) return;

            if (_nextStarIndex != index || allOtherStarSystems.Any(s => s.IsAnyStarActive))
            {
                foreach (StarSystem system in allStarSystems)
                {
                    if (system.IsAnyStarActive)
                    {
                        system.ResetProgress();
                    }
                }

                return;
            }

            IsAnyStarActive = true;

            var currStar = _Stars[_nextStarIndex];
            currStar.SetLineAnimationActive(true);
            currStar.Line.enabled = true;

            if (_nextStarIndex != 0)
            {
                var prevStar = _Stars[_nextStarIndex - 1];
                prevStar.SetLineAnimationActive(false);
                prevStar.Line.End = currStar.transform.position - prevStar.transform.position;
            }

            _nextStarIndex++;

            if (_nextStarIndex == _Stars.Length)
            {
                currStar.SetLineAnimationActive(false);
                currStar.Line.enabled = false;

                FinishStarNumber();
            }
        }

        public void ResetProgress()
        {
            _Stars.ForEach(s => s.Line.enabled = false);
            _nextStarIndex = 0;
            IsAnyStarActive = false;
        }

        private void FinishStarNumber() => UniTask.Void(async cancellationToken => {
            IsFinished = true;
            _Stars.ForEach(s => s.DisableLineAbsoluteScale());

            var slot = FindObjectsByType<StarNumberSlot>(FindObjectsSortMode.None).First(e => e.Number == _Number);

            // Bump animation
            MakeBlack();
            await transform.DOScale(Vector3.one * 1.15f, 0.08f).SetEase(Ease.OutCubic).ToUniTask(cancellationToken: cancellationToken);
            await transform.DOScale(Vector3.one * 1.00f, 0.2f).SetEase(Ease.OutCubic).ToUniTask(cancellationToken: cancellationToken);

            await UniTask.WaitForSeconds(0.3f, cancellationToken: cancellationToken);

            // Move to clock animation
            const float animationDuration = 1.3f;
            const Ease animationEase = Ease.InOutCubic;
            Sequence sequence = DOTween.Sequence()
                .Append(transform.DOMove(slot.transform.position, animationDuration).SetEase(animationEase))
                .Join(transform.DOScale(Vector3.one * 0.3f, animationDuration).SetEase(animationEase));

            await sequence.ToUniTask(cancellationToken: cancellationToken);

            transform.parent = ClockManager.Instance.Clock.transform;
            IsInPlace = true;
        }, destroyCancellationToken);

        private void MakeBlack()
        {
            _Stars.ForEach(s => {
                s.GetComponentInChildren<SpriteRenderer>().color = Color.black;
                s.Line.Color = Color.black;
            });
        }
    }
}
