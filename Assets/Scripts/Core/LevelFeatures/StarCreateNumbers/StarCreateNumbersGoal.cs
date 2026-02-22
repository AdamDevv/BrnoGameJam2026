using System.Linq;
using APGame.LevelFeatures.Common;
using APGame.Managers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace APGame.LevelFeatures.StarCreateNumbers
{
    [System.Serializable]
    public class StarCreateNumbersGoal : SetTimeGoal
    {
        private StarSystem[] _starSystems = System.Array.Empty<StarSystem>();

        public override void Initialize()
        {
            base.Initialize();
            UniTask.Void(async cancellationToken =>
            {
                await UniTask.WaitForSeconds(0.1f, cancellationToken: cancellationToken);
                _starSystems = Object.FindObjectsByType<StarSystem>(FindObjectsSortMode.None);
            }, LevelManager.Instance.destroyCancellationToken);
        }

        public override void Update()
        {
            if (IsTimeWithinTolerance() && _starSystems.All(s => s.IsInPlace))
            {
                FinishLevel();
            }
        }
    }
}
