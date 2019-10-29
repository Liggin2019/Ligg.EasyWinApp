using System;
using System.Data;
using System.IO;
using Ligg.Base.DataModel.Enums;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
//rd-1
using Ligg.EasyWinApp.Implementation;
using Ligg.EasyWinApp.ImplInterface;
using Ligg.Winform.DataModel;

namespace Ligg.Winform.Forms
{
    public partial class DebugForm : FunctionForm
    {
        private IAdapter _adapter = null;
        public DebugForm(FunctionInitParamSet funcInitParamSet)
        {
            InitializeComponent();

            FunctionInitParamSet = funcInitParamSet;
            GlobalConfiguration.AppCode = funcInitParamSet.ApplicationCode;
            GlobalConfiguration.AppCode = funcInitParamSet.ApplicationCode;
            GlobalConfiguration.SupportMutiCultures = funcInitParamSet.SupportMutiCultures;
            GlobalConfiguration.DefaultLanguageCode = CultureHelper.DefaultLanguageCode;
            GlobalConfiguration.CurrentLanguageCode = CultureHelper.CurrentLanguageCode;
            var startParams = funcInitParamSet.StartParams;
            if (!startParams.IsNullOrEmpty())
            {
                GlobalConfiguration.SetStartParams(startParams);
            }
            var implementationDllPath = funcInitParamSet.ImplementationDllPath;
            implementationDllPath = FileHelper.GetFilePath(implementationDllPath, DirectoryHelper.DeleteLastSlashes(Directory.GetCurrentDirectory()));
            if (!implementationDllPath.IsNullOrEmpty())
            {
                GlobalConfiguration.ImplementationDir = FileHelper.GetFileDetailByOption(implementationDllPath, FilePathComposition.Directory);
                //rd-2
                _adapter = new Adapter();
                _adapter.Initialize();
            }

        }


        private void DebugForm_Load(object sender, EventArgs e)
        {
            try
            {
                KeyPreview = true;

            }
            catch (Exception ex)
            {
                var methodName = "DebugForm_Load";
                PopupMessage.PopupError(FunctionNameForException + ">" + GetType().FullName + "." + methodName + " Error: ", ex.Message, AdditionalInfoForException);
            }
        }


        //#override
        protected override string ResolveConstantsEx(string text)
        {
            try
            {
                var retStr = _adapter.ResolveConstants(text);
                return retStr;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".ResolveConstantsEx Error: " + ex.Message);
            }
        }

        protected override string GetTextEx(string funName, string[] funcParamArray)
        {
            try
            {
                var retStr = _adapter.GetText(funName, funcParamArray);
                return retStr;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetTextEx Error: " + ex.Message);
            }
        }

        protected override DataTable GetValueTextDataTableEx(string funcName, string[] funcParamArray)
        {
            try
            {
                var retDt = new DataTable();
                retDt = _adapter.GetValueTextDataTable(funcName, funcParamArray);
                return retDt;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetDataTableEx Error: " + ex.Message);
            }
        }


        protected override string ActEx(string action, string[] actionParamArray)
        {
            var returnMsg = "";
            try
            {
                var retStr = _adapter.Act(action, actionParamArray);
                return retStr;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".ActEx Error: action=" + action + "; " + ex.Message);
            }
        }


    }
}
