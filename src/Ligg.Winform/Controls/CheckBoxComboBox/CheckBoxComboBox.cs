using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using Ligg.Base.DataModel;

namespace Ligg.Winform.Controls.CheckBoxComboBox
{
    /// <summary>
    /// Martin Lottering : 2007-10-27
    /// --------------------------------
    /// This is a usefull control in Filters. Allows you to save space and can replace a Grouped Box of CheckBoxes.
    /// Currently used on the TasksFilter for TaskStatusses, which means the user can select which Statusses to include
    /// in the "Search".
    /// This control does not implement a CheckBoxListBox, instead it adds a wrapper for the normal ComboBox and Items. 
    /// See the CheckBoxItems property.
    /// ----------------
    /// ALSO IMPORTANT: In Data Binding when setting the DataSource. The ValueMember must be a bool type property, because it will 
    /// be binded to the Checked property of the displayed CheckBox. Also see the DisplayMemberSingleItem for more information.
    /// ----------------
    /// Extends the CodeProject DropDownComboBox "Simple pop-up control" "http://www.codeproject.com/cs/miscctrl/simplepopup.asp"
    /// by Lukasz Swiatkowski.
    /// </summary>
    [ToolboxBitmap(typeof(System.Windows.Forms.ComboBox)), ToolboxItem(true), ToolboxItemFilter("System.Windows.Forms"), Description("Displays an editable text box with a drop-down list of permitted values.")]
    public  class CheckBoxComboBox : DropDownComboBox
    {

        public CheckBoxComboBox(): base()
        {
            InitializeComponent();
            _checkBoxProperties = new CheckBoxProperties();
            _checkBoxProperties.PropertyChanged += new EventHandler(_CheckBoxProperties_PropertyChanged);
            // Dumps the ListControl in a(nother) Container to ensure the ScrollBar on the ListControl does not
            // Paint over the Size grip. Setting the Padding or Margin on the DropDownStrip or host control does
            // not work as I expected. I don't think it can work that way.
            CheckBoxComboBoxListControlContainer ContainerControl = new CheckBoxComboBoxListControlContainer();
            _checkBoxComboBoxListControl = new CheckBoxComboBoxListControl(this);
            _checkBoxComboBoxListControl.Items.CheckBoxCheckedChanged += new EventHandler(ItemsCheckBoxCheckedChanged);
            ContainerControl.Controls.Add(_checkBoxComboBoxListControl);
            // This padding spaces neatly on the left-hand side and allows space for the size grip at the bottom.
            ContainerControl.Padding = new Padding(4, 0, 0, 14);
            // The ListControl FILLS the ListContainer.
            _checkBoxComboBoxListControl.Dock = DockStyle.Fill;
            // The DropDownControl used by the base class. Will be wrapped in a popup by the base class.
            DropDownControl = ContainerControl;
            // Must be set after the DropDownControl is set, since the popup is recreated.
            // NOTE: I made the dropDown protected so that it can be accessible here. It was private.
            dropDown.Resizable = true;
        }

        public event EventHandler CheckBoxCheckedChanged;


        #region private property
        private readonly CheckBoxComboBoxListControl _checkBoxComboBoxListControl;
        private string _displayMemberSingleItem = null;
        //private List<ValueText> _selectionValueTextList;
        private ListSelection<ValueText> _valueTextListSelection;
        private List<ValueText> _dataSourceList;
        #endregion

        #region public property
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

        public List<ValueText> DataSourceList
        {
            get
            {
                return _dataSourceList;
            }
            set
            {
                if (value!=null)
                {
                    _dataSourceList = value;
                    _valueTextListSelection = new ListSelection<ValueText>(_dataSourceList, "Text");
                    this.DataSource = _valueTextListSelection;
                    this.DisplayMemberSingleItem = "Name";
                    this.DisplayMember = "NameConcatenated";
                    this.ValueMember = "Selected";
                }

            }
        }

        public string SelectedValues
        {
            get
            {
                var txtsStr = _valueTextListSelection.SelectedNames;
                var txtArry = txtsStr.Split(',');
                for (int i = 0;i < txtArry.Length; i++)
                {
                    txtArry[i] = txtArry[i].Trim();
                }
                var valTxts = _dataSourceList.FindAll(x => txtArry.Contains(x.Text));
                var txts = "";
                for (int i = 0; i < valTxts.Count; i++)
                {
                    txts += (
                            i == 0 ?
                            String.Format("{0}", valTxts[i].Value)
                            : String.Format(", {0}", valTxts[i].Value)
                            );
                }
                return txts;
            }
            set
            {
                if (value != null)
                {
                    var valTxts = new List<ValueText>();
                    var valArry = value.Split(',');
                    foreach (var valTxt in _dataSourceList)
                    {
                        foreach (var val in valArry)
                        {
                            if(valTxt.Value==val.Trim())
                            {
                                valTxts.Add(valTxt); 
                            }
                        }
                    }

                    if (valTxts.Count > 0)
                    {
                        foreach (var valTxt in valTxts)
                        {
                            _valueTextListSelection.FindObjectWithItem(valTxt).Selected = true;
                        }

                        var i = 0;
                        foreach (var valTxtSel in _valueTextListSelection)
                        {
                            if (valTxtSel.Selected)
                            CheckBoxItems[i].Checked = true;
                            i++;
                        }

                            
                    }
                }
            }
        }

        public string SelectedTexts
        {
            get
            {
                return _valueTextListSelection.SelectedNames;
            }
        }

        public int DropDownStripHeight
        {
            set { dropDown.Height = value; }
        }

        [Browsable(false)]
        public CheckBoxComboBoxItemList CheckBoxItems
        {
            get { return _checkBoxComboBoxListControl.Items; }
        }

        public new object DataSource
        {
            get { return base.DataSource; }
            set
            {
                base.DataSource = value;
                if (!string.IsNullOrEmpty(ValueMember))
                    // This ensures that at least the checkboxitems are available to be initialised.
                    _checkBoxComboBoxListControl.SynchroniseControlsWithComboBoxItems();
            }
        }

        public new string ValueMember
        {
            get { return base.ValueMember; }
            set
            {
                base.ValueMember = value;
                if (!string.IsNullOrEmpty(ValueMember))
                    // This ensures that at least the checkboxitems are available to be initialised.
                    _checkBoxComboBoxListControl.SynchroniseControlsWithComboBoxItems();
            }
        }

        public string DisplayMemberSingleItem
        {
            get { if (string.IsNullOrEmpty(_displayMemberSingleItem)) return DisplayMember; else return _displayMemberSingleItem; }
            set { _displayMemberSingleItem = value; }
        }


        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new ObjectCollection Items
        {
            get { return base.Items; }
        }
        #endregion

        #region method
        private void ItemsCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            OnCheckBoxCheckedChanged(sender, e);
        }

        protected void OnCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (DropDownStyle != ComboBoxStyle.DropDownList)
            {
                string ListText = "";
                foreach (CheckBoxComboBoxItem Item in _checkBoxComboBoxListControl.Items)
                {
                    if (Item.Checked) ListText += string.IsNullOrEmpty(ListText) ? Item.Text : String.Format(",{0}", Item.Text);
                } 
                Text = ListText;
            }

            EventHandler handler = CheckBoxCheckedChanged;
            if (handler != null)
                handler(sender, e);
        }

        protected override void OnResize(EventArgs e)
        {
            // When the ComboBox is resized, the width of the dropdown 
            // is also resized to match the width of the ComboBox. I think it looks better.
            Size Size = new Size(Width, DropDownControl.Height);
            dropDown.Size = Size;
            base.OnResize(e);
        }

        #endregion

        #region proc
        public string GetSelection()
        {
            if (_getText) return SelectedTexts;
            else return SelectedValues;
        }
        public void ClearSelection()
        {
            foreach (var item in _valueTextListSelection)
            {
                if (item.Selected)
                    item.Selected = false;
            }
            foreach (CheckBoxComboBoxItem Item in CheckBoxItems)
                if (Item.Checked)
                    Item.Checked = false;
            //base.Invalidate();
            //OnCheckBoxCheckedChanged(this, null);
            ;
        }

        public void FillData(DataTable dt)
        {
            if (dt != null)
            {
                var vatTxts = new List<ValueText>();
                foreach (DataRow dr in dt.Rows)
                {
                    var txt = dr["Text"].ToString();
                    var val = dr["Value"].ToString();
                    vatTxts.Add(new ValueText(val, txt));
                }
                if (vatTxts.Count > 0)
                {
                    DataSourceList = vatTxts;
                }
            }
        }

        #endregion

        #region CHECKBOX PROPERTIES (DEFAULTS)

        private CheckBoxProperties _checkBoxProperties;
        [Description("The properties that will be assigned to the checkboxes as default values.")]
        [Browsable(true)]
        public CheckBoxProperties CheckBoxProperties
        {
            get { return _checkBoxProperties; }
            set { _checkBoxProperties = value; _CheckBoxProperties_PropertyChanged(this, EventArgs.Empty); }
        }

        private void _CheckBoxProperties_PropertyChanged(object sender, EventArgs e)
        {
            foreach (CheckBoxComboBoxItem Item in CheckBoxItems)
                Item.ApplyProperties(CheckBoxProperties);
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

    [ToolboxItem(false)]
    public class CheckBoxComboBoxListControlContainer : UserControl
    {
        #region CONSTRUCTOR

        public CheckBoxComboBoxListControlContainer() : base()
        {
            BackColor = SystemColors.Window;
            BorderStyle = BorderStyle.FixedSingle;
            AutoScaleMode = AutoScaleMode.Inherit;
            ResizeRedraw = true;
            // If you don't set this, then resize operations cause an error in the base class.
            MinimumSize = new Size(1, 1);
            MaximumSize = new Size(500, 500);
        }
        #endregion

        #region RESIZE OVERRIDE REQUIRED BY THE POPUP CONTROL

        /// <summary>
        /// Prescribed by the DropDownStrip class to ensure Resize operations work correctly.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if ((Parent as PopupStrip).ProcessResizing(ref m))
            {
                return;
            }
            base.WndProc(ref m);
        }
        #endregion
    }

    /// <summary>
    /// This ListControl that pops up to the User. It contains the CheckBoxComboBoxItems. 
    /// The items are docked DockStyle.Top in this control.
    /// </summary>
    [ToolboxItem(false)]
    public class CheckBoxComboBoxListControl : ScrollableControl
    {
        #region CONSTRUCTOR

        public CheckBoxComboBoxListControl(CheckBoxComboBox owner)
            : base()
        {
            DoubleBuffered = true;
            _checkBoxComboBox = owner;
            BackColor = SystemColors.Window;
            // AutoScaleMode = AutoScaleMode.Inherit;
            AutoScroll = true;
            ResizeRedraw = true;
            // if you don't set this, a Resize operation causes an error in the base class.
            MinimumSize = new Size(1, 1);
            MaximumSize = new Size(500, 500);
        }

        #endregion

        #region PRIVATE PROPERTIES

        /// <summary>
        /// Simply a reference to the CheckBoxComboBox.
        /// </summary>
        private readonly CheckBoxComboBox _checkBoxComboBox;
        /// <summary>
        /// A Typed list of ComboBoxCheckBoxItems.
        /// </summary>
        private readonly CheckBoxComboBoxItemList _items = new CheckBoxComboBoxItemList();

        #endregion

        public CheckBoxComboBoxItemList Items { get { return _items; } }

        #region RESIZE OVERRIDE REQUIRED BY THE POPUP CONTROL

        protected override void WndProc(ref Message m)
        {
            if ((Parent.Parent as PopupStrip).ProcessResizing(ref m))
            {
                return;
            }
            base.WndProc(ref m);
        }
        #endregion

        #region PROTECTED MEMBERS
        protected override void OnVisibleChanged(EventArgs e)
        {
            // Synchronises the CheckBox list with the items in the ComboBox.
            SynchroniseControlsWithComboBoxItems();
            base.OnVisibleChanged(e);
        }
        /// <summary>
        /// Maintains the controls displayed in the list by keeping them in sync with the actual 
        /// items in the combobox. (e.g. removing and adding as well as ordering)
        /// </summary>
        public void SynchroniseControlsWithComboBoxItems()
        {
            SuspendLayout();
            Controls.Clear();
            #region Disposes all items that are no longer in the combo box list

            for (int Index = _items.Count - 1; Index >= 0; Index--)
            {
                CheckBoxComboBoxItem Item = _items[Index];
                if (!_checkBoxComboBox.Items.Contains(Item.ComboBoxItem))
                {
                    _items.Remove(Item);
                    Item.Dispose();
                }
            }

            #endregion
            #region Recreate the list in the same order of the combo box items

            CheckBoxComboBoxItemList NewList = new CheckBoxComboBoxItemList();
            foreach (object Object in _checkBoxComboBox.Items)
            {
                CheckBoxComboBoxItem Item = _items.Find(new Predicate<CheckBoxComboBoxItem>(
                                                        delegate(CheckBoxComboBoxItem target)
                                                        {
                                                            return target.ComboBoxItem == Object;
                                                        }));
                if (Item == null)
                {
                    Item = new CheckBoxComboBoxItem(_checkBoxComboBox, Object);
                    Item.ApplyProperties(_checkBoxComboBox.CheckBoxProperties);
                    // Item.TextAlign = ContentAlignment.MiddleCenter;
                }
                NewList.Add(Item);
                Item.Dock = DockStyle.Top;
            }
            //NewList.CheckBoxCheckedChanged += _Items.CheckBoxCheckedChanged;
            _items.Clear();
            _items.AddRange(NewList);

            #endregion
            #region Add the items to the controls in reversed order to maintain correct docking order

            if (NewList.Count > 0)
            {
                // This reverse helps to maintain correct docking order.
                NewList.Reverse();
                // If you get an error here that "Cannot convert to the desired type, it probably
                // means the controls are not binding correctly.
                // The Checked property is binded to the ValueMember property. It must be a bool for example.
                Controls.AddRange(NewList.ToArray());
            }

            #endregion
            ResumeLayout();
        }

        #endregion
    }

    /// <summary>
    /// The CheckBox items displayed in the DropDownStrip of the ComboBox.
    /// </summary>
    [ToolboxItem(false)]
    public class CheckBoxComboBoxItem : CheckBox
    {
        #region CONSTRUCTOR
        public CheckBoxComboBoxItem(CheckBoxComboBox owner, object comboBoxItem): base()
        {
            DoubleBuffered = true;
            _CheckBoxComboBox = owner;
            _ComboBoxItem = comboBoxItem;
            if (_CheckBoxComboBox.DataSource != null)
                AddBindings();
            else
                Text = comboBoxItem.ToString();
        }
        #endregion

        #region PRIVATE PROPERTIES

        /// <summary>
        /// A reference to the CheckBoxComboBox.
        /// </summary>
        private CheckBoxComboBox _CheckBoxComboBox;
        /// <summary>
        /// A reference to the Item in ComboBox.Items that this object is extending.
        /// </summary>
        private object _ComboBoxItem;

        #endregion

        #region PUBLIC PROPERTIES

        /// <summary>
        /// A reference to the Item in ComboBox.Items that this object is extending.
        /// </summary>
        public object ComboBoxItem
        {
            get { return _ComboBoxItem; }
        }

        #endregion

        #region BINDING HELPER

        /// <summary>
        /// When using Data Binding operations via the DataSource property of the ComboBox. This
        /// adds the required Bindings for the CheckBoxes.
        /// </summary>
        public void AddBindings()
        {
            // Note, the text uses "DisplayMemberSingleItem", not "DisplayMember" (unless its not assigned)
            DataBindings.Add(
                "Text",
                _ComboBoxItem,
                _CheckBoxComboBox.DisplayMemberSingleItem);
            // The ValueMember must be a bool type property usable by the CheckBox.Checked.
            DataBindings.Add(
                "Checked",
                _ComboBoxItem,
                _CheckBoxComboBox.ValueMember,
                false,
                // This helps to maintain proper selection state in the Binded object,
                // even when the controls are added and removed.
                DataSourceUpdateMode.OnPropertyChanged,
                false, null, null);
        }

        #endregion

        #region PROTECTED MEMBERS

        protected override void OnCheckedChanged(EventArgs e)
        {
            // Found that when this event is raised, the bool value of the binded item is not yet updated.
            if (_CheckBoxComboBox.DataSource != null)
            {
                PropertyInfo PI = ComboBoxItem.GetType().GetProperty(_CheckBoxComboBox.ValueMember);
                PI.SetValue(ComboBoxItem, Checked, null);
            }
            base.OnCheckedChanged(e);
            // Forces a refresh of the Text displayed in the main TextBox of the ComboBox,
            // since that Text will most probably represent a concatenation of selected values.
            // Also see DisplayMemberSingleItem on the CheckBoxComboBox for more information.
            if (_CheckBoxComboBox.DataSource != null)
            {
                string OldDisplayMember = _CheckBoxComboBox.DisplayMember;
                _CheckBoxComboBox.DisplayMember = null;
                _CheckBoxComboBox.DisplayMember = OldDisplayMember;
            }
        }

        #endregion

        #region HELPER MEMBERS

        internal void ApplyProperties(CheckBoxProperties properties)
        {
            this.Appearance = properties.Appearance;
            this.AutoCheck = properties.AutoCheck;
            this.AutoEllipsis = properties.AutoEllipsis;
            this.AutoSize = properties.AutoSize;
            this.CheckAlign = properties.CheckAlign;
            this.FlatAppearance.BorderColor = properties.FlatAppearanceBorderColor;
            this.FlatAppearance.BorderSize = properties.FlatAppearanceBorderSize;
            this.FlatAppearance.CheckedBackColor = properties.FlatAppearanceCheckedBackColor;
            this.FlatAppearance.MouseDownBackColor = properties.FlatAppearanceMouseDownBackColor;
            this.FlatAppearance.MouseOverBackColor = properties.FlatAppearanceMouseOverBackColor;
            this.FlatStyle = properties.FlatStyle;
            this.ForeColor = properties.ForeColor;
            this.RightToLeft = properties.RightToLeft;
            this.TextAlign = properties.TextAlign;
            this.ThreeState = properties.ThreeState;
        }

        #endregion
    }

    /// <summary>
    /// A Typed List of the CheckBox items.
    /// Simply a wrapper for the CheckBoxComboBox.Items. A list of CheckBoxComboBoxItem objects.
    /// This List is automatically synchronised with the Items of the ComboBox and extended to
    /// handle the additional boolean value. That said, do not Add or Remove using this List, 
    /// it will be lost or regenerated from the ComboBox.Items.
    /// </summary>
    [ToolboxItem(false)]
    public class CheckBoxComboBoxItemList : List<CheckBoxComboBoxItem>
    {
        #region EVENTS, This could be moved to the list control if needed

        public event EventHandler CheckBoxCheckedChanged;

        protected void OnCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            EventHandler handler = CheckBoxCheckedChanged;
            if (handler != null)
                handler(sender, e);
        }
        private void item_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckBoxCheckedChanged(sender, e);
        }

        #endregion

        #region LIST MEMBERS & OBSOLETE INDICATORS

        [Obsolete("Do not add items to this list directly. Use the ComboBox items instead.", false)]
        public new void Add(CheckBoxComboBoxItem item)
        {
            item.CheckedChanged += new EventHandler(item_CheckedChanged);
            base.Add(item);
        }

        public new void AddRange(IEnumerable<CheckBoxComboBoxItem> collection)
        {
            foreach (CheckBoxComboBoxItem Item in collection)
                Item.CheckedChanged += new EventHandler(item_CheckedChanged);
            base.AddRange(collection);
        }

        public new void Clear()
        {
            foreach (CheckBoxComboBoxItem Item in this)Item.CheckedChanged -= item_CheckedChanged;
            base.Clear();
        }

        [Obsolete("Do not remove items from this list directly. Use the ComboBox items instead.", false)]
        public new bool Remove(CheckBoxComboBoxItem item)
        {
            item.CheckedChanged -= item_CheckedChanged;
            return base.Remove(item);
        }

        #endregion
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CheckBoxProperties
    {
        public CheckBoxProperties() { }

        #region PRIVATE PROPERTIES

        private Appearance _appearance = Appearance.Normal;
        private bool _autoSize = false;
        private bool _autoCheck = true;
        private bool _autoEllipsis = false;
        private ContentAlignment _checkAlign = ContentAlignment.MiddleLeft;
        private Color _flatAppearanceBorderColor = Color.Empty;
        private int _flatAppearanceBorderSize = 1;
        private Color _flatAppearanceCheckedBackColor = Color.Empty;
        private Color _flatAppearanceMouseDownBackColor = Color.Empty;
        private Color _flatAppearanceMouseOverBackColor = Color.Empty;
        private FlatStyle _flatStyle = FlatStyle.Standard;
        private Color _foreColor = SystemColors.ControlText;
        private RightToLeft _rightToLeft = RightToLeft.No;
        private ContentAlignment _textAlign = ContentAlignment.MiddleLeft;
        private bool _threeState = false;

        #endregion

        #region PUBLIC PROPERTIES

        [DefaultValue(Appearance.Normal)]
        public Appearance Appearance
        {
            get { return _appearance; }
            set { _appearance = value; OnPropertyChanged(); }
        }
        [DefaultValue(true)]
        public bool AutoCheck
        {
            get { return _autoCheck; }
            set { _autoCheck = value; OnPropertyChanged(); }
        }
        [DefaultValue(false)]
        public bool AutoEllipsis
        {
            get { return _autoEllipsis; }
            set { _autoEllipsis = value; OnPropertyChanged(); }
        }
        [DefaultValue(false)]
        public bool AutoSize
        {
            get { return _autoSize; }
            set { _autoSize = true; OnPropertyChanged(); }
        }
        [DefaultValue(ContentAlignment.MiddleLeft)]
        public ContentAlignment CheckAlign
        {
            get { return _checkAlign; }
            set { _checkAlign = value; OnPropertyChanged(); }
        }
        [DefaultValue(typeof(Color), "")]
        public Color FlatAppearanceBorderColor
        {
            get { return _flatAppearanceBorderColor; }
            set { _flatAppearanceBorderColor = value; OnPropertyChanged(); }
        }
        [DefaultValue(1)]
        public int FlatAppearanceBorderSize
        {
            get { return _flatAppearanceBorderSize; }
            set { _flatAppearanceBorderSize = value; OnPropertyChanged(); }
        }
        [DefaultValue(typeof(Color), "")]
        public Color FlatAppearanceCheckedBackColor
        {
            get { return _flatAppearanceCheckedBackColor; }
            set { _flatAppearanceCheckedBackColor = value; OnPropertyChanged(); }
        }
        [DefaultValue(typeof(Color), "")]
        public Color FlatAppearanceMouseDownBackColor
        {
            get { return _flatAppearanceMouseDownBackColor; }
            set { _flatAppearanceMouseDownBackColor = value; OnPropertyChanged(); }
        }
        [DefaultValue(typeof(Color), "")]
        public Color FlatAppearanceMouseOverBackColor
        {
            get { return _flatAppearanceMouseOverBackColor; }
            set { _flatAppearanceMouseOverBackColor = value; OnPropertyChanged(); }
        }
        [DefaultValue(FlatStyle.Standard)]
        public FlatStyle FlatStyle
        {
            get { return _flatStyle; }
            set { _flatStyle = value; OnPropertyChanged(); }
        }
        [DefaultValue(typeof(SystemColors), "ControlText")]
        public Color ForeColor
        {
            get { return _foreColor; }
            set { _foreColor = value; OnPropertyChanged(); }
        }
        [DefaultValue(RightToLeft.No)]
        public RightToLeft RightToLeft
        {
            get { return _rightToLeft; }
            set { _rightToLeft = value; OnPropertyChanged(); }
        }
        [DefaultValue(ContentAlignment.MiddleLeft)]
        public ContentAlignment TextAlign
        {
            get { return _textAlign; }
            set { _textAlign = value; OnPropertyChanged(); }
        }
        [DefaultValue(false)]
        public bool ThreeState
        {
            get { return _threeState; }
            set { _threeState = value; OnPropertyChanged(); }
        }

        #endregion

        #region EVENTS AND EVENT CALLERS
        public event EventHandler PropertyChanged;

        protected void OnPropertyChanged()
        {
            EventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
        #endregion


    }
}
