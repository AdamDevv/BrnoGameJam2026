using System.Collections.Generic;

namespace APX.Extra.StateControls.Toggle
{
    public class ToggleItems : AToggleableBehaviour
    {
#region Public Fields
        public List<AToggleableBehaviour> Items;
#endregion


#region Public Methods
        public virtual void SetToggleableAt(int index, bool state) { Items[index].State = state; }

        public override void UpdateState(bool immediate)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                SetToggleableAt(i, _state);
            }
        }
#endregion
    }
}
