using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace APX.Extra.StateControls.Toggle
{
    public class ToggleActiveObjects : AToggleableBehaviour
    {
        [PropertySpace(4)]
        [SerializeField]
        [ListDrawerSettings(ShowFoldout = false)]
        internal List<GameObject> _OnTargets = new List<GameObject>();

        [SerializeField]
        [ListDrawerSettings(ShowFoldout = false)]
        internal List<GameObject> _OffTargets = new List<GameObject>();

        public override void UpdateState(bool immediate)
        {
            SetTargetsActive(!State, false);
            SetTargetsActive(State, true);
        }

        private void SetTargetsActive(bool state, bool active)
        {
            var targets = state ? _OnTargets : _OffTargets;
            foreach (var target in targets)
            {
                if (target == null) continue;
                target.SetActive(active);
            }
        }
    }
}
