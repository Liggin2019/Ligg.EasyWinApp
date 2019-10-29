using System;
using Ligg.Base.DataModel.Enums;
using Ligg.Utility.Admin.Helpers.Network;

namespace Ligg.EasyWinApp.Implementation.Services
{
    internal class ServerConnectionService
    {
        internal void InitServerConnectionStatus()
        {
            try
            {
                new NetworkLocationService().InitCurrentNetworkLocation();
                if (RunningParams.ServerConnectionStatus == UniversalStatus.Unknown) RefreshServerConnectionStatus();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".InitServerConnectionStatus Error: " + ex.Message);
            }
        }

        internal void RefreshServerConnectionStatus()
        {
            try
            {
                RefreshPingServerStatus();
                //if (RunningParams.PingServerStatus == UniversalStatus.NotOk)
                //{
                //    return;
                //}
                RefreshTelnetServerStatus();
            }
            catch (Exception ex)
            {
                RunningParams.ServerConnectionStatus = UniversalStatus.Unknown;
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshServerConnection Error: " + ex.Message);
            }
        }

        internal void RefreshPingServerStatus()
        {
            try
            {
                if (!NetworkHelper.IsPingSucceeded(RunningParams.CurrentNetworkLocation.ServerAddress))
                {
                    RunningParams.PingServerStatus = UniversalStatus.NotOk;
                    //RunningParams.TelnetServerStatus = UniversalStatus.NotOk;
                }
                else
                {
                    RunningParams.PingServerStatus = UniversalStatus.Ok;
                }
                UpdateServerConnectionStatus();

            }
            catch (Exception ex)
            {
                RunningParams.ServerConnectionStatus = UniversalStatus.Unknown;
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshPingServerStatus Error: " + ex.Message);
            }
        }

        internal void RefreshTelnetServerStatus()
        {
            try
            {
                if (!NetworkHelper.IsTelnetSucceeded(RunningParams.CurrentNetworkLocation.ServerAddress, RunningParams.CurrentNetworkLocation.ServerPort, 50))
                {
                    RunningParams.TelnetServerStatus = UniversalStatus.NotOk;
                }
                else
                {
                    RunningParams.TelnetServerStatus = UniversalStatus.Ok;
                    //RunningParams.PingServerStatus = UniversalStatus.Ok;
                }

                UpdateServerConnectionStatus();
            }
            catch (Exception ex)
            {
                RunningParams.ServerConnectionStatus = UniversalStatus.Unknown;
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshTelnetServerStatus Error: " + ex.Message);
            }
        }

        internal void UpdateServerConnectionStatus()
        {
            try
            {
                //if (RunningParams.PingServerStatus == UniversalStatus.Ok & RunningParams.TelnetServerStatus == UniversalStatus.Ok)
                if (RunningParams.TelnetServerStatus == UniversalStatus.Ok)
                {
                    RunningParams.ServerConnectionStatus = UniversalStatus.Ok;
                }
                else { RunningParams.ServerConnectionStatus = UniversalStatus.NotOk; }

            }
            catch (Exception ex)
            {
                RunningParams.ServerConnectionStatus = UniversalStatus.Unknown;
                throw new ArgumentException("\n>> " + GetType().FullName + ".UpdateServerConnectionStatus Error: " + ex.Message);
            }
        }





    }
}