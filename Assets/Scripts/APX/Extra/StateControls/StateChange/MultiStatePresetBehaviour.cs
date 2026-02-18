using System;
using System.Collections.Generic;
using System.Linq;
using APX.Extra.Misc;
using APX.Extra.OdinExtensions;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace APX.Extra.StateControls.StateChange
{
    public class MultiStatePresetBehaviour : AMultiStateBehaviour
    {
        [FormerlySerializedAs("_statePresets")]
        [SerializeField]
        [InlineProperty]
        [HideReferenceObjectPicker]
        [CustomValueDrawer("CustomStatePresetDrawer")]
        internal List<StatePreset> _StatePresets = new();

        public override bool ApplyState(string stateID)
        {
            base.ApplyState(stateID);
            if (_StatePresets == null) return false;
            if (_StatePresets.All(p => p.StateID != stateID))
            {
                Debug.LogError($"[MultiStateBehaviour] No state with ID '{stateID}' at object '{gameObject.GetPath()}'", this);
                return false;
            }
            foreach (var statePreset in _StatePresets)
            {
                if (statePreset.StateID != stateID) continue;
                statePreset.Apply();
                return true;
            }

            return false;
        }

        public override IEnumerable<string> GetAvailableStates()
        {
            return _StatePresets.Select(s => s.StateID);
        }

#if UNITY_EDITOR
        [UsedImplicitly]
        private StatePreset CustomStatePresetDrawer(StatePreset value, GUIContent label, Func<GUIContent, bool> callNextDrawer)
        {
            var previousBgColor = GUI.backgroundColor;
            GUI.backgroundColor = OdinColors.GetRainbowColor(_StatePresets.IndexOf(value), _StatePresets.Count);
            Sirenix.Utilities.Editor.SirenixEditorGUI.BeginBox();
            GUI.backgroundColor = previousBgColor;
            callNextDrawer(label);
            Sirenix.Utilities.Editor.SirenixEditorGUI.EndBox();
            return value;
        }

        internal override IEnumerable<string> GetAllStates()
        {
            return _StatePresets.Select(statePreset => statePreset.StateID);
        }

        internal override void AddState(string id)
        {
            _StatePresets.Add(new StatePreset(id));
        }

        internal override bool RemoveState(string id)
        {
            return _StatePresets.RemoveAll(s => s.StateID == id) > 0;
        }
#endif
    }
}
