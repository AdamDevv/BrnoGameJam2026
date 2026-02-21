using System;
using APGame.Enums;
using APGame.Managers;
using UnityEngine;

namespace APGame.InGame
{
    public class ClockHand : MonoBehaviour
    {
        public int DisplayedValue { get; private set; }

        [SerializeField] private ClockHandType _ClockHandType;
        private int _positionCount;
        private int _positionAngle;

        private void Start()
        {
            _positionCount = _ClockHandType switch
            {
                ClockHandType.Minute => 60,
                ClockHandType.Hour => 12,
                _ => throw new ArgumentOutOfRangeException()
            };
            _positionAngle = 360 / _positionCount;

            switch (_ClockHandType)
            {
                case ClockHandType.Minute:
                    GameManager.Instance.MinuteClockHand = this;
                    break;
                case ClockHandType.Hour:
                    GameManager.Instance.HourClockHand = this;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnMouseDrag()
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 worldMousePos = CameraManager.MainCamera.ScreenToWorldPoint(mousePos);
            Vector2 direction = worldMousePos - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            // angle = Mathf.Round(angle / _positionAngle) * _positionAngle;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            UpdateDisplayedValue();
        }

        private void UpdateDisplayedValue()
        {
            float normalized = 360 - transform.rotation.eulerAngles.z + 360 % 360;
            DisplayedValue = Mathf.FloorToInt((normalized / 360f) * _positionCount);
        }

        public void SetDisplayedValue(int targetHour)
        {
            DisplayedValue = targetHour % _positionCount;
            float angle = 360 - ((float)DisplayedValue / _positionCount) * 360 + 360 % 360;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
