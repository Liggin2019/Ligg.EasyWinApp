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
    internal class WinChgConfigGroupService
    {
        internal void Init()
        {
            try
            {
                InitData();

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".Init Error: " + ex.Message);
            }
        }

        //#get
        internal string GetProperty(int id, string property)
        {
            try
            {
                var winConfigGrp = WinConfigServiceData.WinChgConfigGroups.Find(x => x.Id == id);

                if (property.ToLower() == "DisplayName".ToLower())
                {
                    var text = GlobalConfiguration.SupportMultiCultures
                        ? AnnexHelper.GetText("", id, WinConfigServiceData.WinChgConfigGroupAnnexes, AnnexTextType.DisplayName, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.StepByStep)
                        : AnnexHelper.GetText("", id, WinConfigServiceData.WinChgConfigGroupAnnexes, AnnexTextType.DisplayName, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.FirstAnnex);
                    return text;
                }
                else if (property.ToLower() == "Description".ToLower())
                {

                    var text = GlobalConfiguration.SupportMultiCultures
                        ? AnnexHelper.GetText("", id, WinConfigServiceData.WinChgConfigGroupAnnexes, AnnexTextType.Description, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.StepByStep)
                        : AnnexHelper.GetText("", id, WinConfigServiceData.WinChgConfigGroupAnnexes, AnnexTextType.Description, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.FirstAnnex);
                    return text;
                }
                else if (property.ToLower() == "FixDes".ToLower())
                {
                    var text = GlobalConfiguration.SupportMultiCultures
                        ? AnnexHelper.GetText("", id, WinConfigServiceData.WinChgConfigGroupAnnexes, AnnexTextType.Remark, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.StepByStep)
                        : AnnexHelper.GetText("", id, WinConfigServiceData.WinChgConfigGroupAnnexes, AnnexTextType.Remark, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.FirstAnnex);
                    return text;
                }
                else if (property.ToLower() == "Fix1Des".ToLower())
                {
                    var text = GlobalConfiguration.SupportMultiCultures
                        ? AnnexHelper.GetText("", id, WinConfigServiceData.WinChgConfigGroupAnnexes, AnnexTextType.Remark1, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.StepByStep)
                        : AnnexHelper.GetText("", id, WinConfigServiceData.WinChgConfigGroupAnnexes, AnnexTextType.Remark1, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.FirstAnnex);
                    return text;
                }
                else if (property.ToLower() == "Fix2Des".ToLower())
                {
                    var text = GlobalConfiguration.SupportMultiCultures
                        ? AnnexHelper.GetText("", id, WinConfigServiceData.WinChgConfigGroupAnnexes, AnnexTextType.Remark2, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.StepByStep)
                        : AnnexHelper.GetText("", id, WinConfigServiceData.WinChgConfigGroupAnnexes, AnnexTextType.Remark2, GlobalConfiguration.CurrentLanguageCode, GetAnnexMode.FirstAnnex);
                    return text;

                }
                else if (property.ToLower() == "JudgeManually".ToLower())
                {
                    return winConfigGrp.JudgeManually.ToString();
                }
                else if (property.ToLower() == "HasSeeDetail".ToLower())
                {
                    return winConfigGrp.HasSeeDetail.ToString();
                }
                else if (property.ToLower() == "HasRefresh".ToLower())
                {
                    return winConfigGrp.HasRefresh.ToString();
                }
                else if (property.ToLower() == "HasRepair".ToLower())
                {
                    return winConfigGrp.HasRepair.ToString();
                }
                else if (property.ToLower() == "HasFix".ToLower())
                {
                    return winConfigGrp.HasFix.ToString();
                }
                else if (property.ToLower() == "FixAction".ToLower())
                {
                    return winConfigGrp.FixAction;
                }
                else if (property.ToLower() == "HasFix1".ToLower())
                {
                    return winConfigGrp.HasFix1.ToString();
                }
                else if (property.ToLower() == "Fix1Action".ToLower())
                {
                    return winConfigGrp.Fix1Action;
                }

                else if (property.ToLower() == "HasFix2".ToLower())
                {
                    return winConfigGrp.HasFix2.ToString();
                }
                else if (property.ToLower() == "Fix2Action".ToLower())
                {
                    return winConfigGrp.Fix2Action;
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
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetProperty Error: " + ex.Message);
            }
        }

        internal int GetStatus(int id)
        {
            try
            {
                var winConfigGrp = WinConfigServiceData.WinChgConfigGroups.Find(x => x.Id == id);
                winConfigGrp.Status = UniversalStatus.Ok;
                return (int)(winConfigGrp.Status);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetStatus Error: " + ex.Message);
            }
        }

        public void RepairSelectedItems(List<Int32> selectedWinChgConfigGroupIds)
        {
            try
            {
                foreach (var id in selectedWinChgConfigGroupIds)
                {
                    Repair(id);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RepairSelectedItems Error: " + ex.Message);
            }
        }

        public void Repair(int id)
        {
            try
            {
                Thread.Sleep(1000);
                Refresh(id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".Repair Error: " + ex.Message);
            }
        }

        public void RefreshSelectedItems(List<Int32> selectedWinChgConfigGroupIds)
        {
            try
            {
                foreach (var id in selectedWinChgConfigGroupIds)
                {
                    Refresh(id);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshSelectedItems Error: " + ex.Message);
            }
        }

        public void Refresh(int id)
        {
            try
            {
                var winConfigGrp = WinConfigServiceData.WinChgConfigGroups.Find(x => x.Id == id);
                winConfigGrp.Status = UniversalStatus.Ok;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".Refresh Error: " + ex.Message);
            }
        }

        public void SaveSelectedItems(List<Int32> selectedWinChgConfigGroupIds)
        {
            try
            {
                foreach (var id in selectedWinChgConfigGroupIds)
                {
                    Save(id);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".SaveSelectedItems Error: " + ex.Message);
            }
        }

        public void Save(int id)
        {
            try
            {
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".Save Error: " + ex.Message);
            }
        }


        private void InitData()
        {
            try
            {
                if (WinConfigServiceData.WinChgConfigGroups is null)
                {
                    WinConfigServiceData.Init();
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".InitData Error: " + ex.Message);
            }
        }

    }

    internal static class WinConfigServiceData
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
        public static List<Annex> WinChgConfigGroupAnnexes;
        public static List<WinChgConfigGroup> WinChgConfigGroups;
        public static void Init()
        {
            try
            {
                if (WinChgConfigGroups is null)
                {
                    var xmlPath = Configuration.DataDir + "\\WinChgConfigGroupService\\WinChgConfigGroups";
                    var xmlMgr = new XmlHandler(xmlPath);
                    WinChgConfigGroups = xmlMgr.ConvertToObject<List<WinChgConfigGroup>>();

                    var xmlPath1 = Configuration.DataDir + "\\WinChgConfigGroupService\\WinChgConfigGroupAnnexes";
                    var xmlMgr1 = new XmlHandler(xmlPath1);
                    WinChgConfigGroupAnnexes = xmlMgr1.ConvertToObject<List<Annex>>();

                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Init Error: " + ex.Message);
            }
        }
    }

    public class WinChgConfigGroup
    {
        public int Id;
        public string Name;
        public UniversalStatus Status;
        public bool JudgeManually;
        public bool HasSeeDetail;
        public bool HasRefresh;
        public bool HasRepair;
        public bool HasFix;
        public string FixAction;
        public bool HasFix1;
        public string Fix1Action;
        public bool HasFix2;
        public string Fix2Action;
        public bool HasSave;
        public bool HasReadDoc;
        public string ReadDocAction;
    }
    

}