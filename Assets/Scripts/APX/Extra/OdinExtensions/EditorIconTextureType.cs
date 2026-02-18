using System;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

namespace APX.Extra.OdinExtensions
{
    public enum EditorIconTextureType
    {
        Raw,
        Inactive,
        Active,
        Highlighted
    }

#if UNITY_EDITOR
    public static class EditorIconTextureTypeExtensions
    {
        public static Texture GetTexture(this EditorIcon icon, EditorIconTextureType type) => type switch
        {
            EditorIconTextureType.Raw => icon.Raw,
            EditorIconTextureType.Inactive => icon.Inactive,
            EditorIconTextureType.Active => icon.Active,
            EditorIconTextureType.Highlighted => icon.Highlighted,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
#endif
}
