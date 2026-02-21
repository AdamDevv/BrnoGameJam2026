using System;
using APGame.Abstractions;
using APGame.InGame;
using APX.Util.OdinAttributes;
using UnityEngine;

namespace APGame.Models.DTO
{
    [Serializable]
    public class LevelData
    {
        [SerializeReference] private ILevelGoal _LevelGoal;
        [SerializeField] [RequiredAsset] private Clock _ClockPrefab;

        public ILevelGoal LevelGoal => _LevelGoal;

        public Clock ClockPrefab => _ClockPrefab;
    }
}
