using System;
using System.Drawing;
using System.Windows.Forms;
using Ligg.WinForm.DataModel.Enums;
using Ligg.WinForm.Resources;

namespace Ligg.WinForm.Skin
{
    internal class MmcControlBoxManager : IDisposable
    {
        private SkinForm _form;
        private bool _mouseDown;
        private ControlState _closBoxState;
        private ControlState _minimizeBoxState;
        private ControlState _maximizeBoxState;

        public MmcControlBoxManager(SkinForm form)
        {
            _form = form;
        }

        public bool IsCloseBoxVisibale
        {
            get
            {
                return _form.ControlBox;
            }
        }

        public bool IsMaximizeBoxVisibale
        {
            get { return _form.ControlBox && _form.MaximizeBox; }
        }

        public bool IsMinimizeBoxVisibale
        {
            get { return _form.ControlBox && _form.MinimizeBox; }
        }

        public Rectangle CloseBoxRect
        {
            get
            {
                if (IsCloseBoxVisibale)
                {
                    Point offset = ControlBoxOffset;
                    Size size = _form.CloseBoxSize;
                    return new Rectangle(
                        _form.Width - offset.X - size.Width,
                        offset.Y,
                        size.Width,
                        size.Height);
                }
                return Rectangle.Empty;
            }
        }

        public Rectangle MaximizeBoxRect
        {
            get
            {
                if (IsMaximizeBoxVisibale)
                {
                    Point offset = ControlBoxOffset;
                    Size size = _form.MaximizeBoxSize;
                    return new Rectangle(
                        CloseBoxRect.X - ControlBoxSpace - size.Width,
                        offset.Y,
                        size.Width,
                        size.Height);
                }
                return Rectangle.Empty;
            }
        }

        public Rectangle MinimizeBoxRect
        {
            get
            {
                if (IsMinimizeBoxVisibale)
                {
                    Point offset = ControlBoxOffset;
                    Size size = _form.MinimizeBoxSize;
                    int x = IsMaximizeBoxVisibale ?
                        MaximizeBoxRect.X - ControlBoxSpace -  size.Width:
                        CloseBoxRect.X - ControlBoxSpace - size.Width;
                    return new Rectangle(
                        x,
                        offset.Y,
                        size.Width,
                        size.Height);
                }
                return Rectangle.Empty;
            }
        }

        public ControlState CloseBoxState
        {
            get { return _closBoxState; }
            protected set
            {
                if (_closBoxState != value)
                {
                    _closBoxState = value;
                    if (_form != null)
                    {
                        Invalidate(CloseBoxRect);
                    }
                }
            }
        }

        public ControlState MinimizeBoxState
        {
            get { return _minimizeBoxState; }
            protected set
            {
                if (_minimizeBoxState != value)
                {
                    _minimizeBoxState = value;
                    if (_form != null)
                    {
                        Invalidate(MinimizeBoxRect);
                    }
                }
            }
        }

        public ControlState MaximizeBoxState
        {
            get { return _maximizeBoxState; }
            protected set
            {
                if (_maximizeBoxState != value)
                {
                    _maximizeBoxState = value;
                    if (_form != null)
                    {
                        Invalidate(MaximizeBoxRect);
                    }
                }
            }
        }

        internal Point ControlBoxOffset
        {
            get { return _form.ControlBoxOffset; }
        }

        internal int ControlBoxSpace
        {
            get { return _form.ControlBoxSpace; }
        }

        public void ProcessMouseAction(
            Point mousePoint, MouseAction action)
        {
            if (!_form.ControlBox)
            {
                return;
            }

            Rectangle closeBoxRect = CloseBoxRect;
            Rectangle minimizeBoxRect = MinimizeBoxRect;
            Rectangle maximizeBoxRect = MaximizeBoxRect;

            bool closeBoxVisibale = IsCloseBoxVisibale;
            bool minimizeBoxVisibale = IsMinimizeBoxVisibale;
            bool maximizeBoxVisibale = IsMaximizeBoxVisibale;

            switch (action)
            {
                case MouseAction.Move:
                    ProcessMouseMove(
                        mousePoint,
                        closeBoxRect,
                        minimizeBoxRect,
                        maximizeBoxRect,
                        closeBoxVisibale,
                        minimizeBoxVisibale,
                        maximizeBoxVisibale);
                    break;
                case MouseAction.Down:
                    ProcessMouseDown(
                        mousePoint,
                        closeBoxRect,
                        minimizeBoxRect,
                        maximizeBoxRect,
                        closeBoxVisibale,
                        minimizeBoxVisibale,
                        maximizeBoxVisibale);
                    break;
                case MouseAction.Up:
                    ProcessMouseUp(
                        mousePoint,
                        closeBoxRect,
                        minimizeBoxRect,
                        maximizeBoxRect,
                        closeBoxVisibale,
                        minimizeBoxVisibale,
                        maximizeBoxVisibale);
                    break;
                case MouseAction.Leave:
                    ProcessMouseLeave(
                        closeBoxVisibale,
                        minimizeBoxVisibale,
                        maximizeBoxVisibale);
                    break;
                case MouseAction.Hover:
                    break;
            }
        }

        private void ProcessMouseMove(
            Point mousePoint,
            Rectangle closeBoxRect,
            Rectangle minimizeBoxRect,
            Rectangle maximizeBoxRect,
            bool closeBoxVisibale,
            bool minimizeBoxVisibale,
            bool maximizeBoxVisibale)
        {
            string toolTip = string.Empty;
            bool hide = true;
            if (closeBoxVisibale)
            {
                if (closeBoxRect.Contains(mousePoint))
                {
                    hide = false;
                    if (!_mouseDown)
                    {
                        if (CloseBoxState != ControlState.Hovering)
                        {
                            toolTip = WinformRes.Close;
                        }
                        CloseBoxState = ControlState.Hovering;
                    }
                    else
                    {
                        if (CloseBoxState == ControlState.PressedLeave)
                        {
                            CloseBoxState = ControlState.Pressed;
                        }
                    }
                }
                else
                {
                    if (!_mouseDown)
                    {
                        CloseBoxState = ControlState.Normal;
                    }
                    else
                    {
                        if (CloseBoxState == ControlState.Pressed)
                        {
                            CloseBoxState = ControlState.PressedLeave;
                        }
                    }
                }
            }

            if (minimizeBoxVisibale)
            {
                if (minimizeBoxRect.Contains(mousePoint))
                {
                    hide = false;
                    if (!_mouseDown)
                    {
                        if (MinimizeBoxState != ControlState.Hovering)
                        {
                            toolTip = WinformRes.Minimize;
                        }
                        MinimizeBoxState = ControlState.Hovering;
                    }
                    else
                    {
                        if (MinimizeBoxState == ControlState.PressedLeave)
                        {
                            MinimizeBoxState = ControlState.Pressed;
                        }
                    }
                }
                else
                {
                    if (!_mouseDown)
                    {
                        MinimizeBoxState = ControlState.Normal;
                    }
                    else
                    {
                        if (MinimizeBoxState == ControlState.Pressed)
                        {
                            MinimizeBoxState = ControlState.PressedLeave;
                        }
                    }
                }
            }

            if (maximizeBoxVisibale)
            {
                if (maximizeBoxRect.Contains(mousePoint))
                {
                    hide = false;
                    if (!_mouseDown)
                    {
                        if (MaximizeBoxState != ControlState.Hovering)
                        {
                            bool maximize = 
                                _form.WindowState == FormWindowState.Maximized;
                            toolTip = maximize ? WinformRes.Restore : WinformRes.Maximize;
                        }
                        MaximizeBoxState = ControlState.Hovering;
                    }
                    else
                    {
                        if (MaximizeBoxState == ControlState.PressedLeave)
                        {
                            MaximizeBoxState = ControlState.Pressed;
                        }
                    }
                }
                else
                {
                    if (!_mouseDown)
                    {
                        MaximizeBoxState = ControlState.Normal;
                    }
                    else
                    {
                        if (MaximizeBoxState == ControlState.Pressed)
                        {
                            MaximizeBoxState = ControlState.PressedLeave;
                        }
                    }
                }
            }

            if (toolTip != string.Empty)
            {
                HideToolTip();
                ShowTooTip(toolTip);
            }

            if (hide)
            {
                HideToolTip();
            }
        }

        private void ProcessMouseDown(
            Point mousePoint,
            Rectangle closeBoxRect,
            Rectangle minimizeBoxRect,
            Rectangle maximizeBoxRect,
            bool closeBoxVisibale,
            bool minimizeBoxVisibale,
            bool maximizeBoxVisibale)
        {
            _mouseDown = true;

            if (closeBoxVisibale)
            {
                if (closeBoxRect.Contains(mousePoint))
                {
                    CloseBoxState = ControlState.Pressed;
                    return;
                }
            }

            if (minimizeBoxVisibale)
            {
                if (minimizeBoxRect.Contains(mousePoint))
                {
                    MinimizeBoxState = ControlState.Pressed;
                    return;
                }
            }

            if (maximizeBoxVisibale)
            {
                if (maximizeBoxRect.Contains(mousePoint))
                {
                    MaximizeBoxState = ControlState.Pressed;
                    return;
                }
            }
        }

        private void ProcessMouseUp(
            Point mousePoint, 
            Rectangle closeBoxRect,
            Rectangle minimizeBoxRect,
            Rectangle maximizeBoxRect, 
            bool closeBoxVisibale, 
            bool minimizeBoxVisibale, 
            bool maximizeBoxVisibale)
        {
            _mouseDown = false;

            if (closeBoxVisibale)
            {
                if (closeBoxRect.Contains(mousePoint))
                {
                    if (CloseBoxState == ControlState.Pressed)
                    {
                        _form.Close();
                        CloseBoxState = ControlState.Normal;
                        return;
                    }
                }
                CloseBoxState = ControlState.Normal;
            }

            if (minimizeBoxVisibale)
            {
                if (minimizeBoxRect.Contains(mousePoint))
                {
                    if (MinimizeBoxState == ControlState.Pressed)
                    {
                        _form.WindowState = FormWindowState.Minimized;
                        MinimizeBoxState = ControlState.Normal;
                        return;
                    }
                }
                MinimizeBoxState = ControlState.Normal;
            }

            if (maximizeBoxVisibale)
            {
                if (maximizeBoxRect.Contains(mousePoint))
                {
                    if (MaximizeBoxState == ControlState.Pressed)
                    {
                        bool maximize =
                            _form.WindowState == FormWindowState.Maximized;
                        if (maximize)
                        {
                            _form.WindowState = FormWindowState.Normal;
                        }
                        else
                        {
                            _form.WindowState = FormWindowState.Maximized;
                        }

                        MaximizeBoxState = ControlState.Normal;
                        return;
                    }
                }
                MaximizeBoxState = ControlState.Normal;
            }
        }

        private void ProcessMouseLeave(
            bool closeBoxVisibale,
            bool minimizeBoxVisibale,
            bool maximizeBoxVisibale)
        {
            if (closeBoxVisibale)
            {
                if (CloseBoxState == ControlState.Pressed)
                {
                    CloseBoxState = ControlState.PressedLeave;
                }
                else
                {
                    CloseBoxState = ControlState.Normal;
                }
            }

            if (minimizeBoxVisibale)
            {
                if (MinimizeBoxState == ControlState.Pressed)
                {
                    MinimizeBoxState = ControlState.PressedLeave;
                }
                else
                {
                    MinimizeBoxState = ControlState.Normal;
                }
            }

            if (maximizeBoxVisibale)
            {
                if (MaximizeBoxState == ControlState.Pressed)
                {
                    MaximizeBoxState = ControlState.PressedLeave;
                }
                else
                {
                    MaximizeBoxState = ControlState.Normal;
                }
            }

            HideToolTip();
        }

        private void Invalidate(Rectangle rect)
        {
            _form.Invalidate(rect);
        }

        private void ShowTooTip(string toolTipText)
        {
            if (_form != null)
            {
                _form.ToolTip.Active = true;
                _form.ToolTip.SetToolTip(_form, toolTipText);
            }
        }

        private void HideToolTip()
        {
            if (_form != null)
            {
                _form.ToolTip.Active = false;
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            _form = null;
        }

        #endregion
    }
}
