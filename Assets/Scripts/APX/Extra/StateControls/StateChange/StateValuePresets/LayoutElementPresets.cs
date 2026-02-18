using UnityEngine.Scripting;
using UnityEngine.UI;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    public class LayoutElementMinHeightPreset : AStateValuePreset<LayoutElement, float>
    {
#if UNITY_EDITOR
        public override string FieldName => "Min Height";
        public override string InfoText => "Setting value to -1 will deactivate Min height";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as LayoutElement); }

        public override void ApplyTo(LayoutElement targetObject)
        {
            if (targetObject != null) targetObject.minHeight = PresetState;
        }
    }

    [Preserve]
    public class LayoutElementMinWidthPreset : AStateValuePreset<LayoutElement, float>
    {
#if UNITY_EDITOR
        public override string FieldName => "Min Width";
        public override string InfoText => "Setting value to -1 will deactivate Min width";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as LayoutElement); }

        public override void ApplyTo(LayoutElement targetObject)
        {
            if (targetObject != null) targetObject.minWidth = PresetState;
        }
    }

    [Preserve]
    public class LayoutElementPreferredHeightPreset : AStateValuePreset<LayoutElement, float>
    {
#if UNITY_EDITOR
        public override string FieldName => "Preferred Height";
        public override string InfoText => "Setting value to -1 will deactivate Preferred height";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as LayoutElement); }

        public override void ApplyTo(LayoutElement targetObject)
        {
            if (targetObject != null) targetObject.preferredHeight = PresetState;
        }
    }

    [Preserve]
    public class LayoutElementPreferredWidthPreset : AStateValuePreset<LayoutElement, float>
    {
#if UNITY_EDITOR
        public override string FieldName => "Preferred  Width";
        public override string InfoText => "Setting value to -1 will deactivate Preferred width";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as LayoutElement); }

        public override void ApplyTo(LayoutElement targetObject)
        {
            if (targetObject != null) targetObject.preferredWidth = PresetState;
        }
    }

    [Preserve]
    public class LayoutElementFlexibleHeightPreset : AStateValuePreset<LayoutElement, float>
    {
#if UNITY_EDITOR
        public override string FieldName => "Flexible Height";
        public override string InfoText => "Setting value to -1 will deactivate Flexible height";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as LayoutElement); }

        public override void ApplyTo(LayoutElement targetObject)
        {
            if (targetObject != null) targetObject.flexibleHeight = PresetState;
        }
    }

    [Preserve]
    public class LayoutElementFlexibleWidthPreset : AStateValuePreset<LayoutElement, float>
    {
#if UNITY_EDITOR
        public override string FieldName => "Flexible Width";
        public override string InfoText => "Setting value to -1 will deactivate Flexible width";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as LayoutElement); }

        public override void ApplyTo(LayoutElement targetObject)
        {
            if (targetObject != null) targetObject.flexibleWidth = PresetState;
        }
    }

    [Preserve]
    public class LayoutElementIgnorePreset : AStateValuePreset<LayoutElement, bool>
    {
#if UNITY_EDITOR
        public override string FieldName => "Ignore Layout";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as LayoutElement); }

        public override void ApplyTo(LayoutElement targetObject)
        {
            if (targetObject != null) targetObject.ignoreLayout = PresetState;
        }
    }

    [Preserve]
    public class LayoutElementPriorityPreset : AStateValuePreset<LayoutElement, int>
    {
#if UNITY_EDITOR
        public override string FieldName => "Layout Priority";
#endif

        public override void ApplyTo(object targetObject) { ApplyTo(targetObject as LayoutElement); }

        public override void ApplyTo(LayoutElement targetObject)
        {
            if (targetObject != null) targetObject.layoutPriority = PresetState;
        }
    }
}
