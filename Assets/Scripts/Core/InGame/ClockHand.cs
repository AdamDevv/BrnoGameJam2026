using System;
using APGame.Enums;
using APGame.Managers;
using UnityEngine;

namespace APGame.InGame
{
    public class ClockHand : MonoBehaviour
    {
        public int Value
        {
            get => _value;
            private set
            {
                PreviousValue = _value;
                _value = value;
            }
        }

        public float ValueSmooth => (360 - transform.rotation.eulerAngles.z + 360 % 360) / 360 * _positionCount;

        public int PreviousValue { get; private set; }

        [SerializeField] private ClockHandType _ClockHandType;
        private int _positionCount;
        private int _positionAngle;
        private Clock _clock;
        private int _value;
        private float _currentDragAngleOffset;

        private void Awake()
        {
            _positionCount = _ClockHandType switch
            {
                ClockHandType.Minute => 60,
                ClockHandType.Hour => 12,
                _ => throw new ArgumentOutOfRangeException()
            };
            _positionAngle = 360 / _positionCount;
            UpdateValueByRotation();
        }

        private void Start()
        {
            _clock = ClockManager.Instance.Clock as Clock;
        }

        private void OnMouseDown()
        {
            if (!GameManager.Instance.IsInputEnabled) return;

            if (_ClockHandType == ClockHandType.Hour && _clock.ClockBehaviourType == ClockBehaviourType.HourDependent)
            {
                float normalizedAngle = 360 - transform.rotation.eulerAngles.z + 360 % 360;
                normalizedAngle %= _positionAngle;
                normalizedAngle -= 15;
                _currentDragAngleOffset = normalizedAngle;
            }
        }

        private void OnMouseDrag()
        {
            if (!GameManager.Instance.IsInputEnabled) return;

            Vector2 mousePos = Input.mousePosition;
            Vector2 worldMousePos = CameraManager.MainCamera.ScreenToWorldPoint(mousePos);
            Vector2 direction = worldMousePos - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90 + _currentDragAngleOffset;

            // Snap values
            // angle = Mathf.Round(angle / _positionAngle) * _positionAngle;

            transform.rotation = Quaternion.Euler(0, 0, angle);

            UpdateValueByRotation();

            float normalizedAngle = 360 - transform.rotation.eulerAngles.z + 360 % 360;

            switch (_ClockHandType)
            {
                case ClockHandType.Minute:
                    _clock.SetTimeUsingMinuteHand(Value);
                    break;
                case ClockHandType.Hour:
                    _clock.SetTimeUsingHourHand(Value, normalizedAngle);
                    break;
            }
        }

        private void UpdateValueByRotation()
        {
            float normalized = 360 - transform.rotation.eulerAngles.z + 360 % 360;
            Value = Mathf.FloorToInt((normalized / 360f) * _positionCount);
        }

        private void UpdateRotationByValue()
        {
            float normalizedValue = Value % _positionCount;
            float angle = 360 - (normalizedValue / _positionCount) * 360 + 360 % 360;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        public void SetValue(int value)
        {
            Value = value % _positionCount;

            UpdateRotationByValue();
        }

        public void SetValueSmooth(float targetValue)
        {
            Value = Mathf.FloorToInt(targetValue) % _positionCount;

            float normalizedValue = targetValue % _positionCount;
            float angle = 360 - (normalizedValue / _positionCount) * 360 + 360 % 360;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
