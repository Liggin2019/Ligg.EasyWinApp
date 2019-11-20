using System;
using System.Windows.Forms;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
using Ligg.Winform.Dialogs;
using Ligg.Winform.Resources;

namespace Ligg.Winform.Controls
{
    public partial class TimerExTimingRun : UserControl
    {
        public TimerExTimingRun()
        {
            InitializeComponent();
            textBoxStartTime.ReadOnly = true;
            textBoxLatestStartTime.ReadOnly = true;
            SetTextByCulture();
        }

        //#event
        public event EventHandler OnRunTimeUp;
        public event EventHandler OnAlarmTimeUp;

        //#property
        private ToolTip _toolTipInputStartTime = new ToolTip();
        private ToolTip _toolTipStartOrPause = new ToolTip();
        private string _customFormat = "yyyy-MM-dd HH:mm:ss";


        private DateTime _startTime;
        private string _startTimeString;
        private DateTime _latestStartTime;
        private string _latestStartTimeString;
        private bool _isAlarmOccured;

        private int _intervalSeconds = 1;
        private int _alarmLeadSeconds = 10;

        private bool _isNowTicked;

        private string _dataSource;
        public string DataSource
        {
            get => _dataSource;
            set
            {
                _dataSource = value;
                InitData();
                if (_alarmLeadSeconds < 2 * _intervalSeconds) _alarmLeadSeconds = 2 * _intervalSeconds;
            }
        }

        private bool _isUserMode;
        //private bool _startTicking;
        public bool Value
        {
            get => _isUserMode;
            set
            {
                if (!_startTimeString.IsNullOrEmpty() & value) _isUserMode = true;
            }
        }


        //#proc
        private void TimerEx_Load(object sender, EventArgs e)
        {
            try
            {
                if (!_isUserMode)
                {
                    _latestStartTimeString = string.Empty;
                }

                Render();
                SetTextByCulture();

                if (_isUserMode)
                {
                    pictureBoxStartOrPause_Click(null, null);
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + "." + "TimerEx_Load Error: " + ex.Message);
            }

        }

        private void TimerExTimingRun_Resize(object sender, EventArgs e)
        {
            labelLeftTime.Left = labelTimeLeft.Left + labelTimeLeft.Width + 1;
            pictureBoxStartOrPause.Left = Width - 16 - 5;
        }

        private void pictureBoxInputStartTime_Click(object sender, EventArgs e)
        {
            var dlg = new DateTimeInputDialog();
            var verifyRule = "CanNotBePastTime";
            dlg.VerificationRule = verifyRule;
            dlg.CustomFormat = _customFormat;
            dlg.ShowDialog();
            var inputText = dlg.InputText;
            if (inputText.IsNullOrEmpty()) return;

            if (!_latestStartTimeString.IsNullOrEmpty())
            {
                if (dlg.InputDateTime > _latestStartTime)
                {
                    PopupMessage.Popup(string.Format(WinformRes.Input + ValidationRes.CanNotBeLaterThan, _latestStartTimeString));
                    return;
                }

            }

            _startTimeString = inputText;
            _startTime = dlg.InputDateTime;
            textBoxStartTime.Text = _startTimeString;
        }

        private void pictureBoxStartOrPause_Click(object sender, EventArgs e)
        {
            StartOrStopTicking(!_isNowTicked);
        }


        private void timerTrigger_Tick(object sender, EventArgs e)
        {
            try
            {
                var span = _startTime.Subtract(SystemTimeHelper.Now());
                var ms = span.TotalMilliseconds;
                labelLeftTime.Text = SystemTimeHelper.GetTimeSpanString(ms, "s", true);
                if (_alarmLeadSeconds != 0 & _isAlarmOccured == false & SystemTimeHelper.Now().AddSeconds(_alarmLeadSeconds) > _startTime)
                {
                    OnAlarmTimeUp?.Invoke(this, null);
                    _isAlarmOccured = true;
                }

                if (SystemTimeHelper.Now() > _startTime)
                {
                    _isNowTicked = false;
                    pictureBoxStartOrPause.Visible = false;
                    pictureBoxInputStartTime.Visible = false;
                    RenderPictureBoxStartOrPause();

                    timerTrigger.Enabled = false;
                    timerTrigger.Stop();
                    OnRunTimeUp?.Invoke(this, null);

                    if (!_isUserMode) pictureBoxStartOrPause.Visible = true;
                    pictureBoxInputStartTime.Visible = true;

                    _isAlarmOccured = false;
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + "." + "Start Error: " + ex.Message);
            }
        }

        //#func
        private void InitData()
        {
            try
            {
                if (_dataSource.IsNullOrEmpty()) return;
                var valArry = _dataSource.Split(_dataSource.GetSubParamSeparator());

                if (valArry.Length > 0)
                {
                    var dt = new DateTime();
                    try
                    {
                        dt = DateTime.ParseExact(valArry[0], _customFormat, System.Globalization.CultureInfo.CurrentCulture);
                        if (dt.ToUtcTime().IsFutureTime())
                        {
                            _startTime = dt;
                            _startTimeString = valArry[0];
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }

                if (valArry.Length > 1)
                {
                    if (valArry[1].Length > 1)
                    {
                        var str = valArry[1];
                        var str1 = str.Substring(0, str.Length - 1);
                        if (str1.IsPlusInteger())
                        {
                            var delayTime = Convert.ToInt32(str1);
                            var unitStr = str.Substring(str.Length - 1, 1);
                            switch (unitStr)
                            {
                                case "H":
                                    {
                                        _latestStartTime = _startTime.AddHours(delayTime);
                                        break;
                                    }
                                case "m":
                                    {
                                        _latestStartTime = _startTime.AddMinutes(delayTime);
                                        break;
                                    }
                                case "s":
                                    {
                                        _latestStartTime = _startTime.AddSeconds(delayTime);
                                        break;
                                    }
                                default:
                                    {
                                        _latestStartTime = _startTime.AddMinutes(delayTime);
                                        break;
                                    }
                            }
                            _latestStartTimeString = _latestStartTime.ToString(_customFormat);
                        }
                    }
                }

                if (valArry.Length > 2)
                {
                    if (!valArry[2].IsPlusInteger())
                    {
                        _alarmLeadSeconds = Convert.ToInt32(valArry[2]);
                    }
                }

                if (valArry.Length > 3)
                {
                    if (valArry[3].IsPlusInteger())
                    {
                        _intervalSeconds = Convert.ToInt32(valArry[3]);
                    }

                }
            }
            catch (Exception ex)
            {
                PopupMessage.Popup(WinformRes.InitData + WinformRes.Error + ": " + ex.Message);
            }
        }

        private void Render()
        {
            try
            {
                if (_isUserMode) pictureBoxStartOrPause.Visible = false;
                RenderPictureBoxStartOrPause();

                if (!_startTimeString.IsNullOrEmpty())
                    textBoxStartTime.Text = _startTimeString;
                pictureBoxInputStartTime.BackgroundImage = imageList.Images[0];
                if (!_latestStartTimeString.IsNullOrEmpty())
                    textBoxLatestStartTime.Text = _latestStartTimeString;

                timerTrigger.Interval = _intervalSeconds * 1000;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + "." + "Render Error: " + ex.Message);
            }
        }



        private void StartOrStopTicking(bool setTickOn)
        {
            try
            {
                if (setTickOn)
                {
                    if (_isNowTicked == false)
                    {
                        if (_startTimeString.IsNullOrEmpty())
                        {
                            PopupMessage.Popup(WinformRes.SetValue + WinformRes.Error + ": " + WinformRes.StartTime + ValidationRes.CannotBeNull);
                            return;
                        }

                        if (_startTime.ToUtcTime().IsPastTime())
                        {
                            PopupMessage.Popup(WinformRes.SetValue + WinformRes.Error + ": " + WinformRes.StartTime + ValidationRes.CanNotBePastTime);
                            return;
                        }

                        _isAlarmOccured = false;
                        _isNowTicked = true;
                        timerTrigger.Enabled = true;
                        timerTrigger.Start();
                    }
                }
                else
                {
                    _isNowTicked = false;
                    timerTrigger.Enabled = false;
                    timerTrigger.Stop();
                    labelLeftTime.Text = string.Empty;
                }

                RenderPictureBoxStartOrPause();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + "." + "SetTick Error: " + ex.Message);
            }
        }

        private void RenderPictureBoxStartOrPause()
        {
            pictureBoxStartOrPause.BackgroundImage = _isNowTicked ? imageList.Images[2] : imageList.Images[1];
            _toolTipStartOrPause.SetToolTip(pictureBoxStartOrPause, _isNowTicked ? WinformRes.Pause : WinformRes.Start);
        }



        public void SetTextByCulture()
        {
            try
            {
                labelTimeLeft.Text = WinformRes.TimeLeft;
                labelStartTime.Text = WinformRes.StartTime;
                labelLatestStartTime.Text = WinformRes.LatestStartTime;
                _toolTipInputStartTime.SetToolTip(pictureBoxInputStartTime, WinformRes.Input + WinformRes.StartTime);
                if (_isNowTicked)
                    _toolTipStartOrPause.SetToolTip(pictureBoxStartOrPause, WinformRes.Pause);
                else _toolTipStartOrPause.SetToolTip(pictureBoxStartOrPause, WinformRes.Start);


            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + "." + "SetTextByCulture Error: " + ex.Message);
            }
        }

    }

}
