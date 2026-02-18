using System.Collections.Generic;

namespace APX.Extra.Misc
{
    public class TempList<T> : List<T>, ITempCollection<T>
    {
    #region Fields
        public static int MaxCapacity = 512;

        private static ObjectCachePool<TempList<T>> _pool = new ObjectCachePool<TempList<T>>(10, () => new TempList<T>());
    #endregion


    #region CONSTRUCTOR
        public TempList() : base() { Initialize(); }

        public TempList(IEnumerable<T> e) : base(e) { Initialize(); }

        public TempList(int count) : base(count) { Initialize(); }
    #endregion


    #region Public Methods
        private static void Initialize()
        {
#if SHOW_TEMP_LIST_LOGS
            _pool.WarnIfObjectCreationsExceed = 8;
#endif
        }
    #endregion


    #region IDisposable Interface
        public void Dispose()
        {
            Clear();

            if (Capacity <= MaxCapacity)
            {
                _pool.Release(this);
            }
        }
    #endregion


    #region Static Methods
        public static TempList<T> GetList() { return _pool.GetInstance(); }

        public static TempList<T> GetList(IEnumerable<T> e)
        {
            if (_pool.TryGetInstance(out var result))
            {
                //result.AddRange(e);
                var e2 = new LightEnumerator<T>(e);
                while (e2.MoveNext())
                {
                    result.Add(e2.Current);
                }
            }
            else
            {
                result = new TempList<T>(e);
            }

            return result;
        }

        public static TempList<T> GetList(int count)
        {
            if (_pool.TryGetInstance(out var result))
            {
                if (result.Capacity < count) result.Capacity = count;
                return result;
            }

            result = new TempList<T>(count);

            return result;
        }
    #endregion
    }
}
