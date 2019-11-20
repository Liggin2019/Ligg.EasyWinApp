using System;
using System.Runtime.InteropServices;
using Ligg.Winform.Resources;

namespace Ligg.Base.Helpers
{
    public static class SystemTimeHelper
    {
        [DllImport("Kernel32.dll")]
        public static extern bool SetSystemTime(ref TimeValue sysTime);

        public static Func<DateTime> UtcNow = () => DateTime.UtcNow;
        public static Func<DateTime> Now = () => DateTime.Now;

        public static string GetTimeSpanString(double milliSeconds,string accuracyFlag,bool isByCulture)
        {
            DateTime dt1 = DateTime.Now;
            DateTime dt2 = DateTime.Now.AddMilliseconds(milliSeconds);
            TimeSpan timeSpan = dt2 - dt1;   
            var timeSpanStr = " ";

            if ((string.IsNullOrEmpty(accuracyFlag)||accuracyFlag.ToLower() == "d" ||accuracyFlag.ToLower() == "h"||accuracyFlag.ToLower() == "m" || accuracyFlag.ToLower() == "s" || accuracyFlag.ToLower() == "ms")&&timeSpan.Days >0)
            {
                var timeFlag = isByCulture ? WinformRes.Days : " Days";
                timeSpanStr = timeSpan.Days  + timeFlag + ", ";
            }
            if ((string.IsNullOrEmpty(accuracyFlag) || accuracyFlag.ToLower() == "h" || accuracyFlag.ToLower() == "m" || accuracyFlag.ToLower() == "s" || accuracyFlag.ToLower() == "ms") && timeSpan.Hours >0)
            {
                var timeFlag = isByCulture ? WinformRes.Hours : " Hours";
                timeSpanStr = timeSpanStr + timeSpan.Hours + timeFlag + ", ";
            }
            if ((string.IsNullOrEmpty(accuracyFlag) || accuracyFlag.ToLower() == "m" || accuracyFlag.ToLower() == "s" || accuracyFlag.ToLower() == "ms") && timeSpan.Minutes > 0)
            {
                var timeFlag = isByCulture ? WinformRes.Minutes : " Mins";
                timeSpanStr = timeSpanStr + timeSpan.Minutes + timeFlag + ", ";
            }
            if ((string.IsNullOrEmpty(accuracyFlag) || accuracyFlag.ToLower() == "s" || accuracyFlag.ToLower() == "ms") && timeSpan.Seconds > 0)
            {
                var timeFlag = isByCulture ? WinformRes.Seconds : " Secs";
                timeSpanStr = timeSpanStr + timeSpan.Seconds + timeFlag + ", ";
            }
            if (string.IsNullOrEmpty(accuracyFlag) || accuracyFlag.ToLower() == "ms" && timeSpan.Milliseconds > 0)
            {
                var timeFlag = isByCulture ? WinformRes.MilliSeconds : " MilliSecs";
                timeSpanStr = timeSpanStr + timeSpan.Milliseconds + timeFlag + ", ";
            }

            if(timeSpanStr.EndsWith(", "))
            timeSpanStr = timeSpanStr.Substring(0,timeSpanStr.Length-2);
            return timeSpanStr;
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TimeValue
    {
        public ushort Year;
        public ushort Month;
        public ushort DayOfWeek;
        public ushort Day;
        public ushort Hour;
        public ushort Minute;
        public ushort Second;
        public ushort Millisecond;

    }
}