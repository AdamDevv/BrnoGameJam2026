using DG.Tweening;

namespace APX.Extra.Tweens
{
    public interface ITweenAnimationPlayer
    {
        Tween Play();
        void Kill(bool complete = false);
        void Complete();
        void Resume();
        void Pause();
        void Rewind();
        
        bool IsPlaying { get; }
        bool IsPaused { get; }
        bool IsInitialized { get; }
    }
}
