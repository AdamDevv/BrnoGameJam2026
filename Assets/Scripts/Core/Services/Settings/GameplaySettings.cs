using System.Collections.Generic;
using APGame.Models.DTO;
using APX.GlobalLocating.Abstractions;
using UnityEngine;

namespace APGame.Services.Settings
{
    public class GameplaySettings : AGlobalLocatorObject<GameplaySettings>
    {
        [SerializeReference] private List<LevelData> _Levels;

        public IReadOnlyList<LevelData> Levels => _Levels;
    }
}
