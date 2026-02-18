using System;
using JetBrains.Annotations;

namespace APX.Extra.Reboot
{
    [MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Method)]
    public class ExecuteOnRebootAttribute : ARebootAttribute
    {
        public ExecuteOnRebootAttribute() { }
        public ExecuteOnRebootAttribute(int order) : base(order) { }
        public ExecuteOnRebootAttribute(RebootPhase phase) : base(phase) { }
    }
}
