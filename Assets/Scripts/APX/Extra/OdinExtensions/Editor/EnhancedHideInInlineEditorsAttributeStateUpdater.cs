using APX.Extra.OdinExtensions.Editor;
using APX.Extra.OdinExtensions.Editor.Drawers;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

[assembly: RegisterStateUpdater(typeof(HideInInlineEditorsAttributeStateUpdater))]
namespace APX.Extra.OdinExtensions.Editor
{
    public sealed class HideInInlineEditorsAttributeStateUpdater : AttributeStateUpdater<HideInInlineEditorsAttribute>
    {
        public override void OnStateUpdate()
        {
            Property.State.Visible = EnhancedInlineEditorAttributeDrawer.CurrentInlineEditorDrawDepth <= 0;
        }
    }
}