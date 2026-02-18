using System;

namespace APX.Extra.Misc
{
    public static class ParamsExtensions
    {
        /// <summary>
        /// Try to find value of type T or it's derived types in args array.
        /// </summary>
        /// <param name="args">Array of params passed to method</param>
        /// <param name="value"></param>
        /// <returns>True if value of type T or it's derived types has been found. </returns>
        public static bool TryGetParam<T>(this object[] args, out T value)
        {
            if (args == null || args.Length == 0)
            {
                value = default;
                return false;
            }

            for (var i = 0; i < args.Length; i++)
            {
                if (args[i] is T targetValue)
                {
                    value = targetValue;
                    return true;
                }
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Try to find value of type T in args array. This method does not return instance of type derived from T
        /// </summary>
        /// <param name="args">Array of params passed to method</param>
        /// <param name="value"></param>
        /// <returns>True if value of exact type T has been found</returns>
        public static bool TryGetExactParam<T>(this object[] args, out T value)
        {
            if (args == null || args.Length == 0)
            {
                value = default;
                return false;
            }

            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                if (arg != null && arg.GetType() == typeof(T))
                {
                    value = (T) arg;
                    return true;
                }
            }

            value = default;
            return false;
        }

        public static object[] Combine(this object[] originalParamList, params object[] paramList)
        {
            if (originalParamList == null || originalParamList.Length == 0) return paramList;
            if (paramList == null || paramList.Length == 0) return originalParamList;

            var newParamList = new object[originalParamList.Length + paramList.Length];

            Array.Copy(originalParamList, newParamList, originalParamList.Length);
            Array.Copy(paramList, 0, newParamList, originalParamList.Length, paramList.Length);
            return newParamList;
        }
    }
}
