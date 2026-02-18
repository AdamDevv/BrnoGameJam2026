using APX.Extra.OdinExtensions.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace APX.Extra.StateControls.Toggle
{
    public class ToggleByUnityToggle : MonoBehaviour
    {
        [Required]
        [SerializeField]
        [ShowGetComponent]
        private UnityEngine.UI.Toggle _Toggle;

        [Required]
        [SerializeField]
        [ShowGetComponent]
        private AToggleableBehaviour _ToggleableBehaviour;

        protected virtual void Awake()
        {
            if (_Toggle == null) return;
            _Toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        private void Start()
        {
            if (_Toggle == null) return;
            _ToggleableBehaviour.State = _Toggle.isOn;
        }

        protected virtual void OnDestroy()
        {
            if (_Toggle == null) return;
            _Toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        }
        
        private void OnToggleValueChanged(bool value)
        {
            if (_ToggleableBehaviour == null) return;
            _ToggleableBehaviour.State = value;
        }
    }
}
