using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Ligg.Base.Extension;
using Ligg.Winform.DataModel.Enums;
using Ligg.Winform.Skin;

namespace Ligg.Winform.Controls
{
    public class ImageTextButton : Button
    {
        public ImageTextButton()
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
        public RoundStyle RoundStyle
        {
            get { return _roundStyle; }
            set
            {
                _roundStyle = value;
            }
        }

        //[DefaultValue(8)]
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

        private int _textOffset=2;
        private int _imageOffset=2;
        public string Offset
        {
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    string str = value;
                    var arry = str.Split(',');
                    _textOffset = (arry[0].IsPlusInteger() | arry[0] == "0") ? Convert.ToInt16(arry[0]) : 2;
                    if (arry.Length <2)
                    {
                        _imageOffset = 2;
                    }
                    else
                    {
                        _imageOffset = (arry[1].IsPlusInteger() | arry[1] == "0") ? Convert.ToInt16(arry[1]) : 2;
                    }
                    
                }
                else
                {
                    _textOffset = 2;
                    _imageOffset = 2;
                }
            }
        }

        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }

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
            ControlState =ControlState.Normal;
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
            Color borderColor = StyleSet.ControlBorderColor;
            Color innerBorderColor = StyleSet.ControlInnerBorderColor;
            if (Enabled)
            {
                switch (ControlState)
                {
                    case ControlState.Hovering:
                        backColor = StyleSet.ControlHoveringBackColor;
                        break;
                    case ControlState.Pressed:
                        backColor = StyleSet.ControlFocusedBackColor;
                        break;
                    default: //normal
                        {
                            if (SensitiveType == ControlSensitiveType.Focus && Focused)
                            {
                                backColor = innerBorderColor = StyleSet.ControlFocusedBackColor;
                            }
                            else if (SensitiveType == ControlSensitiveType.Check && Checked)
                            {
                                backColor = innerBorderColor = StyleSet.ControlFocusedBackColor;
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
                                    borderColor = StyleSet.ControlBorderColor;
                                    innerBorderColor = StyleSet.ControlInnerBorderColor;
                                }
                            }
                        }
                        break;
                }
            }
            else
            {
                backColor = StyleSet.ControlDisabledBaseColor;
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
            Rectangle imageRect;
            Rectangle textRect;

            CalculateRect(out imageRect, out textRect);
            if (Image != null)
            {
                g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                g.DrawImage(
                    Image,
                    imageRect,
                    0,
                    0,
                    Image.Width,
                    Image.Height,
                    GraphicsUnit.Pixel);
            }

            TextRenderer.DrawText(
                 g,
                 Text,
                 Font,
                 textRect,
                 ForeColor,
                 GetTextFormatFlags(TextAlign, RightToLeft == RightToLeft.Yes));
        }

        private void CalculateRect(out Rectangle imageRect, out Rectangle textRect)
        {
            var imageWidth = 0;
            var imageHeight = 0;
            imageRect = Rectangle.Empty;
            textRect = Rectangle.Empty;
            if (Image == null)
            {
                textRect = new Rectangle(
                   _textOffset,
                   0,
                   Width - _textOffset,
                   Height);
                return;
            }
            
            switch (TextImageRelation)
            {
                case TextImageRelation.Overlay:
                    {
                        imageWidth = (ImageWidth < 0 | ImageWidth > Width - _imageOffset) ? Width : ImageWidth;
                        imageHeight = (ImageHeight < 0 | ImageHeight > Height) ? Height : ImageHeight;
                        imageRect = new Rectangle(
                        _imageOffset,
                        (Height - imageHeight) / 2,
                        imageWidth,
                        imageHeight);

                        textRect = new Rectangle(
                            _textOffset,
                            0,
                            Width - _textOffset,
                            Height);
                        break;
                    }

                case TextImageRelation.ImageAboveText:
                    {
                        imageWidth = (ImageWidth < 0 | ImageWidth > Width) ? Convert.ToInt16(Width / 1.6) : ImageWidth;
                        imageHeight = (ImageHeight < 0 | ImageHeight > Height - _textOffset - _imageOffset) ? imageWidth : ImageHeight;
                        imageRect = new Rectangle(
                            (Width - imageWidth) / 2,
                            _imageOffset,
                            imageWidth,
                            imageHeight);

                        textRect = new Rectangle(
                            0,
                            imageRect.Bottom+_textOffset,
                            Width,
                            Height - imageRect.Bottom - _imageOffset - _textOffset);
                        break;
                    }

                case TextImageRelation.TextAboveImage:
                    {
                        imageWidth = (ImageWidth < 0 | ImageWidth > Width) ? Convert.ToInt16(Width / 1.6) : ImageWidth;
                        imageHeight = (ImageHeight < 0 | ImageHeight > Height - _textOffset - _imageOffset) ? imageWidth : ImageHeight;

                        textRect = new Rectangle(
                        0,
                        _textOffset,
                        Width,
                        Height - imageHeight - _imageOffset - _textOffset);
                        imageRect = new Rectangle(
                            (Width - imageWidth) / 2,
                            textRect.Bottom + _imageOffset,
                            imageWidth,
                            imageHeight);

                        break;

                    }

                case TextImageRelation.ImageBeforeText:
                    {
                        imageHeight = (ImageHeight < 0 | ImageHeight > Height) ? Convert.ToInt16(Height / 1.4) : ImageHeight;
                        imageWidth = (ImageWidth < 0 | ImageWidth > Width - _textOffset - _imageOffset) ? imageHeight : ImageWidth;
                        imageRect = new Rectangle(
                            _imageOffset,
                            (Height - imageHeight) / 2,
                            imageWidth,
                            imageHeight);
                        textRect = new Rectangle(
                            imageRect.Right+_textOffset,
                            0,
                            Width - imageRect.Right - _textOffset,
                            Height);
                        break;
                    }


                case TextImageRelation.TextBeforeImage:
                    {
                        imageHeight = (ImageHeight < 0 | ImageHeight > Height) ? Convert.ToInt16(Height / 1.4) : ImageHeight;
                        imageWidth = (ImageWidth < 0 | ImageWidth > Width - _textOffset - _imageOffset) ? imageHeight : ImageWidth;

                        textRect = new Rectangle(
                         _textOffset,
                        0,
                        Width - _imageOffset - _textOffset - imageWidth,
                        Height);

                        imageRect = new Rectangle(
                            textRect.Right + _imageOffset,
                            (Height - imageHeight) / 2,
                            imageWidth,
                            imageHeight);
                        break;
                    }
            }

            if (RightToLeft == RightToLeft.Yes)
            {
                imageRect.X = Width - imageRect.Right;
                textRect.X = Width - textRect.Right;
            }
        }

        internal  TextFormatFlags GetTextFormatFlags(ContentAlignment alignment, bool rightToleft)
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

