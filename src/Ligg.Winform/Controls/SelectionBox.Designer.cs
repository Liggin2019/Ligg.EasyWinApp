namespace Ligg.Winform.Controls
{
    partial class SelectionBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectionBox));
            this.textBox = new System.Windows.Forms.TextBox();
            this.popupButton = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.popupButton)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBox.Location = new System.Drawing.Point(0, 0);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(156, 22);
            this.textBox.TabIndex = 0;
            this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // popupButton
            // 
            this.popupButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.popupButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.popupButton.ErrorImage = null;
            this.popupButton.Image = ((System.Drawing.Image)(resources.GetObject("popupButton.Image")));
            this.popupButton.ImageLocation = "";
            this.popupButton.Location = new System.Drawing.Point(162, 0);
            this.popupButton.Name = "popupButton";
            this.popupButton.Size = new System.Drawing.Size(19, 22);
            this.popupButton.TabIndex = 1;
            this.popupButton.TabStop = false;
            this.popupButton.Click += new System.EventHandler(this.popupButton_Click);
            // 
            // SelectionBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.popupButton);
            this.Controls.Add(this.textBox);
            this.Name = "SelectionBox";
            this.Size = new System.Drawing.Size(181, 22);
            this.Load += new System.EventHandler(this.SelectionBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.popupButton)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.PictureBox popupButton;


    }
}
