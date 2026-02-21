using APGame.InGame;
using APX.Managers.GameObjects;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace APGame.Managers
{
    public class GameManager : ASingleton<GameManager>
    {
        public bool IsInputEnabled { get; set; } = false;
        
        [SerializeField] [Required] private TMP_Text _DebugTimeText;

        protected override void Initialize()
        {
            base.Initialize();
        }

        private async UniTask Start()
        {
            await UniTask.WaitForSeconds(0.3f);
            LevelManager.Instance.SetLevelGoalFinished();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        private void Update()
        {
            if (ClockManager.Instance.Clock is Clock clock)
            {
                _DebugTimeText.text = $"{clock.HourHand.Value:00}:{clock.MinuteHand.Value:00}";
            }
            else
            {
                _DebugTimeText.text = "--:--";
            }
        }
    }
}
