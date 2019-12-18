using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Ligg.WinForm.DataModel.Enums;
using Ligg.WinForm.Skin;

namespace Ligg.WinForm.Controls
{
    public class TextButton : Button
    {
       
        public TextButton()
            : base()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);

        }

        private RoundStyle _roundStyle;
        //[DefaultValue(typeof(RoundStyle), "1")]
        public RoundStyle RoundStyle
        {
            get { return _roundStyle; }
            set
            {
                _roundStyle = value;
            }
        }

        private int _radius;
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

        private bool _checked;
        public bool Checked
        {
            get { return _checked; }
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

        public bool HasBorder { get; set; }

        public ControlSensitiveType SensitiveType
        {
            get;
            set;
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
            _checked = true;
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
                    if (SensitiveType == ControlSensitiveType.Focus)
                    {
                        ControlState = Focused ? ControlState.Normal : ControlState.Hovering;
                    }
                    else if (SensitiveType == ControlSensitiveType.Check)
                    {
                        ControlState = Checked ? ControlState.Normal : ControlState.Hovering;
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
            //base.OnPaint(e);
            //base.OnPaintBackground(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color backColor;
            Color borderColor = StyleSheet.ControlBorderColor;
            Color innerBorderColor = StyleSheet.ControlInnerBorderColor;
            if (Enabled)
            {
                switch (ControlState)
                {
                    case ControlState.Hovering:
                        backColor = StyleSheet.ControlHoveringBackColor;
                        break;
                    case ControlState.Pressed:
                        backColor = StyleSheet.ControlFocusedBackColor;
                        break;

                    default: //normal
                        {
                            if (SensitiveType == ControlSensitiveType.Focus && Focused)
                            {
                                backColor = innerBorderColor = StyleSheet.ControlFocusedBackColor;
                            }
                            else if (SensitiveType == ControlSensitiveType.Check && Checked)
                            {
                                backColor = innerBorderColor = StyleSheet.ControlFocusedBackColor;
                            }
                            else
                            {
                                backColor = BackColor;
                                if (!HasBorder)
                                {
                                    borderColor = BackColor;
                                    innerBorderColor = BackColor;
                                }
                                else
                                {
                                    borderColor = StyleSheet.ControlBorderColor;
                                    innerBorderColor = StyleSheet.ControlInnerBorderColor;
                                }
                            }
                        }
                        break;
                }
            }
            else
            {
                backColor = StyleSheet.ControlDisabledBaseColor;
            }


            GraphicsHelper.DrawBackground(
                g,
                ClientRectangle,
                backColor,
                borderColor,
                innerBorderColor,
                _roundStyle,
                _radius,
                0,
                true,
                true,
                LinearGradientMode.Vertical);
            Rectangle textRect  = new Rectangle(
                        2,
                        2,
                        Width - 4,
                        Height - 4); ;

            TextRenderer.DrawText(
                 g,
                 Text,
                 Font,
                 textRect,
                 ForeColor,
                 GetTextFormatFlags(TextAlign, RightToLeft == RightToLeft.Yes));
        }

        private  TextFormatFlags GetTextFormatFlags(ContentAlignment alignment, bool rightToleft)
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

