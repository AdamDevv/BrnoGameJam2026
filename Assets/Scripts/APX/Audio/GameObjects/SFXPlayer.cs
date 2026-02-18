using System.Collections.Generic;
using APX.Audio.Models.Data.Variations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace APX.Audio.GameObjects
{
    public class SFXPlayer : MonoBehaviour
    {
        const int MAX_AUDIO_SOURCES = 50;

        private readonly List<AudioSource> _audioSources = new();

        [Button]
        [DisableInEditorMode]
        public void Play(SFXInstance sfxInstance)
        {
            var audioSource = GetAudioSource();

            if (audioSource == null)
            {
                return;
            }

            audioSource.clip = sfxInstance.AudioClip;
            audioSource.volume = sfxInstance.Volume;
            audioSource.pitch = sfxInstance.Pitch;
            audioSource.outputAudioMixerGroup = sfxInstance.MixerGroup;
            audioSource.loop = false;

            audioSource.Play();
        }

        private AudioSource GetAudioSource()
        {
            var audioSource = _audioSources.Find(audioSource => !audioSource.isPlaying);

            if (audioSource)
            {
                return audioSource;
            }

            if (_audioSources.Count >= MAX_AUDIO_SOURCES)
            {
                Debug.LogError("Maximum number of audio sources reached. Cannot create new audio source.");
                return null;
            }

            var newAudioSourceObject = new GameObject("SoundEffectSource");
            newAudioSourceObject.transform.SetParent(transform);
            audioSource = newAudioSourceObject.AddComponent<AudioSource>();

            _audioSources.Add(audioSource);

            return audioSource;
        }
    }
}
