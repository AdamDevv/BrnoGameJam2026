using System;
using APGame.Abstractions;
using APGame.Managers;
using UnityEngine;

namespace APGame.LevelFeatures.RotateClock
{
    [Serializable]
    public class RotateClockGoal : ILevelGoal
    {
        private const int TOLERANCE_DEGREES = 2;

        [SerializeField] private int _TargetClockRotation;
        [SerializeField] private int _DisplayedTargetHour;
        [SerializeField] private int _DisplayedTargetMinute;
        private RotatingClock _clock;

        public void Initialize()
        {
            _clock = ClockManager.Instance.Clock as RotatingClock;
        }

        public void Update()
        {
            var diff = Mathf.Abs(_clock.Angle - _TargetClockRotation);
            if (diff < TOLERANCE_DEGREES && !Input.GetMouseButton(0))
            {
                LevelManager.Instance.SetLevelGoalFinished();
            }
        }

        public string GetLevelText()
        {
            return $"Set time to {_DisplayedTargetHour}:{_DisplayedTargetMinute:00}";
        }
    }
}
