#if UNITY_EDITOR

using APX.Audio.Models.Definitions;
using APX.Extra.GameManagement;
using APX.Extra.OdinExtensions;
using UnityEngine;

namespace APX.Audio.ManagementPanels
{
    public class SFXDefinitionsManagement : ACollectionSelectorManagementPanel<SFXDefinition>
    {
        public override string PanelPath => "Audio/Sound effects";
        public override Texture PanelIcon => FontAwesomeEditorIcons.MusicRegular.Highlighted;
        public override float Priority => 0f;
    }
}

#endif