using DG.Tweening;
using UnityEngine;

namespace APX.Extra.Misc
{
    public static class CanvasGroupExtensions
    {
        public static void SetVisible(this CanvasGroup cg, bool visible)
        {
            cg.DOKill(false);
            cg.interactable = visible;
            cg.blocksRaycasts = visible;
            cg.alpha = visible ? 1f : 0f;
        }

        public static void SetVisible(this CanvasGroup cg, bool visible, float alphaFadeDuration, Ease alphaFadeEase = Ease.InOutCubic)
        {
            cg.DOKill(false);
            cg.interactable = visible;
            cg.blocksRaycasts = visible;
            cg.DOFade(visible ? 1f : 0f, alphaFadeDuration)
                .SetEase(alphaFadeEase);
        }
        
        public static Tweener DOVisible(this CanvasGroup cg, bool visible, float alphaFadeDuration, Ease alphaFadeEase = Ease.InOutCubic)
        {
            return cg.DOFade(visible ? 1f : 0f, alphaFadeDuration)
                .OnStart(() =>
                {
                    cg.interactable = visible;
                    cg.blocksRaycasts = visible;
                })
                .SetEase(alphaFadeEase);
        }
    }
}
