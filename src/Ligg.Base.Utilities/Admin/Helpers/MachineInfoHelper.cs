using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using Ligg.Base.DataModel;
using Ligg.Base.Extension;
using Ligg.Utility.Admin.Helpers.Network;
using Microsoft.Win32;

namespace Ligg.Utility.Admin.Helpers
{
    public static class MachineInfoHelper
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
        public static string GetMachineInfo(string flag)
        {
            RegistryKey currentVersionKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            RegistryKey centreProcessorKey = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0");
            RegistryKey biosKey = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS");
            try
            {
                switch (flag.ToLower())
                {
                    //##hardware
                    case "mac": return NetworkHelper.GetMacAddresses();
                    case "cpusinfo": return centreProcessorKey.GetValue("ProcessorNameString").ToString();
                    case "cpusfrequency": return centreProcessorKey.GetValue("~MHz").ToString();
                    case "baseboardinfo": return biosKey.GetValue("BaseBoardManufacturer") + "   " + biosKey.GetValue("BaseBoardProduct");
                    //case "assemblybits": return (IntPtr.Size * 8).ToString();
                    //NetBIOSname
                    case "machinename": return Environment.MachineName;

                    //##softare
                    case "ips":
                        return NetworkHelper.GetIps();
                    case "domainname":
                        return System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;


                    case "osinfo"://Microsoft Windows NT 5.2.3790 Service Pack 2
                        return currentVersionKey.GetValue("ProductName").ToString() + "  " + currentVersionKey.GetValue("CurrentVersion").ToString() + " " + currentVersionKey.GetValue("CurrentBuildNumber").ToString();

                    case "osversionname"://Microsoft Windows NT 5.2.3790 Service Pack 2
                        return Environment.OSVersion.ToString();

                    case "osproductname": //Microsoft Windows Server 2003 R2
                        return currentVersionKey.GetValue("ProductName").ToString(); ;

                    case "osversionno": //Os version no, i.g. 5.2 
                        return currentVersionKey.GetValue("CurrentVersion").ToString();

                    case "osversionmainno": //Os version no, i.g. 5 
                        {
                            var str = currentVersionKey.GetValue("CurrentVersion").ToString();
                            if (str.Contains("."))
                                return str.Substring(0, str.IndexOf('.'));
                            else return str;
                        }

                    case "osbuildno": //Os Build Number, i.g. 3790
                        return currentVersionKey.GetValue("CurrentBuildNumber").ToString();

                    case "osbits":
                        {
                            if (Environment.Is64BitOperatingSystem) return "64";
                            return "32";
                        }
                    case "osbitflag":
                        {
                            if (Environment.Is64BitOperatingSystem) return "x64";
                            return "x86";
                        }

                    case "osid":
                        return currentVersionKey.GetValue("ProductId").ToString();

                    case "osculture":
                        return System.Globalization.CultureInfo.InstalledUICulture.Name;
                    //System.Globalization.CultureInfo.InstalledUICulture.NativeName;

                    case "currentuser":
                        return GetCurrentUserName(true);
                    case "registeredowner":
                        return currentVersionKey.GetValue("RegisteredOwner").ToString();

                    //time
                    //**获取系统启动后经过的ms second 数
                    case "timespanafterboot":
                        return Environment.TickCount.ToString();
                    //**s
                    //dirs
                    case "systemdrive"://C:
                        return Environment.ExpandEnvironmentVariables("%SystemDrive%");

                    case "systemrootdirectory": //C:\WINDOWS
                        return currentVersionKey.GetValue("SystemRoot").ToString();

                    case "system32directory": //C:\WINDOWS\system32
                        return Environment.SystemDirectory;

                    case "myprofiledirectory": //win7 C:\Users\chris.li	xp C:\Documents and Settings\Administrator
                        {
                            var tempStr = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                            return tempStr.Substring(0, tempStr.LastIndexOf("\\"));
                            break;
                        }
                    case "mydocumentsdirectory": //win7 C:\Users\chris.li\Documents; xp C:\Documents and Settings\Administrator\My Documents 
                        {
                            return System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        }

                    case "commonappdatadirectory"://win7 C:\ProgramData; xp C:\Documents and Settings\All Users\Application Data
                        {
                            return System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                        }

                    case "localappdatadirectory": //win7 C:\Users\chris.li\AppData\Local; xp C:\Documents and Settings\Administrator\Local Settings\Application Data	
                        {
                            return System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); ;
                            break;
                        }

                    case "roamingappdatadirectory"://win7 C:\Users\chris.li\AppData\Roaming; xp C:\Documents and Settings\Administrator\Application Data 
                        {
                            return System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                        }
                    case "programfilesdirectory":
                        {
                            return System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                        }

                    case "userdomainname"://esselpropack  or machine name
                        return Environment.UserDomainName;

                    case "workingset"://获取映射到进程上下文的物理内存量
                        return Environment.WorkingSet.ToString();

                    default:
                        {
                            throw new ArgumentException("MachineInfo has no flag '" + flag + "'! ");
                            //return string.Empty;
                        }

                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".GetMachineInfo error: " + ex.Message);
                //return string.Empty;
            }
        }


        public static string GetCurrentUserName(bool isOnlyUserName)
        {
            try
            {
                //var mc = new ManagementClass("Win32_ComputerSystem");
                //ManagementObjectCollection moc = mc.GetInstances();
                //foreach (ManagementObject mo in moc)
                //{
                //    str = mo["UserName"].ToString();
                //}

                if (isOnlyUserName) return Environment.UserName;

                var usrName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                //var machineName = Environment.MachineName; netbois name
                //Dns.GetHostName();
                //if (usrName.Contains(machineName))
                //{
                //    usrName = usrName.Replace(machineName + @"\", "");
                //}

                return usrName;//essselpropack\usr.name
            }
            catch
            {
                return string.Empty;
            }
        }



    }



}

