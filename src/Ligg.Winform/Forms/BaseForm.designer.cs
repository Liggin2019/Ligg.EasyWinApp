using System.Windows.Forms;
using Ligg.WinForm.Controls;
using Ligg.WinForm.DataModel.Enums;

namespace Ligg.WinForm.Forms
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
            this.RunningStatusSection = new Ligg.WinForm.Controls.ContainerPanel();
            this.RunningStatusSectionBackTaskRegion = new System.Windows.Forms.Panel();
            this.RunningStatusSectionBackTaskRegionImageTextButton = new Ligg.WinForm.Controls.ImageTextButton();
            this.RunningStatusSectionBackTaskRegionProgressCircleZone = new System.Windows.Forms.Panel();
            this.RunningStatusSectionMsgRegion = new System.Windows.Forms.Panel();
            this.RunningStatusSectionMsgRegionMsgZone = new System.Windows.Forms.Panel();
            this.RunningStatusSectionMsgRegionLabelMsg3 = new System.Windows.Forms.Label();
            this.RunningStatusSectionMsgRegionLabelMsg2 = new System.Windows.Forms.Label();
            this.RunningStatusSectionMsgRegionLabelMsg1 = new System.Windows.Forms.Label();
            this.RunningStatusSectionMsgRegionLabelMsg = new System.Windows.Forms.Label();
            this.RunningMessageSection = new Ligg.WinForm.Controls.ContainerPanel();
            this.RunningMessageSectionRichTextBox = new System.Windows.Forms.RichTextBox();
            this.BackTaskDetailContainer = new Ligg.WinForm.Controls.PopupContainer();
            this.BackTaskDetailPanel = new System.Windows.Forms.Panel();
            this.BackTaskDetailLabel = new System.Windows.Forms.Label();
            this.GroundPanel.SuspendLayout();
            this.RunningStatusSection.SuspendLayout();
            this.RunningStatusSectionBackTaskRegion.SuspendLayout();
            this.RunningStatusSectionMsgRegion.SuspendLayout();
            this.RunningStatusSectionMsgRegionMsgZone.SuspendLayout();
            this.RunningMessageSection.SuspendLayout();
            this.BackTaskDetailPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroundPanel
            // 
            this.GroundPanel.Controls.Add(this.BackTaskDetailPanel);
            this.GroundPanel.Controls.Add(this.BackTaskDetailContainer);
            this.GroundPanel.Controls.Add(this.RunningMessageSection);
            this.GroundPanel.Controls.Add(this.RunningStatusSection);
            this.GroundPanel.Size = new System.Drawing.Size(796, 570);
            // 
            // RunningStatusSection
            // 
            this.RunningStatusSection.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.RunningStatusSection.BorderWidthOnBottom = 0;
            this.RunningStatusSection.BorderWidthOnTop = 1;
            this.RunningStatusSection.Controls.Add(this.RunningStatusSectionBackTaskRegion);
            this.RunningStatusSection.Controls.Add(this.RunningStatusSectionMsgRegion);
            this.RunningStatusSection.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.RunningStatusSection.Location = new System.Drawing.Point(0, 547);
            this.RunningStatusSection.Name = "RunningStatusSection";
            this.RunningStatusSection.Padding = new System.Windows.Forms.Padding(1);
            this.RunningStatusSection.Radius = 4;
            this.RunningStatusSection.Size = new System.Drawing.Size(796, 23);
            this.RunningStatusSection.StyleType = Ligg.WinForm.Controls.ContainerPanel.ContainerPanelStyle.Borders;
            this.RunningStatusSection.TabIndex = 9;
            // 
            // RunningStatusSectionBackTaskRegion
            // 
            this.RunningStatusSectionBackTaskRegion.Controls.Add(this.RunningStatusSectionBackTaskRegionImageTextButton);
            this.RunningStatusSectionBackTaskRegion.Controls.Add(this.RunningStatusSectionBackTaskRegionProgressCircleZone);
            this.RunningStatusSectionBackTaskRegion.Dock = System.Windows.Forms.DockStyle.Right;
            this.RunningStatusSectionBackTaskRegion.Location = new System.Drawing.Point(645, 1);
            this.RunningStatusSectionBackTaskRegion.Name = "RunningStatusSectionBackTaskRegion";
            this.RunningStatusSectionBackTaskRegion.Size = new System.Drawing.Size(150, 21);
            this.RunningStatusSectionBackTaskRegion.TabIndex = 2;
            // 
            // RunningStatusSectionBackTaskRegionImageTextButton
            // 
            this.RunningStatusSectionBackTaskRegionImageTextButton.Checked = false;
            this.RunningStatusSectionBackTaskRegionImageTextButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.RunningStatusSectionBackTaskRegionImageTextButton.HasBorder = false;
            this.RunningStatusSectionBackTaskRegionImageTextButton.Image = ((System.Drawing.Image)(resources.GetObject("RunningStatusSectionBackTaskRegionImageTextButton.Image")));
            this.RunningStatusSectionBackTaskRegionImageTextButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.RunningStatusSectionBackTaskRegionImageTextButton.ImageHeight = 18;
            this.RunningStatusSectionBackTaskRegionImageTextButton.ImageWidth = 18;
            this.RunningStatusSectionBackTaskRegionImageTextButton.Location = new System.Drawing.Point(30, 0);
            this.RunningStatusSectionBackTaskRegionImageTextButton.Name = "RunningStatusSectionBackTaskRegionImageTextButton";
            this.RunningStatusSectionBackTaskRegionImageTextButton.Radius = 0;
            this.RunningStatusSectionBackTaskRegionImageTextButton.RoundStyle = Ligg.WinForm.DataModel.Enums.RoundStyle.None;
            this.RunningStatusSectionBackTaskRegionImageTextButton.SensitiveType = Ligg.WinForm.DataModel.Enums.ControlSensitiveType.None;
            this.RunningStatusSectionBackTaskRegionImageTextButton.Size = new System.Drawing.Size(101, 21);
            this.RunningStatusSectionBackTaskRegionImageTextButton.TabIndex = 1;
            this.RunningStatusSectionBackTaskRegionImageTextButton.Text = "Task List";
            this.RunningStatusSectionBackTaskRegionImageTextButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.RunningStatusSectionBackTaskRegionImageTextButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.RunningStatusSectionBackTaskRegionImageTextButton.UseVisualStyleBackColor = true;
            this.RunningStatusSectionBackTaskRegionImageTextButton.Click += new System.EventHandler(this.RunningStatusSectionBackTaskRegionImageTextButton_Click);
            // 
            // RunningStatusSectionBackTaskRegionProgressCircleZone
            // 
            this.RunningStatusSectionBackTaskRegionProgressCircleZone.Dock = System.Windows.Forms.DockStyle.Left;
            this.RunningStatusSectionBackTaskRegionProgressCircleZone.Location = new System.Drawing.Point(0, 0);
            this.RunningStatusSectionBackTaskRegionProgressCircleZone.Name = "RunningStatusSectionBackTaskRegionProgressCircleZone";
            this.RunningStatusSectionBackTaskRegionProgressCircleZone.Size = new System.Drawing.Size(30, 21);
            this.RunningStatusSectionBackTaskRegionProgressCircleZone.TabIndex = 20;
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
            this.RunningMessageSection.StyleType = Ligg.WinForm.Controls.ContainerPanel.ContainerPanelStyle.Borders;
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
            // BackTaskDetailContainer
            // 
            this.BackTaskDetailContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.BackTaskDetailContainer.Location = new System.Drawing.Point(396, 257);
            this.BackTaskDetailContainer.Name = "BackTaskDetailContainer";
            this.BackTaskDetailContainer.Size = new System.Drawing.Size(362, 202);
            this.BackTaskDetailContainer.TabIndex = 11;
            this.BackTaskDetailContainer.Visible = false;
            // 
            // BackTaskDetailPanel
            // 
            this.BackTaskDetailPanel.Controls.Add(this.BackTaskDetailLabel);
            this.BackTaskDetailPanel.Location = new System.Drawing.Point(449, 312);
            this.BackTaskDetailPanel.Name = "BackTaskDetailPanel";
            this.BackTaskDetailPanel.Size = new System.Drawing.Size(200, 92);
            this.BackTaskDetailPanel.TabIndex = 14;
            this.BackTaskDetailPanel.Visible = false;
            // 
            // BackTaskDetailLabel
            // 
            this.BackTaskDetailLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.BackTaskDetailLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BackTaskDetailLabel.Location = new System.Drawing.Point(0, 0);
            this.BackTaskDetailLabel.Name = "BackTaskDetailLabel";
            this.BackTaskDetailLabel.Size = new System.Drawing.Size(200, 19);
            this.BackTaskDetailLabel.TabIndex = 0;
            this.BackTaskDetailLabel.Text = "BackTaskLabel";
            this.BackTaskDetailLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BackTaskDetailLabel.Visible = false;
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
            this.RunningStatusSectionBackTaskRegion.ResumeLayout(false);
            this.RunningStatusSectionMsgRegion.ResumeLayout(false);
            this.RunningStatusSectionMsgRegionMsgZone.ResumeLayout(false);
            this.RunningStatusSectionMsgRegionMsgZone.PerformLayout();
            this.RunningMessageSection.ResumeLayout(false);
            this.BackTaskDetailPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public Ligg.WinForm.Controls.ContainerPanel RunningMessageSection;
        public System.Windows.Forms.RichTextBox RunningMessageSectionRichTextBox;
        public Ligg.WinForm.Controls.ContainerPanel RunningStatusSection;
        public System.Windows.Forms.Panel RunningStatusSectionMsgRegion;
        private Panel RunningStatusSectionMsgRegionMsgZone;
        public Label RunningStatusSectionMsgRegionLabelMsg2;
        public Label RunningStatusSectionMsgRegionLabelMsg1;
        public Label RunningStatusSectionMsgRegionLabelMsg;
        public ImageTextButton RunningStatusSectionBackTaskRegionImageTextButton;
        public Panel RunningStatusSectionBackTaskRegion;
        private PopupContainer BackTaskDetailContainer;
        private Panel BackTaskDetailPanel;
        //public ListViewEx BackgroundTaskDetailListViewEx;
        private Panel RunningStatusSectionBackTaskRegionProgressCircleZone;
        //public ProgressCircle RunningStatusSectionBackgroundTaskRegionProgressCircle;
        public Label RunningStatusSectionMsgRegionLabelMsg3;
        protected Label BackTaskDetailLabel;
    }
}