using System;
using APX.Extra.OdinExtensions.Attributes;
using UnityEngine;
using UnityEngine.Scripting;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Serializable, Preserve]
    public class CanvasSortingLayerPreset : AStateValuePreset<Canvas>
    {
        [SortingLayer]
        [SerializeField]
        private string _SortingLayer = "Default";

#if UNITY_EDITOR
        [Preserve]
        public override string FieldName => "Sorting Layer";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as Canvas); }

        public override void ApplyTo(Canvas targetObject)
        {
            if (targetObject != null) targetObject.sortingLayerName = _SortingLayer;
        }
    }
}
