using System;
using System.Data;
using Ligg.Base.Helpers;
using Ligg.EasyWinApp.Common;
using Ligg.EasyWinApp.ImplInterface;
using Ligg.Winform.DataModel;
using Ligg.Winform.Forms;

namespace Ligg.EasyWinForm
{
    public partial class StartForm : FunctionForm
    {
        private IAdapter _adapter = null;

        public StartForm(FunctionInitParamSet functionInitParamSet)
        {
            try
            {
                FunctionInitParamSet = functionInitParamSet;
                _adapter = CblpDllAdapter.Adapter;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".StartForm Error: " + ex.Message);
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


        protected override string ActEx(string action, string[] actionParamArray)
        {
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

        protected override void PopupZoneDialogEx(FunctionInitParamSet functionInitParamSet)
        {
            try
            {
                var form = new StartForm(functionInitParamSet);
                form.ShowInTaskbar = false;
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".PopupZoneDialogEx Error: " + ex.Message);
            }
        }

        protected override string GetCurrentUserToken()
        {
            try
            {
                return GlobalConfiguration.UserToken;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetCurrentUserToken Error: " + ex.Message);
            }
        }

        protected override string GetCurrentUserCode()
        {
            try
            {
                return GlobalConfiguration.UserCode;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetCurrentUserCode Error: " + ex.Message);
            }
        }

        protected override void OnCurrentLanguageChanged()
        {
            try
            {
                GlobalConfiguration.CurrentLanguageCode = CultureHelper.CurrentLanguageCode;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".OnCurrentLanguageChanged Error: " + ex.Message);
            }
        }



    }

}
