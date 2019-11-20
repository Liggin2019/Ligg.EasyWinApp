using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Ligg.Base.DataModel;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
using Ligg.Winform.Helpers;
using Ligg.Winform.Resources;

namespace Ligg.Winform.Dialogs
{
    public partial class RunningErrorViewer
    {
        public string AdditionalInfo { get; set; }
        public string ErrorText { get; set; }
        public string ExceptionMsg { get; set; }
        public string OccurringTime { get; set; }

        public int FormWidth;
        private double _hwRatio;

        public RunningErrorViewer()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.ShowIcon = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            if (Owner != null)
            {
                Left = Owner.Location.X + (Owner.Width / 2 - Width / 2);
            }
            else
            {
                var rect = new Rectangle();
                rect = Screen.GetWorkingArea(this);
                Left = rect.Width / 2 - Width / 2;
            }

            imageTextButtonSendErrorInfo.Text = WinformRes.SendErrorInfoToDev;
            GroundPanel.BackColor = StyleSet.NavigationSectionBackColor;
        }


        private void RunningErrorViewer_Load(object sender, EventArgs e)
        {

            
            labelError.Text = WinformRes.ErrorMsg+":";
            richTextBoxError.Text = ErrorText + ExceptionMsg;
            richTextBoxError.Height = Height - panelTop.Height - panelBottom.Height - 28;
            OccurringTime = SystemTimeHelper.UtcNow().ToString("yy-MM-dd HH:mm");
        }

        private void richTextBoxError_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FormWidth = Width + 50;
            ResizeComponent();
        }

        private void imageTextButtonSendErrorInfo_Click(object sender, EventArgs e)
        {
            
            var helpdeskMail = AdditionalInfo.SplitByTwoDifferentStrings("HelpdeskEmail:", ";", true)[0];
            var userCode = AdditionalInfo.SplitByTwoDifferentStrings("UserCode:", ";", true)[0];
            var appVersion = AdditionalInfo.SplitByTwoDifferentStrings("ApplicationVersion:", ";", true)[0];
            var valueText = new ValueText { Value = ".NET Framework", Text = "" };
            var listValueText = new List<ValueText>();
            listValueText.Add(valueText);
            var installedNetFxes = SystemInfoHelper.GetInstalledSoftwareList(listValueText);
            var installedNetFxesStr = "";
            for (int index = 0; index < installedNetFxes.Count; index++)
            {
                var installedSoftware = installedNetFxes[index];
                if (index == 0)
                {
                    installedNetFxesStr = installedSoftware.ProductName + " " +
                                      installedSoftware.VersionString;
                }
                else
                {
                    installedNetFxesStr = installedNetFxesStr + "; " + installedSoftware.ProductName + " " +
                                       installedSoftware.VersionString;
                }

            }
            var body = "Dear Developer%0D%0A"
                       + "%0D%0A"
                       + "Following is system running error infomation for you:"
                       + (userCode.IsNullOrEmpty() ? "" : " (send by " + userCode + ")") + "%0D%0A"
                        + "Application Version : " + appVersion + "%0D%0A"
                        + "Assembly Bits : " + IntPtr.Size * 8 + "%0D%0A"
                       + "OS : " + SystemInfoHelper.GetSystemInfo("osinfo") + " " + SystemInfoHelper.GetSystemInfo("osbit") + " bits" + "%0D%0A"
                       + "Machine Name: " + SystemInfoHelper.GetSystemInfo("machinename") + "%0D%0A"
                       + "IP: " + SystemInfoHelper.GetSystemInfo("Ips") + "%0D%0A"
                       + "Windows Account: " + SystemInfoHelper.GetSystemInfo("currentuser") + "%0D%0A"
                       + "Occurring Time: " + OccurringTime + "%0D%0A"
                       + "Installed dotNetFx: " + installedNetFxesStr + "%0D%0A"
                       + "Exception Message: " + ExceptionMsg;
            body = body.Replace("\n", "%0D%0A");
            var subject = ErrorText + " occured at " + OccurringTime;
            LocalEmailHelper.Send(helpdeskMail, subject, body);
           
        }

        private void ResizeComponent()
        {
            _hwRatio = Height * 1.00 / Width;
            this.Width = FormWidth;
            this.Height = Convert.ToInt32(Width * _hwRatio);
            richTextBoxError.Height = Height - panelTop.Height - panelBottom.Height - 28;
        }

        //common




    }
}
