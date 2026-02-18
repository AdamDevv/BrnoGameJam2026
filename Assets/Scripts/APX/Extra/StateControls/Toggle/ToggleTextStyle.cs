using System;
using APX.Extra.Presets;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace APX.Extra.StateControls.Toggle
{
    [RequireComponent(typeof(TMP_Text))]
    public class ToggleTextStyle : AToggleableBehaviour
    {
#region Public Fields
        public bool VisibleOnState = true;
        public bool VisibleOffState = true;

        [BoxGroup("Color", false)]
        public bool ChangeColor;
        [ShowIf("ChangeColor"), BoxGroup("Color")]
        public ColorStylePreset OnColor;
        [ShowIf("ChangeColor"), BoxGroup("Color")]
        public ColorStylePreset OffColor;
#endregion


#region Private Fields
        [NonSerialized]
        private TMP_Text _text;
#endregion


#region Public Methods
        public override void UpdateState(bool immediate)
        {
            if (_text == null) _text = GetComponent<TMP_Text>();
            if (_text != null)
            {
                if (_state == true)
                {
                    _text.enabled = VisibleOnState;
                    if (VisibleOnState)
                    {
                        if (ChangeColor && OnColor != null) _text.color = OnColor.Color;
                    }
                }
                else
                {
                    _text.enabled = VisibleOffState;
                    if (VisibleOffState)
                    {
                        if (ChangeColor && OffColor != null) _text.color = OffColor.Color;
                    }
                }
            }
        }
#endregion
    }
}
