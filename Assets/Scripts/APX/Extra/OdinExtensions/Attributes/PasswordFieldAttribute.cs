using System;
using System.Diagnostics;

namespace APX.Extra.OdinExtensions.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    [Conditional("UNITY_EDITOR")]
    public class PasswordFieldAttribute : Attribute
    {
        
    }
}
