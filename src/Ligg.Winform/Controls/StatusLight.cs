using System;
using System.Drawing;
using System.Windows.Forms;
using Ligg.Winform.Helpers;

namespace Ligg.Winform.Controls
{
    public partial class StatusLight : UserControl
    {
        public StatusLight()
        {
            InitializeComponent();
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _title = value;
                    label1.Text = value;
                }
                else
                {
                    _title = "";
                    label1.Text = "";
                }
                    
            }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                var prevValue = _value;
                _value = value;
                if(prevValue!=value)
                {
                    this.Refresh();
                }
            }
        }

        public  string LabelStyle
        {
            set
            {
                if(!string.IsNullOrEmpty(value))
                {
                    ControlHelper.SetControlBackColor(label1, value);
                    ControlHelper.SetControlForeColor(label1, value);
                    ControlHelper.SetControlFont(label1, value);
                    ControlHelper.SetControlPadding(label1, value);
                }
            }
        }

        private void OkIndicator_Load(object sender, EventArgs e)
        {
            //pictureBox1.Width = Height;
            //label1.Location = new Point(pictureBox1.Width + 2, 0);
            //label1.Width = Width - pictureBox1.Width - 2;
            //label1.Height = Height;
        }

        private void OkIndicator_Paint(object sender, PaintEventArgs e)
        {
            pictureBox1.Width = Height;
            label1.Width = Width - pictureBox1.Width - 2;
            label1.Height = Height;
            label1.Location = new Point(pictureBox1.Width + 2, 0);

            if (_value == "true" | _value == "True" | _value == "1")
            {
                pictureBox1.BackgroundImage = imageList1.Images[1];
                //pictureBox1.Image = imageList1.Images[1];
            }
            else if (_value == "false" | _value == "False" | _value == "0")
            {
                pictureBox1.BackgroundImage = imageList1.Images[2];
                //pictureBox1.Image = imageList1.Images[3];
            }
            else
            {
                pictureBox1.BackgroundImage = imageList1.Images[0];
                //pictureBox1.Image = imageList1.Images[0];
            }
        }
    }
}
