using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Ligg.Base.DataModel;
using Ligg.Base.DataModel.Enums;
using Ligg.Base.Extension;
using Ligg.Base.Handlers;
using Ligg.Base.Helpers;
using Ligg.WinForm.DataModel;
using Ligg.WinForm.DataModel.Enums;
using Ligg.WinForm.Helpers;
using Ligg.WinForm.Resources;

namespace Ligg.WinForm.Forms
{
    public partial class BaseForm : GroundForm
    {
        protected int RunningMessageSectionHeight = 49;
        protected int RunningStatusSectionHeight = 21;
        protected int RunningStatusSectionBackgroundTaskRegionWidth = 0;
        private const int BackgroundTaskProgressCircleSize = 21;

        private List<IdName> _backgroundTaskTypeList = new List<IdName>();
        private List<Annex> _backgroundTaskTypeAnnexes = new List<Annex>();
        protected string BackgroundTaskImageTextButtonText = WinformRes.TaskList;
        protected string BackgroundTaskInfoLabelText = WinformRes.TaskList;

        protected BaseForm()
        {
            InitializeComponent();
        }

        //#method
        private void BaseForm_Load(object sender, EventArgs e)
        {
            InitBaseComponent();
        }

        protected void RunningStatusSectionBackTaskRegionImageTextButton_Click(object sender, EventArgs e)
        {
            var baseCtrl = RunningStatusSectionBackgroundTaskRegion;
            var p = baseCtrl.PointToClient(new Point(0, 0));
            var p1 = GroundPanel.PointToClient(new Point(0, 0));
            var pos = new Point(p1.X - p.X, p1.Y - p.Y);
            pos.X = pos.X - BackgroundTaskDetailContainer.Width + RunningStatusSectionBackgroundTaskRegionWidth - 5;
            pos.Y = pos.Y - BackgroundTaskDetailContainer.Height - 5;
            BackgroundTaskDetailContainer.Location = pos;
            BackgroundTaskDetailContainer.Visible = true;
            BackgroundTaskDetailContainer.BringToFront();
        }

        //#proc
        protected void InitBaseComponent()
        {
            RunningMessageSection.BackColor = StyleSheet.GroundColor;
            RunningMessageSection.StyleType = Ligg.WinForm.Controls.ContainerPanel.ContainerPanelStyle.Borders;
            RunningMessageSection.RoundStyle = RoundStyle.None;
            RunningMessageSection.Radius = 0;
            RunningMessageSection.BorderWidthOnLeft = 0;
            RunningMessageSection.BorderWidthOnTop = 1;
            RunningMessageSection.BorderWidthOnRight = 0;
            RunningMessageSection.BorderWidthOnBottom = 0;
            RunningMessageSection.BorderColor = StyleSheet.ControlBorderColor;
            RunningMessageSection.Padding = new Padding(2);

            RunningStatusSection.BackColor = StyleSheet.RunningStatusSectionBackColor;
            RunningStatusSection.StyleType = Ligg.WinForm.Controls.ContainerPanel.ContainerPanelStyle.Borders;
            RunningStatusSection.RoundStyle = RoundStyle.None;
            RunningStatusSection.Radius = 0;
            RunningStatusSection.BorderWidthOnLeft = 0;
            RunningStatusSection.BorderWidthOnTop = 1;
            RunningStatusSection.BorderWidthOnRight = 0;
            RunningStatusSection.BorderWidthOnBottom = 0;
            RunningStatusSection.BorderColor = StyleSheet.ControlBorderColor;

            RunningStatusSectionMsgRegionLabelMsg.Text = WinformRes.Ready;
            RunningStatusSectionMsgRegionLabelMsg2.Text = "";
            RunningStatusSectionMsgRegionLabelMsg3.Text = "";


            RunningStatusSectionBackgroundTaskRegion.Visible = false;
            RunningStatusSectionBackgroundTaskRegionProgressCircleZone.Width = 0;
            //RunningStatusSectionBackgroundTaskRegionProgressCircle.BaseColor = StyleSet.BaseColor;
            //RunningStatusSectionBackgroundTaskRegionProgressCircle.Location = new System.Drawing.Point(1, 1);
            //RunningStatusSectionBackgroundTaskRegionProgressCircle.RingThickness = 1;
            //RunningStatusSectionBackgroundTaskRegionProgressCircle.Size = new System.Drawing.Size(BackgroundTaskProgressCircleSize, BackgroundTaskProgressCircleSize);
            //RunningStatusSectionBackgroundTaskRegionProgressCircle.SpokeNumber = 10;
            //RunningStatusSectionBackgroundTaskRegionProgressCircle.SpokeThickness = 3;
            //RunningStatusSectionBackgroundTaskRegionProgressCircle.Stop();
            RunningStatusSectionBackgroundTaskRegionImageTextButton.Text = BackgroundTaskImageTextButtonText + @" 0/0";
            BackgroundTaskDetailLabel.Text = BackgroundTaskInfoLabelText;
        }

        protected void ResizeBaseComponent()
        {
            RunningMessageSection.Height = RunningMessageSectionHeight;
            RunningMessageSectionRichTextBox.Height = RunningMessageSectionHeight - 3;

            RunningStatusSection.Height = RunningStatusSectionHeight;
            RunningStatusSectionBackgroundTaskRegion.Width = RunningStatusSectionBackgroundTaskRegionWidth;

            RunningStatusSectionMsgRegion.Width = RunningMessageSection.Width - RunningStatusSectionBackgroundTaskRegion.Width - 5;
            RunningStatusSectionMsgRegionMsgZone.Width = RunningStatusSectionMsgRegion.Width;
        }

        protected void ResetBaseTextByCulture()
        {
            RunningStatusSectionMsgRegionLabelMsg.Text = WinformRes.Ready;
        }

        //#RuningdMessage
        //1.command, 2.suceeded, 3.failed
        protected void WriteRunningdMessage(int type, string message)
        {
            if (type == 1) WriteRunningMessage("# " + message, StyleSheet.ColorCommand, true);
            else if (type == 2) WriteRunningMessage("# " + message, StyleSheet.ColorSucceeded, true);
            else if (type == 3) WriteRunningMessage("# " + message, StyleSheet.ColorError, true);
            else WriteRunningMessage(message, StyleSheet.ColorDefault, true);
        }

        protected void WriteRunningMessage(string message, Color color, bool isNewLine)
        {
            RunningMessageSectionRichTextBox.Focus();
            RunningMessageSectionRichTextBox.AppendText("");
            RunningMessageSectionRichTextBox.SelectionColor = color;
            RunningMessageSectionRichTextBox.AppendText(message + (isNewLine ? "\r\n" : null));
        }

        //#RunningStatus
        protected void InitRunningStatusMessageComponent()
        {
            RunningStatusSectionMsgRegionLabelMsg.Text = WinformRes.Ready;
            RunningStatusSectionMsgRegionLabelMsg1.Text = "";
            RunningStatusSectionMsgRegionLabelMsg2.Text = "";
            RunningStatusSectionMsgRegionLabelMsg3.Text = "";
            RunningStatusSectionMsgRegion.Refresh();
        }

        protected void RefreshRunningStatusMessageComponent(string msg, string msg1, string msg2, string msg3)
        {
            RunningStatusSectionMsgRegionLabelMsg.Text = msg;
            RunningStatusSectionMsgRegionLabelMsg1.Text = msg1;
            RunningStatusSectionMsgRegionLabelMsg2.Text = msg2;
            RunningStatusSectionMsgRegionLabelMsg3.Text = msg3;
            RunningStatusSectionMsgRegion.Refresh();
        }



    }
}
