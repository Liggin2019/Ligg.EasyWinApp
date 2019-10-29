using System;
using System.IO;
using System.Security.Principal;
using Ligg.Base.DataModel.Enums;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
using Ligg.EasyWinApp.Implementation.DataModel.Enums;
using Ligg.EasyWinApp.Implementation.Helpers;
using Ligg.EasyWinApp.ImplInterface;
using Ligg.Utility.Admin.Helpers;
using Ligg.Utility.Admin.Helpers.Account;

namespace Ligg.EasyWinApp.Implementation.Services
{
    internal class RunAsAdminAccountService
    {
        //##RunAsAdminAccountStatus
        internal void InitRunAsAdminAccountStatus()
        {
            try
            {
                if (RunningParams.RunAsAdminAccountStatus == UniversalStatus.Unknown) RefreshRunAsAdminAccountStatus();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".InitRunAsAdminAccountStatus Error: " + ex.Message);
            }
        }

        internal void RefreshRunAsAdminAccountStatus()
        {
            try
            {
                RefreshCurrentWinIdAsRunAsAdminAccountStatus();
                if (RunningParams.CurrentWinIdAsRunAsAdminAccountStatus == UniversalStatus.Ok)
                {
                    return;
                }
                else
                {
                    RefreshWin10CompatibilityStatus();
                    if (RunningParams.Win10CompatibilityStatus == UniversalStatus.NotOk)
                    {
                        return;
                    }
                    RefreshSeclogonWinServiceStatus();
                    if (RunningParams.SeclogonWinServiceStatus == UniversalStatus.NotOk)
                    {
                        return;
                    }
                    else
                    {
                        RefreshDefaultRunAsAdminAccountStatus();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshRunAsAdminAccountStatus Error: " + ex.Message);
            }
        }


        internal bool JudgeRunAsAdminAccountStatusByNonCurrentWinId()
        {
            if (RunningParams.Win10CompatibilityStatus == UniversalStatus.NotOk)
            {
                return false;
            }
            if (RunningParams.SeclogonWinServiceStatus == UniversalStatus.NotOk)
            {
                return false;
            }
            if (RunningParams.DefaultRunAsAdminAccountStatus == UniversalStatus.Ok | RunningParams.DesignatedRunAsAdminAccountStatus == UniversalStatus.Ok)
            {
                return true;
            }

            return false;
        }

        //##CurrentWinIdAsRunAsAdminAccountStatus
        internal void RefreshCurrentWinIdAsRunAsAdminAccountStatus()
        {
            try
            {
                var domainOfCurWinId = "";
                var accountNameOfCurWinId = "";
                var machineName = Environment.MachineName;
                var currentWinIdentity = WindowsIdentity.GetCurrent();
                var fullAcctNameOfCurWinId = currentWinIdentity.Name;
                var curWinIdInfoArray = fullAcctNameOfCurWinId.Split('\\');
                if (fullAcctNameOfCurWinId.ToLower().StartsWith(machineName.ToLower()))
                {
                    accountNameOfCurWinId = curWinIdInfoArray[1];
                }
                else
                {
                    domainOfCurWinId = curWinIdInfoArray[0];
                    accountNameOfCurWinId = curWinIdInfoArray[1];
                }
                if (NetworkAndSystemHelper.IsWinIdAdmin(domainOfCurWinId, accountNameOfCurWinId, "Administrators", "Domain Admins"))
                {
                    RunningParams.CurrentWinIdAsRunAsAdminAccountStatus = UniversalStatus.Ok;
                    RunningParams.CurrentRunAsAdminAccountDomain = domainOfCurWinId;
                    RunningParams.CurrentRunAsAdminAccountName = "Current Windows Id: " + accountNameOfCurWinId;
                    RunningParams.CurrentRunAsAdminAccountPassword = "";
                }
                else
                {
                    RunningParams.CurrentWinIdAsRunAsAdminAccountStatus = UniversalStatus.NotOk;
                }
                UpdateRunAsAdminAccountStatus();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshCurrentWinIdAsRunAsAdminAccountStatus Error: " + ex.Message);
            }
        }

        //##Win10CompatibilityStatus
        internal void RefreshWin10CompatibilityStatus()
        {
            try
            {
                if (!NetworkAndSystemHelper.IsWin10CompatibilityOk())
                {
                    RunningParams.Win10CompatibilityStatus = UniversalStatus.NotOk;
                }
                else
                {
                    RunningParams.Win10CompatibilityStatus = UniversalStatus.Ok;
                }

                UpdateRunAsAdminAccountStatus();

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshWin10CompatibilityStatus Error: " + ex.Message);
            }
        }

        public void RepairRepairWin10CompatibilityStatus()
        {
            try
            {
                var osVersionMainNo = MachineInfoHelper.GetMachineInfo("OsVersionMainNo");
                var osBits = MachineInfoHelper.GetMachineInfo("OsBits");
                if (osVersionMainNo == "6" && osBits == "64")
                {
                    if (RunningParams.AssemblyBits == 32)
                    {
                        var configUpdatorText = "";
                        var registeredOwner32 = RegistryHelper.GetValue("lm\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\RegisteredOwner", 32);
                        if (registeredOwner32.IsNullOrEmpty())
                        {
                            var registeredOwner64 = RegistryHelper.GetValue("lm\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\RegisteredOwner", 64);
                            configUpdatorText = "Registry;lm;lm\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\RegisteredOwner;" + registeredOwner64 + "\r\n";
                        }
                        var registeredOrganization32 = RegistryHelper.GetValue("lm\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\RegisteredOrganization", 32);
                        if (registeredOrganization32.IsNullOrEmpty())
                        {
                            var registeredOrganization64 = RegistryHelper.GetValue("lm\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\RegisteredOrganization", 64);
                            var configUpdatorText1 =
                                "Registry;lm;lm\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\RegisteredOrganization;" + registeredOrganization64;
                            configUpdatorText = configUpdatorText.IsNullOrEmpty() ? configUpdatorText1 : configUpdatorText + ";" + configUpdatorText1;
                        }
                        if (!configUpdatorText.IsNullOrEmpty())
                        {
                            configUpdatorText = EncryptionHelper.SmEncrypt(configUpdatorText);
                            new WinConfigService().UpdateWinConfig(configUpdatorText);

                        }
                        RefreshWin10CompatibilityStatus();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RepairRepairWin10CompatibilityStatus Error: " + ex.Message);
            }
        }

        //##SeclogonWinServiceStatus
        internal void RefreshSeclogonWinServiceStatus()
        {
            try
            {
                if (!NetworkAndSystemHelper.IsSeclogonWinServiceRunning())
                {
                    RunningParams.SeclogonWinServiceStatus = UniversalStatus.NotOk;
                }
                else
                {
                    RunningParams.SeclogonWinServiceStatus = UniversalStatus.Ok;
                }
                UpdateRunAsAdminAccountStatus();

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshSeclogonWinServiceStatus Error: " + ex.Message);
            }
        }

        //##DefaultRunAsAdminAccountStatus
        internal void RefreshDefaultRunAsAdminAccountStatus()
        {
            try
            {
                var account = "";
                var password = "";
                RunningParams.DefaultRunAsAdminAccountStatus = UniversalStatus.NotOk;
                //domain account
                if (!string.IsNullOrEmpty(Configuration.OrganizationSetting.RunAsAdminDomainAccount))
                {
                    new NetworkLocationService().RefreshNetworkDistance(); //? necessary
                    //local account check
                    if (!string.IsNullOrEmpty(Configuration.OrganizationSetting.RunAsAdminLocalAccount))
                    {
                        account = Configuration.OrganizationSetting.RunAsAdminLocalAccount;
                        var encrptedPassword = Configuration.OrganizationSetting.RunAsAdminLocalAccountPassword;
                        password =encrptedPassword;
                        if (NetworkAndSystemHelper.IsWinIdAdmin("", account, "administrators", "Domain Admins"))
                        {
                            if (LocalAccountHelper.IsValidAccountAndPassword("", account, password))
                            {
                                RunningParams.DefaultRunAsAdminAccountStatus = UniversalStatus.Ok;
                                if (RunningParams.CurrentWinIdAsRunAsAdminAccountStatus == UniversalStatus.NotOk)
                                {
                                    RunningParams.CurrentRunAsAdminAccountDomain = string.Empty;
                                    RunningParams.CurrentRunAsAdminAccountName = account;
                                    RunningParams.CurrentRunAsAdminAccountPassword = password;
                                }
                            }
                        }
                        return;
                    }//local account check end

                    //domain account check
                    if (RunningParams.NetworkDistance != (int)NetworkDistance.Wan & RunningParams.NetworkDistance != NetworkDistance.Unknown)
                    {

                        if (MachineInfoHelper.GetMachineInfo("domainname") == Configuration.OrganizationSetting.FullDomainName.ToLower())
                        {
                            var domain = Configuration.OrganizationSetting.ShortDomainName;
                            account = Configuration.OrganizationSetting.RunAsAdminDomainAccount;
                            var encrptedPassword = Configuration.OrganizationSetting.RunAsAdminDomainAccountPassword;
                            password =encrptedPassword;
                            if (NetworkAndSystemHelper.IsWinIdAdmin(domain, account, "administrators", "Domain Admins"))
                            {
                                if (LocalAccountHelper.IsValidAccountAndPassword(domain, account, password))
                                {
                                    RunningParams.DefaultRunAsAdminAccountStatus = UniversalStatus.Ok;
                                    if (RunningParams.CurrentWinIdAsRunAsAdminAccountStatus == UniversalStatus.NotOk)
                                    {
                                        //RunningParams.CurrentRunAsAdminAccountDomain = domain;
                                        //RunningParams.CurrentRunAsAdminAccountName = account;
                                        //RunningParams.CurrentRunAsAdminAccountPassword = password;
                                    }
                                    if (RunningParams.Win10CompatibilityStatus == UniversalStatus.Ok &&
                                        RunningParams.SeclogonWinServiceStatus == UniversalStatus.Ok)
                                    {
                                        RunningParams.CurrentRunAsAdminAccountDomain = domain;
                                        RunningParams.CurrentRunAsAdminAccountName = account;
                                        RunningParams.CurrentRunAsAdminAccountPassword = password;
                                    }
                                }

                            }
                        }//domain account check ends
                    }

                }
                UpdateRunAsAdminAccountStatus();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshDefaultRunAsAdminAccountStatus Error: " + ex.Message);
            }
        }

        public void RepairDefaultRunAsAdminAccountStatus()
        {
            try
            {
                var configUpdatorText = "LocalAccount;GroupIncludesAccounts;Administrators;" + Configuration.OrganizationSetting.RunAsAdminLocalAccount + "\r\n";
                configUpdatorText = configUpdatorText + "LocalAccount;AccountPassword;" + Configuration.OrganizationSetting.RunAsAdminLocalAccount + ";" + Configuration.OrganizationSetting.RunAsAdminLocalAccountPassword;
                configUpdatorText = EncryptionHelper.SmEncrypt(configUpdatorText);
                if (!configUpdatorText.IsNullOrEmpty())
                {
                    configUpdatorText = EncryptionHelper.SmEncrypt(configUpdatorText);
                    new WinConfigService().UpdateWinConfig(configUpdatorText);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RepairDefaultRunAsAdminAccountStatus Error: " + ex.Message);
            }
        }

        //DesignatedRunAsAdminAccountStatus
        internal void RefreshDesignatedRunAsAdminAccountStatus()
        {
            try
            {
                RunningParams.DesignatedRunAsAdminAccountStatus = UniversalStatus.NotOk;
                var domain = "";
                var account = RunningParams.DesignatedRunAsAdminAccountName;
                var password = RunningParams.DesignatedRunAsAdminAccountPassword;
                if (string.IsNullOrEmpty(account) | string.IsNullOrEmpty(password))
                {
                    return;
                }

                if (RunningParams.IsDesignatedRunAsAdminAccountDomainAcct)
                {
                    new NetworkLocationService().RefreshNetworkDistance(); //? necessary
                    if (RunningParams.NetworkDistance == NetworkDistance.Wan | RunningParams.NetworkDistance == NetworkDistance.Unknown)
                    {
                        return;
                    }

                    if (MachineInfoHelper.GetMachineInfo("domainname") != Configuration.OrganizationSetting.FullDomainName.ToLower())
                    {
                        return;
                    }
                    domain = Configuration.OrganizationSetting.ShortDomainName;
                }

                if (!NetworkAndSystemHelper.IsWinIdAdmin(domain, account, "administrators", "Domain Admins"))
                {
                    return;
                }

                if (LocalAccountHelper.IsValidAccountAndPassword(domain, account, password))
                {
                    RunningParams.DesignatedRunAsAdminAccountStatus = UniversalStatus.Ok;
                    if (RunningParams.CurrentWinIdAsRunAsAdminAccountStatus == UniversalStatus.NotOk)
                    {
                        //RunningParams.CurrentRunAsAdminAccountDomain = domain;
                        //RunningParams.CurrentRunAsAdminAccountName = account;
                        //RunningParams.CurrentRunAsAdminAccountPassword = password;
                    }

                    if (RunningParams.Win10CompatibilityStatus == UniversalStatus.Ok &&
                        RunningParams.SeclogonWinServiceStatus == UniversalStatus.Ok)
                    {
                        RunningParams.CurrentRunAsAdminAccountDomain = domain;
                        RunningParams.CurrentRunAsAdminAccountName = account;
                        RunningParams.CurrentRunAsAdminAccountPassword = password;
                    }

                }
                UpdateRunAsAdminAccountStatus();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshDesignatedRunAsAdminAccountStatus Error: " + ex.Message);
            }
        }

        //##RunAsAdmin
        internal string RunAsAdmin(string funcName, string[] funcParamArray)
        {
            var returnStr = "";
            try
            {

                var isRunAsAdminAccountStatus = RunningParams.RunAsAdminAccountStatus == UniversalStatus.Ok ? true : false;
                if (!isRunAsAdminAccountStatus) throw new ArgumentException(" RunAsAdmin Account Status is not OK" + "! ");
                var actArgsStr = "";
                var inputStr = "";
                var runAsAdimAccount = RunningParams.CurrentRunAsAdminAccountName;
                var runAsAdimAccountPassword = RunningParams.CurrentRunAsAdminAccountPassword;
                var runAsAdimAccountDomain = RunningParams.CurrentRunAsAdminAccountPassword;
                bool isCurWinIdIsAdmin = RunningParams.CurrentWinIdAsRunAsAdminAccountStatus == UniversalStatus.Ok ? true : false;
                if (funcName.ToLower() == "RunAsAdmin".ToLower())
                {
                    if (funcParamArray.Length > 1)
                        actArgsStr = funcParamArray[1];
                    if (isCurWinIdIsAdmin)
                        ProcessHelper.Run(funcParamArray[0], actArgsStr);
                    else ProcessHelper.Run(funcParamArray[0], actArgsStr, runAsAdimAccountDomain, runAsAdimAccount, runAsAdimAccountPassword);
                }
                else if (funcName.ToLower() == ("RunCmdAsAdmin".ToLower()) | funcName.ToLower() == ("ExecCmdAsAdmin".ToLower()))
                {
                    var execCmdModeStr = "0";
                    if (funcName.ToLower() == ("RunCmdAsAdmin".ToLower()))
                    {
                        if (!File.Exists(funcParamArray[0])) throw new ArgumentException("File:'" + funcParamArray[0] + "' does not exist!");
                        if (funcParamArray.Length > 1)
                            actArgsStr = funcParamArray[1];
                        if (funcParamArray.Length > 2)
                        {
                            if (funcParamArray[2].IsPlusInteger())
                                execCmdModeStr = funcParamArray[2];
                        }
                        inputStr = actArgsStr.IsNullOrEmpty() ? funcParamArray[0] : funcParamArray[0] + " " + actArgsStr;
                    }

                    else if (funcName.ToLower() == "ExecCmdAsAdmin".ToLower())
                    {
                        if (funcParamArray.Length > 1)
                        {
                            if (funcParamArray[1].IsPlusInteger())
                                execCmdModeStr = funcParamArray[1];
                        }
                        inputStr = funcParamArray[0];
                    }

                    var execCmdModeInt = (ExecCmdMode)Convert.ToInt16(execCmdModeStr);
                    var execCmdMode = (ExecCmdMode)(execCmdModeInt);
                    if (RunningParams.CurrentRunAsAdminAccountName.StartsWith("Current Windows Id:"))
                        returnStr = ProcessHelper.Cmd(inputStr, execCmdMode);
                    else returnStr = ProcessHelper.Cmd(inputStr, execCmdMode, runAsAdimAccountDomain, runAsAdimAccount, runAsAdimAccountPassword);

                }
                else if (funcName.ToLower() == "OpenFileAsAdmin".ToLower())
                {
                    if (RunningParams.CurrentRunAsAdminAccountName.StartsWith("Current Windows Id:"))
                        ProcessHelper.OpenFile(funcParamArray[0], actArgsStr);
                    else ProcessHelper.OpenFile(funcParamArray[0], actArgsStr, runAsAdimAccountDomain, runAsAdimAccount, runAsAdimAccountPassword);
                }
                else if (funcName.ToLower() == "OpenFolderAsAdmin".ToLower())
                {
                    if (RunningParams.CurrentRunAsAdminAccountName.StartsWith("Current Windows Id:"))
                        ProcessHelper.OpenFolder(funcParamArray[0]);
                    else ProcessHelper.OpenFolder(funcParamArray[0], runAsAdimAccountDomain, runAsAdimAccount, runAsAdimAccountPassword);
                }


                else if (funcName.ToLower() == ("RedirectAsAdmin").ToLower())
                {
                    if (RunningParams.CurrentRunAsAdminAccountName.StartsWith("Current Windows Id:"))
                        ProcessHelper.Redirect(funcParamArray[0]);
                    else ProcessHelper.Redirect(funcParamArray[0], runAsAdimAccountDomain, runAsAdimAccount, runAsAdimAccountPassword);
                }
                else throw new ArgumentException(funcName + " has no param: " + funcParamArray[0] + "! ");
                return returnStr;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RunAsAdmin Error: " + ex.Message);
            }
        }

        //#common
        private void UpdateRunAsAdminAccountStatus()
        {
            try
            {
                RunningParams.RunAsAdminAccountStatus = UniversalStatus.NotOk;
                if (RunningParams.CurrentWinIdAsRunAsAdminAccountStatus == UniversalStatus.Ok)
                {
                    RunningParams.RunAsAdminAccountStatus = UniversalStatus.Ok;
                }
                else if (JudgeRunAsAdminAccountStatusByNonCurrentWinId())
                {
                    RunningParams.RunAsAdminAccountStatus = UniversalStatus.Ok;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".UpdateRunAsAdminAccountStatus Error: " + ex.Message);
            }
        }

    }


}