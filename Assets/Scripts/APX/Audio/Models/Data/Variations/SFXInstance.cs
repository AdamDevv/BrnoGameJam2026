using UnityEngine;
using UnityEngine.Audio;

namespace APX.Audio.Models.Data.Variations
{
    public class SFXInstance
    {
        public AudioClip AudioClip { get; }
        public float Volume { get; }
        public float Pitch { get; }
        public AudioMixerGroup MixerGroup { get; }

        public SFXInstance(AudioClip audioClip, float volume, float pitch, AudioMixerGroup mixerGroup)
        {
            AudioClip = audioClip;
            Volume = volume;
            Pitch = pitch;
            MixerGroup = mixerGroup;
        }
    }
}
