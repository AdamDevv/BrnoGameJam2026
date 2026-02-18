#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace APX.Extra.Misc
{
    public static class AssetUtils
    {
        [MenuItem("Assets/Set Selection Dirty", false, 50000)]
        [MenuItem("GameObject/Set Selection Dirty", false, 50000)]
        private static void SetSelectionDirty(MenuCommand menuCommand)
        {
            foreach (var file in Selection.objects)
            {
                if (file == null) continue;
                EditorUtility.SetDirty(file);
                if (file is GameObject go)
                {
                    var components = go.GetComponentsInChildren<Component>();
                    foreach (var component in components)
                    {
                        if (component == null) continue;
                        EditorUtility.SetDirty(component);
                    }

                    PrefabUtility.RecordPrefabInstancePropertyModifications(go);
                }
            }

            AssetDatabase.SaveAssets();
        }

        [MenuItem("Assets/Force Reserialize Assets", false, 50000)]
        [MenuItem("GameObject/Force Reserialize Assets", false, 50000)]
        private static void ForceReserializeAssets(MenuCommand menuCommand)
        {
            foreach (var file in Selection.objects)
            {
                var pathToAsset = AssetDatabase.GetAssetPath(file);
                var assetPaths = new List<string>();
                if (!string.IsNullOrEmpty(pathToAsset))
                {
                    assetPaths.Add(pathToAsset);
                }
                else
                {
                    Debug.LogWarning($"AssetDatabase.GetAssetPath didn't find asset {file.name} {file}");
                }

                AssetDatabase.ForceReserializeAssets(assetPaths);
            }
        }

        [MenuItem("Tools/Reserialize Whole Project")]
        private static void ForceReserializeAssets() { AssetDatabase.ForceReserializeAssets(); }

        [MenuItem("Tools/Set Whole Project Dirty")]
        private static void SetProjectDirty()
        {
            if (!EditorUtility.DisplayDialog(
                    "Set project dirty?",
                    "This action will set dirty all files in the project and may take a while!\nAre you sure you want to continue?",
                    "Continue", "Cancel")) return;
            try
            {
                var filters = new[] {"*.prefab", "*.asset"};
                var filePaths = filters.SelectMany(filter => Directory.GetFiles(Application.dataPath, filter, SearchOption.AllDirectories)).ToArray();

                for (var i = 0; i < filePaths.Length; i++)
                {
                    var path = filePaths[i].Replace(Application.dataPath, "Assets");
                    if (EditorUtility.DisplayCancelableProgressBar("Setting project dirty - file " + i + "/" + filePaths.Length, path, i / (float) filePaths.Length))
                        break;

                    var file = AssetDatabase.LoadMainAssetAtPath(path);
                    if (file == null) continue;
                    EditorUtility.SetDirty(file);
                    if (file is GameObject go)
                    {
                        var components = go.GetComponentsInChildren<Component>();
                        foreach (var component in components)
                        {
                            if (component == null)
                            {
                                Debug.LogError($"invalid component on {go.name}", go);
                                continue;
                            }

                            EditorUtility.SetDirty(component);
                        }

                        PrefabUtility.RecordPrefabInstancePropertyModifications(file);
                    }

                    file = null;
                }

                EditorUtility.UnloadUnusedAssetsImmediate(true);
                AssetDatabase.SaveAssets();
                GC.Collect();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            EditorUtility.ClearProgressBar();
        }
    }
}
#endif
