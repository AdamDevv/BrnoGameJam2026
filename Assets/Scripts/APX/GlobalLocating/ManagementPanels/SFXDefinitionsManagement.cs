#if UNITY_EDITOR

using APX.Extra.GameManagement;
using APX.Extra.OdinExtensions;
using APX.ObjectLocating;
using Sirenix.OdinInspector;
using UnityEngine;

namespace APX.GlobalLocating.ManagementPanels
{
    internal class GlobalLocatorManagement : AManagementPanel
    {
        [HideLabel]
        [ShowInInspector]
        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        public GlobalLocator Locator
        {
            get => GlobalLocator.Instance;
            // ReSharper disable once ValueParameterNotUsed
            set { }
        }

        public override string PanelPath => "Global Locator";
        public override Texture PanelIcon => FontAwesomeEditorIcons.MemoSolid.Highlighted;
        public override float Priority => -1;
    }
}

#endif
