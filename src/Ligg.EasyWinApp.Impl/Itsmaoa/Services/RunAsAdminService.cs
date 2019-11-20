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
    internal class RunAsAdminService
    {
       
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

    }


}