#if UNITY_EDITOR

using APX.Extra.GameManagement;
using APX.Extra.OdinExtensions;
using APX.Managers.Definitions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace APX.Managers.ManagementPanels
{
    internal class GlobalManagersManagement : AManagementPanel
    {
        [HideLabel]
        [ShowInInspector]
        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        public GlobalManagersSettings Locator
        {
            get => GlobalManagersSettings.Instance;
            // ReSharper disable once ValueParameterNotUsed
            set { }
        }

        public override string PanelPath => "Global Managers";
        public override Texture PanelIcon => FontAwesomeEditorIcons.UserTieSolid.Highlighted;
        public override float Priority => 50;
    }
}

#endif
