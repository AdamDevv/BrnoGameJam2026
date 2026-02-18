using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace APX.Extra.Tweens.ObjectActions
{
    [Serializable]
    public class SliderValueTweenAction : ATweenObjectAction<Slider>
    {
        [SerializeField]
        private float _EndValue;
        
        [SerializeField]
        private bool _Snapping;

        protected override Tween GetTween(Slider target) => target.DOValue(_EndValue, _Duration, _Snapping);
    }
}