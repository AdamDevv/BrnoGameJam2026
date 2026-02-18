using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace APX.Extra.Misc
{
    public static class StringHelper
    {
        public static readonly StringBuilder StringBuilder = new StringBuilder(50);

        public static void ResetStringBuilder() { StringBuilder.Length = 0; }

        /// <summary>
        /// Returns alphanumeric unique ID (almost! - !100%). Maximum length 22!
        /// Example: gV1gBUvsX0ujulB
        /// </summary>
        /// <param name="length">Length of the string (maximum 22!)</param>
        public static string GetGloballyUniqueID(int length = 15)
        {
            if (length > 22) throw new ArgumentOutOfRangeException("Max Guid length is 22! Entered: " + length);

            var g = Guid.NewGuid();
            var stringBuilder = new StringBuilder(25);

            do
            {
                stringBuilder.Length = 0;
                stringBuilder.Append(Convert.ToBase64String(g.ToByteArray()));
                stringBuilder.Replace("=", string.Empty);
                stringBuilder.Replace("+", string.Empty);
                stringBuilder.Replace("/", string.Empty);
            } while (stringBuilder.Length < length);

            return stringBuilder.ToString(0, length);
        }

        /// <summary>
        /// Adds specified chars in front of the string so that it has the desired length.
        /// This method version fills stringBuilder
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <param name="length"></param>
        /// <param name="stringBuilder"></param>
        public static void AddLeadingChars(string str, char c, int length, StringBuilder stringBuilder)
        {
            var currentLength = str.Length;
            if (currentLength >= length)
            {
                stringBuilder.Append(str);
                return;
            }

            while (currentLength < length)
            {
                stringBuilder.Append(c);
                currentLength++;
            }

            stringBuilder.Append(str);
        }

        /// <summary>
        /// Adds specified chars in front of the string so that it has the desired length
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string AddLeadingChars(string str, char c, int length)
        {
            var stringBuilder = StringBuilder;
            stringBuilder.Clear();

            AddLeadingChars(str, c, length, stringBuilder);

            return stringBuilder.ToString();
        }

        public static string GetOrdinalSuffix(int num)
        {
            if (num <= 0) return string.Empty;

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return "th";
            }

            switch (num % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }

        public class AlphanumComparatorFast : IComparer
        {
            public int Compare(object x, object y)
            {
                var s1 = x as string;
                if (s1 == null)
                {
                    return 0;
                }

                var s2 = y as string;
                if (s2 == null)
                {
                    return 0;
                }

                var len1 = s1.Length;
                var len2 = s2.Length;
                var marker1 = 0;
                var marker2 = 0;

                // Walk through two the strings with two markers.
                while (marker1 < len1 && marker2 < len2)
                {
                    var ch1 = s1[marker1];
                    var ch2 = s2[marker2];

                    // Some buffers we can build up characters in for each chunk.
                    var space1 = new char[len1];
                    var loc1 = 0;
                    var space2 = new char[len2];
                    var loc2 = 0;

                    // Walk through all following characters that are digits or
                    // characters in BOTH strings starting at the appropriate marker.
                    // Collect char arrays.
                    do
                    {
                        space1[loc1++] = ch1;
                        marker1++;

                        if (marker1 < len1)
                        {
                            ch1 = s1[marker1];
                        }
                        else
                        {
                            break;
                        }
                    } while (char.IsDigit(ch1) == char.IsDigit(space1[0]));

                    do
                    {
                        space2[loc2++] = ch2;
                        marker2++;

                        if (marker2 < len2)
                        {
                            ch2 = s2[marker2];
                        }
                        else
                        {
                            break;
                        }
                    } while (char.IsDigit(ch2) == char.IsDigit(space2[0]));

                    // If we have collected numbers, compare them numerically.
                    // Otherwise, if we have strings, compare them alphabetically.
                    var str1 = new string(space1);
                    var str2 = new string(space2);

                    int result;

                    if (char.IsDigit(space1[0]) && char.IsDigit(space2[0]))
                    {
                        var thisNumericChunk = int.Parse(str1);
                        var thatNumericChunk = int.Parse(str2);
                        result = thisNumericChunk.CompareTo(thatNumericChunk);
                    }
                    else
                    {
                        result = str1.CompareTo(str2);
                    }

                    if (result != 0)
                    {
                        return result;
                    }
                }

                return len1 - len2;
            }
        }

        public abstract class AlphanumComparatorFast<T> : AlphanumComparatorFast, IComparer<T>
        {
            protected abstract string GetSortedString(T item);
            public virtual int Compare(T x, T y) { return Compare(GetSortedString(x), GetSortedString(y)); }
        }
    }
}
