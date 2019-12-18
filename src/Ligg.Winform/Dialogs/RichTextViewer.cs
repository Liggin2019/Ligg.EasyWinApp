using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Ligg.Base.Extension;
using Ligg.WinForm.Forms;

namespace Ligg.WinForm.Dialogs
{
    public partial class MessageViewer : GroundForm
    {
        public string Title;
        public int FormWidth;
        public string Content;
        private int _screenWidth = 0;
        private int _screenheight = 0;
        private double _hwRatio;

        private readonly List<string> _textList = new List<string>();
       
        public MessageViewer()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.ShowIcon = false;
            GroundPanel.BackColor = StyleSheet.PopupContainerBackColor;
            richTextBoxContent.BackColor = StyleSheet.PopupContainerBackColor;
            this.richTextBoxContent.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        //#method
        private void MessageViewer_Load(object sender, EventArgs e)
        {
            var rect = new Rectangle();
            rect = Screen.GetWorkingArea(this);
            _screenWidth = rect.Width;
            _screenheight = rect.Height;
            _hwRatio = _screenheight * 1.00 / _screenWidth;
            Text = Title.IsNullOrEmpty() ? "Rich Text Viewer" : Title;

            richTextBoxContent.Text = Content;
            //richTextBoxContent.Focus();

            ResizeComponent();
        }

        private void richTextBoxContent_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FormWidth = Width + 50;
            ResizeComponent();
        }

        //#proc
        private void ResizeComponent()
        {
            if (FormWidth <10) FormWidth = 600;
            this.Width = FormWidth;
            this.Height = Convert.ToInt32(FormWidth * _hwRatio);

            //richTextBoxContent.Height = Height -10;

        }


    }
}
