using APGame.Managers;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace APGame.LevelFeatures.Golf
{
    public class GolfBall : MonoBehaviour
    {
        private Rigidbody2D _rigidBody;
        private Vector3 _startPos;
        private bool _ready = true;
        private bool _clicked;
        private float _clickTimer;


        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _rigidBody.bodyType = RigidbodyType2D.Kinematic;
            _startPos = transform.localPosition;
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.M))
            {
                _rigidBody.bodyType = RigidbodyType2D.Kinematic;
                _rigidBody.linearVelocity = Vector2.zero;
                _rigidBody.angularVelocity = 0;
                transform.localPosition = _startPos;
                transform.rotation = Quaternion.identity;
                _clicked = false;
            }
#endif
        }

        private void OnMouseDown()
        {
            if (!_ready) return;

            if (_clicked)
            {
                _rigidBody.bodyType = RigidbodyType2D.Kinematic;
                _rigidBody.linearVelocity = Vector2.zero;
                _rigidBody.angularVelocity = 0;

                UniTask.Void(async cancellationToken => {
                    await DOTween.Sequence()
                        .Append(transform.DOLocalMove(_startPos, 0.5f).SetEase(Ease.OutQuint))
                        .Join(transform.DORotate(Vector3.zero, 0.5f).SetEase(Ease.OutQuint))
                        .ToUniTask(cancellationToken: cancellationToken);

                    _ready = true;
                }, LevelManager.Instance.destroyCancellationToken);

                _ready = false;
                _clicked = false;
                return;
            }

            _clicked = true;
            _clickTimer = 0;
            _rigidBody.angularDamping = 5;
            _rigidBody.gravityScale = 2;
            _rigidBody.bodyType = RigidbodyType2D.Dynamic;
            // _rigidBody.AddForce(new Vector2(-8.8f, 16), ForceMode2D.Impulse);
            _rigidBody.AddForce(new Vector2(-8.5f, 15), ForceMode2D.Impulse);
            // _rigidBody.AddForce(new Vector2(-12.5f, 5), ForceMode2D.Impulse);
        }
    }
}
