using UnityEngine;

namespace APX.Extra.Misc
{
    public static class ColorHelper
    {
        private const float OneOver255 = 1.0f / 255.0f;

        public static Color ConvertStringToColor(string hex)
        {
            //remove the # at the front
            hex = hex.Replace("#", "");

            byte a = 255;

            //convert RGB characters to bytes
            var r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            var g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            var b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            //handle ARGB strings (8 characters long)
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }

            return new Color(r * OneOver255, g * OneOver255, b * OneOver255, a * OneOver255);
        }
    }
}
