using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Serializable, Preserve]
    public class CanvasOrderInLayerPreset : AStateValuePreset<Canvas, int>
    {

#if UNITY_EDITOR
        [Preserve]
        public override string FieldName => "Order In Layer";
#endif

        public override void ApplyTo(object targetObject)
        {
            ApplyTo(targetObject as Canvas);
        }

        public override void ApplyTo(Canvas targetObject)
        {
            if (targetObject != null) targetObject.sortingOrder = PresetState;
        }
    }
}
