namespace Ligg.WinForm.DataModel
{
    public class ApplicationStartParamSet
    {
        public string ApplicationVersion { get; set; }
        public string HelpdeskEmail { get; set; }
        public string ImplementationDllPath { get; set; }
        public string AdapterFullClassName { get; set; }
        public bool SupportMultiCultures { get; set; }
        public string StyleSheetCode { get; set; }

        public bool VerifyPasswordAtStart { get; set; }
        public string PasswordVerificationRule { get; set; }

        public bool ShowSoftwareCoverAtStart { get; set; }
        public string SoftwareCoverZoneLocation { get; set; }

        public bool LogonAtStart { get; set; }
        public string LogonZoneLocation { get; set; }

        public bool CheckHostingLocation { get; set; }
        public string HostingServers { get; set; }
        public bool CheckLicenseAvailability { get; set; }
        public bool CheckPublicationValidity { get; set; }
        public bool CheckSoftwareVersion { get; set; }

    }


}
