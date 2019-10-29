using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ligg.Base.DataModel;
using Ligg.Base.Extension;

namespace Ligg.Winform.Controls
{
    public partial class SelectionBox : UserControl
    {
        public SelectionBox()
        {
            InitializeComponent();
            Height = 20;
            popupButton.Width = 16;
        }

        public int PopupType { get; set; }
        public string PopupParams { get; set; }

        private bool _isEditable;
        public bool IsEditable
        {
            get
            {
                return _isEditable;
            }
            set
            {
                if (value != _isEditable)
                {
                    _isEditable = value;
                    ShowValue = true;
                    GetText = false;
                }
            }
        }

        private bool _showValue;
        public bool ShowValue
        {
            get
            {
                return _showValue;
            }
            set
            {
                if (value != null)
                {
                    _showValue = value;
                }
            }
        }

        private bool _getText;
        public bool GetText
        {
            get
            {
                return _getText;
            }
            set
            {
                if (value != null)
                {
                    _getText = value;
                }
            }
        }

        private List<ValueText> _dataSourceList;
        public List<ValueText> DataSourceList
        {
            get
            {
                return _dataSourceList;
            }
            set
            {
                if (value != null)
                {
                   _dataSourceList = value;
                }

            }
        }

        public string SelectedValues
        {
            get
            {
                var txts = "";
                if (_dataSourceList == null)
                    return "";
                for (int i = 0; i < _dataSourceList.Count; i++)
                {
                    txts += (
                            i == 0 ?
                            String.Format("{0}", _dataSourceList[i].Value)
                            : String.Format(", {0}", _dataSourceList[i].Value)
                            );
                }
                return txts;
            }
        }

        public string SelectedTexts
        {
            get
            {
                var txts = "";
                if(_dataSourceList==null)
                return "";
                for (int i = 0; i < _dataSourceList.Count; i++)
                {
                    txts += (
                            i == 0 ?
                            String.Format("{0}", _dataSourceList[i].Text)
                            : String.Format(", {0}", _dataSourceList[i].Text)
                            );
                }
                return txts;
            }
        }
        //#method
        private void SelectionBox_Load(object sender, EventArgs e)
        {
            popupButton.Width = 16;
            textBox.Width = Width - 18;
            if(_isEditable)
            {
                textBox.ReadOnly = false;
            }
            else
            {
                textBox.ReadOnly = true; 
            }
            ShowSelection();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (_isEditable && _showValue)//input manully
            {
                if (textBox.Focused)
                {
                    var strs = textBox.Text.Trim();
                    if (!strs.IsNullOrEmpty())
                    {
                        String[] strArry;
                        if (strs.Contains(","))
                        {
                            strArry = strs.Split(',');
                        }
                        else if (strs.Contains(";"))
                        {
                            strArry = strs.Split(',');
                        }
                        else if (strs.Contains("，"))
                        {
                            strArry = strs.Split('，');
                        }
                        else if (strs.Contains("；"))
                        {
                            strArry = strs.Split('；');
                        }
                        else
                        {
                            strArry = strs.Split(' ');
                        }
                        var list = new List<ValueText>();

                        foreach (var str in strArry)
                        {
                            var str1 = str.Trim();
                            var valText = new ValueText();
                            valText.Value = str1;
                            valText.Text = "";
                            list.Add(valText);

                        }
                        _dataSourceList.Clear();
                        _dataSourceList = list;
                    }
                }
            }

        }

        private void popupButton_Click(object sender, EventArgs e)
        {
            //var dgl = new AboutDialog();
            //dgl.ShowDialog();
        }

        //#proc
        public void ClearSelection()
        {
            _dataSourceList.Clear();
            textBox.Text = "";
        }

        public string GetSelection()
        {
            if (_getText) return SelectedTexts;
            else return SelectedValues;
        }

        public void FillData(DataTable dt)
        {
            if (dt != null)
            {
                //_dataSourceList.Clear();
                if (dt.Rows.Count > 0)
                {
                    var vatTxts = new List<ValueText>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        var val = dr["Value"].ToString();
                        var txt = dr["Text"].ToString();
                        vatTxts.Add(new ValueText(val, txt));
                    }
                    DataSourceList = vatTxts;
                    ShowSelection();
                }
            }
        }

        public void FillData(List<ValueText> list )
        {
            _dataSourceList.Clear();
            _dataSourceList = list;
            ShowSelection();
        }

        public void ShowSelection()
        {
            textBox.Text =_showValue?SelectedValues:SelectedTexts;
        }



    }
}
