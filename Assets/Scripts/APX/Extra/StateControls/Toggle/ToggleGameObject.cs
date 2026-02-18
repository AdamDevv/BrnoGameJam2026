using UnityEngine;

namespace APX.Extra.StateControls.Toggle
{
    public class ToggleGameObject : AToggleableBehaviour
    {
#region Public Fields
        public GameObject OnGameObject;
        public GameObject OffGameObject;
#endregion


#region Public Methods
        public override void UpdateState(bool immediate)
        {
            if (OnGameObject != null) OnGameObject.SetActive(_state);
            if (OffGameObject != null) OffGameObject.SetActive(!_state);
        }
#endregion
    }
}
