using System;
using System.Collections.Generic;
using System.Linq;

namespace APX.Extra.Signals
{
    public class ConsumableSignal : BaseSignal
    {
        private class Listener: IComparable<Listener>
        {
            public int Priority;
            public Func<bool> Callback;

            public int CompareTo(Listener other) { return Priority.CompareTo(other.Priority); }

            public Listener(Func<bool> callback, int priority)
            {
                Priority = priority;
                Callback = callback;
            }
        }

        private SortedSet<Listener> _listeners = new SortedSet<Listener>();

        /// <summary>
        /// Lower number means higher priority
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="priority"></param>
        public void AddListener(Func<bool> callback, int priority)
        {
            AddUnique(_listeners, callback, priority);
        }

        public void RemoveListener(Func<bool> callback) { _listeners.RemoveWhere(l => l.Callback == callback); }
        
        public override List<Type> GetTypes() { return new List<Type>(); }

        public bool Dispatch()
        {
            foreach (var listener in _listeners)
            {
                bool consumed = listener.Callback.Invoke();
                if (consumed)
                    return true;
            }
            
            base.Dispatch(null);
            return false;
        }
        
        private void AddUnique(SortedSet<Listener> listeners, Func<bool> callback, int priority)
        {
            if (listeners.All(l => l.Callback != callback))
            {
                listeners.Add(new Listener(callback, priority));
            }
        }
        
        public override void RemoveAllListeners()
        {
            _listeners.Clear();
            base.RemoveAllListeners();
        }
    }
}
