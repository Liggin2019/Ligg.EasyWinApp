using System.Windows.Forms;
using Ligg.Winform.Controls;
using Ligg.Winform.DataModel.Enums;

namespace Ligg.Winform.Forms
{
    partial class BaseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseForm));
            this.RunningStatusSection = new Ligg.Winform.Controls.ContainerPanel();
            this.RunningStatusSectionBackgroundTaskRegion = new System.Windows.Forms.Panel();
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton = new Ligg.Winform.Controls.ImageTextButton();
            this.RunningStatusSectionBackgroundTaskRegionProgressCircleZone = new System.Windows.Forms.Panel();
            this.RunningStatusSectionBackgroundTaskRegionProgressCircle = new Ligg.Winform.Controls.ProgressCircle();
            this.RunningStatusSectionMsgRegion = new System.Windows.Forms.Panel();
            this.RunningStatusSectionMsgRegionMsgZone = new System.Windows.Forms.Panel();
            this.RunningStatusSectionMsgRegionLabelMsg3 = new System.Windows.Forms.Label();
            this.RunningStatusSectionMsgRegionLabelMsg2 = new System.Windows.Forms.Label();
            this.RunningStatusSectionMsgRegionLabelMsg1 = new System.Windows.Forms.Label();
            this.RunningStatusSectionMsgRegionLabelMsg = new System.Windows.Forms.Label();
            this.RunningMessageSection = new Ligg.Winform.Controls.ContainerPanel();
            this.RunningMessageSectionRichTextBox = new System.Windows.Forms.RichTextBox();
            this.BackgroundTaskDetailContainer = new Ligg.Winform.Controls.PopupContainer();
            //this.BackgroundTaskDetailListViewEx = new Ligg.Winform.Controls.ListViewEx();
            this.BackgroundTaskDetailPanel = new System.Windows.Forms.Panel();
            this.BackgroundTaskDetailLabel = new System.Windows.Forms.Label();
            this.GroundPanel.SuspendLayout();
            this.RunningStatusSection.SuspendLayout();
            this.RunningStatusSectionBackgroundTaskRegion.SuspendLayout();
            this.RunningStatusSectionBackgroundTaskRegionProgressCircleZone.SuspendLayout();
            this.RunningStatusSectionMsgRegion.SuspendLayout();
            this.RunningStatusSectionMsgRegionMsgZone.SuspendLayout();
            this.RunningMessageSection.SuspendLayout();
            this.BackgroundTaskDetailPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroundPanel
            // 
            this.GroundPanel.Controls.Add(this.BackgroundTaskDetailPanel);
            this.GroundPanel.Controls.Add(this.BackgroundTaskDetailContainer);
            this.GroundPanel.Controls.Add(this.RunningMessageSection);
            this.GroundPanel.Controls.Add(this.RunningStatusSection);
            this.GroundPanel.Size = new System.Drawing.Size(796, 570);
            // 
            // RunningStatusSection
            // 
            this.RunningStatusSection.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.RunningStatusSection.BorderWidthOnBottom = 0;
            this.RunningStatusSection.BorderWidthOnTop = 1;
            this.RunningStatusSection.Controls.Add(this.RunningStatusSectionBackgroundTaskRegion);
            this.RunningStatusSection.Controls.Add(this.RunningStatusSectionMsgRegion);
            this.RunningStatusSection.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.RunningStatusSection.Location = new System.Drawing.Point(0, 547);
            this.RunningStatusSection.Name = "RunningStatusSection";
            this.RunningStatusSection.Padding = new System.Windows.Forms.Padding(1);
            this.RunningStatusSection.Radius = 4;
            this.RunningStatusSection.Size = new System.Drawing.Size(796, 23);
            this.RunningStatusSection.StyleType = Ligg.Winform.Controls.ContainerPanel.ContainerPanelStyle.Borders;
            this.RunningStatusSection.TabIndex = 9;
            // 
            // RunningStatusSectionBackgroundTaskRegion
            // 
            this.RunningStatusSectionBackgroundTaskRegion.Controls.Add(this.RunningStatusSectionBackgroundTaskRegionImageTextButton);
            this.RunningStatusSectionBackgroundTaskRegion.Controls.Add(this.RunningStatusSectionBackgroundTaskRegionProgressCircleZone);
            this.RunningStatusSectionBackgroundTaskRegion.Dock = System.Windows.Forms.DockStyle.Right;
            this.RunningStatusSectionBackgroundTaskRegion.Location = new System.Drawing.Point(645, 1);
            this.RunningStatusSectionBackgroundTaskRegion.Name = "RunningStatusSectionBackgroundTaskRegion";
            this.RunningStatusSectionBackgroundTaskRegion.Size = new System.Drawing.Size(150, 21);
            this.RunningStatusSectionBackgroundTaskRegion.TabIndex = 2;
            // 
            // RunningStatusSectionBackgroundTaskRegionImageTextButton
            // 
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.Checked = false;
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.HasBorder = false;
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.Image = ((System.Drawing.Image)(resources.GetObject("RunningStatusSectionBackgroundTaskRegionImageTextButton.Image")));
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.ImageHeight = 18;
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.ImageWidth = 18;
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.Location = new System.Drawing.Point(30, 0);
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.Name = "RunningStatusSectionBackgroundTaskRegionImageTextButton";
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.Radius = 0;
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.RoundStyle = Ligg.Winform.DataModel.Enums.RoundStyle.None;
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.Size = new System.Drawing.Size(101, 21);
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.SensitiveType = ControlSensitiveType.None;
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.TabIndex = 1;
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.Text = "Task List";
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.UseVisualStyleBackColor = true;
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.Click += new System.EventHandler(this.RunningStatusSectionBackTaskRegionImageTextButton_Click);
            // 
            // RunningStatusSectionBackgroundTaskRegionProgressCircleZone
            // 
            this.RunningStatusSectionBackgroundTaskRegionProgressCircleZone.Controls.Add(this.RunningStatusSectionBackgroundTaskRegionProgressCircle);
            this.RunningStatusSectionBackgroundTaskRegionProgressCircleZone.Dock = System.Windows.Forms.DockStyle.Left;
            this.RunningStatusSectionBackgroundTaskRegionProgressCircleZone.Location = new System.Drawing.Point(0, 0);
            this.RunningStatusSectionBackgroundTaskRegionProgressCircleZone.Name = "RunningStatusSectionBackgroundTaskRegionProgressCircleZone";
            this.RunningStatusSectionBackgroundTaskRegionProgressCircleZone.Size = new System.Drawing.Size(30, 21);
            this.RunningStatusSectionBackgroundTaskRegionProgressCircleZone.TabIndex = 20;
            // 
            // RunningStatusSectionBackgroundTaskRegionProgressCircle
            // 
            this.RunningStatusSectionBackgroundTaskRegionProgressCircle.ForeColor = System.Drawing.Color.Red;
            this.RunningStatusSectionBackgroundTaskRegionProgressCircle.Location = new System.Drawing.Point(0, 0);
            this.RunningStatusSectionBackgroundTaskRegionProgressCircle.Margin = new System.Windows.Forms.Padding(0);
            this.RunningStatusSectionBackgroundTaskRegionProgressCircle.Name = "RunningStatusSectionBackgroundTaskRegionProgressCircle";
            this.RunningStatusSectionBackgroundTaskRegionProgressCircle.Size = new System.Drawing.Size(27, 21);
            this.RunningStatusSectionBackgroundTaskRegionProgressCircle.TabIndex = 18;
            // 
            // RunningStatusSectionMsgRegion
            // 
            this.RunningStatusSectionMsgRegion.Controls.Add(this.RunningStatusSectionMsgRegionMsgZone);
            this.RunningStatusSectionMsgRegion.Dock = System.Windows.Forms.DockStyle.Left;
            this.RunningStatusSectionMsgRegion.Location = new System.Drawing.Point(1, 1);
            this.RunningStatusSectionMsgRegion.Margin = new System.Windows.Forms.Padding(1);
            this.RunningStatusSectionMsgRegion.Name = "RunningStatusSectionMsgRegion";
            this.RunningStatusSectionMsgRegion.Size = new System.Drawing.Size(536, 21);
            this.RunningStatusSectionMsgRegion.TabIndex = 1;
            // 
            // RunningStatusSectionMsgRegionMsgZone
            // 
            this.RunningStatusSectionMsgRegionMsgZone.Controls.Add(this.RunningStatusSectionMsgRegionLabelMsg3);
            this.RunningStatusSectionMsgRegionMsgZone.Controls.Add(this.RunningStatusSectionMsgRegionLabelMsg2);
            this.RunningStatusSectionMsgRegionMsgZone.Controls.Add(this.RunningStatusSectionMsgRegionLabelMsg1);
            this.RunningStatusSectionMsgRegionMsgZone.Controls.Add(this.RunningStatusSectionMsgRegionLabelMsg);
            this.RunningStatusSectionMsgRegionMsgZone.Dock = System.Windows.Forms.DockStyle.Left;
            this.RunningStatusSectionMsgRegionMsgZone.Location = new System.Drawing.Point(0, 0);
            this.RunningStatusSectionMsgRegionMsgZone.Name = "RunningStatusSectionMsgRegionMsgZone";
            this.RunningStatusSectionMsgRegionMsgZone.Padding = new System.Windows.Forms.Padding(0, 4, 4, 4);
            this.RunningStatusSectionMsgRegionMsgZone.Size = new System.Drawing.Size(423, 21);
            this.RunningStatusSectionMsgRegionMsgZone.TabIndex = 18;
            // 
            // RunningStatusSectionMsgRegionLabelMsg3
            // 
            this.RunningStatusSectionMsgRegionLabelMsg3.AutoSize = true;
            this.RunningStatusSectionMsgRegionLabelMsg3.Dock = System.Windows.Forms.DockStyle.Left;
            this.RunningStatusSectionMsgRegionLabelMsg3.Location = new System.Drawing.Point(81, 4);
            this.RunningStatusSectionMsgRegionLabelMsg3.Name = "RunningStatusSectionMsgRegionLabelMsg3";
            this.RunningStatusSectionMsgRegionLabelMsg3.Size = new System.Drawing.Size(29, 12);
            this.RunningStatusSectionMsgRegionLabelMsg3.TabIndex = 20;
            this.RunningStatusSectionMsgRegionLabelMsg3.Text = "msg3";
            this.RunningStatusSectionMsgRegionLabelMsg3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RunningStatusSectionMsgRegionLabelMsg2
            // 
            this.RunningStatusSectionMsgRegionLabelMsg2.AutoSize = true;
            this.RunningStatusSectionMsgRegionLabelMsg2.Dock = System.Windows.Forms.DockStyle.Left;
            this.RunningStatusSectionMsgRegionLabelMsg2.Location = new System.Drawing.Point(52, 4);
            this.RunningStatusSectionMsgRegionLabelMsg2.Name = "RunningStatusSectionMsgRegionLabelMsg2";
            this.RunningStatusSectionMsgRegionLabelMsg2.Size = new System.Drawing.Size(29, 12);
            this.RunningStatusSectionMsgRegionLabelMsg2.TabIndex = 19;
            this.RunningStatusSectionMsgRegionLabelMsg2.Text = "msg2";
            this.RunningStatusSectionMsgRegionLabelMsg2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RunningStatusSectionMsgRegionLabelMsg1
            // 
            this.RunningStatusSectionMsgRegionLabelMsg1.AutoSize = true;
            this.RunningStatusSectionMsgRegionLabelMsg1.Dock = System.Windows.Forms.DockStyle.Left;
            this.RunningStatusSectionMsgRegionLabelMsg1.Location = new System.Drawing.Point(23, 4);
            this.RunningStatusSectionMsgRegionLabelMsg1.Name = "RunningStatusSectionMsgRegionLabelMsg1";
            this.RunningStatusSectionMsgRegionLabelMsg1.Size = new System.Drawing.Size(29, 12);
            this.RunningStatusSectionMsgRegionLabelMsg1.TabIndex = 18;
            this.RunningStatusSectionMsgRegionLabelMsg1.Text = "msg1";
            this.RunningStatusSectionMsgRegionLabelMsg1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RunningStatusSectionMsgRegionLabelMsg
            // 
            this.RunningStatusSectionMsgRegionLabelMsg.AutoSize = true;
            this.RunningStatusSectionMsgRegionLabelMsg.Dock = System.Windows.Forms.DockStyle.Left;
            this.RunningStatusSectionMsgRegionLabelMsg.Location = new System.Drawing.Point(0, 4);
            this.RunningStatusSectionMsgRegionLabelMsg.Name = "RunningStatusSectionMsgRegionLabelMsg";
            this.RunningStatusSectionMsgRegionLabelMsg.Size = new System.Drawing.Size(23, 12);
            this.RunningStatusSectionMsgRegionLabelMsg.TabIndex = 17;
            this.RunningStatusSectionMsgRegionLabelMsg.Text = "msg";
            this.RunningStatusSectionMsgRegionLabelMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RunningMessageSection
            // 
            this.RunningMessageSection.BorderColor = System.Drawing.Color.Transparent;
            this.RunningMessageSection.BorderWidthOnBottom = 0;
            this.RunningMessageSection.BorderWidthOnTop = 1;
            this.RunningMessageSection.Controls.Add(this.RunningMessageSectionRichTextBox);
            this.RunningMessageSection.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.RunningMessageSection.Location = new System.Drawing.Point(0, 475);
            this.RunningMessageSection.Name = "RunningMessageSection";
            this.RunningMessageSection.Radius = 4;
            this.RunningMessageSection.Size = new System.Drawing.Size(796, 72);
            this.RunningMessageSection.StyleType = Ligg.Winform.Controls.ContainerPanel.ContainerPanelStyle.Borders;
            this.RunningMessageSection.TabIndex = 10;
            // 
            // RunningMessageSectionRichTextBox
            // 
            this.RunningMessageSectionRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RunningMessageSectionRichTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.RunningMessageSectionRichTextBox.Location = new System.Drawing.Point(0, 31);
            this.RunningMessageSectionRichTextBox.Name = "RunningMessageSectionRichTextBox";
            this.RunningMessageSectionRichTextBox.Size = new System.Drawing.Size(796, 41);
            this.RunningMessageSectionRichTextBox.TabIndex = 0;
            this.RunningMessageSectionRichTextBox.Text = "message";
            // 
            // BackgroundTaskDetailContainer
            // 
            this.BackgroundTaskDetailContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.BackgroundTaskDetailContainer.Location = new System.Drawing.Point(396, 257);
            this.BackgroundTaskDetailContainer.Name = "BackgroundTaskDetailContainer";
            this.BackgroundTaskDetailContainer.Size = new System.Drawing.Size(362, 202);
            this.BackgroundTaskDetailContainer.TabIndex = 11;
            this.BackgroundTaskDetailContainer.Visible = false;
            // 
            // BackgroundTaskDetailListViewEx
            // 
            //this.BackgroundTaskDetailListViewEx.CanOrder = false;
            //this.BackgroundTaskDetailListViewEx.Dock = System.Windows.Forms.DockStyle.Top;
            //this.BackgroundTaskDetailListViewEx.HasPager = false;
            //this.BackgroundTaskDetailListViewEx.IsOrderDescending = false;
            //this.BackgroundTaskDetailListViewEx.Location = new System.Drawing.Point(0, 19);
            //this.BackgroundTaskDetailListViewEx.Name = "BackgroundTaskDetailListViewEx";
            //this.BackgroundTaskDetailListViewEx.OrderFieldName = null;
            //this.BackgroundTaskDetailListViewEx.Size = new System.Drawing.Size(200, 57);
            //this.BackgroundTaskDetailListViewEx.TabIndex = 13;
            //this.BackgroundTaskDetailListViewEx.Visible = false;
            // 
            // BackgroundTaskDetailPanel
            // 
            //this.BackgroundTaskDetailPanel.Controls.Add(this.BackgroundTaskDetailListViewEx);
            this.BackgroundTaskDetailPanel.Controls.Add(this.BackgroundTaskDetailLabel);
            this.BackgroundTaskDetailPanel.Location = new System.Drawing.Point(449, 312);
            this.BackgroundTaskDetailPanel.Name = "BackgroundTaskDetailPanel";
            this.BackgroundTaskDetailPanel.Size = new System.Drawing.Size(200, 92);
            this.BackgroundTaskDetailPanel.TabIndex = 14;
            this.BackgroundTaskDetailPanel.Visible = false;
            // 
            // BackgroundTaskDetailLabel
            // 
            this.BackgroundTaskDetailLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.BackgroundTaskDetailLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BackgroundTaskDetailLabel.Location = new System.Drawing.Point(0, 0);
            this.BackgroundTaskDetailLabel.Name = "BackgroundTaskDetailLabel";
            this.BackgroundTaskDetailLabel.Size = new System.Drawing.Size(200, 19);
            this.BackgroundTaskDetailLabel.TabIndex = 0;
            this.BackgroundTaskDetailLabel.Text = "BackTaskLabel";
            this.BackgroundTaskDetailLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BackgroundTaskDetailLabel.Visible = false;
            // 
            // BaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "BaseForm";
            this.Text = "BaseForm";
            this.Load += new System.EventHandler(this.BaseForm_Load);
            this.GroundPanel.ResumeLayout(false);
            this.RunningStatusSection.ResumeLayout(false);
            this.RunningStatusSectionBackgroundTaskRegion.ResumeLayout(false);
            this.RunningStatusSectionBackgroundTaskRegionProgressCircleZone.ResumeLayout(false);
            this.RunningStatusSectionMsgRegion.ResumeLayout(false);
            this.RunningStatusSectionMsgRegionMsgZone.ResumeLayout(false);
            this.RunningStatusSectionMsgRegionMsgZone.PerformLayout();
            this.RunningMessageSection.ResumeLayout(false);
            this.BackgroundTaskDetailPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public Ligg.Winform.Controls.ContainerPanel RunningMessageSection;
        public System.Windows.Forms.RichTextBox RunningMessageSectionRichTextBox;
        public Ligg.Winform.Controls.ContainerPanel RunningStatusSection;
        public System.Windows.Forms.Panel RunningStatusSectionMsgRegion;
        private Panel RunningStatusSectionMsgRegionMsgZone;
        public Label RunningStatusSectionMsgRegionLabelMsg2;
        public Label RunningStatusSectionMsgRegionLabelMsg1;
        public Label RunningStatusSectionMsgRegionLabelMsg;
        public ImageTextButton RunningStatusSectionBackgroundTaskRegionImageTextButton;
        public Panel RunningStatusSectionBackgroundTaskRegion;
        private PopupContainer BackgroundTaskDetailContainer;
        private Panel BackgroundTaskDetailPanel;
        //public ListViewEx BackgroundTaskDetailListViewEx;
        private Panel RunningStatusSectionBackgroundTaskRegionProgressCircleZone;
        public ProgressCircle RunningStatusSectionBackgroundTaskRegionProgressCircle;
        public Label RunningStatusSectionMsgRegionLabelMsg3;
        protected Label BackgroundTaskDetailLabel;
    }
}