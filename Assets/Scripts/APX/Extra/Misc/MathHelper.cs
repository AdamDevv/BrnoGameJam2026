using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace APX.Extra.Misc
{
    public static class MathHelper
    {
        public static long Min(long value1, long value2) { return value1 < value2 ? value1 : value2; }
        public static int Min(int value1, int value2) { return value1 < value2 ? value1 : value2; }

        public static long Max(long value1, long value2) { return value1 > value2 ? value1 : value2; }
        public static int Max(int value1, int value2) { return value1 > value2 ? value1 : value2; }

        public static float Saturate(float value)
        {
            if (value < .0f) return .0f;
            if (value > 1.0f) return 1.0f;
            return value;
        }

        public static int Limit(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static float Limit(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static double Limit(double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
        
        public static bool IsInRange(this int i, int min, int max) => i >= min && i <= max;
        
        public static bool IsInRange(this float i, float min, float max) => i >= min && i <= max;
        
        public static bool IsOutsideLimit(float value, float min, float max)
        {
            if (value < min) return true;
            if (value > max) return true;
            return false;
        }
        
        public static bool Approximately(this float a, float b, float epsilon = 1e-6f) { return Mathf.Abs(a - b) < epsilon; }
        
        public static bool IsSignificantlyLarger(this float a, float b, float significance = 1e-6f) { return a - b > significance; }
        public static bool IsSignificantlySmaller(this float a, float b, float significance = 1e-6f) { return a - b < -significance; }

        public static long PositiveModulo(this long x, long m)
        {
            var r = x % m;
            return r < 0 ? r + m : r;
        }

        public static int PositiveModulo(this int x, int m)
        {
            var r = x % m;
            return r < 0 ? r + m : r;
        }

        public static float PositiveModulo(this float x, float m)
        {
            var c = x % m;
            if ((c < 0 && m > 0) || (c > 0 && m < 0))
            {
                c += m;
            }

            return c;
        }

        public static float LimitAngle(float value, float min, float max)
        {
#if DEBUG
            if (min < 0 || min > 360)
            {
                Debug.LogWarning("LimitAngle - \"min\" parameter should be in interval [0,360]!");
            }

            if (max < 0 || max > 360)
            {
                Debug.LogWarning("LimitAngle - \"max\" parameter should be in interval [0,360]!");
            }

            if (value < 0 || value > 360)
            {
                Debug.LogWarning("LimitAngle - \"value\" parameter should be in interval [0,360]!");
            }
#endif
            if (min < max)
            {
                return Limit(value, min, max);
            }

            if (min > max)
            {
                if (value > min || value < max)
                    return value;

                float middle = (min + max) / 2;
                if (value > middle)
                    return min;

                return max;
            }

            return min;
        }

        public static bool IsOutsideAngle(float value, float min, float max)
        {
#if DEBUG
            if (min < 0 || min > 360)
            {
                Debug.LogWarning("LimitAngle - \"min\" parameter should be in interval [0,360]!");
            }

            if (max < 0 || max > 360)
            {
                Debug.LogWarning("LimitAngle - \"max\" parameter should be in interval [0,360]!");
            }

            if (value < 0 || value > 360)
            {
                Debug.LogWarning("LimitAngle - \"value\" parameter should be in interval [0,360]!");
            }
#endif
            if (min < max)
            {
                return IsOutsideLimit(value, min, max);
            }

            if (min > max)
            {
                if (value > min || value < max)
                    return false;

                float middle = (min + max) / 2;
                if (value > middle)
                    return true;

                return true;
            }

            return true;
        }

        public static float Atan2Normalize(float value)
        {
            if (value < 0)
                return -value;

            return 360 - value;
        }

        public static bool IsPowerOfTwo(int num)
        {
            if (num == 0)
                return false;

            return (num & (num - 1)) == 0;
        }

        public static Vector3 Lerp(Vector3 from, Vector3 to, float delta) { return new Vector3(from.x + (to.x - from.x) * delta, from.y + (to.y - from.y) * delta, from.z + (to.z - from.z) * delta); }

        // Source: https://wiki.unity3d.com/index.php/3d_Math_functions
        //create a vector of direction "vector" with length "size"
        public static Vector3 SetVectorLength(Vector3 vector, float size)
        {
            //normalize the vector
            Vector3 vectorNormalized = Vector3.Normalize(vector);

            //scale the vector
            return vectorNormalized *= size;
        }

        // Source: https://wiki.unity3d.com/index.php/3d_Math_functions
        //Get the intersection between a line and a plane. 
        //If the line and plane are not parallel, the function outputs true, otherwise false.
        public static bool LinePlaneIntersection(out Vector3 intersection, Vector3 linePoint, Vector3 lineVec, Vector3 planeNormal, Vector3 planePoint)
        {
            float length;
            float dotNumerator;
            float dotDenominator;
            Vector3 vector;
            intersection = Vector3.zero;

            //calculate the distance between the linePoint and the line-plane intersection point
            dotNumerator = Vector3.Dot((planePoint - linePoint), planeNormal);
            dotDenominator = Vector3.Dot(lineVec, planeNormal);

            //line and plane are not parallel
            if (dotDenominator != 0.0f)
            {
                length = dotNumerator / dotDenominator;

                //create a vector from the linePoint to the intersection point
                vector = SetVectorLength(lineVec, length);

                //get the coordinates of the line-plane intersection point
                intersection = linePoint + vector;

                return true;
            }

            //output not valid
            else
            {
                return false;
            }
        }

        public static int RoundToNearest(this int i, int nearest)
        {
            if (nearest <= 0 || nearest % 10 != 0)
                throw new System.ArgumentOutOfRangeException("nearest", "Must round to a positive multiple of 10");

            return (i + 5 * nearest / 10) / nearest * nearest;
        }

        public static double RoundToXLeadingDigits(double origValue, int leadingDigits, RoundingMode decimalDigitsRoundingMode)
        {
            if (System.Math.Abs(origValue) < 10)
            {
                return Round(origValue, decimalDigitsRoundingMode);
            }

            double helper = origValue;
            int origValueDigits = (int) System.Math.Floor(System.Math.Log10(System.Math.Abs(origValue)) + 1);
            for (int i = 0; i < origValueDigits - leadingDigits; ++i)
            {
                helper /= 10.0;
            }

            helper = System.Math.Round(helper, System.MidpointRounding.AwayFromZero);
            for (int i = 0; i < origValueDigits - leadingDigits; ++i)
            {
                helper *= 10.0;
            }

            return helper;
        }

        public static int CountDigits(double number) { return (int) System.Math.Floor(System.Math.Log10(System.Math.Abs(number)) + 1); }

        public static float Average(params float[] values)
        {
            float sum = 0f;
            foreach (var value in values)
            {
                sum += value;
            }

            return sum / values.Length;
        }

        public enum RoundingMode
        {
            Floor,
            Nearest,
            Ceil,
            None
        }


        public static float Round(this float value, RoundingMode roundingMode = RoundingMode.Nearest)
        {
            switch (roundingMode)
            {
                case RoundingMode.Floor:
                    return Mathf.Floor(value);
                case RoundingMode.Nearest:
                    return Mathf.Round(value);
                case RoundingMode.Ceil:
                    return Mathf.Ceil(value);
                case RoundingMode.None:
                    return value;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(roundingMode), roundingMode, null);
            }
        }

        public static float Round(this float value, int digits, RoundingMode roundingMode = RoundingMode.Nearest)
        {
            var digitsOffset = Mathf.Pow(10, digits);
            switch (roundingMode)
            {
                case RoundingMode.Floor:
                    return Mathf.Floor(value * digitsOffset) / digitsOffset;
                case RoundingMode.Nearest:
                    return Mathf.Round(value * digitsOffset) / digitsOffset;
                case RoundingMode.Ceil:
                    return Mathf.Ceil(value * digitsOffset) / digitsOffset;
                case RoundingMode.None:
                    return value * digitsOffset / digitsOffset;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(roundingMode), roundingMode, null);
            }
        }

        public static double Round(this double value, RoundingMode roundingMode = RoundingMode.Nearest)
        {
            switch (roundingMode)
            {
                case RoundingMode.Floor:
                    return System.Math.Floor(value);
                case RoundingMode.Nearest:
                    return System.Math.Round(value);
                case RoundingMode.Ceil:
                    return System.Math.Ceiling(value);
                case RoundingMode.None:
                    return value;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(roundingMode), roundingMode, null);
            }
        }

        public static double Round(this double value, int digits, RoundingMode roundingMode = RoundingMode.Nearest)
        {
            var digitsOffset = System.Math.Pow(10, digits);
            switch (roundingMode)
            {
                case RoundingMode.Floor:
                    return System.Math.Floor(value * digitsOffset) / digitsOffset;
                case RoundingMode.Nearest:
                    return System.Math.Round(value * digitsOffset) / digitsOffset;
                case RoundingMode.Ceil:
                    return System.Math.Ceiling(value * digitsOffset) / digitsOffset;
                case RoundingMode.None:
                    return value * digitsOffset / digitsOffset;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(roundingMode), roundingMode, null);
            }
        }

        public static int RoundToInt(this float value, RoundingMode roundingMode = RoundingMode.Nearest)
        {
            switch (roundingMode)
            {
                case RoundingMode.Floor:
                    return Mathf.FloorToInt(value);
                case RoundingMode.Nearest:
                    return Mathf.RoundToInt(value);
                case RoundingMode.Ceil:
                    return Mathf.CeilToInt(value);
                case RoundingMode.None:
                    return (int) value;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(roundingMode), roundingMode, null);
            }
        }

        public static int RoundToInt(this double value, RoundingMode roundingMode = RoundingMode.Nearest) { return System.Convert.ToInt32(Round(value, roundingMode)); }

        public static int MultiplyAndRound(this int value, float byValue, RoundingMode roundingMode = RoundingMode.Nearest) { return RoundToInt(value * byValue); }

        /// <summary>
        /// Filter out items and remove items from list with one RemoveRange call. Warning! does not maintain order
        /// </summary>
        /// <param name="list"></param>
        /// <param name="filterMethod">Return True to leave item in the list</param>
        /// <typeparam name="T"></typeparam>
        public static void BatchListFilter<T>(this List<T> list, System.Func<T, bool> filterMethod)
        {
            if (list != null && list.Count > 0)
            {
                int currentIndex = 0;
                int currentLength = list.Count;

                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[currentIndex];
                    if (filterMethod(item))
                    {
                        currentIndex++;
                    }
                    else
                    {
                        currentLength--;
                        list[currentIndex] = list[currentLength];
                    }
                }

                list.RemoveRange(currentIndex, list.Count - currentLength);
            }
        }


    #region Direct bit conversion
        [StructLayout(LayoutKind.Explicit)]
        internal struct IntFloatUnion
        {
            [FieldOffset(0)]
            public int intValue;
            [FieldOffset(0)]
            public float floatValue;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct LongDoubleUnion
        {
            [FieldOffset(0)]
            public long longValue;
            [FieldOffset(0)]
            public double doubleValue;
        }

        /// <summary>Returns the bit pattern of an int as a float.</summary>
        public static float AsFloat(int x)
        {
            IntFloatUnion u;
            u.floatValue = 0;
            u.intValue = x;

            return u.floatValue;
        }

        public static float AsFloat(uint x) { return AsFloat((int) x); }


        /// <summary>Returns the bit pattern of a long as a double.</summary>
        public static double AsDouble(long x)
        {
            LongDoubleUnion u;
            u.doubleValue = 0;
            u.longValue = x;
            return u.doubleValue;
        }

        /// <summary>Returns the bit pattern of a ulong as a double.</summary>
        public static double AsDouble(ulong x) { return AsDouble((long) x); }
    #endregion
    }
}
