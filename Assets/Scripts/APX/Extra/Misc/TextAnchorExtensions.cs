using UnityEngine;

namespace APX.Extra.Misc
{
    public static class TextAnchorExtensions
    {
        public static VerticalAlignment GetVerticalAlignment(this TextAnchor anchor)
        {
            return anchor switch
            {
                TextAnchor.UpperLeft or TextAnchor.UpperCenter or TextAnchor.UpperRight    => VerticalAlignment.Top,
                TextAnchor.MiddleLeft or TextAnchor.MiddleCenter or TextAnchor.MiddleRight => VerticalAlignment.Middle,
                TextAnchor.LowerLeft or TextAnchor.LowerCenter or TextAnchor.LowerRight    => VerticalAlignment.Bottom,
                _ => VerticalAlignment.Middle
            };
        }
        
        public static HorizontalAlignment GetHorizontalAlignment(this TextAnchor anchor)
        {
            return anchor switch
            {
                TextAnchor.UpperLeft or TextAnchor.MiddleLeft or TextAnchor.LowerLeft       => HorizontalAlignment.Left,
                TextAnchor.UpperCenter or TextAnchor.MiddleCenter or TextAnchor.LowerCenter => HorizontalAlignment.Center,
                TextAnchor.UpperRight or TextAnchor.MiddleRight or TextAnchor.LowerRight    => HorizontalAlignment.Right,
                _ => HorizontalAlignment.Center
            };
        }
    }
    
    public enum VerticalAlignment
    {
        Top,
        Middle,
        Bottom
    }
    
    public enum HorizontalAlignment
    {
        Left,
        Center,
        Right
    }
}
