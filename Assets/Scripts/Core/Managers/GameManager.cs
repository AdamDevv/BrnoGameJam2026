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
            int targetIndex = GetIndexFromCzechKeys(Input.inputString);
            if (targetIndex != -1)
            {
                LevelManager.Instance.DebugSetLevel(targetIndex);
            }

            if (Input.GetKey(KeyCode.L))
            {
                LevelManager.Instance.SetLevelGoalFinished();
            }

            if (ClockManager.Instance.Clock is Clock clock)
            {
                _DebugTimeText.text = $"{clock.HourHand.Value:00}:{clock.MinuteHand.Value:00}";
            }
            else
            {
                _DebugTimeText.text = "--:--";
            }
        }

        private int GetIndexFromCzechKeys(string input)
        {
            if (string.IsNullOrEmpty(input)) return -1;

            // Vezmeme první znak vstupu
            char c = input[0];

            return c switch
            {
                '+' => 1,
                'ě' => 2,
                'š' => 3,
                'č' => 4,
                'ř' => 5,
                'ž' => 6,
                'ý' => 7,
                'á' => 8,
                'í' => 9,
                'é' => 0,
                _ => -1 // Pokud stiskneš cokoli jiného, vrátí -1
            };
        }
    }
}
