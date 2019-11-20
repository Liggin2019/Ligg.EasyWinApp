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
using Ligg.Winform.DataModel;
using Ligg.Winform.DataModel.Enums;
using Ligg.Winform.Helpers;
using Ligg.Winform.Resources;

namespace Ligg.Winform.Forms
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
            RunningMessageSection.BackColor = StyleSet.GroundColor;
            RunningMessageSection.StyleType = Ligg.Winform.Controls.ContainerPanel.ContainerPanelStyle.Borders;
            RunningMessageSection.RoundStyle = RoundStyle.None;
            RunningMessageSection.Radius = 0;
            RunningMessageSection.BorderWidthOnLeft = 0;
            RunningMessageSection.BorderWidthOnTop = 1;
            RunningMessageSection.BorderWidthOnRight = 0;
            RunningMessageSection.BorderWidthOnBottom = 0;
            RunningMessageSection.BorderColor = StyleSet.ControlBorderColor;
            RunningMessageSection.Padding = new Padding(2);

            RunningStatusSection.BackColor = StyleSet.RunningStatusSectionBackColor;
            RunningStatusSection.StyleType = Ligg.Winform.Controls.ContainerPanel.ContainerPanelStyle.Borders;
            RunningStatusSection.RoundStyle = RoundStyle.None;
            RunningStatusSection.Radius = 0;
            RunningStatusSection.BorderWidthOnLeft = 0;
            RunningStatusSection.BorderWidthOnTop = 1;
            RunningStatusSection.BorderWidthOnRight = 0;
            RunningStatusSection.BorderWidthOnBottom = 0;
            RunningStatusSection.BorderColor = StyleSet.ControlBorderColor;

            RunningStatusSectionMsgRegionLabelMsg.Text = WinformRes.Ready;
            RunningStatusSectionMsgRegionLabelMsg2.Text = "";
            RunningStatusSectionMsgRegionLabelMsg3.Text = "";


            RunningStatusSectionBackgroundTaskRegion.Visible = false;
            RunningStatusSectionBackgroundTaskRegionProgressCircleZone.Width = 0;
            RunningStatusSectionBackgroundTaskRegionProgressCircle.BaseColor = StyleSet.BaseColor;
            RunningStatusSectionBackgroundTaskRegionProgressCircle.Location = new System.Drawing.Point(1, 1);
            RunningStatusSectionBackgroundTaskRegionProgressCircle.RingThickness = 1;
            RunningStatusSectionBackgroundTaskRegionProgressCircle.Size = new System.Drawing.Size(BackgroundTaskProgressCircleSize, BackgroundTaskProgressCircleSize);
            RunningStatusSectionBackgroundTaskRegionProgressCircle.SpokeNumber = 10;
            RunningStatusSectionBackgroundTaskRegionProgressCircle.SpokeThickness = 3;
            RunningStatusSectionBackgroundTaskRegionProgressCircle.Stop();
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
            if (type == 1) WriteRunningMessage("# " + message, StyleSet.ColorCommand, true);
            else if (type == 2) WriteRunningMessage("# " + message, StyleSet.ColorSucceeded, true);
            else if (type == 3) WriteRunningMessage("# " + message, StyleSet.ColorError, true);
            else WriteRunningMessage(message, StyleSet.ColorDefault, true);
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

        protected void ShowRunningStatusMsg2(string msg2)
        {
            RunningStatusSectionMsgRegionLabelMsg2.Text = msg2;
            RunningStatusSectionMsgRegion.Refresh();
        }

        protected void EraseRunningStatusMsg2()
        {
            RunningStatusSectionMsgRegionLabelMsg2.Text = "";
            RunningStatusSectionMsgRegion.Refresh();
        }

        //#InitBackgroundTaskComponent
        protected void InitBackgroundTaskComponent(int popupWidth, int popupHeight, int rightMargin, string imageTextButtonText, string labelText, List<IdName> typeList, string typeAnnexesXmlPath, string listViewExHeaderConfigXmlPath, string listViewExCttMenuConfigXmlPath)
        {
            try
            {
                RunningStatusSectionBackgroundTaskRegion.Visible = true;
                if (!imageTextButtonText.IsNullOrEmpty())
                {
                    BackgroundTaskImageTextButtonText = imageTextButtonText;
                    RunningStatusSectionBackgroundTaskRegionImageTextButton.Text = BackgroundTaskImageTextButtonText + @" 0/0";
                }
                if (!labelText.IsNullOrEmpty())
                {
                    BackgroundTaskInfoLabelText = labelText;
                    BackgroundTaskDetailLabel.Text = BackgroundTaskInfoLabelText;

                }

                BackgroundTaskDetailContainer.Width = popupWidth + 9;
                BackgroundTaskDetailContainer.Height = popupHeight + 9;
                var shadowPanelCtrl = ControlHelper.GetControlByNameByContainer(BackgroundTaskDetailContainer, "shadowPanel");
                BackgroundTaskDetailPanel.Visible = true;
                BackgroundTaskDetailPanel.Width = popupWidth;
                BackgroundTaskDetailPanel.Height = popupHeight;
                BackgroundTaskDetailPanel.Location = new Point(3, 3);
                shadowPanelCtrl.Controls.Add(BackgroundTaskDetailPanel);
                BackgroundTaskDetailLabel.Visible = true;
                BackgroundTaskDetailLabel.BackColor = StyleSet.BaseColor;
                BackgroundTaskDetailLabel.ForeColor = StyleSet.CaptionTextColor;

                if (typeList != null) _backgroundTaskTypeList = typeList;
                if (File.Exists(typeAnnexesXmlPath))
                {
                    var annexesXmlMgr = new XmlHandler(typeAnnexesXmlPath);
                    _backgroundTaskTypeAnnexes = annexesXmlMgr.ConvertToObject<List<Annex>>();
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + "." + "InitBackgroundTaskComponent Error: " + ex.Message);
            }
        }


        protected void RefreshBackgroundTaskComponent(List<BackgroundTaskPool.BackgroundTask> backTasks)
        {
            try
            {
                var backTasksProcessingNo = backTasks.FindAll(x => x.Status == (int)TaskStatus.Processing).Count;
                var backTasksWaitingNo = backTasks.FindAll(x => x.Status == (int)TaskStatus.Waiting).Count;
                RunningStatusSectionBackgroundTaskRegionImageTextButton.Text = BackgroundTaskImageTextButtonText + @" " + backTasksProcessingNo + @"/" + backTasksWaitingNo;
                if (backTasksProcessingNo > 0)
                {
                    RunningStatusSectionBackgroundTaskRegionProgressCircleZone.Width = BackgroundTaskProgressCircleSize;
                    RunningStatusSectionBackgroundTaskRegionProgressCircle.Visible = true;
                    RunningStatusSectionBackgroundTaskRegionProgressCircle.Width = BackgroundTaskProgressCircleSize;
                    RunningStatusSectionBackgroundTaskRegionProgressCircle.Start();
                }
                else
                {
                    RunningStatusSectionBackgroundTaskRegionProgressCircle.Visible = false;
                    RunningStatusSectionBackgroundTaskRegionProgressCircle.Stop();
                    RunningStatusSectionBackgroundTaskRegionProgressCircleZone.Width = 0;
                }
                BackgroundTaskDetailLabel.Text = BackgroundTaskInfoLabelText;
                var idValTextList = new List<IdValueText>();
                foreach (var task in backTasks)
                {
                    var typeIdName = _backgroundTaskTypeList.Find(x => x.Id == task.Type);
                    if (typeIdName != null)
                    {
                        task.TypeName = AnnexHelper.GetText("", typeIdName.Name, _backgroundTaskTypeAnnexes, AnnexTextType.DisplayName, CultureHelper.CurrentLanguageCode, GetAnnexMode.StepByStep);
                    }
                    else
                    {
                        task.TypeName = WinformRes.Unknown;
                    }

                    Type t = task.GetType();
                    var myFields = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    for (int i = 0; i < myFields.Length; i++)
                    {
                        var idValueText = new IdValueText();
                        idValueText.Id = task.Id;
                        idValueText.Value = myFields[i].Name;
                        var obj = myFields[i].GetValue(task);
                        idValueText.Text = obj == null ? "" : obj.ToString();
                        idValTextList.Add(idValueText);
                    }
                }
                //BackgroundTaskDetailListViewEx.Render(idValTextList);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + "." + "RefreshBackgroundTaskComponent Error: " + ex.Message);
            }
        }



    }
}
