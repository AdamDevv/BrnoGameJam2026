using UnityEngine;

namespace APX.Extra.StateControls.Toggle
{
    public class ToggleItemsGroup : ToggleItems
    {
#region Public Fields
        public int StartIndex;
        public int Count = -1;
#endregion


#region Public Methods
        public virtual void Setup(bool state, int startIndex, int count = -1)
        {
            StartIndex = startIndex;
            Count = count;
            State = state;
        }
#endregion


#region Public Methods
        public override void UpdateState(bool immediate)
        {
            int endIndex = Count > -1 ? StartIndex + Count : Items.Count;
            int maxIndex = Mathf.Max(endIndex, Items.Count);
            for (int i = 0; i < maxIndex; i++)
            {
                SetToggleableAt(i, i >= StartIndex && i < endIndex);
            }
        }
#endregion
    }
}
