using UnityEngine;

namespace APX.Audio.Models.Data.Variations
{
    [System.Serializable]
    public class SFXAudioClipVariationInRange : SFXAudioClipVariation
    {
        [SerializeField]
        [HideInInspector]
        private bool _DebugSolo;

        public bool DebugSolo
        {
            get => _DebugSolo;
            set => _DebugSolo = value;
        }
    }
}
