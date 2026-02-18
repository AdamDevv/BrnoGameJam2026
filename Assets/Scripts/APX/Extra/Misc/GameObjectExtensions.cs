using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace APX.Extra.Misc
{
    public static class GameObjectExtensions
    {
        public static void SetLayer(this GameObject parent, int layer, bool includeChildren = false)
        {
            parent.layer = layer;
            if (!includeChildren) return;
            foreach (var trans in parent.transform.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = layer;
            }
        }

        /// <summary>
        /// Run an action on GameObject and its children
        /// Example: someObject.RunOnChildrenRecursive(child => child.layer = LayerMask.NameToLayer("UI"));
        /// </summary>
        /// <param name="go">Root GameObject</param>
        /// <param name="action">Action with GameObject as parameter</param>
        /// <param name="includeInactive">Should Components on inactive GameObjects be included in the found set? includeInactive decides which children of the GameObject will be searched. The GameObject that you call GetComponentsInChildren on is always searched regardless.</param>
        public static void RunOnChildrenRecursive(this GameObject go, Action<GameObject> action, bool includeInactive = true)
        {
            if (go == null) return;
            foreach (var trans in go.GetComponentsInChildren<Transform>(includeInactive))
            {
                action(trans.gameObject);
            }
        }

        private static readonly Char[] Splitters = {'/', '\\'};

        public static Transform FindIncludingInactiveInAnyOpenedScene(string path)
        {
            Transform matchedTransform = null;
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);

                matchedTransform = FindIncludingInactiveInScene(path, scene);
                if (matchedTransform != null)
                {
                    break;
                }
            }

            return matchedTransform;
        }

#if UNITY_EDITOR
        public static Transform FindIncludingInactiveInSceneOrPrefabSceneEditorOnly(string componentPath, string path)
        {
            if (path == null || componentPath == null) return null;
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null && prefabStage.assetPath == path)
            {
                return FindIncludingInactiveInScene(componentPath, prefabStage.scene);
            }

            return FindIncludingInactiveInScene(componentPath, SceneManager.GetSceneByPath(path));
        }
#endif

        public static Transform FindIncludingInactiveInScene(string path, string scenePath)
        {
            if (scenePath == null) return null;
            return FindIncludingInactiveInScene(path, SceneManager.GetSceneByPath(scenePath));
        }

        public static Transform FindIncludingInactiveInScene(string path, Scene scene)
        {
            if (string.IsNullOrEmpty(path)) return null;
            if (!scene.isLoaded || !scene.IsValid()) return null;

            Transform matchedTransform = null;

            if (path.StartsWith("/") || path.StartsWith("\\"))
            {
                path = path.Substring(1);
            }

            string[] pathStrings = path.Split(Splitters);
            GameObject[] rootGameObjects = scene.GetRootGameObjects();
            for (int i = 0; i < rootGameObjects.Length; i++)
            {
                GameObject rootGameObject = rootGameObjects[i];
                matchedTransform = FindAnywhereWithPath(rootGameObject.transform, pathStrings);
                if (matchedTransform != null)
                {
                    break;
                }
            }

            return matchedTransform;
        }

        private static Transform FindAnywhereWithPath(Transform current, string[] path, int currentDepth = 0)
        {
            var targetName = path[currentDepth];

            var matchedTransform = current.name == targetName ? current : null;
            if (matchedTransform != null && path.Length - 1 > currentDepth)
            {
                var children = matchedTransform.GetComponentInChildren<Transform>(true);
                foreach (Transform child in children)
                {
                    matchedTransform = FindAnywhereWithPath(child, path, currentDepth + 1);
                    if (matchedTransform != null)
                    {
                        break;
                    }
                }
            }

            return matchedTransform;
        }


        public static T GetComponentInParent<T>(this GameObject go, bool includeInactive) where T : class
        {
            using (var lst = TempCollection.GetList<T>())
            {
                go.GetComponentsInParent<T>(includeInactive, lst);
                return lst.Count > 0 ? lst[0] : null;
            }
        }

        public static T GetComponentInParent<T>(this Component component, bool includeInactive) where T : class
        {
            using (var lst = TempCollection.GetList<T>())
            {
                component.GetComponentsInParent<T>(includeInactive, lst);
                return lst.Count > 0 ? lst[0] : null;
            }
        }

        public static T GetComponentInParentExclusive<T>(this Component component, bool includeInactive) where T : class
        {
            Transform parent = component.transform.parent;
            if (parent != null)
            {
                using (var lst = TempCollection.GetList<T>())
                {
                    parent.GetComponentsInParent<T>(includeInactive, lst);
                    return lst.Count > 0 ? lst[0] : null;
                }
            }

            return null;
        }


        public static T GetComponentInParents<T>(this Component self) where T : class { return self.transform.GetComponentInParents<T>(); }

        public static T GetComponentInParents<T>(this GameObject self) where T : class { return self.transform.GetComponentInParents<T>(); }

        public static T GetComponentInParents<T>(this Transform self) where T : class
        {
            T component = self.GetComponent<T>();
            if (component != null)
            {
                return component;
            }

            if (self.parent == null)
            {
                return null;
            }

            return self.parent.GetComponentInParents<T>();
        }

        public static T GetComponentInParentsExclusive<T>(this Component component) where T : class
        {
            Transform parent = component.transform.parent;
            if (parent != null)
                return parent.GetComponentInParents<T>();

            return null;
        }

        public static List<T> GetTopComponentsInChildren<T>(this Component self, List<T> destList = null) where T : class { return self.transform.GetTopComponentsInChildren<T>(destList); }

        public static List<T> GetTopComponentsInChildren<T>(this GameObject self, List<T> destList = null) where T : class { return self.transform.GetTopComponentsInChildren<T>(destList); }

        public static List<T> GetTopComponentsInChildren<T>(this Transform self, List<T> destList = null) where T : class
        {
            if (destList == null) destList = new List<T>();

            self.GetTopComponentsInChildrenInternal(destList);

            return destList;
        }

        private static void GetTopComponentsInChildrenInternal<T>(this Transform self, List<T> destList) where T : class
        {
            T component = self.GetComponent<T>();
            if (component != null)
            {
                destList.Add(component);
                return;
            }

            for (int i = 0; i < self.childCount; i++)
            {
                self.GetChild(i).GetTopComponentsInChildrenInternal(destList);
            }
        }

        public static List<T> GetComponentsUpToFirstChildren<T>(this GameObject go)
        {
            var components = new List<T>();
            components.AddRange(go.GetComponents<T>());

            var transform = go.transform;
            var childCount = transform.childCount;
            for (var i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                components.AddRange(child.GetComponents<T>());
            }

            return components;
        }

        /// <summary>
        ///     Gets or add a component. Usage example:
        ///     BoxCollider boxCollider = gameObject.GetOrAddComponent<BoxCollider>();
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject gameObj, out bool newlyAdded) where T : Component
        {
            var component = gameObj.GetComponent<T>();
            newlyAdded = false;
            if (component == null)
            {
                component = gameObj.AddComponent<T>();
                newlyAdded = true;
            }

            return component;
        }

        public static Component GetOrAddComponent(this GameObject gameObj, Type type)
        {
            var component = gameObj.GetComponent(type);
            if (component == null)
            {
                component = gameObj.AddComponent(type);
            }

            return component;
        }

        public static T GetOrAddComponent<T>(this GameObject gameObj) where T : Component
        {
            var component = gameObj.GetComponent<T>();
            if (component == null)
            {
                component = gameObj.AddComponent<T>();
            }

            return component;
        }

        public static BaseT GetOrAddComponent<BaseT, T>(this GameObject gameObj)
            where BaseT : class
            where T : Component, BaseT
        {
            var component = gameObj.GetComponent<BaseT>();
            if (component == null)
            {
                component = gameObj.AddComponent<T>();
            }

            return component;
        }

        public static void SetActive(this GameObject[] gameObjects, bool active)
        {
            if (gameObjects == null)
                return;

            foreach (var gameObject in gameObjects)
            {
                gameObject.SetActive(active);
            }
        }

        public static void SetActive(this List<GameObject> gameObjects, bool active)
        {
            if (gameObjects == null)
                return;

            foreach (var gameObject in gameObjects)
            {
                gameObject.SetActive(active);
            }
        }

        public static bool TryGetComponentInParent<T>(this GameObject go, out T result, bool includeInactive = false)
        {
            result = go.GetComponentInParent<T>(includeInactive);
            return result != null;
        }

        public static bool TryGetComponentInParent<T>(this Component self, out T component, bool includeInactive = false)
        {
            component = self.GetComponentInParent<T>(includeInactive);
            return component != null;
        }
        
        public static bool TryGetComponentInChildren<T>(this GameObject go, out T result, bool includeInactive = false)
        {
            result = go.GetComponentInChildren<T>(includeInactive);
            return result != null;
        }

        public static bool TryGetComponentInChildren<T>(this Component self, out T component, bool includeInactive = false)
        {
            component = self.GetComponentInChildren<T>(includeInactive);
            return component != null;
        }

        public static void ApplyHideFlagsRecursively(this GameObject go, HideFlags hideFlags)
        {
            go.hideFlags = hideFlags;
            var t = go.transform;
            for (int i = 0; i < t.childCount; ++i)
            {
                t.GetChild(i).gameObject.ApplyHideFlagsRecursively(hideFlags);
            }
        }
    }
}
