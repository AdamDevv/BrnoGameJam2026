using APGame.LevelFeatures.Common;
using APGame.Managers;
using UnityEngine;

namespace APGame.LevelFeatures.RearrangedNumbers
{
    public class RearrangedNumbersGoal : SetTimeGoal
    {
        const float MINUTE_TOLERANCE_PERCENTAGE = 0.025f;
        const float HOUR_TOLERANCE_PERCENTAGE = 0.06f;

        public override void Initialize()
        {
            _StartHour = 8;
            _StartMinute = 0;
            _TargetHour = 4;
            _TargetMinute = 30;
            
            base.Initialize();
        }

        public override void Update()
        {
            var minuteDiff = Mathf.Abs(ClockManager.Instance.Clock.MinuteHand.ValueSmooth - 40) / 60;
            var hourDiff = Mathf.Abs(ClockManager.Instance.Clock.HourHand.ValueSmooth - 11) / 12;

            if (minuteDiff < MINUTE_TOLERANCE_PERCENTAGE && hourDiff < HOUR_TOLERANCE_PERCENTAGE && !Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.L))
            {
                LevelManager.Instance.SetLevelGoalFinished();
            }
        }
    }
}
