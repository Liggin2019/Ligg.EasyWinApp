using System;
using System.Collections.Generic;
using System.Globalization;
using Ligg.Base.DataModel;
using Ligg.Base.DataModel.Enums;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
using Ligg.EasyWinApp.Implementation.Services;

namespace Ligg.EasyWinApp.Implementation
{
    internal static class AdapterGetHelper
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
        internal static string GetText(string funcName, string[] funcParamArray)
        {
            try
            {
                //#extended from LayoutParser
                if (funcName.ToLower() == "GetValidationResult".ToLower())
                {
                    return new ValidationService().GetValidationResult(funcParamArray[0], funcParamArray[1]);
                }

                //#RunningParam
                else if (funcName.ToLower().ToLower() == "RunningParam".ToLower().ToLower())
                {
                    if (funcParamArray[0].ToLower() == "AssemblyBits".ToLower())
                    {
                        var assemblyBits = RunningParams.AssemblyBits;
                        return assemblyBits.ToString();
                    }

                    else if (funcParamArray[0].ToLower() == "CurrentNetworkLocation".ToLower())
                    {
                        if (funcParamArray[1].ToLower() == "ShortName".ToLower())
                        {
                            return RunningParams.CurrentNetworkLocation.ShortName;
                        }
                        else if (funcParamArray[1].ToLower() == "ServerAddress".ToLower())
                        {
                            return RunningParams.CurrentNetworkLocation.ServerAddress;

                        }
                        else if (funcParamArray[1].ToLower() == "ServerPort".ToLower())
                        {
                            return RunningParams.CurrentNetworkLocation.ServerPort.ToString();
                        }
                        else if (funcParamArray[1].ToLower() == "MonitoredNetworkAddresses".ToLower())
                        {
                            return RunningParams.CurrentNetworkLocation.MonitoredNetworkAddresses.ToString();
                        }
                        else throw new ArgumentException("funcName: " + funcName + " has no param '" + funcParamArray[0] + "/" + funcParamArray[1] + "'! ");
                    }

                    else if (funcParamArray[0].ToLower() == "ChosenNetworkLocation".ToLower())
                    {
                        if (funcParamArray[1].ToLower() == "ShortName".ToLower())
                        {
                            return RunningParams.ChosenNetworkLocation.ShortName;
                        }
                        else if (funcParamArray[1].ToLower() == "MonitoredNetworkAddresses".ToLower())
                        {
                            return RunningParams.ChosenNetworkLocation.MonitoredNetworkAddresses.ToString();
                        }
                        else if (funcParamArray[1].ToLower() == "MonitoredServerAddresses".ToLower())
                        {
                            return RunningParams.ChosenNetworkLocation.MonitoredServerAddresses.ToString();
                        }
                        else throw new ArgumentException("funcName: " + funcName + " has no param '" + funcParamArray[0] + "/" + funcParamArray[1] + "'! ");
                    }

                    //##ServerConnectionStatus
                    else if (funcParamArray[0].ToLower() == "ServerConnectionStatus".ToLower())
                    {
                        return ((int)RunningParams.ServerConnectionStatus).ToString();
                    }

                    //##RunAsAdminAccountStatus
                    else if (funcParamArray[0].ToLower() == "RunAsAdminAccountStatus".ToLower())
                    {
                        return ((int)RunningParams.RunAsAdminAccountStatus).ToString();
                    }
                    else if (funcParamArray[0].ToLower() == "CurrentRunAsAdminAccountName".ToLower())
                    {
                        var raaAcctDomain = RunningParams.CurrentRunAsAdminAccountDomain;
                        var raaAcctName = RunningParams.CurrentRunAsAdminAccountName;
                        return raaAcctDomain.IsNullOrEmpty() ? raaAcctName : raaAcctDomain + @"\" + raaAcctName;
                    }


                    else throw new ArgumentException("funcName: " + funcName + " has no param " + funcParamArray[0] + "! ");
                }

                //#CommonService
                else if (funcName.ToLower() == ("CommonService".ToLower()))
                {
                    if (funcParamArray[0].ToLower() == "MachineInfo".ToLower())
                    {
                        return new CommonService().GetMachineInfo(funcParamArray[1]);
                    }
                    else if (funcParamArray[0].ToLower() == "NetworkInfo".ToLower())
                    {
                        return new CommonService().GetNetworkInfo(funcParamArray[1], funcParamArray[2]);
                    }
                    else throw new ArgumentException("funcName: " + funcName + " has no param '" + funcParamArray[0] + "'! ");
                }

                //#NetworkLocationService
                if (funcName.ToLower().ToLower() == "NetworkLocationService".ToLower().ToLower())
                {
                    if (funcParamArray[0].ToLower() == "NetworkLocations".ToLower())
                    {
                        var networkLocations = NetworkLocationServiceData.NetworkLocations;
                        var valTxts = new List<ValueText>();
                        foreach (var networkLocation in networkLocations)
                        {
                            var valTxt = new ValueText(networkLocation.ShortName, networkLocation.Name);
                            valTxts.Add(valTxt);
                        }

                        var str = ObjectHelper.ConvertToJson(valTxts);
                        return str;
                    }
                    else if (funcParamArray[0].ToLower() == "PingServerStatus".ToLower())
                    {
                        return ((int)ServerConnectionServiceData.PingServerStatus).ToString();
                    }
                    else if (funcParamArray[0].ToLower() == "TelnetServerStatus".ToLower())
                    {
                        return ((int)ServerConnectionServiceData.TelnetServerStatus).ToString();
                    }

                    else throw new ArgumentException("funcName: " + funcName + " has no param '" + funcParamArray[0] + "'! ");
                }

                else if (funcName.ToLower().ToLower() == "ServerConnectionService".ToLower().ToLower())
                {
                    if (funcParamArray[0].ToLower() == "PingServerStatus".ToLower())
                    {
                        return ((int)ServerConnectionServiceData.PingServerStatus).ToString();
                    }
                    else if (funcParamArray[0].ToLower() == "TelnetServerStatus".ToLower())
                    {
                        return ((int)ServerConnectionServiceData.TelnetServerStatus).ToString();
                    }

                    else throw new ArgumentException("funcName: " + funcName + " has no param '" + funcParamArray[0] + "'! ");
                }
                else if (funcName.ToLower().ToLower() == "RunAsAdminAccountService".ToLower().ToLower())
                {
                    if (funcParamArray[0].ToLower() == "CurrentWinIdAsRunAsAdminAccountStatus".ToLower())
                    {
                        return ((int)RunAsAdminAccountServiceData.CurrentWinIdAsRunAsAdminAccountStatus).ToString();
                    }
                    else if (funcParamArray[0].ToLower() == "Win10CompatibilityStatus".ToLower())
                    {
                        return ((int)RunAsAdminAccountServiceData.Win10CompatibilityStatus).ToString();
                    }
                    else if (funcParamArray[0].ToLower() == "SeclogonWinServiceStatus".ToLower())
                    {
                        return ((int)RunAsAdminAccountServiceData.SeclogonWinServiceStatus).ToString();
                    }
                    else if (funcParamArray[0].ToLower() == "DefaultRunAsAdminAccountStatus".ToLower())
                    {
                        var retStr = (RunAsAdminAccountServiceData.DefaultRunAsAdminAccountStatus).ToString(CultureInfo.InvariantCulture);
                        return retStr;
                    }
                    else if (funcParamArray[0].ToLower() == "DesignatedRunAsAdminAccountStatus".ToLower())
                    {
                        var retStr = (RunAsAdminAccountServiceData.DesignatedRunAsAdminAccountStatus).ToString(CultureInfo.InvariantCulture);
                        return retStr;
                    }
                    else if (funcParamArray[0].ToLower() == "SeclogonWinServiceStatus".ToLower())
                    {
                        return ((int)RunAsAdminAccountServiceData.SeclogonWinServiceStatus).ToString();
                    }

                    else throw new ArgumentException("funcName: " + funcName + " has no param '" + funcParamArray[0] + "'! ");
                }

                //#UserService
                else if (funcName.ToLower() == ("UserService".ToLower()))
                {
                    if (funcParamArray[0].ToLower() == "Logon".ToLower())
                    {
                        var tmpArray = funcParamArray[1].Split(funcParamArray[1].GetSubParamSeparator());
                        var result = new UserService().Logon(tmpArray[0], tmpArray[1]);
                        return result.IsOk ? "true" : "Logon error";
                    }
                    else throw new ArgumentException("funcName: " + funcName + " has no param '" + funcParamArray[0] + "'! ");
                }

                //#JobService
                else if (funcName.ToLower() == ("JobService".ToLower()))
                {
                    if (funcParamArray[0].ToLower() == "GetJobExecType".ToLower())
                    {
                        return new JobService().GetJobExecType(Convert.ToInt32(funcParamArray[1])).ToString();
                    }
                    else if (funcParamArray[0].ToLower() == "GetJobExecMode".ToLower())
                    {
                        return new JobService().GetJobExecMode(Convert.ToInt32(funcParamArray[1])).ToString();
                    }
                    else if (funcParamArray[0].ToLower() == "GetJobExecParams".ToLower())
                    {
                        return new JobService().GetJobExecParams(Convert.ToInt32(funcParamArray[1]));
                    }
                    else if (funcParamArray[0].ToLower() == "GetJobDisplayName".ToLower())
                    {
                        return new JobService().GetJobDisplayName(Convert.ToInt32(funcParamArray[1]));
                    }

                    else if (funcParamArray[0].ToLower() == "GetJobTaskCount".ToLower())
                    {
                        return (new JobService().GetJobTaskCount(Convert.ToInt32(funcParamArray[1]), funcParamArray[2])).ToString();
                    }
                    else if (funcParamArray[0].ToLower() == "GetCurrentJobTaskList".ToLower())
                    {
                        return new JobService().GetCurrentJobTaskList(Convert.ToInt32(funcParamArray[1]), funcParamArray[2]);
                    }
                    else if (funcParamArray[0].ToLower() == "GetJobCurrentTaskAction".ToLower())
                    {
                        return new JobService().GetJobCurrentTaskAction(Convert.ToInt32(funcParamArray[1]), funcParamArray[2], Convert.ToInt32(funcParamArray[3]));
                    }

                    else throw new ArgumentException("funcName: " + funcName + " has no param '" + funcParamArray[0] + "'! ");

                }
                //#WinChgConfigGroupService
                else if (funcName.ToLower() == ("WinChgConfigGroupService".ToLower()))
                {
                    if (funcParamArray[0].ToLower() == "GetProperty".ToLower())
                    {
                        return new WinChgConfigGroupService().GetProperty(Convert.ToInt16(funcParamArray[1]), funcParamArray[2]);
                    }
                    else if (funcParamArray[0].ToLower() == "GetStatus".ToLower())
                    {
                        return new WinChgConfigGroupService().GetStatus(Convert.ToInt16(funcParamArray[1])).ToString();
                    }
                    else throw new ArgumentException("funcName: " + funcName + " has no param '" + funcParamArray[0] + "'! ");
                }

                else throw new ArgumentException(" has no funcName " + funcName + "! ");

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".GetText error: " + ex.Message);
            }
        }


    }
}
