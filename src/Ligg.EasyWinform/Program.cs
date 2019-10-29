using System;
using System.IO;
using System.Windows.Forms;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
using Ligg.EasyWinform.Resources;
using Ligg.Winform;
using Ligg.Winform.DataModel;
using Ligg.Winform.DataModel.Enums;
using Ligg.Winform.Forms;

namespace Ligg.EasyWinform
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                if (File.Exists("Ui.ini"))
                {
                    var txt = File.ReadAllText("Ui.ini");
                    args = txt.Split(' ');
                }
                else goto End;
            }

            var passedArg0 = args[0];

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                StartHelper.InitGlobalConfiguration();
                //+1
                //passedArg0 = EncryptionHelper.SmDecrypt(args[0],EncryptionHelper.GlobalKey1,EncryptionHelper.GlobalKey2);
                var passedArg0Arry = passedArg0.Split('@');
                if (passedArg0Arry.Length < 4) goto End;

                //passedArg0 format
                //invisibleStr@formTypeStr=0/func@startAppStr@startFuncStr   @startViewMenuIdStr@startParams@startActionsStr@passwordStr@formTitle@usrCode@usrToken  passedCultureName
                //invisibleStr@formTypeStr=1/task@startAppStr@startZoneLocStr@inputZoneVars     @startParams@startActionsStr@passwordStr@formTitle@usrCode@usrToken  passedCultureName
                var invisibleStr = "";
                var formTypeStr = "";
                var startAppStr = "";
                var startFuncOrZoneLocStr = "";
                var startViewMenuIdOrinputZoneVarsStr = "";
                var startActionsStr = "";
                var startPassword = "";
                var startParams = "";
                var formTitle = "";
                var usrCode = "";
                var usrToken = "";


                //#receive passedArg0
                invisibleStr = passedArg0Arry[0];
                formTypeStr = passedArg0Arry[1];
                startAppStr = passedArg0Arry[2];
                startFuncOrZoneLocStr = passedArg0Arry[3];
                if (passedArg0Arry.Length > 4) startViewMenuIdOrinputZoneVarsStr = passedArg0Arry[4];
                if (passedArg0Arry.Length > 5) startParams = passedArg0Arry[5];
                if (passedArg0Arry.Length > 6) startActionsStr = passedArg0Arry[6];
                if (passedArg0Arry.Length > 7) startPassword = passedArg0Arry[7];
                if (passedArg0Arry.Length > 8) formTitle = passedArg0Arry[8];
                if (passedArg0Arry.Length > 9) usrCode = passedArg0Arry[9];
                if (passedArg0Arry.Length > 10) usrToken = passedArg0Arry[10];


                //#subsequent action by passedArg0
                FunctionFormType formType = (formTypeStr == "0" | formTypeStr.IsNullOrEmpty()) ? FunctionFormType.MutiView : FunctionFormType.SingleView;
                StartHelper.SetPaths(formType, startAppStr, startFuncOrZoneLocStr);

                StartHelper.SetApplicationStartParamSet(formType);
                var appInitParamSet = StartHelper.ApplicationStartParamSet;

                if (appInitParamSet.SupportMutiCultures)
                {
                    var passedCultureName = "";
                    if (args.Length > 1) passedCultureName = args[1];
                    StartHelper.SetCultures();
                    var cultureName = StartHelper.DefaultCultureName;
                    if (!passedCultureName.IsNullOrEmpty() && CultureHelper.IsCultureNameValid(passedCultureName))
                    {
                        cultureName = passedCultureName;
                    }
                    CultureHelper.SetCurrentCulture(cultureName);
                }

                //#verify
                if (!StartHelper.Startup(appInitParamSet, null)) goto End; ;
                if (appInitParamSet.VerifyPasswordAtStart)
                {
                    if (!StartHelper.VerifyPassword(appInitParamSet.PasswordVerification, startPassword))
                    {
                        goto End;
                    }
                }

                //#set funcInitParamSet
                var funcInitParamSet = new FunctionInitParamSet();
                funcInitParamSet.IsFormInvisible = invisibleStr.ToLower() == "true" ? true : false;
                funcInitParamSet.FormType = formType;
                funcInitParamSet.ApplicationCode = startAppStr;
                if (formType == FunctionFormType.MutiView)
                {
                    funcInitParamSet.FunctionCode = startFuncOrZoneLocStr;
                }
                else
                {
                    var temArry1 = startFuncOrZoneLocStr.SplitByLastSeparator('\\');
                    funcInitParamSet.FunctionCode = temArry1.Length == 0 ? temArry1[0] : temArry1[1];
                    funcInitParamSet.ZoneLocationForNonMutiViewForm = StartHelper.StartZoneLocation;
                }

                if (!startViewMenuIdOrinputZoneVarsStr.IsNullOrEmpty())
                {
                    if (formType == FunctionFormType.MutiView)
                        funcInitParamSet.StartViewMenuId = Convert.ToInt32(startViewMenuIdOrinputZoneVarsStr);
                    else funcInitParamSet.InputZoneVariablesForNonMutiViewForm = startViewMenuIdOrinputZoneVarsStr;
                }

                funcInitParamSet.StartParams = startParams;
                funcInitParamSet.StartActions = startActionsStr;
                funcInitParamSet.StartPassword = startPassword;
                funcInitParamSet.FormTitle = formTitle;
                funcInitParamSet.HelpdeskEmail = appInitParamSet.HelpdeskEmail;
                funcInitParamSet.ApplicationVersion = appInitParamSet.ApplicationVersion;
                funcInitParamSet.ImplementationDllPath = appInitParamSet.ImplementationDllPath;
                funcInitParamSet.AdapterFullClassName = appInitParamSet.AdapterFullClassName;
                funcInitParamSet.SupportMutiCultures = appInitParamSet.SupportMutiCultures;

                //#ShowSoftwareCover
                if (appInitParamSet.ShowSoftwareCover)
                {
                    StartHelper.ShowSoftwareCover(funcInitParamSet);
                }

                //#Logon
                if (!(usrCode.IsNullOrEmpty())) appInitParamSet.LogonAtStart = false;
                if (appInitParamSet.LogonAtStart)
                {
                    if (!StartHelper.Logon(funcInitParamSet)) goto End;
                }

                //rd-1
                var form = new DebugForm(funcInitParamSet);
                //rd+1
                //var form = new ReleaseForm(funcInitParamSet);
                Application.Run(form);

            }
            catch (Exception ex)
            {
                PopupMessage.PopupError(EasyWinAppRes.ApplicationStartError, EasyWinAppRes.ApplicationStartError + ": " + ex.Message);
                goto End;
            }
        End:;
        }



    }
}
