using Sirenix.OdinInspector;
using UnityEngine;

namespace APX.Extra.Misc
{
    public class SyncObjectPosition : MonoBehaviour
    {
        public Transform TargetTransform;

        private Transform _transform;

        private void Awake() { _transform = transform; }

        private void LateUpdate()
        {
            if (TargetTransform.hasChanged)
            {
                _transform.position = TargetTransform.position;
            }
        }

        [Button]
        private void SyncNow() { transform.position = TargetTransform.position; }
    }
}
