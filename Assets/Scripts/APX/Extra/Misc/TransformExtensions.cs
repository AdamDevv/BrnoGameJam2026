using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace APX.Extra.Misc
{
    public static class TransformExtensions
    {
        public static void DestroyChildren(this Transform t, List<Transform> excludedObjects = null)
        {
            foreach (Transform child in t)
            {
                if (excludedObjects == null || !excludedObjects.Contains(child))
                {
                    if (Application.isPlaying)
                        Object.Destroy(child.gameObject);
                    else 
                        Object.DestroyImmediate(child.gameObject);
                }
            }
        }

        public static void ReParentChildren(this Transform t, Transform newTransform, bool worldPositionStays)
        {
            while (t.childCount > 0)
            {
                t.GetChild(0).SetParent(newTransform, worldPositionStays);
            }
        }

        public static IEnumerable<Transform> GetChildren(this Transform t)
        {
            for (var i = 0; i < t.childCount; i++)
            {
                yield return t.GetChild(i);
            }
        }

        public static void DestroyChildrenImmediate(this Transform t, List<Transform> excludedObjects = null)
        {
            var skipped = 0;
            while (t.childCount > skipped)
            {
                var child = t.GetChild(skipped);
                if (excludedObjects != null && excludedObjects.Contains(child))
                {
                    skipped++;
                    continue;
                }
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    UnityEditor.Undo.DestroyObjectImmediate(child.gameObject);
                    continue;
                }
#endif
                Object.DestroyImmediate(child.gameObject);
            }
        }

        public static string GetPath(this GameObject gameObject)
        {
            return gameObject.transform.GetPath();
        }

        public static string GetPath(this Transform transform)
        {
            var pathBuilder = new StringBuilder();
            while(transform != null)
            {
                pathBuilder.Insert(0, transform.name);
                pathBuilder.Insert(0, "/");
                transform = transform.parent;
            }
            return pathBuilder.ToString();
        }
        
        public static string GetPath(this Component component)
        {
            return component.transform.GetPath();
        }
        

        public static string GetFullPath(this Component component) { return component.transform.GetPath() + " C:" + component.GetType().Name.ToString(); }

        public static Vector3 GetCanvasPosition(this Transform transform)
        {
            var parentCanvas = transform.GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                return parentCanvas.transform.InverseTransformPoint(transform.position);
            }

            return Vector3.zero;
        }

        public static bool TryGetCanvasCamera(this RectTransform transform, out Camera camera)
        {
            var parentCanvas = transform?.GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                camera = parentCanvas.worldCamera;
                return camera;
            }

            camera = default;
            return false;
        }
        
        /// <summary>
        /// Reset all world-space values on a transform to identity
        /// </summary>
        /// <param name="tr"></param>
        public static void Reset(this Transform tr)
        {
            tr.position = Vector3.zero;
            tr.rotation = Quaternion.identity;
            tr.localScale = Vector3.one;
        }
        
        /// <summary>
        /// Reset all local values on a transform to identity
        /// </summary>
        /// <param name="tr"></param>
        public static void ResetLocal(this Transform tr)
        {
            tr.localPosition = Vector3.zero;
            tr.localRotation = Quaternion.identity;
            tr.localScale = Vector3.one;
        }
        
        public static void RegisterChangeSignalListener(this Transform t, Action<Transform> listenerMethod)
        {
            var monitor = t.GetOrAddComponent<TransformChangeMonitor>();
            monitor.hideFlags = HideFlags.NotEditable | HideFlags.DontSave;
            monitor.TransformChanged.AddListener(listenerMethod);
        }

        public static void UnregisterChangeSignalListener(this Transform t, Action<Transform> listenerMethod)
        {
            if (t.TryGetComponent<TransformChangeMonitor>(out var monitor))
            {
                monitor.TransformChanged.RemoveListener(listenerMethod);
            }
        }
    }
}
