using System.Windows.Forms;

namespace Ligg.WinForm.Forms
{
    public partial class InvisibleForm : Form
    {
        public InvisibleForm()
        {
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
            InitializeComponent();
        }
    }
}
