using System;
using System.Diagnostics;
using System.Linq;

namespace APX.Extra.StateControls.StateChange
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class ValidateMultiStateBehaviourWithEnumAttribute : Attribute
    {
        public Type EnumType;
        public Enum[] OptionalValues;

        public ValidateMultiStateBehaviourWithEnumAttribute(Type enumType, params object[] optionalValues)
        {
            EnumType = enumType;
            OptionalValues = optionalValues.OfType<Enum>().ToArray();
        }
    }

}
