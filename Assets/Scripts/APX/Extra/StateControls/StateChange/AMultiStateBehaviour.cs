using System;
using System.Collections.Generic;
using System.Linq;
using APX.Extra.OdinExtensions;
using APX.Extra.OdinExtensions.Attributes;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace APX.Extra.StateControls.StateChange
{
    [ScriptIcon(FontAwesomeEditorIconType.SquareSlidersSolid, true)]
    public abstract class AMultiStateBehaviour : MonoBehaviour
    {
        [FormerlySerializedAs("currentState")]
        [SerializeField]
        [HideInInspector]
        private string _CurrentState;

        [PropertyOrder(-1)]
        [PropertySpace(SpaceBefore = 0, SpaceAfter = 8)]
        [ShowInInspector]
        [ValueDropdown("GetAllStates")]
        [ValidateInput("ValidateCurrentState")]
        [HorizontalGroup("State")]
        public string CurrentState
        {
            get => _CurrentState;
            set
            {
                _CurrentState = value;
                ApplyState(_CurrentState);
            }
        }

        private void Awake()
        {
            ApplyState(_CurrentState);
        }

        public virtual bool ApplyState(string stateID)
        {
            _CurrentState = stateID;
            return true;
        }

        public virtual bool ApplyState(Enum stateID)
        {
            return ApplyState(stateID.ToString());
        }

        public abstract IEnumerable<string> GetAvailableStates();

#if UNITY_EDITOR
        [UsedImplicitly]
        private bool ValidateCurrentState(string value, ref string errorMessage, ref InfoMessageType? messageType)
        {
            if (GetAllStates().All(v => v != value))
            {
                errorMessage = "Default state is invalid!";
                messageType = InfoMessageType.Error;
                return false;
            }

            return true;
        }

        [UsedImplicitly]
        internal abstract IEnumerable<string> GetAllStates();

        [UsedImplicitly]
        internal abstract void AddState(string id);

        [UsedImplicitly]
        internal abstract bool RemoveState(string id);

        [Button("Refresh")]
        [HorizontalGroup("State", 60)]
        private void RefreshCurrentState()
        {
            ApplyState(_CurrentState);
        }
#endif
    }
}
