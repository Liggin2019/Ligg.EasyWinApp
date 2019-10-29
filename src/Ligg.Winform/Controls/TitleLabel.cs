using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Ligg.Winform.Controls
{
    public sealed class TitleLabel : Label
    {
        public TitleLabel()
        {
        }

        private Color _bottomLineColor=Color.Black ; //useful

        [DefaultValue(typeof(Color), "Black")]//no use
        public bool HasBottomLine { get; set; }

        public Color BottomLineColor 
        {
            get { return _bottomLineColor; }
            set { _bottomLineColor = value; }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.BorderStyle =BorderStyle.None;
            base.OnPaint(e);
            if (HasBottomLine)
            {
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                    BackColor, 0, ButtonBorderStyle.Solid,
                    BackColor, 0, ButtonBorderStyle.Solid,
                    BackColor, 0, ButtonBorderStyle.Solid,
                    _bottomLineColor, 1, ButtonBorderStyle.Solid
                );
            }
       
        }

    }
}
