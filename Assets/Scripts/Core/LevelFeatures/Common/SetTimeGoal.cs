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
            GameManager.Instance.HourClockHand.SetDisplayedValue(_StartHour);
            GameManager.Instance.MinuteClockHand.SetDisplayedValue(_StartMinute);
        }

        public void Update()
        {
            // var currentTotalMinutes = GameManager.Instance.HourClockHand.DisplayedValue * 60 + GameManager.Instance.MinuteClockHand.DisplayedValue;
            // var targetTotalMinutes = _TargetHour * 60 + _TargetMinute;
            // if (Mathf.Abs(currentTotalMinutes - targetTotalMinutes) > TOLERANCE)
            // {
            //     _ = LevelManager.Instance.SetLevelGoalFinished();
            // }

            if (GameManager.Instance.HourClockHand.DisplayedValue == _TargetHour && GameManager.Instance.MinuteClockHand.DisplayedValue == _TargetMinute)
            {
                _ = LevelManager.Instance.SetLevelGoalFinished();
            }
        }

        public string GetLevelText()
        {
            return $"Set time to {_TargetHour}:{_TargetMinute:00}";
        }
    }
}
