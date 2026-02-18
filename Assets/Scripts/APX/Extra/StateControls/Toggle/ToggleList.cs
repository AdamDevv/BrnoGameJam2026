using System;
using APX.Extra.Presets;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace APX.Extra.StateControls.Toggle
{
    public class ToggleList : AToggleableBehaviour
    {
#region Public Fields
        public string ToggleID;

        [PropertyOrder(10)]
        public Togglable[] Toggles;

        [PropertyOrder(20)]
        public AToggleableBehaviour[] ToggleableBehaviours;
#endregion


#region Public Methods
        public override void UpdateState(bool immediate)
        {
            if (Toggles != null)
            {
                for (int i = 0; i < Toggles.Length; i++)
                {
                    Toggles[i].SetState(_state);
                }
            }

            if (ToggleableBehaviours != null)
            {
                for (int i = 0; i < ToggleableBehaviours.Length; i++)
                {
                    ToggleableBehaviours[i].State = _state;
                }
            }
        }
#endregion
    }


    [Serializable]
    public struct Togglable : IToggleable
    {
        public enum VisibilityChange
        {
            Ignore,
            Activate,
            Deactivate,
            Enable,
            Disable
        }


#region Public Fields
        [SerializeField, HideInInspector]
        private GameObject _target;
        [ShowInInspector, LabelText("### Target"), PropertyOrder(-10)]
        public GameObject Target { get => _target; set => UpdateTarget(value); }

        [SerializeField, ReadOnly, ShowIf("_targetImage"), PropertyOrder(-9)]
        private Image _targetImage;

        [SerializeField, ReadOnly, ShowIf("_targetText"), PropertyOrder(-9)]
        private TMP_Text _targetText;

        [ShowIfGroup("Visibility", Condition = "IsTargetAvailable")]
        [BoxGroup("Visibility/Box", false), PropertyOrder(-8)]
        public bool ChangeVisibility;
        [ShowIf("ChangeVisibility"), BoxGroup("Visibility/Box"), PropertyOrder(-7)]
        public VisibilityChange OnStateSet;
        [ShowIf("ChangeVisibility"), BoxGroup("Visibility/Box"), PropertyOrder(-6)]
        public VisibilityChange OffStateSet;

        [ShowIfGroup("Sprite", Condition = "IsChangeSpriteAvailable")]
        [BoxGroup("Sprite/Box", false)]
        public bool ChangeSprite;
        [ShowIf("ChangeSprite"), BoxGroup("Sprite/Box")]
        public SpriteStylePreset OnSprite;
        [ShowIf("ChangeSprite"), BoxGroup("Sprite/Box")]
        public SpriteStylePreset OffSprite;

        [ShowIfGroup("Color", Condition = "IsChangeColorAvailable")]
        [BoxGroup("Color/Box", false)]
        public bool ChangeColor;
        [ShowIf("ChangeColor"), BoxGroup("Color/Box")]
        public ColorStylePreset OnColor;
        [ShowIf("ChangeColor"), BoxGroup("Color/Box")]
        public ColorStylePreset OffColor;

        [SerializeField, HideInInspector]
        private bool _state;

        [ShowInInspector]
        [InlineButton("UpdateState", "Update State")]
        public bool State { get => _state; set => SetState(value); }
#endregion


#region Private Methods
#if UNITY_EDITOR
        private bool IsTargetAvailable() { return _target != null; }
        private bool IsChangeSpriteAvailable() { return _targetImage != null; }
        private bool IsChangeColorAvailable() { return _targetImage != null || _targetText != null; }

        private string GetTargetName() { return _target == null ? "---" : _target.name; }
#endif

        private void UpdateTarget(GameObject value)
        {
            _target = value;
            _targetImage = _target != null ? _target.GetComponent<Image>() : null;
            _targetText = _target != null ? _target.GetComponent<TMP_Text>() : null;
        }

        public void SetState(bool state)
        {
            _state = state;
            UpdateState();
        }

        public void UpdateState()
        {
            bool isVisible = true;
            if (ChangeVisibility)
            {
                if (_state == true)
                {
                    isVisible = SetVisibleState(OnStateSet);
                }
                else
                {
                    isVisible = SetVisibleState(OffStateSet);
                }
            }

            if (!isVisible) return;

            if (_targetImage != null)
            {
                if (_state == true)
                {
                    if (ChangeSprite && OnSprite != null) _targetImage.sprite = OnSprite.Sprite;
                    if (ChangeColor && OnColor != null) _targetImage.color = OnColor.Color;
                }
                else
                {
                    if (ChangeSprite && OffSprite != null) _targetImage.sprite = OffSprite.Sprite;
                    if (ChangeColor && OffColor != null) _targetImage.color = OffColor.Color;
                }
            }

            if (_targetText != null)
            {
                if (_state == true)
                {
                    if (ChangeColor && OnColor != null) _targetText.color = OnColor.Color;
                }
                else
                {
                    if (ChangeColor && OffColor != null) _targetText.color = OffColor.Color;
                }
            }
        }

        private bool SetVisibleState(VisibilityChange state)
        {
            bool isVisible = true;
            switch (state)
            {
                case VisibilityChange.Ignore:
                    break;
                case VisibilityChange.Enable:
                    if (_targetImage != null) _targetImage.enabled = true;
                    if (_targetText != null) _targetText.enabled = true;
                    break;
                case VisibilityChange.Disable:
                    if (_targetImage != null) _targetImage.enabled = false;
                    if (_targetText != null) _targetText.enabled = false;
                    isVisible = false;
                    break;
                case VisibilityChange.Activate:
                    if (_target != null) _target.SetActive(true);
                    break;
                case VisibilityChange.Deactivate:
                    if (_target != null) _target.SetActive(false);
                    isVisible = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return isVisible;
        }
#endregion
    }
}