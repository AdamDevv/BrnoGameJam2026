using System.Collections.Generic;

namespace APX.Extra.Misc
{
    public class TempHashSet<T> : HashSet<T>, ITempCollection<T>
    {
        private const int MAX_SIZE_INBYTES = 1024;


    #region Fields
        private static ObjectCachePool<TempHashSet<T>> _pool = new ObjectCachePool<TempHashSet<T>>(10, () => new TempHashSet<T>());

        private int _maxCapacityOnRelease;
    #endregion


    #region CONSTRUCTOR
        public TempHashSet()
            : base()
        {
            var tp = typeof(T);
            var sz = (tp.IsValueType && !tp.IsEnum) ? System.Runtime.InteropServices.Marshal.SizeOf(tp) : 4;
            _maxCapacityOnRelease = MAX_SIZE_INBYTES / sz;
        }

        public TempHashSet(IEnumerable<T> e)
            : base(e)
        {
            var tp = typeof(T);
            var sz = (tp.IsValueType && !tp.IsEnum) ? System.Runtime.InteropServices.Marshal.SizeOf(tp) : 4;
            _maxCapacityOnRelease = MAX_SIZE_INBYTES / sz;
        }
    #endregion


    #region IDisposable Interface
        public void Dispose()
        {
            Clear();
            _pool.Release(this);
        }
    #endregion


    #region Static Methods
        public static TempHashSet<T> GetSet() { return _pool.GetInstance(); }

        public static TempHashSet<T> GetSet(IEnumerable<T> e)
        {
            TempHashSet<T> result;
            if (_pool.TryGetInstance(out result))
            {
                var le = LightEnumerator.Create<T>(e);
                while (le.MoveNext())
                {
                    result.Add(le.Current);
                }
            }
            else
            {
                result = new TempHashSet<T>(e);
            }

            return result;
        }
    #endregion
    }
}
