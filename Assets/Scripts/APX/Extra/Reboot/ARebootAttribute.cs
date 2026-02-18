using System;

namespace APX.Extra.Reboot
{
    public abstract class ARebootAttribute : Attribute
    {
        public int Order { get; }

        protected ARebootAttribute() { Order = 0; }
        protected ARebootAttribute(int order) { Order = order; }
        protected ARebootAttribute(RebootPhase phase) { Order = (int)phase; }
    }
}
