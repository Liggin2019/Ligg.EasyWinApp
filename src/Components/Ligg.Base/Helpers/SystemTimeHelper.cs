using System;
using System.Runtime.InteropServices;
using Ligg.Base.Resources;

namespace Ligg.Base.Helpers
{
    public static class SystemTimeHelper
    {
        [DllImport("Kernel32.dll")]
        public static extern bool SetSystemTime(ref TimeValue sysTime);

        public static Func<DateTime> UtcNow = () => DateTime.UtcNow;
        public static Func<DateTime> Now = () => DateTime.Now;
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