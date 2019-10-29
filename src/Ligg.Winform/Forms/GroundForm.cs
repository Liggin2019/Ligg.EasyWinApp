using System;
using System.Windows.Forms;
using Ligg.Winform.Skin;

namespace Ligg.Winform.Forms
{
    public partial class GroundForm : SkinForm
    {
        public GroundForm()
        {
            ToolStripManager.Renderer = new ToolStripRender();
            InitializeComponent();
            
        }

        private void GroundForm_Load(object sender, EventArgs e)
        {
           
        }


    }
}
