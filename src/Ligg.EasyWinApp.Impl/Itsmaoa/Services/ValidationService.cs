using System;
using System.Collections.Generic;
using Ligg.Base.DataModel;
using Ligg.Base.DataModel.Enums;
using Ligg.Base.Extension;
using Ligg.Base.Handlers;
using Ligg.Base.Helpers;

using Ligg.EasyWinApp.ImplInterface;

namespace Ligg.EasyWinApp.Implementation.Services
{
    internal class ValidationService
    {
        internal string GetValidationResult(string text, string validationRule)
        {
            try
            {
                var subParamSeparator = validationRule.GetSubParamSeparator();
                var ruleArray = validationRule.Split(subParamSeparator);
                var ruleName = ruleArray[0];
                if (ruleName == "PasswordFormat2")
                {
                    if (text.IsPassword())
                    {
                        return "true";
                    }
                    else
                    {
                        if (ValidationServiceData.ValidationAnnexes == null) ValidationServiceData.Init();

                        var txt = "";
                        txt = GlobalConfiguration.SupportMutiCultures ? AnnexHelper.GetText("", ruleName, ValidationServiceData.ValidationAnnexes, AnnexTextType.Description, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.StepByStep) :
                            AnnexHelper.GetText("", ruleName, JobServiceData.JobAnnexes, AnnexTextType.Description, "", GetAnnexMode.FirstAnnex);

                        return txt;

                    }
                }
                
                else throw new ArgumentException(" has no validation rule: " + ruleArray[0] + "! ");
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetValidationResult Error: " + ex.Message);
            }


        }
    }

    internal static class ValidationServiceData
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        public static List<Annex> ValidationAnnexes;
        public static void Init()
        {
            try
            {
                if (ValidationAnnexes == null)
                {
                    var xmlPath1 = Configuration.DataDir + "\\ValidationService\\ValidationAnnexes";
                    var xmlMgr1 = new XmlHandler(xmlPath1);
                    ValidationAnnexes = xmlMgr1.ConvertToObject<List<Annex>>();
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Init Error: " + ex.Message);
            }
        }
    }
}