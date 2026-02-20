using System;
using System.Collections.Generic;
using System.Linq;
using APX.Events.Data;
using APX.Util;
using Cysharp.Threading.Tasks;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

// ReSharper disable MemberHidesStaticFromOuterClass

namespace APX.Events
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<IHandlerData>> _handlersDictionary = new();

        public static int HandlersCount { get; private set; }

        public static void Subscribe(Type type, Action<object> handler)
        {
            SubscribeInternal(type, handler);
        }

        public static void Subscribe(Type type, Func<object, UniTaskVoid> handler)
        {
            SubscribeInternal(type, handler);
        }

        public static void Subscribe<T>(Action<T> handler)
        {
            SubscribeInternal(typeof(T), handler);
        }

        public static void Subscribe<T>(Func<T, UniTaskVoid> handler)
        {
            SubscribeInternal(typeof(T), handler);
        }

        public static void SubscribeOnce(Type type, Action<object> handler)
        {
            SubscribeInternal(type, handler, true);
        }

        public static void SubscribeOnce(Type type, Func<object, UniTaskVoid> handler)
        {
            SubscribeInternal(type, handler, true);
        }

        public static void SubscribeOnce<T>(Action<T> handler)
        {
            SubscribeInternal(typeof(T), handler, true);
        }

        public static void SubscribeOnce<T>(Func<T, UniTaskVoid> handler)
        {
            SubscribeInternal(typeof(T), handler, true);
        }

        private static void SubscribeInternal<T>(Type type, Action<T> handler, bool triggerOnce = false)
        {
            if (!_handlersDictionary.TryGetValue(type, out var handlers))
            {
                handlers = new();
                _handlersDictionary.Add(type, handlers);
            }

            handlers.Add(new HandlerData<T>(handler, triggerOnce));
            HandlersCount++;
        }

        private static void SubscribeInternal<T>(Type type, Func<T, UniTaskVoid> handler, bool triggerOnce = false)
        {
            if (!_handlersDictionary.TryGetValue(type, out var handlers))
            {
                handlers = new();
                _handlersDictionary.Add(type, handlers);
            }

            handlers.Add(new HandlerDataUniTask<T>(handler, triggerOnce));
            HandlersCount++;
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            UnsubscribeInternal(typeof(T), handler);
        }

        public static void Unsubscribe<T>(Func<T, UniTaskVoid> handler)
        {
            UnsubscribeInternal(typeof(T), handler);
        }

        public static void Unsubscribe(Type type, Action<object> handler) => UnsubscribeInternal(type, handler);

        public static void Unsubscribe(Type type, Func<object, UniTaskVoid> handler) => UnsubscribeInternal(type, handler);

        private static void UnsubscribeInternal<THandler>(Type type, THandler handler)
        {
            if (!_handlersDictionary.TryGetValue(type, out List<IHandlerData> handlers))
            {
                return;
            }

            for (int i = handlers.Count - 1; i >= 0; i--)
            {
                if (handler.Equals(handlers[i].HandlerAsObject))
                {
                    handlers.RemoveAt(i);
                    HandlersCount--;
                    break;
                }
            }
        }

        public static void Trigger<T>(T @event) => Trigger(typeof(T), @event);

        public static void Trigger(object @event)
        {
            if (@event is null) throw new ArgumentNullException(nameof(@event));

            Trigger(@event.GetType(), @event);
        }

        public static void Trigger(Type type, object @event)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (@event is null) throw new ArgumentNullException(nameof(@event));

            if (!_handlersDictionary.TryGetValue(type, out var handlers))
            {
                return;
            }

            foreach (IHandlerData handlerData in handlers.ToList())
            {
                if (handlerData.TriggerOnce)
                {
                    handlers.Remove(handlerData);
                }

                try
                {
                    handlerData.Trigger(@event);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error while triggering event {@event}:\n{e}");
                }
            }
        }

        public static void Reset()
        {
            _handlersDictionary.Clear();
            HandlersCount = 0;
        }

        #if UNITY_EDITOR

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void InitializeOnLoad()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state is PlayModeStateChange.ExitingPlayMode or PlayModeStateChange.EnteredEditMode)
            {
                Reset();
            }
        }

        #endif
    }
}
