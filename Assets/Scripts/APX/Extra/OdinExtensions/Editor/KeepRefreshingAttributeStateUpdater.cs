using System;
using APX.Extra.OdinExtensions.Attributes;
using APX.Extra.OdinExtensions.Editor;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

[assembly: RegisterStateUpdater(typeof(KeepRefreshingAttributeStateUpdater))]
namespace APX.Extra.OdinExtensions.Editor
{
    public sealed class KeepRefreshingAttributeStateUpdater: AttributeStateUpdater<KeepRefreshingAttribute>, IDisposable
    {
        protected override void Initialize()
        {
            base.Initialize();
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }
        
        public void Dispose()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        }
        
        private void OnPlayModeChanged(PlayModeStateChange obj)
        {
            OnStateUpdate();
        }

        public override void OnStateUpdate()
        {
            var update = (EditorApplication.isPlaying && Attribute.UpdateInPlayMode) || (!EditorApplication.isPlaying && Attribute.UpdateInEditorMode);
            if (Property.State.Visible && update)
            {
                KeepRefreshingUtility.AddProperty(Property);
            }
            else
            {
                KeepRefreshingUtility.RemoveProperty(Property);
            }
        }
    }
}
    
