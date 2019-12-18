using System;
using System.Text.RegularExpressions;
using Ligg.Base.Extension;

namespace Ligg.EasyWinApp.Implementation
{
    internal static partial class AdapterResolveHelper
    {
        private static readonly string
            TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        internal static string ResolveConstants(string text)
        {
            try
            {
                if (text.IsNullOrEmpty()) return string.Empty;
                if (!text.Contains("%")) return text;

                var toBeRplStr = "%MdlDir%".ToLower();
                if (text.ToLower().Contains(toBeRplStr))
                {
                    var rplStr = RunningParams.CurrentNetworkLocation.MediaLibLocation;
                    text = Regex.Replace(text, toBeRplStr, rplStr, RegexOptions.IgnoreCase);
                }

                if (text.Contains("%")) throw new ArgumentException("'" + text + "' can't be resolved ");
                return text;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".ResolveConstants error: " + ex.Message);
            }
        }


    }
}

