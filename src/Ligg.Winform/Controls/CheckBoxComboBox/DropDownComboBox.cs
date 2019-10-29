using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Security.Permissions;
using Ligg.Winform.Skin;


namespace Ligg.Winform.Controls.CheckBoxComboBox
{
    [ToolboxItem(false)]
    public  class DropDownComboBox : ComboBox
    {
        public DropDownComboBox()
        {
            InitializeComponent();
            base.DropDownHeight = base.DropDownWidth = 1;
            base.IntegralHeight = false;

            KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MyKeyPress);
        }

        protected PopupStrip dropDown;

        private Control _dropDownControl;
        public Control DropDownControl
        {
            get
            {
                return _dropDownControl;
            }
            set
            {
                if (_dropDownControl == value)
                    return;
                _dropDownControl = value;
                dropDown = new PopupStrip(value);
            }
        }

        public void ShowDropDown()
        {
            if (dropDown != null)
            {
                dropDown.Show(this);
            }
        }

        /// <summary>
        /// Hides the drop down.
        /// </summary>
        public void HideDropDown()
        {
            if (dropDown != null)
            {
                dropDown.Hide();
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (NativeMethods.WM_REFLECT + NativeMethods.WM_COMMAND))
            {
                if (NativeMethods.HIWORD(m.WParam) == NativeMethods.CBN_DROPDOWN)
                {
                    // Blocks a redisplay when the user closes the control by clicking 
                    // on the combobox.
                    TimeSpan TimeSpan = DateTime.Now.Subtract(dropDown.LastClosedTimeStamp);
                    if (TimeSpan.TotalMilliseconds > 500)
                        ShowDropDown();
                    return;
                }
            }
            base.WndProc(ref m);
        }

        protected  void MyKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        #region " Unused Properties "

        /// <summary>This property is not relevant for this class.</summary>
        /// <returns>This property is not relevant for this class.</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new int DropDownWidth
        {
            get { return base.DropDownWidth; }
            set { base.DropDownWidth = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new int DropDownHeight
        {
            get { return base.DropDownHeight; }
            set
            {
                dropDown.Height = value;
                base.DropDownHeight = value;
            }
        }

        /// <summary>This property is not relevant for this class.</summary>
        /// <returns>This property is not relevant for this class.</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool IntegralHeight
        {
            get { return base.IntegralHeight; }
            set { base.IntegralHeight = value; }
        }

        /// <summary>This property is not relevant for this class.</summary>
        /// <returns>This property is not relevant for this class.</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new ObjectCollection Items
        {
            get { return base.Items; }
        }

        /// <summary>This property is not relevant for this class.</summary>
        /// <returns>This property is not relevant for this class.</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new int ItemHeight
        {
            get { return base.ItemHeight; }
            set { base.ItemHeight = value; }
        }

        #endregion


        #region Design.cs
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion
    }
}
