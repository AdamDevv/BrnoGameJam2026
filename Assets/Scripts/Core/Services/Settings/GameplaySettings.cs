using System.Collections.Generic;
using APGame.Abstractions;
using APX.GlobalLocating.Abstractions;
using UnityEngine;

namespace APGame.Services.Settings
{
    public class GameplaySettings : AGlobalLocatorObject<GameplaySettings>
    {
        [SerializeReference] private List<ILevelGoal> _Levels;

        public List<ILevelGoal> Levels => _Levels;
    }
}
