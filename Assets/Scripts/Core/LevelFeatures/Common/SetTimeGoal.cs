using System;
using APGame.Abstractions;
using APGame.Managers;
using UnityEngine;

namespace APGame.LevelFeatures.Common
{
    [Serializable]
    public class SetTimeGoal : ILevelGoal
    {
        private const float TOLERANCE = 0.01f;

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
            // var currentTotalMinutes = ClockManager.Instance.Clock.HourHand.Value * 60 + ClockManager.Instance.Clock.MinuteHand.Value;
            // var targetTotalMinutes = _TargetHour * 60 + _TargetMinute;
            // if (Mathf.Abs(currentTotalMinutes - targetTotalMinutes) > TOLERANCE)
            // {
            //     _ = LevelManager.Instance.SetLevelGoalFinished();
            // }

            if (ClockManager.Instance.Clock.HourHand.Value == _TargetHour && ClockManager.Instance.Clock.MinuteHand.Value == _TargetMinute)
            {
                // _ = LevelManager.Instance.SetLevelGoalFinished();
            }
        }

        public string GetLevelText()
        {
            return $"Set time to {_TargetHour}:{_TargetMinute:00}";
        }
    }
}
