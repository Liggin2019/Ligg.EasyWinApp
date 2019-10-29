using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
using Ligg.Base.Handlers;
using Ligg.EasyWinform.Resources;
using Ligg.Winform;
using Ligg.Winform.DataModel;
using Ligg.Winform.DataModel.Enums;
using Ligg.Winform.Dialogs;
using Ligg.Winform.Helpers;

namespace Ligg.EasyWinform
{
    public static class StartHelper
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        internal static string DefaultCultureName = "";
        private static string _startUpDir = "";
        private static string _appDir = "";
        private static string _uiDir = "";
        private static string _zonesDir = "";
        private static string _startFuncLocation = "";
        internal static string StartZoneLocation = "";

        internal static ApplicationStartParamSet ApplicationStartParamSet;


        //#set
        internal static void InitGlobalConfiguration()
        {
            var gcHandler = new GcHandler();
            gcHandler.Init();
            EncryptionHelper.Key1 = gcHandler.GetGlobalKey1();
            EncryptionHelper.Key2 = gcHandler.GetGlobalKey2();

        }




        internal static void SetPaths(FunctionFormType formType, string appCode, string funcCodeOrZoneLoc)
        {
            var executablePath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            //var workingDirectory = Directory.GetParent(executablePath).ToString();
            var workingDirectory = executablePath;
            Directory.SetCurrentDirectory(DirectoryHelper.DeleteLastSlashes(workingDirectory));
            _startUpDir = DirectoryHelper.DeleteLastSlashes(Directory.GetCurrentDirectory());

            _appDir = _startUpDir + "\\Applications\\" + appCode;
            _uiDir = _startUpDir + "\\Applications\\" + appCode + "\\Ui";
            _zonesDir = _uiDir + "\\Zones";
            if (formType == FunctionFormType.MutiView)
                _startFuncLocation = _startUpDir + "\\Applications\\" + appCode + "\\Ui\\Functions\\" + funcCodeOrZoneLoc;
            else StartZoneLocation = FileHelper.GetFilePath(funcCodeOrZoneLoc, _startUpDir + "\\Applications\\" + appCode + "\\Ui\\Zones");
        }

        internal static void SetCultures()
        {
            try
            {
                var xmlPath = _appDir + "\\Cultures\\Cultures";
                var xmlMgr = new XmlHandler(xmlPath);
                var cultures = xmlMgr.ConvertToObject<List<Culture>>();
                if (cultures != null)
                {
                    CultureHelper.Cultures = cultures;
                    var defCulture = cultures.Find(x => x.IsDefault);
                    if (defCulture != null)
                    {
                        DefaultCultureName = defCulture.Name;
                    }
                    else
                    {
                        DefaultCultureName = cultures.First().Name;
                    }

                }
                else
                {
                    throw new ArgumentException("Can't get valid Cultures!");
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".SetCultures Error: " + ex.Message);
            }
        }

        internal static void SetApplicationStartParamSet(FunctionFormType formType)
        {
            try
            {
                var xmlPath = _appDir + "\\ApplicationStartParamSet";
                var xmlMgr = new XmlHandler(xmlPath);
                var applicationStartParamSet = xmlMgr.ConvertToObject<ApplicationStartParamSet>();

                var functionStartParamSet = new FunctionStartParamSet();
                if (formType == FunctionFormType.MutiView)
                {
                    xmlPath = _startFuncLocation + "\\FunctionStartParamSet";
                    if (File.Exists(xmlPath + ".xml") | File.Exists(xmlPath + ".exml"))
                    {
                        xmlMgr = new XmlHandler(xmlPath);
                        functionStartParamSet = xmlMgr.ConvertToObject<FunctionStartParamSet>();
                    }
                }
                else
                {
                    xmlPath = StartZoneLocation + "\\FunctionStartParamSet";
                    if (File.Exists(xmlPath + ".xml") | File.Exists(xmlPath + ".exml"))
                    {
                        xmlMgr = new XmlHandler(xmlPath);
                        functionStartParamSet = xmlMgr.ConvertToObject<FunctionStartParamSet>();
                    }
                }
                applicationStartParamSet.ShowSoftwareCover = functionStartParamSet.ShowSoftwareCover;
                applicationStartParamSet.SoftwareCoverLocation = functionStartParamSet.SoftwareCoverLocation;
                applicationStartParamSet.SoftwareCoverActionsAtStart = functionStartParamSet.SoftwareCoverActionsAtStart;
                applicationStartParamSet.VerifyPasswordAtStart = functionStartParamSet.VerifyPasswordAtStart;
                applicationStartParamSet.PasswordVerification = functionStartParamSet.PasswordVerification;
                applicationStartParamSet.LogonAtStart = functionStartParamSet.LogonAtStart;
                applicationStartParamSet.LogonZoneLocation = functionStartParamSet.LogonZoneLocation;
                applicationStartParamSet.CheckLicenseAvailability = functionStartParamSet.CheckLicenseAvailability;
                applicationStartParamSet.CheckPublishmentValidity = functionStartParamSet.CheckPublishmentValidity;
                applicationStartParamSet.CheckSoftwareVersion = functionStartParamSet.CheckSoftwareVersion;
                applicationStartParamSet.CheckHostingLocation = functionStartParamSet.CheckHostingLocation;
                applicationStartParamSet.HostingServers = functionStartParamSet.HostingServers;

                ApplicationStartParamSet = applicationStartParamSet;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".SetApplicationStartParamSet Error: " + ex.Message);
            }
        }

        //#act
        internal static bool Startup(ApplicationStartParamSet appStartParamSet, Control ctrl)
        {
            try
            {
                var msg = "";
                if (appStartParamSet.CheckLicenseAvailability)
                {
                    msg = "Checking License Availability";
                    if (ctrl != null)
                    {
                        Thread.Sleep(50);
                        RefreshCtrl(ctrl, msg + "...");
                    }
                    if (!CheckLicenseAvailability())
                    {
                        PopupMessage.PopupError(EasyWinAppRes.ApplicationStartError, EasyWinAppRes.ApplicationStartError + ": " + msg + " " + "Error");
                        return false;
                    }
                }

                if (appStartParamSet.CheckPublishmentValidity)
                {
                    msg = "Checking publishment Validity";
                    if (ctrl != null)
                    {
                        Thread.Sleep(50);
                        RefreshCtrl(ctrl, msg + "...");
                    }

                    if (!CheckPublishmentValidity())
                    {
                        PopupMessage.PopupError(EasyWinAppRes.ApplicationStartError,
                            EasyWinAppRes.ApplicationStartError + ": " + msg + " " + "Error");
                        return false;
                    }
                }

                if (appStartParamSet.CheckSoftwareVersion)
                {
                    msg = "Checking Software Version";
                    if (ctrl != null)
                    {
                        Thread.Sleep(50);
                        RefreshCtrl(ctrl, msg + "...");
                    }

                    if (!CheckSoftwareVersion())
                    {
                        PopupMessage.PopupError(EasyWinAppRes.ApplicationStartError,
                            EasyWinAppRes.ApplicationStartError + ": " + msg + " " + "Error");
                        return false;
                    }
                }

                if (appStartParamSet.CheckHostingLocation)
                {
                    msg = "Verifying Assembly Hosting Location";
                    if (ctrl != null)
                    {
                        Thread.Sleep(50);
                        RefreshCtrl(ctrl, msg + "...");
                    }

                    if (!CheckHostingLocation(appStartParamSet.HostingServers))
                    {
                        PopupMessage.PopupError(EasyWinAppRes.ApplicationStartError,
                            EasyWinAppRes.ApplicationStartError + ": " + msg + " " + "Error");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                PopupMessage.PopupError(EasyWinAppRes.ApplicationStartError, EasyWinAppRes.ApplicationStartError + ": " + ex.Message);
                return false;
            }
        }



        internal static bool VerifyPassword(string passwordVerificationStr, string password)
        {
            try
            {
                if (!passwordVerificationStr.IsNullOrEmpty())
                {
                    //passwordVerificationStr = EncryptionHelper.SmDecrypt(passwordVerificationStr,
                    //                          StartHelper.GlobalEncrptKey1, StartHelper.GlobalEncrptKey2);
                    if (passwordVerificationStr.Contains("；")) passwordVerificationStr = passwordVerificationStr.Replace("；", ";");
                    if (passwordVerificationStr.Contains("，")) passwordVerificationStr = passwordVerificationStr.Replace("，", ",");
                    var accessType = passwordVerificationStr.SplitByTwoDifferentStrings("AccessType:", ";", true)[0];
                    var verifyRule = passwordVerificationStr.SplitByTwoDifferentStrings("VerificationRule:", ";", true)[0];
                    var verifyParams = passwordVerificationStr.SplitByTwoDifferentStrings("VerificationParams:", ";", true)[0];

                    if (verifyRule.ToLower() != "ClearText".ToLower()&verifyRule.ToLower() != "TdePassword".ToLower() & verifyRule.ToLower() != "Password".ToLower())
                        throw new ArgumentException("verifyRule is not correct!");

                    if (accessType.ToLower() == "Manual".ToLower())
                    {
                        var dlg = new TextInputDialog();
                        {
                            dlg.Text = EasyWinAppRes.PlsInputPassword;
                            dlg.VerificationRule = verifyRule;
                            dlg.VerificationParams = verifyParams;
                            dlg.ShowDialog();
                            return dlg.IsOk ;
                        }
                    }
                    else if (accessType.ToLower() == "Auto".ToLower())
                    {
                        if (password.IsNullOrEmpty())
                        {
                            throw new ArgumentException("Password can't be empty!");
                        }
                        else
                        {
                            if (!TextVerificationHelper.Verify(password, verifyRule, verifyParams))
                                throw new ArgumentException("Password is incorrect!");
                        }
                    }
                    else if (accessType.ToLower() == "Both".ToLower())
                    {
                        if (password.IsNullOrEmpty())
                        {
                            var dlg = new TextInputDialog();
                            {
                                dlg.Text = EasyWinAppRes.PlsInputPassword;
                                dlg.VerificationRule = verifyRule;
                                dlg.VerificationParams = verifyParams;
                                dlg.ShowDialog();
                                return dlg.IsOk;
                            }
                        }
                        else
                        {
                            if (!TextVerificationHelper.Verify(password, verifyRule, verifyParams))
                                throw new ArgumentException("Password is incorrect!");
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".CheckStartPassword Error: " + ex.Message);
            }
            return true;

        }

        internal static void ShowSoftwareCover(FunctionInitParamSet mainFunctionInitParamSet)
        {
            var singleViewFormInitParamSet = new FunctionInitParamSet();
            singleViewFormInitParamSet.FormType = FunctionFormType.SingleView;
            singleViewFormInitParamSet.AssemblyCode = mainFunctionInitParamSet.AssemblyCode;
            singleViewFormInitParamSet.ApplicationCode = mainFunctionInitParamSet.ApplicationCode;
            var temArry = ApplicationStartParamSet.SoftwareCoverLocation.SplitByLastSeparator('\\');

            singleViewFormInitParamSet.FunctionCode = temArry.Length == 0 ? temArry[0] : temArry[1];
            singleViewFormInitParamSet.ZoneLocationForNonMutiViewForm = FileHelper.GetFilePath(ApplicationStartParamSet.SoftwareCoverLocation, _zonesDir);

            singleViewFormInitParamSet.StartParams = string.Empty;
            singleViewFormInitParamSet.StartActions = ApplicationStartParamSet.SoftwareCoverActionsAtStart;
            singleViewFormInitParamSet.StartPassword = string.Empty;
            singleViewFormInitParamSet.InputZoneVariablesForNonMutiViewForm = mainFunctionInitParamSet.FunctionCode;
            singleViewFormInitParamSet.HelpdeskEmail = mainFunctionInitParamSet.HelpdeskEmail;
            singleViewFormInitParamSet.ApplicationVersion = mainFunctionInitParamSet.ApplicationVersion;
            singleViewFormInitParamSet.ImplementationDllPath = mainFunctionInitParamSet.ImplementationDllPath;
            singleViewFormInitParamSet.AdapterFullClassName = mainFunctionInitParamSet.AdapterFullClassName;
            singleViewFormInitParamSet.SupportMutiCultures = mainFunctionInitParamSet.SupportMutiCultures;
            //rd+1
            //var form = new Ligg.Winform.Forms.ReleaseForm(singleViewFormInitParamSet);
            //rd-1
            var form = new Ligg.Winform.Forms.DebugForm(singleViewFormInitParamSet);

            Application.Run(form);
        }

        internal static bool Logon(FunctionInitParamSet mainFunctionInitParamSet)
        {
            var singleViewFormInitParamSet = new FunctionInitParamSet();
            singleViewFormInitParamSet.FormType = FunctionFormType.SingleView;
            singleViewFormInitParamSet.AssemblyCode = mainFunctionInitParamSet.AssemblyCode;
            singleViewFormInitParamSet.ApplicationCode = mainFunctionInitParamSet.ApplicationCode;
            var temArry = ApplicationStartParamSet.LogonZoneLocation.SplitByLastSeparator('\\');
            singleViewFormInitParamSet.FunctionCode = temArry.Length == 0 ? temArry[0] : temArry[1];
            singleViewFormInitParamSet.ZoneLocationForNonMutiViewForm = FileHelper.GetFilePath(ApplicationStartParamSet.LogonZoneLocation, _zonesDir);
            singleViewFormInitParamSet.InputZoneVariablesForNonMutiViewForm = string.Empty;

            singleViewFormInitParamSet.StartParams = string.Empty;
            singleViewFormInitParamSet.StartActions = string.Empty;
            singleViewFormInitParamSet.StartPassword = string.Empty;
            //singleViewFormInitParamSet.StartUserToken = string.Empty;
            //singleViewFormInitParamSet.StartUserShortName = string.Empty;
            singleViewFormInitParamSet.HelpdeskEmail = mainFunctionInitParamSet.HelpdeskEmail;
            singleViewFormInitParamSet.ApplicationVersion = mainFunctionInitParamSet.ApplicationVersion;
            singleViewFormInitParamSet.ImplementationDllPath = mainFunctionInitParamSet.ImplementationDllPath;
            singleViewFormInitParamSet.AdapterFullClassName = mainFunctionInitParamSet.AdapterFullClassName;
            singleViewFormInitParamSet.SupportMutiCultures = mainFunctionInitParamSet.SupportMutiCultures;
            //rd+1
            //var form = new Ligg.Winform.Forms.ReleaseForm(singleViewFormInitParamSet);
            //rd-1
            var form = new Ligg.Winform.Forms.DebugForm(singleViewFormInitParamSet);

            Application.Run(form);
            if (form.Exit) return false;
            return true;
        }

        //##common
        private static bool CheckPublishmentValidity()
        {
            return true;
        }

        private static bool CheckLicenseAvailability()
        {
            return true;
        }

        private static bool CheckSoftwareVersion()
        {
            return true;
        }

        private static bool CheckHostingLocation(string hostingServers)
        {
            if (hostingServers.IsNullOrEmpty()) return true;
            var hostingServerArry = hostingServers.SplitThenTrim(',');
            if (_startUpDir.ToLower().StartsWith(("\\\\")))
            {
                if (hostingServerArry.Where(x => !x.IsNullOrEmpty()).Any(x => _startUpDir.ToLower().StartsWith(("\\\\" + x).ToLower())))
                {
                    return true;
                }
            }
            else
            {
                var machineName = Environment.MachineName;
                var ips = GetIps().SplitThenTrim(',');
                foreach (var x in hostingServerArry)
                {
                    if (x.ToLower() == machineName.ToLower() | (ips.Contains(x))) return true;
                }
            }
            return false;
        }

        private static void RefreshCtrl(Control ctrl, string text)
        {
            ctrl.Text = text;
            ctrl.Refresh();
        }

        private static string GetIps()
        {
            try
            {
                IPAddress[] arrIPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
                var ipsStr = "";
                int count = 0;
                foreach (IPAddress ip in arrIPAddresses)
                {
                    if (!ip.ToString().Contains(":"))
                    {
                        ipsStr = count == 0 ? ip.ToString() : ipsStr + ", " + ip.ToString();
                        count++;
                    }
                }
                return ipsStr;
            }
            catch
            {
                return string.Empty;
            }
        }


    }
}