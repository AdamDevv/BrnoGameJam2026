using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace APX.Extra.StateControls.Toggle
{
    [RequireComponent(typeof(Image))]
    public class ToggleImage : AToggleableBehaviour
    {
#region Public Fields
        public bool VisibleOnState = true;
        public bool VisibleOffState = true;

        [BoxGroup("Sprite", false)]
        public bool ChangeSprite;
        [ShowIf("ChangeSprite"), BoxGroup("Sprite")]
        public Sprite OnSprite;
        [ShowIf("ChangeSprite"), BoxGroup("Sprite")]
        public Sprite OffSprite;

        [BoxGroup("Color", false)]
        public bool ChangeColor;
        [ShowIf("ChangeColor"), BoxGroup("Color")]
        public Color OnStateColor = Color.white;
        [ShowIf("ChangeColor"), BoxGroup("Color")]
        public Color OffStateColor = Color.gray;
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
                        if (ChangeSprite) _image.sprite = OnSprite;
                        if (ChangeColor) _image.color = OnStateColor;
                    }
                }
                else
                {
                    _image.enabled = VisibleOffState;
                    if (VisibleOffState)
                    {
                        if (ChangeSprite) _image.sprite = OffSprite;
                        if (ChangeColor) _image.color = OffStateColor;
                    }
                }
            }
        }
#endregion
    }
}
