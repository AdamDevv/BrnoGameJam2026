using APX.Managers.Definitions;
using APX.Managers.GameObjects;
using UnityEngine;

namespace APX.Managers
{
    public static class ManagerInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeOnLoad()
        {
            GlobalManagersSettings globalManagersSettings = GlobalManagersSettings.Instance;

            if (globalManagersSettings is null)
            {
                return;
            }

            foreach (ASingleton manager in globalManagersSettings.Managers)
            {
                GameObject.Instantiate(manager);
            }
        }
    }
}
