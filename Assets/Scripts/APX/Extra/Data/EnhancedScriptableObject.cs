using APX.Extra.Reboot;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace APX.Extra.Data
{
    public class EnhancedScriptableObject : ScriptableObject
    {
        protected virtual void OnEnable()
        {
#if UNITY_EDITOR
            _isLoaded = true;
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                PlayModeStarted();
            }
#else
            PlayModeStarted();
#endif
        }

        protected virtual void OnDisable()
        {
#if UNITY_EDITOR
            _isLoaded = false;
#else
            PlayModeEnded();
#endif
        }

#if UNITY_EDITOR
        [System.NonSerialized]
        private bool _isLoaded;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void InitializeOnLoad()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state is not (PlayModeStateChange.EnteredPlayMode or PlayModeStateChange.ExitingPlayMode))
                return;

            var guids = AssetDatabase.FindAssets("t:EnhancedScriptableObject");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);

                if (!AssetDatabase.IsMainAssetAtPathLoaded(path))
                {
#if SHOW_ENHANCED_SO_LOGS
                        Debug.Log($"[EnhancedSO] Main asset at path NOT loaded! '{path}', skipping");
#endif
                    continue;
                }

                var result = AssetDatabase.LoadAssetAtPath<EnhancedScriptableObject>(path);
                if (result._isLoaded)
                {
                    switch (state)
                    {
                        case PlayModeStateChange.EnteredPlayMode:
#if SHOW_ENHANCED_SO_LOGS
                            Debug.Log($"[EnhancedSO][{result.GetType().Name}] EnteredPlayMode - {result.name}");
#endif
                            result.PlayModeStarted();
                            break;
                        case PlayModeStateChange.ExitingPlayMode:
#if SHOW_ENHANCED_SO_LOGS
                            Debug.Log($"[EnhancedSO][{result.GetType().Name}] ExitingPlayMode - {result.name}");
#endif
                            result.PlayModeEnded();
                            break;
                    }
                }
            }
        }
#endif

        [ExecuteOnReboot(1000)]
        private static async UniTask OnReboot()
        {
            var targets = Resources.FindObjectsOfTypeAll<EnhancedScriptableObject>();
            foreach (var target in targets)
            {
                target.PlayModeEnded();
            }

            await UniTask.Yield();
            foreach (var target in targets)
            {
                target.PlayModeStarted();
            }
        }

        protected virtual void PlayModeStarted() { }

        protected virtual void PlayModeEnded() { }
    }
}
