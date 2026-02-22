using APGame.LevelFeatures.Common;
using APGame.Managers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace APGame.LevelFeatures.SkeletonHand
{
    public class SkeletonHandGoal : SetTimeGoal
    {
        private SkeletonHand _skeletonHand;

        public override void Initialize()
        {
            base.Initialize();
            UniTask.Void(async cancellationToken => {
                await UniTask.WaitForSeconds(0.1f, cancellationToken: cancellationToken);
                _skeletonHand = Object.FindAnyObjectByType<SkeletonHand>();
            }, LevelManager.Instance.destroyCancellationToken);
        }

        public override void Update()
        {
            if (IsTimeWithinTolerance() && _skeletonHand.IsClockHandAttached)
            {
                FinishLevel();
            }
        }
    }
}
