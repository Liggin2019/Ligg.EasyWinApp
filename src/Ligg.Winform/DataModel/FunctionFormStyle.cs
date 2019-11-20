
namespace Ligg.Winform.DataModel
{
    public class FunctionFormStyle
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int TopLocationX { get; set; }
        public int TopLocationY { get; set; }
        public bool MaximizeBox { get; set; }
        public bool MinimizeBox { get; set; }
        public bool HasNoControlBox { get; set; }
        public bool TreatCloseBoxAsMinimizeBox { get; set; }
        public int ViewMenuMode { get; set; }
        public string StartWindowState { get; set; }//x
        public string OrdinaryWindowStatus { get; set; }
        public bool DrawIcon { get; set; }
        public int WindowRadius { get; set; }
        public string IconUrl { get; set; }
        public bool HasTray { get; set; }
        public string TrayIconUrl { get; set; }
        public string TrayDataSource { get; set; }
        public string ResizeRegionParams { get; set; }

    }

}
