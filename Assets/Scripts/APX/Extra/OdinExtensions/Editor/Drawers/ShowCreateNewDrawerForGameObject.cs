using System;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace APX.Extra.OdinExtensions.Editor.Drawers
{
    [DrawerPriority(DrawerPriorityLevel.WrapperPriority)]
    public class ShowCreateNewDrawerForGameObject : ShowCreateNewDrawerBase<GameObject>
    {
        protected override bool TryCreateInstance(Type type, string defaultPath, string defaultName, out GameObject result)
        {
            var path = EditorUtility.SaveFilePanel($"Create new {type.Name}", defaultPath, defaultName, "prefab");
            if (!string.IsNullOrEmpty(path))
            {
                path = $"Assets{path.Replace(Application.dataPath, "")}";
                var instance = new GameObject("GameObject");
                if (type != null) instance.AddComponent(type);
                result = PrefabUtility.SaveAsPrefabAsset(instance, path);
                Object.DestroyImmediate(instance);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                return true;
            }

            result = default;
            return false;
        }
    }
}
