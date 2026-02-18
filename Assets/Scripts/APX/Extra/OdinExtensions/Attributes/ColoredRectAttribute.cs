using System;
using System.Diagnostics;
using Sirenix.OdinInspector;
using UnityEngine;

namespace APX.Extra.OdinExtensions.Attributes
{
    [DontApplyToListElements]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    [Conditional("UNITY_EDITOR")]
    public class ColoredRectAttribute : Attribute
    {
#region Public Fields
        public Color Color;
        public string GetColor;

        public DisplayMode Mode = DisplayMode.Top;
        public float Width = 0;
        public float Height = 0;
        
        public float SpaceBefore = 0;
        public float SpaceAfter = 0;
#endregion


#region Constructor
        public ColoredRectAttribute(float r, float g, float b, float a) { Color = new Color(r, g, b, a); }
        public ColoredRectAttribute(Color color) { Color = color; }
        public ColoredRectAttribute(string getColor) { GetColor = getColor; }
#endregion


        public enum DisplayMode
        {
            Left,
            Right,
            Top,
            Bottom
        }
    }
}
