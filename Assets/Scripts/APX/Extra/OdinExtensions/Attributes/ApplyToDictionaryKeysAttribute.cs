using System;
using System.Diagnostics;

namespace APX.Extra.OdinExtensions.Attributes
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Class)]
    public class ApplyToDictionaryKeysAttribute : Attribute { }
}