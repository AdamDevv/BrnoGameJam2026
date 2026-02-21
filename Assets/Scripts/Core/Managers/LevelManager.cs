using APGame.Abstractions;
using APGame.Models.DTO;
using APGame.Services.Settings;
using APX.Managers.GameObjects;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace APGame.Managers
{
    public class LevelManager : ASingleton<LevelManager>
    {
        private ILevelGoal _currentLevelGoal;
        private LevelData _currentLevelData;
        private int _currentLevelIndex = -1;

        public void SetNextLevel()
        {
            _currentLevelIndex++;
            _currentLevelData = GameplaySettings.Instance.Levels[_currentLevelIndex];
            _currentLevelGoal = _currentLevelData.LevelGoal;
        }

        private void Update()
        {
            _currentLevelGoal?.Update();
        }

        public void SetLevelGoalFinished() => UniTask.Void(async cancellationToken => {
            GameManager.Instance.IsInputEnabled = false;
            _currentLevelGoal = null;

            if (ClockManager.Instance.Clock)
            {
                var seq = DOTween.Sequence()
                    .Append(ClockManager.Instance.Clock.transform.DOScale(Vector3.one * 1.05f, 0.08f).SetEase(Ease.OutCubic))
                    .Append(ClockManager.Instance.Clock.transform.DOScale(Vector3.one * 1.00f, 0.2f).SetEase(Ease.OutCubic));
                seq.ToUniTask(cancellationToken: cancellationToken).Forget();
                await UniTask.WaitForSeconds(1.5f, cancellationToken: cancellationToken);
            }

            // Hide animations
            if (ClockManager.Instance.Clock)
            {
                _ = UIManager.Instance.HideLevelText();
                await ClockManager.Instance.Clock.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).ToUniTask(cancellationToken: destroyCancellationToken);
            }

            // Initialize new level
            SetNextLevel();
            ClockManager.Instance.UpdateClockObject(_currentLevelData.ClockPrefab);
            _currentLevelGoal!.Initialize();

            await UniTask.Yield();
            UIManager.Instance.LevelText.text = _currentLevelGoal!.GetLevelText();

            // Show animations
            _ = UIManager.Instance.ShowLevelText();
            await ClockManager.Instance.Clock.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack).ToUniTask(cancellationToken: destroyCancellationToken);

            await UniTask.WaitForSeconds(0.5f, cancellationToken: cancellationToken);
            GameManager.Instance.IsInputEnabled = true;
        }, destroyCancellationToken);
    }
}
