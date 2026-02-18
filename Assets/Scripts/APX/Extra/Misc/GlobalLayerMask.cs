using Sirenix.OdinInspector;
using UnityEngine;

namespace APX.Extra.Misc
{
    public class GlobalLayerMask : ScriptableObject
    {
        [SerializeField]
        [HideLabel]
        private LayerMask _LayerMask;
        
        public LayerMask Mask => _LayerMask;
        
        public static implicit operator LayerMask (GlobalLayerMask globalMask) => globalMask.Mask;
        public static implicit operator int (GlobalLayerMask globalMask) => globalMask.Mask;
    }
}
