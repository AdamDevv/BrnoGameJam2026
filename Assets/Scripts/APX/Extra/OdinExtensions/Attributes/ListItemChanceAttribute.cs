using System;
using System.Diagnostics;

namespace APX.Extra.OdinExtensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    [Conditional("UNITY_EDITOR")]
    public class ListItemChanceAttribute : Attribute
    {

    }
}
