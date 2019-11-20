using System;
using System.Collections.Generic;
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

                //#runningparams
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
                        else if (funcParamArray[1].ToLower() == "MonitoredServers".ToLower())
                        {
                            return RunningParams.ChosenNetworkLocation.MonitoredNetworkAddresses.ToString();
                        }
                        else throw new ArgumentException("funcName: " + funcName + " has no param '" + funcParamArray[0] + "/" + funcParamArray[1] + "'! ");
                    }

                    //##ServerConnectionStatus
                    else if (funcParamArray[0].ToLower() == "ServerConnectionStatus".ToLower())
                    {
                        return RunningParams.ServerConnectionStatus == UniversalStatus.Unknown ? "Unknown" :
                                    (RunningParams.ServerConnectionStatus == UniversalStatus.Ok ? "true" : "false");
                    }
                    else if (funcParamArray[0].ToLower() == "PingServerStatus".ToLower())
                    {
                        return RunningParams.PingServerStatus == UniversalStatus.Unknown ? "Unknown" :
                            (RunningParams.PingServerStatus == UniversalStatus.Ok ? "true" : "false");
                    }
                    else if (funcParamArray[0].ToLower() == "TelnetServerStatus".ToLower())
                    {
                        return RunningParams.TelnetServerStatus == UniversalStatus.Unknown ? "Unknown" :
                            (RunningParams.TelnetServerStatus == UniversalStatus.Ok ? "true" : "false");
                    }

                    //##RunAsAdminAccountStatus
                    else if (funcParamArray[0].ToLower() == "RunAsAdminAccountStatus".ToLower())
                    {
                        return RunningParams.RunAsAdminAccountStatus == UniversalStatus.Unknown ? "Unknown" :
                            (RunningParams.RunAsAdminAccountStatus == UniversalStatus.Ok ? "true" : "false");
                    }
                    else if (funcParamArray[0].ToLower() == "CurrentRunAsAdminAccountName".ToLower())
                    {
                        var raaAcctDomain = RunningParams.CurrentRunAsAdminAccountDomain;
                        var raaAcctName = RunningParams.CurrentRunAsAdminAccountName;
                        return raaAcctDomain.IsNullOrEmpty() ? raaAcctName : raaAcctDomain + @"\" + raaAcctName;
                    }
                    else if (funcParamArray[0].ToLower() == "CurrentWinIdAsRunAsAdminAccountStatus".ToLower())
                    {
                        return RunningParams.CurrentWinIdAsRunAsAdminAccountStatus == UniversalStatus.Unknown ? "Unknown" :
                            (RunningParams.CurrentWinIdAsRunAsAdminAccountStatus == UniversalStatus.Ok ? "true" : "false");
                    }
                    else if (funcParamArray[0].ToLower() == "Win10CompatibilityStatus".ToLower())
                    {
                        return RunningParams.Win10CompatibilityStatus == UniversalStatus.Unknown ? "Unknown" :
                            (RunningParams.Win10CompatibilityStatus == UniversalStatus.Ok ? "true" : "false");
                    }
                    else if (funcParamArray[0].ToLower() == "SeclogonWinServiceStatus".ToLower())
                    {
                        return RunningParams.SeclogonWinServiceStatus == UniversalStatus.Unknown ? "Unknown" :
                            (RunningParams.SeclogonWinServiceStatus == UniversalStatus.Ok ? "true" : "false");
                    }
                    else if (funcParamArray[0].ToLower() == "DefaultRunAsAdminAccountStatus".ToLower())
                    {
                        return RunningParams.DefaultRunAsAdminAccountStatus == UniversalStatus.Unknown ? "Unknown" :
                            (RunningParams.DefaultRunAsAdminAccountStatus == UniversalStatus.Ok ? "true" : "false");
                    }
                    else if (funcParamArray[0].ToLower() == "DesignatedRunAsAdminAccountStatus".ToLower())
                    {
                        return RunningParams.DesignatedRunAsAdminAccountStatus == UniversalStatus.Unknown ? "Unknown" :
                            (RunningParams.DesignatedRunAsAdminAccountStatus == UniversalStatus.Ok ? "true" : "false");
                    }
                    else if (funcParamArray[0].ToLower() == "xxxx".ToLower())
                    {
                        return RunningParams.Win10CompatibilityStatus == UniversalStatus.Unknown ? "Unknown" :
                            (RunningParams.Win10CompatibilityStatus == UniversalStatus.Ok ? "true" : "false");
                    }
                    else if (funcParamArray[0].ToLower() == "Win10CompatibilityStatus".ToLower())
                    {
                        //return (ApplicationHelper.IsWin10CompatibilityOk()) ? "true" : "false";
                        return "false";
                    }
                    else if (funcParamArray[0].ToLower() == "SeclogonWinServiceStatus".ToLower())
                    {
                        //return (ApplicationHelper.IsSeclogonWinServiceRunning()) ? "true" : "false";
                        return "false";
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

                    else throw new ArgumentException("funcName: " + funcName + " has no param '" + funcParamArray[0] + "'! ");
                }

                //#JobService
                else if (funcName.ToLower() == ("JobService".ToLower()))
                {
                    if (funcParamArray[0].ToLower() == "GetJobExecType".ToLower())
                    {
                        return new JobService().GetJobExecType(Convert.ToInt32(funcParamArray[1]));
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
                        return (new JobService().GetJobTaskCount(Convert.ToInt32(funcParamArray[1]))).ToString();
                    }
                    else if (funcParamArray[0].ToLower() == "GetCurrentJobTaskList".ToLower())
                    {
                        return new JobService().GetCurrentJobTaskList(Convert.ToInt32(funcParamArray[1]), funcParamArray[2]);
                    }
                    else if (funcParamArray[0].ToLower() == "GetJobCurrentTaskAction".ToLower())
                    {
                        return new JobService().GetJobCurrentTaskAction(Convert.ToInt32(funcParamArray[1]), Convert.ToInt32(funcParamArray[2]));
                    }

                    else throw new ArgumentException("funcName: " + funcName + " has no param '" + funcParamArray[0] + "'! ");

                }

                //#WinConfigService
                else if (funcName.ToLower() == ("WinConfigService".ToLower()))
                {
                    if (funcParamArray[0].ToLower() == "GetWinConfigGroupProperty".ToLower())
                    {
                        return new WinConfigService().GetWinConfigGroupProperty(Convert.ToInt16(funcParamArray[1]), funcParamArray[2]);
                    }
                    else if (funcParamArray[0].ToLower() == "GetWinConfigGroupStatus".ToLower())
                    {
                        return new WinConfigService().GetWinConfigGroupStatus(Convert.ToInt16(funcParamArray[1])).ToString();
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

                else if (funcName.ToLower() == ("TestService".ToLower()))
                {
                    if (funcParamArray[0].ToLower() == "Test1".ToLower())
                    {
                        return WinConfigServiceData.test;
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
