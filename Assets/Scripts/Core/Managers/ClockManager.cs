using APGame.Abstractions;
using APX.Managers.GameObjects;
using UnityEngine;

namespace APGame.Managers
{
    public class ClockManager : ASingleton<ClockManager>
    {
        public IClock Clock { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();
            Destroy(GameObject.Find("DefaultClock"));
        }

        public void UpdateClockObject(GameObject clockPrefab)
        {
            if (Clock is not null)
            {
                Destroy(Clock.gameObject);
                Clock = null;
            }

            Clock = Instantiate(clockPrefab).GetComponent<IClock>();
            Clock.transform.localScale = Vector3.zero;
        }
    }
}
