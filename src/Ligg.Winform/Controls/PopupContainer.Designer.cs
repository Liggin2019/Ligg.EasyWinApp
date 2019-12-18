namespace Ligg.WinForm.Controls
{
    partial class PopupContainer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PopupContainer));
            this.shadowPanel = new Ligg.WinForm.Controls.ShadowPanel.ShadowPanel();
            this.closeButton = new System.Windows.Forms.PictureBox();
            this.shadowPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.closeButton)).BeginInit();
            this.SuspendLayout();
            // 
            // shadowPanel
            // 
            this.shadowPanel.BorderColor = System.Drawing.Color.Empty;
            this.shadowPanel.Controls.Add(this.closeButton);
            this.shadowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shadowPanel.Location = new System.Drawing.Point(0, 0);
            this.shadowPanel.Name = "shadowPanel";
            this.shadowPanel.PanelColor = System.Drawing.Color.Empty;
            this.shadowPanel.Size = new System.Drawing.Size(366, 219);
            this.shadowPanel.TabIndex = 0;
            // 
            // closeButton
            // 
            this.closeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("closeButton.BackgroundImage")));
            this.closeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.closeButton.Location = new System.Drawing.Point(348, 0);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(18, 18);
            this.closeButton.TabIndex = 0;
            this.closeButton.TabStop = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // PopupContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.shadowPanel);
            this.Name = "PopupContainer";
            this.Size = new System.Drawing.Size(366, 219);
            this.Load += new System.EventHandler(this.PopupContainer_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PopupContainer_Paint);
            this.shadowPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.closeButton)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShadowPanel.ShadowPanel shadowPanel;
        private System.Windows.Forms.PictureBox closeButton;
    }
}
