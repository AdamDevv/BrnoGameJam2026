using System.Linq;
using APGame.Managers;
using UnityEngine;

namespace APGame.LevelFeatures.DragDropNumbers
{
    public class DragNumber : MonoBehaviour
    {
        [SerializeField] private Sprite[] _NumberSprites;

        public int Number { get; private set; }
        public int PositionNumber { get; set; }

        private Vector2 _targetPosition;
        private DragDropNumbersClock _clock;
        private bool _isDragging;
        private SpriteRenderer _spriteRenderer;

        public void Initialize(int number, int positionNumber, DragDropNumbersClock clock)
        {
            _clock = clock;
            Number = number;
            PositionNumber = positionNumber;
            UpdateTargetPosition();
            transform.localPosition = _targetPosition;

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _NumberSprites[number - 1];
        }

        private void UpdateTargetPosition()
        {
            _targetPosition = _clock.GetTargetPositionForNumberPosition(PositionNumber);
        }

        private void Update()
        {
            if (_isDragging)
            {
                Vector3 mouseWorldPos = CameraManager.MainCamera.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z);

                if (Input.GetMouseButtonUp(0))
                {
                    var colidingDragNumber = _clock.DragNumbers.Except(new[] { this })
                        .Select(n => new { DragNumber = n, Distance = Vector2.Distance(transform.position, n.transform.position) })
                        .Where(x => x.Distance < 0.5f)
                        .OrderBy(x => x.Distance)
                        .FirstOrDefault()
                        ?.DragNumber;

                    if (colidingDragNumber is not null)
                    {
                        (PositionNumber, colidingDragNumber.PositionNumber) = (colidingDragNumber.PositionNumber, PositionNumber);
                        UpdateTargetPosition();
                        colidingDragNumber.UpdateTargetPosition();
                    }

                    _spriteRenderer.sortingOrder--;
                    _isDragging = false;
                }
            }
            else
            {
                transform.localPosition = Vector2.Lerp(transform.localPosition, _targetPosition, Time.deltaTime * 15f);
            }
        }

        private void OnMouseDown()
        {
            _spriteRenderer.sortingOrder++;
            _isDragging = true;
        }
    }
}
