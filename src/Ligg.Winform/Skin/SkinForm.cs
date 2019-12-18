using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Ligg.WinForm.DataModel.Enums;

namespace Ligg.WinForm.Skin
{
    public class SkinForm : Form
    {
        #region Fields
        private bool _isActive;
        private bool _isWindowPosChanged;

        private int _borderWidth = 2;
        private int _captionHeight = 28;
        private Font _captionFont = SystemFonts.CaptionFont;
        private Size _minimizeBoxSize = new Size(24, 18);
        private Size _maximizeBoxSize = new Size(24, 18);
        
        private Size _closeBoxSize = new Size(24, 18);
        private Point _controlBoxOffset = new Point(4, 0);
        private int _controlBoxSpace = -1;

        private RoundStyle _roundStyle = RoundStyle.None;
        private int _radius = 0;

        private FormRenderer _renderer;
        private MmcControlBoxManager _mmcControlBoxManager;

        private Padding _padding;
        private ToolTip _toolTip;
        private static readonly object EventRendererChanged = new object();
        #endregion

        #region Properties
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public FormRenderer Renderer
        {
            get { return _renderer ?? (_renderer = new FormPainter()); }
            set
            {
                _renderer = value;
                OnRendererChanged(EventArgs.Empty);
            }
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                base.Invalidate(new Rectangle(
                    0,
                    0,
                    Width,
                    CaptionHeight + 1));
            }
        }

        [DefaultValue(true)]
        public bool Resizable{ get; set; }

        [DefaultValue(false)]
        public bool DrawCationBackground { get; set; }

        [DefaultValue(false)]
        public bool DrawIcon { get; set; }

        [DefaultValue(typeof(RoundStyle), "0")]
        public RoundStyle RoundStyle
        {
            get { return _roundStyle; }
            set
            {
                if (_roundStyle != value)
                {
                    _roundStyle = value;
                    SetRegion();
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(4)]
        public int Radius
        {
            get { return _radius; }
            set
            {
                if (_radius != value)
                {
                    _radius = value < 4 ? 4 : value;
                    SetRegion();
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(24)]
        public int CaptionHeight
        {
            get { return _captionHeight; }
            set
            {
                if (_captionHeight != value)
                {
                    _captionHeight = value < _borderWidth ?
                                    _borderWidth : value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(3)]
        public int BorderWidth
        {
            get { return _borderWidth; }
            set
            {
                if (_borderWidth != value)
                {
                    _borderWidth = value < 1 ? 1 : value;
                }
            }
        }

        [DefaultValue(typeof(Font), "CaptionFont")]
        public Font CaptionFont
        {
            get { return _captionFont; }
            set
            {
                if (value == null)
                {
                    _captionFont = SystemFonts.CaptionFont;
                }
                else
                {
                    _captionFont = value;
                }
                base.Invalidate(CaptionRect);
            }
        }

        public bool TreatCloseBoxAsMinimizeBox { get; set; }
        public bool IsRealClosing { get; set; }

        [DefaultValue(typeof(Size), "32, 18")]
        public Size MinimizeBoxSize
        {
            get { return _minimizeBoxSize; }
            set
            {
                if (_minimizeBoxSize != value)
                {
                    _minimizeBoxSize = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Size), "24, 18")]
        public Size MaximizeBoxSize
        {
            get { return _maximizeBoxSize; }
            set
            {
                if (_maximizeBoxSize != value)
                {
                    _maximizeBoxSize = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Size), "24, 18")]
        public Size CloseBoxSize
        {
            get { return _closeBoxSize; }
            set
            {
                if (_closeBoxSize != value)
                {
                    _closeBoxSize = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Point), "4, 0")]
        public Point ControlBoxOffset
        {
            get { return _controlBoxOffset; }
            set
            {
                if (_controlBoxOffset != value)
                {
                    _controlBoxOffset = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(-1)]
        public int ControlBoxSpace
        {
            get { return _controlBoxSpace; }
            set
            {
                if (_controlBoxSpace != value)
                {
                    _controlBoxSpace = value < 0 ? 0 : value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Padding), "0")]
        public new Padding Padding
        {
            get { return _padding; }
            set
            {
                _padding = value;
                base.Padding = new Padding(
                    BorderWidth + _padding.Left,
                    CaptionHeight + _padding.Top,
                    BorderWidth + _padding.Right,
                    BorderWidth + _padding.Bottom);
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new FormBorderStyle FormBorderStyle
        {
            get { return base.FormBorderStyle; }
            set { base.FormBorderStyle = FormBorderStyle.None; }
        }

        protected override Padding DefaultPadding
        {
            get
            {
                return new Padding(
                    BorderWidth,
                    CaptionHeight,
                    BorderWidth,
                    BorderWidth);
            }
        }

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams parms = base.CreateParams;

                if (!DesignMode)
                {
                    parms.Style |= (int)NativeMethods.WindowStyle.WS_THICKFRAME;

                    if (ControlBox)
                    {
                        parms.Style |= (int)NativeMethods.WindowStyle.WS_SYSMENU;
                    }

                    if (MinimizeBox)
                    {
                        parms.Style |= (int)NativeMethods.WindowStyle.WS_MINIMIZEBOX;
                    }

                    if (!MaximizeBox)
                    {
                        parms.Style &= ~(int)NativeMethods.WindowStyle.WS_MAXIMIZEBOX;
                    }

                    if (_isWindowPosChanged)
                    {
                        parms.Style &= ~((int)NativeMethods.WindowStyle.WS_THICKFRAME | (int)NativeMethods.WindowStyle.WS_SYSMENU);
                        parms.ExStyle &= ~((int)NativeMethods.WindowStyleEx.WS_EX_DLGMODALFRAME | (int)NativeMethods.WindowStyleEx.WS_EX_WINDOWEDGE);
                    }
                }

                return parms;
            }
        }

        internal Rectangle CaptionRect
        {
            get { return new Rectangle(0, 0, Width, CaptionHeight); }
        }

        internal MmcControlBoxManager MmcControlBoxManager
        {
            get
            {
                if (_mmcControlBoxManager == null)
                {
                    _mmcControlBoxManager = new MmcControlBoxManager(this);
                }
                return _mmcControlBoxManager;
            }
        }

        internal Rectangle IconRect
        {
            get
            {
                if (base.ShowIcon && base.Icon != null)
                {
                    int width = SystemInformation.SmallIconSize.Width;
                    if (CaptionHeight - BorderWidth - 4 < width)
                    {
                        width = CaptionHeight - BorderWidth - 4;
                    }
                    return new Rectangle(
                        BorderWidth,
                        BorderWidth + (CaptionHeight - BorderWidth - width) / 2,
                        width,
                        width);
                }
                return Rectangle.Empty;
            }
        }

        internal ToolTip ToolTip
        {
            get { return _toolTip; }
        }
        #endregion

        #region Constructors
        public SkinForm(): base()
        {
            DrawCationBackground = false;

            SetStyles();
            Init();
        }
        #endregion

        #region Events
        public event EventHandler RendererChanged
        {
            add { base.Events.AddHandler(EventRendererChanged, value); }
            remove { base.Events.RemoveHandler(EventRendererChanged, value); }
        }
        #endregion



        #region Override Methods
        protected virtual void OnRendererChanged(EventArgs e)
        {
            Renderer.InitGroundForm(this);
            var handler = base.Events[EventRendererChanged] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
            base.Invalidate();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            SetRegion();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetRegion();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            MmcControlBoxManager.ProcessMouseAction(e.Location, MouseAction.Move);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MmcControlBoxManager.ProcessMouseAction(e.Location, MouseAction.Down);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            MmcControlBoxManager.ProcessMouseAction(e.Location, MouseAction.Up);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            MmcControlBoxManager.ProcessMouseAction(Point.Empty, MouseAction.Leave);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            MmcControlBoxManager.ProcessMouseAction(PointToClient(MousePosition), MouseAction.Hover);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = ClientRectangle;

            FormRenderer renderer = Renderer;
            renderer.DrawGroundFormBackground(new FormBackgroundRenderEventArgs(this, g, rect));
            renderer.DrawGroundFormCaption(new FormCaptionRenderEventArgs(this, g, CaptionRect, _isActive));
            renderer.DrawGroundFormBorder(new FormBorderRenderEventArgs(this, g, rect, _isActive));

            if (MmcControlBoxManager.IsCloseBoxVisibale)
            {
                renderer.DrawGroundFormControlBox(
                    new FormControlBoxRenderEventArgs(
                    this,
                    g,
                    MmcControlBoxManager.CloseBoxRect,
                    _isActive,
                    ControlBoxStyle.Close,
                    MmcControlBoxManager.CloseBoxState));
            }

            if (MmcControlBoxManager.IsMaximizeBoxVisibale)
            {
                renderer.DrawGroundFormControlBox(
                    new FormControlBoxRenderEventArgs(
                    this,
                    g,
                    MmcControlBoxManager.MaximizeBoxRect,
                    _isActive,
                    ControlBoxStyle.Maximize,
                    MmcControlBoxManager.MaximizeBoxState));
            }

            if (MmcControlBoxManager.IsMinimizeBoxVisibale)
            {
                renderer.DrawGroundFormControlBox(
                    new FormControlBoxRenderEventArgs(
                    this,
                    g,
                    MmcControlBoxManager.MinimizeBoxRect,
                    _isActive,
                    ControlBoxStyle.Minimize,
                    MmcControlBoxManager.MinimizeBoxState));
            }
        }

        protected override void WndProc(ref Message msg)
        {
            switch (msg.Msg)
            {
                case (int)NativeMethods.WindowMessages.WM_CLOSE:
                    {
                        if (TreatCloseBoxAsMinimizeBox)
                        {
                            if (!IsRealClosing)
                                this.WindowState = FormWindowState.Minimized;
                            else
                            {
                                base.WndProc(ref msg);
                            }
                        }
                        else base.WndProc(ref msg);
                        break;
                    }
                case (int)NativeMethods.WindowMessages.WM_NCHITTEST:
                    WmNcHit(ref msg);
                    break;
                case (int)NativeMethods.WindowMessages.WM_NCPAINT:
                case (int)NativeMethods.WindowMessages.WM_NCCALCSIZE:
                    break;
                case (int)NativeMethods.WindowMessages.WM_WINDOWPOSCHANGED:
                    _isWindowPosChanged = true;
                    base.WndProc(ref msg);
                    _isWindowPosChanged = false;
                    break;
                case (int)NativeMethods.WindowMessages.WM_GETMINMAXINFO:
                    WmGetMinMaxInfo(ref msg);
                    break;
                case (int)NativeMethods.WindowMessages.WM_NCACTIVATE:
                    WmNcActive(ref msg);
                    break;
                
                default:
                    base.WndProc(ref msg);
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (_mmcControlBoxManager != null)
                {
                    _mmcControlBoxManager.Dispose();
                    _mmcControlBoxManager = null;
                }

                _renderer = null;
                _toolTip.Dispose();
            }
        }

        #endregion

        #region Message Methods
        private void WmNcHit(ref Message m)
        {
            int wparam = m.LParam.ToInt32();
            Point point = new Point(NativeMethods.LOWORD(wparam),NativeMethods.HIWORD(wparam));
            point = PointToClient(point);

            if (IconRect.Contains(point))
            {
                m.Result = new IntPtr(
                    (int)NativeMethods.NCHITTEST.HTSYSMENU);
                return;
            }

            if (Resizable)
            {
                if (point.X < 5 && point.Y < 5)
                {
                    m.Result = new IntPtr(
                        (int)NativeMethods.NCHITTEST.HTTOPLEFT);
                    return;
                }

                if (point.X > Width - 5 && point.Y < 5)
                {
                    m.Result = new IntPtr(
                        (int)NativeMethods.NCHITTEST.HTTOPRIGHT);
                    return;
                }

                if (point.X < 5 && point.Y > Height - 5)
                {
                    m.Result = new IntPtr(
                        (int)NativeMethods.NCHITTEST.HTBOTTOMLEFT);
                    return;
                }

                if (point.X > Width - 5 && point.Y > Height - 5)
                {
                    m.Result = new IntPtr(
                        (int)NativeMethods.NCHITTEST.HTBOTTOMRIGHT);
                    return;
                }

                if (point.Y < 3)
                {
                    m.Result = new IntPtr(
                        (int)NativeMethods.NCHITTEST.HTTOP);
                    return;
                }

                if (point.Y > Height - 3)
                {
                    m.Result = new IntPtr(
                        (int)NativeMethods.NCHITTEST.HTBOTTOM);
                    return;
                }

                if (point.X < 3)
                {
                    m.Result = new IntPtr(
                       (int)NativeMethods.NCHITTEST.HTLEFT);
                    return;
                }

                if (point.X > Width - 3)
                {
                    m.Result = new IntPtr(
                       (int)NativeMethods.NCHITTEST.HTRIGHT);
                    return;
                }
            }

            if (point.Y < CaptionHeight)
            {
                if (!MmcControlBoxManager.CloseBoxRect.Contains(point) &&
                    !MmcControlBoxManager.MaximizeBoxRect.Contains(point) &&
                    !MmcControlBoxManager.MinimizeBoxRect.Contains(point))
                {
                    m.Result = new IntPtr(
                      (int)NativeMethods.NCHITTEST.HTCAPTION);
                    return;
                }
            }
            m.Result = new IntPtr(
                     (int)NativeMethods.NCHITTEST.HTCLIENT);
        }

        private void WmGetMinMaxInfo(ref Message m)
        {
            NativeMethods.MINMAXINFO minmax =
                (NativeMethods.MINMAXINFO)Marshal.PtrToStructure(
                m.LParam, typeof(NativeMethods.MINMAXINFO));

            if (MaximumSize != Size.Empty)
            {
                minmax.maxTrackSize = MaximumSize;
            }
            else
            {
                Rectangle rect = Screen.GetWorkingArea(this);

                minmax.maxPosition = new Point(
                    rect.X - BorderWidth,
                    rect.Y);
                minmax.maxTrackSize = new Size(
                    rect.Width + BorderWidth * 2,
                    rect.Height + BorderWidth);
            }

            if (MinimumSize != Size.Empty)
            {
                minmax.minTrackSize = MinimumSize;
            }
            else
            {
                minmax.minTrackSize = new Size(
                    CloseBoxSize.Width + MinimizeBoxSize.Width +
                    MaximizeBoxSize.Width + ControlBoxOffset.X +
                    ControlBoxSpace * 2 + SystemInformation.SmallIconSize.Width +
                    BorderWidth * 2 + 3,
                    CaptionHeight);
            }

            Marshal.StructureToPtr(minmax, m.LParam, false);
        }

        private void WmNcActive(ref Message m)
        {
            if (m.WParam.ToInt32() == 1)
            {
                _isActive = true;
            }
            else
            {
                _isActive = false;
            }
            m.Result = NativeMethods.TRUE;
            base.Invalidate();
        }

        #endregion

        #region Private Methods
        private void SetStyles()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw, true);
            UpdateStyles();
        }

        private void SetRegion()
        {
            if (base.Region != null)
            {
                base.Region.Dispose();
            }
            base.Region = Renderer.CreateRegion(this);
        }

        private void Init()
        {
            _toolTip = new ToolTip();
            base.FormBorderStyle = FormBorderStyle.None;
            Renderer.InitGroundForm(this);
            base.Padding = DefaultPadding;
        }

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout(); 
        //    this.ClientSize = new System.Drawing.Size(292, 273);
        //    this.Name = "SkinForm";
        //    this.ResumeLayout(false);
        //}
        #endregion

    }

}
