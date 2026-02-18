#if UNITY_EDITOR

using UnityEngine;

namespace APX.Extra.GameManagement.SharedPanels
{
    public class GlobalDataManagementPanel : AParentManagementPanel
    {
        public override string PanelPath { get; }
        public override Texture PanelIcon { get; }
    }
}

#endif
