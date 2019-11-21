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

        private static readonly string
            TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        //#symmetric
        public static string SmEncrypt(string source)
        {
            return SmEncrypt(source, Key1, Key2);
        }

        public static string SmEncrypt(string source, string key1, string key2)
        {
            try
            {
                byte[] bytIn = Encoding.UTF8.GetBytes(source);
                var ms = new MemoryStream();
                MobjCryptoService.Key = GetLegalKey(key1);
                MobjCryptoService.IV = GetLegalIv(key2);
                ICryptoTransform encrypto = MobjCryptoService.CreateEncryptor();
                var cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                ms.Close();
                var bytOut = ms.ToArray();
                return Convert.ToBase64String(bytOut);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".SmEncrypt Error: " + ex.Message);
            }
        }

     

        private static readonly SymmetricAlgorithm MobjCryptoService = new RijndaelManaged();


        private static byte[] GetLegalKey(string key)
        {
            string tempStr = key;
            MobjCryptoService.GenerateKey();
            byte[] bytTemp = MobjCryptoService.Key;
            int keyLength = bytTemp.Length;
            if (tempStr.Length > keyLength)
                tempStr = tempStr.Substring(0, keyLength);
            else if (tempStr.Length < keyLength)
                tempStr = tempStr.PadRight(keyLength, ' ');
            return Encoding.ASCII.GetBytes(tempStr);
        }


        private static byte[] GetLegalIv(string key)
        {
            string tempStr = key;
            MobjCryptoService.GenerateIV();
            byte[] bytTemp = MobjCryptoService.IV;
            int ivLength = bytTemp.Length;
            if (tempStr.Length > ivLength)
                tempStr = tempStr.Substring(0, ivLength);
            else if (tempStr.Length < ivLength)
                tempStr = tempStr.PadRight(ivLength, ' ');
            return Encoding.ASCII.GetBytes(tempStr);
        }



    }
}

