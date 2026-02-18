using System;
using UnityEditor;
using UnityEngine;

namespace APX.Extra.Misc
{
    public static class PlatformUtils
    {
        public static RuntimePlatform GetCurrentBuildTarget()
        {
#if UNITY_EDITOR
            BuildTarget activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            RuntimePlatform runtimePlatform = BuildTargetToRuntimePlatform(activeBuildTarget);

            return runtimePlatform;
#else
            return Application.platform;
#endif
        }
        
#if UNITY_EDITOR
        private static RuntimePlatform BuildTargetToRuntimePlatform(BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case BuildTarget.StandaloneOSX:
                    return RuntimePlatform.OSXPlayer;
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return RuntimePlatform.WindowsPlayer;
                case BuildTarget.StandaloneLinux64:
                    return RuntimePlatform.LinuxPlayer;
                case BuildTarget.iOS:
                    return RuntimePlatform.IPhonePlayer;
                case BuildTarget.Android:
                    return RuntimePlatform.Android;
                case BuildTarget.WebGL:
                    return RuntimePlatform.WebGLPlayer;
                case BuildTarget.WSAPlayer:
                    return RuntimePlatform.WSAPlayerX86;
                default:
                    throw new Exception("Unknown build target: " + buildTarget);
            }
        }
#endif
    }
}