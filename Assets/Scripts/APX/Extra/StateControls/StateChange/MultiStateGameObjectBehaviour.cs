using System;
using System.Collections.Generic;
using System.Linq;
using APX.Extra.OdinExtensions;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace APX.Extra.StateControls.StateChange
{
    public class MultiStateGameObjectBehaviour : AMultiStateBehaviour
    {
        [FormerlySerializedAs("statePresets")]
        [SerializeField]
        [InlineProperty]
        [HideReferenceObjectPicker]
        [CustomValueDrawer("CustomStatePresetDrawer")]
        internal List<GameObjectItem> _StatePresets = new List<GameObjectItem>();

        public override bool ApplyState(string stateID)
        {
            base.ApplyState(stateID);
            if (_StatePresets == null) return false;
            if (_StatePresets.All(p => p.StateID != stateID))
            {
                Debug.LogError($"[MultiStateBehaviour] No state with ID '{stateID}' at {name}", this);
                return false;
            }

            foreach (var statePreset in _StatePresets)
            {
                statePreset.SetActive(false);
            }
            foreach (var statePreset in _StatePresets)
            {
                if (statePreset.StateID != stateID) continue;
                statePreset.SetActive(true);
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
        private GameObjectItem CustomStatePresetDrawer(GameObjectItem value, GUIContent label, Func<GUIContent, bool> callNextDrawer)
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
            _StatePresets.Add(new GameObjectItem(id));
        }

        internal override bool RemoveState(string id)
        {
            return _StatePresets.RemoveAll(s => s.StateID == id) > 0;
        }
#endif
    }
    
    [Serializable]
    public class GameObjectItem
    {
        [FormerlySerializedAs("stateID")]
        [SerializeField]
        private string _StateID;

        [FormerlySerializedAs("targets")]
        [SerializeField]
        private List<GameObject> _Targets = new List<GameObject>();

        public string StateID => _StateID;
        public List<GameObject> Targets => _Targets;

        public void SetActive(bool active)
        {
            foreach (var target in _Targets)
            {
                if(target != null) target.SetActive(active);
            }
        }

        public GameObjectItem() { }

        public GameObjectItem(string stateID, List<GameObject> targets = null)
        {
            _StateID = stateID;
            _Targets = targets ?? new List<GameObject>();
        }
    }
}
