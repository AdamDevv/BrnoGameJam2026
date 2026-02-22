using System.Linq;
using APGame.LevelFeatures.Common;
using APGame.Managers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace APGame.LevelFeatures.TreeBranch
{
    public class TreeBranchGoal : SetTimeGoal
    {
        private TreeBranchOne[] _branchOnes = System.Array.Empty<TreeBranchOne>();

        public override void Initialize()
        {
            base.Initialize();
            UniTask.Void(async cancellationToken =>
            {
                await UniTask.WaitForSeconds(3f, cancellationToken: cancellationToken);
                _branchOnes = Object.FindObjectsByType<TreeBranchOne>(FindObjectsSortMode.None);
            }, LevelManager.Instance.destroyCancellationToken);
        }

        public override void Update()
        {
            if (_branchOnes.Length == 0) return;
            
            if (IsTimeWithinTolerance() && _branchOnes.All(s => s.IsInPlace))
            {
                FinishLevel();
            }
        }
    }
}
