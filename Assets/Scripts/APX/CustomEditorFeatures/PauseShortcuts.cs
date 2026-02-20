#if UNITY_EDITOR
using UnityEditor;

namespace APX.CustomEditorFeatures
{
    internal static class PauseShortcuts
    {
        [MenuItem("APX/Util/Pause/Toggle Pause _F3", priority = 1)]
        private static void TogglePause()
        {
            EditorApplication.isPaused = !EditorApplication.isPaused;
        }

        [MenuItem("APX/Util/Pause/Step _F4", priority = 2)]
        private static void PerformStep()
        {
            EditorApplication.Step();
        }
    }
}
#endif