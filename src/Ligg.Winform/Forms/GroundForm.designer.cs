namespace Ligg.WinForm.Forms
{
    partial class GroundForm
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
            this.GroundPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // GroundPanel
            // 
            this.GroundPanel.BackColor = StyleSheet.GroundColor;
            this.GroundPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.GroundPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroundPanel.Location = new System.Drawing.Point(2, 28);
            this.GroundPanel.Name = "GroundPanel";
            this.GroundPanel.Size = new System.Drawing.Size(296, 270);
            this.GroundPanel.TabIndex = 0;
            // 
            // GroundForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Controls.Add(this.GroundPanel);
            this.Name = "GroundForm";
            this.Text = "Ground";
            this.Load += new System.EventHandler(this.GroundForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel GroundPanel;

    }
}