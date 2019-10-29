using System;

namespace Ligg.EasyWinApp.ImplInterface
{
    public static class GlobalConfiguration
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
        //#set from init() 
        public static string AssemblyCode = "";

        public static string OrganizationCode = "";
        public static string UserCode = "";
        public static string UserToken = "";
        public static string GlobalKey1 = "";
        public static string GlobalKey2 = "";
        public static string LicensedAppCodes = "";

        //#set from front end
        public static string AppCode = "";
        public static string ImplementationDir = "";
        public static bool SupportMutiCultures;
        public static string DefaultLanguageCode = "";
        public static string CurrentLanguageCode = "";
        public static string[] StartParams;


        public static void init()
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

        public static void SetStartParams(string startParams)
        {
            try
            {
                StartParams = startParams.Split(GetParamStringSeparator(startParams));
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".SetStartParams Error: " + ex.Message);
            }


        }

        public static char GetParamStringSeparator(string text)
        {
            var separator = ';';
            if (text.Contains("^")) separator = '^';
            else if (text.Contains("~")) separator = '~';
            return separator;
        }
        public static char GetSubParamSeparator(string text)
        {
            try
            {
                var separator = ',';
                if (text.Contains("`")) separator = '`';
                return separator;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".GetSubParamSeparator Error: " + ex.Message);
            }
        }
    }

}