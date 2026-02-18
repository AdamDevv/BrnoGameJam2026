using System;
using System.Diagnostics;
using UnityEngine;

namespace APX.Extra.OdinExtensions.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    [Conditional("UNITY_EDITOR")]
    public class LayerAttribute : PropertyAttribute
    {
    }
}
