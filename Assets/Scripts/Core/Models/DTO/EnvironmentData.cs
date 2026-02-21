using System;
using APGame.Enums;
using APX.Util.OdinAttributes;
using UnityEngine;

namespace APGame.Models.DTO
{
    [Serializable]
    public class EnvironmentData
    {
        [SerializeField] [RequiredAsset] private GameObject _EnvironmentPrefab;
        [SerializeField] private EnvironmentLayerAppearAnimation _EnvironmentLayerAppearAnimation;

        public GameObject EnvironmentPrefab => _EnvironmentPrefab;

        public EnvironmentLayerAppearAnimation EnvironmentLayerAppearAnimation => _EnvironmentLayerAppearAnimation;
    }
}
