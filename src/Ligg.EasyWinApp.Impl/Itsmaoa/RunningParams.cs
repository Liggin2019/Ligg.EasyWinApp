using System;
using Ligg.Base.DataModel.Enums;
using Ligg.EasyWinApp.Implementation.DataModel;
using Ligg.EasyWinApp.Implementation.DataModel.Enums;

namespace Ligg.EasyWinApp.Implementation
{
    internal static class RunningParams
    {
        //#property
        internal static int AssemblyBits => IntPtr.Size * 8;

        internal static NetworkDistance NetworkDistance;

        internal static UniversalStatus CurrentNetworkLocationStatus = UniversalStatus.Unknown;
        internal static NetworkLocation CurrentNetworkLocation = new NetworkLocation();
        internal static NetworkLocation ChosenNetworkLocation = new NetworkLocation();

        internal static UniversalStatus ServerConnectionStatus = UniversalStatus.Unknown;

        internal static UniversalStatus RunAsAdminAccountStatus = UniversalStatus.Unknown;
        internal static string CurrentRunAsAdminAccountDomain = "";
        internal static string CurrentRunAsAdminAccountName = "";
        internal static string CurrentRunAsAdminAccountPassword = "";
        




    }


}
