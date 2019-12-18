using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Ligg.Base.DataModel.Enums;
using Ligg.Base.Handlers;
using Ligg.EasyWinApp.Implementation.DataModel;
using Ligg.EasyWinApp.Implementation.DataModel.Enums;
using Ligg.EasyWinApp.Implementation.Helpers;

namespace Ligg.EasyWinApp.Implementation.Services
{
    internal class NetworkLocationService
    {
        internal void Init()
        {
            try
            {
                InitData();
                if (RunningParams.CurrentNetworkLocationStatus == UniversalStatus.Unknown)
                {
                    RefreshCurrentNetworkLocation();
                    RunningParams.ChosenNetworkLocation = RunningParams.CurrentNetworkLocation;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".InitCurrentNetworkLocation Error: " + ex.Message);
            }
        }

        internal void RefreshCurrentNetworkLocation()
        {
            try
            {
                RefreshNetworkDistance();
                if (RunningParams.NetworkDistance == NetworkDistance.RemoteLan | RunningParams.NetworkDistance == NetworkDistance.CloseLan | RunningParams.NetworkDistance == NetworkDistance.CentralLan)
                {
                    IPAddress[] arrIPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
                    foreach (IPAddress ip in arrIPAddresses)
                    {
                        if (ip.AddressFamily.Equals(AddressFamily.InterNetwork))
                        {
                            var curIp = ip.ToString();
                            var networkLocations = NetworkLocationServiceData.NetworkLocations;
                            foreach (var networkLocation in networkLocations.FindAll(x => !string.IsNullOrEmpty(x.ShortName)))
                            {
                                var ipPrefixArray = networkLocation.IpPrefixes.Split(',');
                                foreach (var v in ipPrefixArray)
                                {
                                    if (curIp.Contains(v))
                                    {
                                        RunningParams.CurrentNetworkLocation = networkLocation;
                                        RunningParams.CurrentNetworkLocationStatus = UniversalStatus.Ok;
                                        return;
                                    }
                                }
                            }

                        }
                    }
                }
                RunningParams.CurrentNetworkLocation = NetworkLocationServiceData.NetworkLocations.Find(x => x.ShortName.ToLower() == "Outside".ToLower());
                RunningParams.CurrentNetworkLocationStatus = UniversalStatus.Ok;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshCurrentNetworkLocation Error: " + ex.Message);
            }
        }

        internal void UpdateChosenNetworkLocation(string networkLocShortName)
        {
            try
            {
                RunningParams.ChosenNetworkLocation = NetworkLocationServiceData.NetworkLocations.Find(x => (x.ShortName).ToLower() == networkLocShortName.ToLower());
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".UpdateChosenNetworkLocation Error: " + ex.Message);
            }
        }

        internal void RefreshNetworkDistance()
        {
            try
            {
                RunningParams.NetworkDistance = NetworkAndSystemHelper.GetNetworkDistance();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshNetworkDistance Error: " + ex.Message);
            }
        }

        private void InitData()
        {
            try
            {
                if (NetworkLocationServiceData.NetworkLocations == null)
                {
                    NetworkLocationServiceData.Init();
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".InitData Error: " + ex.Message);
            }
        }
    }

    internal static class NetworkLocationServiceData
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        internal static List<NetworkLocation> NetworkLocations = null;
        public static void Init()
        {
            try
            {
                var xmlPath = Configuration.DataDir + "\\NetworkLocationsSetting";
                var xmlMgr = new XmlHandler(xmlPath);
                NetworkLocationServiceData.NetworkLocations = xmlMgr.ConvertToObject<List<NetworkLocation>>();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Init Error: " + ex.Message);
            }
        }
    }

}