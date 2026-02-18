using System;
using System.Diagnostics;
using UnityEngine;

namespace APX.Extra.OdinExtensions.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    [Conditional("UNITY_EDITOR")]
    public class GUIBackgroundColorAttribute : Attribute
    {
        public Color Color;
        public string GetColor;
        
        public GUIBackgroundColorAttribute(float r, float g, float b, float a = 1f) => Color = new Color(r, g, b, a);
        public GUIBackgroundColorAttribute(string getColor) => GetColor = getColor;
    }
}
