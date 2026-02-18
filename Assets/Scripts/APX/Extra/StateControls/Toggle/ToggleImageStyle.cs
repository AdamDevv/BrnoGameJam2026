using System;
using APX.Extra.Presets;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace APX.Extra.StateControls.Toggle
{
    [RequireComponent(typeof(Image))]
    public class ToggleImageStyle : AToggleableBehaviour
    {
#region Public Fields
        public bool VisibleOnState = true;
        public bool VisibleOffState = true;

        [BoxGroup("Sprite", false)]
        public bool ChangeSprite;
        [ShowIf("ChangeSprite"), BoxGroup("Sprite")]
        public SpriteStylePreset OnSprite;
        [ShowIf("ChangeSprite"), BoxGroup("Sprite")]
        public SpriteStylePreset OffSprite;

        [BoxGroup("Color", false)]
        public bool ChangeColor;
        [ShowIf("ChangeColor"), BoxGroup("Color")]
        public ColorStylePreset OnColor;
        [ShowIf("ChangeColor"), BoxGroup("Color")]
        public ColorStylePreset OffColor;
#endregion


#region Private Fields
        [NonSerialized]
        private Image _image;
#endregion


#region Public Methods
        public override void UpdateState(bool immediate)
        {
            if (_image == null) _image = GetComponent<Image>();
            if (_image != null)
            {
                if (_state == true)
                {
                    _image.enabled = VisibleOnState;
                    if (VisibleOnState)
                    {
                        if (ChangeSprite && OnSprite != null) _image.sprite = OnSprite.Sprite;
                        if (ChangeColor && OnColor != null) _image.color = OnColor.Color;
                    }
                }
                else
                {
                    _image.enabled = VisibleOffState;
                    if (VisibleOffState)
                    {
                        if (ChangeSprite && OffSprite != null) _image.sprite = OffSprite.Sprite;
                        if (ChangeColor && OffColor != null) _image.color = OffColor.Color;
                    }
                }
            }
        }
#endregion
    }
}
