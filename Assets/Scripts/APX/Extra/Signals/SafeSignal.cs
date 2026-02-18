using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace APX.Extra.Signals
{
    /// Base concrete form for a Signal with no parameters
    public class SafeSignal : BaseSafeSignal, ISignal
    {
        public event Action Listener = null;
        public event Action OnceListener = null;

        public void AddListener(Action callback) { Listener = AddUnique(Listener, callback); }

        public void AddOnce(Action callback) { OnceListener = AddUnique(OnceListener, callback); }

        public void RemoveListener(Action callback)
        {
            if (Listener != null)
                Listener -= callback;
        }

        public override List<Type> GetTypes() { return new List<Type>(); }

        public void Dispatch()
        {
            if (Listener != null)
                InvokeSafe(Listener);
            if (OnceListener != null)
                InvokeSafe(OnceListener);
            OnceListener = null;
            base.Dispatch(null);
        }

        private void InvokeSafe(Action listeners)
        {
            Delegate[] invocationList = listeners.GetInvocationList();
            for (int i = 0; i < invocationList.Length; i++)
            {
                try
                {
                    if (invocationList[i] is Action action) action.Invoke();
                }
                catch (Exception exception)
                {
                    LogException(exception);
                }
            }
        }

        private Action AddUnique(Action listeners, Action callback)
        {
            if (listeners == null || !listeners.GetInvocationList().Contains(callback))
            {
                listeners += callback;
            }

            return listeners;
        }

        public override void RemoveAllListeners()
        {
            Listener = null;
            OnceListener = null;
            base.RemoveAllListeners();
        }

        public Delegate listener { get { return Listener ?? (Listener = delegate { }); } set { Listener = (Action) value; } }
    }

    /// Base concrete form for a Signal with one parameter
    public class SafeSignal<T> : BaseSignal, ISignal
    {
        public event Action<T> Listener = null;
        public event Action<T> OnceListener = null;

        public void AddListener(Action<T> callback) { Listener = AddUnique(Listener, callback); }

        public void AddOnce(Action<T> callback) { OnceListener = AddUnique(OnceListener, callback); }

        public void RemoveListener(Action<T> callback)
        {
            if (Listener != null)
                Listener -= callback;
        }

        public override List<Type> GetTypes()
        {
            List<Type> retv = new List<Type>();
            retv.Add(typeof(T));
            return retv;
        }

        public void Dispatch(T type1)
        {
            if (Listener != null)
                InvokeSafe(Listener, type1);
            if (OnceListener != null)
                InvokeSafe(OnceListener, type1);
            OnceListener = null;
            object[] outv = {type1};
            base.Dispatch(outv);
        }

        private void InvokeSafe(Action<T> listeners, T type1)
        {
            Delegate[] invocationList = listeners.GetInvocationList();
            for (int i = 0; i < invocationList.Length; i++)
            {
                try
                {
                    if (invocationList[i] is Action<T> action) action.Invoke(type1);
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception);
                }
            }
        }

        private Action<T> AddUnique(Action<T> listeners, Action<T> callback)
        {
            if (listeners == null || !listeners.GetInvocationList().Contains(callback))
            {
                listeners += callback;
            }

            return listeners;
        }

        public override void RemoveAllListeners()
        {
            Listener = null;
            OnceListener = null;
            base.RemoveAllListeners();
        }

        public Delegate listener { get { return Listener ?? (Listener = delegate { }); } set { Listener = (Action<T>) value; } }
    }

    /// Base concrete form for a Signal with two parameters
    public class SafeSignal<T, U> : BaseSignal, ISignal
    {
        public event Action<T, U> Listener = null;
        public event Action<T, U> OnceListener = null;

        public void AddListener(Action<T, U> callback) { Listener = AddUnique(Listener, callback); }

        public void AddOnce(Action<T, U> callback) { OnceListener = AddUnique(OnceListener, callback); }

        public void RemoveListener(Action<T, U> callback)
        {
            if (Listener != null)
                Listener -= callback;
        }

        public override List<Type> GetTypes()
        {
            List<Type> retv = new List<Type>();
            retv.Add(typeof(T));
            retv.Add(typeof(U));
            return retv;
        }

        public void Dispatch(T type1, U type2)
        {
            if (Listener != null)
                InvokeSafe(Listener, type1, type2);
            if (OnceListener != null)
                InvokeSafe(OnceListener, type1, type2);
            OnceListener = null;
            object[] outv = {type1, type2};
            base.Dispatch(outv);
        }

        private void InvokeSafe(Action<T, U> listeners, T type1, U type2)
        {
            Delegate[] invocationList = listeners.GetInvocationList();
            for (int i = 0; i < invocationList.Length; i++)
            {
                try
                {
                    if (invocationList[i] is Action<T, U> action) action.Invoke(type1, type2);
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception);
                }
            }
        }

        private Action<T, U> AddUnique(Action<T, U> listeners, Action<T, U> callback)
        {
            if (listeners == null || !listeners.GetInvocationList().Contains(callback))
            {
                listeners += callback;
            }

            return listeners;
        }

        public override void RemoveAllListeners()
        {
            Listener = null;
            OnceListener = null;
            base.RemoveAllListeners();
        }

        public Delegate listener { get { return Listener ?? (Listener = delegate { }); } set { Listener = (Action<T, U>) value; } }
    }

    /// Base concrete form for a Signal with three parameters
    public class SafeSignal<T, U, V> : BaseSignal, ISignal
    {
        public event Action<T, U, V> Listener = null;
        public event Action<T, U, V> OnceListener = null;

        public void AddListener(Action<T, U, V> callback) { Listener = AddUnique(Listener, callback); }

        public void AddOnce(Action<T, U, V> callback) { OnceListener = AddUnique(OnceListener, callback); }

        public void RemoveListener(Action<T, U, V> callback)
        {
            if (Listener != null)
                Listener -= callback;
        }

        public override List<Type> GetTypes()
        {
            List<Type> retv = new List<Type>();
            retv.Add(typeof(T));
            retv.Add(typeof(U));
            retv.Add(typeof(V));
            return retv;
        }

        public void Dispatch(T type1, U type2, V type3)
        {
            if (Listener != null)
                InvokeSafe(Listener, type1, type2, type3);
            if (OnceListener != null)
                InvokeSafe(OnceListener, type1, type2, type3);
            OnceListener = null;
            object[] outv = {type1, type2, type3};
            base.Dispatch(outv);
        }

        private void InvokeSafe(Action<T, U, V> listeners, T type1, U type2, V type3)
        {
            Delegate[] invocationList = listeners.GetInvocationList();
            for (int i = 0; i < invocationList.Length; i++)
            {
                try
                {
                    if (invocationList[i] is Action<T, U, V> action) action.Invoke(type1, type2, type3);
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception);
                }
            }
        }

        private Action<T, U, V> AddUnique(Action<T, U, V> listeners, Action<T, U, V> callback)
        {
            if (listeners == null || !listeners.GetInvocationList().Contains(callback))
            {
                listeners += callback;
            }

            return listeners;
        }

        public override void RemoveAllListeners()
        {
            Listener = null;
            OnceListener = null;
            base.RemoveAllListeners();
        }

        public Delegate listener { get { return Listener ?? (Listener = delegate { }); } set { Listener = (Action<T, U, V>) value; } }
    }

    /// Base concrete form for a Signal with four parameters
    public class SafeSignal<T, U, V, W> : BaseSignal, ISignal
    {
        public event Action<T, U, V, W> Listener = null;
        public event Action<T, U, V, W> OnceListener = null;

        public void AddListener(Action<T, U, V, W> callback) { Listener = AddUnique(Listener, callback); }

        public void AddOnce(Action<T, U, V, W> callback) { OnceListener = AddUnique(OnceListener, callback); }

        public void RemoveListener(Action<T, U, V, W> callback)
        {
            if (Listener != null)
                Listener -= callback;
        }

        public override List<Type> GetTypes()
        {
            List<Type> retv = new List<Type>();
            retv.Add(typeof(T));
            retv.Add(typeof(U));
            retv.Add(typeof(V));
            retv.Add(typeof(W));
            return retv;
        }

        public void Dispatch(T type1, U type2, V type3, W type4)
        {
            if (Listener != null)
                InvokeSafe(Listener, type1, type2, type3, type4);
            if (OnceListener != null)
                InvokeSafe(OnceListener, type1, type2, type3, type4);
            OnceListener = null;
            object[] outv = {type1, type2, type3, type4};
            base.Dispatch(outv);
        }

        private void InvokeSafe(Action<T, U, V, W> listeners, T type1, U type2, V type3, W type4)
        {
            Delegate[] invocationList = listeners.GetInvocationList();
            for (int i = 0; i < invocationList.Length; i++)
            {
                try
                {
                    if (invocationList[i] is Action<T, U, V, W> action) action.Invoke(type1, type2, type3, type4);
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception);
                }
            }
        }

        private Action<T, U, V, W> AddUnique(Action<T, U, V, W> listeners, Action<T, U, V, W> callback)
        {
            if (listeners == null || !listeners.GetInvocationList().Contains(callback))
            {
                listeners += callback;
            }

            return listeners;
        }

        public override void RemoveAllListeners()
        {
            Listener = null;
            OnceListener = null;
            base.RemoveAllListeners();
        }

        public Delegate listener { get { return Listener ?? (Listener = delegate { }); } set { Listener = (Action<T, U, V, W>) value; } }
    }
}
