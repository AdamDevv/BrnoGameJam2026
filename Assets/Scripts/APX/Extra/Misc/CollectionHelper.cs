using System.Collections.Generic;

namespace APX.Extra.Misc
{
    public static class CollectionHelper
    {
        public static List<TVal> ClearOrCreateNew<TVal>(List<TVal> currentValue)
        {
            if (currentValue != null)
            {
                currentValue.Clear();
                return currentValue;
            }

            return new List<TVal>();
        }
        
        public static Queue<TVal> ClearOrCreateNew<TVal>(Queue<TVal> currentValue)
        {
            if (currentValue != null)
            {
                currentValue.Clear();
                return currentValue;
            }

            return new Queue<TVal>();
        }

        public static Dictionary<TKey, TVal> ClearOrCreateNew<TKey, TVal>(Dictionary<TKey, TVal> currentValue)
        {
            if (currentValue != null)
            {
                currentValue.Clear();
                return currentValue;
            }

            return new Dictionary<TKey, TVal>();
        }
    }
}
