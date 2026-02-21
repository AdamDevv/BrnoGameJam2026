using APGame.InGame;
using APX.Managers.GameObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace APGame.Managers
{
    public class ClockManager : ASingleton<ClockManager>
    {
        [SerializeField] 
        [Required]
        private Clock _Clock;

        public Clock Clock => _Clock;
    }
}
