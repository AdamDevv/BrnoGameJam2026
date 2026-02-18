using System.Collections.Generic;
using System.Text;

namespace APX.Extra.Misc
{
    /// <summary>
    /// Creates a pool that will cache instances of objects for later use so that you don't have to construct them again. 
    /// There is a max cache size, if set to 0 or less, it's considered endless in size.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectCachePool<T> : ICachePool<T> where T : class
    {
    #region Fields
        private HashSet<T> _inactive;

        private int _cacheSize;
        private System.Func<T> _constructorDelegate;
        private System.Action<T> _resetObjectDelegate;
        private bool _resetOnGet;

#if SHOW_TEMP_LIST_LOGS
        private int _totalCreations;
        public int WarnIfObjectCreationsExceed = 0;
#endif
    #endregion


    #region CONSTRUCTOR
        public ObjectCachePool(int cacheSize)
        {
            _cacheSize = cacheSize;
            _inactive = new HashSet<T>();
            _constructorDelegate = SimpleConstructor;
        }

        public ObjectCachePool(int cacheSize, System.Func<T> constructorDelegate)
        {
            _cacheSize = cacheSize;
            _inactive = new HashSet<T>();
            _constructorDelegate = constructorDelegate ?? SimpleConstructor;
        }

        public ObjectCachePool(int cacheSize, System.Func<T> constructorDelegate, System.Action<T> resetObjectDelegate)
        {
            _cacheSize = cacheSize;
            _inactive = new HashSet<T>();
            _constructorDelegate = constructorDelegate ?? SimpleConstructor;
            _resetObjectDelegate = resetObjectDelegate;
        }

        public ObjectCachePool(int cacheSize, System.Func<T> constructorDelegate, System.Action<T> resetObjectDelegate, bool resetOnGet)
        {
            _cacheSize = cacheSize;
            _inactive = new HashSet<T>();
            _constructorDelegate = constructorDelegate ?? SimpleConstructor;
            _resetObjectDelegate = resetObjectDelegate;
            _resetOnGet = resetOnGet;
        }

        private T SimpleConstructor() { return System.Activator.CreateInstance<T>(); }
    #endregion


    #region Properties
        public int CacheSize { get => _cacheSize; set => _cacheSize = value; }

        public bool ResetOnGet { get => _resetOnGet; set => _resetOnGet = value; }

        public int InactiveCount => _inactive.Count;
    #endregion


    #region Methods
        public bool TryGetInstance(out T result)
        {
            result = null;
            lock (_inactive)
            {
                if (_inactive.Count > 0)
                {
                    result = _inactive.Pop();
#if SHOW_TEMP_LIST_LOGS
                    Debug.Log($"Pool for {typeof(T).GetFriendlyName()} - popping instance");
#endif
                }
            }

            if (result != null)
            {
                if (_resetOnGet && _resetObjectDelegate != null)
                    _resetObjectDelegate(result);
                return true;
            }
            else
            {
                return false;
            }
        }

        public T GetInstance()
        {
            T result = null;
            lock (_inactive)
            {
                if (_inactive.Count > 0)
                {
                    result = _inactive.Pop();
#if SHOW_TEMP_LIST_LOGS
                    Debug.Log($"Pool for {typeof(T).GetFriendlyName()} - popping instance");
#endif
                }
            }

            if (result != null)
            {
                if (_resetOnGet && _resetObjectDelegate != null)
                    _resetObjectDelegate(result);
                return result;
            }
            else
            {
#if SHOW_TEMP_LIST_LOGS
                _totalCreations++;
                if (WarnIfObjectCreationsExceed > 0 && _totalCreations > WarnIfObjectCreationsExceed)
                {
                    Debug.LogWarning($"Pool for {typeof(T).GetFriendlyName()} - created new instance (n{_totalCreations})");
                }
                else
                {
                    Debug.Log($"Pool for {typeof(T).GetFriendlyName()} - creating new instance (n{_totalCreations})");
                }
#endif
                return _constructorDelegate();
            }
        }

        public bool Release(T obj)
        {
            if (obj == null) throw new System.ArgumentNullException("obj");

            var cacheSize = _cacheSize > 0 ? _cacheSize : 1024; //we actually don't allow the cache size to get out of hand
            if (!_resetOnGet && _resetObjectDelegate != null && _inactive.Count < cacheSize) _resetObjectDelegate(obj);

            lock (_inactive)
            {
                if (_inactive.Count < cacheSize)
                {
                    _inactive.Add(obj);
#if SHOW_TEMP_LIST_LOGS
                    Debug.Log($"Pool for {typeof(T).GetFriendlyName()} - releasing instance (pooled {_inactive.Count}, created {_totalCreations})");
#endif
                    return true;
                }
            }

            return false;
        }

        void ICachePool<T>.Release(T obj) { Release(obj); }

        public bool IsTreatedAsInactive(T obj) { return _inactive.Contains(obj); }

        public string PrintCapacities()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Pool for '");
            stringBuilder.Append(typeof(T).GetFriendlyName());
            stringBuilder.Append("' count:");
            stringBuilder.Append(_inactive.Count);

            var count = 0;
            foreach (var item in _inactive)
            {
                if (item is List<T> list)
                {
                    stringBuilder.Append(count == 0 ? "  Capacities:{" : ", ");
                    stringBuilder.Append(list.Capacity);
                }

                count++;
            }

            if (count > 0) stringBuilder.Append("}");

            return stringBuilder.ToString();
        }
    #endregion
    }
}
