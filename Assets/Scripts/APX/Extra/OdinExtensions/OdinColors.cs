using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using UnityEngine;

namespace APX.Extra.OdinExtensions
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Coz i want it like this, duh!")]
    [SuppressMessage("ReSharper", "CheckNamespace", Justification = "I SAID I WANT IT LIKE THIS! Just kidding, its so odin always finds it in magic strings ;)")]
    public static class OdinColors
    {
        [UsedImplicitly] public static readonly Color Red       = new Color(0.9f,   0.05f, 0.05f);
        [UsedImplicitly] public static readonly Color Orange    = new Color(0.9f,   0.45f, 0.05f);
        [UsedImplicitly] public static readonly Color Yellow    = new Color(1f,     1f,    0f);
        [UsedImplicitly] public static readonly Color Lime      = new Color(0.65f,  0.9f,  0.05f);
        [UsedImplicitly] public static readonly Color Green     = new Color(0.29f,  0.9f,  0.05f);
        [UsedImplicitly] public static readonly Color Cyan      = new Color(0f,     1f,    0.75f);
        [UsedImplicitly] public static readonly Color Aqua      = new Color(0.05f,  0.75f, 0.9f);
        [UsedImplicitly] public static readonly Color Blue      = new Color(0.05f,  0.26f, 0.9f);
        [UsedImplicitly] public static readonly Color Purple    = new Color(0.65f,  0.15f, 1f);
        [UsedImplicitly] public static readonly Color Magenta   = new Color(0.95f,  0.05f, 0.85f);
        [UsedImplicitly] public static readonly Color Pink      = new Color(1f,     0f,    0.5f);

        [UsedImplicitly] public static readonly Color White     = new Color(1,      1,     1);
        [UsedImplicitly] public static readonly Color LightGrey = new Color(0.75f,  0.75f, 0.75f);
        [UsedImplicitly] public static readonly Color Grey      = new Color(0.5f,   0.5f,  0.5f);
        [UsedImplicitly] public static readonly Color DarkGrey  = new Color(0.25f,  0.25f, 0.25f);
        [UsedImplicitly] public static readonly Color Black     = new Color(0,      0,     0);

        [UsedImplicitly] public static readonly Color Error     = new Color(1f,   0.32f, 0.29f);
        [UsedImplicitly] public static readonly Color Warning   = new Color(1f,   0.75f, 0.02f);

        public static Color GetRainbowColor(int index, int count)
        {
            if (count <= 0 || index < 0 || index >= count)
                return Red;

            var hue = (float) index / count;
            return Color.HSVToRGB(hue, 0.95f, 1);
        }
    }
}
