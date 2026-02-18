using APX.Extra.OdinExtensions;
using APX.Extra.OdinExtensions.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

#if UNITY_EDITOR
using APX.Audio.Utils;
#endif

namespace APX.Audio.Models.Data.Variations
{
    [System.Serializable]
    public class SFXAudioClipVariation
    {
        private const string SWITCH_TO_RANGE_ICON = "ArrowsLeftRightToLineSolid";
        private const string SWITCH_TO_SINGLE_ICON = "ReflectHorizontalSolid";
        private const string SWITCH_TO_RANGE_TOOLTIP = "Switch to range";
        private const string SWITCH_TO_SINGLE_TOOLTIP = "Switch to single";

        [Required]
        [SerializeField]
        [AssetSelector]
        [InlineIconButton(EditorIconsBundle.FontAwesome, "StopSolid", "StopPreview")]
        [InlineIconButton(EditorIconsBundle.FontAwesome, "PlaySolid", "PlayPreview")]
        private AudioClip _AudioClip;

        [Range(0f, 1f)]
        [InlineIconButton(EditorIconsBundle.FontAwesome, SWITCH_TO_RANGE_ICON, "ToggleHasVolumeRange", tooltip: SWITCH_TO_RANGE_TOOLTIP)]
        [SerializeField]
        [HideIf(nameof(_HasVolumeRange), Animate = false)]
        private float _Volume = 0.5f;

        [MinMaxSlider(0f, 1f, true)]
        [InlineIconButton(EditorIconsBundle.FontAwesome, SWITCH_TO_SINGLE_ICON, "ToggleHasVolumeRange", tooltip: SWITCH_TO_SINGLE_TOOLTIP)]
        [SerializeField]
        [ShowIf(nameof(_HasVolumeRange), Animate = false)]
        private Vector2 _VolumeRange = Vector2.one;

        [Range(0f, 3f)]
        [InlineIconButton(EditorIconsBundle.FontAwesome, SWITCH_TO_RANGE_ICON, "ToggleHasPitchRange", tooltip: SWITCH_TO_RANGE_TOOLTIP)]
        [SerializeField]
        [HideIf(nameof(_HasPitchRange), Animate = false)]
        private float _Pitch = 1f;

        [MinMaxSlider(0f, 3f, true)]
        [InlineIconButton(EditorIconsBundle.FontAwesome, SWITCH_TO_SINGLE_ICON, "ToggleHasPitchRange", tooltip: SWITCH_TO_SINGLE_TOOLTIP)]
        [SerializeField]
        [ShowIf(nameof(_HasPitchRange), Animate = false)]
        private Vector2 _PitchRange = Vector2.one;

        [AssetSelector]
        [SerializeField]
        private AudioMixerGroup _MixerGroup;

        [SerializeField]
        [HideInInspector]
        private bool _HasVolumeRange;

        [SerializeField]
        [HideInInspector]
        private bool _HasPitchRange;

        public SFXInstance GetSFXInstance()
        {
            var volume = _HasVolumeRange ? Random.Range(_VolumeRange.x, _VolumeRange.y) : _Volume;
            var pitch = _HasPitchRange ? Random.Range(_PitchRange.x, _PitchRange.y) : _Pitch;

            return new SFXInstance(_AudioClip, volume, pitch, _MixerGroup);
        }

        #if UNITY_EDITOR

        private void PlayPreview()
        {
            var instance = GetSFXInstance();
            EditorAudioUtils.PlayPreview(instance.AudioClip, instance.Volume, instance.Pitch, instance.MixerGroup);
        }

        private void StopPreview()
        {
            EditorAudioUtils.StopPreview();
        }

        private void ToggleHasVolumeRange()
        {
            _HasVolumeRange = !_HasVolumeRange;
        }

        private void ToggleHasPitchRange()
        {
            _HasPitchRange = !_HasPitchRange;
        }
        #endif
    }
}
