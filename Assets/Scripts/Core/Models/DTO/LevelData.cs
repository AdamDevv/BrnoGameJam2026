using System;
using APGame.Abstractions;
using APX.Util.OdinAttributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace APGame.Models.DTO
{
    [Serializable]
    public class LevelData
    {
        [SerializeField] private string _Comment;
        [SerializeReference] private ILevelGoal _LevelGoal;
        [SerializeField] [RequiredAsset] private GameObject _ClockPrefab;

        [SerializeField] [Space] private bool _MultipleEnvironments;

        [SerializeField] [HideIf(nameof(_MultipleEnvironments))]
        private EnvironmentData _EnvironmentData;

        [SerializeField] [ShowIf(nameof(_MultipleEnvironments))]
        private EnvironmentData[] _MultiEnvironmentData;

        public ILevelGoal LevelGoal => _LevelGoal;
        public GameObject ClockPrefab => _ClockPrefab;
        public EnvironmentData[] EnvironmentData => _MultipleEnvironments ? _MultiEnvironmentData : new[] { _EnvironmentData };
    }
}
