using System;
using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Forms;
using Ligg.Base.Extension;
using Ligg.Winform.DataModel.Enums;
using Ligg.Winform.Forms;
using Ligg.Winform.Helpers;
using Ligg.Winform.Resources;

namespace Ligg.Winform.Dialogs
{
    public partial class DateTimeInputDialog : GroundForm
    {
        public DateTimeInputDialog()
        {
            InitializeComponent();
            textButtonOk.Text = WinformRes.Ok;
            textButtonOk.BackColor = StyleSet.ControlBackColor;
            textButtonOk.HasBorder = true;
            textButtonOk.SensitiveType = ControlSensitiveType.Check;
            textButtonOk.Checked = true;
            Text = WinformRes.PlsInputDateTime;
            textButtonCancel.Text = WinformRes.Cancel;
            textButtonCancel.BackColor = StyleSet.ControlBackColor;
            textButtonCancel.HasBorder = true;
            textButtonCancel.SensitiveType = ControlSensitiveType.Check;

            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;

            if (Owner != null)
            {
                Left = Owner.Location.X + (Owner.Width / 2 - Width / 2);
            }
            else
            {
                var rect = new Rectangle();
                rect = Screen.GetWorkingArea(this);
                Left = rect.Width / 2 - Width / 2;
                Top = rect.Height > Height ? (rect.Height / 2 - Height / 2) / 3 : 10;
            }
        }


        public string CustomFormat { get; set; }
        public string VerificationRule { get; set; }
        public string VerificationParams { get; set; }
        public string InputText { get; set; }
        public DateTime InputDateTime { get; set; }
        public bool IsOk { get; set; }

        private void DateTimeInputDialog_Load(object sender, EventArgs e)
        {
            if (!CustomFormat.IsNullOrEmpty()) dateTimePicker.CustomFormat = CustomFormat;
            else dateTimePicker.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            if (dateTimePicker.CustomFormat == "yyyy-MM")
            {
                this.dateTimePicker.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day);
            }
        }

        private void textButtonOk_Click(object sender, EventArgs e)
        {
            try
            {
                IsOk = false;
                var dt = dateTimePicker.Value;
                if (!VerificationRule.IsNullOrEmpty())
                {
                    if (VerificationRule.ToLower() == "CanNotBeFutureTime".ToLower())
                    {
                        if (dt.ToUtcTime().IsFutureTime())
                        {
                            PopupMessage.Popup(WinformRes.Input + ValidationRes.CanNotBeFutureTime);
                            return;
                        }
                    }
                    else if (VerificationRule.ToLower() == "CanNotBePastTime".ToLower())
                    {
                        if (dt.ToUtcTime().IsPastTime())
                        {
                            PopupMessage.Popup(WinformRes.Input + ValidationRes.CanNotBePastTime);
                            return;
                        }
                    }
                    else if (VerificationRule.ToLower() == "CanNotBeEarlierThan".ToLower())
                    {
                        var dt1 = DateTime.ParseExact(VerificationParams, dateTimePicker.CustomFormat, System.Globalization.CultureInfo.CurrentCulture);
                        if (dt < dt1)
                        {
                            PopupMessage.Popup(string.Format(WinformRes.Input + ValidationRes.CanNotBeEarlierThan, VerificationParams));
                            return;
                        }

                    }
                    else if (VerificationRule.ToLower() == "CanNotBeLaterThan".ToLower())
                    {
                        var dt1 = DateTime.ParseExact(VerificationParams, dateTimePicker.CustomFormat, System.Globalization.CultureInfo.CurrentCulture);
                        if (dt > dt1)
                        {

                            PopupMessage.Popup(string.Format(WinformRes.Input + ValidationRes.CanNotBeLaterThan, VerificationParams));
                            return;
                        }
                    }
                    else if (VerificationRule.ToLower() == "ShouldBeBetween".ToLower())
                    {
                        var verificationParamArry = VerificationParams.Split(VerificationParams.GetSubParamSeparator());
                        var dt1 = DateTime.ParseExact(verificationParamArry[0], dateTimePicker.CustomFormat, System.Globalization.CultureInfo.CurrentCulture);
                        var dt2 = DateTime.ParseExact(verificationParamArry[1], dateTimePicker.CustomFormat, System.Globalization.CultureInfo.CurrentCulture);
                        if (dt1 > dt2)
                        {
                            PopupMessage.Popup(WinformRes.VerificationParamsHaveProblem);
                            Close();
                        }
                        
                        if (!(dt > dt1 & dt < dt2))
                        {
                            PopupMessage.Popup(string.Format(WinformRes.Input + ValidationRes.ShouldBeBetween, verificationParamArry[0], verificationParamArry[1]));
                            return;
                        }
                    }
                }

                InputText = dateTimePicker.Text;
                InputDateTime = dateTimePicker.Value;
                IsOk = true;
                Close();
            }
            catch (Exception ex)
            {
                PopupMessage.Popup(ex.Message);
            }
        }

        private void Dialog_KeyPress(object sender, KeyPressEventArgs e)
        {
            //'enter'
            if (e.KeyChar == '\r')
            {
                textButtonOk_Click(null, null);
            }

            //'esc'
            if ((int)(e.KeyChar) == Convert.ToChar(Keys.Escape))
            {
                IsOk = false;
                this.Close();
            }
        }

        private void textButtonCancel_Click(object sender, EventArgs e)
        {
            IsOk = false;
            this.Close();
        }


    }
}
