using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace APX.Extra.StateControls.Toggle
{
    public class ToggleState : AToggleableBehaviour
    {
    #region Public Fields
        [FormerlySerializedAs("_objectToggleStatePreset")]
        [SerializeField, ShowInInspector, ListDrawerSettings(CustomAddFunction = "CustomAddFunction"), HideReferenceObjectPicker]
        internal List<ObjectToggleStatePreset> _ObjectToggleStatePreset = new List<ObjectToggleStatePreset>();
    #endregion


    #region Public Methods
        public override void UpdateState(bool immediate = false)
        {
            if (_ObjectToggleStatePreset == null) return;

            for (var i = 0; i < _ObjectToggleStatePreset.Count; i++)
            {
                var statePreset = _ObjectToggleStatePreset[i];
                if (statePreset != null)
                    statePreset.State = _state;
            }
        }
    #endregion


    #region Private Methods
#if UNITY_EDITOR
        private void CustomAddFunction() { _ObjectToggleStatePreset.Add(new ObjectToggleStatePreset()); }
#endif
    #endregion
    }
}
