using APGame.InGame;
using APX.Managers.GameObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace APGame.Managers
{
    public class ClockManager : ASingleton<ClockManager>
    {
        [SerializeField] [Required] private Clock _Clock;

        public Clock Clock => _Clock;

        protected override void Initialize()
        {
            base.Initialize();
            if (_Clock)
            {
                Destroy(_Clock.gameObject);
                _Clock = null;
            }
        }

        public void UpdateClockObject(Clock clockPrefab)
        {
            if (_Clock)
            {
                Destroy(_Clock.gameObject);
                _Clock = null;
            }

            _Clock = Instantiate(clockPrefab);
            _Clock.transform.transform.localScale = Vector3.zero;
        }
    }
}
