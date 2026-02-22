using UnityEngine;

namespace APGame.LevelFeatures.TreeBranch
{
    public class TreeBranchOneSlot : MonoBehaviour
    {
        [SerializeField] private int _Index;

        public int Index => _Index;
    }
}
