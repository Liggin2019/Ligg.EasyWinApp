using System.Windows.Forms;
using Ligg.Winform.Controls;
using Ligg.Winform.Helpers;

namespace Ligg.Winform.Forms
{
    partial class FrameForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrameForm));
            this.MainMenuSection = new Ligg.Winform.Controls.ContainerPanel();
            this.MainMenuSectionRightRegion = new System.Windows.Forms.Panel();
            this.MainMenuSectionCenterRegion = new System.Windows.Forms.Panel();
            this.MainMenuSectionLeftRegion = new System.Windows.Forms.Panel();
            this.ToolBarSection = new Ligg.Winform.Controls.ContainerPanel();
            this.ToolBarSectionRightRegion = new System.Windows.Forms.Panel();
            this.ToolBarSectionCenterRegion = new System.Windows.Forms.Panel();
            this.ToolBarSectionPublicRegion = new System.Windows.Forms.Panel();
            this.ToolBarSectionPublicRegionToolStrip = new System.Windows.Forms.ToolStrip();
            this.ToolBarSectionLeftRegion = new System.Windows.Forms.Panel();
            this.NavigationSection = new Ligg.Winform.Controls.ContainerPanel();
            this.NavigationSectionCenterRegion = new System.Windows.Forms.Panel();
            this.NavigationSectionRightRegion = new System.Windows.Forms.Panel();
            this.NavigationSectionLeftRegion = new System.Windows.Forms.Panel();
            this.ShortcutSection = new Ligg.Winform.Controls.ContainerPanel();
            this.ShortcutSectionRightRegion = new System.Windows.Forms.Panel();
            this.ShortcutSectionCenterRegion = new System.Windows.Forms.Panel();
            this.ShortcutSectionLeftRegion = new System.Windows.Forms.Panel();
            this.MainSection = new Ligg.Winform.Controls.ContainerPanel();
            this.MainSectionMainDivision = new System.Windows.Forms.Panel();
            this.MainSectionMainDivisionDownRegion = new System.Windows.Forms.Panel();
            this.MainSectionMainDivisionMidRegion = new System.Windows.Forms.Panel();
            this.MainSectionMainDivisionUpRegion = new System.Windows.Forms.Panel();
            this.MainSectionRightDivision = new System.Windows.Forms.Panel();
            this.MainSectionRightDivisionDownRegion = new System.Windows.Forms.Panel();
            this.MainSectionRightDivisionMidRegion = new System.Windows.Forms.Panel();
            this.MainSectionRightDivisionUpRegion = new System.Windows.Forms.Panel();
            this.MainSectionHorizontalResizeDivision = new System.Windows.Forms.Panel();
            this.HorizontalResizeButton = new System.Windows.Forms.PictureBox();
            this.MainSectionSplitter = new System.Windows.Forms.Splitter();
            this.MainSectionRightNavDivision = new System.Windows.Forms.Panel();
            this.MainSectionRightNavDivisionDownRegion = new System.Windows.Forms.Panel();
            this.MainSectionRightNavDivisionMidRegion = new System.Windows.Forms.Panel();
            this.MainSectionRightNavDivisionUpRegion = new System.Windows.Forms.Panel();
            this.MainSectionLeftNavDivision = new System.Windows.Forms.Panel();
            this.MainSectionLeftNavDivisionDownRegion = new System.Windows.Forms.Panel();
            this.MainSectionLeftNavDivisionMidRegion = new System.Windows.Forms.Panel();
            this.MainSectionLeftNavDivisionUpRegion = new System.Windows.Forms.Panel();
            this.PictureList = new System.Windows.Forms.ImageList(this.components);
            this.RunningMessageSection.SuspendLayout();
            this.RunningStatusSection.SuspendLayout();
            this.RunningStatusSectionBackgroundTaskRegion.SuspendLayout();
            this.GroundPanel.SuspendLayout();
            this.MainMenuSection.SuspendLayout();
            this.ToolBarSection.SuspendLayout();
            this.ToolBarSectionPublicRegion.SuspendLayout();
            this.NavigationSection.SuspendLayout();
            this.ShortcutSection.SuspendLayout();
            this.ShortcutSectionRightRegion.SuspendLayout();
            this.MainSection.SuspendLayout();
            this.MainSectionMainDivision.SuspendLayout();
            this.MainSectionRightDivision.SuspendLayout();
            this.MainSectionHorizontalResizeDivision.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HorizontalResizeButton)).BeginInit();
            this.MainSectionRightNavDivision.SuspendLayout();
            this.MainSectionLeftNavDivision.SuspendLayout();
            this.SuspendLayout();
            // 
            // RunningMessageSection
            // 
            this.RunningMessageSection.BackColor = System.Drawing.SystemColors.Window;
            this.RunningMessageSection.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.RunningMessageSection.Location = new System.Drawing.Point(0, 517);
            this.RunningMessageSection.Padding = new System.Windows.Forms.Padding(2);
            this.RunningMessageSection.Size = new System.Drawing.Size(916, 49);
            // 
            // RunningMessageSectionRichTextBox
            // 
            this.RunningMessageSectionRichTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.RunningMessageSectionRichTextBox.Location = new System.Drawing.Point(2, 3);
            this.RunningMessageSectionRichTextBox.Size = new System.Drawing.Size(912, 44);
            // 
            // RunningStatusSection
            // 
            this.RunningStatusSection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.RunningStatusSection.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.RunningStatusSection.Location = new System.Drawing.Point(0, 566);
            this.RunningStatusSection.Size = new System.Drawing.Size(916, 24);
            // 
            // RunningStatusSectionMsgRegion
            // 
            this.RunningStatusSectionMsgRegion.Size = new System.Drawing.Size(536, 22);
            // 
            // RunningStatusSectionMsgRegionLabelMsg2
            // 
            this.RunningStatusSectionMsgRegionLabelMsg2.Location = new System.Drawing.Point(0, 4);
            this.RunningStatusSectionMsgRegionLabelMsg2.Size = new System.Drawing.Size(0, 12);
            this.RunningStatusSectionMsgRegionLabelMsg2.Text = "";
            // 
            // RunningStatusSectionMsgRegionLabelMsg1
            // 
            this.RunningStatusSectionMsgRegionLabelMsg1.Location = new System.Drawing.Point(0, 4);
            this.RunningStatusSectionMsgRegionLabelMsg1.Size = new System.Drawing.Size(0, 12);
            this.RunningStatusSectionMsgRegionLabelMsg1.Text = "";
            // 
            // RunningStatusSectionMsgRegionLabelMsg
            // 
            this.RunningStatusSectionMsgRegionLabelMsg.Size = new System.Drawing.Size(0, 12);
            this.RunningStatusSectionMsgRegionLabelMsg.Text = "";
            // 
            // RunningStatusSectionBackgroundTaskRegionImageTextButton
            // 
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.Location = new System.Drawing.Point(0, 0);
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.Size = new System.Drawing.Size(150, 22);
            this.RunningStatusSectionBackgroundTaskRegionImageTextButton.Text = " Task List 0/0";
            // 
            // RunningStatusSectionBackgroundTaskRegion
            // 
            this.RunningStatusSectionBackgroundTaskRegion.Location = new System.Drawing.Point(765, 1);
            this.RunningStatusSectionBackgroundTaskRegion.Size = new System.Drawing.Size(150, 22);
            // 
            // BackgroundTaskDetailListViewEx
            // 
            //this.BackgroundTaskDetailListViewEx.Location = new System.Drawing.Point(0, 18);
            // 
            // RunningStatusSectionBackgroundTaskRegionProgressCircle
            // 
            this.RunningStatusSectionBackgroundTaskRegionProgressCircle.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(145)))), ((int)(((byte)(242)))));
            this.RunningStatusSectionBackgroundTaskRegionProgressCircle.Location = new System.Drawing.Point(1, 1);
            this.RunningStatusSectionBackgroundTaskRegionProgressCircle.RingThickness = 1;
            this.RunningStatusSectionBackgroundTaskRegionProgressCircle.Size = new System.Drawing.Size(0, 21);
            this.RunningStatusSectionBackgroundTaskRegionProgressCircle.SpokeNumber = 10;
            this.RunningStatusSectionBackgroundTaskRegionProgressCircle.SpokeThickness = 3;
            // 
            // RunningStatusSectionMsgRegionLabelMsg3
            // 
            this.RunningStatusSectionMsgRegionLabelMsg3.Location = new System.Drawing.Point(0, 4);
            this.RunningStatusSectionMsgRegionLabelMsg3.Size = new System.Drawing.Size(0, 12);
            this.RunningStatusSectionMsgRegionLabelMsg3.Text = "";
            // 
            // BackgroundTaskDetailLabel
            // 
            this.BackgroundTaskDetailLabel.Size = new System.Drawing.Size(200, 18);
            this.BackgroundTaskDetailLabel.Text = " Task List";
            // 
            // GroundPanel
            // 
            this.GroundPanel.BackColor = System.Drawing.Color.White;
            this.GroundPanel.Controls.Add(this.MainSection);
            this.GroundPanel.Controls.Add(this.ShortcutSection);
            this.GroundPanel.Controls.Add(this.NavigationSection);
            this.GroundPanel.Controls.Add(this.ToolBarSection);
            this.GroundPanel.Controls.Add(this.MainMenuSection);
            this.GroundPanel.Size = new System.Drawing.Size(916, 590);
            this.GroundPanel.Controls.SetChildIndex(this.RunningStatusSection, 0);
            this.GroundPanel.Controls.SetChildIndex(this.RunningMessageSection, 0);
            this.GroundPanel.Controls.SetChildIndex(this.MainMenuSection, 0);
            this.GroundPanel.Controls.SetChildIndex(this.ToolBarSection, 0);
            this.GroundPanel.Controls.SetChildIndex(this.NavigationSection, 0);
            this.GroundPanel.Controls.SetChildIndex(this.ShortcutSection, 0);
            this.GroundPanel.Controls.SetChildIndex(this.MainSection, 0);
            // 
            // MainMenuSection
            // 
            this.MainMenuSection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(145)))), ((int)(((byte)(242)))));
            this.MainMenuSection.BorderColor = System.Drawing.Color.Empty;
            this.MainMenuSection.BorderWidthOnBottom = 0;
            this.MainMenuSection.Controls.Add(this.MainMenuSectionRightRegion);
            this.MainMenuSection.Controls.Add(this.MainMenuSectionCenterRegion);
            this.MainMenuSection.Controls.Add(this.MainMenuSectionLeftRegion);
            this.MainMenuSection.Dock = System.Windows.Forms.DockStyle.Top;
            this.MainMenuSection.Location = new System.Drawing.Point(0, 0);
            this.MainMenuSection.Name = "MainMenuSection";
            this.MainMenuSection.Radius = 4;
            this.MainMenuSection.Size = new System.Drawing.Size(916, 28);
            this.MainMenuSection.StyleType = Ligg.Winform.Controls.ContainerPanel.ContainerPanelStyle.None;
            this.MainMenuSection.TabIndex = 22;
            // 
            // MainMenuSectionRightRegion
            // 
            this.MainMenuSectionRightRegion.Dock = System.Windows.Forms.DockStyle.Right;
            this.MainMenuSectionRightRegion.Location = new System.Drawing.Point(716, 0);
            this.MainMenuSectionRightRegion.Name = "MainMenuSectionRightRegion";
            this.MainMenuSectionRightRegion.Size = new System.Drawing.Size(200, 28);
            this.MainMenuSectionRightRegion.TabIndex = 24;
            // 
            // MainMenuSectionCenterRegion
            // 
            this.MainMenuSectionCenterRegion.Dock = System.Windows.Forms.DockStyle.Left;
            this.MainMenuSectionCenterRegion.Location = new System.Drawing.Point(164, 0);
            this.MainMenuSectionCenterRegion.Name = "MainMenuSectionCenterRegion";
            this.MainMenuSectionCenterRegion.Size = new System.Drawing.Size(152, 28);
            this.MainMenuSectionCenterRegion.TabIndex = 23;
            // 
            // MainMenuSectionLeftRegion
            // 
            this.MainMenuSectionLeftRegion.Dock = System.Windows.Forms.DockStyle.Left;
            this.MainMenuSectionLeftRegion.Location = new System.Drawing.Point(0, 0);
            this.MainMenuSectionLeftRegion.Name = "MainMenuSectionLeftRegion";
            this.MainMenuSectionLeftRegion.Size = new System.Drawing.Size(164, 28);
            this.MainMenuSectionLeftRegion.TabIndex = 20;
            // 
            // ToolBarSection
            // 
            this.ToolBarSection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(145)))), ((int)(((byte)(242)))));
            this.ToolBarSection.BorderColor = System.Drawing.Color.White;
            this.ToolBarSection.BorderWidthOnBottom = 0;
            this.ToolBarSection.Controls.Add(this.ToolBarSectionRightRegion);
            this.ToolBarSection.Controls.Add(this.ToolBarSectionCenterRegion);
            this.ToolBarSection.Controls.Add(this.ToolBarSectionPublicRegion);
            this.ToolBarSection.Controls.Add(this.ToolBarSectionLeftRegion);
            this.ToolBarSection.Dock = System.Windows.Forms.DockStyle.Top;
            this.ToolBarSection.Location = new System.Drawing.Point(0, 28);
            this.ToolBarSection.Name = "ToolBarSection";
            this.ToolBarSection.Radius = 4;
            this.ToolBarSection.Size = new System.Drawing.Size(916, 59);
            this.ToolBarSection.StyleType = Ligg.Winform.Controls.ContainerPanel.ContainerPanelStyle.None;
            this.ToolBarSection.TabIndex = 21;
            // 
            // ToolBarSectionRightRegion
            // 
            this.ToolBarSectionRightRegion.Dock = System.Windows.Forms.DockStyle.Left;
            this.ToolBarSectionRightRegion.Location = new System.Drawing.Point(285, 0);
            this.ToolBarSectionRightRegion.Name = "ToolBarSectionRightRegion";
            this.ToolBarSectionRightRegion.Size = new System.Drawing.Size(202, 59);
            this.ToolBarSectionRightRegion.TabIndex = 25;
            // 
            // ToolBarSectionCenterRegion
            // 
            this.ToolBarSectionCenterRegion.Dock = System.Windows.Forms.DockStyle.Left;
            this.ToolBarSectionCenterRegion.Location = new System.Drawing.Point(158, 0);
            this.ToolBarSectionCenterRegion.Name = "ToolBarSectionCenterRegion";
            this.ToolBarSectionCenterRegion.Size = new System.Drawing.Size(127, 59);
            this.ToolBarSectionCenterRegion.TabIndex = 24;
            // 
            // ToolBarSectionPublicRegion
            // 
            this.ToolBarSectionPublicRegion.BackColor = System.Drawing.Color.Red;
            this.ToolBarSectionPublicRegion.Controls.Add(this.ToolBarSectionPublicRegionToolStrip);
            this.ToolBarSectionPublicRegion.Dock = System.Windows.Forms.DockStyle.Right;
            this.ToolBarSectionPublicRegion.Location = new System.Drawing.Point(811, 0);
            this.ToolBarSectionPublicRegion.Name = "ToolBarSectionPublicRegion";
            this.ToolBarSectionPublicRegion.Size = new System.Drawing.Size(105, 59);
            this.ToolBarSectionPublicRegion.TabIndex = 23;
            // 
            // ToolBarSectionPublicRegionToolStrip
            // 
            this.ToolBarSectionPublicRegionToolStrip.AutoSize = false;
            this.ToolBarSectionPublicRegionToolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolBarSectionPublicRegionToolStrip.GripMargin = new System.Windows.Forms.Padding(2, 2, 4, 2);
            this.ToolBarSectionPublicRegionToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolBarSectionPublicRegionToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.ToolBarSectionPublicRegionToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.ToolBarSectionPublicRegionToolStrip.Location = new System.Drawing.Point(0, 0);
            this.ToolBarSectionPublicRegionToolStrip.Name = "ToolBarSectionPublicRegionToolStrip";
            this.ToolBarSectionPublicRegionToolStrip.Size = new System.Drawing.Size(105, 59);
            this.ToolBarSectionPublicRegionToolStrip.TabIndex = 22;
            this.ToolBarSectionPublicRegionToolStrip.Tag = "$Main";
            // 
            // ToolBarSectionLeftRegion
            // 
            this.ToolBarSectionLeftRegion.Dock = System.Windows.Forms.DockStyle.Left;
            this.ToolBarSectionLeftRegion.Location = new System.Drawing.Point(0, 0);
            this.ToolBarSectionLeftRegion.Name = "ToolBarSectionLeftRegion";
            this.ToolBarSectionLeftRegion.Size = new System.Drawing.Size(158, 59);
            this.ToolBarSectionLeftRegion.TabIndex = 19;
            // 
            // NavigationSection
            // 
            this.NavigationSection.BorderColor = System.Drawing.Color.Empty;
            this.NavigationSection.BorderWidthOnBottom = 0;
            this.NavigationSection.Controls.Add(this.NavigationSectionCenterRegion);
            this.NavigationSection.Controls.Add(this.NavigationSectionRightRegion);
            this.NavigationSection.Controls.Add(this.NavigationSectionLeftRegion);
            this.NavigationSection.Dock = System.Windows.Forms.DockStyle.Top;
            this.NavigationSection.Location = new System.Drawing.Point(0, 87);
            this.NavigationSection.Name = "NavigationSection";
            this.NavigationSection.Radius = 4;
            this.NavigationSection.Size = new System.Drawing.Size(916, 26);
            this.NavigationSection.StyleType = Ligg.Winform.Controls.ContainerPanel.ContainerPanelStyle.None;
            this.NavigationSection.TabIndex = 18;
            // 
            // NavigationSectionCenterRegion
            // 
            this.NavigationSectionCenterRegion.Dock = System.Windows.Forms.DockStyle.Left;
            this.NavigationSectionCenterRegion.Location = new System.Drawing.Point(46, 0);
            this.NavigationSectionCenterRegion.Name = "NavigationSectionCenterRegion";
            this.NavigationSectionCenterRegion.Size = new System.Drawing.Size(128, 26);
            this.NavigationSectionCenterRegion.TabIndex = 18;
            // 
            // NavigationSectionRightRegion
            // 
            this.NavigationSectionRightRegion.Dock = System.Windows.Forms.DockStyle.Right;
            this.NavigationSectionRightRegion.Location = new System.Drawing.Point(761, 0);
            this.NavigationSectionRightRegion.Name = "NavigationSectionRightRegion";
            this.NavigationSectionRightRegion.Size = new System.Drawing.Size(155, 26);
            this.NavigationSectionRightRegion.TabIndex = 17;
            // 
            // NavigationSectionLeftRegion
            // 
            this.NavigationSectionLeftRegion.Dock = System.Windows.Forms.DockStyle.Left;
            this.NavigationSectionLeftRegion.Location = new System.Drawing.Point(0, 0);
            this.NavigationSectionLeftRegion.Name = "NavigationSectionLeftRegion";
            this.NavigationSectionLeftRegion.Size = new System.Drawing.Size(46, 26);
            this.NavigationSectionLeftRegion.TabIndex = 16;
            // 
            // ShortcutSection
            // 
            this.ShortcutSection.BorderColor = System.Drawing.Color.Empty;
            this.ShortcutSection.BorderWidthOnBottom = 0;
            this.ShortcutSection.Controls.Add(this.ShortcutSectionRightRegion);
            this.ShortcutSection.Controls.Add(this.ShortcutSectionLeftRegion);
            this.ShortcutSection.Dock = System.Windows.Forms.DockStyle.Top;
            this.ShortcutSection.Location = new System.Drawing.Point(0, 113);
            this.ShortcutSection.Name = "ShortcutSection";
            this.ShortcutSection.Radius = 4;
            this.ShortcutSection.Size = new System.Drawing.Size(916, 26);
            this.ShortcutSection.StyleType = Ligg.Winform.Controls.ContainerPanel.ContainerPanelStyle.None;
            this.ShortcutSection.TabIndex = 15;
            // 
            // ShortcutSectionRightRegion
            // 
            this.ShortcutSectionRightRegion.Controls.Add(this.ShortcutSectionCenterRegion);
            this.ShortcutSectionRightRegion.Dock = System.Windows.Forms.DockStyle.Left;
            this.ShortcutSectionRightRegion.Location = new System.Drawing.Point(81, 0);
            this.ShortcutSectionRightRegion.Name = "ShortcutSectionRightRegion";
            this.ShortcutSectionRightRegion.Size = new System.Drawing.Size(737, 26);
            this.ShortcutSectionRightRegion.TabIndex = 14;
            // 
            // ShortcutSectionCenterRegion
            // 
            this.ShortcutSectionCenterRegion.Dock = System.Windows.Forms.DockStyle.Left;
            this.ShortcutSectionCenterRegion.Location = new System.Drawing.Point(0, 0);
            this.ShortcutSectionCenterRegion.Name = "ShortcutSectionCenterRegion";
            this.ShortcutSectionCenterRegion.Size = new System.Drawing.Size(78, 26);
            this.ShortcutSectionCenterRegion.TabIndex = 14;
            // 
            // ShortcutSectionLeftRegion
            // 
            this.ShortcutSectionLeftRegion.Dock = System.Windows.Forms.DockStyle.Left;
            this.ShortcutSectionLeftRegion.Location = new System.Drawing.Point(0, 0);
            this.ShortcutSectionLeftRegion.Name = "ShortcutSectionLeftRegion";
            this.ShortcutSectionLeftRegion.Size = new System.Drawing.Size(81, 26);
            this.ShortcutSectionLeftRegion.TabIndex = 13;
            // 
            // MainSection
            // 
            this.MainSection.BorderColor = System.Drawing.Color.Empty;
            this.MainSection.BorderWidthOnBottom = 0;
            this.MainSection.Controls.Add(this.MainSectionMainDivision);
            this.MainSection.Controls.Add(this.MainSectionRightDivision);
            this.MainSection.Controls.Add(this.MainSectionHorizontalResizeDivision);
            this.MainSection.Controls.Add(this.MainSectionSplitter);
            this.MainSection.Controls.Add(this.MainSectionRightNavDivision);
            this.MainSection.Controls.Add(this.MainSectionLeftNavDivision);
            this.MainSection.Dock = System.Windows.Forms.DockStyle.Top;
            this.MainSection.Location = new System.Drawing.Point(0, 139);
            this.MainSection.Name = "MainSection";
            this.MainSection.Radius = 4;
            this.MainSection.Size = new System.Drawing.Size(916, 367);
            this.MainSection.StyleType = Ligg.Winform.Controls.ContainerPanel.ContainerPanelStyle.None;
            this.MainSection.TabIndex = 12;
            // 
            // MainSectionMainDivision
            // 
            this.MainSectionMainDivision.BackColor = System.Drawing.Color.White;
            this.MainSectionMainDivision.Controls.Add(this.MainSectionMainDivisionDownRegion);
            this.MainSectionMainDivision.Controls.Add(this.MainSectionMainDivisionMidRegion);
            this.MainSectionMainDivision.Controls.Add(this.MainSectionMainDivisionUpRegion);
            this.MainSectionMainDivision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSectionMainDivision.Location = new System.Drawing.Point(258, 0);
            this.MainSectionMainDivision.Name = "MainSectionMainDivision";
            this.MainSectionMainDivision.Size = new System.Drawing.Size(531, 367);
            this.MainSectionMainDivision.TabIndex = 13;
            // 
            // MainSectionMainDivisionDownRegion
            // 
            this.MainSectionMainDivisionDownRegion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.MainSectionMainDivisionDownRegion.Location = new System.Drawing.Point(0, 319);
            this.MainSectionMainDivisionDownRegion.Name = "MainSectionMainDivisionDownRegion";
            this.MainSectionMainDivisionDownRegion.Size = new System.Drawing.Size(531, 48);
            this.MainSectionMainDivisionDownRegion.TabIndex = 2;
            // 
            // MainSectionMainDivisionMidRegion
            // 
            this.MainSectionMainDivisionMidRegion.BackColor = System.Drawing.Color.White;
            this.MainSectionMainDivisionMidRegion.Dock = System.Windows.Forms.DockStyle.Top;
            this.MainSectionMainDivisionMidRegion.Location = new System.Drawing.Point(0, 28);
            this.MainSectionMainDivisionMidRegion.Name = "MainSectionMainDivisionMidRegion";
            this.MainSectionMainDivisionMidRegion.Size = new System.Drawing.Size(531, 145);
            this.MainSectionMainDivisionMidRegion.TabIndex = 1;
            // 
            // MainSectionMainDivisionUpRegion
            // 
            this.MainSectionMainDivisionUpRegion.BackColor = System.Drawing.Color.White;
            this.MainSectionMainDivisionUpRegion.Dock = System.Windows.Forms.DockStyle.Top;
            this.MainSectionMainDivisionUpRegion.Location = new System.Drawing.Point(0, 0);
            this.MainSectionMainDivisionUpRegion.Name = "MainSectionMainDivisionUpRegion";
            this.MainSectionMainDivisionUpRegion.Size = new System.Drawing.Size(531, 28);
            this.MainSectionMainDivisionUpRegion.TabIndex = 0;
            // 
            // MainSectionRightDivision
            // 
            this.MainSectionRightDivision.BackColor = System.Drawing.SystemColors.Window;
            this.MainSectionRightDivision.Controls.Add(this.MainSectionRightDivisionDownRegion);
            this.MainSectionRightDivision.Controls.Add(this.MainSectionRightDivisionMidRegion);
            this.MainSectionRightDivision.Controls.Add(this.MainSectionRightDivisionUpRegion);
            this.MainSectionRightDivision.Dock = System.Windows.Forms.DockStyle.Right;
            this.MainSectionRightDivision.Location = new System.Drawing.Point(789, 0);
            this.MainSectionRightDivision.Name = "MainSectionRightDivision";
            this.MainSectionRightDivision.Size = new System.Drawing.Size(127, 367);
            this.MainSectionRightDivision.TabIndex = 12;
            // 
            // MainSectionRightDivisionDownRegion
            // 
            this.MainSectionRightDivisionDownRegion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.MainSectionRightDivisionDownRegion.Location = new System.Drawing.Point(0, 332);
            this.MainSectionRightDivisionDownRegion.Name = "MainSectionRightDivisionDownRegion";
            this.MainSectionRightDivisionDownRegion.Size = new System.Drawing.Size(127, 35);
            this.MainSectionRightDivisionDownRegion.TabIndex = 2;
            // 
            // MainSectionRightDivisionMidRegion
            // 
            this.MainSectionRightDivisionMidRegion.BackColor = System.Drawing.SystemColors.Window;
            this.MainSectionRightDivisionMidRegion.Dock = System.Windows.Forms.DockStyle.Top;
            this.MainSectionRightDivisionMidRegion.Location = new System.Drawing.Point(0, 28);
            this.MainSectionRightDivisionMidRegion.Name = "MainSectionRightDivisionMidRegion";
            this.MainSectionRightDivisionMidRegion.Size = new System.Drawing.Size(127, 158);
            this.MainSectionRightDivisionMidRegion.TabIndex = 1;
            // 
            // MainSectionRightDivisionUpRegion
            // 
            this.MainSectionRightDivisionUpRegion.Dock = System.Windows.Forms.DockStyle.Top;
            this.MainSectionRightDivisionUpRegion.Location = new System.Drawing.Point(0, 0);
            this.MainSectionRightDivisionUpRegion.Name = "MainSectionRightDivisionUpRegion";
            this.MainSectionRightDivisionUpRegion.Size = new System.Drawing.Size(127, 28);
            this.MainSectionRightDivisionUpRegion.TabIndex = 0;
            // 
            // MainSectionHorizontalResizeDivision
            // 
            this.MainSectionHorizontalResizeDivision.BackColor = System.Drawing.SystemColors.Window;
            this.MainSectionHorizontalResizeDivision.Controls.Add(this.HorizontalResizeButton);
            this.MainSectionHorizontalResizeDivision.Dock = System.Windows.Forms.DockStyle.Left;
            this.MainSectionHorizontalResizeDivision.Location = new System.Drawing.Point(252, 0);
            this.MainSectionHorizontalResizeDivision.Name = "MainSectionHorizontalResizeDivision";
            this.MainSectionHorizontalResizeDivision.Size = new System.Drawing.Size(6, 367);
            this.MainSectionHorizontalResizeDivision.TabIndex = 11;
            // 
            // HorizontalResizeButton
            // 
            this.HorizontalResizeButton.BackColor = System.Drawing.Color.Transparent;
            this.HorizontalResizeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.HorizontalResizeButton.Location = new System.Drawing.Point(0, 16);
            this.HorizontalResizeButton.Name = "HorizontalResizeButton";
            this.HorizontalResizeButton.Size = new System.Drawing.Size(6, 50);
            this.HorizontalResizeButton.TabIndex = 7;
            this.HorizontalResizeButton.TabStop = false;
            this.HorizontalResizeButton.Click += new System.EventHandler(this.HorizontalResizeButton_Click);
            // 
            // MainSectionSplitter
            // 
            this.MainSectionSplitter.BackColor = System.Drawing.SystemColors.Window;
            this.MainSectionSplitter.Location = new System.Drawing.Point(251, 0);
            this.MainSectionSplitter.Name = "MainSectionSplitter";
            this.MainSectionSplitter.Size = new System.Drawing.Size(1, 367);
            this.MainSectionSplitter.TabIndex = 6;
            this.MainSectionSplitter.TabStop = false;
            // 
            // MainSectionRightNavDivision
            // 
            this.MainSectionRightNavDivision.BackColor = System.Drawing.SystemColors.Window;
            this.MainSectionRightNavDivision.Controls.Add(this.MainSectionRightNavDivisionDownRegion);
            this.MainSectionRightNavDivision.Controls.Add(this.MainSectionRightNavDivisionMidRegion);
            this.MainSectionRightNavDivision.Controls.Add(this.MainSectionRightNavDivisionUpRegion);
            this.MainSectionRightNavDivision.Dock = System.Windows.Forms.DockStyle.Left;
            this.MainSectionRightNavDivision.Location = new System.Drawing.Point(113, 0);
            this.MainSectionRightNavDivision.Name = "MainSectionRightNavDivision";
            this.MainSectionRightNavDivision.Size = new System.Drawing.Size(138, 367);
            this.MainSectionRightNavDivision.TabIndex = 5;
            // 
            // MainSectionRightNavDivisionDownRegion
            // 
            this.MainSectionRightNavDivisionDownRegion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.MainSectionRightNavDivisionDownRegion.Location = new System.Drawing.Point(0, 332);
            this.MainSectionRightNavDivisionDownRegion.Name = "MainSectionRightNavDivisionDownRegion";
            this.MainSectionRightNavDivisionDownRegion.Size = new System.Drawing.Size(138, 35);
            this.MainSectionRightNavDivisionDownRegion.TabIndex = 5;
            // 
            // MainSectionRightNavDivisionMidRegion
            // 
            this.MainSectionRightNavDivisionMidRegion.Dock = System.Windows.Forms.DockStyle.Top;
            this.MainSectionRightNavDivisionMidRegion.Location = new System.Drawing.Point(0, 28);
            this.MainSectionRightNavDivisionMidRegion.Name = "MainSectionRightNavDivisionMidRegion";
            this.MainSectionRightNavDivisionMidRegion.Size = new System.Drawing.Size(138, 84);
            this.MainSectionRightNavDivisionMidRegion.TabIndex = 4;
            // 
            // MainSectionRightNavDivisionUpRegion
            // 
            this.MainSectionRightNavDivisionUpRegion.Dock = System.Windows.Forms.DockStyle.Top;
            this.MainSectionRightNavDivisionUpRegion.Location = new System.Drawing.Point(0, 0);
            this.MainSectionRightNavDivisionUpRegion.Name = "MainSectionRightNavDivisionUpRegion";
            this.MainSectionRightNavDivisionUpRegion.Size = new System.Drawing.Size(138, 28);
            this.MainSectionRightNavDivisionUpRegion.TabIndex = 3;
            // 
            // MainSectionLeftNavDivision
            // 
            this.MainSectionLeftNavDivision.BackColor = System.Drawing.SystemColors.Window;
            this.MainSectionLeftNavDivision.Controls.Add(this.MainSectionLeftNavDivisionDownRegion);
            this.MainSectionLeftNavDivision.Controls.Add(this.MainSectionLeftNavDivisionMidRegion);
            this.MainSectionLeftNavDivision.Controls.Add(this.MainSectionLeftNavDivisionUpRegion);
            this.MainSectionLeftNavDivision.Dock = System.Windows.Forms.DockStyle.Left;
            this.MainSectionLeftNavDivision.Location = new System.Drawing.Point(0, 0);
            this.MainSectionLeftNavDivision.Name = "MainSectionLeftNavDivision";
            this.MainSectionLeftNavDivision.Size = new System.Drawing.Size(113, 367);
            this.MainSectionLeftNavDivision.TabIndex = 2;
            // 
            // MainSectionLeftNavDivisionDownRegion
            // 
            this.MainSectionLeftNavDivisionDownRegion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.MainSectionLeftNavDivisionDownRegion.Location = new System.Drawing.Point(0, 335);
            this.MainSectionLeftNavDivisionDownRegion.Name = "MainSectionLeftNavDivisionDownRegion";
            this.MainSectionLeftNavDivisionDownRegion.Size = new System.Drawing.Size(113, 32);
            this.MainSectionLeftNavDivisionDownRegion.TabIndex = 2;
            // 
            // MainSectionLeftNavDivisionMidRegion
            // 
            this.MainSectionLeftNavDivisionMidRegion.Dock = System.Windows.Forms.DockStyle.Top;
            this.MainSectionLeftNavDivisionMidRegion.Location = new System.Drawing.Point(0, 28);
            this.MainSectionLeftNavDivisionMidRegion.Name = "MainSectionLeftNavDivisionMidRegion";
            this.MainSectionLeftNavDivisionMidRegion.Size = new System.Drawing.Size(113, 84);
            this.MainSectionLeftNavDivisionMidRegion.TabIndex = 1;
            // 
            // MainSectionLeftNavDivisionUpRegion
            // 
            this.MainSectionLeftNavDivisionUpRegion.Dock = System.Windows.Forms.DockStyle.Top;
            this.MainSectionLeftNavDivisionUpRegion.Location = new System.Drawing.Point(0, 0);
            this.MainSectionLeftNavDivisionUpRegion.Name = "MainSectionLeftNavDivisionUpRegion";
            this.MainSectionLeftNavDivisionUpRegion.Size = new System.Drawing.Size(113, 28);
            this.MainSectionLeftNavDivisionUpRegion.TabIndex = 0;
            // 
            // PictureList
            // 
            this.PictureList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("PictureList.ImageStream")));
            this.PictureList.TransparentColor = System.Drawing.Color.Transparent;
            this.PictureList.Images.SetKeyName(0, "go_left_old.png");
            this.PictureList.Images.SetKeyName(1, "go_right_old.png");
            this.PictureList.Images.SetKeyName(2, "go_left.png");
            this.PictureList.Images.SetKeyName(3, "go_right.png");
            // 
            // FrameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 620);
            this.Name = "FrameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Frame Form";
            this.RunningMessageSection.ResumeLayout(false);
            this.RunningStatusSection.ResumeLayout(false);
            this.RunningStatusSectionBackgroundTaskRegion.ResumeLayout(false);
            this.GroundPanel.ResumeLayout(false);
            this.MainMenuSection.ResumeLayout(false);
            this.ToolBarSection.ResumeLayout(false);
            this.ToolBarSectionPublicRegion.ResumeLayout(false);
            this.NavigationSection.ResumeLayout(false);
            this.ShortcutSection.ResumeLayout(false);
            this.ShortcutSectionRightRegion.ResumeLayout(false);
            this.MainSection.ResumeLayout(false);
            this.MainSectionMainDivision.ResumeLayout(false);
            this.MainSectionRightDivision.ResumeLayout(false);
            this.MainSectionHorizontalResizeDivision.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.HorizontalResizeButton)).EndInit();
            this.MainSectionRightNavDivision.ResumeLayout(false);
            this.MainSectionLeftNavDivision.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        protected ContainerPanel MainMenuSection;
        protected ContainerPanel ToolBarSection;
        protected ContainerPanel NavigationSection;
        protected ContainerPanel ShortcutSection;
        protected ContainerPanel MainSection;

        protected System.Windows.Forms.Panel ShortcutSectionLeftRegion;
        protected System.Windows.Forms.Panel NavigationSectionLeftRegion;
        protected System.Windows.Forms.Panel NavigationSectionRightRegion;

        protected System.Windows.Forms.Panel ShortcutSectionRightRegion;
        protected System.Windows.Forms.Panel MainSectionLeftNavDivision;
        protected System.Windows.Forms.Panel MainSectionRightNavDivision;
        protected System.Windows.Forms.Panel MainSectionLeftNavDivisionMidRegion;
        protected System.Windows.Forms.Panel MainSectionLeftNavDivisionUpRegion;
        protected System.Windows.Forms.Panel MainSectionRightNavDivisionMidRegion;
        protected System.Windows.Forms.Panel MainSectionRightNavDivisionUpRegion;
        protected System.Windows.Forms.Splitter MainSectionSplitter;
        protected System.Windows.Forms.ImageList PictureList;
        protected Panel ToolBarSectionPublicRegion;
        protected System.Windows.Forms.ToolStrip ToolBarSectionPublicRegionToolStrip;
        protected System.Windows.Forms.Panel MainSectionHorizontalResizeDivision;
        protected System.Windows.Forms.PictureBox HorizontalResizeButton;
        protected System.Windows.Forms.Panel MainSectionRightDivision;
        protected System.Windows.Forms.Panel MainSectionMainDivision;
        protected System.Windows.Forms.Panel MainSectionMainDivisionUpRegion;
        protected System.Windows.Forms.Panel ToolBarSectionLeftRegion;
        protected System.Windows.Forms.Panel MainSectionLeftNavDivisionDownRegion;
        protected System.Windows.Forms.Panel MainSectionRightNavDivisionDownRegion;
        protected System.Windows.Forms.Panel MainSectionRightDivisionDownRegion;
        protected System.Windows.Forms.Panel MainSectionRightDivisionMidRegion;
        protected System.Windows.Forms.Panel MainSectionRightDivisionUpRegion;
        protected System.Windows.Forms.Panel MainSectionMainDivisionDownRegion;
        protected System.Windows.Forms.Panel MainSectionMainDivisionMidRegion;
        protected Panel MainMenuSectionRightRegion;
        protected Panel MainMenuSectionCenterRegion;
        protected Panel MainMenuSectionLeftRegion;
        protected Panel ShortcutSectionCenterRegion;
        protected Panel NavigationSectionCenterRegion;
        protected Panel ToolBarSectionRightRegion;
        protected Panel ToolBarSectionCenterRegion;
    }
}