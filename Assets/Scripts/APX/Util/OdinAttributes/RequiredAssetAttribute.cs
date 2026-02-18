using System;
using Sirenix.OdinInspector;

namespace APX.Util.OdinAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    [IncludeMyAttributes]
    [Required]
    [AssetSelector]
    [AssetsOnly]
    public class RequiredAssetAttribute : Attribute { }
}
