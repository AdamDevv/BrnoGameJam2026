using System.Diagnostics;
using Sirenix.OdinInspector;

namespace APX.Extra.OdinExtensions.Attributes
{
    [Conditional("UNITY_EDITOR")]
    public class OnSceneGUIAttribute : ShowInInspectorAttribute
    {
        
    }
}
