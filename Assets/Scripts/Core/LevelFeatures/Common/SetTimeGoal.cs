using System;
using APGame.Abstractions;
using APGame.Managers;
using UnityEngine;

namespace APGame.LevelFeatures.Common
{
    [Serializable]
    public class SetTimeGoal : ILevelGoal
    {
        private const int TOLERANCE_MINUTES = 2;

        [SerializeField] private int _StartHour = 3;
        [SerializeField] private int _StartMinute = 0;

        [SerializeField] private int _TargetHour;
        [SerializeField] private int _TargetMinute;

        public void Initialize()
        {
            ClockManager.Instance.Clock.SetTime(_StartHour, _StartMinute);
        }

        public void Update()
        {
            int currentTotalMinutes = ClockManager.Instance.Clock.HourHand.Value * 60 + ClockManager.Instance.Clock.MinuteHand.Value;
            int targetTotalMinutes = _TargetHour * 60 + _TargetMinute;
            
            if ((Mathf.Abs(currentTotalMinutes - targetTotalMinutes) < TOLERANCE_MINUTES && !Input.GetMouseButton(0)) || Input.GetKeyDown(KeyCode.L))
            {
                ClockManager.Instance.Clock.SetTime(_TargetHour, _TargetMinute);
                LevelManager.Instance.SetLevelGoalFinished();
            }
        }

        public string GetLevelText()
        {
            return $"Set time to {_TargetHour}:{_TargetMinute:00}";
        }
    }
}
