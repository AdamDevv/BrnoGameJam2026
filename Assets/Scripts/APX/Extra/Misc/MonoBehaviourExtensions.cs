using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace APX.Extra.Misc
{
    public static class MonoBehaviourExtensions
    {
        /// <summary>
        ///     Gets or add a component. Usage example:
        ///     BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
        /// </summary>
        public static T GetOrAddComponent<T>(this Component child, out bool newlyAdded) where T : Component
        {
            if (child.TryGetComponent<T>(out var component))
            {
                newlyAdded = false;
                return component;
            }

            newlyAdded = true;
            return child.gameObject.AddComponent<T>();
        }

        public static Component GetOrAddComponent(this Component child, System.Type type)
        {
            if (child.TryGetComponent(type, out var component))
            {
                return component;
            }

            return child.gameObject.AddComponent(type);
        }

        public static T GetOrAddComponent<T>(this Component child) where T : Component
        {
            if (child.TryGetComponent<T>(out var component))
            {
                return component;
            }

            return child.gameObject.AddComponent<T>();
        }

        public static TBase GetOrAddComponent<TBase, T>(this Component child)
            where TBase : class
            where T : Component, TBase
        {
            if (child.TryGetComponent<TBase>(out var component))
            {
                return component;
            }

            return child.gameObject.AddComponent<T>();
        }

        public static void SortChildren(this Transform transform, System.Comparison<Transform> comparison)
        {
            var children = new List<Transform>();
            for (var i = 0; i < transform.childCount; i++)
            {
                children.Add(transform.GetChild(i));
            }

            children.Sort(comparison);
            for (var i = 0; i < children.Count; i++)
            {
                children[i].SetSiblingIndex(i);
            }
        }

        public static Coroutine InvokeAfterTime(this MonoBehaviour me, System.Action action, float time)
        {
            if (action == null) throw new System.ArgumentNullException();
            return me.StartCoroutine(ExecuteAfterTime(action, time));
        }

        public static Coroutine InvokeAfterTimeRealtime(this MonoBehaviour me, System.Action action, float time)
        {
            if (action == null) throw new System.ArgumentNullException();
            return me.StartCoroutine(ExecuteAfterTimeRealtime(action, time));
        }

        public static Coroutine InvokeInFrames(this MonoBehaviour me, System.Action action, int frames)
        {
            if (action == null) throw new System.ArgumentNullException();
            return me.StartCoroutine(ExecuteAfterFrames(action, frames));
        }

        public static Coroutine InvokeNextFrame(this MonoBehaviour me, System.Action action)
        {
            if (action == null) throw new System.ArgumentNullException();
            return me.StartCoroutine(ExecuteAfterFrames(action, 1));
        }

        public static Coroutine InvokeNextEndOfFrame(this MonoBehaviour me, System.Action action)
        {
            if (action == null) throw new System.ArgumentNullException();
            return me.StartCoroutine(ExecuteOnNextEndOfFrame(action));
        }

        public static Coroutine InvokeOnEndOfFrame(this MonoBehaviour me, System.Action action)
        {
            if (action == null) throw new System.ArgumentNullException();
            return me.StartCoroutine(ExecuteOnEndOfFrame(action));
        }

        public static Coroutine InvokeWhen(this MonoBehaviour me, System.Func<bool> predicate, System.Action action)
        {
            if (action == null) throw new System.ArgumentNullException();
            return me.StartCoroutine(ExecuteOnPredicate(predicate, action));
        }

        private static IEnumerator ExecuteAfterTime(System.Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }

        private static IEnumerator ExecuteAfterTimeRealtime(System.Action action, float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            action.Invoke();
        }

        private static IEnumerator ExecuteAfterFrames(System.Action action, int frames)
        {
            for (var i = 0; i < frames; i++) yield return null;
            action.Invoke();
        }

        private static IEnumerator ExecuteOnNextEndOfFrame(System.Action action)
        {
            yield return null;
            yield return new WaitForEndOfFrame();
            action.Invoke();
        }

        private static IEnumerator ExecuteOnEndOfFrame(System.Action action)
        {
            yield return new WaitForEndOfFrame();
            action.Invoke();
        }

        private static IEnumerator ExecuteOnPredicate(System.Func<bool> predicate, System.Action action)
        {
            yield return new WaitUntil(predicate);
            action.Invoke();
        }
    }
}
