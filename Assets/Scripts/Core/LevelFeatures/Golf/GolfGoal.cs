using APGame.LevelFeatures.Common;
using APGame.Managers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace APGame.LevelFeatures.Golf
{
    public class GolfGoal : SetTimeGoal
    {
        private GolfBall _ball;

        public override void Initialize()
        {
            base.Initialize();
            UniTask.Void(async cancellationToken => {
                await UniTask.WaitForSeconds(3f, cancellationToken: cancellationToken);
                _ball = Object.FindAnyObjectByType<GolfBall>();
            }, LevelManager.Instance.destroyCancellationToken);
        }

        public override void Update()
        {
            if (_ball is null) return;

            if (_ball.transform.position.y < -23.5f)
            {
                Object.Destroy(GameObject.Find("Meadow_2_Front"));
                Object.Destroy(_ball);
                FinishLevel();
            }
        }

        public override string GetLevelText()
        {
            return "Score a hole in one!";
        }
    }
}
