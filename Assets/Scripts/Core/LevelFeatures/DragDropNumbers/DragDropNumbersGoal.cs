using APGame.Abstractions;
using APGame.Managers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace APGame.LevelFeatures.DragDropNumbers
{
    [System.Serializable]
    public class DragDropNumbersGoal : ILevelGoal
    {
        [SerializeField] protected int _RequiredHourNumber;
        [SerializeField] protected int _RequiredMinuteNumber;

        [SerializeField] protected int _StartHourNumberPosition;
        [SerializeField] protected int _StartMinuteNumberPosition;

        private DragDropNumbersClock _clock;

        public void Initialize()
        {
            _clock = ClockManager.Instance.Clock as DragDropNumbersClock;

            _clock!.SetTime(_StartHourNumberPosition, _StartMinuteNumberPosition * 5);
        }

        public void Update()
        {
            if (!GameManager.Instance.IsInputEnabled) return;

            var hourMatches = _clock.GetNumberOnNumberPosition(_StartHourNumberPosition) == _RequiredHourNumber;
            var minuteMatches = _clock.GetNumberOnNumberPosition(_StartMinuteNumberPosition) == _RequiredMinuteNumber;

            if (hourMatches && minuteMatches && !Input.GetMouseButton(0))
            {
                GameManager.Instance.IsInputEnabled = false; // dirty hack
                UniTask.Void(async cancellationToken => {
                    await UniTask.WaitForSeconds(0.2f, cancellationToken: cancellationToken);
                    LevelManager.Instance.SetLevelGoalFinished();
                }, LevelManager.Instance.gameObject.GetCancellationTokenOnDestroy());
            }
        }

        public string GetLevelText()
        {
            return $"Set time to {_RequiredHourNumber}:{_RequiredMinuteNumber * 5:00}";
        }
    }
}
