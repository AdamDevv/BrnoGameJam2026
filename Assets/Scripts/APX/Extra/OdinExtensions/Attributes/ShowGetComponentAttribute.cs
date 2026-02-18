using System;
using System.Diagnostics;

namespace APX.Extra.OdinExtensions.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    [Conditional("UNITY_EDITOR")]
    public class ShowGetComponentAttribute : Attribute
    {
        public GetComponentTargetPlace TargetPlace = GetComponentTargetPlace.Local;

        public ShowGetComponentAttribute() { }

        public ShowGetComponentAttribute(GetComponentTargetPlace targetPlace)
        {
            TargetPlace = targetPlace;
        }
        
        public ShowGetComponentAttribute(bool required, GetComponentTargetPlace targetPlace)
        {
            TargetPlace = targetPlace;
        }
    }

    [Flags]
    public enum GetComponentTargetPlace
    {
        Local       = 1 << 0,
        Children    = 1 << 1,
        Parent      = 1 << 2,
        Anywhere    = 1 << 3,
    }
}
