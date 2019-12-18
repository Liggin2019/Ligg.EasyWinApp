
using Ligg.WinForm.DataModel.Enums;

namespace Ligg.WinForm.Dialogs
{
    partial class TextInputDialog
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
            this.textBoxInput = new System.Windows.Forms.TextBox();
            this.textButtonOk = new Ligg.WinForm.Controls.TextButton();
            this.textButtonCancel = new Ligg.WinForm.Controls.TextButton();
            this.GroundPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroundPanel
            // 
            this.GroundPanel.Controls.Add(this.textButtonCancel);
            this.GroundPanel.Controls.Add(this.textButtonOk);
            this.GroundPanel.Controls.Add(this.textBoxInput);
            this.GroundPanel.Size = new System.Drawing.Size(310, 95);
            // 
            // textBoxInput
            // 
            this.textBoxInput.Location = new System.Drawing.Point(30, 22);
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.Size = new System.Drawing.Size(254, 21);
            this.textBoxInput.TabIndex = 0;
            // 
            // textButtonOk
            // 
            this.textButtonOk.Checked = false;
            this.textButtonOk.HasBorder = false;
            this.textButtonOk.Location = new System.Drawing.Point(27, 58);
            this.textButtonOk.Name = "textButtonOk";
            this.textButtonOk.Radius = 0;
            this.textButtonOk.RoundStyle = Ligg.WinForm.DataModel.Enums.RoundStyle.None;
            this.textButtonOk.SensitiveType = Ligg.WinForm.DataModel.Enums.ControlSensitiveType.None;
            this.textButtonOk.Size = new System.Drawing.Size(75, 23);
            this.textButtonOk.TabIndex = 1;
            this.textButtonOk.Text = "OK";
            this.textButtonOk.UseVisualStyleBackColor = true;
            this.textButtonOk.Click += new System.EventHandler(this.textButtonOk_Click);
            // 
            // textButtonCancel
            // 
            this.textButtonCancel.Checked = false;
            this.textButtonCancel.HasBorder = false;
            this.textButtonCancel.Location = new System.Drawing.Point(209, 58);
            this.textButtonCancel.Name = "textButtonCancel";
            this.textButtonCancel.Radius = 0;
            this.textButtonCancel.RoundStyle = Ligg.WinForm.DataModel.Enums.RoundStyle.None;
            this.textButtonCancel.SensitiveType = Ligg.WinForm.DataModel.Enums.ControlSensitiveType.None;
            this.textButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.textButtonCancel.TabIndex = 2;
            this.textButtonCancel.Text = "Cancel";
            this.textButtonCancel.UseVisualStyleBackColor = true;
            this.textButtonCancel.Click += new System.EventHandler(this.textButtonCancel_Click);
            // 
            // TextInputDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 125);
            this.KeyPreview = true;
            this.Name = "TextInputDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CheckInputDialog";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.TextInputDialog_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Dialog_KeyPress);
            this.GroundPanel.ResumeLayout(false);
            this.GroundPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxInput;
        private Ligg.WinForm.Controls.TextButton textButtonOk;
        private Ligg.WinForm.Controls.TextButton textButtonCancel;
    }
}