#if UNITY_EDITOR

using System;
using Sirenix.OdinInspector;

namespace APX.Extra.GameManagement
{
    [Serializable]
    public abstract class ASimpleAssetManagementPanel : AManagementPanel, IObjectManagementPanel
    {
        [ShowInInspector]
        [EnableGUI]
        [InlineEditor(Expanded = true, ObjectFieldMode = InlineEditorObjectFieldModes.CompletelyHidden)]
        public abstract UnityEngine.Object SourceAsset { get; }

        public object SourceObject => SourceAsset;
    }
}

#endif