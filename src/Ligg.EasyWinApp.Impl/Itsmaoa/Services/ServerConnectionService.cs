using System;
using Ligg.Base.DataModel.Enums;
using Ligg.Utility.Admin.Helpers.Network;

namespace Ligg.EasyWinApp.Implementation.Services
{
    internal class ServerConnectionService
    {
        internal void Init()
        {
            try
            {
                new NetworkLocationService().Init();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".Init Error: " +ex.Message);
            }
        }

        internal void RefreshServerConnectionStatus()
        {
            try
            {
                RefreshTelnetServerStatus();
                //UpdatePingServerStatus();
            }
            catch (Exception ex)
            {
                RunningParams.ServerConnectionStatus = UniversalStatus.Unknown;
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshServerConnection Error: " +
                                            ex.Message);
            }
        }

        internal void RefreshTelnetServerStatus()
        {
            try
            {
                if (!NetworkHelper.IsTelnetSucceeded(RunningParams.CurrentNetworkLocation.ServerAddress,
                    RunningParams.CurrentNetworkLocation.ServerPort, 50))
                {
                    ServerConnectionServiceData.TelnetServerStatus = UniversalStatus.NotOk;
                }
                else
                {
                    ServerConnectionServiceData.TelnetServerStatus = UniversalStatus.Ok;
                }

                UpdateServerConnectionStatus();
            }
            catch (Exception ex)
            {
                RunningParams.ServerConnectionStatus = UniversalStatus.Unknown;
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshTelnetServerStatus Error: " +
                                            ex.Message);
            }
        }


        internal void UpdatePingServerStatus()
        {
            try
            {
                if (!NetworkHelper.IsPingSucceeded(RunningParams.CurrentNetworkLocation.ServerAddress))
                {
                    ServerConnectionServiceData.PingServerStatus = UniversalStatus.NotOk;
                }
                else
                {
                    ServerConnectionServiceData.PingServerStatus = UniversalStatus.Ok;
                }
            }
            catch (Exception ex)
            {
                RunningParams.ServerConnectionStatus = UniversalStatus.Unknown;
                throw new ArgumentException("\n>> " + GetType().FullName + ".UpdatePingServerStatus Error: " +
                                            ex.Message);
            }
        }


        private void UpdateServerConnectionStatus()
        {
            try
            {
                //if (RunningParams.PingServerStatus == UniversalStatus.Ok & RunningParams.TelnetServerStatus == UniversalStatus.Ok)
                if (ServerConnectionServiceData.TelnetServerStatus == UniversalStatus.Ok)
                {
                    RunningParams.ServerConnectionStatus = UniversalStatus.Ok;
                }
                else
                {
                    RunningParams.ServerConnectionStatus = UniversalStatus.NotOk;
                }

            }
            catch (Exception ex)
            {
                RunningParams.ServerConnectionStatus = UniversalStatus.Unknown;
                throw new ArgumentException("\n>> " + GetType().FullName + ".UpdateServerConnectionStatus Error: " +
                                            ex.Message);
            }
        }

    }

    internal static class ServerConnectionServiceData
    {
        internal static UniversalStatus PingServerStatus = UniversalStatus.Unknown;
        internal static UniversalStatus TelnetServerStatus = UniversalStatus.Unknown;
    }
}