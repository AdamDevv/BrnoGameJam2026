using UnityEngine;

namespace APX.Extra.Misc
{
    public static class MathExtensions
    {
        public static bool Approximately(this float a, float b) => Mathf.Approximately(a, b);
        public static bool InRange(this float v, float min, float max) => v >= min && v <= max;
        public static bool InRange(this int v, int min, int max) => v >= min && v <= max;
    }
}