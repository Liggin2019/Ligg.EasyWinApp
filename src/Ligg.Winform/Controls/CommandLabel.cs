using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Ligg.WinForm.DataModel.Enums;

namespace Ligg.WinForm.Controls
{
    public class CommandLabel : Label
    {
        public CommandLabel() : base()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
        }

        private bool _checked;
        private bool Checked
        {
            get => _checked;
            set
            {
                if (_checked != value) //changed
                {
                    _checked = value;
                    if (value == false)
                    {
                        base.Invalidate();
                    }
                }
            }
        }

        private bool _changeColorOnClick = true;
        public bool ChangeColorOnClick
        {
            get { return _changeColorOnClick; }
            set { _changeColorOnClick = value; }
        }

        private bool _hasBottomLine = false;
        public bool HasBottomLine
        {
            get { return _hasBottomLine; }
            set { _hasBottomLine = value; }
        }

        private Color _bottomLineColor = Color.FromArgb(140, 140, 140);
        public Color BottomLineColor
        {
            get { return _bottomLineColor; }
            set { _bottomLineColor = value; }
        }

        private Color _hoveringColor = Color.FromArgb(131, 209, 255);
        public Color HoveringColor
        {
            get { return _hoveringColor; }
            set { _hoveringColor = value; }
        }

        private Color _focusedColor = StyleSheet.BaseColor;
        public Color FocusedColor
        {
            get { return _focusedColor; }
            set { _focusedColor = value; }
        }

        private ControlState _controlState;
        private ControlState ControlState
        {
            get { return _controlState; }
            set
            {
                if (_controlState != value) //changed
                {
                    _controlState = value;
                    base.Invalidate();
                }
            }
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            ControlState = ControlState.Hovering;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ControlState = ControlState.Normal;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _checked = _changeColorOnClick ? true : false;
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                ControlState = ControlState.Pressed;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                if (ClientRectangle.Contains(e.Location))
                {
                    if (Checked)
                    {
                        ControlState = ControlState.Normal;
                    }
                    else
                    {
                        ControlState = ControlState.Hovering;
                    }
                }
                else
                {
                    ControlState = ControlState.Normal;
                }
            }
        }


        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color textColor = ForeColor;
            if (Enabled)
            {
                switch (ControlState)
                {
                    case ControlState.Hovering:
                        textColor = _hoveringColor;
                        break;
                    case ControlState.Pressed:
                        textColor = _focusedColor;
                        break;

                    default: //normal
                        {
                            if (_checked)
                            {
                                textColor = _focusedColor;
                            }
                            else
                            {
                                textColor = ForeColor;

                            }
                        }
                        break;
                }
            }
            else
            {
                textColor = StyleSheet.ControlDisabledBaseColor;
            }

            Rectangle textRect = new Rectangle(
                        0,
                        0,
                        Width,
                        Height);

            TextRenderer.DrawText(
                 g,
                 Text,
                 Font,
                 textRect,
                 textColor,
                 GetTextFormatFlags(TextAlign, RightToLeft == RightToLeft.Yes));

            if (_hasBottomLine)
            {
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                    BackColor, 0, ButtonBorderStyle.Solid,
                    BackColor, 0, ButtonBorderStyle.Solid,
                    BackColor, 0, ButtonBorderStyle.Solid,
                    _bottomLineColor, 1, ButtonBorderStyle.Solid
                );
            }
        }

        //#common
        private TextFormatFlags GetTextFormatFlags(ContentAlignment alignment, bool rightToleft)
        {
            TextFormatFlags flags = TextFormatFlags.WordBreak | TextFormatFlags.SingleLine;
            if (rightToleft)
            {
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            }

            switch (alignment)
            {
                case ContentAlignment.BottomCenter:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.BottomLeft:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
                    break;
                case ContentAlignment.BottomRight:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                    break;
                case ContentAlignment.MiddleCenter:
                    flags |= TextFormatFlags.HorizontalCenter |
                             TextFormatFlags.VerticalCenter;
                    break;
                case ContentAlignment.MiddleLeft:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    break;
                case ContentAlignment.MiddleRight:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                    break;
                case ContentAlignment.TopCenter:
                    flags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.TopLeft:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Left;
                    break;
                case ContentAlignment.TopRight:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Right;
                    break;
            }

            return flags;
        }





    }


}

