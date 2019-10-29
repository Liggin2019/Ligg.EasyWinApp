using System;
using System.Drawing;
using System.ComponentModel;
using System.Security.Permissions;

namespace Ligg.Winform.Skin
{
    public abstract class FormRenderer
    {
        #region Fields
        private EventHandlerList _events;
        private static readonly object EventRenderGroundFormCaption = new object();
        private static readonly object EventRenderGroundFormBorder = new object();
        private static readonly object EventRenderGroundFormBackground = new object();
        private static readonly object EventRenderGroundFormControlBox = new object();
        #endregion

        #region Constructors
        protected FormRenderer()
        {
        }
        #endregion

        #region Properties

        protected EventHandlerList Events
        {
            get
            {
                if (_events == null)
                {
                    _events = new EventHandlerList();
                }
                return _events;
            }
        }

        #endregion

        #region Events
        public event FormCaptionRenderEventHandler RenderGroundFormCaption
        {
            add { AddHandler(EventRenderGroundFormCaption, value); }
            remove { RemoveHandler(EventRenderGroundFormCaption, value); }
        }

        public event FormBorderRenderEventHandler RenderGroundFormBorder
        {
            add { AddHandler(EventRenderGroundFormBorder, value); }
            remove { RemoveHandler(EventRenderGroundFormBorder, value); }
        }

        public event FormBackgroundRenderEventHandler RenderGroundFormBackground
        {
            add { AddHandler(EventRenderGroundFormBackground, value); }
            remove { RemoveHandler(EventRenderGroundFormBackground, value); }
        }

        public event FormControlBoxRenderEventHandler RenderGroundFormControlBox
        {
            add { AddHandler(EventRenderGroundFormControlBox, value); }
            remove { RemoveHandler(EventRenderGroundFormControlBox, value); }
        }

        #endregion

        #region Public Methods

        public abstract Region CreateRegion(SkinForm form);

        public abstract void InitGroundForm(SkinForm  form);

        public void DrawGroundFormCaption(FormCaptionRenderEventArgs e)
        {
            OnRenderGroundFormCaption(e);
            var handle =Events[EventRenderGroundFormCaption]as FormCaptionRenderEventHandler;
            if (handle != null)
            {
                handle(this, e);
            }
        }


        public void DrawGroundFormBorder(FormBorderRenderEventArgs e)
        {
            OnRenderGroundFormBorder(e);
            var handle =Events[EventRenderGroundFormBorder]as FormBorderRenderEventHandler;
            if (handle != null)
            {
                handle(this, e);
            }
        }


        public void DrawGroundFormBackground(FormBackgroundRenderEventArgs e)
        {
            OnRenderGroundFormBackground(e);
            var handle = Events[EventRenderGroundFormBackground] as FormBackgroundRenderEventHandler;
            if (handle != null)
            {
                handle(this, e);
            }
        }

        public void DrawGroundFormControlBox(FormControlBoxRenderEventArgs e)
        {
            OnRenderGroundFormControlBox(e);
            var handle =Events[EventRenderGroundFormControlBox] as FormControlBoxRenderEventHandler;
            if (handle != null)
            {
                handle(this, e);
            }
        }

        #endregion

        #region Protected Render Methods
        protected abstract void OnRenderGroundFormCaption(FormCaptionRenderEventArgs e);
        protected abstract void OnRenderGroundFormBorder(FormBorderRenderEventArgs e);
        protected abstract void OnRenderGroundFormBackground(FormBackgroundRenderEventArgs e);
        protected abstract void OnRenderGroundFormControlBox(FormControlBoxRenderEventArgs e);

        #endregion

        #region Protected Methods
        [UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
        protected void AddHandler(object key, Delegate value)
        {
            Events.AddHandler(key, value);
        }

        [UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
        protected void RemoveHandler(object key, Delegate value)
        {
            Events.RemoveHandler(key, value);
        }

        #endregion
    }
}
