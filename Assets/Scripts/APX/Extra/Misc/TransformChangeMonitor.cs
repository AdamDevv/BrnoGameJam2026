using APX.Extra.Signals;
using UnityEngine;

namespace APX.Extra.Misc
{
    public class TransformChangeMonitor : MonoBehaviour
    {
        private Signal<Transform> _transformChanged;
        public Signal<Transform> TransformChanged => _transformChanged ??= new Signal<Transform>();

        private void LateUpdate()
        {
            if (transform.hasChanged) 
            {
                TransformChanged.Dispatch(transform);
                transform.hasChanged = false;
            }
        }
    }
}
