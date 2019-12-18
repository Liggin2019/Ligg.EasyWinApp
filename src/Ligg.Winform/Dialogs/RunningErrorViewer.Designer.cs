using Ligg.WinForm.Forms;

namespace Ligg.WinForm.Dialogs
{
    using Ligg.WinForm.DataModel.Enums;
    partial class RunningErrorViewer:GroundForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RunningErrorViewer));
            this.imageTextButtonSendErrorInfo = new Ligg.WinForm.Controls.ImageTextButton();
            this.pictureBoxError = new System.Windows.Forms.PictureBox();
            this.labelError = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.richTextBoxError = new System.Windows.Forms.RichTextBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.GroundPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxError)).BeginInit();
            this.panelTop.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroundPanel
            // 
            this.GroundPanel.Controls.Add(this.panelBottom);
            this.GroundPanel.Controls.Add(this.richTextBoxError);
            this.GroundPanel.Controls.Add(this.panelTop);
            this.GroundPanel.Size = new System.Drawing.Size(467, 188);
            // 
            // imageTextButtonSendErrorInfo
            // 
            this.imageTextButtonSendErrorInfo.AutoSize = true;
            this.imageTextButtonSendErrorInfo.Checked = false;
            this.imageTextButtonSendErrorInfo.Dock = System.Windows.Forms.DockStyle.Right;
            this.imageTextButtonSendErrorInfo.ForeColor = System.Drawing.Color.DimGray;
            this.imageTextButtonSendErrorInfo.HasBorder = false;
            this.imageTextButtonSendErrorInfo.Image = ((System.Drawing.Image)(resources.GetObject("imageTextButtonSendErrorInfo.Image")));
            this.imageTextButtonSendErrorInfo.ImageHeight = 16;
            this.imageTextButtonSendErrorInfo.ImageWidth = 16;
            this.imageTextButtonSendErrorInfo.Location = new System.Drawing.Point(241, 0);
            this.imageTextButtonSendErrorInfo.Name = "imageTextButtonSendErrorInfo";
            this.imageTextButtonSendErrorInfo.Radius = 4;
            this.imageTextButtonSendErrorInfo.RoundStyle = Ligg.WinForm.DataModel.Enums.RoundStyle.None;
            this.imageTextButtonSendErrorInfo.Size = new System.Drawing.Size(226, 27);
            this.imageTextButtonSendErrorInfo.SensitiveType = Ligg.WinForm.DataModel.Enums.ControlSensitiveType.None;
            this.imageTextButtonSendErrorInfo.TabIndex = 3;
            this.imageTextButtonSendErrorInfo.Text = "Send error info. to developer";
            this.imageTextButtonSendErrorInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.imageTextButtonSendErrorInfo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.imageTextButtonSendErrorInfo.UseVisualStyleBackColor = true;
            this.imageTextButtonSendErrorInfo.Click += new System.EventHandler(this.imageTextButtonSendErrorInfo_Click);
            // 
            // pictureBoxError
            // 
            this.pictureBoxError.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxError.Image")));
            this.pictureBoxError.Location = new System.Drawing.Point(4, 5);
            this.pictureBoxError.Name = "pictureBoxError";
            this.pictureBoxError.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxError.TabIndex = 5;
            this.pictureBoxError.TabStop = false;
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.Font = new System.Drawing.Font("SimSun", 9F);
            this.labelError.Location = new System.Drawing.Point(24, 7);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(83, 12);
            this.labelError.TabIndex = 9;
            this.labelError.Text = "Error message";
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.labelError);
            this.panelTop.Controls.Add(this.pictureBoxError);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(467, 27);
            this.panelTop.TabIndex = 100;
            // 
            // richTextBoxError
            // 
            this.richTextBoxError.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBoxError.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxError.Dock = System.Windows.Forms.DockStyle.Top;
            this.richTextBoxError.Location = new System.Drawing.Point(0, 27);
            this.richTextBoxError.Name = "richTextBoxError";
            this.richTextBoxError.ReadOnly = true;
            this.richTextBoxError.Size = new System.Drawing.Size(467, 128);
            this.richTextBoxError.TabIndex = 101;
            this.richTextBoxError.Text = "";
            this.richTextBoxError.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.richTextBoxError_MouseDoubleClick);
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.imageTextButtonSendErrorInfo);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 161);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(467, 27);
            this.panelBottom.TabIndex = 102;
            // 
            // RunningErrorViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 218);
            this.Name = "RunningErrorViewer";
            this.Text = "RunningErrorViewer";
            this.Load += new System.EventHandler(this.RunningErrorViewer_Load);
            this.GroundPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxError)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ImageTextButton imageTextButtonSendErrorInfo;
        private System.Windows.Forms.PictureBox pictureBoxError;
        private System.Windows.Forms.Label labelError;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.RichTextBox richTextBoxError;
        private System.Windows.Forms.Panel panelTop;


    }
}