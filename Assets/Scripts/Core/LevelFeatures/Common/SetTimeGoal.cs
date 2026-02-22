using APGame.Abstractions;
using APGame.InGame;
using APGame.Managers;
using UnityEngine;

namespace APGame.LevelFeatures.Common
{
    [System.Serializable]
    public class SetTimeGoal : ILevelGoal
    {
        private const int TOLERANCE_MINUTES = 2;

        [SerializeField] protected int _StartHour = 3;
        [SerializeField] protected int _StartMinute = 0;

        [SerializeField] protected int _TargetHour;
        [SerializeField] protected int _TargetMinute;
        
        protected Clock _clock;

        public virtual void Initialize()
        {
            _clock = ClockManager.Instance.Clock as Clock;
            _clock!.SetTime(_StartHour, _StartMinute);
        }

        public virtual void Update()
        {
            if (IsTimeWithinTolerance())
            {
                FinishLevel();
            }
        }

        protected void FinishLevel()
        {
            // _clock.SetTime(_TargetHour, _TargetMinute);
            LevelManager.Instance.SetLevelGoalFinished();
        }

        protected bool IsTimeWithinTolerance()
        {
            int currentTotalMinutes = _clock.HourHand.Value * 60 + _clock.MinuteHand.Value;
            int targetTotalMinutes = _TargetHour * 60 + _TargetMinute;
            return Mathf.Abs(currentTotalMinutes - targetTotalMinutes) < TOLERANCE_MINUTES && !Input.GetMouseButton(0);
        }

        public virtual string GetLevelText()
        {
            return $"Set time to {_TargetHour}:{_TargetMinute:00}";
        }
    }
}
