using System;
using System.Diagnostics;
using Ligg.Base.Helpers;

namespace Ligg.Base.Extension
{
    public static class DateTimeExtension
    {
        private static readonly DateTime MinDate = new DateTime(1900, 1, 1);
        private static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);


        public static bool IsValid(this DateTime target)
        {
            return (target >= MinDate) && (target <= MaxDate);
        }

        public static bool IsPastTime(this DateTime target)
        {
            return (target < SystemTimeHelper.UtcNow());
        }

        public static bool IsFutureTime(this DateTime target)
        {
            return (target > SystemTimeHelper.UtcNow());
        }

        public static DateTime ToUtcTime(this DateTime target)
        {
            return (target.ToUniversalTime());
        }

        public static DateTime ToLocalTime(this DateTime target)
        {
            return (target.ToLocalTime());
        }

    }
}