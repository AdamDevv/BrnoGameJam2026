using APX.Extra.OdinExtensions.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace APX.Extra.StateControls.Toggle
{
    public class ToggleByButton : MonoBehaviour
    {
        [Required]
        [SerializeField]
        [ShowGetComponent]
        private Button _Button;

        [Required]
        [SerializeField]
        [ShowGetComponent]
        private AToggleableBehaviour _ToggleableBehaviour;

        protected virtual void Awake()
        {
            if (_Button == null) return;
            _Button.onClick.AddListener(OnButtonClicked);
        }

        protected virtual void OnDestroy()
        {
            if (_Button == null) return;
            _Button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            if (_ToggleableBehaviour == null) return;
            _ToggleableBehaviour.State = !_ToggleableBehaviour.State;
        }
    }
}
