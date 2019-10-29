using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using Ligg.Winform.DataModel.Enums;
using Ligg.Winform.Skin;


namespace Ligg.Winform.Controls.CheckBoxComboBox
{
    [CLSCompliant(true), ToolboxItem(false)]
    public partial class PopupStrip : ToolStripDropDown
    {
        #region " Fields & Properties "
        private readonly Control _content;

        public Control Content
        {
            get { return _content; }
        }

        private bool _fade;
        public bool UseFadeEffect
        {
            get { return _fade; }
            set
            {
                if (_fade == value) return;
                _fade = value;
            }
        }

        private bool _focusOnOpen = true;
        public bool FocusOnOpen
        {
            get { return _focusOnOpen; }
            set { _focusOnOpen = value; }
        }

        private bool _acceptAlt = true;
        public bool AcceptAlt
        {
            get { return _acceptAlt; }
            set { _acceptAlt = value; }
        }

        private PopupStrip _ownerPopup;
        private PopupStrip _childPopup;

        private bool _resizable;
        private bool _resizable1;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PopupControl.DropDownStrip" /> is resizable.
        /// </summary>
        /// <value><c>true</c> if resizable; otherwise, <c>false</c>.</value>
        public bool Resizable
        {
            get { return _resizable1 && _resizable; }
            set { _resizable1 = value; }
        }

        private ToolStripControlHost host;

        /// <summary>
        /// Gets or sets the size that is the lower limit that <see cref="M:System.Windows.Forms.Control.GetPreferredSize(System.Drawing.Size)" /> can specify.
        /// </summary>
        /// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
        public new Size MinimumSize { get; set; }

        private Size _maxSize;
        /// <summary>
        /// Gets or sets the size that is the upper limit that <see cref="M:System.Windows.Forms.Control.GetPreferredSize(System.Drawing.Size)" /> can specify.
        /// </summary>
        /// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
        public new Size MaximumSize
        {
            get { return _maxSize; }
            set { _maxSize = value; }
        }

        /// <summary>
        /// Gets parameters of a new window.
        /// </summary>
        /// <returns>An object of type <see cref="T:System.Windows.Forms.CreateParams" /> used when creating a new window.</returns>
        protected override CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= NativeMethods.WS_EX_NOACTIVATE;
                return cp;
            }
        }

        #endregion

        #region " Constructors "

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupControl.DropDownStrip"/> class.
        /// </summary>
        /// <param name="content">The content of the pop-up.</param>
        /// <remarks>
        /// Pop-up will be disposed immediately after disposion of the content control.
        /// </remarks>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="content" /> is <code>null</code>.</exception>
        public PopupStrip(Control content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }
            this._content = content;
            this._fade = SystemInformation.IsMenuAnimationEnabled && SystemInformation.IsMenuFadeEnabled;
            this._resizable = true;
            InitializeComponent();
            AutoSize = false;
            DoubleBuffered = true;
            ResizeRedraw = true;
            host = new ToolStripControlHost(content);
            Padding = Margin = host.Padding = host.Margin = Padding.Empty;
            MinimumSize = content.MinimumSize;
            content.MinimumSize = content.Size;
            MaximumSize = content.MaximumSize;
            content.MaximumSize = content.Size;
            Size = content.Size;
            content.Location = Point.Empty;
            Items.Add(host);
            content.Disposed += delegate(object sender, EventArgs e)
            {
                content = null;
                Dispose(true);
            };
            content.RegionChanged += delegate(object sender, EventArgs e)
            {
                UpdateRegion();
            };
            content.Paint += delegate(object sender, PaintEventArgs e)
            {
                PaintSizeGrip(e);
            };
            UpdateRegion();
        }

        #endregion

        #region " Methods "

        /// Processes a dialog box key.
        /// true if the key was processed by the control; otherwise, false.
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (_acceptAlt && ((keyData & Keys.Alt) == Keys.Alt)) return false;
            return base.ProcessDialogKey(keyData);
        }

        /// Updates the pop-up region.
        protected void UpdateRegion()
        {
            if (this.Region != null)
            {
                this.Region.Dispose();
                this.Region = null;
            }
            if (_content.Region != null)
            {
                this.Region = _content.Region.Clone();
            }
        }

        public void Show(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            SetOwnerItem(control);
            Show(control, control.ClientRectangle, DropDownStripAlignType.Left);
        }

        public void Show(Control control, DropDownStripAlignType alignType)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            SetOwnerItem(control);
            Show(control, control.ClientRectangle, alignType);
        }


        public void Show(Control control, Rectangle area, DropDownStripAlignType alignType)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            SetOwnerItem(control);

            Point location = control.PointToScreen(new Point(area.Left, area.Top + area.Height));
            Rectangle screen = Screen.FromControl(control).WorkingArea;

            resizableTop = resizableRight = false;
            if (alignType == DropDownStripAlignType.Left)
            {
                //if (location.X + Size.Width > screen.Left + screen.Width)
                //{
                //    resizableRight = true;
                //    location.X = screen.Left + screen.Width - Size.Width;
                //}
            }
            else if (alignType == DropDownStripAlignType.Center)
            {
                //if (location.X + (area.Width + Size.Width) / 2 > screen.Right)
                //{
                //    resizableRight = true;
                //    location.X = screen.Right - Size.Width;
                //}
                //else
                //{
                //    resizableRight = true;
                //    location.X = location.X - (Size.Width - area.Width) / 2;
                //}
                resizableRight = true;
                location.X = location.X - (Size.Width - area.Width) / 2;
            }
            else //right
            {
                resizableRight = true;
                location.X = location.X - Size.Width + area.Width;
            }

            if (location.Y + Size.Height > (screen.Top + screen.Height))
            {
                resizableTop = true;
                location.Y -= Size.Height + area.Height;
            }
            location = control.PointToClient(location);
            Show(control, location, ToolStripDropDownDirection.BelowRight);
        }

        private const int frames = 1;
        private const int totalduration = 0; // ML : 2007-11-05 : was 100 but caused a flicker.
        private const int frameduration = totalduration / frames;

        protected override void SetVisibleCore(bool visible)
        {
            double opacity = Opacity;
            if (visible && _fade && _focusOnOpen) Opacity = 0;
            base.SetVisibleCore(visible);
            if (!visible || !_fade || !_focusOnOpen) return;
            for (int i = 1; i <= frames; i++)
            {
                if (i > 1)
                {
                    System.Threading.Thread.Sleep(frameduration);
                }
                Opacity = opacity * (double)i / (double)frames;
            }
            Opacity = opacity;
        }

        private bool resizableTop;
        private bool resizableRight;

        private void SetOwnerItem(Control control)
        {
            if (control == null)
            {
                return;
            }
            if (control is PopupStrip)
            {
                PopupStrip popupControl = control as PopupStrip;
                _ownerPopup = popupControl;
                _ownerPopup._childPopup = this;
                OwnerItem = popupControl.Items[0];
                return;
            }
            if (control.Parent != null)
            {
                SetOwnerItem(control.Parent);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            _content.MinimumSize = Size;
            _content.MaximumSize = Size;
            _content.Size = Size;
            _content.Location = Point.Empty;
            base.OnSizeChanged(e);
        }

        protected override void OnOpening(CancelEventArgs e)
        {
            if (_content.IsDisposed || _content.Disposing)
            {
                e.Cancel = true;
                return;
            }
            UpdateRegion();
            base.OnOpening(e);
        }

        protected override void OnOpened(EventArgs e)
        {
            if (_ownerPopup != null)
            {
                _ownerPopup._resizable = false;
            }
            if (_focusOnOpen)
            {
                _content.Focus();
            }
            base.OnOpened(e);
        }

        protected override void OnClosed(ToolStripDropDownClosedEventArgs e)
        {
            if (_ownerPopup != null)
            {
                _ownerPopup._resizable = true;
            }
            base.OnClosed(e);
        }

        public DateTime LastClosedTimeStamp = DateTime.Now;

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible == false)
                LastClosedTimeStamp = DateTime.Now;
            base.OnVisibleChanged(e);
        }

        #endregion

        #region " Resizing Support "
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            if (InternalProcessResizing(ref m, false))
            {
                return;
            }
            base.WndProc(ref m);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public bool ProcessResizing(ref Message m)
        {
            return InternalProcessResizing(ref m, true);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private bool InternalProcessResizing(ref Message m, bool contentControl)
        {
            if (m.Msg == NativeMethods.WM_NCACTIVATE && m.WParam != IntPtr.Zero && _childPopup != null && _childPopup.Visible)
            {
                _childPopup.Hide();
            }
            if (!Resizable)
            {
                return false;
            }
            if (m.Msg == NativeMethods.WM_NCHITTEST)
            {
                return OnNcHitTest(ref m, contentControl);
            }
            else if (m.Msg == NativeMethods.WM_GETMINMAXINFO)
            {
                return OnGetMinMaxInfo(ref m);
            }
            return false;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private bool OnGetMinMaxInfo(ref Message m)
        {
            NativeMethods.MINMAXINFO minmax = (NativeMethods.MINMAXINFO)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.MINMAXINFO));
            minmax.maxTrackSize = this.MaximumSize;
            minmax.minTrackSize = this.MinimumSize;
            Marshal.StructureToPtr(minmax, m.LParam, false);
            return true;
        }

        private bool OnNcHitTest(ref Message m, bool contentControl)
        {
            int x = NativeMethods.LOWORD(m.LParam);
            int y = NativeMethods.HIWORD(m.LParam);
            Point clientLocation = PointToClient(new Point(x, y));

            GripBounds gripBouns = new GripBounds(contentControl ? _content.ClientRectangle : ClientRectangle);
            IntPtr transparent = new IntPtr(NativeMethods.HTTRANSPARENT);

            if (resizableTop)
            {
                if (resizableRight && gripBouns.TopLeft.Contains(clientLocation))
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTTOPLEFT;
                    return true;
                }
                if (!resizableRight && gripBouns.TopRight.Contains(clientLocation))
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTTOPRIGHT;
                    return true;
                }
                if (gripBouns.Top.Contains(clientLocation))
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTTOP;
                    return true;
                }
            }
            else
            {
                if (resizableRight && gripBouns.BottomLeft.Contains(clientLocation))
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTBOTTOMLEFT;
                    return true;
                }
                if (!resizableRight && gripBouns.BottomRight.Contains(clientLocation))
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTBOTTOMRIGHT;
                    return true;
                }
                if (gripBouns.Bottom.Contains(clientLocation))
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTBOTTOM;
                    return true;
                }
            }
            if (resizableRight && gripBouns.Left.Contains(clientLocation))
            {
                m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTLEFT;
                return true;
            }
            if (!resizableRight && gripBouns.Right.Contains(clientLocation))
            {
                m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTRIGHT;
                return true;
            }
            return false;
        }

        private VisualStyleRenderer sizeGripRenderer;
        /// <summary>
        /// Paints the size grip.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs" /> instance containing the event data.</param>
        public void PaintSizeGrip(PaintEventArgs e)
        {
            if (e == null || e.Graphics == null || !_resizable1)
            {
                return;
            }
            Size clientSize = _content.ClientSize;
            if (Application.RenderWithVisualStyles)
            {
                if (this.sizeGripRenderer == null)
                {
                    this.sizeGripRenderer = new VisualStyleRenderer(VisualStyleElement.Status.Gripper.Normal);
                }
                this.sizeGripRenderer.DrawBackground(e.Graphics, new Rectangle(clientSize.Width - 0x10, clientSize.Height - 0x10, 0x10, 0x10));
            }
            else
            {
                ControlPaint.DrawSizeGrip(e.Graphics, _content.BackColor, clientSize.Width - 0x10, clientSize.Height - 0x10, 0x10, 0x10);
            }
        }

        #endregion


        #region Design.cs
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion
    }
}
