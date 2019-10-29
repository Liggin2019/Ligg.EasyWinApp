namespace Ligg.Winform.Controls
{
    partial class TimerExRecurringRun
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimerExRecurringRun));
            this.labelTimeLeft = new System.Windows.Forms.Label();
            this.textBoxStartTime = new System.Windows.Forms.TextBox();
            this.timerTrigger = new System.Windows.Forms.Timer(this.components);
            this.labelLeftTime = new System.Windows.Forms.Label();
            this.labelStartTime = new System.Windows.Forms.Label();
            this.pictureBoxInputStartTime = new System.Windows.Forms.PictureBox();
            this.labelLatestStartTime = new System.Windows.Forms.Label();
            this.textBoxLatestStartTime = new System.Windows.Forms.TextBox();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.pictureBoxStartOrPause = new System.Windows.Forms.PictureBox();
            this.textBoxFirstRunTime = new System.Windows.Forms.TextBox();
            this.labelFirstRunTime = new System.Windows.Forms.Label();
            this.labelCycleTime = new System.Windows.Forms.Label();
            this.textBoxCyclePeriod = new System.Windows.Forms.TextBox();
            this.comboBoxUnit = new System.Windows.Forms.ComboBox();
            this.pictureBoxInputFirstRunTime = new System.Windows.Forms.PictureBox();
            this.pictureBoxShowLog = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInputStartTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStartOrPause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInputFirstRunTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxShowLog)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTimeLeft
            // 
            this.labelTimeLeft.AutoSize = true;
            this.labelTimeLeft.Location = new System.Drawing.Point(3, 3);
            this.labelTimeLeft.Name = "labelTimeLeft";
            this.labelTimeLeft.Size = new System.Drawing.Size(59, 12);
            this.labelTimeLeft.TabIndex = 1;
            this.labelTimeLeft.Text = "Time Left";
            // 
            // textBoxStartTime
            // 
            this.textBoxStartTime.Location = new System.Drawing.Point(3, 44);
            this.textBoxStartTime.Name = "textBoxStartTime";
            this.textBoxStartTime.Size = new System.Drawing.Size(130, 21);
            this.textBoxStartTime.TabIndex = 2;
            // 
            // timerTrigger
            // 
            this.timerTrigger.Tick += new System.EventHandler(this.timerTrigger_Tick);
            // 
            // labelLeftTime
            // 
            this.labelLeftTime.AutoSize = true;
            this.labelLeftTime.ForeColor = System.Drawing.Color.Red;
            this.labelLeftTime.Location = new System.Drawing.Point(79, 3);
            this.labelLeftTime.Name = "labelLeftTime";
            this.labelLeftTime.Size = new System.Drawing.Size(0, 12);
            this.labelLeftTime.TabIndex = 4;
            // 
            // labelStartTime
            // 
            this.labelStartTime.AutoSize = true;
            this.labelStartTime.Location = new System.Drawing.Point(3, 26);
            this.labelStartTime.Name = "labelStartTime";
            this.labelStartTime.Size = new System.Drawing.Size(65, 12);
            this.labelStartTime.TabIndex = 5;
            this.labelStartTime.Text = "Start Time";
            // 
            // pictureBoxInputStartTime
            // 
            this.pictureBoxInputStartTime.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxInputStartTime.Location = new System.Drawing.Point(135, 42);
            this.pictureBoxInputStartTime.Name = "pictureBoxInputStartTime";
            this.pictureBoxInputStartTime.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxInputStartTime.TabIndex = 6;
            this.pictureBoxInputStartTime.TabStop = false;
            this.pictureBoxInputStartTime.Click += new System.EventHandler(this.pictureBoxInputStartTime_Click);
            // 
            // labelLatestStartTime
            // 
            this.labelLatestStartTime.AutoSize = true;
            this.labelLatestStartTime.Location = new System.Drawing.Point(165, 26);
            this.labelLatestStartTime.Name = "labelLatestStartTime";
            this.labelLatestStartTime.Size = new System.Drawing.Size(107, 12);
            this.labelLatestStartTime.TabIndex = 7;
            this.labelLatestStartTime.Text = "Latest Start Time";
            // 
            // textBoxLatestStartTime
            // 
            this.textBoxLatestStartTime.Location = new System.Drawing.Point(165, 44);
            this.textBoxLatestStartTime.Name = "textBoxLatestStartTime";
            this.textBoxLatestStartTime.Size = new System.Drawing.Size(130, 21);
            this.textBoxLatestStartTime.TabIndex = 8;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "input.png");
            this.imageList.Images.SetKeyName(1, "start.png");
            this.imageList.Images.SetKeyName(2, "pause.png");
            this.imageList.Images.SetKeyName(3, "log.png");
            // 
            // pictureBoxStartOrPause
            // 
            this.pictureBoxStartOrPause.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxStartOrPause.Location = new System.Drawing.Point(283, 3);
            this.pictureBoxStartOrPause.Name = "pictureBoxStartOrPause";
            this.pictureBoxStartOrPause.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxStartOrPause.TabIndex = 9;
            this.pictureBoxStartOrPause.TabStop = false;
            this.pictureBoxStartOrPause.Click += new System.EventHandler(this.pictureBoxStartOrPause_Click);
            // 
            // textBoxFirstRunTime
            // 
            this.textBoxFirstRunTime.Location = new System.Drawing.Point(3, 91);
            this.textBoxFirstRunTime.Name = "textBoxFirstRunTime";
            this.textBoxFirstRunTime.Size = new System.Drawing.Size(130, 21);
            this.textBoxFirstRunTime.TabIndex = 10;
            // 
            // labelFirstRunTime
            // 
            this.labelFirstRunTime.AutoSize = true;
            this.labelFirstRunTime.Location = new System.Drawing.Point(7, 73);
            this.labelFirstRunTime.Name = "labelFirstRunTime";
            this.labelFirstRunTime.Size = new System.Drawing.Size(89, 12);
            this.labelFirstRunTime.TabIndex = 11;
            this.labelFirstRunTime.Text = "First Run Time";
            // 
            // labelCycleTime
            // 
            this.labelCycleTime.AutoSize = true;
            this.labelCycleTime.Location = new System.Drawing.Point(165, 73);
            this.labelCycleTime.Name = "labelCycleTime";
            this.labelCycleTime.Size = new System.Drawing.Size(65, 12);
            this.labelCycleTime.TabIndex = 12;
            this.labelCycleTime.Text = "Cycle Time";
            // 
            // textBoxCyclePeriod
            // 
            this.textBoxCyclePeriod.Location = new System.Drawing.Point(165, 91);
            this.textBoxCyclePeriod.Name = "textBoxCyclePeriod";
            this.textBoxCyclePeriod.Size = new System.Drawing.Size(48, 21);
            this.textBoxCyclePeriod.TabIndex = 13;
            this.textBoxCyclePeriod.TextChanged += new System.EventHandler(this.textBoxCyclePeriod_TextChanged);
            // 
            // comboBoxUnit
            // 
            this.comboBoxUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUnit.FormattingEnabled = true;
            this.comboBoxUnit.Location = new System.Drawing.Point(218, 91);
            this.comboBoxUnit.Name = "comboBoxUnit";
            this.comboBoxUnit.Size = new System.Drawing.Size(77, 20);
            this.comboBoxUnit.TabIndex = 14;
            this.comboBoxUnit.SelectedIndexChanged += new System.EventHandler(this.comboBoxUnit_SelectedIndexChanged);
            // 
            // pictureBoxInputFirstRunTime
            // 
            this.pictureBoxInputFirstRunTime.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxInputFirstRunTime.Location = new System.Drawing.Point(135, 90);
            this.pictureBoxInputFirstRunTime.Name = "pictureBoxInputFirstRunTime";
            this.pictureBoxInputFirstRunTime.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxInputFirstRunTime.TabIndex = 15;
            this.pictureBoxInputFirstRunTime.TabStop = false;
            this.pictureBoxInputFirstRunTime.Click += new System.EventHandler(this.pictureBoxInputFirstRunTime_Click);
            // 
            // pictureBoxShowLog
            // 
            this.pictureBoxShowLog.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxShowLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxShowLog.Location = new System.Drawing.Point(256, 3);
            this.pictureBoxShowLog.Name = "pictureBoxShowLog";
            this.pictureBoxShowLog.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxShowLog.TabIndex = 16;
            this.pictureBoxShowLog.TabStop = false;
            this.pictureBoxShowLog.Click += new System.EventHandler(this.pictureBoxShowLog_Click);
            // 
            // TimerExRecurringRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.pictureBoxShowLog);
            this.Controls.Add(this.pictureBoxInputFirstRunTime);
            this.Controls.Add(this.comboBoxUnit);
            this.Controls.Add(this.textBoxCyclePeriod);
            this.Controls.Add(this.labelCycleTime);
            this.Controls.Add(this.labelFirstRunTime);
            this.Controls.Add(this.textBoxFirstRunTime);
            this.Controls.Add(this.pictureBoxStartOrPause);
            this.Controls.Add(this.labelLatestStartTime);
            this.Controls.Add(this.textBoxLatestStartTime);
            this.Controls.Add(this.pictureBoxInputStartTime);
            this.Controls.Add(this.labelStartTime);
            this.Controls.Add(this.labelLeftTime);
            this.Controls.Add(this.textBoxStartTime);
            this.Controls.Add(this.labelTimeLeft);
            this.Name = "TimerExRecurringRun";
            this.Size = new System.Drawing.Size(310, 120);
            this.Load += new System.EventHandler(this.TimerEx_Load);
            this.Resize += new System.EventHandler(this.TimerExTimingRun_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInputStartTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStartOrPause)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInputFirstRunTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxShowLog)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelTimeLeft;
        private System.Windows.Forms.TextBox textBoxStartTime;
        private System.Windows.Forms.Timer timerTrigger;
        private System.Windows.Forms.Label labelLeftTime;
        private System.Windows.Forms.Label labelStartTime;
        private System.Windows.Forms.PictureBox pictureBoxInputStartTime;
        private System.Windows.Forms.Label labelLatestStartTime;
        private System.Windows.Forms.TextBox textBoxLatestStartTime;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.PictureBox pictureBoxStartOrPause;
        private System.Windows.Forms.TextBox textBoxFirstRunTime;
        private System.Windows.Forms.Label labelFirstRunTime;
        private System.Windows.Forms.Label labelCycleTime;
        private System.Windows.Forms.TextBox textBoxCyclePeriod;
        private System.Windows.Forms.ComboBox comboBoxUnit;
        private System.Windows.Forms.PictureBox pictureBoxInputFirstRunTime;
        private System.Windows.Forms.PictureBox pictureBoxShowLog;
    }
}
