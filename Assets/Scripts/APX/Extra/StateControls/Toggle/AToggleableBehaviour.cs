using APX.Extra.OdinExtensions;
using APX.Extra.OdinExtensions.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace APX.Extra.StateControls.Toggle
{
    [ScriptIcon(FontAwesomeEditorIconType.ToggleOffSolid, true)]
    public abstract class AToggleableBehaviour : MonoBehaviour, IToggleable
    {
        [FormerlySerializedAs("Description")]
        [SerializeField]
        private string _Description;

        protected bool _state;

        [ShowInInspector]
        [InlineButton("UpdateState", "Update State")]
        public virtual bool State
        {
            get => _state;
            set => SetState(value);
        }

        public virtual void SetState(bool state, bool immediate = false)
        {
            _state = state;
            UpdateState(immediate);
        }

        public abstract void UpdateState(bool immediate = false);
    }
}
