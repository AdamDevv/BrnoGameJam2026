using System;
using Sirenix.OdinInspector;

namespace APX.Util.OdinAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    [IncludeMyAttributes]
    [ShowInInspector]
    [TitleGroup("Debug exposed", "These variables are not serialized and are exposed only for debugging purposes",
        Order = 999)]
    public class DebugExposeAttribute : Attribute { }
}
