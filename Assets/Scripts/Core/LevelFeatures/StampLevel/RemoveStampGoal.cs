using APGame.LevelFeatures.Common;
using UnityEngine;

namespace APGame.LevelFeatures.StampLevel
{
    public class RemoveStampGoal : SetTimeGoal
    {
        private Stamp _stamp;

        public override void Initialize()
        {
            base.Initialize();
            _stamp = Object.FindAnyObjectByType<Stamp>();
        }

        public override void Update()
        {
            if (IsTimeWithinTolerance() && _stamp.IsDetached)
            {
                FinishLevel();
            }
        }
    }
}
