using System;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace APX.Extra.OdinExtensions.Editor.Drawers.Addressables
{
    [DrawerPriority(0, 2, 0)]
    public class ShowCreateNewDrawerForAssetReferenceGameObject: ShowCreateNewDrawerBase<AssetReferenceGameObject>
    {
        protected override bool TryCreateInstance(Type type, string defaultPath, string defaultName, out AssetReferenceGameObject result)
        {
            var path = EditorUtility.SaveFilePanel($"Create new {type.Name}", defaultPath, defaultName, "prefab");
            if (!string.IsNullOrEmpty(path))
            {
                path = $"Assets{path.Replace(Application.dataPath, "")}";
                var instance = new GameObject("GameObject");
                instance.AddComponent(type);
                var prefab = PrefabUtility.SaveAsPrefabAsset(instance, path);
                Object.DestroyImmediate(instance);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(prefab.gameObject, out var guid, out long _))
                {
                    result = new AssetReferenceGameObject(guid);
                    return true;
                }
            }

            result = default;
            return false;
        }
    }
}
