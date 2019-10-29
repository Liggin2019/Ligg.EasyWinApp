
using Ligg.Winform.DataModel.Enums;

namespace Ligg.Winform.Dialogs
{
    partial class DateTimeInputDialog
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
            this.textButtonOk = new Ligg.Winform.Controls.TextButton();
            this.textButtonCancel = new Ligg.Winform.Controls.TextButton();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.GroundPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroundPanel
            // 
            this.GroundPanel.Controls.Add(this.dateTimePicker);
            this.GroundPanel.Controls.Add(this.textButtonCancel);
            this.GroundPanel.Controls.Add(this.textButtonOk);
            this.GroundPanel.Size = new System.Drawing.Size(310, 99);
            // 
            // textButtonOk
            // 
            this.textButtonOk.Checked = false;
            this.textButtonOk.HasBorder = false;
            this.textButtonOk.Location = new System.Drawing.Point(27, 58);
            this.textButtonOk.Name = "textButtonOk";
            this.textButtonOk.Radius = 0;
            this.textButtonOk.RoundStyle = Ligg.Winform.DataModel.Enums.RoundStyle.None;
            this.textButtonOk.SensitiveType = Ligg.Winform.DataModel.Enums.ControlSensitiveType.None;
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
            this.textButtonCancel.RoundStyle = Ligg.Winform.DataModel.Enums.RoundStyle.None;
            this.textButtonCancel.SensitiveType = Ligg.Winform.DataModel.Enums.ControlSensitiveType.None;
            this.textButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.textButtonCancel.TabIndex = 2;
            this.textButtonCancel.Text = "Cancel";
            this.textButtonCancel.UseVisualStyleBackColor = true;
            this.textButtonCancel.Click += new System.EventHandler(this.textButtonCancel_Click);
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker.Location = new System.Drawing.Point(50, 21);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(215, 21);
            this.dateTimePicker.TabIndex = 3;
            // 
            // DateTimeInputDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 129);
            this.KeyPreview = true;
            this.Name = "DateTimeInputDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "DateTimeInputDialog";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.DateTimeInputDialog_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Dialog_KeyPress);
            this.GroundPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Ligg.Winform.Controls.TextButton textButtonOk;
        private Ligg.Winform.Controls.TextButton textButtonCancel;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
    }
}