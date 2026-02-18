using System;
using System.Diagnostics;
using Sirenix.OdinInspector;

namespace APX.Extra.OdinExtensions.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    [Conditional("UNITY_EDITOR")]
    [DontApplyToListElements]
    public class SimpleListAttribute : Attribute
    {

    }
}
