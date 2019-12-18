using System;
using System.Security.Principal;
using Ligg.Base.DataModel.Enums;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
using Ligg.EasyWinApp.Implementation.DataModel.Enums;
using Ligg.EasyWinApp.Implementation.Helpers;
using Ligg.Utility.Admin.Helpers;
using Ligg.Utility.Admin.Helpers.Account;

namespace Ligg.EasyWinApp.Implementation.Services
{
    internal class RunAsAdminAccountService
    {
        //#Init
        internal void Init()
        {
            try
            {
                new NetworkLocationService().Init();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".Init Error: " + ex.Message);
            }
        }

        //#Refresh
        internal void RefreshRunAsAdminAccountStatus()
        {
            try
            {
                RefreshCurrentWinIdAsRunAsAdminAccountStatus();
                if (RunAsAdminAccountServiceData.CurrentWinIdAsRunAsAdminAccountStatus == UniversalStatus.Ok)
                {
                    return;
                }

                UpdateWin10CompatibilityStatus();
                if (RunAsAdminAccountServiceData.Win10CompatibilityStatus == UniversalStatus.NotOk)
                {
                    return;
                }

                UpdateSeclogonWinServiceStatus();
                if (RunAsAdminAccountServiceData.SeclogonWinServiceStatus == UniversalStatus.NotOk)
                {
                    return;
                }

                RefreshDefaultRunAsAdminAccountStatus();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshRunAsAdminAccountStatus Error: " +
                                            ex.Message);
            }
        }

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

                if (NetworkAndSystemHelper.IsWinIdAdmin(domainOfCurWinId, accountNameOfCurWinId, "Administrators",
                    "Domain Admins"))
                {
                    RunAsAdminAccountServiceData.CurrentWinIdAsRunAsAdminAccountStatus = UniversalStatus.Ok;
                    RunningParams.CurrentRunAsAdminAccountDomain = domainOfCurWinId;
                    RunningParams.CurrentRunAsAdminAccountName = "Current Windows Id: " + accountNameOfCurWinId;
                    RunningParams.CurrentRunAsAdminAccountPassword = "";
                }
                else
                {
                    RunAsAdminAccountServiceData.CurrentWinIdAsRunAsAdminAccountStatus = UniversalStatus.NotOk;
                }

                UpdateRunAsAdminAccountStatus();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName +
                                            ".RefreshCurrentWinIdAsRunAsAdminAccountStatus Error: " + ex.Message);
            }
        }

        internal void UpdateWin10CompatibilityStatus()
        {
            try
            {
                if (!NetworkAndSystemHelper.IsWin10CompatibilityOk())
                {
                    RunAsAdminAccountServiceData.Win10CompatibilityStatus = UniversalStatus.NotOk;
                }
                else
                {
                    RunAsAdminAccountServiceData.Win10CompatibilityStatus = UniversalStatus.Ok;
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshWin10CompatibilityStatus Error: " +
                                            ex.Message);
            }
        }

        public void RepairWin10CompatibilityStatus()
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
                        var registeredOwner32 =
                            RegistryHelper.GetValue(
                                "lm\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\RegisteredOwner", 32);
                        if (registeredOwner32.IsNullOrEmpty())
                        {
                            var registeredOwner64 =
                                RegistryHelper.GetValue(
                                    "lm\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\RegisteredOwner", 64);
                            configUpdatorText =
                                "Registry;lm;lm\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\RegisteredOwner;" +
                                registeredOwner64 + "\r\n";
                        }

                        var registeredOrganization32 = RegistryHelper.GetValue(
                            "lm\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\RegisteredOrganization", 32);
                        if (registeredOrganization32.IsNullOrEmpty())
                        {
                            var registeredOrganization64 = RegistryHelper.GetValue(
                                "lm\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\RegisteredOrganization", 64);
                            var configUpdatorText1 =
                                "Registry;lm;lm\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\RegisteredOrganization;" +
                                registeredOrganization64;
                            configUpdatorText = configUpdatorText.IsNullOrEmpty()
                                ? configUpdatorText1
                                : configUpdatorText + ";" + configUpdatorText1;
                        }

                        if (!configUpdatorText.IsNullOrEmpty())
                        {
                        }

                        UpdateWin10CompatibilityStatus();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName +
                                            ".RepairWin10CompatibilityStatus Error: " + ex.Message);
            }
        }

        internal void UpdateSeclogonWinServiceStatus()
        {
            try
            {
                if (!NetworkAndSystemHelper.IsSeclogonWinServiceRunning())
                {
                    RunAsAdminAccountServiceData.SeclogonWinServiceStatus = UniversalStatus.NotOk;
                }
                else
                {
                    RunAsAdminAccountServiceData.SeclogonWinServiceStatus = UniversalStatus.Ok;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshSeclogonWinServiceStatus Error: " + ex.Message);
            }
        }

        public void RepairSeclogonWinServiceStatus()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RepairSeclogonWinServiceStatus Error: " + ex.Message);
            }
        }

        internal void RefreshDefaultRunAsAdminAccountStatus()
        {
            try
            {
                var account = "";
                var password = "";
                RunAsAdminAccountServiceData.DefaultRunAsAdminAccountStatus = 0;
                //local account check
                if (!string.IsNullOrEmpty(Configuration.OrganizationSetting.RunAsAdminLocalAccount))
                {
                    account = Configuration.OrganizationSetting.RunAsAdminLocalAccount;
                    password = Configuration.OrganizationSetting.RunAsAdminLocalAccountPassword;
                    //password = EncryptionHelper.SmDecrypt(encrptedPassword);
                    if (NetworkAndSystemHelper.IsWinIdAdmin("", account, "administrators", "Domain Admins"))
                    {
                        if (LocalAccountHelper.IsValidAccountAndPassword("", account, password))
                        {
                            RunAsAdminAccountServiceData.DefaultRunAsAdminAccountStatus = 0.5f; //0.5 OK
                                                                                                //UpdateRunAsAdminAccountStatus();
                            if (RunAsAdminAccountServiceData.Win10CompatibilityStatus == UniversalStatus.Ok
                              && RunAsAdminAccountServiceData.SeclogonWinServiceStatus == UniversalStatus.Ok)
                            {
                                RunningParams.CurrentRunAsAdminAccountDomain = string.Empty;
                                RunningParams.CurrentRunAsAdminAccountName = account;
                                RunningParams.CurrentRunAsAdminAccountPassword = password;
                                RunAsAdminAccountServiceData.DefaultRunAsAdminAccountStatus = 1.0f;
                            }
                        }
                    }
                }//local account check ends

                if (RunAsAdminAccountServiceData.DefaultRunAsAdminAccountStatus > 0.5f)
                {
                    UpdateRunAsAdminAccountStatus();
                    return;
                }

                //domain account check
                if (!string.IsNullOrEmpty(Configuration.OrganizationSetting.RunAsAdminDomainAccount))
                {
                    new NetworkLocationService().RefreshNetworkDistance(); //? necessary
                    if (RunningParams.NetworkDistance != (int)NetworkDistance.Wan & RunningParams.NetworkDistance != NetworkDistance.Unknown)
                    {
                        if (MachineInfoHelper.GetMachineInfo("domainname") == Configuration.OrganizationSetting.FullDomainName.ToLower())
                        {
                            var domain = Configuration.OrganizationSetting.ShortDomainName;
                            account = Configuration.OrganizationSetting.RunAsAdminDomainAccount;
                            password = Configuration.OrganizationSetting.RunAsAdminDomainAccountPassword;
                            //password = EncryptionHelper.SmDecrypt(encrptedPassword);
                            if (NetworkAndSystemHelper.IsWinIdAdmin(domain, account, "administrators", "Domain Admins"))
                            {
                                if (LocalAccountHelper.IsValidAccountAndPassword(domain, account, password))
                                {
                                    RunAsAdminAccountServiceData.DefaultRunAsAdminAccountStatus = 0.5f;
                                    if (RunAsAdminAccountServiceData.Win10CompatibilityStatus == UniversalStatus.Ok
                                      && RunAsAdminAccountServiceData.SeclogonWinServiceStatus == UniversalStatus.Ok)
                                    {
                                        RunningParams.CurrentRunAsAdminAccountDomain = domain;
                                        RunningParams.CurrentRunAsAdminAccountName = account;
                                        RunningParams.CurrentRunAsAdminAccountPassword = password;
                                        RunAsAdminAccountServiceData.DefaultRunAsAdminAccountStatus = 1.0f;
                                        UpdateRunAsAdminAccountStatus();
                                    }
                                }
                            }
                        }
                    }
                }//domain account check ends 

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

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RepairDefaultRunAsAdminAccountStatus Error: " + ex.Message);
            }
        }

        internal void RefreshDesignatedRunAsAdminAccountStatus()
        {
            try
            {
                RunAsAdminAccountServiceData.DesignatedRunAsAdminAccountStatus = 0;
                var domain = "";
                var account = RunAsAdminAccountServiceData.DesignatedRunAsAdminAccountName;
                var password = RunAsAdminAccountServiceData.DesignatedRunAsAdminAccountPassword;
                if (string.IsNullOrEmpty(account) | string.IsNullOrEmpty(password))
                {
                    UpdateRunAsAdminAccountStatus();
                    return;
                }

                if (RunAsAdminAccountServiceData.IsDesignatedRunAsAdminAccountDomainAcct)
                {
                    new NetworkLocationService().RefreshNetworkDistance(); //? necessary
                    if (RunningParams.NetworkDistance == NetworkDistance.Wan | RunningParams.NetworkDistance == NetworkDistance.Unknown)
                    {
                        UpdateRunAsAdminAccountStatus();
                        return;
                    }

                    if (MachineInfoHelper.GetMachineInfo("domainname") != Configuration.OrganizationSetting.FullDomainName.ToLower())
                    {
                        UpdateRunAsAdminAccountStatus();
                        return;
                    }
                    domain = Configuration.OrganizationSetting.ShortDomainName;
                }

                if (!NetworkAndSystemHelper.IsWinIdAdmin(domain, account, "administrators", "Domain Admins"))
                {
                    UpdateRunAsAdminAccountStatus();
                    return;
                }

                if (LocalAccountHelper.IsValidAccountAndPassword(domain, account, password))
                {
                    RunAsAdminAccountServiceData.DesignatedRunAsAdminAccountStatus = 0.5f;
                    if (RunAsAdminAccountServiceData.Win10CompatibilityStatus == UniversalStatus.Ok &&
                        RunAsAdminAccountServiceData.SeclogonWinServiceStatus == UniversalStatus.Ok)
                    {
                        RunningParams.CurrentRunAsAdminAccountDomain = domain;
                        RunningParams.CurrentRunAsAdminAccountName = account;
                        RunningParams.CurrentRunAsAdminAccountPassword = password;
                        RunAsAdminAccountServiceData.DesignatedRunAsAdminAccountStatus = 1f;
                    }
                }
                UpdateRunAsAdminAccountStatus();

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshDesignatedRunAsAdminAccountStatus Error: " + ex.Message);
            }
        }


        //#common
        private void UpdateRunAsAdminAccountStatus()
        {
            try
            {
                RunningParams.RunAsAdminAccountStatus = UniversalStatus.NotOk;
                if (RunAsAdminAccountServiceData.CurrentWinIdAsRunAsAdminAccountStatus == UniversalStatus.Ok)
                {
                    RunningParams.RunAsAdminAccountStatus = UniversalStatus.Ok;
                    return;
                }

                if (RunAsAdminAccountServiceData.DefaultRunAsAdminAccountStatus > 0.5 | RunAsAdminAccountServiceData.DesignatedRunAsAdminAccountStatus > 0.5)
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

    internal static class RunAsAdminAccountServiceData
    {
        internal static UniversalStatus CurrentWinIdAsRunAsAdminAccountStatus = UniversalStatus.Unknown;
        internal static UniversalStatus Win10CompatibilityStatus = UniversalStatus.Unknown;
        internal static UniversalStatus SeclogonWinServiceStatus = UniversalStatus.Unknown;
        internal static float DefaultRunAsAdminAccountStatus = -1;
        internal static float DesignatedRunAsAdminAccountStatus = -1;
        internal static bool IsDesignatedRunAsAdminAccountDomainAcct = false;
        internal static string DesignatedRunAsAdminAccountDomain = "";
        internal static string DesignatedRunAsAdminAccountName = "";
        internal static string DesignatedRunAsAdminAccountPassword = "";
    }

}