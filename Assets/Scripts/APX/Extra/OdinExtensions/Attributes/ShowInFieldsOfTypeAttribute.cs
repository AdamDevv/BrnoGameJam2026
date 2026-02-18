using System;
using System.Diagnostics;

namespace APX.Extra.OdinExtensions.Attributes
{
    [Conditional("UNITY_EDITOR")]
    public class ShowInFieldsOfTypeAttribute : Attribute
    {
        public Type[] FieldTypes;
        public bool IncludeChildTypes;

        public ShowInFieldsOfTypeAttribute(params Type[] fieldTypes)
        {
            FieldTypes = fieldTypes;
        }
        
        public ShowInFieldsOfTypeAttribute(bool includeChildTypes, params Type[] fieldTypes)
        {
            IncludeChildTypes = includeChildTypes;
            FieldTypes = fieldTypes;
        }
    }
}
