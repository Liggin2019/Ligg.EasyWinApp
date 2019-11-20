using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
using Ligg.Base.Handlers;
using Ligg.EasyWinForm.Resources;
using Ligg.Winform;
using Ligg.Winform.DataModel;
using Ligg.Winform.DataModel.Enums;
using Ligg.Winform.Dialogs;
using Ligg.Winform.Helpers;
using Ligg.EasyWinApp.Common;
using Ligg.EasyWinApp.ImplInterface;

namespace Ligg.EasyWinForm
{
    internal class BootStrap
    {
        internal string DefaultCultureName = "";
        internal string StartUpDir = "";
        private string _appDir = "";
        private string _formDir = "";
        private string _zonesDir = "";
        private string _startFuncLocation = "";
        internal string StartZoneLocation = "";
        internal static ApplicationStartParamSet ApplicationStartParamSet;

        internal BootStrap()
        {
            GlobalConfiguration.ReadParams();
            EncryptionHelper.Key1 = GlobalConfiguration.GlobalKey2;
            EncryptionHelper.Key2 = GlobalConfiguration.GlobalKey2;
        }

        internal void SetPaths(FunctionFormType formType, string appCode, string funcCodeOrZoneLoc)
        {
            var executableDir = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            var StartUpDir = Directory.GetParent(executableDir).ToString();
            Directory.SetCurrentDirectory(DirectoryHelper.DeleteLastSlashes(StartUpDir));
            //StartUpDir = DirectoryHelper.DeleteLastSlashes(Directory.GetCurrentDirectory());

            _appDir = StartUpDir + "\\Applications\\" + appCode;
            _formDir = StartUpDir + "\\Applications\\" + appCode + "\\Form";
            _zonesDir = _formDir + "\\Zones";
            if (formType == FunctionFormType.MutiView)
                _startFuncLocation = _formDir + "\\Functions\\" + funcCodeOrZoneLoc;
            else StartZoneLocation = FileHelper.GetFilePath(funcCodeOrZoneLoc, _zonesDir);
        }

        //#set
        internal void SetCultures(string passedCultureName)
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

                var cultureName = DefaultCultureName;
                if (!passedCultureName.IsNullOrEmpty() && CultureHelper.IsCultureNameValid(passedCultureName))
                {
                    cultureName = passedCultureName;
                }
                CultureHelper.SetCurrentCulture(cultureName);

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".SetCultures Error: " + ex.Message);
            }
        }

        internal void SetApplicationStartParamSet(FunctionFormType formType)
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

                applicationStartParamSet.VerifyPasswordAtStart = functionStartParamSet.VerifyPasswordAtStart;
                applicationStartParamSet.PasswordVerificationRule = functionStartParamSet.PasswordVerificationRule;

                applicationStartParamSet.ShowSoftwareCoverAtStart = functionStartParamSet.ShowSoftwareCoverAtStart ? functionStartParamSet.ShowSoftwareCoverAtStart : applicationStartParamSet.ShowSoftwareCoverAtStart;
                applicationStartParamSet.LogonAtStart = functionStartParamSet.LogonAtStart ? functionStartParamSet.LogonAtStart : applicationStartParamSet.LogonAtStart;

                ApplicationStartParamSet = applicationStartParamSet;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".SetApplicationStartParamSet Error: " + ex.Message);
            }
        }

        //#act
        internal bool VerifyStartPassword(string passwordVerificationStr, string password)
        {
            try
            {
                if (!passwordVerificationStr.IsNullOrEmpty())
                {
                    if (passwordVerificationStr.Contains("；")) passwordVerificationStr = passwordVerificationStr.Replace("；", ";");
                    if (passwordVerificationStr.Contains("，")) passwordVerificationStr = passwordVerificationStr.Replace("，", ",");
                    var accessType = passwordVerificationStr.SplitByTwoDifferentStrings("AccessType:", ";", true)[0];
                    var verifyRule = passwordVerificationStr.SplitByTwoDifferentStrings("VerificationRule:", ";", true)[0];
                    var verifyParams = passwordVerificationStr.SplitByTwoDifferentStrings("VerificationParams:", ";", true)[0];

                    if (verifyRule.ToLower() != "ClearText".ToLower() & verifyRule.ToLower() != "TdePassword".ToLower() & verifyRule.ToLower() != "Password".ToLower())
                        throw new ArgumentException("verifyRule is not correct!");

                    if (accessType.ToLower() == "Manual".ToLower())
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
                throw new ArgumentException("\n>> " + GetType().FullName + ".VerifyStartPassword Error: " + ex.Message);
            }
            return true;

        }

        internal bool CheckApplicationUsability(ApplicationStartParamSet appStartParamSet)
        {
            try
            {
                var msg = "";
                if (appStartParamSet.CheckHostingLocation)
                {
                    msg = "Verifying Assembly Hosting Location";

                    if (!StartHelper.CheckHostingLocation(appStartParamSet.HostingServers, StartUpDir))
                    {
                        PopupMessage.PopupError(EasyWinAppRes.ApplicationStartError, EasyWinAppRes.ApplicationStartError + ": " + msg + " " + "Error");
                        return false;
                    }
                }

                if (appStartParamSet.CheckLicenseAvailability)
                {
                    msg = "Checking License Availability";

                    if (!StartHelper.CheckLicenseAvailability())
                    {
                        PopupMessage.PopupError(EasyWinAppRes.ApplicationStartError, EasyWinAppRes.ApplicationStartError + ": " + msg + " " + "Error");
                        return false;
                    }
                }

                if (appStartParamSet.CheckPublishmentValidity)
                {
                    msg = "Checking publishment Validity";


                    if (!StartHelper.CheckPublishmentValidity())
                    {
                        PopupMessage.PopupError(EasyWinAppRes.ApplicationStartError,
                            EasyWinAppRes.ApplicationStartError + ": " + msg + " " + "Error");
                        return false;
                    }
                }

                if (appStartParamSet.CheckSoftwareVersion)
                {
                    msg = "Checking Software Version";


                    if (!StartHelper.CheckSoftwareVersion())
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

        public void InitGlobalConfiguration(string applicationCode, bool supportMutiCultures, string defaultLanguageCode, string currentLanguageCode, string startParams, string implementationDir)
        {
            GlobalConfiguration.AppCode = applicationCode;
            GlobalConfiguration.SupportMutiCultures = supportMutiCultures;
            GlobalConfiguration.DefaultLanguageCode = defaultLanguageCode;
            GlobalConfiguration.CurrentLanguageCode = currentLanguageCode;
            GlobalConfiguration.StartParams = startParams.Split(startParams.GetSubParamSeparator());
            GlobalConfiguration.ImplementationDir = implementationDir;
        }

        internal void ShowSoftwareCover(FunctionInitParamSet mainFunctionInitParamSet)
        {
            var singleViewFormInitParamSet = new FunctionInitParamSet();
            singleViewFormInitParamSet.FormType = FunctionFormType.SingleView;
            singleViewFormInitParamSet.AssemblyCode = mainFunctionInitParamSet.AssemblyCode;
            singleViewFormInitParamSet.ApplicationCode = mainFunctionInitParamSet.ApplicationCode;
            var temArry = ApplicationStartParamSet.SoftwareCoverZoneLocation.SplitByLastSeparator('\\');
            singleViewFormInitParamSet.FunctionCode = temArry.Length == 0 ? temArry[0] : temArry[1];
            singleViewFormInitParamSet.ZoneLocationForNonMutiViewForm = FileHelper.GetFilePath(ApplicationStartParamSet.SoftwareCoverZoneLocation, _zonesDir);
            singleViewFormInitParamSet.InputZoneVariablesForNonMutiViewForm = mainFunctionInitParamSet.FunctionCode;

            singleViewFormInitParamSet.StartParams = string.Empty;
            singleViewFormInitParamSet.StartPassword = string.Empty;
            singleViewFormInitParamSet.HelpdeskEmail = mainFunctionInitParamSet.HelpdeskEmail;
            singleViewFormInitParamSet.ApplicationVersion = mainFunctionInitParamSet.ApplicationVersion;

            singleViewFormInitParamSet.ImplementationDir = mainFunctionInitParamSet.ImplementationDir;
            singleViewFormInitParamSet.SupportMutiCultures = mainFunctionInitParamSet.SupportMutiCultures;
            var form = new StartForm(singleViewFormInitParamSet);
            Application.Run(form);
        }


        internal bool VerifyUserToken(string userCode, string userToken)
        {
            return GlobalConfiguration.VerifyUserToken(userCode, userToken);
        }

        internal bool Logon(FunctionInitParamSet mainFunctionInitParamSet)
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

            singleViewFormInitParamSet.ImplementationDir = mainFunctionInitParamSet.ImplementationDir;
            singleViewFormInitParamSet.SupportMutiCultures = mainFunctionInitParamSet.SupportMutiCultures;

            var form = new StartForm(singleViewFormInitParamSet);
            Application.Run(form);
            if (!form.IsOk) return false;
            return true;
        }



    }
}