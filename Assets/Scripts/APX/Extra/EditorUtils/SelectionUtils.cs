#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace APX.Extra.EditorUtils
{
    public static class SelectionUtils
    {
        /// <summary>
        /// Retrieves selected folder on Project view.
        /// </summary>
        /// <returns></returns>
        public static string GetSelectedPathOrFallback()
        {
            var path = "Assets";

            foreach (var obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                    break;
                }
            }

            return path;
        }

        /// <summary>
        /// Recursively gather all files under the given path including all its subfolders.
        /// </summary>
        private static IEnumerable<string> GetFiles(string path)
        {
            var queue = new Queue<string>();
            queue.Enqueue(path);
            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                try
                {
                    foreach (var subDir in Directory.GetDirectories(path))
                    {
                        queue.Enqueue(subDir);
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex.Message);
                }

                string[] files = null;
                try
                {
                    files = Directory.GetFiles(path);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex.Message);
                }

                if (files != null)
                {
                    for (var i = 0; i < files.Length; i++)
                    {
                        yield return files[i];
                    }
                }
            }
        }
    }
}

#endif