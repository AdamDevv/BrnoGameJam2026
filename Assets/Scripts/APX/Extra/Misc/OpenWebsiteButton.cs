using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace APX.Extra.Misc
{
    [RequireComponent(typeof(Button))]
    public class OpenWebsiteButton : MonoBehaviour
    {
        [FormerlySerializedAs("AndroidURL")]
        [SerializeField]
        [LabelWidth(100)]
        private string _AndroidURL;

        [FormerlySerializedAs("IosURL")]
        [SerializeField]
        [LabelWidth(100)]
        private string _IosURL;
        
        [SerializeField]
        [LabelWidth(100)]
        private string _StandaloneURL;

        private Button _button;
        private Button Button => _button != null ? _button : _button = GetComponent<Button>();

        private void Awake()
        {
            Button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            Button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            var url = GetURL();
            if (!string.IsNullOrEmpty(url))
            {
                Application.OpenURL(url);
            }
        }

        private string GetURL()
        {
#if UNITY_ANDROID
            return _AndroidURL;
#elif UNITY_IOS
            return _IosURL;
#elif UNITY_STANDALONE
            return _StandaloneURL;
#else
            //not implemented
            throw new System.NotImplementedException();
#endif
        }
    }
}
