using UnityEditor;
using UnityEngine;

namespace APX.Extra.Misc
{
    public static class EditorHelper
    {
        public static bool IsEditor => Application.isEditor;
        public static bool IsEditorEditMode => IsEditor && !Application.isPlaying;
        public static bool IsEditorPlayMode => Application.isEditor && Application.isPlaying;
        public static bool IsExitingPlayMode =>
#if UNITY_EDITOR
            EditorApplication.isPlayingOrWillChangePlaymode &&
#endif
            !Application.isPlaying;

#if UNITY_EDITOR
        // Used for access from different threads
        public static bool IsInPlayMode { get; private set; }

        [InitializeOnLoadMethod]
        private static void InitializeEditorHelper()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            IsInPlayMode = EditorApplication.isPlayingOrWillChangePlaymode;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredPlayMode:
                case PlayModeStateChange.ExitingEditMode:
                    IsInPlayMode = true;
                    break;

                case PlayModeStateChange.EnteredEditMode:
                case PlayModeStateChange.ExitingPlayMode:
                    IsInPlayMode = false;
                    break;

                default:
                    throw new System.ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public static Vector2Int GetScreenSize()
        {
            var res = UnityStats.screenRes.Split('x');
            return new Vector2Int(int.Parse(res[0]), int.Parse(res[1]));
        }
#endif
    }
}

