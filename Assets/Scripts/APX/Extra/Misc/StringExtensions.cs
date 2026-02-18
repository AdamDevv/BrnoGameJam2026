using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace APX.Extra.Misc
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            if (source == null) return false;
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static string Nicify(this string original)
        {
            var camel = Regex.Replace(original, "(\\B[A-Z])", " $1");
            var snake = camel.Replace("_", " ");
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(snake);
        }

        public static string Truncate(this string value, int maxLength, bool addDotsToEnd = false)
        {
            if (string.IsNullOrEmpty(value)) return value;
            if (value.Length <= maxLength)
                return value;

            var truncated = value.Substring(0, maxLength);
            return addDotsToEnd ? $"{truncated}..." : truncated;
        }

        public static string EnsureConstantLength(this string str, int constantLength, char? paddingCharacter = null, string truncationIndicator = "~")
        {
            str ??= string.Empty;

            if (str.Length > constantLength)
            {
                var indicatorLength = string.IsNullOrEmpty(truncationIndicator) ? 0 : truncationIndicator.Length;
                str = $"{str.Substring(0, constantLength - indicatorLength)}{truncationIndicator}";
            }
            else if (str.Length < constantLength)
            {
                str = paddingCharacter.HasValue ? str.PadRight(constantLength, paddingCharacter.Value) : str.PadRight(constantLength);
            }

            return str;
        }

        public static bool IsNullEmptyOrWhitespace(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                for (var index = 0; index < str.Length; ++index)
                {
                    if (!char.IsWhiteSpace(str[index]))
                        return false;
                }
            }

            return true;
        }
    }
}
