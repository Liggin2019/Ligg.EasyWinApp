using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Ligg.Base.DataModel;
using Ligg.Base.DataModel.Enums;
using Ligg.Base.Handlers;
using Ligg.Base.Helpers;
using Ligg.WinForm.DataModel;
using Ligg.WinForm.Helpers;

namespace Ligg.WinForm.Controls
{
    public sealed class ContextMenuStripEx : ContextMenuStrip
    {
        //#event
        public event EventHandler OnItemClick;

        //#property
        private List<ContextMenuItem> _menuItems = null;
        private List<Annex> _annexes = null;
        private bool _supportMultiLanguages;

        public ControlAction CurrentAction
        {
            get;
            set;
        }

        //#constructor
        public ContextMenuStripEx(bool supportMultiLanguages, string contentMenuCfgXmPath)
        {
            try
            {
                _supportMultiLanguages = supportMultiLanguages;
                var dir = FileHelper.GetFileDetailByOption(contentMenuCfgXmPath, FilePathComposition.Directory);
                var fileTitle = FileHelper.GetFileDetailByOption(contentMenuCfgXmPath, FilePathComposition.FileTitle);
                var contentMenuItemsAnnexesCfgXmlPath = dir + "\\" + fileTitle + "Annexes";
                var xmlMgr = new XmlHandler(contentMenuCfgXmPath);
                _menuItems = xmlMgr.ConvertToObject<List<ContextMenuItem>>();
                var xmlMgr1 = new XmlHandler(contentMenuItemsAnnexesCfgXmlPath);
                _annexes = xmlMgr1.ConvertToObject<List<Annex>>();

                CheckMenuConfigData(_menuItems);
                if (_menuItems.Count > 0)
                {
                    InitComponent(null, -1);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + "." + "ContextMenuStripEx Error: " + ex.Message);
            }
        }

        public ContextMenuStripEx(bool supportMultiLanguages, List<ContextMenuItem> contentMenuItems, List<Annex> annexes)
        {
            try
            {
                _supportMultiLanguages = supportMultiLanguages;
                _menuItems = contentMenuItems;
                _annexes = annexes;
                CheckMenuConfigData(_menuItems);
                if (_menuItems.Count > 0)
                {
                    InitComponent(null, -1);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + "." + "ContextMenuStripEx Error: " + ex.Message);
            }
        }

        //#method
        private void toolStripMenuItem_Click(object sender, EventArgs e)
        {
            var menuItemName = (sender as ToolStripMenuItem).Name;
            this.Tag = menuItemName;
            var menuItem = _menuItems.Find(x => x.Name == menuItemName);
            var args = new ContextMenuItemClickEventArgs(null, menuItemName, menuItem.Action);
            if (OnItemClick != null)
            {
                var curAction = new ControlAction();
                curAction.Action = menuItem.Action;
                curAction.DisplayName = LayoutHelper.GetControlDisplayName(_supportMultiLanguages, "", menuItem.Name, _annexes, menuItem.DisplayName);

                CurrentAction = curAction;
                OnItemClick(this, args);
            }
        }

        //#proc
        public void SetTextByCulture()
        {
            try
            {
                foreach (var menuItem in _menuItems)
                {
                    var menuItemCpnt = this.Items.Find(menuItem.Name, true);
                    menuItemCpnt[0].Text = LayoutHelper.GetControlDisplayName(_supportMultiLanguages, "", menuItem.Name, _annexes, menuItem.DisplayName);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + "." + "SetTextByCulture Error: " + ex.Message);
            }
        }


        public void Render(List<ContextMenuItem> menuItems, List<Annex> annexes)
        {
            Items.Clear();
            if (menuItems != null)
            {
                _menuItems = menuItems;
            }

            if (annexes != null)
            {
                _annexes = annexes;
            }

            InitComponent(null, -1);//如有image，会报错如ResetEnabled
        }

        private void InitComponent(ToolStripMenuItem toolStripMenuItem, int id)
        {
            try
            {
                foreach (var menuItem in _menuItems.FindAll(x => x.ParentId == id))
                {
                    //Visible
                    bool isCpntVisible = true;
                    var invisibleFlag = menuItem.InvisibleFlag;
                    if (string.IsNullOrEmpty(invisibleFlag)) invisibleFlag = "false";
                    isCpntVisible = (invisibleFlag.ToLower() != "true");

                    //Enabled
                    bool isCpntEnabled = true;
                    var disabledFlag = menuItem.DisabledFlag;
                    if (string.IsNullOrEmpty(disabledFlag)) disabledFlag = "false";
                    isCpntEnabled = (disabledFlag.ToLower() != "true");

                    if (menuItem.ControlTypeName.ToLower() != "Separator".ToLower())
                    {
                        var menuItemCpnt = new ToolStripMenuItem();
                        menuItemCpnt.Name = menuItem.Name;
                        menuItemCpnt.Text = LayoutHelper.GetControlDisplayName(_supportMultiLanguages, "", menuItemCpnt.Name, _annexes, menuItem.DisplayName);

                        menuItemCpnt.TextAlign = ContentAlignment.MiddleLeft;
                        var img1 = ControlHelper.GetImage(menuItem.ImageUrl);
                        if (img1 != null) menuItemCpnt.Image = img1;
                        menuItemCpnt.Visible = isCpntVisible ? true : false;
                        menuItemCpnt.Enabled = isCpntEnabled ? true : false;
                        if (id > 0)
                        {
                            toolStripMenuItem.DropDownItems.Add(menuItemCpnt);
                        }
                        else
                        {
                            Items.Add(menuItemCpnt);
                        }
                        var item = menuItem;
                        var subMenuItems = _menuItems.FindAll(x => x.ParentId == item.Id);
                        if (subMenuItems.Count > 0)
                        {
                            InitComponent(menuItemCpnt, menuItem.Id);
                        }
                        else
                        {
                            menuItemCpnt.Click += new System.EventHandler(toolStripMenuItem_Click);
                        }
                    }
                    else if (menuItem.ControlTypeName.ToLower().Contains("Separator".ToLower()))
                    {
                        var menuItemCpnt = new System.Windows.Forms.ToolStripSeparator();
                        menuItemCpnt.Name = menuItem.Name;
                        menuItemCpnt.Visible = isCpntVisible ? true : false;
                        menuItemCpnt.Enabled = isCpntEnabled ? true : false;
                        if (id > 0)
                        {
                            toolStripMenuItem.DropDownItems.Add(menuItemCpnt);
                        }
                        else
                        {
                            Items.Add(menuItemCpnt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + "." + "InitComponent Error: " + ex.Message);
            }
        }

        private static void CheckMenuConfigData(List<ContextMenuItem> items)
        {
            if (items == null) throw new ArgumentException("MenuItem can't be null");
            foreach (var item in items)
            {
                if (items.FindAll(x => x.Id == item.Id).Count > 1)
                {
                    throw new ArgumentException("MenuItem can't have duplicated Id, MenuItem Id=" + item.Id);
                }
                if (items.FindAll(x => x.Name == item.Name).Count > 1)
                {
                    throw new ArgumentException("MenuItem can't have duplicated name, MenuItem Name=" + item.Name);
                }
            }
        }



        public void Reset(List<ContextMenuItem> menuItems)
        {
            try
            {
                foreach (var menuItem in menuItems)
                {
                    var cpnts = Items.Find(menuItem.Name, true);
                    if (cpnts != null && cpnts.Length > 0)
                    {
                        //Visible
                        bool isCpntVisible = true;
                        var invisibleFlag = menuItem.InvisibleFlag;
                        if (string.IsNullOrEmpty(invisibleFlag)) invisibleFlag = "false";
                        isCpntVisible = (invisibleFlag.ToLower() != "true");

                        //Enabled
                        bool isCpntEnabled = true;
                        var disabledFlag = menuItem.DisabledFlag;
                        if (string.IsNullOrEmpty(disabledFlag)) disabledFlag = "false";
                        isCpntEnabled = (disabledFlag.ToLower() != "true");

                        cpnts[0].Visible = isCpntVisible != false;

                        if (isCpntEnabled == false & cpnts[0].Enabled == true)
                            cpnts[0].Enabled = false;
                        else if (isCpntEnabled == true & cpnts[0].Enabled == false)
                            cpnts[0].Enabled = true;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + "." + "Reset Error: " + ex.Message);
            }
        }
    }

    public class ContextMenuItemClickEventArgs : EventArgs
    {
        public ContextMenuItemClickEventArgs(ContextMenuStripEx contextMenuStripEx, string itemName, string action)
        {
            ContextMenuStripEx = contextMenuStripEx;
            ItemName = itemName;
            Action = action;
        }

        public ContextMenuStripEx ContextMenuStripEx { get; set; }
        public string ItemName { get; set; }
        public string Action { get; set; }
    }

    //#ResetEnabled
    //****if item has image, cause 'outof memory error' as following
    //        See the end of this message for details on invoking 
    //just-in-time (JIT) debugging instead of this dialog box.

    //************** Exception Text **************
    //System.OutOfMemoryException: Out of memory.
    //   at System.Drawing.Graphics.CheckErrorStatus(Int32 status)
    //   at System.Drawing.Graphics.DrawImage(Image image, Rectangle destRect, Int32 srcX, Int32 srcY, Int32 srcWidth, Int32 srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback, IntPtr callbackData)
    //   at System.Drawing.Graphics.DrawImage(Image image, Rectangle destRect, Int32 srcX, Int32 srcY, Int32 srcWidth, Int32 srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr)
    //   at System.Windows.Forms.ToolStripRenderer.CreateDisabledImage(Image normalImage)
    //   at System.Windows.Forms.ToolStripRenderer.OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
    //   at System.Windows.Forms.ToolStripRenderer.DrawItemImage(ToolStripItemImageRenderEventArgs e)
    //   at System.Windows.Forms.ToolStripMenuItem.OnPaint(PaintEventArgs e)
    //   at System.Windows.Forms.ToolStripItem.HandlePaint(PaintEventArgs e)
    //   at System.Windows.Forms.ToolStripItem.FireEvent(EventArgs e, ToolStripItemEventType met)
    //   at System.Windows.Forms.ToolStrip.OnPaint(PaintEventArgs e)
    //   at System.Windows.Forms.Control.PaintWithErrorHandling(PaintEventArgs e, Int16 layer)
    //   at System.Windows.Forms.Control.WmPaint(Message& m)
    //   at System.Windows.Forms.Control.WndProc(Message& m)
    //   at System.Windows.Forms.ScrollableControl.WndProc(Message& m)
    //   at System.Windows.Forms.ToolStrip.WndProc(Message& m)
    //   at System.Windows.Forms.ToolStripDropDown.WndProc(Message& m)
    //   at System.Windows.Forms.Control.ControlNativeWindow.OnMessage(Message& m)
    //   at System.Windows.Forms.Control.ControlNativeWindow.WndProc(Message& m)
    //   at System.Windows.Forms.NativeWindow.Callback(IntPtr hWnd, Int32 msg, IntPtr wparam, IntPtr lparam)


}
