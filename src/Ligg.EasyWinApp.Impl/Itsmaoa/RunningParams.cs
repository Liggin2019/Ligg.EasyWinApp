using System;
using Ligg.Base.DataModel.Enums;
using Ligg.EasyWinApp.Implementation.DataModel;
using Ligg.EasyWinApp.Implementation.DataModel.Enums;

namespace Ligg.EasyWinApp.Implementation
{
    internal static class RunningParams
    {
        //#property
        internal static int AssemblyBits
        {
            get { return IntPtr.Size * 8; }
        }

        internal static NetworkDistance NetworkDistance;

        internal static UniversalStatus CurrentNetworkLocationStatus = UniversalStatus.Unknown;
        internal static NetworkLocation CurrentNetworkLocation = new NetworkLocation();
        internal static NetworkLocation ChosenNetworkLocation = new NetworkLocation();

        internal static UniversalStatus ServerConnectionStatus = UniversalStatus.Unknown;
        internal static UniversalStatus PingServerStatus = UniversalStatus.Unknown;
        internal static UniversalStatus TelnetServerStatus = UniversalStatus.Unknown;

        internal static UniversalStatus RunAsAdminAccountStatus = UniversalStatus.Unknown;
        internal static string CurrentRunAsAdminAccountDomain = "";
        internal static string CurrentRunAsAdminAccountName = "";
        internal static string CurrentRunAsAdminAccountPassword = "";
        internal static UniversalStatus CurrentWinIdAsRunAsAdminAccountStatus = UniversalStatus.Unknown;
        internal static UniversalStatus Win10CompatibilityStatus = UniversalStatus.Unknown;
        internal static UniversalStatus SeclogonWinServiceStatus = UniversalStatus.Unknown;
        internal static UniversalStatus DefaultRunAsAdminAccountStatus = UniversalStatus.Unknown;
        internal static UniversalStatus DesignatedRunAsAdminAccountStatus = UniversalStatus.Unknown;
        internal static bool IsDesignatedRunAsAdminAccountDomainAcct = false;
        internal static string DesignatedRunAsAdminAccountDomain = "";
        internal static string DesignatedRunAsAdminAccountName = "";
        internal static string DesignatedRunAsAdminAccountPassword = "";





    }


}
