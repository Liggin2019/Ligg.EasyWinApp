using System;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
using Ligg.Winform.DataModel.Enums;

namespace Ligg.Winform.Helpers
{
    public static class TextVerificationHelper
    {
        public static bool Verify(string text, string verificationRule, string verificationParams)
        {
            try
            {
                var verificationType = (TextVerificationType)Enum.Parse(typeof(TextVerificationType), verificationRule);
                return Verify(text, verificationType, verificationParams);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static bool Verify(string text, TextVerificationType verificationType, string verificationParams)
        {
            try
            {
                var isOk = false;
                if (verificationType == TextVerificationType.ClearText | verificationType == TextVerificationType.ClearPassword)
                {
                    var clearCode = verificationParams;
                    if (clearCode == text) isOk = true;
                    return isOk;
                }
                else if (verificationType == TextVerificationType.TdeText | verificationType == TextVerificationType.TdePassword)
                {
                    var verParams = verificationParams.Split(verificationParams.GetSubParamSeparator());
                    var encryptedCode = text;
                    var clearCode = verParams[0];

                    var spanMin = Convert.ToInt16(verParams[1]);

                    var isLocalTime = false;
                    if (verParams.Length > 2)
                        isLocalTime = verParams[2].ToLower() == "true" ? true : false;

                    //isOk = EncryptionHelper.VerifyTimeDynamicPassword(encryptedCode, clearCode, spanMin, isLocalTime);
                    return isOk;
                }

                else if ((verificationType == TextVerificationType.RegularExpression))
                {
                    //to be updated
                    return false;
                }
                else if ((verificationType == TextVerificationType.None))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}