using APX.Audio.GameObjects;
using APX.Audio.Models.Definitions;

namespace APX.Audio.Utils
{
    public static class SFXExtensions
    {
        public static void PlayByAudioManager(this SFXDefinition sfxDefinition)
        {
            SFXManager.Instance.Play(sfxDefinition.SFXModel.GetSFXInstance());
        }
    }
}
