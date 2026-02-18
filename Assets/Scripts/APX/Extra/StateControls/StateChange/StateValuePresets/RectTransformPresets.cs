using System;
using APX.Extra.DataStructures.RectTransformSnapshots;
using APX.Extra.Misc;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class RectTransformAnchoredPositionPreset : AStateValuePreset<RectTransform, Vector2>
    {
#if UNITY_EDITOR
        public override string FieldName => "AnchoredPosition";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as RectTransform); }

        public override void ApplyTo(RectTransform targetObject)
        {
            if (targetObject != null)
            {
                targetObject.anchoredPosition = PresetState;
            }
        }
    }
    
    [Preserve]
    public class RectTransformSizeDeltaPreset : AStateValuePreset<RectTransform, Vector2>
    {
#if UNITY_EDITOR
        public override string FieldName => "SizeDelta";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as RectTransform); }

        public override void ApplyTo(RectTransform targetObject)
        {
            if (targetObject != null)
            {
                targetObject.sizeDelta = PresetState;
            }
        }
    }

    [Preserve]
    public class RectTransformSizeDeltaWidthPreset : AStateValuePreset<RectTransform, float>
    {
#if UNITY_EDITOR
        public override string FieldName => "SizeDeltaWidth";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as RectTransform); }

        public override void ApplyTo(RectTransform targetObject)
        {
            if (targetObject != null)
            {
                targetObject.sizeDelta = targetObject.sizeDelta.WithX(PresetState);
            }
        }
    }

    [Preserve]
    public class RectTransformSizeDeltaHeightPreset : AStateValuePreset<RectTransform, float>
    {
#if UNITY_EDITOR
        public override string FieldName => "SizeDeltaHeight";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as RectTransform); }

        public override void ApplyTo(RectTransform targetObject)
        {
            if (targetObject != null)
            {
                targetObject.sizeDelta = targetObject.sizeDelta.WithY(PresetState);
            }
        }
    }
    
    [Preserve]
    public class RectTransformPivotPreset : AStateValuePreset<RectTransform, Vector2>
    {
#if UNITY_EDITOR
        public override string FieldName => "Pivot";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as RectTransform); }

        public override void ApplyTo(RectTransform targetObject)
        {
            if (targetObject != null)
            {
                targetObject.pivot = PresetState;
            }
        }
    }
    
    [Preserve]
    public class RectTransformAnchorsPreset : AStateValuePreset<RectTransform, RectTransformAnchorsPreset.Anchors>
    {
#if UNITY_EDITOR
        public override string FieldName => "Anchors";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as RectTransform); }

        public override void ApplyTo(RectTransform targetObject)
        {
            if (targetObject != null)
            {
                targetObject.anchorMin = PresetState._Min;
                targetObject.anchorMax = PresetState._Max;
            }
        }

        [Serializable]
        public struct Anchors
        {
            [InlineProperty]
            public Vector2 _Min;
            [InlineProperty]
            public Vector2 _Max;
        }
    }
    
    [Preserve]
    public class RectTransformOffsetsPreset : AStateValuePreset<RectTransform, RectTransformOffsetsPreset.Offsets>
    {
#if UNITY_EDITOR
        public override string FieldName => "Offsets";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as RectTransform); }

        public override void ApplyTo(RectTransform targetObject)
        {
            if (targetObject != null)
            {
                targetObject.offsetMin = new Vector2(PresetState._Left, targetObject.offsetMin.y);
                targetObject.offsetMax  = new Vector2(-PresetState._Right, targetObject.offsetMax.y);
                targetObject.offsetMax = new Vector2(targetObject.offsetMin.x, -PresetState._Top);
                targetObject.offsetMin  = new Vector2(targetObject.offsetMin.x, PresetState._Bottom);
            }
        }

        [Serializable]
        public struct Offsets
        {
            [InlineProperty]
            public float _Left;
            [InlineProperty]
            public float _Right;
            [InlineProperty]
            public float _Top;
            [InlineProperty]
            public float _Bottom;
        }
    }
    
    [Preserve]
    public class RectTransformMatchPreset : AStateValuePreset<RectTransform, RectTransform>
    {
#if UNITY_EDITOR
        public override string FieldName => "Match RectTransform";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as RectTransform); }

        public override void ApplyTo(RectTransform targetObject)
        {
            if (targetObject == null) return;
            
            targetObject.anchoredPosition = PresetState.anchoredPosition;
            targetObject.sizeDelta = PresetState.sizeDelta;
            targetObject.pivot = PresetState.pivot;
            targetObject.anchorMin = PresetState.anchorMin;
            targetObject.anchorMax = PresetState.anchorMax;
            targetObject.offsetMin = PresetState.offsetMin;
            targetObject.offsetMax = PresetState.offsetMax;
        }
    }
    
    [Preserve]
    public class RectTransformSnapshotPreset : AStateValuePreset<RectTransform, RectTransformSnapshot>
    {
#if UNITY_EDITOR
        public override string FieldName => "Layout";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as RectTransform); }

        public override void ApplyTo(RectTransform targetObject)
        {
            if (targetObject == null) return;
            PresetState.ApplyTo(targetObject);
        }
    }
}
