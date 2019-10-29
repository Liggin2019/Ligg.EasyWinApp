using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
using Ligg.Winform.Dialogs;
using Ligg.Winform.Resources;

namespace Ligg.Winform.Controls
{
    public partial class TimerExRecurringRun : UserControl
    {
        public TimerExRecurringRun()
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
        private bool _isLoadingCompleted;
        private ToolTip _toolTipInputFirstRunTime = new ToolTip();
        private ToolTip _toolTipInputStartTime = new ToolTip();
        private ToolTip _toolTipStartOrPause = new ToolTip();
        private ToolTip _toolTipShowLog = new ToolTip();
        private string _customFormat = "yyyy-MM-dd HH:mm:ss";


        private string _firstRunTimeString;
        private DateTime _firstRunTime;
        private int _circlePeriod = 0;
        private int _circleUnitIndex = 1;
        private int _circlePeriodSeconds = 0;
        private int _delaySeconds = 0;

        private string _nextRunTimeString;
        private DateTime _nextRunTime;
        private string _startTimeString;
        private DateTime _startTime;
        private string _latestStartTimeString;
        private DateTime _latestStartTime;


        private bool _isAlarmOccured;
        private int _alarmLeadSeconds = 10;
        private int _intervalSeconds = 1;
        private List<TimerExRecurringRunLog> _logs = new List<TimerExRecurringRunLog>();


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
        public bool Value
        {
            get => _isUserMode;
            set
            {
                if (value & (!_firstRunTimeString.IsNullOrEmpty()) & (_circlePeriodSeconds > 0))
                {
                    _isUserMode = true;
                }
            }
        }


        //#proc
        //#load
        private void TimerEx_Load(object sender, EventArgs e)
        {
            try
            {
                if (!_isUserMode)
                {
                    _delaySeconds = 0;
                    SetLatestStartTime();
                }
                Render();
                SetTextByCulture();
                _isLoadingCompleted = true;

                if (_isUserMode)
                {
                    pictureBoxStartOrPause_Click(null,null);
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
            pictureBoxShowLog.Left = Width - 16 - 5 - 22;
        }

        private void pictureBoxInputFirstRunTime_Click(object sender, EventArgs e)
        {
            var dlg = new DateTimeInputDialog();

            dlg.CustomFormat = _customFormat;
            dlg.ShowDialog();
            var inputText = dlg.InputText;
            if (inputText.IsNullOrEmpty()) return;

            _firstRunTimeString = inputText;
            _firstRunTime = dlg.InputDateTime;
            textBoxFirstRunTime.Text = _firstRunTimeString;
            SetNextRunTime(); textBoxStartTime.Text = _startTimeString;
        }

        private void textBoxCyclePeriod_TextChanged(object sender, EventArgs e)
        {
            if (!_isLoadingCompleted) return;
            if (!textBoxCyclePeriod.Text.IsPlusInteger()) return;
            _circlePeriod = Convert.ToInt32(textBoxCyclePeriod.Text);
            SetCirclePeriod();
            SetNextRunTime(); textBoxStartTime.Text = _startTimeString;
        }

        private void comboBoxUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isLoadingCompleted) return;
            _circleUnitIndex = comboBoxUnit.SelectedIndex;
            SetCirclePeriod();
            SetNextRunTime(); textBoxStartTime.Text = _startTimeString;
            //SetLatestStartTime(); textBoxLatestStartTime.Text = _latestStartTimeString;
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
            if (!_isNowTicked)
            {
                SetNextRunTime(); textBoxStartTime.Text = _startTimeString;
            }

            SetTick(!_isNowTicked);

            RenderPictureBoxInputStartTime();

        }

        private void pictureBoxShowLog_Click(object sender, EventArgs e)
        {
            var fields = WinformRes.Times + "\t" + WinformRes.StartTime + "\t" + WinformRes.FinishTime;
            var logTxt = ListHelper.ConvertToRichText(_logs, true);
            logTxt = fields + '\n' + logTxt;
            var dlg = new RichTextViewer();
            dlg.Title = WinformRes.RunningLog;
            dlg.Content = logTxt;
            dlg.FormWidth = 0;
            dlg.ShowDialog();
        }

        private void timerTrigger_Tick(object sender, EventArgs e)
        {
            try
            {
                var span = _startTime.Subtract(SystemTimeHelper.Now());
                var ms = span.TotalMilliseconds;
                labelLeftTime.Text = SystemTimeHelper.GetTimeSpanString(ms, "s", true);
                if (_alarmLeadSeconds != 0 & _isAlarmOccured == false &
                    SystemTimeHelper.Now().AddSeconds(_alarmLeadSeconds) > _startTime)
                {
                    OnAlarmTimeUp?.Invoke(this, null);
                    _isAlarmOccured = true;
                }

                if (SystemTimeHelper.Now() > _startTime)
                {
                    _isNowTicked = false;
                    RenderPictureBoxStartOrPause();
                    RenderPictureBoxInputStartTime();

                    pictureBoxShowLog.Visible = false;
                    pictureBoxStartOrPause.Visible = false;
                    pictureBoxInputFirstRunTime.Visible = false;
                    textBoxCyclePeriod.Enabled = false;
                    comboBoxUnit.Enabled = false;

                    timerTrigger.Enabled = false;
                    timerTrigger.Stop();

                    var log = new TimerExRecurringRunLog();
                    log.Id = ListHelper.GetLargestId(_logs) + 1;
                    log.StartTime = DateTime.Now.ToString(_customFormat);
                    OnRunTimeUp?.Invoke(this, null);
                    log.FinishTime = DateTime.Now.ToString(_customFormat);
                    _logs.Add(log);

                    SetNextRunTime(); textBoxStartTime.Text = _startTimeString;

                    if (!_isUserMode)
                    {
                        pictureBoxStartOrPause.Visible = true;
                        pictureBoxInputFirstRunTime.Visible = true;
                        textBoxCyclePeriod.Enabled = true;
                        comboBoxUnit.Enabled = true;
                    }
                    pictureBoxShowLog.Visible = true;
                    _isAlarmOccured = false;

                    _isNowTicked = true;
                    RenderPictureBoxStartOrPause();
                    RenderPictureBoxInputStartTime();
                    timerTrigger.Enabled = true;
                    timerTrigger.Start();
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
                        _firstRunTimeString = valArry[0];
                        _firstRunTime = dt;
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
                            _circlePeriod = Convert.ToInt32(str1);
                            var unitStr = str.Substring(str.Length - 1, 1);
                            switch (unitStr)
                            {
                                case "M":
                                    {
                                        _circleUnitIndex = 0;
                                        break;
                                    }
                                case "d":
                                    {
                                        _circleUnitIndex = 1;
                                        break;
                                    }
                                case "H":
                                    {
                                        _circleUnitIndex = 2;
                                        break;
                                    }
                                case "m":
                                    {
                                        _circleUnitIndex = 3;
                                        break;
                                    }
                                case "s":
                                    {
                                        _circleUnitIndex = 4;
                                        break;
                                    }
                                default:
                                    {
                                        _circleUnitIndex = 1;
                                        break;
                                    }
                            }

                            SetCirclePeriod();
                            SetNextRunTime();
                        }
                    }
                }

                if (valArry.Length > 2)
                {
                    var str = valArry[2];
                    var str1 = str.Substring(0, str.Length - 1);
                    if (str1.IsPlusInteger())
                    {
                        var delaySecondsTmp = Convert.ToInt32(str1);
                        var unitStr = str.Substring(str.Length - 1, 1);
                        switch (unitStr)
                        {
                            case "H":
                                {
                                    _delaySeconds = delaySecondsTmp * 60 * 60;
                                    break;
                                }
                            case "m":
                                {
                                    _delaySeconds = delaySecondsTmp * 60;
                                    break;
                                }
                            case "s":
                                {
                                    _delaySeconds = delaySecondsTmp;
                                    break;
                                }
                            default:
                                {
                                    _delaySeconds = delaySecondsTmp * 60;
                                    break;
                                }
                        }

                        SetLatestStartTime();
                    }
                }

                if (valArry.Length > 3)
                {
                    if (!valArry[3].IsPlusInteger())
                    {
                        _alarmLeadSeconds = Convert.ToInt32(valArry[3]);
                    }
                }

                if (valArry.Length > 4)
                {
                    if (!valArry[4].IsPlusInteger())
                    {
                        _intervalSeconds = Convert.ToInt32(valArry[4]);
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
                pictureBoxInputStartTime.BackgroundImage = imageList.Images[0];
                pictureBoxInputFirstRunTime.BackgroundImage = imageList.Images[0];
                pictureBoxShowLog.BackgroundImage = imageList.Images[3];

                if (_isUserMode)
                {
                    pictureBoxStartOrPause.Visible = false;
                    pictureBoxInputFirstRunTime.Visible = false;
                    textBoxCyclePeriod.Enabled = false;
                    comboBoxUnit.Enabled = false;
                }

                RenderPictureBoxStartOrPause();
                RenderPictureBoxInputStartTime();

                if (!_firstRunTimeString.IsNullOrEmpty()) textBoxFirstRunTime.Text = _firstRunTimeString;

                if (_circlePeriod > 0) textBoxCyclePeriod.Text = _circlePeriod.ToString();
                comboBoxUnit.SelectedIndex = _circleUnitIndex;

                textBoxStartTime.Text = _startTimeString;

                textBoxLatestStartTime.Text = _latestStartTimeString;

                timerTrigger.Interval = _intervalSeconds * 1000;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + "." + "Render Error: " + ex.Message);
            }
        }

        private void SetCirclePeriod()
        {
            switch (_circleUnitIndex)
            {
                case 0:
                    {
                        _circlePeriodSeconds = _circlePeriod * 60 * 60 * 24 * 30;
                        break;
                    }
                case 1:
                    {
                        _circlePeriodSeconds = _circlePeriod * 60 * 60 * 24;
                        break;
                    }
                case 2:
                    {
                        _circlePeriodSeconds = _circlePeriod * 60 * 60;
                        break;
                    }
                case 3:
                    {
                        _circlePeriodSeconds = _circlePeriod * 60;
                        break;
                    }
                case 4:
                    {
                        _circlePeriodSeconds = _circlePeriod;
                        break;
                    }
                default:
                    {
                        _circlePeriodSeconds = _circlePeriod * 60 * 60 * 60;
                        break;
                    }
            }
        }

        private void SetNextRunTime()
        {
            if (_firstRunTimeString.IsNullOrEmpty()) return;
            if (_firstRunTime.ToUtcTime().IsFutureTime())
            {
                _nextRunTimeString = _firstRunTimeString;
                _nextRunTime = _firstRunTime;
            }
            else
            {
                if (_circlePeriodSeconds > 0)
                {
                    var tmpDec = (decimal)((DateTime.Now - _firstRunTime).TotalSeconds / _circlePeriodSeconds);
                    var tmpInt = Convert.ToInt32(Math.Ceiling(tmpDec));
                    if (_circleUnitIndex != 0)
                    {
                        _nextRunTime = _firstRunTime.AddSeconds(tmpInt * _circlePeriodSeconds);
                        _nextRunTimeString = _nextRunTime.ToString(_customFormat);
                    }
                    else
                    {
                        _nextRunTime = SetNextRunTimeByMonth(_firstRunTime, _circlePeriod);
                        _nextRunTimeString = _nextRunTime.ToString(_customFormat);
                    }

                }
            }

            _startTimeString = _nextRunTimeString;
            _startTime = _nextRunTime;

        }

        private DateTime SetNextRunTimeByMonth(DateTime firstRunTime, int circlePeriod)
        {
            firstRunTime = firstRunTime.AddMonths(circlePeriod);
            if (firstRunTime < DateTime.Now)
            {
                firstRunTime = SetNextRunTimeByMonth(firstRunTime, circlePeriod);
            }
            return firstRunTime;
        }

        private void SetLatestStartTime()
        {
            if (_delaySeconds > 0)
            {
                _latestStartTime = _nextRunTime.AddSeconds(_delaySeconds);
                _latestStartTimeString = _latestStartTime.ToString(_customFormat);
            }
            else
            {
                _latestStartTimeString = string.Empty;
            }
        }


        private void SetTick(bool setTickOn)
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

        private void RenderPictureBoxInputStartTime()
        {
            pictureBoxInputStartTime.Visible = _isNowTicked;
        }



        public void SetTextByCulture()
        {
            try
            {
                labelTimeLeft.Text = WinformRes.TimeLeft;
                labelStartTime.Text = WinformRes.StartTime;
                _toolTipInputStartTime.SetToolTip(pictureBoxInputStartTime, WinformRes.Input + WinformRes.StartTime);
                _toolTipInputFirstRunTime.SetToolTip(pictureBoxInputFirstRunTime, WinformRes.Input + WinformRes.FirstRunTime);
                _toolTipShowLog.SetToolTip(pictureBoxShowLog, WinformRes.ShowLog);
                labelLatestStartTime.Text = WinformRes.LatestStartTime;

                if (_isNowTicked)
                    _toolTipStartOrPause.SetToolTip(pictureBoxStartOrPause, WinformRes.Pause);
                else _toolTipStartOrPause.SetToolTip(pictureBoxStartOrPause, WinformRes.Start);

                labelFirstRunTime.Text = WinformRes.FirstRunTime;

                labelCycleTime.Text = WinformRes.CycleTime;

                var selectedIndex = comboBoxUnit.SelectedIndex == -1 ? 0 : comboBoxUnit.SelectedIndex;
                comboBoxUnit.DataSource = new string[] { WinformRes.Months, WinformRes.Days, WinformRes.Hours, WinformRes.Minutes, WinformRes.Seconds };
                comboBoxUnit.SelectedIndex = selectedIndex;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + "." + "SetTextByCulture Error: " + ex.Message);
            }
        }
    }

    internal class TimerExRecurringRunLog
    {
        internal long Id;
        internal string StartTime;
        internal string FinishTime;
    }

}
