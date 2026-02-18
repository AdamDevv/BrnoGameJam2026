using System;
using Sirenix.OdinInspector;

namespace APX.Util.OdinAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    [IncludeMyAttributes]
    [Required]
    [ChildGameObjectsOnly]
    public class RequiredChildAttribute : Attribute { }
}
