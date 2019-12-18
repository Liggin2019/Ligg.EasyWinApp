namespace Ligg.EasyWinApp.Implementation.DataModel
{
    public class OrganizationSetting
    {
        public string FullDomainName
        {
            get;
            set;
        }

        public string MachineIpToCheckIfInLan
        {
            get;
            set;
        }

        public string MachineNameToCheckIfInLan
        {
            get;
            set;
        }

        public string ShortDomainName
        {
            get;
            set;
        }

        public string CentralLanIpPrefixes
        {
            get;
            set;
        }

        public string CloseLanIpPrefixes
        {
            get;
            set;
        }

        public string DcNames
        {
            get;
            set;
        }

        public string RunAsAdminLocalAccount
        {
            get;
            set;
        }

        public string RunAsAdminLocalAccountPassword
        {
            get;
            set;
        }

        public string RunAsAdminDomainAccount
        {
            get;
            set;
        }

        public string RunAsAdminDomainAccountPassword
        {
            get;
            set;
        }


    }
}
