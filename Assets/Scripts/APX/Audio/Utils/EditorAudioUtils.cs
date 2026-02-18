#if UNITY_EDITOR

using APX.Audio.Models.Data;
using APX.Audio.Models.Data.Variations;
using UnityEngine;
using UnityEngine.Audio;

namespace APX.Audio.Utils
{
    public static class EditorAudioUtils
    {
        private static AudioSource _previewAudioSource;

        private static AudioSource PreviewAudioSource
        {
            get
            {
                if (_previewAudioSource == null)
                {
                    var audioSourceObject = new GameObject("EditorAudioSource");
                    _previewAudioSource = audioSourceObject.AddComponent<AudioSource>();
                    audioSourceObject.hideFlags = HideFlags.HideAndDontSave;
                }

                return _previewAudioSource;
            }
        }

        public static void PlayPreview(SFXInstance sfxInstance)
        {
            if (sfxInstance is null)
            {
                return;
            }

            PlayPreview(sfxInstance.AudioClip, sfxInstance.Volume, sfxInstance.Pitch, sfxInstance.MixerGroup);
        }

        public static void PlayPreview(AudioClip clip, float volume = 0.5f, float pitch = 1f, AudioMixerGroup mixerGroup = null)
        {
            if (clip == null)
            {
                return;
            }

            AudioSource audioSource = PreviewAudioSource;

            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.clip = clip;
            audioSource.outputAudioMixerGroup = mixerGroup;
            audioSource.Play();
        }

        public static void StopPreview()
        {
            _previewAudioSource?.Stop();
        }
    }
}

#endif
