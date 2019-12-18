using System.Windows.Forms;
using System.Drawing;
using Ligg.WinForm.DataModel.Enums;

namespace Ligg.WinForm.Skin
{
    public delegate void FormBackgroundRenderEventHandler(object sender, FormBackgroundRenderEventArgs e);
    public class FormBackgroundRenderEventArgs : PaintEventArgs
    {
        private readonly SkinForm _form;

        public FormBackgroundRenderEventArgs(SkinForm form, Graphics g, Rectangle clipRect): base(g, clipRect)
        {
            _form = form;
        }

        public SkinForm Form
        {
            get { return _form; }
        }
    }


    public delegate void FormBorderRenderEventHandler(object sender, FormBorderRenderEventArgs e);
    public class FormBorderRenderEventArgs : PaintEventArgs
    {
        private readonly SkinForm _form;
        private readonly bool _isActive;

        public FormBorderRenderEventArgs(
            SkinForm form,
            Graphics g,
            Rectangle clipRect,
            bool active)
            : base(g, clipRect)
        {
            _form = form;
            _isActive = active;
        }

        public SkinForm Form
        {
            get { return _form; }
        }

        public bool IsActive
        {
            get { return _isActive; }
        }
    }

    public delegate void FormCaptionRenderEventHandler(object sender, FormCaptionRenderEventArgs e);
    public class FormCaptionRenderEventArgs : PaintEventArgs
    {
        private readonly SkinForm _form;
        private readonly bool _isActive;

        public FormCaptionRenderEventArgs(
            SkinForm form,
            Graphics g,
            Rectangle clipRect,
            bool active)
            : base(g, clipRect)
        {
            _form = form;
            _isActive = active;
        }

        public SkinForm Form
        {
            get { return _form; }
        }

        public bool IsActive
        {
            get { return _isActive; }
        }
    }

    public delegate void FormControlBoxRenderEventHandler(object sender, FormControlBoxRenderEventArgs e);
    public class FormControlBoxRenderEventArgs : PaintEventArgs
    {
        private readonly SkinForm _form;
        private readonly bool _isActive;
        private readonly ControlState _controlBoxState;
        private readonly ControlBoxStyle _controlBoxStyle;

        public FormControlBoxRenderEventArgs(
            SkinForm form,
            Graphics graphics,
            Rectangle clipRect,
            bool isActive,
            ControlBoxStyle controlBoxStyle,
            ControlState controlBoxState)
            : base(graphics, clipRect)
        {
            _form = form;
            _isActive = isActive;
            _controlBoxState = controlBoxState;
            _controlBoxStyle = controlBoxStyle;
        }

        public SkinForm Form
        {
            get { return _form; }
        }

        public bool IsActive
        {
            get { return _isActive; }
        }

        public ControlBoxStyle ControlBoxStyle
        {
            get { return _controlBoxStyle; }
        }

        public ControlState ControlBoxtate
        {
            get { return _controlBoxState; }
        }
    }
}
