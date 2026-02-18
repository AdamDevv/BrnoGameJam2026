using System;
using System.Collections.Generic;
using System.Linq;
using APX.Util;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace APX.Events
{
    public static class EventBus
    {
        private interface IHandlerData
        {
            object HandlerAsObject { get; }
            bool TriggerOnce { get; }

            void Trigger(object @event);
        }

        private class HandlerData<T> : IHandlerData
        {
            public object HandlerAsObject => Handler;
            public Action<T> Handler { get; }
            public bool TriggerOnce { get; }

            public HandlerData(Action<T> handler, bool triggerOnce)
            {
                Handler = handler;
                TriggerOnce = triggerOnce;
            }

            void IHandlerData.Trigger(object @event) => Handler((T)@event);
        }

        private static readonly Dictionary<Type, List<IHandlerData>> _handlersDictionary = new();
        private static readonly Dictionary<Type, object> _eventInstancePool = new();

        public static int HandlersCount { get; private set; }

        public static void Subscribe(Type type, Action<object> handler)
        {
            SubscribeInternal(type, handler);
        }

        public static void Subscribe<T>(Action<T> handler)
        {
            SubscribeInternal(typeof(T), handler);
        }

        public static void SubscribeOnce(Type type, Action<object> handler)
        {
            SubscribeInternal(type, handler, true);
        }

        public static void SubscribeOnce<T>(Action<T> handler)
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

        public static void Unsubscribe<T>(Action<T> handler)
        {
            UnsubscribeInternal(typeof(T), handler);
        }

        public static void Unsubscribe(Type type, Action<object> handler)
        {
            UnsubscribeInternal(type, handler);
        }

        private static void UnsubscribeInternal<T>(Type type, Action<T> handler)
        {
            if (!_handlersDictionary.TryGetValue(type, out List<IHandlerData> handlers))
            {
                return;
            }

            for (int i = handlers.Count - 1; i >= 0; i--)
            {
                if (ReferenceEquals(handlers[i].HandlerAsObject, handler))
                {
                    handlers.RemoveAt(i);
                    HandlersCount--;
                    break;
                }
            }
        }

        public static void Trigger<T>() => Trigger(typeof(T));

        public static void Trigger<T>(T @event) => Trigger(typeof(T), @event);

        public static void Trigger(object @event) => Trigger(@event.GetType(), @event);

        public static void Trigger(Type type)
        {
            if (!_eventInstancePool.TryGetValue(type, out object @event))
            {
                try
                {
                    @event = type.CreateDefaultValue();
                }
                catch (Exception e)
                {
                    Debug.LogError($"[{nameof(EventBus)}] Creating default event instance error: {e.Message}");
                    throw;
                }

                _eventInstancePool.Add(type, @event);
            }

            Trigger(type, @event);
        }

        public static void Trigger(Type type, object @event)
        {
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
                    Debug.LogError(e);
                }
            }
        }

        public static void Reset()
        {
            _handlersDictionary.Clear();
            _eventInstancePool.Clear();
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
