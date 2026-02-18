#if UNITY_EDITOR
using System;
using System.Collections;
using APX.Extra.Promises;
using Unity.EditorCoroutines.Editor;

namespace APX.Extra.EditorUtils
{
    public static class WaitInEditor
    {
        public static void ForSeconds(float seconds, Action callback)
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(WaitForSeconds(seconds, callback));
        }
        
        public static Promise ForSeconds(float seconds)
        {
            var waitPromise = new Promise();
            EditorCoroutineUtility.StartCoroutineOwnerless(WaitForSeconds(seconds, waitPromise.Dispatch));
            return waitPromise;
        }
        
        public static void ForFrames(int frames, Action callback)
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(WaitForFrames(frames, callback));
        }
        
        public static Promise ForFrames(int frames)
        {
            var waitPromise = new Promise();
            EditorCoroutineUtility.StartCoroutineOwnerless(WaitForFrames(frames, waitPromise.Dispatch));
            return waitPromise;
        }
        
        public static void ForNextFrame(Action callback)
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(WaitForFrames(1, callback));
        }
        
        public static Promise ForNextFrame()
        {
            var waitPromise = new Promise();
            EditorCoroutineUtility.StartCoroutineOwnerless(WaitForFrames(1, waitPromise.Dispatch));
            return waitPromise;
        }

        private static IEnumerator WaitForSeconds(float seconds, Action afterWaitCallback)
        {
            yield return new EditorWaitForSeconds(seconds);
            afterWaitCallback?.Invoke();
        }
        
        private static IEnumerator WaitForFrames(int frames, Action afterWaitCallback)
        {
            for (var i = 0; i < frames; i++)
            {
                yield return null;
            }
            afterWaitCallback?.Invoke();
        }
    }
}
#endif
