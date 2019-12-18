using System;
using System.Drawing;
using System.Windows.Forms;
using Ligg.WinForm.Helpers;

namespace Ligg.WinForm.Controls
{
    public partial class ScoreLight : UserControl
    {
        public ScoreLight()
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

        private float _value;
        public float Value
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
            button1.Width = (Height - 8) < 0 ? 0 : (Height - 8);
            button1.Height = (Height - 8) < 0 ? 0 : (Height - 8);
            button1.Location = new Point(0, 4);
            label1.Width = Width - button1.Width - 2;
            label1.Height = Height;
            label1.Location = new Point(button1.Width + 2, 0);
            if (_value == 0)
            {
                button1.BackColor = Color.FromArgb(255, 0, 0);
            }
            else if (_value > 0 & _value < 0.5)
            {
                int v1 = Convert.ToInt32((510 * (_value - 1) / 1.0));
                if (v1 > 255) v1 = 255;
                if (v1 < 0) v1 = 0;
                button1.BackColor = Color.FromArgb(255, v1, 0);

            }
            else if (_value == 0.5)
            {
                button1.BackColor = Color.FromArgb(255, 255, 0);
            }
            else if (_value > 0.5 & _value < 1)
            {

                int v1 = Convert.ToInt32((510 * (1 - _value)) / 1.0);
                if (v1 > 255) v1 = 255;
                if (v1 < 0) v1 = 0;
                int v2 = Convert.ToInt32((382 - 127 * _value) / 1.0);
                if (v2 > 255) v2 = 255;
                if (v2 < 0) v2 = 0;
                button1.BackColor = Color.FromArgb(v1, v2, 0);
            }
            else if (_value == 1)
            {

                button1.BackColor = Color.FromArgb(0, 128, 0);
            }
            else
            {
                button1.BackColor = Color.FromArgb(180, 180, 180);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OnLightClick?.Invoke(this, null);
        }
    }
}
