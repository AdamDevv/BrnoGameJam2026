using System;
using APGame.Abstractions;
using APX.Util.OdinAttributes;
using UnityEngine;

namespace APGame.Models.DTO
{
    [Serializable]
    public class LevelData
    {
        [SerializeField] private string _Comment;
        [SerializeReference] private ILevelGoal _LevelGoal;
        [SerializeField] [RequiredAsset] private GameObject _ClockPrefab;

        public ILevelGoal LevelGoal => _LevelGoal;

        public GameObject ClockPrefab => _ClockPrefab;
    }
}
