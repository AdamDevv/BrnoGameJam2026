using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace APX.Extra.Tweens.ObjectActions
{
    [Serializable]
    public class TextMeshProColorTweenAction : ATweenObjectAction<TMP_Text>
    {
        [SerializeField]
        private Color _EndColor;

        protected override Tween GetTween(TMP_Text target) => target.DOColor(_EndColor, _Duration);
    }
    
    [Serializable]
    public class TextMeshProFaceColorTweenAction : ATweenObjectAction<TMP_Text>
    {
        [SerializeField]
        private Color _EndColor;

        protected override Tween GetTween(TMP_Text target) => target.DOFaceColor(_EndColor, _Duration);
    }
    
    [Serializable]
    public class TextMeshProGlowColorTweenAction : ATweenObjectAction<TMP_Text>
    {
        [SerializeField]
        private Color _EndColor;
        
        protected override Tween GetTween(TMP_Text target) => target.DOGlowColor(_EndColor, _Duration);
    }
    
    [Serializable]
    public class TextMeshProOutlineColorTweenAction : ATweenObjectAction<TMP_Text>
    {
        [SerializeField]
        private Color _EndColor;
        
        protected override Tween GetTween(TMP_Text target) => target.DOOutlineColor(_EndColor, _Duration);
    }
    
    [Serializable]
    public class TextMeshProFaceFadeTweenAction : ATweenObjectAction<TMP_Text>
    {
        [SerializeField]
        private float _EndValue;
        
        protected override Tween GetTween(TMP_Text target) => target.DOFaceFade(_EndValue, _Duration);
    }
    
    [Serializable]
    public class TextMeshProFadeTweenAction : ATweenObjectAction<TMP_Text>
    {
        [SerializeField]
        private float _EndValue;
        
        protected override Tween GetTween(TMP_Text target) => target.DOFade(_EndValue, _Duration);
    }
    
    [Serializable]
    public class TextMeshProFontSizeTweenAction : ATweenObjectAction<TMP_Text>
    {
        [SerializeField]
        private float _EndValue;
        
        protected override Tween GetTween(TMP_Text target) => target.DOFontSize(_EndValue, _Duration);
    }
    
    [Serializable]
    public class TextMeshProScaleTweenAction : ATweenObjectAction<TMP_Text>
    {
        [SerializeField]
        private float _EndValue;
        
        protected override Tween GetTween(TMP_Text target) => target.DOScale(_EndValue, _Duration);
    }
    
    [Serializable]
    public class TextMeshProMaxVisibleCharactersTweenAction : ATweenObjectAction<TMP_Text>
    {
        [SerializeField]
        private int _EndCharacterCount;

        protected override Tween GetTween(TMP_Text target) => target.DOMaxVisibleCharacters(_EndCharacterCount, _Duration);
    }
    
    [Serializable]
    public class TextMeshProTextTweenAction : ATweenObjectAction<TMP_Text>
    {
        [SerializeField]
        private string _EndText;
        
        [SerializeField]
        private bool _RichTextEnabled = true;
        
        [SerializeField]
        private ScrambleMode _ScrambleMode = ScrambleMode.None;
        
        [SerializeField]
        private string _ScrambleChars;

        protected override Tween GetTween(TMP_Text target) => target.DOText(_EndText, _Duration, _RichTextEnabled, _ScrambleMode, string.IsNullOrEmpty(_ScrambleChars) ? null : _ScrambleChars);
    }
}
