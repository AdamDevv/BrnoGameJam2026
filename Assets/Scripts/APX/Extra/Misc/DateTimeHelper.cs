using System.Text;

namespace APX.Extra.Misc
{
    public static class DateTimeHelper
    {
        private static StringBuilder _stringBuilder;
        private static StringBuilder StringBuilder
        {
            get
            {
                if (_stringBuilder == null) _stringBuilder = new StringBuilder(20);
                return _stringBuilder;
            }
        }

        public static string TimeSpanToString(System.TimeSpan timeSpan, string divider = null)
        {
            return $"{timeSpan.Days:D2}d{divider}{timeSpan.Hours:D2}h{divider}{timeSpan.Minutes:D2}m{divider}{timeSpan.Seconds:D2}s";
        }

        public static string TimeToString(float seconds, string divider = null) { return TimeSpanToString(System.TimeSpan.FromSeconds(seconds), divider); }
        public static string TicksToString(long ticks, string divider = null) { return TimeSpanToString(System.TimeSpan.FromTicks(ticks), divider); }

        /// <summary>
        /// Shows Days, Hours, Minutes and Seconds only if the value for it is >0
        /// </summary>
        /// <param name="timeSpan">Time span</param>
        /// <param name="divider">Optional divider between values</param>
        /// <returns></returns>
        public static string TimeSpanToStringRelevantValues(System.TimeSpan timeSpan, string divider = null)
        {
            StringBuilder.Clear();
            bool hasPrevValue = false;
            if (divider == string.Empty) divider = null;

            if (timeSpan.Days > 0)
            {
                StringBuilder.Append($"{timeSpan.Days:D1}d");
                hasPrevValue = true;
            }

            if (timeSpan.Hours > 0)
            {
                if (divider != null && hasPrevValue) StringBuilder.Append(divider);
                StringBuilder.Append($"{timeSpan.Hours:D1}h");
                hasPrevValue = true;
            }

            if (timeSpan.Minutes > 0)
            {
                if (divider != null && hasPrevValue) StringBuilder.Append(divider);
                StringBuilder.Append($"{timeSpan.Minutes:D1}m");
                hasPrevValue = true;
            }

            if (timeSpan.Seconds > 0)
            {
                if (divider != null && hasPrevValue) StringBuilder.Append(divider);
                StringBuilder.Append($"{timeSpan.Seconds}s");
            }

            return StringBuilder.ToString();
        }

        /// <summary>
        /// Shows Days, Hours, Minutes and Seconds only if the value for it is >0
        /// </summary>
        /// <param name="time">Time in seconds</param>
        /// <param name="divider">Optional divider between values</param>
        /// <returns></returns>
        public static string TimeToStringRelevantValues(double time, string divider = null) { return TimeSpanToStringRelevantValues(System.TimeSpan.FromSeconds(time), divider); }


        /// <summary>
        /// Shows Days, Hours, Minutes and Seconds only if the value for it is >0
        /// </summary>
        /// <param name="ticks">Ticks</param>
        /// <param name="divider">Optional divider between values</param>
        /// <returns></returns>
        public static string TicksToStringRelevantValues(long ticks, string divider = null) { return TimeSpanToStringRelevantValues(System.TimeSpan.FromTicks(ticks), divider); }

        /// <summary>
        /// Shows only first two values (Days, Hours, Minutes or Seconds) that are relevant (value for it is >0) 
        /// </summary>
        /// <param name="timeSpan">Time span</param>
        /// <param name="divider">Optional divider between values</param>
        /// <returns>String in format {firstValue:D1}X{divider}{secondValue:D2}Y</returns>
        public static string TimeSpanToStringHighest2(System.TimeSpan timeSpan, string divider = null)
        {
            if (timeSpan.Days > 0) return $"{timeSpan.Days:D1}d{divider}{timeSpan.Hours:D2}h";
            if (timeSpan.Hours > 0) return $"{timeSpan.Hours:D1}h{divider}{timeSpan.Minutes:D2}m";
            if (timeSpan.Minutes > 0) return $"{timeSpan.Minutes:D1}m{divider}{timeSpan.Seconds:D2}s";
            return $"{timeSpan.Seconds}s";
        }

        /// <summary>
        /// Shows only first two values (Days, Hours, Minutes or Seconds) that are relevant (value for it is >0) 
        /// </summary>
        /// <param name="time">Time in seconds</param>
        /// <param name="divider">Optional divider between values</param>
        /// <returns>String in format {firstValue:D1}X{divider}{secondValue:D2}Y</returns>
        public static string TimeToStringHighest2(double time, string divider = null) { return TimeSpanToStringHighest2(System.TimeSpan.FromSeconds(time), divider); }

        /// <summary>
        /// Shows only first two values (Days, Hours, Minutes or Seconds) that are relevant (value for it is >0) 
        /// </summary>
        /// <param name="ticks">Ticks</param>
        /// <param name="divider">Optional divider between values</param>
        /// <returns>String in format {firstValue:D1}X{divider}{secondValue:D2}Y</returns>
        public static string TicksToStringHighest2(long ticks, string divider = null) { return TimeSpanToStringHighest2(System.TimeSpan.FromTicks(ticks), divider); }

        /// <summary>
        /// Shows only first two values (Days, Hours, Minutes or Seconds) that are relevant (value for it is >0) 
        /// </summary>
        /// <param name="time">Time in seconds</param>
        /// <param name="divider">Optional divider between values</param>
        /// <returns>String in format {firstValue:D1}X{divider}{secondValue:D2}Y</returns>
        public static string TimeToStringHighest2(float time, string divider = null) { return TimeToStringHighest2((double) time, divider); }


        /// <summary>
        /// Shows only first two values (Days, Hours, Minutes or Seconds) that are relevant (value for it is >0) 
        /// </summary>
        /// <param name="timeSpan">Time span</param>
        /// <param name="divider">Optional divider between values</param>
        /// <returns>String in format {firstValue:D1}X{divider}{secondValue:D2}Y</returns>
        public static string TimeSpanToStringHighestOne(System.TimeSpan timeSpan, string divider = null)
        {
            if (timeSpan.Days > 0) return $"{timeSpan.Days:D1}d";
            if (timeSpan.Hours > 0) return $"{timeSpan.Hours:D1}h";
            if (timeSpan.Minutes > 0) return $"{timeSpan.Minutes:D1}m";
            return $"{timeSpan.Seconds}s";
        }

        public static string ToFormattedString(this System.TimeSpan timeSpan, TimeDisplayFormat format, string divider = null) => format switch
        {
            TimeDisplayFormat.Full => TimeSpanToString(timeSpan, divider),
            TimeDisplayFormat.RelevantValues => TimeSpanToStringRelevantValues(timeSpan, divider),
            TimeDisplayFormat.HighestTwo => TimeSpanToStringHighest2(timeSpan, divider),
            TimeDisplayFormat.HighestOne => TimeSpanToStringHighestOne(timeSpan, divider),
            TimeDisplayFormat.Digital => TimeSpanToIntervalDigital(timeSpan),
            _ => throw new System.ArgumentOutOfRangeException()
        };

        /// <summary>
        /// Shows only first two values (Days, Hours, Minutes or Seconds) that are relevant (value for it is >0) 
        /// </summary>
        /// <param name="time">Time in seconds</param>
        /// <param name="divider">Optional divider between values</param>
        /// <returns>String in format {firstValue:D1}X{divider}{secondValue:D2}Y</returns>
        public static string TimeToStringHighestOne(double time, string divider = null) { return TimeSpanToStringHighestOne(System.TimeSpan.FromSeconds(time), divider); }

        /// <summary>
        /// Shows only first two values (Days, Hours, Minutes or Seconds) that are relevant (value for it is >0) 
        /// </summary>
        /// <param name="ticks">Ticks</param>
        /// <param name="divider">Optional divider between values</param>
        /// <returns>String in format {firstValue:D1}X{divider}{secondValue:D2}Y</returns>
        public static string TicksToStringHighestOne(long ticks, string divider = null) { return TimeSpanToStringHighestOne(System.TimeSpan.FromTicks(ticks), divider); }

        /// <summary>
        /// Shows only first two values (Days, Hours, Minutes or Seconds) that are relevant (value for it is >0) 
        /// </summary>
        /// <param name="time">Time in seconds</param>
        /// <param name="divider">Optional divider between values</param>
        /// <returns>String in format {firstValue:D1}X{divider}{secondValue:D2}Y</returns>
        public static string TimeToStringHighestOne(float time, string divider = null) { return TimeToStringHighestOne((double) time, divider); }


        /// <summary>
        /// Shows Days, Hours, Minutes and Seconds with specified divider
        /// Each shown value has two displayed digits
        /// Always shows Minutes and Seconds
        /// </summary>
        /// <param name="timeSpan">Time span</param>
        /// <param name="divider">Divider between values</param>
        /// <returns>Example 00:05 or 01:12:25:08</returns>
        public static string TimeSpanToIntervalDigital(System.TimeSpan timeSpan, string divider = ":")
        {
            if (timeSpan.TotalSeconds <= 0)
            {
                return "00:00";
            }

            StringBuilder.Clear();
            bool hasPrevValue = false;
            if (divider == string.Empty) divider = null;

            if (timeSpan.Days > 0)
            {
                StringBuilder.Append($"{timeSpan.Days:D2}");
                hasPrevValue = true;
            }

            if (timeSpan.Hours > 0)
            {
                if (divider != null && hasPrevValue) StringBuilder.Append(divider);
                StringBuilder.Append($"{timeSpan.Hours:D2}");
                hasPrevValue = true;
            }

            if (divider != null && hasPrevValue) StringBuilder.Append(divider);
            StringBuilder.Append($"{timeSpan.Minutes:D2}");

            if (divider != null) StringBuilder.Append(divider);
            StringBuilder.Append($"{timeSpan.Seconds:D2}");

            return StringBuilder.ToString();
        }

        /// <summary>
        /// Shows Days, Hours, Minutes and Seconds with specified divider
        /// Each shown value has two displayed digits
        /// Always shows Minutes and Seconds
        /// </summary>
        /// <param name="time">Time in seconds</param>
        /// <param name="divider">Divider between values</param>
        /// <returns>Example 00:05 or 01:12:25:08</returns>
        public static string TimeToIntervalDigital(double time, string divider = ":") { return TimeSpanToIntervalDigital(System.TimeSpan.FromSeconds(time), divider); }

        /// <summary>
        /// Shows Days, Hours, Minutes and Seconds with specified divider
        /// Each shown value has two displayed digits
        /// Always shows Minutes and Seconds
        /// </summary>
        /// <param name="ticks">Ticks</param>
        /// <param name="divider">Divider between values</param>
        /// <returns>Example 00:05 or 01:12:25:08</returns>
        public static string TimeToIntervalDigital(long ticks, string divider = ":") { return TimeSpanToIntervalDigital(System.TimeSpan.FromTicks(ticks), divider); }

        /// <summary>
        /// Shows Days, Hours, Minutes and Seconds with specified divider
        /// Each shown value has two displayed digits
        /// Always shows Minutes and Seconds
        /// </summary>
        /// <param name="time">Time in seconds</param>
        /// <param name="divider">Divider between values</param>
        /// <returns>Example 00:05 or 01:12:25:08</returns>
        public static string TimeToIntervalDigital(float time, string divider = ":") { return TimeSpanToIntervalDigital(System.TimeSpan.FromSeconds(time), divider); }

    #region Epoch
        public static readonly System.DateTime EpochDateTime = new System.DateTime(1970, 1, 1);

        public static System.TimeSpan ToEpochTime(this System.DateTime dateTime)
        {
            return dateTime - EpochDateTime;
        }

        public static long ToEpochTimeMilliseconds(this System.DateTime dateTime)
        {
            return (long) dateTime.ToEpochTime().TotalMilliseconds;
        }
        
        public static long ToEpochTimeInSeconds(this System.DateTime dateTime)
        {
            System.TimeSpan timeSpan = dateTime - EpochDateTime;
            return (long) timeSpan.TotalSeconds;
        }
        
        public static string FormatTimeIntervalDigital(float secs)
        {
            if (secs < 0)
            {
                return "00:00";
            }

            int hours = (int) secs / 3600;
            int minutes = (int) (secs / 60) - (hours * 60);
            int seconds = (int) (secs - ((hours * 3600) + (minutes * 60)));

            string retH = hours.ToString();
            string retM = minutes.ToString();
            string retS = seconds.ToString();

            if (hours < 10)
            {
                retH = "0" + retH;
            }

            if (minutes < 10)
            {
                retM = "0" + retM;
            }

            if (seconds < 10)
            {
                retS = "0" + retS;
            }

            if (hours == 0)
            {
                return retM + ":" + retS;
            }

            return retH + ":" + retM + ":" + retS;
        }

        public static System.DateTime FromEpochTime(long epochTime)
        {
            System.TimeSpan timeSpan = System.TimeSpan.FromMilliseconds(epochTime);
            return EpochDateTime + timeSpan;
        }

        public static System.TimeSpan TimeSpanSinceEpoch(this System.DateTime dateTime)
        {
            return dateTime - EpochDateTime;
        }
#endregion
    }
}
