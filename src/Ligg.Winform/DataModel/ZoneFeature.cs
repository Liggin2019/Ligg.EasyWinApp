
namespace Ligg.Winform.DataModel
{
    public class ZoneFeature
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int ArrangementType { get; set; }
        public string StyleText { get; set; }

        public bool HasNoControlBox { get; set; }
        public bool TreatCloseBoxAsMinimizeBox { get; set; }
        public bool DrawIcon { get; set; }
        public string IconUrl { get; set; }
        public bool HasTray { get; set; }
        public string TrayIconUrl { get; set; }
        public string TrayDataSource { get; set; }

        public bool ShowRunningStatusSection { get; set; }
        public bool VerifyPasswordOnLoad { get; set; }
        public string PasswordVerification { get; set; }

    }

}
