using System;

namespace Ligg.EasyWinApp.ImplInterface
{
    public static class GlobalConfiguration
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
        //#set from ReadParams() 
        public static string AssemblyCode = "";
        public static string OrganizationCode = "";
        public static string GlobalKey1 = "";
        public static string GlobalKey2 = "";

        //#set from front end
        public static string AppCode = "";
        public static string ImplementationDir = "";
        public static bool SupportMutiCultures;
        public static string DefaultLanguageCode = "";
        public static string CurrentLanguageCode = "";
        public static string[] StartParams;

        public static string UserCode = "";
        public static string UserToken = "";
        public static string LicensedAppCodes = "";

        public static void ReadParams()
        {
            try
            {
                //Get from License certificate/dog
                AssemblyCode = "Ligg";
                OrganizationCode = "LgTech";
                GlobalKey1 = "GlobalEncrptKey1";
                GlobalKey2 = "GlobalEncrptKey2";
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".init Error: " + ex.Message);
            }
        }

        public static bool VerifyUserToken(string userCode, string userToken)
        {
            try
            {
                UserCode = userCode;
                UserToken = userToken;
                return true;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".VerifyUserToken Error: " + ex.Message);
            }


        }


    }
}