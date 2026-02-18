#if UNITY_EDITOR
using System;
using System.Linq;
using APX.Extra.StateControls.StateChange;
using Sirenix.OdinInspector.Editor.Validation;

[assembly: RegisterValidator(typeof(MultiStateBehaviourWithEnumAttributeValidator<>))]
namespace APX.Extra.StateControls.StateChange
{
    public class MultiStateBehaviourWithEnumAttributeValidator<T> : AttributeValidator<ValidateMultiStateBehaviourWithEnumAttribute, T> where T : AMultiStateBehaviour
    {
        protected override void Validate(ValidationResult result)
        {
            if (Value == null) return;

            var allEnumValues = Enum.GetNames(Attribute.EnumType);
            var requiredEnumValues = allEnumValues.Except(Attribute.OptionalValues.Select(e => e.ToString()));

            var allStates = Value.GetAllStates();
            var missing = requiredEnumValues.Except(allStates);
            var extra = allStates.Except(allEnumValues);

            if (missing.Any() || extra.Any())
            {
                result.AddError($"Mismatch between states and enum values {(missing.Any() ? $", missing: [{string.Join(",", missing)}]" : "")}{(extra.Any() ? $", extra: [{string.Join(",", extra)}]" : "")}")
                    .WithFix(() =>
                    {
                        foreach (var state in missing)
                        {
                            Value.AddState(state);
                        }
                        foreach (var state in extra)
                        {
                            Value.RemoveState(state);
                        }
                        Property.MarkSerializationRootDirty();
                    });
            }
        }
    }
}
#endif