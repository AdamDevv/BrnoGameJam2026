using System;
using APGame.Abstractions;
using APGame.Enums;
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
        [SerializeField] [RequiredAsset] private GameObject _EnvironmentPrefab;
        [SerializeField] private EnvironmentLayerAppearAnimation _EnvironmentLayerAppearAnimation;

        public ILevelGoal LevelGoal => _LevelGoal;
        public GameObject ClockPrefab => _ClockPrefab;
        public GameObject EnvironmentPrefab => _EnvironmentPrefab;
        public EnvironmentLayerAppearAnimation EnvironmentLayerAppearAnimation => _EnvironmentLayerAppearAnimation;
    }
}
