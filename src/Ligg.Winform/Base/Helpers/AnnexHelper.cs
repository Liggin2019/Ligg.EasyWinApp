using System;
using System.Collections.Generic;
using Ligg.Base.DataModel;
using Ligg.Base.DataModel.Enums;
using Ligg.Base.Extension;


namespace Ligg.Base.Helpers
{

    public static class AnnexHelper
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        public static string DefaultLanguageCode;

        //#GetText by masterId
        public static string GetText(string className, long masterId, List<Annex> annexes, AnnexTextType textType, string curLangCode, GetAnnexMode getAnnexMode)
        {
            try
            {
                var annex = Get(className, masterId, annexes, curLangCode, getAnnexMode);
                if (annex == null) return string.Empty;
                else
                {
                    return GetTextByAnnex(annex, textType);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".GetText Error: " + ex.Message);
            }
        }

        //#GetText by masterName
        public static string GetText(string className, string masterName, List<Annex> annexes, AnnexTextType textType, string curLangCode, GetAnnexMode getAnnexMode)
        {
            try
            {
                var annex = Get(className, masterName, annexes, curLangCode, getAnnexMode);
                if (annex == null) return string.Empty;
                else
                {
                    return GetTextByAnnex(annex, textType);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".GetText Error: " + ex.Message);
            }
        }

        public static AnnexTextType GetTextType(string annexTypeStr)
        {
            try
            {
                var annexType = AnnexTextType.DisplayName;
                if (annexTypeStr.ToLower() == "displayName")
                {
                    annexType = AnnexTextType.DisplayName;
                }
                else if (annexTypeStr.ToLower() == "description")
                {
                    annexType = AnnexTextType.Description;
                }
                else if (annexTypeStr.ToLower() == "remark")
                {
                    annexType = AnnexTextType.Remark;
                }
                else if (annexTypeStr.ToLower() == "remark1")
                {
                    annexType = AnnexTextType.Remark1;
                }
                else if (annexTypeStr.ToLower() == "remark2")
                {
                    annexType = AnnexTextType.Remark2;
                }
                else if (annexTypeStr.ToLower() == "body")
                {
                    annexType = AnnexTextType.Body;
                }
                else if (annexTypeStr.ToLower() == "other")
                {
                    annexType = AnnexTextType.Other;
                }
                return annexType;
            }

            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".GetTextType Error: " + ex.Message);
            }
        }

        //##common
        //#Get by masterId
        private static Annex Get(string className, long masterId, List<Annex> annexes, string langCode, GetAnnexMode getAnnexMode)
        {
            if (annexes == null) return null;
            try
            {
                var annex = new Annex();
                if (getAnnexMode == GetAnnexMode.FirstAnnex)
                {
                    annex = className.IsNullOrEmpty() ? annexes.Find(x => x.MasterId == masterId)
                        : annexes.Find(x => x.ClassName == className && x.MasterId == masterId);
                }
                else if (getAnnexMode == GetAnnexMode.OnlyByCurLang)
                {
                    annex = className.IsNullOrEmpty() ? annexes.Find(x => x.MasterId == masterId && x.LanguageCode == langCode)
                        : annexes.Find(x => x.ClassName == className && x.MasterId == masterId && x.LanguageCode == langCode);
                }
                else if (getAnnexMode == GetAnnexMode.StepByStep)
                {
                    annex = className.IsNullOrEmpty() ? annexes.Find(x => x.MasterId == masterId && x.LanguageCode == langCode)
                        : annexes.Find(x => x.ClassName == className && x.MasterId == masterId && x.LanguageCode == langCode);

                    if (annex == null)
                    {
                        annex = className.IsNullOrEmpty() ? annexes.Find(x => x.MasterId == masterId && x.LanguageCode == DefaultLanguageCode)
                            : annexes.Find(x => x.ClassName == className && x.MasterId == masterId && x.LanguageCode == DefaultLanguageCode);
                    }

                    if (annex == null)
                    {
                        annex = className.IsNullOrEmpty() ? annexes.Find(x => x.MasterId == masterId)
                            : annexes.Find(x => x.ClassName == className && x.MasterId == masterId);
                    }
                }
                return annex ?? null;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Get Error: " + ex.Message);
            }
        }

        //#Get by masterName
        private static Annex Get(string className, string masterName, List<Annex> annexes, string langCode, GetAnnexMode getAnnexMode)
        {
            if (annexes == null) return null;
            try
            {
                var annex = new Annex();
                if (getAnnexMode == GetAnnexMode.FirstAnnex)
                {
                    annex = className.IsNullOrEmpty() ? annexes.Find(x => x.MasterName == masterName)
                        : annexes.Find(x => x.ClassName == className && x.MasterName == masterName);
                }
                else if (getAnnexMode == GetAnnexMode.OnlyByCurLang)
                {
                    annex = className.IsNullOrEmpty() ? annexes.Find(x => x.MasterName == masterName && x.LanguageCode == langCode)
                        : annexes.Find(x => x.ClassName == className && x.MasterName == masterName && x.LanguageCode == langCode);
                }
                else if (getAnnexMode == GetAnnexMode.StepByStep)
                {
                    annex = className.IsNullOrEmpty() ? annexes.Find(x => x.MasterName == masterName && x.LanguageCode == langCode)
                        : annexes.Find(x => x.ClassName == className && x.MasterName == masterName && x.LanguageCode == langCode);

                    if (annex == null)
                    {
                        annex = className.IsNullOrEmpty() ? annexes.Find(x => x.MasterName == masterName && x.LanguageCode == DefaultLanguageCode)
                            : annexes.Find(x => x.ClassName == className && x.MasterName == masterName && x.LanguageCode == DefaultLanguageCode);
                    }

                    if (annex == null)
                    {
                        annex = className.IsNullOrEmpty() ? annexes.Find(x => x.MasterName == masterName)
                            : annexes.Find(x => x.ClassName == className && x.MasterName == masterName);
                    }
                }
                return annex ?? null;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Get Error: " + ex.Message);
            }
        }

        //##GetTextByAnnex
        private static string GetTextByAnnex(Annex annex, AnnexTextType textType)
        {
            try
            {
                if (annex != null)
                {
                    if (textType == AnnexTextType.DisplayName) return annex.DisplayName;
                    else if (textType == AnnexTextType.Description) return annex.Description;
                    else if (textType == AnnexTextType.Body) return annex.Body;
                    else if (textType == AnnexTextType.Remark) return annex.Remark;
                    else if (textType == AnnexTextType.Remark1) return annex.Remark1;
                    else if (textType == AnnexTextType.Remark2) return annex.Remark2;
                    else if (textType == AnnexTextType.Other) return annex.Other;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".GetTextByAnnex Error: " + ex.Message);
            }
        }

    }

    public enum GetAnnexMode
    {
        FirstAnnex = 0,
        OnlyByCurLang = 1,
        StepByStep = 2,
    }
}
