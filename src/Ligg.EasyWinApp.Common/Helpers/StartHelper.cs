using System;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Ligg.EasyWinApp.Common.Helpers
{
    public static class StartHelper
    {
        public static bool CheckHostingLocation(string hostingServers, string startUpDir)
        {
            if (string.IsNullOrEmpty(hostingServers)) return true;
            var hostingServerArry = hostingServers.Split(',');
            if (startUpDir.ToLower().StartsWith(("\\\\")))
            {
                return hostingServerArry.Any(x => startUpDir.ToLower().StartsWith(("\\\\" + x).ToLower()));
            }
            else
            {
                var machineName = Environment.MachineName;
                var ips = GetIps().Split(',');
                foreach (var x in hostingServerArry)
                {
                    if (x.ToLower() == machineName.ToLower() | (ips.Contains(x))) return true;
                }
            }
            return false;
        }

        public static bool CheckPublicationValidity()
        {
            return true;
        }

        public static bool CheckLicenseAvailability()
        {
            return true;
        }

        public static bool CheckSoftwareVersion()
        {
            return true;
        }

        public static bool VerifyUserPassword(string userCode, string userPassword)
        {
            return CblpDllAdapter.Adapter.Logon(userCode, userPassword);
        }


        //#common
        private static string GetIps()
        {
            try
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
            catch
            {
                return string.Empty;
            }
        }

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);
        public static string ReadIniString(string filePath, string section, string key, string defVal)
        {
            var buffer = new Byte[65535];
            int bufLen = GetPrivateProfileString(section, key, defVal, buffer, buffer.GetUpperBound(0), filePath);
            string s = Encoding.GetEncoding(0).GetString(buffer);
            s = s.Substring(0, bufLen);
            return s.Trim();
        }


    }
}