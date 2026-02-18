using System;
using System.Diagnostics;

namespace APX.Extra.OdinExtensions.Attributes
{
    [Conditional("UNITY_EDITOR")]
    public class RichTextLabelAttribute : Attribute
    { }
}
