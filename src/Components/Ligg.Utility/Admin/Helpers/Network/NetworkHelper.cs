using System;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using Ligg.Base.Extension;

namespace Ligg.Utility.Admin.Helpers.Network
{
    public static class NetworkHelper
    {
        //network
        public static string GetMacAddresses()
        {
            try
            {
                string macAddresses = "";
                var mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                int count = 0;
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        var macAddress = mo["MacAddress"].ToString();
                        macAddresses = count == 0 ? macAddress : macAddresses + " \n" + macAddress;
                    }

                    mo.Dispose();
                }
                return macAddresses;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        [DllImport("Iphlpapi.dll")]
        private static extern int SendARP(Int32 dest, Int32 host, ref Int64 mac, ref Int32 length);
        [DllImport("Ws2_32.dll")]
        private static extern Int32 inet_addr(string ip);
        public static string GetRemoteMacByIp(string ip)
        {
            try
            {
                var macInfo = new Int64();
                Int32 destIp = inet_addr(ip); //目的ip
                Int32 len = 6;
                int res = SendARP(destIp, 0, ref macInfo, ref len);
                var macStr = Convert.ToString(macInfo, 16);
                macStr = macStr.AddCharTillLength(12, '0');
                var macsStr = "";
                var strArry = new string[6];
                for (int i = 0; i < 6; i++)
                {
                    strArry[i] = macStr.Substring(2 * i, 2);
                }
                for (int i = 0; i < 6; i++)
                {
                    macsStr = macsStr + strArry[5 - i];
                }
                return macsStr;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GetHostNameByIpByDns(string ip)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(ip);
                return hostEntry.HostName; ;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GetIpByHostNameByDns(string hostName)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
                IPEndPoint ipEndPoint = new IPEndPoint(hostEntry.AddressList[0], 0);
                string ipAddress = ipEndPoint.Address.ToString();
                return ipAddress;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);
        public static bool IsConnected()
        {
            try
            {
                int I = 0;
                bool state = InternetGetConnectedState(out I, 0);
                return state;
            }
            catch
            {
                return false;
            }
        }



        public static bool IsPingSucceeded(string ip)
        {
            try
            {
                var ping = new Ping();
                var pingIp = ip;
                PingReply result = ping.Send(pingIp);
                if (result != null && result.Status == IPStatus.Success)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsTelnetSucceeded(string ip, int port, int timeout)
        {
            try
            {
                var tn = new Telnet(ip, port, timeout);
                if (tn.Connect())
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsInSpecifiedSubnet(string ipPrefixs, char seperator)
        {
            try
            {
                if (!string.IsNullOrEmpty(ipPrefixs))
                {
                    IPAddress[] arrIPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
                    foreach (IPAddress ip in arrIPAddresses)
                    {
                        if (ip.AddressFamily.Equals(AddressFamily.InterNetwork))
                        {
                            var curIp = ip.ToString();
                            var ipPrefixArray = ipPrefixs.Split(seperator);
                            foreach (var v in ipPrefixArray)
                            {
                                if (curIp.Contains(v))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static string GetIps()
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


    }
}
