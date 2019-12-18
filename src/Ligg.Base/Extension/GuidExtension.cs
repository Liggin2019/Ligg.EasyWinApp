using System;
using System.Diagnostics;

namespace Ligg.Base.Extension
{
    public static class GuidExtension
    {
        [DebuggerStepThrough]
        public static string ToBase64String(this Guid target)
        {
            if (target.IsNullOrEmpty()) throw new ArgumentException("\"{0}\" cannot be empty guid.".FormatWith("target"));

            string base64 = Convert.ToBase64String(target.ToByteArray());

            string encoded = base64.Replace("/", "_").Replace("+", "-");

            return encoded.Substring(0, 22);
        }



        [DebuggerStepThrough]
        public static bool IsNullOrEmpty(this Guid target)
        {
            return target == Guid.Empty;
        }
    }
}