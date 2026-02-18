using System;
using DG.Tweening;
using UnityEngine;

namespace APX.Extra.Tweens.ObjectActions
{
    [Serializable]
    public class CanvasGroupFadeTweenAction : ATweenObjectAction<CanvasGroup>
    {
        [SerializeField]
        private float _EndValue;

        protected override Tween GetTween(CanvasGroup target) => target.DOFade(_EndValue, _Duration);
    }
}