using Ligg.WinForm.Resources;

namespace Ligg.WinForm.Dialogs
{
    partial class RichTextViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RichTextViewer));
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSaveAs = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonZoomIn = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonExportToExcel = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBoxSearchText = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButtonClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSearch = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFind = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonRestore = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSortAscend = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSortDescend = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.richTextBoxContent = new System.Windows.Forms.RichTextBox();
            this.containerPanelMsg = new Ligg.WinForm.Controls.ContainerPanel();
            this.labelMsgFind = new System.Windows.Forms.Label();
            this.labelMsgLine = new System.Windows.Forms.Label();
            this.GroundPanel.SuspendLayout();
            this.toolBar.SuspendLayout();
            this.containerPanelMsg.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroundPanel
            // 
            this.GroundPanel.Controls.Add(this.containerPanelMsg);
            this.GroundPanel.Controls.Add(this.richTextBoxContent);
            this.GroundPanel.Controls.Add(this.toolBar);
            this.GroundPanel.Size = new System.Drawing.Size(765, 400);
            // 
            // toolBar
            // 
            this.toolBar.GripMargin = new System.Windows.Forms.Padding(2, 2, 4, 2);
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonOpen,
            this.toolStripButtonSave,
            this.toolStripButtonSaveAs,
            this.toolStripButtonExportToExcel,
            this.toolStripSeparator1,
            this.toolStripButtonZoomIn,
            this.toolStripButtonZoomOut,
            this.toolStripSeparator5,
            this.toolStripTextBoxSearchText,
            this.toolStripButtonClear,
            this.toolStripButtonSearch,
            this.toolStripButtonFind,
            this.toolStripSeparator2,
            this.toolStripButtonRestore,
            this.toolStripButtonSortAscend,
            this.toolStripButtonSortDescend,
            this.toolStripSeparator4});
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(765, 25);
            this.toolBar.TabIndex = 0;
            this.toolBar.Text = "toolStrip1";
            // 
            // toolStripButtonOpen
            // 
            this.toolStripButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpen.Image")));
            this.toolStripButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpen.Name = "toolStripButtonOpen";
            this.toolStripButtonOpen.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonOpen.Text = global::Ligg.WinForm.Resources.WinformRes.OpenTextFile;
            this.toolStripButtonOpen.Click += new System.EventHandler(this.toolStripButtonOpen_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSave.Text = global::Ligg.WinForm.Resources.WinformRes.Save;
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripButtonSaveAs
            // 
            this.toolStripButtonSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSaveAs.Image")));
            this.toolStripButtonSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveAs.Name = "toolStripButtonSaveAs";
            this.toolStripButtonSaveAs.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSaveAs.Text = global::Ligg.WinForm.Resources.WinformRes.SaveAs;
            this.toolStripButtonSaveAs.Click += new System.EventHandler(this.toolStripButtonSaveAs_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonZoomIn
            // 
            this.toolStripButtonZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonZoomIn.Image")));
            this.toolStripButtonZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomIn.Name = "toolStripButtonZoomIn";
            this.toolStripButtonZoomIn.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonZoomIn.Text = global::Ligg.WinForm.Resources.WinformRes.ZoomIn;
            this.toolStripButtonZoomIn.Click += new System.EventHandler(this.toolStripButtonZoomIn_Click);
            // 
            // toolStripButtonZoomOut
            // 
            this.toolStripButtonZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonZoomOut.Image")));
            this.toolStripButtonZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomOut.Name = "toolStripButtonZoomOut";
            this.toolStripButtonZoomOut.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonZoomOut.Text = global::Ligg.WinForm.Resources.WinformRes.ZoomOut;
            this.toolStripButtonZoomOut.Click += new System.EventHandler(this.toolStripButtonZoomOut_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonExportToExcel
            // 
            this.toolStripButtonExportToExcel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonExportToExcel.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonExportToExcel.Image")));
            this.toolStripButtonExportToExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExportToExcel.Name = "toolStripButtonExportToExcel";
            this.toolStripButtonExportToExcel.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonExportToExcel.Text = WinformRes.ExportToExcel;
            this.toolStripButtonExportToExcel.Click += new System.EventHandler(this.toolStripButtonExportToExcel_Click);
            // 
            // toolStripTextBoxSearchText
            // 
            this.toolStripTextBoxSearchText.Name = "toolStripTextBoxSearchText";
            this.toolStripTextBoxSearchText.Size = new System.Drawing.Size(200, 25);
            // 
            // toolStripButtonClear
            // 
            this.toolStripButtonClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonClear.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonClear.Image")));
            this.toolStripButtonClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClear.Name = "toolStripButtonClear";
            this.toolStripButtonClear.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonClear.Text = global::Ligg.WinForm.Resources.WinformRes.Clear;
            this.toolStripButtonClear.Click += new System.EventHandler(this.toolStripButtonClear_Click);
            // 
            // toolStripButtonSearch
            // 
            this.toolStripButtonSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSearch.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSearch.Image")));
            this.toolStripButtonSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSearch.Name = "toolStripButtonSearch";
            this.toolStripButtonSearch.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSearch.Text = global::Ligg.WinForm.Resources.WinformRes.Search;
            this.toolStripButtonSearch.Click += new System.EventHandler(this.toolStripButtonSearch_Click);
            // 
            // toolStripButtonFind
            // 
            this.toolStripButtonFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFind.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFind.Image")));
            this.toolStripButtonFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFind.Name = "toolStripButtonFind";
            this.toolStripButtonFind.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonFind.Text = global::Ligg.WinForm.Resources.WinformRes.Find;
            this.toolStripButtonFind.Click += new System.EventHandler(this.toolStripButtonFind_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonRestore
            // 
            this.toolStripButtonRestore.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRestore.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRestore.Image")));
            this.toolStripButtonRestore.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRestore.Name = "toolStripButtonRestore";
            this.toolStripButtonRestore.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRestore.Text = global::Ligg.WinForm.Resources.WinformRes.RestoreOriginalContent;
            this.toolStripButtonRestore.Click += new System.EventHandler(this.toolStripButtonRestore_Click);
            // 
            // toolStripButtonSortAscend
            // 
            this.toolStripButtonSortAscend.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSortAscend.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSortAscend.Image")));
            this.toolStripButtonSortAscend.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSortAscend.Name = "toolStripButtonSortAscend";
            this.toolStripButtonSortAscend.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSortAscend.Text = global::Ligg.WinForm.Resources.WinformRes.SortByAscending;
            this.toolStripButtonSortAscend.Click += new System.EventHandler(this.toolStripButtonSortAscend_Click);
            // 
            // toolStripButtonSortDescend
            // 
            this.toolStripButtonSortDescend.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSortDescend.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSortDescend.Image")));
            this.toolStripButtonSortDescend.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSortDescend.Name = "toolStripButtonSortDescend";
            this.toolStripButtonSortDescend.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSortDescend.Text = global::Ligg.WinForm.Resources.WinformRes.SortByDescending;
            this.toolStripButtonSortDescend.Click += new System.EventHandler(this.toolStripButtonSortDescend_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // richTextBoxContent
            // 
            this.richTextBoxContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.richTextBoxContent.Font = new System.Drawing.Font("Arial", 8.25F);
            this.richTextBoxContent.ImeMode = System.Windows.Forms.ImeMode.On;
            this.richTextBoxContent.Location = new System.Drawing.Point(0, 25);
            this.richTextBoxContent.Margin = new System.Windows.Forms.Padding(6);
            this.richTextBoxContent.Name = "richTextBoxContent";
            this.richTextBoxContent.Size = new System.Drawing.Size(765, 196);
            this.richTextBoxContent.TabIndex = 1;
            this.richTextBoxContent.Text = "";
            this.richTextBoxContent.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.richTextBoxContent_MouseDoubleClick);
            // 
            // containerPanelMsg
            // 
            this.containerPanelMsg.BorderColor = System.Drawing.Color.White;
            this.containerPanelMsg.BorderWidthOnBottom = 0;
            this.containerPanelMsg.BorderWidthOnTop = 1;
            this.containerPanelMsg.Controls.Add(this.labelMsgFind);
            this.containerPanelMsg.Controls.Add(this.labelMsgLine);
            this.containerPanelMsg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.containerPanelMsg.Location = new System.Drawing.Point(0, 376);
            this.containerPanelMsg.Name = "containerPanelMsg";
            this.containerPanelMsg.Radius = 4;
            this.containerPanelMsg.Size = new System.Drawing.Size(765, 24);
            this.containerPanelMsg.StyleType = Ligg.WinForm.Controls.ContainerPanel.ContainerPanelStyle.Borders;
            this.containerPanelMsg.TabIndex = 3;
            // 
            // labelMsgFind
            // 
            this.labelMsgFind.AutoSize = true;
            this.labelMsgFind.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelMsgFind.ForeColor = System.Drawing.Color.Red;
            this.labelMsgFind.Location = new System.Drawing.Point(77, 0);
            this.labelMsgFind.Name = "labelMsgFind";
            this.labelMsgFind.Size = new System.Drawing.Size(77, 12);
            this.labelMsgFind.TabIndex = 2;
            this.labelMsgFind.Text = "labelMsgFind";
            this.labelMsgFind.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMsgLine
            // 
            this.labelMsgLine.AutoSize = true;
            this.labelMsgLine.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelMsgLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.labelMsgLine.Location = new System.Drawing.Point(0, 0);
            this.labelMsgLine.Name = "labelMsgLine";
            this.labelMsgLine.Size = new System.Drawing.Size(77, 12);
            this.labelMsgLine.TabIndex = 1;
            this.labelMsgLine.Text = "labelMsgLine";
            this.labelMsgLine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RichTextViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 430);
            this.MaximizeBox = false;
            this.MaximizeBoxSize = new System.Drawing.Size(0, 0);
            this.MinimizeBox = false;
            this.MinimizeBoxSize = new System.Drawing.Size(0, 0);
            this.Name = "RichTextViewer";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RichTextViewer";
            this.Load += new System.EventHandler(this.MessageViewer_Load);
            this.GroundPanel.ResumeLayout(false);
            this.GroundPanel.PerformLayout();
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.containerPanelMsg.ResumeLayout(false);
            this.containerPanelMsg.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxSearchText;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpen;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSearch;
        private System.Windows.Forms.ToolStripButton toolStripButtonFind;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonRestore;
        private System.Windows.Forms.ToolStripButton toolStripButtonClear;
        private System.Windows.Forms.ToolStripButton toolStripButtonSortAscend;
        private System.Windows.Forms.ToolStripButton toolStripButtonSortDescend;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomIn;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomOut;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButtonExportToExcel;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveAs;
        public System.Windows.Forms.RichTextBox richTextBoxContent;
        private Ligg.WinForm.Controls.ContainerPanel containerPanelMsg;
        private System.Windows.Forms.Label labelMsgFind;
        private System.Windows.Forms.Label labelMsgLine;
    }
}