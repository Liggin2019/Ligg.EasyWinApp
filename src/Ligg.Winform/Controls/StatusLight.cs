using System;
using System.Drawing;
using System.Windows.Forms;
using Ligg.WinForm.Helpers;

namespace Ligg.WinForm.Controls
{
    public partial class StatusLight : UserControl
    {
        public StatusLight()
        {
            InitializeComponent();
        }

        public event EventHandler OnLightClick;

        private string _text;
        public override string Text
        {
            get => _text;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _text = value;
                    label1.Text = value;
                }
                else
                {
                    _text = "";
                    label1.Text = "";
                }
            }
        }

        private Int16 _value;
        public Int16 Value
        {
            get => _value;
            set
            {
                var prevValue = _value;
                _value = value;
                if (prevValue != value)
                {
                    this.Refresh();
                }
            }
        }

        public string LabelStyle
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    ControlHelper.SetControlBackColor(label1, value);
                    ControlHelper.SetControlForeColor(label1, value);
                    ControlHelper.SetControlFont(label1, value);
                    ControlHelper.SetControlPadding(label1, value);
                }
            }
        }

        private void StatusLight_Load(object sender, EventArgs e)
        {
        }

        private void StatusLight_Paint(object sender, PaintEventArgs e)
        {
            pictureBox1.Width = Height;
            label1.Width = Width - pictureBox1.Width - 2;
            label1.Height = Height;
            label1.Location = new Point(pictureBox1.Width + 2, 0);

            if (_value ==1)
            {
                pictureBox1.BackgroundImage = imageList1.Images[1];
                //pictureBox1.Image = imageList1.Images[1];
            }
            else if (_value ==0)
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OnLightClick?.Invoke(this,null);
        }
    }
}
