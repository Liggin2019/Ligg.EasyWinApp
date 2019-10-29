using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
using Ligg.Winform.Forms;
using Ligg.Winform.Resources;

namespace Ligg.Winform.Dialogs
{
    public partial class RichTextViewer : GroundForm
    {
        public string Title;
        public int FormWidth;
        public string Content;
        private string _filePath = "";
        private int _screenWidth = 0;
        private int _screenheight = 0;
        private double _hwRatio;
        MouseButtons _mouseButtons;
        DateTime _lastRightClickTime;

        private List<string> _textList = new List<string>();
        private List<string> _curTextList = new List<string>();



        public RichTextViewer()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.ShowIcon = false;
            GroundPanel.BackColor = StyleSet.PopupContainerBackColor;
            richTextBoxContent.BackColor = StyleSet.PopupContainerBackColor;
            containerPanelMsg.Padding = new Padding(2);
            containerPanelMsg.Height = 19;
            containerPanelMsg.BackColor = StyleSet.RunningStatusSectionBackColor;
            containerPanelMsg.BorderColor = StyleSet.ControlBorderColor;
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

            var textArry = Content.Split('\n');
            foreach (var v in textArry)
            {
                _textList.Add(v);
            }

            Restore();
            Title = Title.IsNullOrEmpty() ? WinformRes.RichTextViewer : Title;
            Text = Title;
            ResizeComponent();
        }

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Title = @"Open";
            //dlgOpenFile.Filter = @"Pdf File(*.DBF)|*.dbf";
            dlg.Multiselect = true;
            dlg.RestoreDirectory = false;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (dlg.FileNames.Count() > 0)
                {
                    try
                    {
                        var firstFilePath = dlg.FileNames[0];
                        var str = System.IO.File.ReadAllText(firstFilePath);
                        Content = str;
                        var textArry = Content.Split('\n');
                        _textList.Clear();
                        foreach (var v in textArry)
                        {
                            _textList.Add(v);
                        }
                        Restore();
                        _filePath = firstFilePath;
                        Text = Title + ":: " + firstFilePath;
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException("File format in incorrect!");
                    }
                }
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                toolStripButtonSaveAs_Click(null, null);
            }
            else
            {
                File.WriteAllText(_filePath, richTextBoxContent.Text);
            }
        }

        private void toolStripButtonSaveAs_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Title = WinformRes.SaveAs;
            dlg.RestoreDirectory = false;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (dlg.FileNames.Count() > 0)
                {
                    var firstFilePath = dlg.FileNames[0];
                    File.WriteAllText(firstFilePath, richTextBoxContent.Text);
                    _filePath = firstFilePath;
                    Text = Title + ":: " + firstFilePath;
                }
            }
        }

        private void toolStripButtonExportToExcel_Click(object sender, EventArgs e)
        {
            var content = richTextBoxContent.Text;
            var title = "ExpertToExcel".ToUniqueStringByNow();
            var folder = DirectoryHelper.GetSpecialDir("personal");

            var path = folder + "\\" + title + ".xls";
            File.WriteAllText(path, content,Encoding.Default);
            ProcessHelper.OpenFile(path, "");
        }

        private void toolStripButtonSearch_Click(object sender, EventArgs e)
        {
            var schStr = toolStripTextBoxSearchText.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(schStr)) return;
            int count = 0;
            var text = "";
            _curTextList = new List<string>();
            foreach (var v in _textList)
            {
                if (v.ToLower().Contains(schStr.ToLower()))
                {
                    text = count == 0 ? v : text + v;
                    _curTextList.Add(v);
                    count++;
                }
            }

            richTextBoxContent.Text =text;
            labelMsgLine.Text = WinformRes.Lines + @": " + count + @"/" + _textList.Count;
            labelMsgFind.Text = "";
        }

        private void toolStripButtonFind_Click(object sender, EventArgs e)
        {
            var schStr = toolStripTextBoxSearchText.Text.Trim().ToLower();
            var cttStr = richTextBoxContent.Text;
            if (string.IsNullOrEmpty(schStr)) return;
            if (string.IsNullOrEmpty(cttStr)) return;
            cttStr = cttStr.ToLower();
            int schStrLen = schStr.Length;
            var msgLen = cttStr.Length;
            int index = 0;
            int pos = 0;
            int count = 0;
            do
            {
                index = cttStr.IndexOf(schStr, pos);
                if (index != -1)
                {
                    richTextBoxContent.Select(index, schStrLen);
                    richTextBoxContent.SelectionColor = Color.Red;
                    count++;
                }
                pos = schStrLen + index;
            } while (index != -1 && pos + schStrLen < msgLen);

            labelMsgFind.Text =WinformRes.FindOut + @": " + count;

        }

        private void toolStripButtonRestore_Click(object sender, EventArgs e)
        {
            Restore();
        }

        private void toolStripButtonClear_Click(object sender, EventArgs e)
        {
            toolStripTextBoxSearchText.Text = "";
        }

        private void toolStripButtonSortAscend_Click(object sender, EventArgs e)
        {
            var curTextList1 = _curTextList.OrderBy(x => x).ToList();
            var text = "";
            int count = 0;
            foreach (var v in curTextList1)
            {
                text = count == 0 ? v : text + v;
                count++;
            }
            richTextBoxContent.Text = " " + text;
            labelMsgLine.Text = WinformRes.Lines + @": " + count + @"/" + _textList.Count;
            labelMsgFind.Text = "";
            richTextBoxContent.Focus();
        }

        private void toolStripButtonSortDescend_Click(object sender, EventArgs e)
        {
            var curTextList1 = _curTextList.OrderByDescending(x => x);
            var text = "";
            int count = 0;
            foreach (var v in curTextList1)
            {
                text = count == 0 ? v : text + v;
                count++;
            }
            richTextBoxContent.Text = " " + text;
            labelMsgLine.Text =WinformRes.Lines + @": " + count + @"/" + _textList.Count;
            labelMsgFind.Text = "";
            richTextBoxContent.Focus();
        }

        private void toolStripButtonZoomIn_Click(object sender, EventArgs e)
        {
            if ((FormWidth + 200 < _screenWidth))
            {
                FormWidth = FormWidth + 200;
                ResizeComponent();
            }
        }


        private void richTextBoxContent_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            toolStripButtonZoomIn_Click(null, null);
        }

        private void toolStripButtonZoomOut_Click(object sender, EventArgs e)
        {
            if ((FormWidth - 200 > 100))
            {
                FormWidth = FormWidth - 200;
                ResizeComponent();
            }
        }



        //#proc
        private void ResizeComponent()
        {
            if (FormWidth < 10) FormWidth = 600;
            this.Width = FormWidth;
            this.Height = Convert.ToInt32(Width * _hwRatio);

            richTextBoxContent.Height = Height - toolBar.Height - containerPanelMsg.Height - 28;
        }

        private void Restore()
        {
            richTextBoxContent.Text = Content;
            labelMsgLine.Text = WinformRes.Lines + @": " + _textList.Count + @"/" + _textList.Count;
            labelMsgFind.Text = "";
            _curTextList.Clear();
            foreach (var v in _textList)
            {
                _curTextList.Add(v);
            }
            toolStripTextBoxSearchText.Focus();
        }


        ////////////////////////////////////////
        //##following is no use



        private void textBoxMsg_MouseDown(object sender, MouseEventArgs e)
        {
            var now = SystemTimeHelper.UtcNow();
            var ts1 = new TimeSpan(_lastRightClickTime.Ticks);
            var ts2 = new TimeSpan(now.Ticks);
            var ts = ts2.Subtract(ts1).Duration();
            if (_mouseButtons == MouseButtons.Right && e.Button == MouseButtons.Right && (ts.Milliseconds < 300))
            {
                if ((FormWidth - 200 > 100))
                {
                    FormWidth = FormWidth - 200;
                    ResizeComponent();
                }
            }
        }
        private void textBoxMsg_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseButtons = e.Button;
            if (e.Button == MouseButtons.Right)
            {
                _lastRightClickTime = SystemTimeHelper.UtcNow();
            }
        }


    }
}
