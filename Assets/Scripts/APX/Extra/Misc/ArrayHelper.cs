using System.Linq;

namespace APX.Extra.Misc
{
    public static class ArrayHelper
    {
        public static T[] ConcatArrays<T>(this T[] array1, T[] array2)
        {
            var concat = new T[array1.Length + array2.Length];
            array1.CopyTo(concat, 0);
            array2.CopyTo(concat, array1.Length);
            return concat;
        }

        public static T[] PrependToArray<T>(T[] array, T item)
        {
            var concat = new T[array.Length + 1];
            concat[0] = item;
            array.CopyTo(concat, 1);
            return concat;
        }

        public static T[] AppendToArray<T>(T[] array, T item)
        {
            var concat = new T[array.Length + 1];
            array.CopyTo(concat, 0);
            concat[array.Length] = item;
            return concat;
        }
        
        public static T[] GetColumn<T>(this T[,] matrix, int columnNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                .Select(x => matrix[columnNumber, x])
                .ToArray();
        }

        public static T[] GetRow<T>(this T[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0))
                .Select(x => matrix[x,rowNumber])
                .ToArray();
        }
        
        public static bool IsIndexInRange<T>(this T[] array, int index)
        {
            if (array == null) return false;
            return 0 <= index && index < array.Length;
        }
    }
}
