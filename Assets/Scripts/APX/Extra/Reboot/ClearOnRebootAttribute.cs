using System;
using JetBrains.Annotations;

namespace APX.Extra.Reboot
{
    [MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ClearOnRebootAttribute : ARebootAttribute
    {
        public ClearOnRebootAttribute() { }
        public ClearOnRebootAttribute(int order) : base(order) { }
        public ClearOnRebootAttribute(RebootPhase phase) : base(phase) { }
    }
}
