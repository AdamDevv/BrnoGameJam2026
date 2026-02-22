using System;
using APGame.Managers;
using APX.Extra.Signals;
using APX.Util.OdinAttributes;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Shapes;
using UnityEngine;

namespace APGame.LevelFeatures.StarCreateNumbers
{
    public class Star : MonoBehaviour
    {
        [SerializeField] [RequiredChild] private Line _Line;
        public Signal OnHovered { get; } = new();

        public Line Line => _Line;

        private bool _lineAnimationActive;
        private bool _lineAbsoluteScale = true;

        private void Start()
        {
            const float sizeMin = 0.7f;
            const float sizeMax = 1.2f;
            const float maxRotateDegrees = 10f;
            const float minAppearDelaySec = 1.5f - 0.9f;
            const float maxAppearDelaySec = 2f - 0.9f;

            Collider2D collider = null;
            TryGetComponent(out collider);
            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, -maxRotateDegrees + maxRotateDegrees * 2 * UnityEngine.Random.value);
            spriteRenderer.transform.localScale = Vector3.zero;

            if (collider is not null)
            {
                collider.enabled = false;
            }

            UniTask.Void(async cancellationToken => {
                await UniTask.WaitForSeconds(minAppearDelaySec + (maxAppearDelaySec - minAppearDelaySec) * UnityEngine.Random.value, cancellationToken: cancellationToken);
                Vector3 targetScale = Vector3.one * (sizeMin + (sizeMax - sizeMin) * UnityEngine.Random.value);
                
                await spriteRenderer
                    .transform.DOScale(targetScale, 0.4f)
                    .SetEase(Ease.OutBack, 3)
                    .ToUniTask(cancellationToken: cancellationToken);

                if (collider is not null)
                {
                    collider.enabled = true;
                }
            }, destroyCancellationToken);
        }

        private void Update()
        {
            if (_lineAnimationActive)
            {
                _Line.DashOffset += Time.deltaTime * 1;
            }

            if (_lineAbsoluteScale)
            {
                _Line.transform.localScale = new Vector3(
                    1 / _Line.transform.parent.lossyScale.x,
                    1 / _Line.transform.parent.lossyScale.y,
                    1
                );
            }
        }

        private void OnMouseEnter()
        {
            if (!Line.enabled && GameManager.Instance.IsInputEnabled)
            {
                transform.DOKill();
                transform.localScale = Vector3.one;
                DOTween.Sequence()
                    .Append(transform.DOScale(Vector3.one * 1.15f, 0.08f).SetEase(Ease.OutCubic))
                    .Append(transform.DOScale(Vector3.one * 1.00f, 0.2f).SetEase(Ease.OutCubic));
            }

            OnHovered.Dispatch();
        }

        public void SetLineAnimationActive(bool value)
        {
            _lineAnimationActive = value;
            Line.Dashed = value;
        }

        public void DisableLineAbsoluteScale()
        {
            _lineAbsoluteScale = false;
        }
    }
}
