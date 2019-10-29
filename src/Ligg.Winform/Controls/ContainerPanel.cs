using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Ligg.Winform.DataModel.Enums;
using Ligg.Winform.Skin;

namespace Ligg.Winform.Controls
{
    public sealed class ContainerPanel : Panel
    {
        private int _radius=4;
        public ContainerPanel()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                //ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw
                //|ControlStyles.SupportsTransparentBackColor
                , true
                )
                ;
        }

        //[DefaultValue(typeof(ContainerPanelStyle), "1")]
        public ContainerPanelStyle StyleType { get; set; }  
        public enum ContainerPanelStyle
        {
            None = 0,
            Borders = 1,
            Rounded= 2,
        }

        [DefaultValue(0)]
        public int BorderWidthOnLeft { get; set; }
        [DefaultValue(0)]
        public int BorderWidthOnTop { get; set; }
        [DefaultValue(0)]
        public int BorderWidthOnRight { get; set; }
        //no use[DefaultValue(1)]
        public int BorderWidthOnBottom { get; set; }
        public Color BorderColor { get; set; }

        [DefaultValue(typeof(RoundStyle), "0")]
        public RoundStyle RoundStyle { get; set; }
        [DefaultValue(8)]
        public int Radius
        {
            get { return _radius; }
            set
            {
                if (_radius != value)
                {
                    _radius = value < 4 ? 4 : value;
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.Padding =this.Padding;
            base.BorderStyle = BorderStyle.None;
            base.OnPaintBackground(e);
            if (StyleType == ContainerPanelStyle.Borders)
            {
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                 BorderColor, BorderWidthOnLeft, ButtonBorderStyle.Solid,
                 BorderColor, BorderWidthOnTop, ButtonBorderStyle.Solid,
                 BorderColor, BorderWidthOnRight, ButtonBorderStyle.Solid,
                 BorderColor, BorderWidthOnBottom, ButtonBorderStyle.Solid
                             );
            }
            else if (StyleType == ContainerPanelStyle.Rounded)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                GraphicsHelper.DrawBackground(
                 g,
                 ClientRectangle,
                 BackColor,
                 BorderColor,
                 Color.Transparent,
                 RoundStyle,
                 _radius,
                 0,
                 true,
                 false,
                 LinearGradientMode.Vertical);
            }

            base.OnPaint(e);
        }
    }

}
