using APGame.Abstractions;
using APGame.Managers;
using UnityEngine;

namespace APGame.LevelFeatures.RotateClock
{
    public class RotatingClockDisc : MonoBehaviour, IClock
    {
        private float _dragOffset;

        private void OnMouseDown()
        {
            Vector2 worldMousePos = CameraManager.MainCamera
                .ScreenToWorldPoint(Input.mousePosition);

            Vector2 direction = worldMousePos - (Vector2)transform.position;

            float mouseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            _dragOffset = transform.eulerAngles.z - mouseAngle;
        }

        private void OnMouseDrag()
        {
            if (!GameManager.Instance.IsInputEnabled) return;

            Vector2 worldMousePos = CameraManager.MainCamera
                .ScreenToWorldPoint(Input.mousePosition);

            Vector2 direction = worldMousePos - (Vector2)transform.position;

            float mouseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            float finalAngle = mouseAngle + _dragOffset;

            transform.rotation = Quaternion.Euler(0, 0, finalAngle);
        }
    }
}