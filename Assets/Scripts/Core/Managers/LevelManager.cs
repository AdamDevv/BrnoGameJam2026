using System;
using APGame.Abstractions;
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
        private int _currentLevelIndex = -1;
        private bool _isGoalInitialized;

        public void SetNextLevel()
        {
            _isGoalInitialized = false;
            _currentLevelIndex++;
            _currentLevelGoal = GameplaySettings.Instance.Levels[_currentLevelIndex];
        }

        private void Update()
        {
            if (_currentLevelGoal is not null && _isGoalInitialized)
            {
                _currentLevelGoal?.Update();
                
            }
        }

        public async UniTaskVoid SetLevelGoalFinished()
        {
            SetNextLevel();
            await UniTask.WaitForSeconds(0.5f);
            
            GameManager.Instance.LevelText.transform.DOScaleY(0, 0.1f).SetEase(Ease.InBack).ToUniTask(cancellationToken: destroyCancellationToken).Forget();
            await ClockManager.Instance.Clock.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).ToUniTask(cancellationToken: destroyCancellationToken);
            
            _currentLevelGoal.Initialize();
            GameManager.Instance.LevelText.text = _currentLevelGoal.GetLevelText();
            _isGoalInitialized = true;
            
            GameManager.Instance.LevelText.transform.DOScaleY(1, 0.1f).SetEase(Ease.InBack).ToUniTask(cancellationToken: destroyCancellationToken).Forget();
            await ClockManager.Instance.Clock.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutCubic).ToUniTask(cancellationToken: destroyCancellationToken);
            
            await UniTask.WaitForSeconds(0.5f);
            
            //enable controls
        }
    }
}
