using System;
using Ligg.Base.DataModel;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
using Ligg.EasyWinApp.ImplInterface;
using Ligg.Utility.Admin.Helpers;
using Ligg.Utility.Admin.Helpers.Network;

namespace Ligg.EasyWinApp.Implementation.Services
{
    internal class CommonService
    {
        internal string GetMachineInfo(string flag)
        {
            try
            {
                return MachineInfoHelper.GetMachineInfo(flag);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetMachineInfo Error: " + ex.Message);
            }
        }

        internal string GetNetworkInfo(string param, string param1)
        {
            try
            {
                if (param.ToLower() == "GetMacByIp".ToLower())
                {
                    return NetworkHelper.GetRemoteMacByIp(param1);
                }
                else if (param.ToLower() == "GetHostNameByIp".ToLower())
                {
                    return NetworkHelper.GetHostNameByIpByDns(param1);
                }
                else if (param.ToLower() == "GetIpByHostName".ToLower())
                {
                    return NetworkHelper.GetIpByHostNameByDns(param1);
                }
                else throw new ArgumentException("has no param: " + param);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetNetworkInfo Error: " + ex.Message);
            }
        }





    }
}