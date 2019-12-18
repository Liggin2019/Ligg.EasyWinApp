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
                if (funcName.ToLower() == "RunAsAdmin".ToLower() | funcName.ToLower() == "RunCmdAsAdmin".ToLower()
                    | funcName.ToLower() == "ExecCmdAsAdmin".ToLower() | funcName.ToLower() == "OpenFileAsAdmin".ToLower()
                    | funcName.ToLower() == "OpenFolderAsAdmin".ToLower() | funcName.ToLower() == ("RedirectAsAdmin").ToLower())
                {
                    returnStr = new RunAsAdminService().RunAsAdmin(funcName, funcParamArray);
                }

                //#Services
                //##NetworkLocationService
                else if (funcName.ToLower() == ("NetworkLocationService".ToLower()))
                {
                    if (funcParamArray[0].ToLower() == "Init".ToLower())
                    {
                        new NetworkLocationService().Init();
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
                    if (funcParamArray[0].ToLower() == "Init".ToLower())
                    {
                        new ServerConnectionService().Init();
                    }
                    //else if (funcParamArray[0].ToLower() == "InitServerConnectionStatus".ToLower())
                    //{
                    //    new ServerConnectionService().InitServerConnectionStatus();
                    //}
                    else if (funcParamArray[0].ToLower() == "RefreshServerConnectionStatus".ToLower())
                    {
                        new ServerConnectionService().RefreshServerConnectionStatus();
                    }
                    else if (funcParamArray[0].ToLower() == "UpdatePingServerStatus".ToLower())
                    {
                        new ServerConnectionService().UpdatePingServerStatus();
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
                    if (funcParamArray[0].ToLower() == "Init".ToLower())
                    {
                        new RunAsAdminAccountService().Init();
                    }
                    //else if (funcParamArray[0].ToLower() == "InitRunAsAdminAccountStatus".ToLower())
                    //{
                    //    new RunAsAdminAccountService().InitRunAsAdminAccountStatus();
                    //}
                    else if (funcParamArray[0].ToLower() == "RefreshRunAsAdminAccountStatus".ToLower())
                    {
                        new RunAsAdminAccountService().RefreshRunAsAdminAccountStatus();
                    }
                    else if (funcParamArray[0].ToLower() == "RefreshCurrentWinIdAsRunAsAdminAccountStatus".ToLower())
                    {
                        new RunAsAdminAccountService().RefreshCurrentWinIdAsRunAsAdminAccountStatus();
                    }
                    else if (funcParamArray[0].ToLower() == "UpdateWin10CompatibilityStatus".ToLower())
                    {
                        new RunAsAdminAccountService().UpdateWin10CompatibilityStatus();
                    }
                    else if (funcParamArray[0].ToLower() == "UpdateSeclogonWinServiceStatus".ToLower())
                    {
                        new RunAsAdminAccountService().UpdateSeclogonWinServiceStatus();
                    }
                    else if (funcParamArray[0].ToLower() == "RepairWin10CompatibilityStatus".ToLower())
                    {
                        new RunAsAdminAccountService().RepairWin10CompatibilityStatus();
                    }

                    else if (funcParamArray[0].ToLower() == "RefreshDefaultRunAsAdminAccountStatus".ToLower())
                    {
                        new RunAsAdminAccountService().RefreshDefaultRunAsAdminAccountStatus();
                    }
                    else if (funcParamArray[0].ToLower() == "RefreshDesignatedRunAsAdminAccountStatus".ToLower())
                    {
                        RunAsAdminAccountServiceData.IsDesignatedRunAsAdminAccountDomainAcct = funcParamArray[1] == "true";
                        RunAsAdminAccountServiceData.DesignatedRunAsAdminAccountName = funcParamArray[2];
                        RunAsAdminAccountServiceData.DesignatedRunAsAdminAccountPassword = funcParamArray[3];
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
                        new JobService().SetCurrentJobCurrentTaskCompleted(Convert.ToInt32(funcParamArray[1]), funcParamArray[2], Convert.ToInt32(funcParamArray[3]));
                    }

                    else if (funcParamArray[0].ToLower() == "SetCurrentJobCurrentTaskProcessing".ToLower())
                    {
                        new JobService().SetCurrentJobCurrentTaskProcessing(Convert.ToInt32(funcParamArray[1]), funcParamArray[2], Convert.ToInt32(funcParamArray[3]));
                    }
                    else throw new ArgumentException(funcName + " has no param: '" + funcParamArray[0] + "'! ");

                }

                //##WinChgConfigGroupService
                else if (funcName.ToLower() == ("WinChgConfigGroupService".ToLower()))
                {
                    if (funcParamArray[0].ToLower() == "Init".ToLower())
                    {
                        new WinChgConfigGroupService().Init();
                    }
                    else if (funcParamArray[0].ToLower() == "RefreshSelectedItems".ToLower())
                    {
                        var idList = funcParamArray[1].ConvertIdsStringToIntegerList<Int32>(',');
                        new WinChgConfigGroupService().RefreshSelectedItems(idList);
                    }
                    else if (funcParamArray[0].ToLower() == "Refresh".ToLower())
                    {
                        new WinChgConfigGroupService().Refresh(Convert.ToInt32(funcParamArray[1]));
                    }
                    else if (funcParamArray[0].ToLower() == "RepairSelectedItems".ToLower())
                    {
                        var idList = funcParamArray[1].ConvertIdsStringToIntegerList<Int32>(',');
                        new WinChgConfigGroupService().RepairSelectedItems(idList);
                    }
                    else if (funcParamArray[0].ToLower() == "Repair".ToLower())
                    {
                        new WinChgConfigGroupService().Repair(Convert.ToInt32(funcParamArray[1]));
                    }
                    else if (funcParamArray[0].ToLower() == "Refresh".ToLower())
                    {
                        new WinChgConfigGroupService().Refresh(Convert.ToInt32(funcParamArray[1]));
                    }

                    else if (funcParamArray[0].ToLower() == "SaveSelectedItems".ToLower())
                    {
                        var idList = funcParamArray[1].ConvertIdsStringToIntegerList<Int32>(',');
                        new WinChgConfigGroupService().SaveSelectedItems(idList);
                    }
                    else if (funcParamArray[0].ToLower() == "Save".ToLower())
                    {
                        new WinChgConfigGroupService().Save(Convert.ToInt32(funcParamArray[1]));
                    }


                    else throw new ArgumentException(funcName + " has no param: '" + funcParamArray[0] + "'! ");

                }
                else if (funcName.ToLower() == ("TestService".ToLower()))
                {

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

