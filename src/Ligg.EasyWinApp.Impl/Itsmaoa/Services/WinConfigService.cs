using System;
using System.Collections.Generic;
using System.Threading;
using Ligg.Base.DataModel;
using Ligg.Base.DataModel.Enums;
using Ligg.Base.Handlers;
using Ligg.Base.Helpers;

using Ligg.EasyWinApp.ImplInterface;

namespace Ligg.EasyWinApp.Implementation.Services
{
    internal class WinConfigService
    {
        internal void InitData()
        {
            try
            {
                if (WinConfigServiceData.WinConfigGroups is null)
                {
                    WinConfigServiceData.Init();
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".InitData Error: " + ex.Message);
            }
        }

        //#WinConfigGroup
        //##GetWinConfigGroupProperty
        internal string GetWinConfigGroupProperty(int id, string property)
        {
            try
            {
                var winConfigGrp = WinConfigServiceData.WinConfigGroups.Find(x => x.Id == id);

                if (property.ToLower() == "DisplayName".ToLower())
                {
                    var text = GlobalConfiguration.SupportMutiCultures
                        ? AnnexHelper.GetText("", id, WinConfigServiceData.WinConfigGroupAnnexes, AnnexTextType.DisplayName, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.StepByStep)
                        : AnnexHelper.GetText("", id, WinConfigServiceData.WinConfigGroupAnnexes, AnnexTextType.DisplayName, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.FirstAnnex);
                    return text;
                }
                else if (property.ToLower() == "Description".ToLower())
                {

                    var text = GlobalConfiguration.SupportMutiCultures
                        ? AnnexHelper.GetText("", id, WinConfigServiceData.WinConfigGroupAnnexes, AnnexTextType.Description, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.StepByStep)
                        : AnnexHelper.GetText("", id, WinConfigServiceData.WinConfigGroupAnnexes, AnnexTextType.Description, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.FirstAnnex);
                    return text;
                }
                else if (property.ToLower() == "ManualFixDes".ToLower())
                {
                    var text = GlobalConfiguration.SupportMutiCultures
                        ? AnnexHelper.GetText("", id, WinConfigServiceData.WinConfigGroupAnnexes, AnnexTextType.Remark, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.StepByStep)
                        : AnnexHelper.GetText("", id, WinConfigServiceData.WinConfigGroupAnnexes, AnnexTextType.Remark, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.FirstAnnex);
                    return text;
                }
                else if (property.ToLower() == "ManualFix1Des".ToLower())
                {
                    var text = GlobalConfiguration.SupportMutiCultures
                        ? AnnexHelper.GetText("", id, WinConfigServiceData.WinConfigGroupAnnexes, AnnexTextType.Remark1, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.StepByStep)
                        : AnnexHelper.GetText("", id, WinConfigServiceData.WinConfigGroupAnnexes, AnnexTextType.Remark1, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.FirstAnnex);
                    return text;
                }
                else if (property.ToLower() == "ManualFix2Des".ToLower())
                {
                    var text = GlobalConfiguration.SupportMutiCultures
                        ? AnnexHelper.GetText("", id, WinConfigServiceData.WinConfigGroupAnnexes, AnnexTextType.Remark2, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.StepByStep)
                        : AnnexHelper.GetText("", id, WinConfigServiceData.WinConfigGroupAnnexes, AnnexTextType.Remark2, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.FirstAnnex);
                    return text;

                }
                else if (property.ToLower() == "HasManualJudge".ToLower())
                {
                    return winConfigGrp.HasManualJudge.ToString();
                }
                else if (property.ToLower() == "HasSeeDetail".ToLower())
                {
                    return winConfigGrp.HasSeeDetail.ToString();
                }
                else if (property.ToLower() == "HasRefresh".ToLower())
                {
                    return winConfigGrp.HasRefresh.ToString();
                }
                else if (property.ToLower() == "HasAutoFix".ToLower())
                {
                    return winConfigGrp.HasAutoFix.ToString();
                }
                else if (property.ToLower() == "HasManualFix".ToLower())
                {
                    return winConfigGrp.HasManualFix.ToString();
                }
                else if (property.ToLower() == "ManualFixAction".ToLower())
                {
                    return winConfigGrp.ManualFixAction;
                }

                else if (property.ToLower() == "HasManualFix1".ToLower())
                {
                    return winConfigGrp.HasManualFix1.ToString();
                }
                else if (property.ToLower() == "ManualFix1Action".ToLower())
                {
                    return winConfigGrp.ManualFix1Action;
                }

                else if (property.ToLower() == "HasManualFix2".ToLower())
                {
                    return winConfigGrp.HasManualFix2.ToString();
                }
                else if (property.ToLower() == "ManualFix2Action".ToLower())
                {
                    return winConfigGrp.ManualFix2Action;
                }

                else if (property.ToLower() == "HasSave".ToLower())
                {
                    return winConfigGrp.HasSave.ToString();
                }
                else if (property.ToLower() == "HasReadDoc".ToLower())
                {
                    return winConfigGrp.HasReadDoc.ToString();
                }
                else if (property.ToLower() == "ReadDocAction".ToLower())
                {
                    return winConfigGrp.ReadDocAction;
                }
                else throw new ArgumentException("has no property: " + property);

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetWinConfigGroupProperty Error: " + ex.Message);
            }
        }

        internal int GetWinConfigGroupStatus(int id)
        {
            try
            {
                var winConfigGrp = WinConfigServiceData.WinConfigGroups.Find(x => x.Id == id);
                winConfigGrp.Status = UniversalStatus.Ok;
                return (int)(winConfigGrp.Status);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetWinConfigGroupProperty Error: " + ex.Message);
            }
        }

        public void RefreshSelectedWinConfigGroups(List<Int32> selectedWinConfigGroupIds)
        {
            try
            {
                foreach (var id in selectedWinConfigGroupIds)
                {
                    RefreshWinConfigGroup(id);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshWinConfigGroups Error: " + ex.Message);
            }
        }

        public void RefreshWinConfigGroup(int id)
        {
            try
            {
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshWinConfigGroup Error: " + ex.Message);
            }
        }

        public void AutoFixSelectedWinConfigGroups(List<Int32> selectedWinConfigGroupIds)
        {
            try
            {
                foreach (var id in selectedWinConfigGroupIds)
                {
                    AutoFixWinConfigGroup(id);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".AutoFixWinConfigGroups Error: " + ex.Message);
            }
        }

        public void AutoFixWinConfigGroup(int id)
        {
            try
            {
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".AutoFixWinConfigGroup Error: " + ex.Message);
            }
        }

        public void SaveSelectedWinConfigGroups(List<Int32> selectedWinConfigGroupIds)
        {
            try
            {
                foreach (var id in selectedWinConfigGroupIds)
                {
                    SaveWinConfigGroup(id);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".SaveWinConfigGroups Error: " + ex.Message);
            }
        }

        public void SaveWinConfigGroup(int id)
        {
            try
            {
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".SaveWinConfigGroup Error: " + ex.Message);
            }
        }


        //#WinConfig
        //##UpdateWinConfig
        public void UpdateWinConfig(string filePathOrConfigParamLines)
        {
            try
            {

                throw new ArgumentException("UpdateWinConfig can not be used! ");
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".UpdateWinConfig Error: " + ex.Message);
            }
        }



    }

    internal static class WinConfigServiceData
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
        public static List<Annex> WinConfigGroupAnnexes;
        public static List<WinConfigGroup> WinConfigGroups;
        public static void Init()
        {
            try
            {
                if (WinConfigGroups is null)
                {
                    var xmlPath = Configuration.DataDir + "\\WinConfigService\\WinConfigGroups";
                    var xmlMgr = new XmlHandler(xmlPath);
                    WinConfigGroups = xmlMgr.ConvertToObject<List<WinConfigGroup>>();

                    var xmlPath1 = Configuration.DataDir + "\\WinConfigService\\WinConfigGroupAnnexes";
                    var xmlMgr1 = new XmlHandler(xmlPath1);
                    WinConfigGroupAnnexes = xmlMgr1.ConvertToObject<List<Annex>>();

                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Init Error: " + ex.Message);
            }
        }

        public static string test = "1234";
    }

    public class WinConfigGroup
    {
        public int Id;
        public string Name;
        public UniversalStatus Status;
        public bool HasManualJudge;
        public bool HasSeeDetail;
        public bool HasRefresh;
        public bool HasAutoFix;
        public bool HasManualFix;
        public string ManualFixAction;
        public bool HasManualFix1;
        public string ManualFix1Action;
        public bool HasManualFix2;
        public string ManualFix2Action;
        public bool HasSave;
        public bool HasReadDoc;
        public string ReadDocAction;
    }
    

}