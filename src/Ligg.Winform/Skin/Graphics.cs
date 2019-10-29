using System;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Ligg.Winform.Skin
{
    public class InterpolationModeGraphics : IDisposable
    {
        private readonly InterpolationMode _mode;
        private readonly System.Drawing.Graphics _graphics;

        public InterpolationModeGraphics(System.Drawing.Graphics graphics)
            : this(graphics, InterpolationMode.HighQualityBicubic)
        {
        }

        public InterpolationModeGraphics(
            System.Drawing.Graphics graphics, InterpolationMode newMode)
        {
            _graphics = graphics;
            _mode = graphics.InterpolationMode;
            graphics.InterpolationMode = newMode;
        }

        public void Dispose()
        {
            _graphics.InterpolationMode = _mode;
        }
    }

    public class SmoothingModeGraphics : IDisposable
    {
        private SmoothingMode _smoothingMode;
        private System.Drawing.Graphics _graphics;

        public SmoothingModeGraphics(System.Drawing.Graphics graphics)
        {
            _graphics = graphics;
            _smoothingMode = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }

        public SmoothingModeGraphics(System.Drawing.Graphics graphics, SmoothingMode newMode)
        {
            _graphics = graphics;
            _smoothingMode = graphics.SmoothingMode;
            graphics.SmoothingMode = newMode;
        }

        public void Dispose()
        {
            _graphics.SmoothingMode = _smoothingMode;
        }
    }

    internal class TextRenderingHintGraphics : IDisposable
    {
        private readonly System.Drawing.Graphics _graphics;
        private TextRenderingHint _textRenderingHint;

        public TextRenderingHintGraphics(System.Drawing.Graphics graphics)
            : this(graphics, TextRenderingHint.AntiAlias)
        {
        }

        public TextRenderingHintGraphics(
            System.Drawing.Graphics graphics,
            TextRenderingHint newTextRenderingHint)
        {
            _graphics = graphics;
            _textRenderingHint = graphics.TextRenderingHint;
            _graphics.TextRenderingHint = newTextRenderingHint;
        }

        public void Dispose()
        {
            _graphics.TextRenderingHint = _textRenderingHint;
        }
    }
}
