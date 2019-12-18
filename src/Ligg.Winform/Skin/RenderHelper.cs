using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Ligg.WinForm.DataModel.Enums;

namespace Ligg.WinForm.Skin
{
    public static class RenderHelper
    {
        public static void CreateRegion(Control control,Rectangle bounds,int radius,RoundStyle roundStyle)
        {
            using (GraphicsPath path =
                GraphicsHelper.CreatePath(
                bounds, radius, roundStyle, true))
            {
                Region region = new Region(path);
                path.Widen(Pens.White);
                region.Union(path);
                if (control.Region != null)
                {
                    control.Region.Dispose();
                }
                control.Region = region;
            }
        }

        public static void CreateRegion(Control control,Rectangle bounds)
        {
            CreateRegion(control, bounds, 8, RoundStyle.All);
        }
    }
}
