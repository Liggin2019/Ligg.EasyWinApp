using Ligg.WinForm.DataModel.Enums;

namespace Ligg.WinForm.DataModel
{
    public class FunctionInitParamSet
    {
        public bool IsFormInvisible { get; set; }
        public FunctionFormType FormType { get; set; }
        public string ArchitectureCode { get; set; }
        public string ArchitectureVersion { get; set; }
        public string ApplicationCode { get; set; }
        public string ApplicationVersion { get; set; }
        public string FunctionCode { get; set; }
        public string ZoneLocationForSingleViewForm { get; set; }
        public string StartParams { get; set; }
        public string InputZoneVariablesForSingleViewForm { get; set; }
        public int StartViewMenuId { get; set; }
        public string StartActions { get; set; }
        public string StartPassword { get; set; }
        public string HelpdeskEmail { get; set; }
        public bool SupportMultiCultures { get; set; }
        public string FormTitle { get; set; }

        public string ImplementationDir { get; set; }
    }

}
