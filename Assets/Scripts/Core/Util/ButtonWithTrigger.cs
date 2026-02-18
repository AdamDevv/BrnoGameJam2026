using System.Collections.Generic;
using APX.Events;
using APX.Extra.OdinExtensions.Attributes;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using APX.Events.Editor;
#endif

namespace APGame.Util
{
    public class ButtonWithTrigger : MonoBehaviour
    {
        [EnhancedValueDropdown("GetEvents")]
        [SerializeReference]
        protected object _event;

        protected Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        protected virtual void OnClick()
        {
            EventBus.Trigger(_event);
        }

        #if UNITY_EDITOR

        [UsedImplicitly]
        private IEnumerable<ValueDropdownItem> GetEvents() => EventUtilsEditor.GetEventsValueDropdown();

        #endif
    }
}
