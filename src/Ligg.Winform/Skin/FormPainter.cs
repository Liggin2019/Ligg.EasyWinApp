using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Ligg.WinForm.DataModel.Enums;

namespace Ligg.WinForm.Skin
{
    public class FormPainter : FormRenderer
    {
        public override Region CreateRegion(SkinForm form)
        {
            var rect = new Rectangle(Point.Empty, form.Size);
            using (GraphicsPath path = GraphicsHelper.CreatePath(rect, form.Radius, form.RoundStyle, false))
            {
                return new Region(path);
            }
        }

        public override void InitGroundForm(SkinForm form)
        {
            form.BackColor = StyleSheet.BaseColor;
        }

        protected override void OnRenderGroundFormCaption(FormCaptionRenderEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = e.ClipRectangle;
            SkinForm form = e.Form;
            Rectangle iconRect = form.IconRect;
            Rectangle textRect;

            bool closeBox = form.ControlBox;
            bool ifHasMinimizeBox = form.ControlBox && form.MinimizeBox;
            bool ifHasMaximizeBox = form.ControlBox && form.MaximizeBox;

            int iconAreaWidth = 0;
            if (closeBox)
            {
                iconAreaWidth += form.CloseBoxSize.Width + form.ControlBoxOffset.X;
            }

            if (ifHasMaximizeBox)
            {
                iconAreaWidth += form.MaximizeBoxSize.Width + form.ControlBoxSpace;
            }

            if (ifHasMinimizeBox)
            {
                iconAreaWidth += form.MinimizeBoxSize.Width + form.ControlBoxSpace;
            }

            textRect = new Rectangle(
                iconRect.Right + 3,
                form.BorderWidth,
                rect.Width - iconRect.Right - iconAreaWidth - 6,
                rect.Height - form.BorderWidth);

            using (var antiGraphics = new SmoothingModeGraphics(g))
            {
                if(form.DrawCationBackground)
                DrawCaptionBackground(g,rect,e.IsActive);

                if (form.DrawIcon && form.Icon != null)
                {
                    DrawIcon(g, iconRect, form.Icon);
                }

                if (!string.IsNullOrEmpty(form.Text))
                {
                    DrawCaptionText(
                        g,
                        textRect,
                        form.Text,
                        form.CaptionFont);
                }
            }
        }

        protected override void OnRenderGroundFormBorder(FormBorderRenderEventArgs e)
        {
            Graphics g = e.Graphics;
            using (SmoothingModeGraphics antiGraphics = new SmoothingModeGraphics(g))
            {
                DrawBorder(g, e.ClipRectangle, e.Form.RoundStyle, e.Form.Radius);
            }
        }

        protected override void OnRenderGroundFormBackground(FormBackgroundRenderEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = e.ClipRectangle;
            SkinForm form = e.Form;
            using (SmoothingModeGraphics antiGraphics = new SmoothingModeGraphics(g))
            {
                using (Brush brush = new SolidBrush(StyleSheet.BaseColor))
                {
                    using (GraphicsPath path = GraphicsHelper.CreatePath(
                        rect, form.Radius, form.RoundStyle, false))
                    {
                        g.FillPath(brush, path);
                    }
                }
            }
        }

        protected override void OnRenderGroundFormControlBox(FormControlBoxRenderEventArgs e)
        {
            SkinForm form = e.Form;
            Graphics g = e.Graphics;
            Rectangle rect = e.ClipRectangle;
            ControlState state = e.ControlBoxtate;
            bool isActive = e.IsActive;

            bool ifHasMinimizeBox = form.ControlBox && form.MinimizeBox;
            bool ifHasMaximizeBox = form.ControlBox && form.MaximizeBox;

            switch (e.ControlBoxStyle)
            {

                case ControlBoxStyle.Maximize:
                    DrawGroundFormMaximizeBox(
                        g,
                        rect,
                        state,
                        isActive,
                        ifHasMinimizeBox,
                        form.WindowState == FormWindowState.Maximized);
                    break;

                case ControlBoxStyle.Minimize:
                    DrawGroundFormMinimizeBox(
                       g,
                       rect,
                       state,
                       isActive);
                    break;

                case ControlBoxStyle.Close:
                    DrawGroundFormCloseBox(
                        g,
                        rect,
                        state,
                        isActive,
                        ifHasMinimizeBox,
                        ifHasMaximizeBox);
                    break;
            }
        }

        #region Draw Methods
        private void DrawIcon(Graphics g, Rectangle iconRect, Icon icon)
        {
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawIcon(icon,iconRect);
        }

        private void DrawCaptionBackground(Graphics g, Rectangle captionRect, bool isActive)
        {
            Color baseColor = isActive ? StyleSheet.CaptionActiveColor : StyleSheet.CaptionInactiveColor;

            GraphicsHelper.DrawBackground(
                g,
                captionRect,
                baseColor,
                StyleSheet.FormBorderColor,
                //StyleSet.FormInnerBorderColor,
                StyleSheet.FormBorderColor,
                RoundStyle.None,
                0,
                //.25f,
                .00f,
                false,
                false,
                LinearGradientMode.Vertical);
        }

        private void DrawCaptionText(Graphics g, Rectangle textRect, string text, Font font)
        {
            TextRenderer.DrawText(
                g,
                text,
                font,
                textRect,
                StyleSheet.CaptionTextColor,
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.Left |
                TextFormatFlags.SingleLine |
                TextFormatFlags.WordEllipsis);
        }

        private void DrawBorder(Graphics g, Rectangle rect,RoundStyle roundStyle, int radius)
        {
            rect.Width -= 1;
            rect.Height -= 1;
            using (GraphicsPath path = GraphicsHelper.CreatePath(rect, radius, roundStyle, false))
            {
                using (Pen pen = new Pen(StyleSheet.FormBorderColor))
                {
                    g.DrawPath(pen, path);
                }
            }

            rect.Inflate(-1, -1);
            using (GraphicsPath path = GraphicsHelper.CreatePath(rect, radius, roundStyle, false))
            {
                using (Pen pen = new Pen(StyleSheet.FormInnerBorderColor))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        private void DrawGroundFormMinimizeBox(Graphics g, Rectangle rect, ControlState state, bool isActive)
        {
            Color baseColor;

            if (state == ControlState.Pressed)
            {
                baseColor = StyleSheet.ControlBoxPressedColor;
            }
            else if (state == ControlState.Hovering)
            {
                baseColor = StyleSheet.ControlBoxHoveringColor;
            }
            else
            {
                baseColor = isActive ? StyleSheet.ControlBoxActiveColor : StyleSheet.ControlBoxInActiveColor;
            }

            RoundStyle roundStyle = RoundStyle.BottomLeft;

            using (var antiGraphics = new SmoothingModeGraphics(g))
            {
                using (GraphicsPath path = CreateMinimizeGraphicsPath(rect))
                {
                    g.FillPath(Brushes.White, path);
                    using (Pen pen = new Pen(baseColor))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }
        }

        private void DrawGroundFormMaximizeBox(Graphics g,Rectangle rect,ControlState state,bool isActive,bool ifHasMinimizeBox,bool maximize)
        {
            Color baseColor;

            if (state == ControlState.Pressed)
            {
                baseColor = StyleSheet.ControlBoxPressedColor;
            }
            else if (state == ControlState.Hovering)
            {
                baseColor = StyleSheet.ControlBoxHoveringColor;
            }
            else
            {
                baseColor = isActive ?StyleSheet.ControlBoxActiveColor :StyleSheet.ControlBoxInActiveColor;
            }

            using (SmoothingModeGraphics antiGraphics = new SmoothingModeGraphics(g))
            {

                using (GraphicsPath path = CreateMaximizeGraphicsPath(rect, maximize))
                {
                    g.FillPath(Brushes.White, path);
                    using (Pen pen = new Pen(baseColor))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }
        }



        private void DrawGroundFormCloseBox(Graphics g,Rectangle rect,ControlState state,bool isActive,bool ifHasMinimizeBox,bool ifHasMaximizeBox)
        {
            Color baseColor = StyleSheet.ControlBoxActiveColor;

            if (state == ControlState.Pressed)
            {
                baseColor = StyleSheet.CloseControlBoxPressedColor;
            }
            else if (state == ControlState.Hovering)
            {
                baseColor = StyleSheet.CloseControlBoxHoveringColor;
            }
            else
            {
                baseColor = isActive ?
                    StyleSheet.ControlBoxActiveColor :
                    StyleSheet.ControlBoxInActiveColor;
            }

            RoundStyle roundStyle = ifHasMinimizeBox || ifHasMaximizeBox ?RoundStyle.BottomRight : RoundStyle.Bottom;

            using (SmoothingModeGraphics antiGraphics = new SmoothingModeGraphics(g))
            {
                GraphicsHelper.DrawBackground(
                    g,
                    rect,
                    baseColor,
                    baseColor,
                    StyleSheet.ControlBoxInnerBorderColor,
                    roundStyle,
                    6,
                    .38F,
                    true,
                    false,
                    LinearGradientMode.Vertical);

                using (Pen pen = new Pen(StyleSheet.FormBorderColor))
                {
                    g.DrawLine(pen, rect.X, rect.Y, rect.Right, rect.Y);
                }

                using (GraphicsPath path = CreateCloseGraphicsPath(rect))
                {
                    g.FillPath(Brushes.White, path);
                    using (Pen pen = new Pen(baseColor))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }
        }
        #endregion


        private GraphicsPath CreateMaximizeGraphicsPath(Rectangle rect, bool maximize)
        {
            PointF centerPoint = new PointF(
               rect.X + rect.Width / 2.0f,
               rect.Y + rect.Height / 2.0f);

            GraphicsPath path = new GraphicsPath();

            if (maximize)
            {
                path.AddLine(
                    centerPoint.X - 3,
                    centerPoint.Y - 3,
                    centerPoint.X - 6,
                    centerPoint.Y - 3);
                path.AddLine(
                    centerPoint.X - 6,
                    centerPoint.Y - 3,
                    centerPoint.X - 6,
                    centerPoint.Y + 5);
                path.AddLine(
                    centerPoint.X - 6,
                    centerPoint.Y + 5,
                    centerPoint.X + 3,
                    centerPoint.Y + 5);
                path.AddLine(
                    centerPoint.X + 3,
                    centerPoint.Y + 5,
                    centerPoint.X + 3,
                    centerPoint.Y + 1);
                path.AddLine(
                    centerPoint.X + 3,
                    centerPoint.Y + 1,
                    centerPoint.X + 6,
                    centerPoint.Y + 1);
                path.AddLine(
                    centerPoint.X + 6,
                    centerPoint.Y + 1,
                    centerPoint.X + 6,
                    centerPoint.Y - 6);
                path.AddLine(
                    centerPoint.X + 6,
                    centerPoint.Y - 6,
                    centerPoint.X - 3,
                    centerPoint.Y - 6);
                path.CloseFigure();

                path.AddRectangle(new RectangleF(
                    centerPoint.X - 4,
                    centerPoint.Y,
                    5,
                    3));

                path.AddLine(
                    centerPoint.X - 1,
                    centerPoint.Y - 4,
                    centerPoint.X + 4,
                    centerPoint.Y - 4);
                path.AddLine(
                    centerPoint.X + 4,
                    centerPoint.Y - 4,
                    centerPoint.X + 4,
                    centerPoint.Y - 1);
                path.AddLine(
                    centerPoint.X + 4,
                    centerPoint.Y - 1,
                    centerPoint.X + 3,
                    centerPoint.Y - 1);
                path.AddLine(
                    centerPoint.X + 3,
                    centerPoint.Y - 1,
                    centerPoint.X + 3,
                    centerPoint.Y - 3);
                path.AddLine(
                    centerPoint.X + 3,
                    centerPoint.Y - 3,
                    centerPoint.X - 1,
                    centerPoint.Y - 3);
                path.CloseFigure();
            }
            else
            {
                path.AddRectangle(new RectangleF(
                    centerPoint.X - 6,
                    centerPoint.Y - 4,
                    12,
                    8));
                path.AddRectangle(new RectangleF(
                    centerPoint.X - 3,
                    centerPoint.Y - 1,
                    6,
                    3));
            }

            return path;
        }

        private GraphicsPath CreateMinimizeGraphicsPath(Rectangle rect)
        {
            PointF centerPoint = new PointF(
                rect.X + rect.Width / 2.0f,
                rect.Y + rect.Height / 2.0f);

            GraphicsPath path = new GraphicsPath();

            path.AddRectangle(new RectangleF(
                centerPoint.X - 6,
                centerPoint.Y + 1,
                12,
                3));
            return path;
        }

        private GraphicsPath CreateCloseGraphicsPath(Rectangle rect)
        {
            PointF centerPoint = new PointF(
                rect.X + rect.Width / 2.0f,
                rect.Y + rect.Height / 2.0f);

            GraphicsPath path = new GraphicsPath();

            path.AddLine(
                centerPoint.X,
                centerPoint.Y - 2,
                centerPoint.X - 2,
                centerPoint.Y - 4);
            path.AddLine(
                centerPoint.X - 2,
                centerPoint.Y - 4,
                centerPoint.X - 6,
                centerPoint.Y - 4);
            path.AddLine(
                centerPoint.X - 6,
                centerPoint.Y - 4,
                centerPoint.X - 2,
                centerPoint.Y);
            path.AddLine(
                centerPoint.X - 2,
                centerPoint.Y,
                centerPoint.X - 6,
                centerPoint.Y + 4);
            path.AddLine(
                centerPoint.X - 6,
                centerPoint.Y + 4,
                centerPoint.X - 2,
                centerPoint.Y + 4);
            path.AddLine(
                centerPoint.X - 2,
                centerPoint.Y + 4,
                centerPoint.X,
                centerPoint.Y + 2);
            path.AddLine(
                centerPoint.X,
                centerPoint.Y + 2,
                centerPoint.X + 2,
                centerPoint.Y + 4);
            path.AddLine(
               centerPoint.X + 2,
               centerPoint.Y + 4,
               centerPoint.X + 6,
               centerPoint.Y + 4);
            path.AddLine(
              centerPoint.X + 6,
              centerPoint.Y + 4,
              centerPoint.X + 2,
              centerPoint.Y);
            path.AddLine(
             centerPoint.X + 2,
             centerPoint.Y,
             centerPoint.X + 6,
             centerPoint.Y - 4);
            path.AddLine(
             centerPoint.X + 6,
             centerPoint.Y - 4,
             centerPoint.X + 2,
             centerPoint.Y - 4);

            path.CloseFigure();
            return path;
        }

    }
}
