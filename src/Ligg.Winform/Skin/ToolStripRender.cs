using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Ligg.WinForm.DataModel.Enums;

namespace Ligg.WinForm.Skin
{
    public class ToolStripRender : ToolStripRenderer
    {
        private static readonly int OffsetMargin =24;
        public ToolStripRender(): base()
        {
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            Graphics g = e.Graphics;
            Rectangle bounds = e.AffectedBounds;

            if (toolStrip is ToolStripDropDown)
            {
                RenderHelper.CreateRegion(toolStrip, bounds);
                using (SolidBrush brush = new SolidBrush(StyleSheet.ToolStripDropDownBackColor))
                {
                    g.FillRectangle(brush, bounds);
                }
            }
            else
            {
                //do nothing
            }
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            Graphics graphics = e.Graphics;
            Rectangle bounds = e.AffectedBounds;

            if (toolStrip is ToolStripDropDown)
            {
                using (SmoothingModeGraphics sg = new SmoothingModeGraphics(graphics))
                {
                    using (GraphicsPath path = GraphicsHelper.CreatePath(bounds, 8, RoundStyle.All, true))
                    {
                        using (Pen pen = new Pen(StyleSheet.ToolStripDropDownBorderColor))
                        //using (Pen pen = new Pen(Color.Red))
                        {
                            path.Widen(pen);
                            graphics.DrawPath(pen, path);
                        }
                    }
                }

                if (!(toolStrip is ToolStripOverflow))
                {
                    bounds.Inflate(-1, -1);
                    using (GraphicsPath innerPath = GraphicsHelper.CreatePath(
                        bounds, 8, RoundStyle.All, true))
                    {
                        using (Pen pen = new Pen(StyleSheet.HeadToolStripItemInnerBorderColor))
                        {
                            graphics.DrawPath(pen, innerPath);
                        }
                    }
                }
            }
            else
            {
                base.OnRenderToolStripBorder(e);
            }
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            var isHeadControl = false;
            if (toolStrip.Tag != null && toolStrip.Tag.ToString().Contains("$")) isHeadControl = true;

            Graphics graphics = e.Graphics;

            if (toolStrip is ToolStripDropDown && e.Item is ToolStripMenuItem)
            {
                bool drawLogo = DrawLogo(toolStrip);
                int offsetMargin = drawLogo ? OffsetMargin : 0;
                Rectangle rect = e.ImageRectangle;

                if (e.Item.RightToLeft == RightToLeft.Yes)
                {
                    rect.X -= offsetMargin + 2;
                }
                else
                {
                    rect.X += offsetMargin + 2;
                }

                rect.Width = 13;
                rect.Y += 1;
                rect.Height -= 3;

                using (SmoothingModeGraphics sg = new SmoothingModeGraphics(graphics))
                {
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddRectangle(rect);
                        using (PathGradientBrush brush = new PathGradientBrush(path))
                        {
                            brush.CenterColor = StyleSheet.ToolStripDropDownPressedCenterColor;
                            brush.CenterColor = Color.Red;
                            brush.SurroundColors = new Color[] { isHeadControl ? StyleSheet.HeadToolStripItemPressedBackColor : StyleSheet.ControlPressedBackColor };
                            Blend blend = new Blend();
                            blend.Positions = new float[] { 0f, 0.3f, 1f };
                            blend.Factors = new float[] { 0f, 0.5f, 1f };
                            brush.Blend = blend;
                            graphics.FillRectangle(brush, rect);
                        }
                    }

                    using (Pen pen = new Pen(StyleSheet.HeadToolStripItemPressedBackColor))
                    {
                        graphics.DrawRectangle(pen, rect);
                    }

                    GraphicsHelper.DrawHollowwRectangle(graphics, rect, isHeadControl ? StyleSheet.HeadToolStripItemCheckedBorderColor : StyleSheet.ControlCheckedBorderColor);
                }
            }
            else
            {
                base.OnRenderItemCheck(e);
            }
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            Rectangle rect = e.Item.ContentRectangle;
            Graphics graphics = e.Graphics;

            Color baseColor = StyleSheet.HeadToolStripBackColor;

            if (toolStrip is ToolStripDropDown)
            {
                bool drawLogo = DrawLogo(toolStrip);

                int offsetMargin = drawLogo ?OffsetMargin * 2 : OffsetMargin;

                if (e.Item.RightToLeft != RightToLeft.Yes)
                {
                    rect.X += offsetMargin + 2;
                }
                rect.Width -= offsetMargin + 4;

                baseColor = StyleSheet.ToolStripDropDownSeparatorColor;
            }

            RenderSeparatorLine(
               graphics,
               rect,
               baseColor,
               StyleSheet.HeadToolStripItemInnerBorderColor,
               Color.Snow,
               e.Vertical);
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            var isHeadControl = false;
            if (toolStrip.Tag!=null && toolStrip.Tag.ToString().Contains("$")) isHeadControl = true;
            var item = e.Item as ToolStripButton;
            Graphics graphcs = e.Graphics;

            if (item != null)
            {
                LinearGradientMode mode =toolStrip.Orientation == Orientation.Horizontal ?
                    LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
                var smoothGraphics = new SmoothingModeGraphics(graphcs);
                var bounds = new Rectangle(Point.Empty, item.Size);

                if (item.BackgroundImage != null)
                {
                    Rectangle clipRect = item.Selected ? item.ContentRectangle : bounds;
                    GraphicsHelper.DrawBackgroundImage(
                        graphcs,
                        item.BackgroundImage,
                        isHeadControl ? StyleSheet.HeadToolStripItemInnerBorderColor : StyleSheet.ControlInnerBorderColor,
                        item.BackgroundImageLayout,
                        bounds,
                        clipRect);
                }

                if (item.CheckState == CheckState.Unchecked)
                {
                    if (item.Selected)
                    {
                        Color color = isHeadControl ? StyleSheet.HeadToolStripItemHoveringBackColor : StyleSheet.ControlHoveringBackColor;
                        if (item.Pressed)
                        {
                            color =isHeadControl ? StyleSheet.HeadToolStripItemPressedBackColor : StyleSheet.ControlPressedBackColor;
                        }
                        GraphicsHelper.DrawBackground(
                            graphcs,
                            bounds,
                            color,
                            isHeadControl ? StyleSheet.HeadToolStripItemBorderColor : StyleSheet.ControlBorderColor,
                            isHeadControl ? StyleSheet.HeadToolStripItemInnerBorderColor : StyleSheet.ControlInnerBorderColor,
                            isHeadControl ? RoundStyle.All : RoundStyle.None, 
                            isHeadControl ?8:0, 
                            isHeadControl ?0.45f:0f,
                            true,
                            true,
                            mode);
                    }
                    else
                    {
                        if (toolStrip is ToolStripOverflow)
                        {
                            using (Brush brush = new SolidBrush(isHeadControl ? StyleSheet.HeadToolStripItemHoveringBackColor : StyleSheet.ControlHoveringBackColor))
                            {
                                graphcs.FillRectangle(brush, bounds);
                            }
                        }
                    }
                }
                else
                {
                    Color color = ControlPaint.Light(isHeadControl ? StyleSheet.HeadToolStripItemHoveringBackColor : StyleSheet.ControlHoveringBackColor);
                    if (item.Selected)
                    {
                        color = isHeadControl ? StyleSheet.HeadToolStripItemHoveringBackColor : StyleSheet.ControlHoveringBackColor;
                    }
                    if (item.Pressed)
                    {
                        color =isHeadControl ? StyleSheet.HeadToolStripItemPressedBackColor : StyleSheet.ControlPressedBackColor;
                    }
                    GraphicsHelper.DrawBackground(
                            graphcs,
                            bounds,
                            color,
                            isHeadControl ? StyleSheet.HeadToolStripItemBorderColor : StyleSheet.ControlBorderColor,
                            isHeadControl ? StyleSheet.HeadToolStripItemInnerBorderColor : StyleSheet.ControlInnerBorderColor,
                            isHeadControl ? RoundStyle.All : RoundStyle.None,
                            isHeadControl ? 8 : 0,
                            isHeadControl ? 0.45f : 0f,
                            true,
                            true,
                            mode);
                }
                smoothGraphics.Dispose();
            }
        }

        protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            var isHeadControl = false;
            if (toolStrip.Tag != null && toolStrip.Tag.ToString().Contains("$")) isHeadControl = true;
            ToolStripSplitButton item = e.Item as ToolStripSplitButton;

            if (item != null)
            {
                Graphics graphics = e.Graphics;
                LinearGradientMode mode = toolStrip.Orientation == Orientation.Horizontal ?
                    LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
                Rectangle bounds = new Rectangle(Point.Empty, item.Size);
                SmoothingModeGraphics sg = new SmoothingModeGraphics(graphics);

                Color arrowColor = SystemColors.ControlDarkDark;

                if (item.BackgroundImage != null)
                {
                    Rectangle clipRect = item.Selected ? item.ContentRectangle : bounds;
                    GraphicsHelper.DrawBackgroundImage(
                        graphics,
                        item.BackgroundImage,
                        isHeadControl ? StyleSheet.HeadToolStripItemInnerBorderColor : StyleSheet.ControlInnerBorderColor,
                        item.BackgroundImageLayout,
                        bounds,
                        clipRect);
                }

                if (item.ButtonPressed)
                {
                    Rectangle buttonBounds = item.ButtonBounds;
                    Padding padding = (item.RightToLeft == RightToLeft.Yes) ?
                        new Padding(0, 1, 1, 1) : new Padding(1, 1, 0, 1);
                    buttonBounds = GraphicsHelper.DeflateRect(buttonBounds, padding);
                    GraphicsHelper.DrawBackground(
                       graphics,
                       bounds,
                       isHeadControl ? StyleSheet.HeadToolStripItemHoveringBackColor : StyleSheet.ControlHoveringBackColor,
                       isHeadControl ? StyleSheet.HeadToolStripItemBorderColor : StyleSheet.ControlBorderColor,
                       isHeadControl ? StyleSheet.HeadToolStripItemInnerBorderColor : StyleSheet.ControlInnerBorderColor,
                       isHeadControl ? RoundStyle.All : RoundStyle.None,
                       isHeadControl ? 8 : 0,
                       isHeadControl ? 0.45f : 0f,
                       true,
                       true,
                       mode);

                    buttonBounds.Inflate(-1, -1);
                    graphics.SetClip(buttonBounds);
                    GraphicsHelper.DrawBackground(
                       graphics,
                       buttonBounds,
                       isHeadControl ? StyleSheet.HeadToolStripItemPressedBackColor : StyleSheet.ControlPressedBackColor,
                       isHeadControl ? StyleSheet.HeadToolStripItemBorderColor : StyleSheet.ControlBorderColor,
                       isHeadControl ? StyleSheet.HeadToolStripItemInnerBorderColor : StyleSheet.ControlInnerBorderColor,
                       isHeadControl ? RoundStyle.All : RoundStyle.None,
                       isHeadControl ? 8 : 0,
                       isHeadControl ? 0.45f : 0f,
                       false,
                       true,
                       mode);
                    graphics.ResetClip();

                    using (Pen pen = new Pen(StyleSheet.HeadToolStripItemBorderColor))
                    {
                        graphics.DrawLine(
                            pen,
                            item.SplitterBounds.Left,
                            item.SplitterBounds.Top,
                            item.SplitterBounds.Left,
                            item.SplitterBounds.Bottom);
                    }
                    base.DrawArrow(
                        new ToolStripArrowRenderEventArgs(
                        graphics,
                        item,
                        item.DropDownButtonBounds,
                        arrowColor,
                        ArrowDirection.Down));
                    return;
                }

                if (item.Pressed || item.DropDownButtonPressed)
                {
                    GraphicsHelper.DrawBackground(
                      graphics,
                      bounds,
                      isHeadControl ? StyleSheet.HeadToolStripItemPressedBackColor : StyleSheet.ControlPressedBackColor,
                      isHeadControl ? StyleSheet.HeadToolStripItemBorderColor : StyleSheet.ControlBorderColor,
                      isHeadControl ? StyleSheet.HeadToolStripItemInnerBorderColor : StyleSheet.ControlInnerBorderColor,
                      isHeadControl ? RoundStyle.All : RoundStyle.None,
                      isHeadControl ? 8 : 0,
                      isHeadControl ? 0.45f : 0f,
                      true,
                      true,
                      mode);
                    base.DrawArrow(
                       new ToolStripArrowRenderEventArgs(
                       graphics,
                       item,
                       item.DropDownButtonBounds,
                       arrowColor,
                       ArrowDirection.Down));
                    return;
                }

                if (item.Selected)
                {
                    GraphicsHelper.DrawBackground(
                      graphics,
                      bounds,
                      isHeadControl ? StyleSheet.HeadToolStripItemHoveringBackColor : StyleSheet.ControlHoveringBackColor,
                      isHeadControl ? StyleSheet.HeadToolStripItemBorderColor : StyleSheet.ControlBorderColor,
                      isHeadControl ? StyleSheet.HeadToolStripItemInnerBorderColor : StyleSheet.ControlInnerBorderColor,
                      isHeadControl ? RoundStyle.All : RoundStyle.None,
                      isHeadControl ? 8 : 0,
                      isHeadControl ? 0.45f : 0f,
                      true,
                      true,
                      mode);
                    using (Pen pen = new Pen(StyleSheet.HeadToolStripItemBorderColor))
                    {
                        graphics.DrawLine(
                           pen,
                           item.SplitterBounds.Left,
                           item.SplitterBounds.Top,
                           item.SplitterBounds.Left,
                           item.SplitterBounds.Bottom);
                    }
                    base.DrawArrow(
                        new ToolStripArrowRenderEventArgs(
                        graphics,
                        item,
                        item.DropDownButtonBounds,
                        arrowColor,
                        ArrowDirection.Down));
                    return;
                }

                base.DrawArrow(
                   new ToolStripArrowRenderEventArgs(
                   graphics,
                   item,
                   item.DropDownButtonBounds,
                   arrowColor,
                   ArrowDirection.Down));
                return;
            }

            base.OnRenderSplitButtonBackground(e);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            ToolStripItem item = e.Item;

            if (!item.Enabled)
            {
                return;
            }

            Graphics graphics = e.Graphics;
            var rect = new Rectangle(Point.Empty, e.Item.Size);

            if (toolStrip is MenuStrip)
            {

            }
            else if (toolStrip is ToolStripDropDown)
            {
                bool drawLogo = DrawLogo(toolStrip);
                int offsetMargin = drawLogo ? OffsetMargin : 0;

                if (item.RightToLeft == RightToLeft.Yes)
                {
                    rect.X += 4;
                }
                else
                {
                    rect.X += offsetMargin + 4;
                }
                rect.Width -= offsetMargin + 8;
                rect.Height--;

                if (item.Selected)
                {
                    GraphicsHelper.DrawBackground(
                       graphics,
                       rect,
                       StyleSheet.ToolStripMenuItemHoveredBackColor,
                       StyleSheet.ToolStripMenuItemBorderColor,
                       StyleSheet.ToolStripMenuItemInnerBorderColor,
                       RoundStyle.All,
                       4, 0.25f,
                       true,
                       true,
                       LinearGradientMode.Vertical);
                }
                else
                {
                    base.OnRenderMenuItemBackground(e);
                }
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }

        protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            ToolStripDropDownItem item = e.Item as ToolStripDropDownItem;

            if (item != null)
            {
                LinearGradientMode mode =toolStrip.Orientation == Orientation.Horizontal ?
                   LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
                Graphics graphics = e.Graphics;
                SmoothingModeGraphics sg = new SmoothingModeGraphics(graphics);
                Rectangle bounds = new Rectangle(Point.Empty, item.Size);

                if (item.Pressed && item.HasDropDownItems)
                {
                    GraphicsHelper.DrawBackground(
                      graphics,
                      bounds,
                      StyleSheet.HeadToolStripItemPressedBackColor,
                      StyleSheet.HeadToolStripItemBorderColor,
                      StyleSheet.HeadToolStripItemInnerBorderColor,
                      RoundStyle.All, 8, 0.45f,
                      true,
                      true,
                      mode);
                }
                else if (item.Selected)
                {
                    GraphicsHelper.DrawBackground(
                      graphics,
                      bounds,
                      StyleSheet.HeadToolStripItemHoveringBackColor,
                      StyleSheet.HeadToolStripItemBorderColor,
                      StyleSheet.HeadToolStripItemInnerBorderColor,
                      RoundStyle.All, 8, 0.45f,
                      true,
                      true,
                      mode);
                }
                else if (toolStrip is ToolStripOverflow)
                {
                    using (Brush brush = new SolidBrush(StyleSheet.HeadToolStripItemInnerBorderColor))
                    {
                        graphics.FillRectangle(brush, bounds);
                    }
                }
                else
                {
                    base.OnRenderDropDownButtonBackground(e);
                }

                sg.Dispose();
            }
        }

        protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStripItem item = e.Item;
            ToolStrip toolStrip = e.ToolStrip;
            Graphics graphics = e.Graphics;
            bool rightToLeft = item.RightToLeft == RightToLeft.Yes;

            SmoothingModeGraphics sg = new SmoothingModeGraphics(graphics);

            RenderOverflowBackground(e, rightToLeft);

            bool bHorizontal = toolStrip.Orientation == Orientation.Horizontal;
            Rectangle empty = Rectangle.Empty;

            if (rightToLeft)
            {
                empty = new Rectangle(0, item.Height - 8, 10, 5);
            }
            else
            {
                empty = new Rectangle(item.Width - 12, item.Height - 8, 10, 5);
            }
            ArrowDirection direction = bHorizontal ?
                ArrowDirection.Down : ArrowDirection.Right;
            int x = (rightToLeft && bHorizontal) ? -1 : 1;
            empty.Offset(x, 1);

            Color arrowColor = SystemColors.ControlDark;

            using (Brush brush = new SolidBrush(arrowColor))
            {
                GraphicsHelper.DrawArrow(graphics, empty, direction, brush);
            }

            if (bHorizontal)
            {
                using (Pen pen = new Pen(arrowColor))
                {
                    graphics.DrawLine(
                        pen,
                        empty.Right - 8,
                        empty.Y - 2,
                        empty.Right - 2,
                        empty.Y - 2);
                    graphics.DrawLine(
                        pen,
                        empty.Right - 8,
                        empty.Y - 1,
                        empty.Right - 2,
                        empty.Y - 1);
                }
            }
            else
            {
                using (Pen pen = new Pen(arrowColor))
                {
                    graphics.DrawLine(
                        pen,
                        empty.X,
                        empty.Y,
                        empty.X,
                        empty.Bottom - 1);
                    graphics.DrawLine(
                        pen,
                        empty.X,
                        empty.Y + 1,
                        empty.X,
                        empty.Bottom);
                }
            }
        }

        protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
        {
            if (e.GripStyle == ToolStripGripStyle.Visible)
            {
                Rectangle bounds = e.GripBounds;
                bool vert = e.GripDisplayStyle == ToolStripGripDisplayStyle.Vertical;
                ToolStrip toolStrip = e.ToolStrip;
                Graphics graphics = e.Graphics;

                if (vert)
                {
                    bounds.X = e.AffectedBounds.X;
                    bounds.Width = e.AffectedBounds.Width;
                    if (toolStrip is MenuStrip)
                    {
                        if (e.AffectedBounds.Height > e.AffectedBounds.Width)
                        {
                            vert = false;
                            bounds.Y = e.AffectedBounds.Y;
                        }
                        else
                        {
                            toolStrip.GripMargin = new Padding(0, 2, 0, 2);
                            bounds.Y = e.AffectedBounds.Y;
                            bounds.Height = e.AffectedBounds.Height;
                        }
                    }
                    else
                    {
                        toolStrip.GripMargin = new Padding(2, 2, 4, 2);
                        bounds.X++;
                        bounds.Width++;
                    }
                }
                else
                {
                    bounds.Y = e.AffectedBounds.Y;
                    bounds.Height = e.AffectedBounds.Height;
                }

                DrawDottedGrip(
                    graphics,
                    bounds,
                    vert,
                    false,
                    StyleSheet.HeadToolStripItemInnerBorderColor,
                    ControlPaint.Dark(StyleSheet.HeadToolStripBackColor, 0.3F));
            }
        }

        protected override void OnRenderStatusStripSizingGrip(ToolStripRenderEventArgs e)
        {
            DrawSolidStatusGrip(
                e.Graphics,
                e.AffectedBounds,
                StyleSheet.HeadToolStripItemInnerBorderColor,
                ControlPaint.Dark(StyleSheet.HeadToolStripBackColor, 0.3f));
        }

        internal void RenderSeparatorLine(
            Graphics graphics,
            Rectangle rect,
            Color baseColor,
            Color backColor,
            Color shadowColor,
            bool vertical)
        {
            if (vertical)
            {
                rect.Y += 2;
                rect.Height -= 4;
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    rect,
                    baseColor,
                    backColor,
                    LinearGradientMode.Vertical))
                {
                    using (Pen pen = new Pen(brush))
                    {
                        graphics.DrawLine(pen, rect.X, rect.Y, rect.X, rect.Bottom);
                    }
                }
            }
            else
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    rect,
                    baseColor,
                    backColor,
                    180F))
                {
                    Blend blend = new Blend();
                    blend.Positions = new float[] { 0f, .2f, .5f, .8f, 1f };
                    blend.Factors = new float[] { 1f, .3f, 0f, .3f, 1f };
                    brush.Blend = blend;
                    using (Pen pen = new Pen(brush))
                    {
                        graphics.DrawLine(pen, rect.X, rect.Y, rect.Right, rect.Y);

                        brush.LinearColors = new Color[] {
                        shadowColor, backColor };
                        pen.Brush = brush;
                        graphics.DrawLine(pen, rect.X, rect.Y + 1, rect.Right, rect.Y + 1);
                    }
                }
            }
        }

        internal void RenderOverflowBackground(ToolStripItemRenderEventArgs e,bool rightToLeft)
        {
            Color color = Color.Empty;
            Graphics graphics = e.Graphics;
            ToolStrip toolStrip = e.ToolStrip;
            ToolStripOverflowButton item = e.Item as ToolStripOverflowButton;
            Rectangle bounds = new Rectangle(Point.Empty, item.Size);
            Rectangle withinBounds = bounds;
            bool bParentIsMenuStrip = !(item.GetCurrentParent() is MenuStrip);
            bool bHorizontal = toolStrip.Orientation == Orientation.Horizontal;

            if (bHorizontal)
            {
                bounds.X += (bounds.Width - 12) + 1;
                bounds.Width = 12;
                if (rightToLeft)
                {
                    bounds = GraphicsHelper.RTLTranslate(bounds, withinBounds);
                }
            }
            else
            {
                bounds.Y = (bounds.Height - 12) + 1;
                bounds.Height = 12;
            }

            if (item.Pressed)
            {
                color = StyleSheet.HeadToolStripItemPressedBackColor;
            }
            else if (item.Selected)
            {
                color = StyleSheet.HeadToolStripItemHoveringBackColor;
            }
            else
            {
                color = StyleSheet.HeadToolStripBackColor;
            }
            if (bParentIsMenuStrip)
            {
                using (Pen pen = new Pen(StyleSheet.HeadToolStripBackColor))
                {
                    Point point = new Point(bounds.Left - 1, bounds.Height - 2);
                    Point point2 = new Point(bounds.Left, bounds.Height - 2);
                    if (rightToLeft)
                    {
                        point.X = bounds.Right + 1;
                        point2.X = bounds.Right;
                    }
                    graphics.DrawLine(pen, point, point2);
                }
            }

            LinearGradientMode mode = bHorizontal ? LinearGradientMode.Vertical : LinearGradientMode.Horizontal;

            GraphicsHelper.DrawBackground(
                graphics,
                bounds,
                color,
                StyleSheet.HeadToolStripItemBorderColor,
                StyleSheet.HeadToolStripItemInnerBorderColor,
                RoundStyle.None,
                0,
                .35f,
                false,
                false,
                mode);

            if (bParentIsMenuStrip)
            {
                using (Brush brush = new SolidBrush(StyleSheet.HeadToolStripBackColor))
                {
                    if (bHorizontal)
                    {
                        Point point3 = new Point(bounds.X - 2, 0);
                        Point point4 = new Point(bounds.X - 1, 1);
                        if (rightToLeft)
                        {
                            point3.X = bounds.Right + 1;
                            point4.X = bounds.Right;
                        }
                        graphics.FillRectangle(brush, point3.X, point3.Y, 1, 1);
                        graphics.FillRectangle(brush, point4.X, point4.Y, 1, 1);
                    }
                    else
                    {
                        graphics.FillRectangle(brush, bounds.Width - 3, bounds.Top - 1, 1, 1);
                        graphics.FillRectangle(brush, bounds.Width - 2, bounds.Top - 2, 1, 1);
                    }
                }
                using (Brush brush = new SolidBrush(StyleSheet.HeadToolStripBackColor))
                {
                    if (bHorizontal)
                    {
                        Rectangle rect = new Rectangle(bounds.X - 1, 0, 1, 1);
                        if (rightToLeft)
                        {
                            rect.X = bounds.Right;
                        }
                        graphics.FillRectangle(brush, rect);
                    }
                    else
                    {
                        graphics.FillRectangle(brush, bounds.X, bounds.Top - 1, 1, 1);
                    }
                }
            }
        }

        private void DrawDottedGrip(
            Graphics graphics,
            Rectangle bounds,
            bool vertical,
            bool largeDot,
            Color innerColor,
            Color outerColor)
        {
            bounds.Height -= 3;
            Point position = new Point(bounds.X, bounds.Y);
            int sep;
            Rectangle posRect = new Rectangle(0, 0, 2, 2);

            using (SmoothingModeGraphics sg = new SmoothingModeGraphics(graphics))
            {
                IntPtr hdc;

                if (vertical)
                {
                    sep = bounds.Height;
                    position.Y += 8;
                    for (int i = 0; position.Y > 4; i += 4)
                    {
                        position.Y = sep - (2 + i);
                        if (largeDot)
                        {
                            posRect.Location = position;
                            DrawCircle(
                                graphics,
                                posRect,
                                outerColor,
                                innerColor);
                        }
                        else
                        {
                            int innerWin32Corlor = ColorTranslator.ToWin32(innerColor);
                            int outerWin32Corlor = ColorTranslator.ToWin32(outerColor);

                            hdc = graphics.GetHdc();

                            SetPixel(
                                hdc,
                                position.X,
                                position.Y,
                                innerWin32Corlor);
                            SetPixel(
                                hdc,
                                position.X + 1,
                                position.Y,
                                outerWin32Corlor);
                            SetPixel(
                                hdc,
                                position.X,
                                position.Y + 1,
                                outerWin32Corlor);

                            SetPixel(
                                hdc,
                                position.X + 3,
                                position.Y,
                                innerWin32Corlor);
                            SetPixel(
                                hdc,
                                position.X + 4,
                                position.Y,
                                outerWin32Corlor);
                            SetPixel(
                                hdc,
                                position.X + 3,
                                position.Y + 1,
                                outerWin32Corlor);

                            graphics.ReleaseHdc(hdc);
                        }
                    }
                }
                else
                {
                    bounds.Inflate(-2, 0);
                    sep = bounds.Width;
                    position.X += 2;

                    for (int i = 1; position.X > 0; i += 4)
                    {
                        position.X = sep - (2 + i);
                        if (largeDot)
                        {
                            posRect.Location = position;
                            DrawCircle(graphics, posRect, outerColor, innerColor);
                        }
                        else
                        {
                            int innerWin32Corlor = ColorTranslator.ToWin32(innerColor);
                            int outerWin32Corlor = ColorTranslator.ToWin32(outerColor);
                            hdc = graphics.GetHdc();

                            SetPixel(
                                hdc,
                                position.X,
                                position.Y,
                                innerWin32Corlor);
                            SetPixel(
                                hdc,
                                position.X + 1,
                                position.Y,
                                outerWin32Corlor);
                            SetPixel(
                                hdc,
                                position.X,
                                position.Y + 1,
                                outerWin32Corlor);

                            SetPixel(
                                hdc,
                                position.X + 3,
                                position.Y,
                                innerWin32Corlor);
                            SetPixel(
                                hdc,
                                position.X + 4,
                                position.Y,
                                outerWin32Corlor);
                            SetPixel(
                                hdc,
                                position.X + 3,
                                position.Y + 1,
                                outerWin32Corlor);

                            graphics.ReleaseHdc(hdc);
                        }
                    }
                }
            }
        }

        private void DrawCircle(
            Graphics graphics,
            Rectangle bounds,
            Color borderColor,
            Color fillColor)
        {
            using (GraphicsPath circlePath = new GraphicsPath())
            {
                circlePath.AddEllipse(bounds);
                circlePath.CloseFigure();

                using (Pen borderPen = new Pen(borderColor))
                {
                    graphics.DrawPath(borderPen, circlePath);
                }

                using (Brush backBrush = new SolidBrush(fillColor))
                {
                    graphics.FillPath(backBrush, circlePath);
                }
            }
        }

        private void DrawDottedStatusGrip(
            Graphics graphics,
            Rectangle bounds,
            Color innerColor,
            Color outerColor)
        {
            Rectangle shape = new Rectangle(0, 0, 2, 2);
            shape.X = bounds.Width - 17;
            shape.Y = bounds.Height - 8;
            using (SmoothingModeGraphics sg = new SmoothingModeGraphics(graphics))
            {
                DrawCircle(graphics, shape, outerColor, innerColor);

                shape.X = bounds.Width - 12;
                DrawCircle(graphics, shape, outerColor, innerColor);

                shape.X = bounds.Width - 7;
                DrawCircle(graphics, shape, outerColor, innerColor);

                shape.Y = bounds.Height - 13;
                DrawCircle(graphics, shape, outerColor, innerColor);

                shape.Y = bounds.Height - 18;
                DrawCircle(graphics, shape, outerColor, innerColor);

                shape.Y = bounds.Height - 13;
                shape.X = bounds.Width - 12;
                DrawCircle(graphics, shape, outerColor, innerColor);
            }
        }

        private void DrawSolidStatusGrip(
            Graphics graphics,
            Rectangle bounds,
            Color innerColor,
            Color outerColor
            )
        {
            using (SmoothingModeGraphics sg = new SmoothingModeGraphics(graphics))
            {
                using (Pen innerPen = new Pen(innerColor),outerPen = new Pen(outerColor))
                {
                    //outer line
                    graphics.DrawLine(
                        outerPen,
                        new Point(bounds.Width - 14, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 16));
                    graphics.DrawLine(
                        innerPen,
                        new Point(bounds.Width - 13, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 15));
                    // line
                    graphics.DrawLine(
                        outerPen,
                        new Point(bounds.Width - 12, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 14));
                    graphics.DrawLine(
                        innerPen,
                        new Point(bounds.Width - 11, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 13));
                    // line
                    graphics.DrawLine(
                        outerPen,
                        new Point(bounds.Width - 10, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 12));
                    graphics.DrawLine(
                        innerPen,
                        new Point(bounds.Width - 9, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 11));
                    // line
                    graphics.DrawLine(
                        outerPen,
                        new Point(bounds.Width - 8, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 10));
                    graphics.DrawLine(
                        innerPen,
                        new Point(bounds.Width - 7, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 9));
                    // inner line
                    graphics.DrawLine(
                        outerPen,
                        new Point(bounds.Width - 6, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 8));
                    graphics.DrawLine(
                        innerPen,
                        new Point(bounds.Width - 5, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 7));
                }
            }
        }

        internal bool DrawLogo(ToolStrip toolStrip)
        {
            ToolStripDropDown dropDown = toolStrip as ToolStripDropDown;
            bool drawLogo =(dropDown.OwnerItem != null
                &&dropDown.OwnerItem.Owner is MenuStrip) 
                ||(toolStrip is ContextMenuStrip);
            //return drawLogo;
            return false;
        }

        [DllImport("gdi32.dll")]
        private static extern uint SetPixel(IntPtr hdc, int X, int Y, int crColor);
    }


}
