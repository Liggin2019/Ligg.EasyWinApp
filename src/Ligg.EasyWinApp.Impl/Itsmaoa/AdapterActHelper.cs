using System;
using Ligg.Base.Extension;
using Ligg.EasyWinApp.Implementation.Services;

namespace Ligg.EasyWinApp.Implementation
{
    internal static partial class AdapterActHelper
    {
        private static readonly string
            TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        internal static string Act(string funcName, string[] funcParamArray)
        {
            var returnStr = "";
            try
            {
                //#AsAdmin
                if (funcName.ToLower().EndsWith("AsAdmin".ToLower()))
                {
                    returnStr = new RunAsAdminService().RunAsAdmin(funcName, funcParamArray);
                }

                //#Services
                //##NetworkLocationService
                else if (funcName.ToLower() == ("NetworkLocationService".ToLower()))
                {
                    if (funcParamArray[0].ToLower() == "InitData".ToLower())
                    {
                        new NetworkLocationService().InitData();
                    }
                    else if (funcParamArray[0].ToLower() == "InitCurrentNetworkLocation".ToLower())
                    {
                        new NetworkLocationService().InitCurrentNetworkLocation();
                    }
                    else if (funcParamArray[0].ToLower() == "RefreshCurrentNetworkLocation".ToLower())
                    {
                        new NetworkLocationService().RefreshCurrentNetworkLocation();
                    }
                    else if (funcParamArray[0].ToLower() == "UpdateChosenNetworkLocation".ToLower())
                    {
                        new NetworkLocationService().UpdateChosenNetworkLocation(funcParamArray[1]);
                    }

                    else throw new ArgumentException(funcName + " has no param: '" + funcParamArray[0] + "'! ");

                }
                //##ServerConnectionService
                else if (funcName.ToLower() == ("ServerConnectionService".ToLower()))
                {
                    if (funcParamArray[0].ToLower() == "InitServerConnectionStatus".ToLower())
                    {
                        new ServerConnectionService().InitServerConnectionStatus();
                    }
                    else if (funcParamArray[0].ToLower() == "RefreshServerConnectionStatus".ToLower())
                    {
                        new ServerConnectionService().RefreshServerConnectionStatus();
                    }
                    else if (funcParamArray[0].ToLower() == "RefreshPingServerStatus".ToLower())
                    {
                        new ServerConnectionService().RefreshPingServerStatus();
                    }
                    else if (funcParamArray[0].ToLower() == "RefreshTelnetServerStatus".ToLower())
                    {
                        new ServerConnectionService().RefreshTelnetServerStatus();
                    }

                    else throw new ArgumentException(funcName + " has no param: '" + funcParamArray[0] + "'! ");
                }
                //##RunAsAdminAccountService
                else if (funcName.ToLower() == ("RunAsAdminAccountService".ToLower()))
                {
                    if (funcParamArray[0].ToLower() == "InitRunAsAdminAccountStatus".ToLower())
                    {
                        new RunAsAdminAccountService().InitRunAsAdminAccountStatus();
                    }
                    else if (funcParamArray[0].ToLower() == "RefreshRunAsAdminAccountStatus".ToLower())
                    {
                        new RunAsAdminAccountService().RefreshRunAsAdminAccountStatus();
                    }
                    else if (funcParamArray[0].ToLower() == "RefreshCurrentWinIdAsRunAsAdminAccountStatus".ToLower())
                    {
                        new RunAsAdminAccountService().RefreshCurrentWinIdAsRunAsAdminAccountStatus();
                    }
                    else if (funcParamArray[0].ToLower() == "RefreshWin10CompatibilityStatus".ToLower())
                    {
                        new RunAsAdminAccountService().RefreshWin10CompatibilityStatus();
                    }
                    else if (funcParamArray[0].ToLower() == "RepairWin10CompatibilityStatus".ToLower())
                    {
                        new RunAsAdminAccountService().RepairRepairWin10CompatibilityStatus();
                    }
                    else if (funcParamArray[0].ToLower() == "RefreshSeclogonWinServiceStatus".ToLower())
                    {
                        new RunAsAdminAccountService().RefreshSeclogonWinServiceStatus();
                    }
                    else if (funcParamArray[0].ToLower() == "RefreshDefaultRunAsAdminAccountStatus".ToLower())
                    {
                        new RunAsAdminAccountService().RefreshDefaultRunAsAdminAccountStatus();
                    }
                    else if (funcParamArray[0].ToLower() == "RefreshDesignatedRunAsAdminAccountStatus".ToLower())
                    {
                        RunningParams.IsDesignatedRunAsAdminAccountDomainAcct = funcParamArray[1] == "true";
                        RunningParams.DesignatedRunAsAdminAccountName = funcParamArray[2];
                        RunningParams.DesignatedRunAsAdminAccountPassword = funcParamArray[3];
                        new RunAsAdminAccountService().RefreshDesignatedRunAsAdminAccountStatus();
                    }
                    else if (funcParamArray[0].ToLower() == "RepairDefaultRunAsAdminAccountStatus".ToLower())
                    {
                        new RunAsAdminAccountService().RepairDefaultRunAsAdminAccountStatus();
                    }

                    else throw new ArgumentException(funcName + " has no param: '" + funcParamArray[0] + "'! ");
                }

                //##JobService
                else if (funcName.ToLower() == ("JobService".ToLower()))
                {
                    if (funcParamArray[0].ToLower() == "InitCurrentJob".ToLower())
                    {
                        new JobService().InitCurrentJob(Convert.ToInt32(funcParamArray[1]), funcParamArray[2]);
                    }
                    else if (funcParamArray[0].ToLower() == "ClearCurrentJobTasksStatuses".ToLower())
                    {
                        new JobService().ClearCurrentJobTasksStatuses(Convert.ToInt32(funcParamArray[1]), funcParamArray[2]);
                    }

                    else if (funcParamArray[0].ToLower() == "SetCurrentJobCurrentTaskCompleted".ToLower())
                    {
                        new JobService().SetCurrentJobCurrentTaskCompleted(Convert.ToInt32(funcParamArray[1]), funcParamArray[2],Convert.ToInt32(funcParamArray[3]));
                    }

                    else if (funcParamArray[0].ToLower() == "SetCurrentJobCurrentTaskProcessing".ToLower())
                    {
                        new JobService().SetCurrentJobCurrentTaskProcessing(Convert.ToInt32(funcParamArray[1]), funcParamArray[2], Convert.ToInt32(funcParamArray[3]));
                    }
                    else throw new ArgumentException(funcName + " has no param: '" + funcParamArray[0] + "'! ");

                }

                //##WinConfigService
                else if (funcName.ToLower() == ("WinConfigService".ToLower()))
                {
                    if (funcParamArray[0].ToLower() == "InitData".ToLower())
                    {
                        new WinConfigService().InitData();
                    }
                    else if (funcParamArray[0].ToLower() == "RefreshSelectedWinConfigGroups".ToLower())
                    {
                        var idList = funcParamArray[1].ConvertIdsStringToIntegerList<Int32>(',');
                        new WinConfigService().RefreshSelectedWinConfigGroups(idList);
                    }
                    else if (funcParamArray[0].ToLower() == "RefreshWinConfigGroup".ToLower())
                    {
                        new WinConfigService().RefreshWinConfigGroup(Convert.ToInt32(funcParamArray[1]));
                    }

                    else if (funcParamArray[0].ToLower() == "AutoFixSelectedWinConfigGroups".ToLower())
                    {
                        var idList = funcParamArray[1].ConvertIdsStringToIntegerList<Int32>(',');
                        new WinConfigService().AutoFixSelectedWinConfigGroups(idList);
                    }
                    else if (funcParamArray[0].ToLower() == "AutoFixWinConfigGroup".ToLower())
                    {
                        new WinConfigService().RefreshWinConfigGroup(Convert.ToInt32(funcParamArray[1]));
                    }

                    else if (funcParamArray[0].ToLower() == "SaveSelectedWinConfigGroups".ToLower())
                    {
                        var idList = funcParamArray[1].ConvertIdsStringToIntegerList<Int32>(',');
                        new WinConfigService().SaveSelectedWinConfigGroups(idList);
                    }
                    else if (funcParamArray[0].ToLower() == "SaveWinConfigGroup".ToLower())
                    {
                        new WinConfigService().SaveWinConfigGroup(Convert.ToInt32(funcParamArray[1]));
                    }


                    else throw new ArgumentException(funcName + " has no param: '" + funcParamArray[0] + "'! ");

                }
                else if (funcName.ToLower() == ("TestService".ToLower()))
                {
                    if (funcParamArray[0].ToLower() == "Test1".ToLower())
                    {
                        WinConfigServiceData.test = funcParamArray[1];
                    }
                }

                else throw new ArgumentException(" has no funcName: '" + funcName + "'! ");

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Act error: " + ex.Message);
            }

            return returnStr;
        }


    }
}

