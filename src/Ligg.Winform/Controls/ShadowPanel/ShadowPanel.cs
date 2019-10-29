using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Ligg.Winform.Controls.ShadowPanel
{
    [ToolboxItem(false)]
    public class ShadowPanel : Panel
    {
        public Color PanelColor { get; set; }
        public Color BorderColor { get; set; }

        private const int ShadowSize = 5;
        private const int ShadowMargin = 2;

        // static for good perfomance 
        static readonly Image ShadowDownRight = new Bitmap(typeof(ShadowPanel), "Images.shadowdownright.png");  //Embedded Resource, donot copy
        static readonly Image ShadowDownLeft = new Bitmap(typeof(ShadowPanel), "Images.shadowdownleft.png");
        static readonly Image ShadowDown = new Bitmap(typeof(ShadowPanel), "Images.shadowdown.png");
        static readonly Image ShadowRight = new Bitmap(typeof(ShadowPanel), "Images.shadowright.png");
        static readonly Image ShadowTopRight = new Bitmap(typeof(ShadowPanel), "Images.shadowtopright.png");

        public ShadowPanel()
        {

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Get the graphics object. We need something to draw with ;-)
            Graphics g = e.Graphics;

            // Create tiled brushes for the shadow on the right and at the bottom.
            TextureBrush shadowRightBrush = new TextureBrush(ShadowRight, WrapMode.Tile);
            TextureBrush shadowDownBrush = new TextureBrush(ShadowDown, WrapMode.Tile);

            // Translate (move) the brushes so the top or left of the image matches the top or left of the
            // area where it's drawed. If you don't understand why this is necessary, comment it out. 
            // Hint: The tiling would start at 0,0 of the control, so the shadows will be offset a little.
            shadowDownBrush.TranslateTransform(0, Height - ShadowSize);
            shadowRightBrush.TranslateTransform(Width - ShadowSize, 0);

            Rectangle shadowRightRectangle = new Rectangle(
                Width - ShadowSize,                             // X
                ShadowSize + ShadowMargin,                      // Y
                ShadowSize,                                     // width
                Height - (ShadowSize * 2 + ShadowMargin)        // height (stretches)
                );

            Rectangle shadowDownRectangle = new Rectangle(
                ShadowSize + ShadowMargin,                      // X
                Height - ShadowSize,                            // Y
                Width - (ShadowSize * 2 + ShadowMargin),        // width (stretches)
                ShadowSize                                      // height
                );



            // And draw the shadow on the right and at the bottom.
            g.FillRectangle(shadowDownBrush, shadowDownRectangle);
            g.FillRectangle(shadowRightBrush, shadowRightRectangle);

            // Now for the corners, draw the 3 5x5 pixel images.
            g.DrawImage(ShadowTopRight, new Rectangle(Width - ShadowSize, ShadowMargin, ShadowSize, ShadowSize));
            g.DrawImage(ShadowDownRight, new Rectangle(Width - ShadowSize, Height - ShadowSize, ShadowSize, ShadowSize));
            g.DrawImage(ShadowDownLeft, new Rectangle(ShadowMargin, Height - ShadowSize, ShadowSize, ShadowSize));

            // Fill the area inside with the color in the PanelColor property.
            // 1 pixel is added to everything to make the rectangle smaller. 
            // This is because the 1 pixel border is actually drawn outside the rectangle.
            Rectangle fullRectangle = new Rectangle(
               1,                                              // X
               1,                                              // Y
               Width - (ShadowSize + ShadowMargin),                       // Width
               Height - (ShadowSize + ShadowMargin)                       // Height
               );

            if (PanelColor != null)
            {
                SolidBrush bgBrush = new SolidBrush(PanelColor);
                g.FillRectangle(bgBrush, fullRectangle);
                Pen borderPen = new Pen(PanelColor);
                g.DrawRectangle(borderPen, fullRectangle);
            }

            // Draw a nice 1 pixel border it a BorderColor is specified
            if (BorderColor != null)
            {
                Pen borderPen = new Pen(BorderColor);
                g.DrawRectangle(borderPen, fullRectangle);
                //ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                //BorderColor, 1, ButtonBorderStyle.Solid,
                //BorderColor, 1, ButtonBorderStyle.Solid,
                //BorderColor, 0, ButtonBorderStyle.Solid,
                //BorderColor, 0, ButtonBorderStyle.Solid
                            //);
            }

            // Memory efficiency
            shadowDownBrush.Dispose();
            shadowRightBrush.Dispose();

            shadowDownBrush = null;
            shadowRightBrush = null;
        }

        // Correct resizing
        protected override void OnResize(EventArgs e)
        {
            base.Invalidate();
            base.OnResize(e);
        }
    }
}
