using UnityEngine;
using UnityEngine.Serialization;

namespace APX.Extra.Misc
{
    public class PlatformObjectDisabler : MonoBehaviour
    {
        [FormerlySerializedAs("DisableInIos")]
        public bool _DisableInIos;
        [FormerlySerializedAs("DisableInAndroid")]
        public bool _DisableInAndroid;
        [FormerlySerializedAs("DisableInWebGL")]
        public bool _DisableInWebGL;
        [FormerlySerializedAs("DisableInRelease")]
        public bool _DisableInRelease;

        private void Start()
        {
            gameObject.SetActive(!IsDisabledOnCurrentPlatform());
        }

        private void OnEnable()
        {
            gameObject.SetActive(!IsDisabledOnCurrentPlatform());
        }

        private bool IsDisabledOnCurrentPlatform()
        {
#if UNITY_IOS
            if (_DisableInIos) return true;
#elif UNITY_ANDROID
            if (_DisableInAndroid) return true;
#elif UNITY_WEBGL
            if (_DisableInWebGL) return true;
#endif
#if !DEBUG
            if (_DisableInRelease) return true;
#endif
            return false;
        }
    }
}
