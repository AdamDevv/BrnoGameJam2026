#if UNITY_EDITOR

using System;
using System.Diagnostics;

namespace APX.Extra.GameManagement.Runtime
{
    [AttributeUsage(AttributeTargets.All)]
    [Conditional("UNITY_EDITOR")]
    public sealed class ShowOpenInManagementAttribute : Attribute
    {
        public string ShowIf;

        public ShowOpenInManagementAttribute() { }

        public ShowOpenInManagementAttribute(string showIf)
        {
            ShowIf = showIf;
        }
    }
}

#endif