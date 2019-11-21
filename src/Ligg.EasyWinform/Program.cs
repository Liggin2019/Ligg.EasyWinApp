using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Ligg.Base.DataModel.Enums;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
using Ligg.EasyWinApp.Common;
using Ligg.EasyWinApp.Common.Helpers;
using Ligg.EasyWinApp.ImplInterface;
using Ligg.EasyWinForm.Resources;
using Ligg.Winform;
using Ligg.Winform.DataModel;
using Ligg.Winform.DataModel.Enums;
using Ligg.Winform.Forms;

namespace Ligg.EasyWinForm
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var debugIniPath = Directory.GetCurrentDirectory() + "\\Debug.ini";
            var debug = false;
            var argsStr = "";
            if (File.Exists(debugIniPath))
            {
                var debugStr = StartHelper.ReadIniString(debugIniPath, "setting", "debug", "");
                if (debugStr.ToLower() == "true") debug = true;
                argsStr = StartHelper.ReadIniString(debugIniPath, "setting", "args", "");
            }

            if (args.Length == 0)
            {
                args = argsStr.Split(' ');
            }

            if (args.Length == 0) goto End;
            var passedArg0 = args[0];

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                var bootStrap = new BootStrap();

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
                FunctionFormType formType = FunctionFormType.MutiView;
                if (formTypeStr == "1") formType = FunctionFormType.SingleView;
                else if (formTypeStr == "2") formType = FunctionFormType.Dialogue;

                bootStrap.SetPaths(formType, startAppStr, startFuncOrZoneLocStr);
                bootStrap.SetApplicationStartParamSet(formType);
                var appInitParamSet = BootStrap.ApplicationStartParamSet;

                if (appInitParamSet.SupportMutiCultures)
                {
                    var passedCultureName = "";
                    if (args.Length > 1) passedCultureName = args[1];
                    bootStrap.SetCultures(passedCultureName);
                }

                //##CheckApplicationUsability
                if (!bootStrap.CheckApplicationUsability(appInitParamSet)) goto End;

                //##VerifyStartPassword
                if (appInitParamSet.VerifyPasswordAtStart)
                {
                    if (!bootStrap.VerifyStartPassword(appInitParamSet.PasswordVerificationRule, startPassword))
                    {
                        goto End;
                    }
                }

                //##set funcInitParamSet
                var funcInitParamSet = new FunctionInitParamSet();
                funcInitParamSet.IsFormInvisible = invisibleStr.ToLower() == "true" ? true : false;
                funcInitParamSet.FormType = formType;
                funcInitParamSet.AssemblyCode = GlobalConfiguration.AssemblyCode;
                funcInitParamSet.ApplicationCode = startAppStr;
                if (formType == FunctionFormType.MutiView)
                {
                    funcInitParamSet.FunctionCode = startFuncOrZoneLocStr;
                }
                else
                {
                    var temArry1 = startFuncOrZoneLocStr.SplitByLastSeparator('\\');
                    funcInitParamSet.FunctionCode = temArry1.Length == 0 ? temArry1[0] : temArry1[1];
                    funcInitParamSet.ZoneLocationForNonMutiViewForm = bootStrap.StartZoneLocation;
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
                funcInitParamSet.SupportMutiCultures = appInitParamSet.SupportMutiCultures;


                var cblpDllPath = FileHelper.GetFilePath(appInitParamSet.ImplementationDllPath, DirectoryHelper.DeleteLastSlashes(Directory.GetCurrentDirectory()));
                var cblpDllDir = cblpDllPath.IsNullOrEmpty() ? "" : FileHelper.GetFileDetailByOption(cblpDllPath, FilePathComposition.Directory);
                funcInitParamSet.ImplementationDir = cblpDllDir;

                bootStrap.InitGlobalConfiguration(startAppStr, funcInitParamSet.SupportMutiCultures, CultureHelper.DefaultLanguageCode, CultureHelper.CurrentLanguageCode, startParams, cblpDllDir);
                if (!cblpDllPath.IsNullOrEmpty())
                    CblpDllAdapter.Init(debug, cblpDllPath, appInitParamSet.AdapterFullClassName);

                //##ShowSoftwareCover
                if (appInitParamSet.ShowSoftwareCoverAtStart)
                {
                    bootStrap.ShowSoftwareCover(funcInitParamSet);
                }

                //##VerifyUserToken
                if (!(usrToken.IsNullOrEmpty()))
                {
                    if (bootStrap.VerifyUserToken(usrCode, usrToken))
                        appInitParamSet.LogonAtStart = false;
                }

                //##Logon
                if (appInitParamSet.LogonAtStart)
                {
                    if (!bootStrap.Logon(funcInitParamSet)) goto End;
                }

                var form = new StartForm(funcInitParamSet);
                Application.Run(form);

            }
            catch (Exception ex)
            {
                PopupMessage.PopupError(EasyWinAppRes.ApplicationStartError, EasyWinAppRes.ApplicationStartError + ": " + ex.Message);
            }
        End:;
        }



    }
}
