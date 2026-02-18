using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace APX.Extra.StateControls.Toggle
{
    [RequireComponent(typeof(TMP_Text))]
    public class ToggleText : AToggleableBehaviour
    {
#region Public Fields
        public bool VisibleOnState = true;
        public bool VisibleOffState = true;

        [BoxGroup("Color", false)]
        public bool ChangeColor;
        [ShowIf("ChangeColor"), BoxGroup("Color")]
        public Color OnStateColor = Color.white;
        [ShowIf("ChangeColor"), BoxGroup("Color")]
        public Color OffStateColor = Color.gray;
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
                        if (ChangeColor) _text.color = OnStateColor;
                    }
                }
                else
                {
                    _text.enabled = VisibleOffState;
                    if (VisibleOffState)
                    {
                        if (ChangeColor) _text.color = OffStateColor;
                    }
                }
            }
        }
#endregion
    }
}
