using APGame.Abstractions;
using APX.Util.OdinAttributes;
using UnityEngine;

namespace APGame.LevelFeatures.RotateClock
{
    public class RotatingClock : MonoBehaviour, IClock
    {
        [SerializeField] [RequiredChild] private RotatingClockDisc _ClockDisc;

        public float Angle => _ClockDisc.transform.eulerAngles.z + 360 % 360;
    }
}
