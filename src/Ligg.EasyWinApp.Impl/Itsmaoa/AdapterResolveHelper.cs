using System;
using System.Text.RegularExpressions;

namespace Ligg.EasyWinApp.Implementation
{
    internal static partial class AdapterResolveHelper
    {
        private static readonly string
            TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        internal static string ResolveConstants(string text)
        {
            var returnStr = "";
            try
            {
                if (!text.Contains("%")) return text;
                var toBeRplStr = "";
                toBeRplStr = "%TestConstant%".ToLower();
                if (text.ToLower().Contains(toBeRplStr))
                {
                    var rplStr = DateTime.Now.ToString("It is a test constant");
                    text = Regex.Replace(text, toBeRplStr, rplStr, RegexOptions.IgnoreCase);
                }
                if (!text.Contains("%"))
                {
                    return text;
                }
                else
                {
                    throw new ArgumentException("Text '" + text + "' can't be resolved!");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".ResolveConstants error: " + ex.Message);
            }
        }


    }
}

