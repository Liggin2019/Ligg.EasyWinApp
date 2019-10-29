namespace Ligg.Winform.Dialogs
{
    partial class MessageViewer
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
            this.richTextBoxContent = new System.Windows.Forms.RichTextBox();
            this.panelBase = new System.Windows.Forms.Panel();
            this.GroundPanel.SuspendLayout();
            this.panelBase.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroundPanel
            // 
            this.GroundPanel.Controls.Add(this.panelBase);
            this.GroundPanel.Size = new System.Drawing.Size(765, 400);
            // 
            // richTextBoxContent
            // 
            this.richTextBoxContent.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBoxContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxContent.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxContent.ForeColor = System.Drawing.SystemColors.WindowText;
            this.richTextBoxContent.ImeMode = System.Windows.Forms.ImeMode.On;
            this.richTextBoxContent.Location = new System.Drawing.Point(1, 1);
            this.richTextBoxContent.Margin = new System.Windows.Forms.Padding(6);
            this.richTextBoxContent.Name = "richTextBoxContent";
            this.richTextBoxContent.Size = new System.Drawing.Size(763, 398);
            this.richTextBoxContent.TabIndex = 1;
            this.richTextBoxContent.Text = "";
            this.richTextBoxContent.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.richTextBoxContent_MouseDoubleClick);
            // 
            // panelBase
            // 
            this.panelBase.Controls.Add(this.richTextBoxContent);
            this.panelBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBase.Location = new System.Drawing.Point(0, 0);
            this.panelBase.Name = "panelBase";
            this.panelBase.Padding = new System.Windows.Forms.Padding(1);
            this.panelBase.Size = new System.Drawing.Size(765, 400);
            this.panelBase.TabIndex = 2;
            // 
            // MessageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 430);
            this.MaximizeBox = false;
            this.MaximizeBoxSize = new System.Drawing.Size(0, 0);
            this.MinimizeBox = false;
            this.MinimizeBoxSize = new System.Drawing.Size(0, 0);
            this.Name = "MessageViewer";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MessageViewer";
            this.Load += new System.EventHandler(this.MessageViewer_Load);
            this.GroundPanel.ResumeLayout(false);
            this.panelBase.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.RichTextBox richTextBoxContent;
        private System.Windows.Forms.Panel panelBase;
    }
}