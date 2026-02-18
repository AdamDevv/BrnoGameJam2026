using System.Collections.Generic;

namespace APX.Extra.Misc
{
    public static class ListHelper
    {
        /// <summary>
        /// Optimized method to remove items in itemsToRemoveList from srcList
        /// ! -WARNING- ! The method also removes any null values from srcList!
        /// </summary>
        /// <param name="srcList">List to modify</param>
        /// <param name="itemsToRemoveList">List with items to remove</param>
        /// <typeparam name="T">Only class type is supported</typeparam>
        /// <returns>Number of removed items</returns>
        public static int RemoveUnion<T>(List<T> srcList, List<T> itemsToRemoveList) where T : class
        {
            if (srcList == null || itemsToRemoveList == null || srcList.Count == 0 || itemsToRemoveList.Count == 0) return 0;

            if (srcList.Count > itemsToRemoveList.Count)
            {
                for (int i = 0, iMax = itemsToRemoveList.Count; i < iMax; i++)
                {
                    T item = itemsToRemoveList[i];
                    int index = srcList.IndexOf(item);
                    if (index > -1)
                    {
                        srcList[index] = null;
                    }
                }
            }
            else
            {
                for (int i = 0, iMax = srcList.Count; i < iMax; i++)
                {
                    if (itemsToRemoveList.Contains(srcList[i]))
                    {
                        srcList[i] = null;
                    }
                }
            }

            return srcList.RemoveAll(item => item == null);
        }
    }

    public static class ListExtensions
    {
        /// <summary>
        /// Optimized method to remove items in itemsToRemoveList from srcList
        /// ! -WARNING- ! The method also removes any null values from srcList!
        /// </summary>
        /// <param name="srcList">List to modify</param>
        /// <param name="itemsToRemoveList">List with items to remove</param>
        /// <typeparam name="T">Only class type is supported</typeparam>
        /// <returns>Number of removed items</returns>
        public static int RemoveUnion<T>(this List<T> srcList, List<T> itemsToRemoveList) where T : class { return ListHelper.RemoveUnion<T>(srcList, itemsToRemoveList); }

        // Copied from Sirenix.Utilities.LinqExtensions
        /// <summary>Sorts an IList</summary>
        public static void Sort<T>(this IList<T> list, System.Comparison<T> comparison)
        {
            if (list is List<T>)
            {
                ((List<T>) list).Sort(comparison);
            }
            else
            {
                List<T> objList = new List<T>((IEnumerable<T>) list);
                objList.Sort(comparison);
                for (int index = 0; index < list.Count; ++index)
                    list[index] = objList[index];
            }
        }

        public static void InvokeAll(this List<System.Action> actList)
        {
            if (actList == null)
                return;

            foreach (var action in actList)
            {
                action?.Invoke();
            }
        }

        public static void SetCount<T>(this List<T> list, int count)
        {
            while (list.Count > count)
                list.RemoveAt(list.Count - 1);
            while (list.Count < count)
                list.Add(default);
        }
        
        public static bool IsIndexInRange<T>(this IReadOnlyList<T> list, int index)
        {
            if (list == null) return false;
            return 0 <= index && index < list.Count;
        }
    }
}
