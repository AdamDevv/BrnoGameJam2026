using UnityEngine;
using UnityEngine.SceneManagement;

namespace APX.Extra.Misc
{
    public static class SceneExtensionMethods
    {
        public static bool TryGetComponentOnRootGO<T>(this Scene scene, out T component) where T : Component
        {
            var rootGameObjects = scene.GetRootGameObjects();
            for (var i = 0; i < rootGameObjects.Length; i++)
            {
                if (rootGameObjects[i].TryGetComponent(out component))
                    return true;
            }

            component = null;
            return false;
        }
    }
}
