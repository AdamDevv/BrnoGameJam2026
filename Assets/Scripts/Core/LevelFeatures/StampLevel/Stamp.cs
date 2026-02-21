using DG.Tweening;
using UnityEngine;

namespace APGame.LevelFeatures.StampLevel
{
    public class Stamp : MonoBehaviour
    {
        public bool IsDetached { get; private set; }
        private int _clickCount;

        private Vector3 _startPos;

        private void Start()
        {
            _startPos = transform.localPosition;
        }

        private void OnMouseDown()
        {
            if (IsDetached) return;

            if (_clickCount < 3)
            {
                transform.DOKill();
                transform.position = _startPos;
                transform.eulerAngles = new Vector3(0, 0, 0);
                transform.DOShakeRotation(0.5f, new Vector3(0, 0, 10 + _clickCount * 7), randomness: 1, randomnessMode: ShakeRandomnessMode.Harmonic);
                _clickCount++;
                return;
            }

            IsDetached = true;
            transform.parent = null;
            var rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 2;
            rb.AddForce(new Vector2(-5f, 7f), ForceMode2D.Impulse);
            rb.AddTorque(-0.5f, ForceMode2D.Impulse);
        }
    }
}
