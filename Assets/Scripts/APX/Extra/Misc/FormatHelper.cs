using System.Globalization;
using System.Text;

namespace APX.Extra.Misc
{
    public static class FormatHelper
    {
        public static string FormatLargeNumber(float value)
        {
            var stringBuilder = StringHelper.StringBuilder;
            // clear the buffer
            stringBuilder.Length = 0;

            stringBuilder.Append(value);
            if (stringBuilder.Length > 3)
            {
                var currentPos = stringBuilder.Length % 3;
                if (currentPos == 0) currentPos += 3;
                while (currentPos < stringBuilder.Length)
                {
                    if (char.IsDigit(stringBuilder[currentPos - 1]))
                    {
                        stringBuilder.Insert(currentPos, ',');
                        currentPos += 4;
                    }
                    else
                    {
                        currentPos += 3;
                    }
                }
            }

            return stringBuilder.ToString();
        }

        public static string FormatLargeNumber(long value)
        {
            var stringBuilder = StringHelper.StringBuilder;
            // clear the buffer
            stringBuilder.Length = 0;

            stringBuilder.Append(value);
            if (stringBuilder.Length > 3)
            {
                var currentPos = stringBuilder.Length % 3;
                if (currentPos == 0) currentPos += 3;
                while (currentPos < stringBuilder.Length)
                {
                    if (char.IsDigit(stringBuilder[currentPos - 1]))
                    {
                        stringBuilder.Insert(currentPos, ',');
                        currentPos += 4;
                    }
                    else
                    {
                        currentPos += 3;
                    }
                }
            }

            return stringBuilder.ToString();
        }

        public static string FormatLargeNumber(int value, int forcedMinDecimalDigits = 0, bool useThousandsSeparatorComma = true)
            => FormatLargeNumber(value.ToString(CultureInfo.InvariantCulture), forcedMinDecimalDigits, useThousandsSeparatorComma);

        public static string FormatLargeNumber(float value, int forcedMinDecimalDigits = 0, bool useThousandsSeparatorComma = true)
            => FormatLargeNumber(value.ToString(CultureInfo.InvariantCulture), forcedMinDecimalDigits, useThousandsSeparatorComma);

        public static string FormatLargeNumber(double value, int forcedMinDecimalDigits = 0, bool useThousandsSeparatorComma = true)
            => FormatLargeNumber(value.ToString(CultureInfo.InvariantCulture), forcedMinDecimalDigits, useThousandsSeparatorComma);

        private static string FormatLargeNumber(string asString, int forcedMinDecimalDigits = 0, bool useThousandsSeparatorComma = true)
        {
            return FormatNumber(asString, forcedMinDecimalDigits, useThousandsSeparatorComma ? "," : null);
        }

        public static string FormatNumber(int value, int forcedMinDecimalDigits = 0, string thousandsSeparator = null)
            => FormatNumber(value.ToString(CultureInfo.InvariantCulture), forcedMinDecimalDigits, thousandsSeparator);

        public static string FormatNumber(float value, int forcedMinDecimalDigits = 0, string thousandsSeparator = null)
            => FormatNumber(value.ToString(CultureInfo.InvariantCulture), forcedMinDecimalDigits, thousandsSeparator);

        public static string FormatNumber(double value, int forcedMinDecimalDigits = 0, string thousandsSeparator = null)
            => FormatNumber(value.ToString(CultureInfo.InvariantCulture), forcedMinDecimalDigits, thousandsSeparator);

        public static string FormatNumber(string asString, int forcedMinDecimalDigits = 0, string thousandsSeparator = null)
        {
            var stringBuilder = StringHelper.StringBuilder;
            // clear the buffer
            stringBuilder.Length = 0;
            stringBuilder.Append(asString);
            return FormatNumber(stringBuilder, forcedMinDecimalDigits, thousandsSeparator).ToString();
        }

        public static StringBuilder FormatNumber(StringBuilder stringBuilder, int forcedMinDecimalDigits = 0, string thousandsSeparator = null)
        {
            var split = stringBuilder.ToString().Split('.');
            var nonDecimalDigits = split[0].Length;
            var currentDecimalDigits = split.Length > 1 ? split[1].Length : 0;

            if (forcedMinDecimalDigits > 0)
            {
                if (currentDecimalDigits < 1)
                    stringBuilder.Append('.');

                while (currentDecimalDigits < forcedMinDecimalDigits)
                {
                    stringBuilder.Append('0');
                    currentDecimalDigits++;
                }
            }

            if (nonDecimalDigits > 3 && !string.IsNullOrEmpty(thousandsSeparator))
            {
                var separatorLength = thousandsSeparator.Length;
                var currentPos = nonDecimalDigits % 3;
                if (currentPos == 0) currentPos += 3;
                while (currentPos < nonDecimalDigits)
                {
                    if (char.IsDigit(stringBuilder[currentPos - 1]))
                    {
                        stringBuilder.Insert(currentPos, thousandsSeparator);
                        currentPos += 3 + separatorLength;
                    }
                    else
                    {
                        currentPos += 3;
                    }
                }
            }

            return stringBuilder;
        }
    }
}
