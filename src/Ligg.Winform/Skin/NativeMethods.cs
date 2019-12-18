using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Ligg.WinForm.Skin
{
    internal class NativeMethods
    {
        #region Bool Fileds

        public static readonly IntPtr TRUE = new IntPtr(1);
        public static readonly IntPtr FALSE = IntPtr.Zero;

        #endregion

        #region WindowMessages
        internal const int WM_NCHITTEST = 0x0084,
                        WM_NCACTIVATE = 0x0086,
                        WS_EX_NOACTIVATE = 0x08000000,
                        HTTRANSPARENT = -1,
                        HTLEFT = 10,
                        HTRIGHT = 11,
                        HTTOP = 12,
                        HTTOPLEFT = 13,
                        HTTOPRIGHT = 14,
                        HTBOTTOM = 15,
                        HTBOTTOMLEFT = 16,
                        HTBOTTOMRIGHT = 17,
                        WM_USER = 0x0400,
                        WM_REFLECT = WM_USER + 0x1C00,
                        WM_COMMAND = 0x0111,
                        CBN_DROPDOWN = 7,
                        WM_GETMINMAXINFO = 0x0024;

        public enum WindowMessages
        {
            WM_NULL = 0x0000,
            WM_CREATE = 0x0001,
            WM_DESTROY = 0x0002,
            WM_MOVE = 0x0003,
            WM_SIZE = 0x0005,
            WM_ACTIVATE = 0x0006,
            WM_SETFOCUS = 0x0007,
            WM_KILLFOCUS = 0x0008,
            WM_ENABLE = 0x000A,
            WM_SETREDRAW = 0x000B,
            WM_SETTEXT = 0x000C,
            WM_GETTEXT = 0x000D,
            WM_GETTEXTLENGTH = 0x000E,
            WM_PAINT = 0x000F,
            WM_CLOSE = 0x0010,

            WM_QUIT = 0x0012,
            WM_ERASEBKGND = 0x0014,
            WM_SYSCOLORCHANGE = 0x0015,
            WM_SHOWWINDOW = 0x0018,

            WM_ACTIVATEAPP = 0x001C,

            WM_SETCURSOR = 0x0020,
            WM_MOUSEACTIVATE = 0x0021,
            WM_GETMINMAXINFO = 0x24,
            WM_WINDOWPOSCHANGING = 0x0046,
            WM_WINDOWPOSCHANGED = 0x0047,

            WM_CONTEXTMENU = 0x007B,
            WM_STYLECHANGING = 0x007C,
            WM_STYLECHANGED = 0x007D,
            WM_DISPLAYCHANGE = 0x007E,
            WM_GETICON = 0x007F,
            WM_SETICON = 0x0080,

            // non client area
            WM_NCCREATE = 0x0081,
            WM_NCDESTROY = 0x0082,
            WM_NCCALCSIZE = 0x0083,
            WM_NCHITTEST = 0x84,
            WM_NCPAINT = 0x0085,
            WM_NCACTIVATE = 0x0086,

            WM_GETDLGCODE = 0x0087,

            WM_SYNCPAINT = 0x0088,

            // non client mouse
            WM_NCMOUSEMOVE = 0x00A0,
            WM_NCLBUTTONDOWN = 0x00A1,
            WM_NCLBUTTONUP = 0x00A2,
            WM_NCLBUTTONDBLCLK = 0x00A3,
            WM_NCRBUTTONDOWN = 0x00A4,
            WM_NCRBUTTONUP = 0x00A5,
            WM_NCRBUTTONDBLCLK = 0x00A6,
            WM_NCMBUTTONDOWN = 0x00A7,
            WM_NCMBUTTONUP = 0x00A8,
            WM_NCMBUTTONDBLCLK = 0x00A9,

            // keyboard
            WM_KEYDOWN = 0x0100,
            WM_KEYUP = 0x0101,
            WM_CHAR = 0x0102,

            WM_SYSCOMMAND = 0x0112,

            // menu
            WM_INITMENU = 0x0116,
            WM_INITMENUPOPUP = 0x0117,
            WM_MENUSELECT = 0x011F,
            WM_MENUCHAR = 0x0120,
            WM_ENTERIDLE = 0x0121,
            WM_MENURBUTTONUP = 0x0122,
            WM_MENUDRAG = 0x0123,
            WM_MENUGETOBJECT = 0x0124,
            WM_UNINITMENUPOPUP = 0x0125,
            WM_MENUCOMMAND = 0x0126,

            WM_CHANGEUISTATE = 0x0127,
            WM_UPDATEUISTATE = 0x0128,
            WM_QUERYUISTATE = 0x0129,

            // mouse
            WM_MOUSEFIRST = 0x0200,
            WM_MOUSEMOVE = 0x0200,
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_LBUTTONDBLCLK = 0x0203,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_RBUTTONDBLCLK = 0x0206,
            WM_MBUTTONDOWN = 0x0207,
            WM_MBUTTONUP = 0x0208,
            WM_MBUTTONDBLCLK = 0x0209,
            WM_MOUSEWHEEL = 0x020A,
            WM_MOUSELAST = 0x020D,

            WM_PARENTNOTIFY = 0x0210,
            WM_ENTERMENULOOP = 0x0211,
            WM_EXITMENULOOP = 0x0212,

            WM_NEXTMENU = 0x0213,
            WM_SIZING = 0x0214,
            WM_CAPTURECHANGED = 0x0215,
            WM_MOVING = 0x0216,

            WM_ENTERSIZEMOVE = 0x0231,
            WM_EXITSIZEMOVE = 0x0232,

            WM_MOUSELEAVE = 0x02A3,
            WM_MOUSEHOVER = 0x02A1,
            WM_NCMOUSEHOVER = 0x02A0,
            WM_NCMOUSELEAVE = 0x02A2,

            WM_MDIACTIVATE = 0x0222,
            WM_HSCROLL = 0x0114,
            WM_VSCROLL = 0x0115,

            WM_PRINT = 0x0317,
            WM_PRINTCLIENT = 0x0318,

            WM_PASTE = 0X302,
        }

        #endregion

        #region SystemCommands



        #endregion

        #region NCHITTEST
        /// <summary>
        /// Location of cursor hot spot returnet in WM_NCHITTEST.
        /// </summary>
        public enum NCHITTEST
        {
            /// <summary>
            /// On the screen background or on a dividing line between windows 
            /// (same as HTNOWHERE, except that the DefWindowProc function produces a system beep to indicate an error).
            /// </summary>
            HTERROR = (-2),
            /// <summary>
            /// In a window currently covered by another window in the same thread 
            /// (the message will be sent to underlying windows in the same thread until one of them returns a code that is not HTTRANSPARENT).
            /// </summary>
            HTTRANSPARENT = (-1),
            /// <summary>
            /// On the screen background or on a dividing line between windows.
            /// </summary>
            HTNOWHERE = 0,
            /// <summary>In a client area.</summary>
            HTCLIENT = 1,
            /// <summary>In a title bar.</summary>
            HTCAPTION = 2,
            /// <summary>In a window menu or in a Close button in a child window.</summary>
            HTSYSMENU = 3,
            /// <summary>In a size box (same as HTSIZE).</summary>
            HTGROWBOX = 4,
            /// <summary>In a menu.</summary>
            HTMENU = 5,
            /// <summary>In a horizontal scroll bar.</summary>
            HTHSCROLL = 6,
            /// <summary>In the vertical scroll bar.</summary>
            HTVSCROLL = 7,
            /// <summary>In a Minimize button.</summary>
            HTMINBUTTON = 8,
            /// <summary>In a Maximize button.</summary>
            HTMAXBUTTON = 9,
            /// <summary>In the left border of a resizable window 
            /// (the user can click the mouse to resize the window horizontally).</summary>
            HTLEFT = 10,
            /// <summary>
            /// In the right border of a resizable window 
            /// (the user can click the mouse to resize the window horizontally).
            /// </summary>
            HTRIGHT = 11,
            /// <summary>In the upper-horizontal border of a window.</summary>
            HTTOP = 12,
            /// <summary>In the upper-left corner of a window border.</summary>
            HTTOPLEFT = 13,
            /// <summary>In the upper-right corner of a window border.</summary>
            HTTOPRIGHT = 14,
            /// <summary>	In the lower-horizontal border of a resizable window 
            /// (the user can click the mouse to resize the window vertically).</summary>
            HTBOTTOM = 15,
            /// <summary>In the lower-left corner of a border of a resizable window 
            /// (the user can click the mouse to resize the window diagonally).</summary>
            HTBOTTOMLEFT = 16,
            /// <summary>	In the lower-right corner of a border of a resizable window 
            /// (the user can click the mouse to resize the window diagonally).</summary>
            HTBOTTOMRIGHT = 17,
            /// <summary>In the border of a window that does not have a sizing border.</summary>
            HTBORDER = 18,

            HTOBJECT = 19,
            /// <summary>In a Close button.</summary>
            HTCLOSE = 20,
            /// <summary>In a Help button.</summary>
            HTHELP = 21,
        }

        #endregion

        #region WindowStyle

        [Flags]
        public enum WindowStyle : uint
        {
            WS_OVERLAPPED = 0x00000000,
            WS_POPUP = 0x80000000,
            WS_CHILD = 0x40000000,
            WS_MINIMIZE = 0x20000000,
            WS_VISIBLE = 0x10000000,
            WS_DISABLED = 0x08000000,
            WS_CLIPSIBLINGS = 0x04000000,
            WS_CLIPCHILDREN = 0x02000000,
            WS_MAXIMIZE = 0x01000000,
            WS_CAPTION = 0x00C00000,
            WS_BORDER = 0x00800000,
            WS_DLGFRAME = 0x00400000,
            WS_VSCROLL = 0x00200000,
            WS_HSCROLL = 0x00100000,
            WS_SYSMENU = 0x00080000,
            WS_THICKFRAME = 0x00040000,
            WS_GROUP = 0x00020000,
            WS_TABSTOP = 0x00010000,
            WS_MINIMIZEBOX = 0x00020000,
            WS_MAXIMIZEBOX = 0x00010000,
            WS_TILED = WS_OVERLAPPED,
            WS_ICONIC = WS_MINIMIZE,
            WS_SIZEBOX = WS_THICKFRAME,
            WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,
            WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU |
                                    WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX),
            WS_POPUPWINDOW = (WS_POPUP | WS_BORDER | WS_SYSMENU),
            WS_CHILDWINDOW = (WS_CHILD)
        }

        #endregion

        #region WindowStyleEx

        [Flags]
        public enum WindowStyleEx
        {
            WS_EX_DLGMODALFRAME = 0x00000001,
            WS_EX_NOPARENTNOTIFY = 0x00000004,
            WS_EX_TOPMOST = 0x00000008,
            WS_EX_ACCEPTFILES = 0x00000010,
            WS_EX_TRANSPARENT = 0x00000020,
            WS_EX_MDICHILD = 0x00000040,
            WS_EX_TOOLWINDOW = 0x00000080,
            WS_EX_WINDOWEDGE = 0x00000100,
            WS_EX_CLIENTEDGE = 0x00000200,
            WS_EX_CONTEXTHELP = 0x00000400,
            WS_EX_RIGHT = 0x00001000,
            WS_EX_LEFT = 0x00000000,
            WS_EX_RTLREADING = 0x00002000,
            WS_EX_LTRREADING = 0x00000000,
            WS_EX_LEFTSCROLLBAR = 0x00004000,
            WS_EX_RIGHTSCROLLBAR = 0x00000000,
            WS_EX_CONTROLPARENT = 0x00010000,
            WS_EX_STATICEDGE = 0x00020000,
            WS_EX_APPWINDOW = 0x00040000,
            WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE),
            WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST),
            WS_EX_LAYERED = 0x00080000,
            WS_EX_NOINHERITLAYOUT = 0x00100000, // Disable inheritence of mirroring by children
            WS_EX_LAYOUTRTL = 0x00400000, // Right to left mirroring
            WS_EX_COMPOSITED = 0x02000000,
            WS_EX_NOACTIVATE = 0x08000000,
        }

        #endregion

        #region RECT

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }

            public Rectangle Rect 
            {
                get 
                {
                    return new Rectangle(
                        this.Left, 
                        this.Top, 
                        this.Right - this.Left, 
                        this.Bottom - this.Top); 
                }
            }

            public static RECT FromXYWH(int x, int y, int width, int height)
            {
                return new RECT(x,
                                y,
                                x + width,
                                y + height);
            }

            public static RECT FromRectangle(Rectangle rect)
            {
                return new RECT(rect.Left,
                                 rect.Top,
                                 rect.Right,
                                 rect.Bottom);
            }
        }

        #endregion

        #region WINDOWPOS

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hWndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public uint flags;
        }

        #endregion

        #region NCCALCSIZE_PARAMS
        
        [StructLayout(LayoutKind.Sequential)]
        public struct NCCALCSIZE_PARAMS
        {
            /// <summary>
            /// Contains the new coordinates of a window that has been moved or resized, that is, it is the proposed new window coordinates.
            /// </summary>
            public RECT rectProposed;
            /// <summary>
            /// Contains the coordinates of the window before it was moved or resized.
            /// </summary>
            public RECT rectBeforeMove;
            /// <summary>
            /// Contains the coordinates of the window's client area before the window was moved or resized.
            /// </summary>
            public RECT rectClientBeforeMove;
            /// <summary>
            /// Pointer to a WINDOWPOS structure that contains the size and position values specified in the operation that moved or resized the window.
            /// </summary>
            public WINDOWPOS lpPos;
        }

        #endregion

        #region MINMAXINFO

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public Point reserved;
            public Size maxSize;
            public Point maxPosition;
            public Size minTrackSize;
            public Size maxTrackSize;
        }

        #endregion

        #region Methods

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public static int LOWORD(int value)
        {
            return value & 0xFFFF;
        }

        internal static int LOWORD(IntPtr n)
        {
            return LOWORD(unchecked((int)(long)n));
        }

        public static int HIWORD(int value)
        {
            return value >> 16;
        }

        internal static int HIWORD(IntPtr n)
        {
            return HIWORD(unchecked((int)(long)n));
        }

        #endregion
    }
}
