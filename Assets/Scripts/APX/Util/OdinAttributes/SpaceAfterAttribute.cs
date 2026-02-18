using System;
using Sirenix.OdinInspector;

namespace APX.Util.OdinAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    [IncludeMyAttributes]
    [PropertySpace(0, 10)]
    public class SpaceAfterAttribute : Attribute { }
}
