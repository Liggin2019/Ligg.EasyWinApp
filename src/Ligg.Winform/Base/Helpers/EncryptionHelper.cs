using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Ligg.Base.Extension;

namespace Ligg.Base.Helpers
{
    public static class EncryptionHelper
    {
        public static string Key1 = _selfKey1;
        public static string Key2 = _selfKey2;
        private static string _selfKey1 = "mOtXb01/2Mp8kIOYD/hbAg==";
        private static string _selfKey2 = "wEdL50/eAJFSx+0thR2hhg==";
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;



    }
}

