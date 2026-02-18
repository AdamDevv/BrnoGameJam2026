using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace APX.Extra.DataStructures.SpriteSheets
{
    public class SpriteSheetDefinition : ScriptableObject
    {
        [SerializeField]
        private Sprite[] _Sprites;
        public Sprite[] Sprites => _Sprites;

#if UNITY_EDITOR
        [Button]
        private void ImportFromSpriteSheet(Texture2D texture2D)
        {
            if (texture2D == null) return;

            var texturePath = AssetDatabase.GetAssetPath(texture2D);
            var textureImporter = (TextureImporter) AssetImporter.GetAtPath(texturePath);
            if (textureImporter && textureImporter.spriteImportMode != SpriteImportMode.Multiple)
            {
                Debug.LogWarning($"Texture {texture2D.name} is not set to Multiple sprite mode, cannot import sprites from sprite sheet.");
                return;
            }

            var objects = AssetDatabase.LoadAllAssetsAtPath(texturePath);
            var spriteList = new List<Sprite>(objects.Length);
            foreach (var obj in objects)
            {
                if (obj is Sprite sprite)
                    spriteList.Add(sprite);
            }

            _Sprites = spriteList.ToArray();
        }
#endif
    }
}
