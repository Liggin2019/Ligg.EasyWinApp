namespace Ligg.Winform.DataModel
{
    public class FunctionStartParamSet
    {
        public bool ShowSoftwareCover { get; set; }
        public string SoftwareCoverLocation { get; set; }
        public string SoftwareCoverActionsAtStart { get; set; }
        
        public bool VerifyPasswordAtStart { get; set; }
        public string PasswordVerification { get; set; }

        public bool LogonAtStart { get; set; }
        public string LogonZoneLocation { get; set; }

        public bool CheckLicenseAvailability { get; set; }
        public bool CheckPublishmentValidity { get; set; }
        public bool CheckSoftwareVersion { get; set; }
        public bool CheckHostingLocation { get; set; }
        public string HostingServers { get; set; }
    }


}
