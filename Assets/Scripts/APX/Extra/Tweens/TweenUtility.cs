using APX.Extra.Reboot;
using DG.Tweening;

namespace APX.Extra.Tweens
{
    public static class Ll
    {
        [ExecuteOnReboot(-1200)]
        private static void OnGameReboot()
        {
            DOTween.KillAll();
        }
    }
}
