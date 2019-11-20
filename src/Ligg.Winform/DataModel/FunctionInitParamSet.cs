using Ligg.Winform.DataModel.Enums;

namespace Ligg.Winform.DataModel
{
    public class FunctionInitParamSet
    {
        public bool IsFormInvisible { get; set; }
        public FunctionFormType FormType { get; set; }
        public string AssemblyCode { get; set; }
        public string ApplicationCode { get; set; }
        public string ApplicationVersion { get; set; }
        public string FunctionCode { get; set; }
        public string ZoneLocationForNonMutiViewForm { get; set; }
        public string StartParams { get; set; }
        public string InputZoneVariablesForNonMutiViewForm { get; set; }
        public int StartViewMenuId { get; set; }
        public string StartActions { get; set; }
        public string StartPassword { get; set; }
        public string HelpdeskEmail { get; set; }
        public bool SupportMutiCultures { get; set; }
        public string FormTitle { get; set; }

        public string ImplementationDir { get; set; }
        //public bool Debug { get; set; }
        //public string ImplementationDllPath { get; set; }
        //public string AdapterFullClassName { get; set; }
    }

}
