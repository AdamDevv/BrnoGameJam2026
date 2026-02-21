using System;
using APGame.Abstractions;
using APGame.InGame;
using APX.Managers.GameObjects;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace APGame.Managers
{
    public class GameManager : ASingleton<GameManager>
    {
        [SerializeField] [Required] private TMP_Text _LevelText;
        [SerializeField] [Required] private TMP_Text _DebugTimeText;

        public TMP_Text LevelText => _LevelText;

        protected override void Initialize()
        {
            base.Initialize();
        }

        private void Start()
        {
            _ = LevelManager.Instance.SetLevelGoalFinished();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        private void Update()
        {
            _DebugTimeText.text = $"{ClockManager.Instance.Clock.HourHand.Value:00}:{ClockManager.Instance.Clock.MinuteHand.Value:00}";
        }
    }
}
