using System;

namespace APX.Extra.Misc
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ToggleableFieldSettingsAttribute : Attribute
    {
        public ToggleableFieldMode Mode { get; }

        public ToggleableFieldSettingsAttribute(ToggleableFieldMode mode)
        {
            Mode = mode;
        }
    }
    
    public enum ToggleableFieldMode
    {
        AlwaysActive,
        EnabledIfChecked,
        DisabledIfChecked
    }
}
