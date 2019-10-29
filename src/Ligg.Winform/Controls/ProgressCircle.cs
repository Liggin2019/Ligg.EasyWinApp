using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using Ligg.Winform.DataModel.Enums;

namespace Ligg.Winform.Controls
{
    public class ProgressCircle : Control
    {
        #region Fields

        private int _spokeNumber = 15;
        private int _spokeThickness = 4;
        private int _ringThickness = 8;
        private int _value = 0;
        private Timer _timer;
        private int _rotationSpeed = 60;
        private Color _baseColor = Color.DodgerBlue;
        private bool _active;
        private ProgressCircleStyle _style;

        #endregion

        #region Constructors

        public ProgressCircle()
            : base()
        {
            SetStyles();
        }

        #endregion

        #region Properties

        [DefaultValue(typeof(ProgressCircleStyle), "0")]
        public ProgressCircleStyle Style
        {
            get { return _style; }
            set
            {
                if (value != _style)
                {
                    _style = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(15)]
        public int SpokeNumber
        {
            get { return _spokeNumber; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(
                        "SpokeNumber",
                        "can't be less than 1!");
                }
                if (_spokeNumber != value)
                {
                    _spokeNumber = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(4)]
        public int SpokeThickness
        {
            get { return _spokeThickness; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(
                        "SpokeThickness",
                        "can't be less than 1!");
                }
                if (_spokeThickness != value)
                {
                    _spokeThickness = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(8)]
        public int RingThickness
        {
            get { return _ringThickness; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(
                       "RingThickness",
                       "can't be less than 1!");
                }
                if (value != _ringThickness)
                {
                    _ringThickness = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(60)]
        public int RotationSpeed
        {
            get { return _rotationSpeed; }
            set
            {
                if (value != _rotationSpeed)
                {
                    _rotationSpeed = value <=10 ? 10 : value;
                    Timer.Interval = _rotationSpeed;
                }
            }
        }

        [DefaultValue(false)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Active
        {
            get { return _active; }
            set
            {
                if (_active != value)
                {
                    if (value)
                    {
                        Start();
                    }
                    else
                    {
                        Stop();
                    }
                }
            }
        }

        [DefaultValue(typeof(Color),"DodgerBlue")]
        public Color BaseColor
        {
            get { return _baseColor; }
            set
            {
                _baseColor = value;
                base.Invalidate();
            }
        }

        internal PointF CenterPoint
        {
            get 
            {
                return new PointF(Width / 2.0f, Height / 2.0f);
            }
        }

        protected override Size DefaultSize
        {
            get { return new Size(100, 100); }
        }

        private Timer Timer
        {
            get
            {
                if (_timer == null)
                {
                    _timer = new Timer();
                    _timer.Interval = _rotationSpeed;
                    _timer.Tick += new EventHandler(TimerTick);
                }
                return _timer;
            }
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            if (!_active && !DesignMode)
            {
                _active = true;
                Timer.Start();
            }
        }

        public void Stop()
        {
            if (_active)
            {
                Timer.Stop();
                _active = false;
            }
        }

        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int value = _value;
            switch (_style)
            {
                case ProgressCircleStyle.Beam:
                    RenderLine(g, value);
                    break;
                case ProgressCircleStyle.Circle:
                    RenderCircle(g, value);
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (_timer != null)
                {
                    Stop();
                    _timer.Dispose();
                    _timer = null;
                }
            }
        }

        #region HelpMethods

        private void SetStyles()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint | 
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.CacheText, true);
            SetStyle(ControlStyles.Opaque, false);
            UpdateStyles();
        }

        private void RenderLine(Graphics g, int value)
        {
            for (int index = 0; index < SpokeNumber; index++)
            {
                value = value % SpokeNumber;
                using (Pen pen = new Pen(GetColor(index), _spokeThickness))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    PointF[] points = GetPoints(value);
                    g.DrawLine(pen, points[0], points[1]);
                }
                value++;
            }
        }

        private void RenderCircle(Graphics g, int value)
        {
            for (int index = 0; index < SpokeNumber; index++)
            {
                value = value % SpokeNumber;

                float radius = (float)RingThickness * (index + 1) / SpokeNumber;
                using (SolidBrush brush = new SolidBrush(GetColor(index)))
                {
                    g.FillEllipse(
                        brush,
                        new RectangleF(
                        GetCircleCenterPoint(value),new SizeF(radius,radius)));
                }
                value++;
            }
        }

        private PointF GetCircleCenterPoint(int position)
        {
            PointF centerPoint = CenterPoint;
            float outerRadius = Math.Min(Width - 2, Height - 2) / 2.0f;
            float innerRadius = outerRadius - RingThickness;
            double angle = CaculateAngle(position);
            return new PointF(
                centerPoint.X + innerRadius * (float)Math.Cos(angle),
                centerPoint.Y + innerRadius * (float)Math.Sin(angle));
        }

        private Color GetColor(int position)
        {
            float step = 255.0f / SpokeNumber;
            int alpha = (int)Math.Ceiling((position + 1) * step);
            if (alpha > 255)
            {
                alpha = 255;
            }
            return Color.FromArgb(alpha, _baseColor);
        }

        private PointF[] GetPoints(int position)
        {
            PointF centerPoint = CenterPoint;
            float outerRadius = Math.Min(Width - 8, Height - 8) / 2.0f;
            float innerRadius = outerRadius - RingThickness;
            PointF[] points = new PointF[2];
            double angle = CaculateAngle(position);
            points[0] = new PointF(
                centerPoint.X + innerRadius * (float)Math.Cos(angle),
                centerPoint.Y + innerRadius * (float)Math.Sin(angle));
            points[1] = new PointF(
               centerPoint.X + outerRadius * (float)Math.Cos(angle),
               centerPoint.Y + outerRadius * (float)Math.Sin(angle));
            return points;
        }

        private double CaculateAngle(int position)
        {
            return Math.PI * (((360D / SpokeNumber) * position) / 180);
        }

        private void TimerTick(object sender, EventArgs e)
        {
            _value = ++_value % _spokeNumber;
            base.Invalidate();
        }

        #endregion
    }
}
