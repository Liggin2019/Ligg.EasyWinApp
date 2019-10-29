namespace Ligg.Winform.DataModel
{
    public class ApplicationStartParamSet
    {
        public string ApplicationVersion { get; set; }
        public string HelpdeskEmail { get; set; }
        public string ImplementationDllPath { get; set; }
        public string AdapterFullClassName { get; set; }

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
        public bool SupportMutiCultures { get; set; }
    }


}
