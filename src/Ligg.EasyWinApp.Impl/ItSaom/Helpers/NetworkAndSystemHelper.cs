using System;
using System.ServiceProcess;
using Ligg.Base.Extension;
using Ligg.EasyWinApp.Implementation.DataModel.Enums;
using Ligg.EasyWinApp.Implementation.Services;
using Ligg.Utility.Admin.Helpers;
using Ligg.Utility.Admin.Helpers.Account;
using Ligg.Utility.Admin.Helpers.Network;

namespace Ligg.EasyWinApp.Implementation.Helpers
{
    public static class NetworkAndSystemHelper
    {
        //act


        //judge
        public static NetworkDistance GetNetworkDistance()
        {
            var status = NetworkDistance.Unknown;
            if (NetworkHelper.IsConnected()) status = NetworkDistance.Wan;
            else return status;
            if (IsInLan())
            {
                if (NetworkHelper.IsInSpecifiedSubnet(Configuration.OrganizationSetting.CentralLanIpPrefixes, ','))
                {
                    status = NetworkDistance.CentralLan;
                }
                else if (NetworkHelper.IsInSpecifiedSubnet(Configuration.OrganizationSetting.CloseLanIpPrefixes, ','))
                {
                    status = NetworkDistance.CloseLan;
                }
                else
                {
                    status = NetworkDistance.RemoteLan;
                }
            }
            return status;
        }

        public static bool IsInLan()
        {
            if (NetworkHelper.GetHostNameByIpByDns(Configuration.OrganizationSetting.MachineIpToCheckIfInLan) == Configuration.OrganizationSetting.MachineNameToCheckIfInLan)
            {
                return true;
                //if (NetworkHelper.IfPingSucceeded(ClientConfiguration.MachineIpForLanToCheckIsInLan))
                //{
                //    return true;
                //}
            }
            return false;
        }

        public static bool IsWinIdAdmin(string acctDomain, string account, string localAdminGrp, string domainAdmiGrp)
        {
            try
            {
                var dcNames = Configuration.OrganizationSetting.DcNames;
                //is non-dc
                if (!dcNames.ToLower().IsIntersectingAfterSplitWithSeparator(MachineInfoHelper.GetMachineInfo("machinename").ToLower(), ';'))
                {
                    var isInAdminGroup = LocalAccountHelper.IsInGroup(acctDomain, account, localAdminGrp);
                    if (isInAdminGroup)
                        return true;
                }
                else//is dc
                {
                    if (Configuration.OrganizationSetting.ShortDomainName == acctDomain & DomainAccountHelper.IsInGroup(Configuration.OrganizationSetting.ShortDomainName, account, "Domain Admins"))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool IsWin10CompatibilityOk()
        {
            var ret = true;
            var osVersionMainNo = MachineInfoHelper.GetMachineInfo("OsVersionMainNo");
            var osBits = MachineInfoHelper.GetMachineInfo("OsBits");
            try
            {
                if (osVersionMainNo == "6" && osBits == "64")
                {
                    if (RunningParams.AssemblyBits == 32)
                    {
                        var isRegValOwnerExsiting = RegistryHelper.IfKeyValueExits("lm\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\RegisteredOwner");
                        var isRegValOrgExsiting = RegistryHelper.IfKeyValueExits("lm\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\RegisteredOrganization");
                        if (!isRegValOwnerExsiting | !isRegValOrgExsiting)
                        {
                            ret = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return ret;
        }

        public static bool IsSeclogonWinServiceRunning()
        {
            var ret = false;
            var servName = "seclogon";
            try
            {
                if (WinServiceHelper.GetStatus(servName) == (int)ServiceControllerStatus.Running
                   //& WinServiceHelper.GetStartMode("seclogon") == (int)ServiceStartMode.Automatic
                   )
                {
                    ret = true;

                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return ret;
        }







    }
}