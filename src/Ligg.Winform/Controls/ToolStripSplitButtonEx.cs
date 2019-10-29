using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Ligg.Base.DataModel;
using Ligg.Base.DataModel.Enums;
using Ligg.Base.Handlers;
using Ligg.Base.Helpers;
using Ligg.Winform.DataModel;
using Ligg.Winform.Helpers;

namespace Ligg.Winform.Controls
{
    public sealed class ToolStripSplitButtonEx : ToolStripSplitButton
    {
        //#event
        public event EventHandler OnMenuItemClick;

        //#property
        private List<ContextMenuItem> _menuItems = null;
        private List<Annex> _annexes = null;
        private bool _supportMutiLanguages;

        public ControlAction CurrentAction
        {
            get;
            set;
        }

        //#constructor
        public ToolStripSplitButtonEx(bool supportMutiLanguages, string itemsCfgXmPath)
        {
            try
            {
                _supportMutiLanguages = supportMutiLanguages;
                var location = FileHelper.GetFileDetailByOption(itemsCfgXmPath, FilePathComposition.Directory);
                var fileTitle = FileHelper.GetFileDetailByOption(itemsCfgXmPath, FilePathComposition.FileTitle);
                var contentMenuItemsAnnexesCfgXmlPath = location + "\\" + fileTitle + "Annexes.xml";
                var xmlMgr = new XmlHandler(itemsCfgXmPath);
                _menuItems = xmlMgr.ConvertToObject<List<ContextMenuItem>>();
                foreach (var menuItem in _menuItems)
                {
                    var imageUrl = FileHelper.GetFilePath(menuItem.ImageUrl, location);
                    menuItem.ImageUrl = imageUrl;
                }
                var xmlMgr1 = new XmlHandler(contentMenuItemsAnnexesCfgXmlPath);
                _annexes = xmlMgr1.ConvertToObject<List<Annex>>();
                CheckMenuConfigData(_menuItems);
                if (_menuItems.Count > 0)
                {
                    InitComponet(null, -1);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + "." + "ToolStripSplitButtonEx Error: " + ex.Message);
            }
        }

        public ToolStripSplitButtonEx(bool supportMutiLanguages, List<ContextMenuItem> items, List<Annex> annexes)
        {
            try
            {
                _supportMutiLanguages = supportMutiLanguages;
                _menuItems = items;
                _annexes = annexes;
                CheckMenuConfigData(_menuItems);
                if (_menuItems.Count > 0)
                {
                    InitComponet(null, -1);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + "." + "ToolStripSplitButtonEx Error: " + ex.Message);
            }
        }

        //#method
        private void toolStripMenuItem_Click(object sender, EventArgs e)
        {
            var menuItemName = (sender as ToolStripMenuItem).Name;
            this.Tag = menuItemName;
            var menuItem = _menuItems.Find(x => x.Name == menuItemName);
            var args = new ContextMenuItemClickEventArgs(null, menuItemName, menuItem.Action);
            if (OnMenuItemClick != null)
            {
                var curAction = new ControlAction();
                curAction.Action = menuItem.Action;
                curAction.DisplayName = LayoutHelper.GetControlDisplayName(_supportMutiLanguages,"", menuItem.Name, _annexes, menuItem.DisplayName);
                CurrentAction = curAction;
                OnMenuItemClick(this, args);
            }
        }

        //#proc
        public void SetTextByCulture()
        {
            try
            {
                foreach (var menuItem in _menuItems)
                {
                    var menuItemCpnt = this.DropDownItems.Find(menuItem.Name, true);
                    menuItemCpnt[0].Text = LayoutHelper.GetControlDisplayName(_supportMutiLanguages, "", menuItem.Name, _annexes, menuItem.DisplayName);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + "." + "SetTextByCulture Error: " + ex.Message);
            }
        }


        public void Render(List<ContextMenuItem> menuItems, List<Annex> annexes)
        {
            DropDownItems.Clear();
            if (menuItems != null)
            {
                _menuItems = menuItems;
            }

            if (annexes != null)
            {
                _annexes = annexes;
            }

            InitComponet(null, -1);//如有image，会报错如ResetEnabled
        }

        private void InitComponet(ToolStripMenuItem toolStripMenuItem, int id)
        {
            try
            {
                foreach (var menuItem in _menuItems.FindAll(x => x.ParentId == id))
                {
                    //Visible
                    bool isCpntVisible = true;
                    var visibleFlag = menuItem.VisibleFlag;
                    if (string.IsNullOrEmpty(visibleFlag)) visibleFlag = "true";
                    isCpntVisible = (visibleFlag.ToLower() == "false" | visibleFlag.ToLower() == "0") ? false : true;

                    //Enabled
                    bool isCpntEnabled = true;
                    var enabledFlag = menuItem.EnabledFlag;
                    if (string.IsNullOrEmpty(enabledFlag)) enabledFlag = "true";
                    isCpntEnabled = (enabledFlag.ToLower() == "false" | enabledFlag.ToLower() == "0") ? false : true;

                    if (menuItem.ControlTypeName == null)
                        menuItem.ControlTypeName = "";
                    if (menuItem.ControlTypeName.ToLower() != "Seperator".ToLower())
                    {
                        var menuItemCpnt = new ToolStripMenuItem();
                        menuItemCpnt.Name = menuItem.Name;

                        menuItemCpnt.Text = LayoutHelper.GetControlDisplayName(_supportMutiLanguages, "", menuItemCpnt.Name, _annexes, menuItem.DisplayName);
                        menuItemCpnt.TextAlign = ContentAlignment.TopCenter;
                        var img1 = ControlHelper.GetImage(menuItem.ImageUrl);
                        if (img1 != null) menuItemCpnt.Image = img1;
                        menuItemCpnt.Visible = isCpntVisible ? true : false;
                        menuItemCpnt.Enabled = isCpntEnabled ? true : false;
                        //menuItemCpnt.AutoSize = false;
                        menuItemCpnt.ImageScaling = ToolStripItemImageScaling.None;

                        if (id > 0)
                        {
                            toolStripMenuItem.DropDownItems.Add(menuItemCpnt);
                        }
                        else
                        {
                            this.DropDownItems.Add(menuItemCpnt);
                        }
                        var item = menuItem;
                        var subMenuItems = _menuItems.FindAll(x => x.ParentId == item.Id);
                        if (subMenuItems.Count > 0)
                        {
                            InitComponet(menuItemCpnt, menuItem.Id);
                        }
                        else
                        {
                            menuItemCpnt.Click += new System.EventHandler(toolStripMenuItem_Click);
                        }
                    }
                    else if (menuItem.ControlTypeName.ToLower().Contains("Seperator".ToLower()))
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
                            DropDownItems.Add(menuItemCpnt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + "." + "InitComponet Error: " + ex.Message);
            }
        }

        private static void CheckMenuConfigData(List<ContextMenuItem> items)
        {
            if (items == null) throw new ArgumentException("Items can't be null");
            foreach (var item in items)
            {
                if (items.FindAll(x => x.Id == item.Id).Count > 1)
                {
                    throw new ArgumentException("Item can't have duplicated Id, MenuItem Id=" + item.Id);
                }
                if (items.FindAll(x => x.Name == item.Name).Count > 1)
                {
                    throw new ArgumentException("Item can't have duplicated name, MenuItem Name=" + item.Name);
                }
            }
        }


    }

}
