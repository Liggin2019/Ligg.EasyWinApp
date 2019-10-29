using System;
using Ligg.Base.Extension;
using Ligg.Winform.Resources;

namespace Ligg.Winform.Helpers
{
    public static class TextValidationHelper
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
        public static string Validate(string text, string validationRule)
        {
            try
            {
                var subParamSeparator = validationRule.GetSubParamSeparator();
                var ruleArray = validationRule.Split(subParamSeparator);
                if (ruleArray[0] == "MandatoryField")
                {
                    if (!text.IsNullOrEmpty())
                    {
                        return "true";
                    }
                    else
                    {
                        return ValidationRes.MandatoryField;
                    }
                }
                if (ruleArray[0] == "StringMinLength")
                {
                    var minLen = Convert.ToInt16(ruleArray[1]);
                    if (text.Length > minLen - 1)
                    {
                        return "true";
                    }
                    else
                    {
                        return string.Format(ValidationRes.StringMinLength, minLen);
                    }
                }
                if (ruleArray[0] == "WebUrlFormat")
                {
                    if (text.IsWebUrl())
                    {
                        return "true";
                    }
                    else
                    {
                        return string.Format(ValidationRes.WebUrlFormat);
                    }
                }
                if (ruleArray[0] == "EmailFormat")
                {
                    if (text.IsEmailAddress())
                    {
                        return "true";
                    }
                    else
                    {
                        return string.Format(ValidationRes.EmailFormat);
                    }
                }
                if (ruleArray[0] == "PasswordFormat")
                {
                    if (text.IsPassword())
                    {
                        return "true";
                    }
                    else
                    {
                        return string.Format(ValidationRes.PasswordFormat);
                    }
                }


                return "OutOfScopeOfTextValidationHelper";
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Validate error: " + ex.Message);
            }
        }



    }
}