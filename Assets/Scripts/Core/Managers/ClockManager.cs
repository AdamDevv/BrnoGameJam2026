using APX.Managers.GameObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace APGame.Managers
{
    public class ClockManager : ASingleton<ClockManager>
    {
        [SerializeField] 
        [Required]
        private Transform _Clock;

        public Transform Clock => _Clock;
    }
}
