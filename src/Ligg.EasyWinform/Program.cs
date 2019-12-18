using System;
using System.IO;
using System.Windows.Forms;
using Ligg.Base.DataModel.Enums;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
using Ligg.EasyWinApp.Common;
using Ligg.EasyWinApp.Common.Helpers;
using Ligg.EasyWinApp.ImplInterface;
using Ligg.EasyWinForm.Resources;
using Ligg.WinForm;
using Ligg.WinForm.DataModel;
using Ligg.WinForm.DataModel.Enums;

namespace Ligg.EasyWinForm
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var debugIniDir = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            var debugIniPath = debugIniDir + "\\Debug.ini";
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

                //##sg+1
                //##passedArg0 = EncryptionHelper.SmDecrypt(args[0],EncryptionHelper.GlobalKey1,EncryptionHelper.GlobalKey2);
                var passedArg0Arry = passedArg0.Split('@');
                if (passedArg0Arry.Length < 4) goto End;

                //##passedArg0 format
                //###formTypeStr=0, multiple view, 指用单窗体模拟多窗体的情况
                //invisibleStr@formTypeStr=0/multipleView@startAppStr@startFuncStr   @startViewMenuIdStr@startParams@startActionsStr@passwordStr@formTitle@userIdStr@usrCode@usrToken@userPassword@isRedirectedStr  passedCultureName

                //###formTypeStr=1, single view, 指单个Zone组成一个窗体
                //invisibleStr@formTypeStr=1/singleView @startAppStr@startZoneLocStr @inputZoneVars      @startParams@startActionsStr@passwordStr@formTitleuserIdStr@usrCode@usrToken@userPassword@isRedirectedStr  passedCultureName
                var invisibleStr = "";
                var formTypeStr = "";
                var startAppStr = "";
                var startFuncOrZoneLocStr = "";
                var startViewMenuIdOrinputZoneVarsStr = "";
                var startActionsStr = "";
                var startPassword = "";
                var startParams = "";
                var formTitle = "";
                var usrIdStr = "";
                var usrCode = "";
                var usrToken = "";
                var usrPassword = "";
                var isRedirectedStr = "";

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
                if (passedArg0Arry.Length > 9) usrIdStr = passedArg0Arry[9];
                if (passedArg0Arry.Length > 10) usrCode = passedArg0Arry[10];
                if (passedArg0Arry.Length > 11) usrToken = passedArg0Arry[11];
                if (passedArg0Arry.Length > 12) usrPassword = passedArg0Arry[12];
                if (passedArg0Arry.Length > 13) isRedirectedStr = passedArg0Arry[13];


                //#subsequent action by passedArg0
                FunctionFormType formType = FunctionFormType.MultipleView;
                if (formTypeStr == "1") formType = FunctionFormType.SingleView;

                bootStrap.SetPaths(formType, startAppStr, startFuncOrZoneLocStr);
                bootStrap.SetApplicationStartParamSet(formType);
                var appStartParamSet = BootStrap.ApplicationStartParamSet;

                if (appStartParamSet.SupportMultiCultures)
                {
                    var passedCultureName = "";
                    if (args.Length > 1) passedCultureName = args[1];
                    bootStrap.SetCultures(passedCultureName);
                }

                if (!appStartParamSet.StyleSheetCode.IsNullOrEmpty())
                {
                    //*init Ligg.WinForm.StyleSheet
                }

                //##CheckApplicationUsability
                if (!bootStrap.CheckApplicationUsability(appStartParamSet)) goto End;//to be extended 

                //##VerifyStartPassword
                var verifyStartPassword = true;
                if (!startPassword.IsNullOrEmpty())
                {
                    if (bootStrap.VerifyStartPassword(false,appStartParamSet.PasswordVerificationRule, startPassword))
                        verifyStartPassword = false;
                }
                if (appStartParamSet.VerifyPasswordAtStart)
                {
                    if (verifyStartPassword)
                        if (!bootStrap.VerifyStartPassword(true,appStartParamSet.PasswordVerificationRule, startPassword))
                        {
                            goto End;
                        }
                }

                //##set funcInitParamSet
                var funcInitParamSet = new FunctionInitParamSet();
                funcInitParamSet.IsFormInvisible = invisibleStr.ToLower() == "true" ? true : false;
                funcInitParamSet.FormType = formType;
                funcInitParamSet.ArchitectureCode = GlobalConfiguration.ArchitectureCode;
                funcInitParamSet.ApplicationCode = startAppStr;
                if (formType == FunctionFormType.MultipleView)
                {
                    funcInitParamSet.FunctionCode = startFuncOrZoneLocStr;
                }
                else
                {
                    var temArry1 = startFuncOrZoneLocStr.SplitByLastSeparator('\\');
                    funcInitParamSet.FunctionCode = temArry1.Length == 0 ? temArry1[0] : temArry1[1];
                    funcInitParamSet.ZoneLocationForNonMultiViewForm = bootStrap.StartZoneLocation;
                }

                if (!startViewMenuIdOrinputZoneVarsStr.IsNullOrEmpty())
                {
                    if (formType == FunctionFormType.MultipleView)
                        funcInitParamSet.StartViewMenuId = Convert.ToInt32(startViewMenuIdOrinputZoneVarsStr);
                    else funcInitParamSet.InputZoneVariablesForNonMutiViewForm = startViewMenuIdOrinputZoneVarsStr;
                }

                funcInitParamSet.StartParams = startParams;
                funcInitParamSet.StartActions = startActionsStr;
                funcInitParamSet.StartPassword = bootStrap.StartPassword;
                funcInitParamSet.FormTitle = formTitle;
                funcInitParamSet.HelpdeskEmail = appStartParamSet.HelpdeskEmail;
                funcInitParamSet.ApplicationVersion = appStartParamSet.ApplicationVersion;
                funcInitParamSet.SupportMultiCultures = appStartParamSet.SupportMultiCultures;

                //##init CblpDllAdapter
                var cblpDllPath = FileHelper.GetFilePath(appStartParamSet.ImplementationDllPath, DirectoryHelper.DeleteLastSlashes(Directory.GetCurrentDirectory()));
                var cblpDllDir = cblpDllPath.IsNullOrEmpty() ? "" : FileHelper.GetFileDetailByOption(cblpDllPath, FilePathComposition.Directory);
                funcInitParamSet.ImplementationDir = cblpDllDir;
                bootStrap.InitGlobalConfiguration(startAppStr, funcInitParamSet.SupportMultiCultures, CultureHelper.DefaultLanguageCode, CultureHelper.CurrentLanguageCode, startParams, cblpDllDir);
                if (!cblpDllPath.IsNullOrEmpty())
                    CblpDllAdapter.Init(debug, cblpDllPath, appStartParamSet.AdapterFullClassName);

                //##ShowSoftwareCover
                bool showSoftwareCover = isRedirectedStr.ToLower() != "true";
                if (appStartParamSet.ShowSoftwareCoverAtStart)
                {
                    if (showSoftwareCover)
                        bootStrap.ShowSoftwareCover(funcInitParamSet);
                }

                var logon = true;
                //##VerifyUserToken
                if (!usrIdStr.IsNullOrEmpty() & !usrCode.IsNullOrEmpty() & !usrToken.IsNullOrEmpty())
                {
                    var userId = Convert.ToInt64(usrIdStr);
                    if (bootStrap.VerifyUserToken(userId, usrCode, usrToken))
                        logon = false;
                }
                //##VerifyUserPassword
                else if (!usrCode.IsNullOrEmpty() & !usrPassword.IsNullOrEmpty())
                {
                    if (bootStrap.VerifyUserPassword(usrCode, usrToken))
                        logon = false;
                }
                //##Logon
                if (appStartParamSet.LogonAtStart)
                {
                    if (logon) if (!bootStrap.Logon(funcInitParamSet)) goto End;
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
