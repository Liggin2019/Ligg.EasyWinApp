using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ligg.Base.DataModel;

namespace Ligg.Winform.Controls
{
    public partial class ComboBoxEx : UserControl
    {
        public ComboBoxEx()
        {
            InitializeComponent();
            Height = 20;
        }
        private List<ValueText> _dataSourceList;

        private bool _getText;
        public bool GetText
        {
            get
            {
                return _getText;
            }
            set
            {
                if (value != _getText)
                {
                    _getText = value;
                }
            }
        }

        public string SelectedValue
        {
            get { return comboBox.SelectedValue.ToString(); }
            set { comboBox.SelectedValue = value; }
        }

        public string SelectedText
        {
            get
            {
                return comboBox.SelectedText;
            }
            set
            {
                comboBox.SelectedText = value;
            }
        }

        //#method
        //#proc
        public string GetSelection()
        {
            if (_getText) return SelectedText;
            else return SelectedValue;
        }

        public void FillData(DataTable dt)
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    var vatTxts = new List<ValueText>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        var val = dr["Value"].ToString();
                        var txt = dr["Text"].ToString();
                        vatTxts.Add(new ValueText(val, txt));
                    }
                    _dataSourceList = vatTxts;
                    comboBox.DataSource = dt;
                    comboBox.ValueMember = "Value";
                    comboBox.DisplayMember = "Text";
                }
            }
        }

        public void FillData(List<ValueText> list)
        {
            if (_dataSourceList.Count > 0)
            {
                _dataSourceList.Clear();
                _dataSourceList = list;
                comboBox.DataSource = list;
                comboBox.ValueMember = "Value";
                comboBox.DisplayMember = "Text";
            }
        }


    }
}
