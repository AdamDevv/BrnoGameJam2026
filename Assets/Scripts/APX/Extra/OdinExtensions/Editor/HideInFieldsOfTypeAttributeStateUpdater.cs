using APX.Extra.OdinExtensions.Attributes;
using APX.Extra.OdinExtensions.Editor;
using Sirenix.OdinInspector.Editor;

[assembly: RegisterStateUpdater(typeof(HideInFieldsOfTypeAttributeStateUpdater))]
namespace APX.Extra.OdinExtensions.Editor
{
    public class HideInFieldsOfTypeAttributeStateUpdater : AttributeStateUpdater<HideInFieldsOfTypeAttribute>
    {
        public override void OnStateUpdate()
        {
            var fieldType = Property.Parent?.Info?.TypeOfValue;
            Property.State.Visible = fieldType != null && !OdinExtensionUtils.IsAssignable(fieldType, Attribute.FieldTypes, Attribute.IncludeChildTypes);
        }
    }
}
