#if UNITY_EDITOR

using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace APX.Extra.GameManagement
{
    [Serializable]
    public class DynamicManagementPanel : AManagementPanel, IObjectManagementPanel
    {
        [ShowInInspector]
        [EnableGUI]
        [ShowIf(nameof(IsUnityObject))]
        [InlineEditor(Expanded = true, ObjectFieldMode = InlineEditorObjectFieldModes.CompletelyHidden)]
        public Object AssetField => SourceObject as Object;

        [ShowInInspector]
        [EnableGUI]
        [HideIf(nameof(IsUnityObject))]
        [HideLabel]
        [InlineProperty]
        public object ObjectField => SourceObject as Object;

        public bool IsUnityObject => SourceObject is Object;

        public object SourceObject { get; }
        public override string PanelPath { get; }
        public override Texture PanelIcon { get; }
        public override float Priority { get; }

        public DynamicManagementPanel(object obj, string panelPath, Texture panelIcon, float priority)
        {
            SourceObject = obj;
            PanelPath = panelPath;
            PanelIcon = panelIcon;
            Priority = priority;
        }
    }
}

#endif