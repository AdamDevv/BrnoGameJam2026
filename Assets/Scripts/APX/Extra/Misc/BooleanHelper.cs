using System.Text.RegularExpressions;

namespace APX.Extra.Misc
{
    public static class BooleanHelper
    {
        internal static readonly Regex BOOLEAN_TRUE_PATTERN = new Regex("^(1|true|t|yes|y|on)$", RegexOptions.IgnoreCase);
        internal static readonly Regex BOOLEAN_FALSE_PATTERN = new Regex("^(0|false|f|no|n|off|)$", RegexOptions.IgnoreCase);

        public static bool ToBoolean(this string str)
        {
            if (BOOLEAN_TRUE_PATTERN.IsMatch(str))
                return true;
            if (BOOLEAN_FALSE_PATTERN.IsMatch(str))
                return false;
            throw new System.FormatException($"ConfigValue '{str}' is not a boolean value");
        }

        public static bool TryParseBoolean(this string str, out bool result)
        {
            if (BOOLEAN_TRUE_PATTERN.IsMatch(str))
            {
                result = true;
                return true;
            }

            if (BOOLEAN_FALSE_PATTERN.IsMatch(str))
            {
                result = false;
                return true;
            }

            result = default;
            return false;
        }
    }
}
