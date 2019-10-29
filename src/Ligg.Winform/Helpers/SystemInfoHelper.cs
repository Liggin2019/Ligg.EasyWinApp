using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using Ligg.Base.DataModel;
using Ligg.Base.Extension;
using Microsoft.Win32;

namespace Ligg.Winform.Helpers
{
    public static class SystemInfoHelper
    {
        public static string GetSystemInfo(string flag)
        {
            RegistryKey currentVersionKey =
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            RegistryKey centreProcessorKey =
                Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0");
            RegistryKey biosKey = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS");
            try
            {
                switch (flag.ToLower())
                {
                    //NetBIOSname
                    case "machinename": return Environment.MachineName;

                    //##softare
                    case "ips":
                        {
                            IPAddress[] arrIPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
                            var ipsStr = "";
                            int count = 0;
                            foreach (IPAddress ip in arrIPAddresses)
                            {
                                if (!ip.ToString().Contains(":"))
                                {
                                    ipsStr = count == 0 ? ip.ToString() : ipsStr + ", " + ip.ToString();
                                    count++;
                                }
                            }

                            return ipsStr;
                        }

                    case "osinfo": //Microsoft Windows NT 5.2.3790 Service Pack 2
                        return currentVersionKey.GetValue("ProductName").ToString() + "  " +
                               currentVersionKey.GetValue("CurrentVersion").ToString() + " " +
                               currentVersionKey.GetValue("CurrentBuildNumber").ToString();


                    case "osbits":
                        {
                            if (Environment.Is64BitOperatingSystem) return "64";
                            return "32";
                        }

                    case "currentuser":
                        return Environment.UserName;
                    case "registeredowner":
                        return currentVersionKey.GetValue("RegisteredOwner").ToString();



                    default:
                        return string.Empty;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }





        //Installed Software List
        public static List<InstalledSoftware> GetInstalledSoftwareList(List<ValueText> nameVesionList)
        {
            var retList = new List<InstalledSoftware>();
            try
            {
                var list = GetInstalledSoftwareList();
                if (nameVesionList == null) return list;
                var addedList = new List<InstalledSoftware>();
                foreach (var v in nameVesionList)
                {
                    var version = v.Text;
                    if (version.IsNullOrEmpty())
                    {
                        var sftwares = list.FindAll(x => x.ProductName.Contains(v.Value));
                        if (sftwares.Count == 0) continue;
                        else addedList.AddRange(sftwares);
                    }
                    else
                    {
                        var sftwares = list.FindAll(x => x.ProductName.Contains(v.Value) & x.VersionString == version);
                        if (sftwares.Count == 0) continue;
                        else addedList.AddRange(sftwares);
                    }
                }

                retList = addedList.Distinct().ToList();
            }
            catch
            {
                return retList;
            }

            return retList;
        }

        private static List<InstalledSoftware> GetInstalledSoftwareList()
        {
            var list = new List<InstalledSoftware>();
            var installedSoftwareListFrMsi = GetMsiInstalledSoftwareList();
            var installedSoftwareListFrReg = GetInstalledSoftwareListFromRegistry();

            for (int i = 0; i < installedSoftwareListFrReg.Count; i++)
            {
                for (int j = i + 1; j < installedSoftwareListFrReg.Count; j++)
                {
                    if (installedSoftwareListFrReg[i].ProductName == installedSoftwareListFrReg[j].ProductName
                        && installedSoftwareListFrReg[i].Bits == installedSoftwareListFrReg[j].Bits
                        && installedSoftwareListFrReg[i].VersionString == installedSoftwareListFrReg[j].VersionString
                        && installedSoftwareListFrReg[i].Publisher == installedSoftwareListFrReg[j].Publisher
                    )
                    {
                        installedSoftwareListFrReg.RemoveAt(
                            installedSoftwareListFrReg.LastIndexOf(installedSoftwareListFrReg[i]));
                        j--;
                    }
                }
            }

            foreach (var v in installedSoftwareListFrReg)
            {
                var msiSf = installedSoftwareListFrMsi.Find(x => x.ProductName == v.ProductName);
                if (msiSf != null)
                {
                    v.Publisher = string.IsNullOrEmpty(msiSf.Publisher) ? msiSf.Publisher : v.Publisher;
                    v.VersionString = string.IsNullOrEmpty(msiSf.VersionString) ? msiSf.VersionString : v.VersionString;
                }

                list.Add(v);
            }

            foreach (var v in installedSoftwareListFrMsi)
            {
                var regSf = installedSoftwareListFrReg.Find(x => x.ProductName == v.ProductName);
                if (regSf == null)
                {
                    list.Add(v);
                }
            }

            return list;
        }


        [DllImport("msi.dll", CharSet = CharSet.Unicode)]
        static extern int MsiGetProductInfo(string szProduct, string szProperty, StringBuilder lpValueBuf,
            ref int pcchValueBuf);

        [DllImport("msi.dll", SetLastError = true)]
        static extern int MsiEnumProducts(int iProductIndex, StringBuilder lpProductBuf);

        private static List<InstalledSoftware> GetMsiInstalledSoftwareList()
        {
            var list = new List<InstalledSoftware>();
            try
            {
                int index = 0;
                var productGuidStrBlder = new StringBuilder(39);
                while (0 == MsiEnumProducts(index++, productGuidStrBlder))
                {
                    var installedSoftware = new InstalledSoftware();
                    //InstallLocation 1024
                    foreach (string property in new string[] { "ProductName", "Publisher", "VersionString" })
                    {
                        int charCount = 512;
                        var value = new StringBuilder(charCount);
                        if (MsiGetProductInfo(productGuidStrBlder.ToString(), property, value, ref charCount) == 0)
                        {
                            value.Length = charCount;
                            if (property == "ProductName")
                            {
                                installedSoftware.ProductName = value.ToString().Trim();
                            }
                            else if (property == "Publisher")
                            {
                                installedSoftware.Publisher = value.ToString();
                            }
                            else if (property == "VersionString")
                            {
                                installedSoftware.VersionString = value.ToString();
                            }
                        }
                    }

                    list.Add(installedSoftware);
                }
            }
            catch
            {
                return list;
            }

            return list;
        }

        private static List<InstalledSoftware> GetInstalledSoftwareListFromRegistry()
        {
            var list = new List<InstalledSoftware>();
            const string softwareKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            var osbit = GetSystemInfo("osbit");
            if (osbit == "64")
            {
                var view32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                using (var key = view32.OpenSubKey(softwareKey, false))
                {
                    foreach (string keyName in key.GetSubKeyNames())
                    {
                        using (RegistryKey subkey = key.OpenSubKey(keyName))
                        {
                            if (subkey != null)
                            {
                                var sfInfo = GetInstalledSoftwareInfoByRegistry(subkey, 64);
                                if (sfInfo != null)
                                {
                                    list.Add(sfInfo);
                                }
                            }
                        }
                    }
                }

                var view64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                using (var key = view64.OpenSubKey(softwareKey, false))
                {
                    foreach (string keyName in key.GetSubKeyNames())
                    {
                        using (RegistryKey subkey = key.OpenSubKey(keyName))
                        {
                            if (subkey != null)
                            {
                                var sfInfo = GetInstalledSoftwareInfoByRegistry(subkey, 64);
                                if (sfInfo != null)
                                {
                                    list.Add(sfInfo);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                using (var key = Registry.LocalMachine.OpenSubKey(softwareKey, false))
                {
                    foreach (string keyName in key.GetSubKeyNames())
                    {
                        using (RegistryKey subkey = key.OpenSubKey(keyName))
                        {
                            if (subkey != null)
                            {
                                var sfInfo = GetInstalledSoftwareInfoByRegistry(subkey, 32);
                                if (sfInfo != null)
                                {
                                    list.Add(sfInfo);
                                }
                            }
                        }
                    }
                }

            }

            return list;
        }

        private static InstalledSoftware GetInstalledSoftwareInfoByRegistry(RegistryKey key, int bits)
        {
            var installedSoftware = new InstalledSoftware();
            try
            {
                if (key.GetValue("DisplayName") != null)
                {
                    var displayName = key.GetValue("DisplayName").ToString();
                    if (!string.IsNullOrEmpty(displayName))
                    {
                        installedSoftware.Bits = bits;
                        installedSoftware.ProductName = displayName.Trim();
                        if (key.GetValue("Publisher") != null)
                            installedSoftware.Publisher = key.GetValue("Publisher").ToString();
                        if (key.GetValue("DisplayVersion") != null)
                            installedSoftware.VersionString = key.GetValue("DisplayVersion").ToString();
                        if (key.GetValue("InstallLocation") != null)
                            installedSoftware.InstallLocation = key.GetValue("InstallLocation").ToString();
                        if (key.GetValue("InstallDate") != null)
                            installedSoftware.InstallDateString = key.GetValue("InstallDate").ToString();
                        if (key.GetValue("Size") != null)
                            installedSoftware.Size = Convert.ToInt32(key.GetValue("Size").ToString());
                        return installedSoftware;
                    }

                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public class InstalledSoftware
        {
            public string ProductName;
            public string Publisher;
            public string VersionString;
            public string InstallLocation;
            public string InstallDateString;
            public int Bits;
            public int Size;
        }



    }
}

