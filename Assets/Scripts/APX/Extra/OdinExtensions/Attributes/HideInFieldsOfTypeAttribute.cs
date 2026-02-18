using System;
using System.Diagnostics;

namespace APX.Extra.OdinExtensions.Attributes
{
    [Conditional("UNITY_EDITOR")]
    public class HideInFieldsOfTypeAttribute : Attribute
    {
        public Type[] FieldTypes;
        public bool IncludeChildTypes;

        public HideInFieldsOfTypeAttribute(params Type[] fieldTypes)
        {
            FieldTypes = fieldTypes;
        }
        
        public HideInFieldsOfTypeAttribute(bool includeChildTypes, params Type[] fieldTypes)
        {
            IncludeChildTypes = includeChildTypes;
            FieldTypes = fieldTypes;
        }

        public HideInFieldsOfTypeAttribute(Type type, bool includeChildTypes = false)
        {
            IncludeChildTypes = includeChildTypes;
            FieldTypes = new [] {type};
        }
    }
}
