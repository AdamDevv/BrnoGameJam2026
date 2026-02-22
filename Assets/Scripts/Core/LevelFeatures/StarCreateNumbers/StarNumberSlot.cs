using UnityEngine;

namespace APGame.LevelFeatures.StarCreateNumbers
{
    public class StarNumberSlot : MonoBehaviour
    {
        [SerializeField] private int _Number;
        
        public int Number => _Number;
    }
}
