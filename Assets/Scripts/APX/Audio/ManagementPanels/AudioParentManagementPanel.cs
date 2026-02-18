#if UNITY_EDITOR

using APX.Extra.GameManagement;
using APX.Extra.OdinExtensions;
using UnityEngine;

namespace APX.Audio.ManagementPanels
{
    public class AudioParentManagementPanel : AParentManagementPanel
    {
        public override string PanelPath => "Audio";
        public override Texture PanelIcon => FontAwesomeEditorIcons.VolumeSolid.Highlighted;
    }
}

#endif