#if UNITY_EDITOR

using APX.Audio.Models.Definitions;
using APX.Extra.GameManagement;
using APX.Extra.OdinExtensions;
using UnityEngine;

namespace APX.Audio.ManagementPanels
{
    public class EventSFXDefinitionsManagement : ACollectionSelectorManagementPanel<EventSFXDefinition>
    {
        public override string PanelPath => "Audio/Event sound effects";
        public override Texture PanelIcon => FontAwesomeEditorIcons.SignalStreamSolid.Highlighted;
        public override float Priority => 10f;
        protected override bool ShowIconBeforeElement => true;

        protected override (Texture texture, Color? color) GetIconDataForElement(EventSFXDefinition element)
        {
            if (element.Disabled)
            {
                return (FontAwesomeEditorIcons.XmarkSolid.Active, new Color(1, 0.3f, 0.3f));
            }

            return (null, null);
        }
    }
}

#endif