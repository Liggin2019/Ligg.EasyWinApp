using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Ligg.Base.DataModel;
using Ligg.Base.DataModel.Enums;
using Ligg.Base.Extension;
using Ligg.Base.Handlers;
using Ligg.Base.Helpers;
using Ligg.WinForm.Controls;
using Ligg.WinForm.Controls.ShadowPanel;
using Ligg.WinForm.DataModel;
using Ligg.WinForm.DataModel.Enums;
using Ligg.WinForm.Helpers;
using Ligg.WinForm.Resources;
using ContentAlignment = System.Drawing.ContentAlignment;


namespace Ligg.WinForm.Forms
{
    public partial class FunctionForm : FrameForm
    {
        public bool IsOk = true;
        private FunctionInitParamSet _functionInitParamSet;
        public FunctionInitParamSet FunctionInitParamSet
        {
            get => _functionInitParamSet;
            set => _functionInitParamSet = value;
        }
        private List<Annex> _functionAnnexes = null;

        private List<ProcedureItem> _procedures = new List<ProcedureItem>();
        private List<Annex> _annexes = new List<Annex>();

        private List<LayoutElement> _layoutElements = new List<LayoutElement>();
        private FunctionFormStyle _functionFormStyle;
        private FunctionFormViewMenuMode _functionFormViewMenuMode;
        private bool _hasTray;
        private int _zoneWidthForSingleViewForm;
        private int _zoneHeightForSingleViewForm;
        private bool _hasRunningStatusSectionForNonMutiViewForm;
        private List<ZoneItem> _zonesItems = new List<ZoneItem>();

        protected int CurrentViewMenuId { get; private set; }
        protected string CurrentViewMenuName { get; private set; }
        protected string CurrentViewName { get; private set; }

        private List<RenderedViewStatus> _renderedViewStatuses = new List<RenderedViewStatus>();

        NotifyIcon _tray;
        ContextMenuStripEx _trayContextMenuStrip = null;
        private FormWindowState _ordinaryWindowStatus = FormWindowState.Normal;
        private readonly ToolTip _pictureBoxToolTip = new ToolTip();

        private string _basicInfoForException;
        private string _additionalInfoForException;

        private string _startupDir;
        private string _appDir;
        private string _formDir;
        private string _functionsDir;
        private string _zonesDir;
        private string _implDir;
        private string _appDataDir;

        public FunctionForm()
        {
            InitializeComponent();
            ToolBarSectionPublicRegionToolStrip.Enabled = true;
        }

        //#load
        private void FunctionForm_Load(object sender, EventArgs e)
        {
            try
            {
                _startupDir = Directory.GetCurrentDirectory();
                _appDir = _startupDir + "\\Applications\\" + _functionInitParamSet.ApplicationCode;
                _formDir = _appDir + "\\Clients\\Form";
                _functionsDir = _formDir + "\\Functions";
                _zonesDir = _formDir + "\\Zones";
                _implDir = _functionInitParamSet.ImplementationDir;
                _appDataDir = DirectoryHelper.GetSpecialDir("commonapplicationdata") + "\\" + _functionInitParamSet.ArchitectureCode + "\\" + _functionInitParamSet.ApplicationCode;
                if (!Directory.Exists(_appDataDir)) Directory.CreateDirectory(_appDataDir);

                _basicInfoForException = _functionInitParamSet.ArchitectureCode + "\\" + _functionInitParamSet.ApplicationCode + "\\" + _functionInitParamSet.FunctionCode;
                _additionalInfoForException = "HelpdeskEmail:" + _functionInitParamSet.HelpdeskEmail + ";" + "ApplicationVersion:" + _functionInitParamSet.ApplicationVersion + ";UserCode:";

                SetFrameTextByCulture(true, FunctionInitParamSet.SupportMultiCultures);
                LoadForm();

            }
            catch (Exception ex)
            {
                PopupMessage.PopupError(_basicInfoForException + ": " + GetType().FullName + ".FunctionForm_Load" + " Error", ex.Message, GetAdditionalInfoForException());
                Application.Exit();
            }
        }

        //#proc
        private void FunctionForm_Resize(object sender, EventArgs e)
        {
            try
            {
                ResizeFrameComponent();
                if (_hasTray == true & WindowState == FormWindowState.Minimized)
                {
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                PopupMessage.PopupError(_basicInfoForException + ": " + GetType().FullName + ".FunctionForm_Resize" + " Error", ex.Message, GetAdditionalInfoForException());
            }
        }

        private void tray_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    ShowForm();
                }
                else if (WindowState == _ordinaryWindowStatus)
                {
                    this.Visible = true;
                    this.Activate();
                }
            }
        }

        private void ToolBarSectionPublicRegionToolStripSplitButtonCultureItemClickHandler(object sender, EventArgs e)
        {
            try
            {
                var ctrl = sender as ToolStripMenuItem;
                var culName = Convert.ToString(ctrl.Tag);
                if (culName != CultureHelper.CurrentCultureName & !culName.IsNullOrEmpty())
                {
                    CultureHelper.SetCurrentCulture(culName);
                    var splitButtonCultures = ToolBarSectionPublicRegionToolStrip.Items.Find("ToolBarSectionPublicRegionToolStripSplitButtonCulture", true);
                    if (splitButtonCultures.Count() > 0)
                    {
                        var splitButtonCulture = splitButtonCultures[0];
                        splitButtonCulture.Text = CultureHelper.CurrentLanguageName;
                        splitButtonCulture.Tag = culName;
                        splitButtonCulture.ToolTipText = WinformRes.ChooseLanguage;
                        OnCurrentLanguageChanged();
                    }

                    SetFrameTextByCulture(false, FunctionInitParamSet.SupportMultiCultures);
                    SetLayoutTextByCulture();
                }
            }
            catch (Exception ex)
            {
                var methodName = "ToolBarSectionPublicRegionToolStripSplitButtonCultureItemClickHandler";
                PopupMessage.PopupError(_basicInfoForException + ": " + GetType().FullName + "." + methodName + " Error", ex.Message, GetAdditionalInfoForException());
            }
        }

        //#event handler 
        //##ViewEventHandler
        private void ViewEventHandler(string viewName, LayoutElementType eventHandlerType)
        {
            try
            {
                var eventHandlers = _layoutElements.FindAll(x => x.View == viewName && x.Type == (int)eventHandlerType);
                foreach (var eventHandler in eventHandlers)
                {
                    var eventHandlerDisplayName = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "ViewItem", eventHandler.Name, _annexes, eventHandler.DisplayName);
                    Act(eventHandler.Name, eventHandler.Action, eventHandlerDisplayName, true);
                }

            }
            catch (Exception ex)
            {
                PopupMessage.PopupError(_basicInfoForException + ": " + GetType().FullName + ".ViewEventHandler" + " Error: viewName='" + viewName + "'; ", ex.Message, GetAdditionalInfoForException());
            }
        }

        //##ZoneEventHandler
        private void ZoneEventHandler(string zoneName, LayoutElementType eventHandlerType)
        {
            try
            {
                var eventHandlers = _zonesItems.FindAll(x => x.Name.StartsWith(zoneName + "_") && x.Type == (int)eventHandlerType);
                foreach (var eventHandler in eventHandlers)
                {
                    var eventHandlerDisplayName = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "ZoneItem", eventHandler.Name, _annexes, eventHandler.DisplayName);
                    Act(eventHandler.Name, eventHandler.Action, eventHandlerDisplayName, true);
                }
            }
            catch (Exception ex)
            {
                PopupMessage.PopupError(_basicInfoForException + ": " + GetType().FullName + ".ZoneEventHandler" + " Error: zoneName='" + zoneName + "'", ex.Message, GetAdditionalInfoForException());
            }
        }

        //##ViewMenuItemClickHandler
        private void ViewMenuItemClickHandler(object sender, EventArgs e)
        {
            var ctrlName = "";
            try
            {
                var type = sender.GetType().ToString();
                if (type.ToLower().EndsWith("PictureBox".ToLower())) ctrlName = (sender as PictureBox).Name;
                else if (type.ToLower().EndsWith("ToolStripButton".ToLower())) ctrlName = (sender as ToolStripButton).Name;
                else if (type.ToLower().EndsWith("Button".ToLower())) ctrlName = (sender as Button).Name;
                else if (type.ToLower().EndsWith("ToolStripMenuItem".ToLower())) ctrlName = (sender as ToolStripMenuItem).Name;
                else throw new ArgumentException("Control type: " + type + " didn't be considered!");

                var menuItem = _layoutElements.Find(x => x.Name == ctrlName);
                if (_functionFormViewMenuMode == FunctionFormViewMenuMode.Simple)
                {
                    if (menuItem.View != null)
                    {
                        if (IsViewRendered(menuItem.View))
                        {
                            HideLastCheckedViewThenShowCurrentView(menuItem.View);
                            SetViewCheckedAndUncheckLastView(menuItem.View);
                        }
                        else
                        {
                            MergeViewItems(menuItem.View, menuItem.DataSource);
                            RenderView(menuItem.View);

                            HideLastCheckedViewThenShowCurrentView(menuItem.View);
                            UncheckLastCheckedView();
                            AddViewStatus(menuItem.View, true);
                        }
                        CurrentViewMenuId = menuItem.Id;
                    }
                }
                else
                {
                    if (!menuItem.IsChecked)
                    {
                        var viewMenuArea = _layoutElements.Find(x => x.Name == menuItem.Container
                                                             && x.Type == (int)LayoutElementType.ViewMenuArea);
                        var lastCheckedParallelMenuItem = _layoutElements.Find(x =>
                            x.Container == viewMenuArea.Name
                            && x.Type == (int)LayoutElementType.ViewMenuItem && x.IsChecked);
                        //*Hide ViewMenuAreas of LastChecked
                        HideViewMenuAreas(lastCheckedParallelMenuItem.Id);
                        CheckViewMenuItemAndUncheckParallelItems(menuItem.Id);
                        UpdateCustomizedViewMenu(menuItem.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                PopupMessage.PopupError(_basicInfoForException + ": " + GetType().FullName + ".ViewMenuItemClickHandler" + " Error: ctrlName='" + ctrlName + "'", ex.Message, GetAdditionalInfoForException());
            }
        }

        //##ContextMenuItemClickHandler
        protected void ContextMenuItemClickHandler(object sender, EventArgs e)
        {
            var ctrlName = "";
            var act = new ControlAction();
            try
            {
                var type = sender.GetType().ToString();
                if (type.ToLower().EndsWith("ListViewEx".ToLower()))
                {
                    //so+

                }
                else if (type.ToLower().EndsWith("ContextMenuStripEx".ToLower()))
                {
                    var cpnt = sender as ContextMenuStripEx;
                    ctrlName = (cpnt).Name;
                    act = cpnt.CurrentAction;
                }
                else if (type.ToLower().EndsWith("ToolStripSplitButtonEx".ToLower()))
                {
                    var cpnt = sender as ToolStripSplitButtonEx;
                    ctrlName = (cpnt).Name;
                    act = cpnt.CurrentAction;
                }
                else throw new ArgumentException("Control type didn't be considered!");

                Act(ctrlName, act.Action, act.DisplayName, true);
            }
            catch (Exception ex)
            {
                PopupMessage.PopupError(_basicInfoForException + ": " + GetType().FullName + "." + "ContextMenuItemClickHandler" + " Error: ctrlName='" + ctrlName + "'", ex.Message, GetAdditionalInfoForException());
            }
        }

        //##ControlEventHandler
        private void ControlEventHandler(object sender, EventArgs e)
        {
            var ctrlName = "";
            try
            {
                var ctrl = sender as Control;
                var type = sender.GetType().ToString();
                //to be improved
                if (type.ToLower().EndsWith("ToolStripMenuItem".ToLower())) ctrlName = (sender as ToolStripMenuItem).Name;
                else if (type.ToLower().EndsWith("ToolStripButton".ToLower())) ctrlName = (sender as ToolStripButton).Name;
                else
                {
                    ctrlName = ctrl.Name;
                }
                var action = "";
                var transDsplName = "";
                if (ctrlName.GetQtyOfIncludedChar('_') < 2) //menuitem==0;viewItem=1
                {
                    var item = _layoutElements.Find(x => x.Name == ctrlName);
                    action = item.Action;
                    transDsplName = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "", item.Name, _annexes, item.DisplayName);
                }
                else if (ctrlName.GetQtyOfIncludedChar('_') == 2)//zone item
                {
                    var item = _zonesItems.Find(x => x.Name == ctrlName);
                    action = item.Action;
                    transDsplName = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "ZoneItem", item.Name, _annexes, item.DisplayName);
                }

                if (!string.IsNullOrEmpty(action))
                {
                    Act(ctrlName, action, transDsplName, true);
                }

                var shadowTransactionElmts = _layoutElements.FindAll(x => x.Type == (int)LayoutElementType.FollowingTransactionItem & x.Remark == ctrlName
                    & !string.IsNullOrEmpty(x.Action));
                foreach (var shadowTransactionElmt in shadowTransactionElmts)
                {
                    action = shadowTransactionElmt.Action;
                    if (!string.IsNullOrEmpty(action))
                    {
                        transDsplName = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "ZoneItem", shadowTransactionElmt.Name, _annexes, shadowTransactionElmt.DisplayName);
                        Act(shadowTransactionElmt.Name, action, transDsplName, true);

                    }
                }
            }
            catch (Exception ex)
            {
                PopupMessage.PopupError(_basicInfoForException + ": " + GetType().FullName + "." + "ControlEventHandler" + " Error: ctrlName='" + ctrlName + "'", ex.Message, GetAdditionalInfoForException());
            }
        }

        //##ControlEventHandler1
        private void ControlEventHandler1(object sender, EventArgs e)
        {
            var ctrlName = "";
            try
            {
                var ctrl = sender as Control;
                var type = sender.GetType().ToString();
                //to be improved
                if (type.ToLower().EndsWith("ToolStripMenuItem".ToLower())) ctrlName = (sender as ToolStripMenuItem).Name;
                else if (type.ToLower().EndsWith("ToolStripButton".ToLower())) ctrlName = (sender as ToolStripButton).Name;
                else
                {
                    ctrlName = ctrl.Name;
                }
                var action = "";
                var transDsplName = "";
                if (ctrlName.GetQtyOfIncludedChar('_') == 2)//zone item
                {
                    var item = _zonesItems.Find(x => x.Name == ctrlName);
                    action = item.Action1;
                    transDsplName = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "ZoneItem", item.Name, _annexes, item.DisplayName);
                }

                if (!string.IsNullOrEmpty(action))
                {
                    Act(ctrlName, action, transDsplName, true);
                }

            }
            catch (Exception ex)
            {
                PopupMessage.PopupError(_basicInfoForException + ": " + GetType().FullName + ". ControlEventHandler Error: ctrlName='" + ctrlName + "'", ex.Message, GetAdditionalInfoForException());
            }
        }

        //##ControlHoverHandler
        private void ControlHoverHandler(object sender, EventArgs e)
        {
            var ctrlName = "";
            try
            {
                var type = sender.GetType().ToString();
                if (type.ToLower().EndsWith("PictureBox".ToLower()))
                {
                    var cpnt = sender as PictureBox;
                    var ctrlTag = cpnt.Tag.ToString();
                    ctrlName = cpnt.Name;
                    _pictureBoxToolTip.SetToolTip(cpnt, ctrlTag);
                }
                else
                {
                    throw new ArgumentException("Control type didn't be considered to trigger ControlHoverHandler!");
                }
            }
            catch (Exception ex)
            {
                PopupMessage.PopupError(_basicInfoForException + ": " + GetType().FullName + ".ControlHoverHandler Error: ctrlName='" + ctrlName + "'", ex.Message, GetAdditionalInfoForException());
            }
        }


        //#func
        //##LoadForm
        private void LoadForm()
        {
            try
            {
                MergeAbbrevAnnexes();
                MergePhraseAnnexes();
                if (_functionInitParamSet.FormType == FunctionFormType.MultipleView)
                {
                    //*initFunctionProcedures
                    var funcProcs = GetAndMergeFunctionProcedures();
                    InitZoneProcedures("", funcProcs);
                    //*getFunctionFormStyle
                    var formStyleCfgXmlPath = FileHelper.GetFilePath("\\FunctionFormStyle", _functionsDir + "\\" + _functionInitParamSet.FunctionCode);
                    var xmlMgr = new XmlHandler(formStyleCfgXmlPath);
                    _functionFormStyle = xmlMgr.ConvertToObject<FunctionFormStyle>();
                    if (_functionFormStyle == null)
                    {
                        throw new ArgumentException("_functionFormStyle can't be null!");
                    }

                    InitLayout(_functionInitParamSet.FormType);
                    ResizeRegion(_functionFormStyle.ResizeRegionParams);
                    if (_functionInitParamSet.SupportMultiCultures) InitPublicRegionComponent();

                    MergeViewItems("Public", "");
                    RenderView("Public");
                    AddViewStatus("Public", false);

                    //*Menu
                    _functionFormViewMenuMode = FunctionFormViewMenuMode.Simple;
                    if (_functionFormStyle.ViewMenuMode == 1)
                    {
                        _functionFormViewMenuMode = FunctionFormViewMenuMode.Customized;
                    }

                    _layoutElements.AddRange(GetMenuItems());
                    var viewMenuAreas = _layoutElements.FindAll(x => x.Type == ((int)LayoutElementType.ViewMenuArea));
                    if (viewMenuAreas.Count > 0)
                    {
                        if (CurrentViewMenuId != 0) //*if RefeshUI, CurrentViewMenuId !=0
                        {
                            _functionInitParamSet.StartViewMenuId = CurrentViewMenuId;
                        }
                        else
                        {
                            CurrentViewMenuId = _functionInitParamSet.StartViewMenuId < 0 ? 0 : _functionInitParamSet.StartViewMenuId;
                        }

                        if (_functionFormViewMenuMode == FunctionFormViewMenuMode.Customized)
                        {
                            if (CurrentViewMenuId != 0)
                            {
                                ResetDefaultViewMenu(_layoutElements, _functionInitParamSet.StartViewMenuId);
                            }
                            UpdateCustomizedViewMenu(0);
                            RenderToolMenuAreasAndItems();
                        }
                        else
                        {
                            RenderSimpleViewMenuAreaAndItems();
                            RenderToolMenuAreasAndItems();
                            var menuItem = new LayoutElement();
                            if (CurrentViewMenuId != 0) //*if RefeshUI, CurrentViewMenuId !=0
                                menuItem = _layoutElements.Find(x => x.Type == (int)LayoutElementType.ViewMenuItem & x.Id == CurrentViewMenuId);
                            else
                            {
                                var defaultViewMenuIdFlag = _layoutElements.Find(x => x.Type == (int)LayoutElementType.ViewMenuArea).DefaultViewMenuIdFlag;
                                menuItem = _layoutElements.Find(x => x.Type == (int)LayoutElementType.ViewMenuItem & x.Id == Convert.ToInt16(defaultViewMenuIdFlag));
                            }

                            MergeViewItems(menuItem.View, menuItem.DataSource);
                            RenderView(menuItem.View);
                            AddViewStatus(menuItem.View, true);
                            CurrentViewMenuId = menuItem.Id;
                        }
                    }
                    //*Menu
                }
                else
                {
                    var zoneStyleCfgXmlPath = _functionInitParamSet.ZoneLocationForNonMultiViewForm + "\\feature";
                    var xmlMgr = new XmlHandler(zoneStyleCfgXmlPath);
                    var zoneFeature = xmlMgr.ConvertToObject<ZoneFeature>();
                    if (zoneFeature == null)
                    {
                        throw new ArgumentException("zone Feature can't be null!");
                    }
                    _hasRunningStatusSectionForNonMutiViewForm = zoneFeature.ShowRunningStatusSection;
                    _zoneWidthForSingleViewForm = zoneFeature.Width;
                    _zoneHeightForSingleViewForm = zoneFeature.Height;

                    _functionFormStyle = new FunctionFormStyle();
                    _functionFormStyle.HasNoControlBox = zoneFeature.HasNoControlBox;
                    _functionFormStyle.TreatCloseBoxAsMinimizeBox = zoneFeature.TreatCloseBoxAsMinimizeBox;
                    _functionFormStyle.DrawIcon = zoneFeature.DrawIcon;
                    _functionFormStyle.IconUrl = zoneFeature.IconUrl;
                    _functionFormStyle.HasTray = zoneFeature.HasTray;
                    _functionFormStyle.TrayIconUrl = zoneFeature.TrayIconUrl;
                    _functionFormStyle.TrayDataSource = zoneFeature.TrayDataSource;

                    _functionFormStyle.ResizeRegionParams = "MainSectionMainDivision: 0,0; " + "MainMenu:0; ToolBarSection: 5,0,-1; NavigationSection: 0,0; ShortcutSection: 0,0; MainSectionLeftNavDivision: 0,0,0; MainSectionRightNavDivision: 0,0,0; MainSectionRightDivision: 0,0,0; RunningMessageSection: 0; " +
                                                            (_hasRunningStatusSectionForNonMutiViewForm ? "RunningStatusSection: 26,0;" : "RunningStatusSection: 0,0;") + "HorResizableDivisionStatus: none";
                    InitLayout(_functionInitParamSet.FormType);
                    ResizeRegion(_functionFormStyle.ResizeRegionParams);
                    _layoutElements = new List<LayoutElement>();
                    var areaLayoutElement = new LayoutElement()
                    {
                        Id = 10,
                        Name = "Public" + "_" + "Public",
                        Type = 120,
                        Container = "MainSectionMainDivisionMidRegion",
                        View = "Public",
                        DockType = 1,
                        DockOrder = "10",
                        Width = -1,
                        Height = -1,
                    };
                    _layoutElements.Add(areaLayoutElement);

                    var zoneName = (_functionInitParamSet.ZoneLocationForNonMultiViewForm).GetLastSeparatedString('\\');
                    var zoneLayoutElement = new LayoutElement()
                    {
                        Id = 1010,
                        Name = "Public" + "_" + zoneName,
                        Type = 260,
                        Container = "Public",
                        View = "Public",
                        Location = _functionInitParamSet.ZoneLocationForNonMultiViewForm,
                        InputVariables = _functionInitParamSet.InputZoneVariablesForNonMutiViewForm,
                        DockType = 5,
                        DockOrder = "1010",
                        Width = -1,
                        Height = -1,
                    };
                    _layoutElements.Add(zoneLayoutElement);
                    RenderView("Public");

                }

                if (!_functionInitParamSet.StartActions.IsNullOrEmpty())
                {
                    var actArray = _functionInitParamSet.StartActions.Split('^');
                    foreach (var act in actArray)
                    {
                        Act("", act, act, true);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".LoadForm Error: " + ex.Message);
                Application.Exit();
            }
        }

        //##ReadAppConfig
        //##GetAndMergeFunctionProcedures
        private List<ProcedureItem> GetAndMergeFunctionProcedures()
        {
            try
            {
                var funcProcedures = new List<ProcedureItem>();
                var funcProceduresTmp = new List<ProcedureItem>();
                var startParamsTxt = _functionInitParamSet.StartParams;
                if (!startParamsTxt.IsNullOrEmpty())
                {
                    var separator = startParamsTxt.GetParamSeparator();
                    var startParamsTxtArry = startParamsTxt.Split(separator);
                    var finputStr = "";
                    int ct = 0;
                    foreach (var str in startParamsTxtArry)
                    {
                        var val = str;
                        if (!str.IsNullOrEmpty())
                        {
                            var txt = ResolveConstants(str);
                            val = GetText(txt);
                        }

                        finputStr = ct == 0 ? val : finputStr + "^" + val;
                        ct = ct + 1;
                    }
                    var variableItem = new ProcedureItem();
                    variableItem.Name = "finput";
                    variableItem.Value = finputStr;
                    variableItem.Type = (int)ProcedureItemType.None;
                    funcProcedures.Add(variableItem);
                }

                var xmlPath = _functionsDir + "\\" + _functionInitParamSet.FunctionCode + "\\Procedures";
                if (XmlHelper.FileExists(xmlPath))
                {
                    var xmlMgr = new XmlHandler(xmlPath);
                    funcProceduresTmp = xmlMgr.ConvertToObject<List<ProcedureItem>>();
                }

                funcProceduresTmp = funcProceduresTmp.FindAll(x => x.TypeName == ProcedureItemType.Variable.ToString()
                                                                  | x.TypeName == ProcedureItemType.Action.ToString()
                                                                  | x.TypeName == ProcedureItemType.Break.ToString()
                                                                  | x.TypeName == ProcedureItemType.Exit.ToString());

                if (funcProceduresTmp.Count > 0)
                {
                    LayoutHelper.CheckProcedures(true, _functionInitParamSet.FunctionCode, funcProceduresTmp);
                    string annexesXmlPath = xmlPath + "Annexes";
                    var annexList = new List<Annex>();
                    if (File.Exists(annexesXmlPath + ".xml") | File.Exists(annexesXmlPath + ".exml"))
                    {
                        var xmlToReadForAnnexes = new XmlHandler(annexesXmlPath);
                        annexList = xmlToReadForAnnexes.ConvertToObject<List<Annex>>();
                    }

                    foreach (var proc in funcProceduresTmp)
                    {
                        LayoutHelper.SetProcedureType(proc);
                        proc.Value = string.IsNullOrEmpty(proc.Value) ? "" : proc.Value;
                        if (proc.GroupId < 0) proc.GroupId = 0;
                        var tempAnnexes = annexList.FindAll(x => x.MasterName == proc.Name);
                        if (tempAnnexes.Count > 0)
                        {
                            foreach (var annex in tempAnnexes)
                            {
                                annex.ClassName = "ProcedureItem";
                                annex.MasterName = proc.Name;
                                _annexes.Add(annex);
                            }
                        }
                    }
                    funcProcedures.AddRange(funcProceduresTmp);
                }

                if (funcProcedures.Count > 0)
                {
                    _procedures.AddRange(funcProcedures);
                }
                return funcProcedures;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetAndMergeFunctionProcedures Error: " + ex.Message);
            }
        }

        //##MergeFunctionAnnexes
        private void MergePhraseAnnexes()
        {
            try
            {
                var xmlPath = _appDir + "\\PhraseAnnexes";
                if (XmlHelper.FileExists(xmlPath))
                {
                    var xmlMgr = new XmlHandler(xmlPath);
                    var annexList = xmlMgr.ConvertToObject<List<Annex>>();
                    if (annexList.Count > 0)
                    {
                        foreach (var annex in annexList)
                        {
                            annex.ClassName = "Phrase";
                            _annexes.Add(annex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".MergePhraseAnnexes Error: " + ex.Message);
            }
        }

        //##MergeAbbrevAnnexes
        private void MergeAbbrevAnnexes()
        {
            try
            {
                var xmlPath = _appDir + "\\AbbrevAnnexes";
                if (XmlHelper.FileExists(xmlPath))
                {
                    var xmlMgr = new XmlHandler(xmlPath);
                    var annexList = xmlMgr.ConvertToObject<List<Annex>>();
                    if (annexList.Count > 0)
                    {
                        foreach (var annex in annexList)
                        {
                            annex.ClassName = "Abbrev";
                            _annexes.Add(annex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".MergeAbbrevAnnexes Error: " + ex.Message);
            }
        }

        //##Init
        //##InitLayout
        protected void InitLayout(FunctionFormType formType)
        {
            try
            {
                if (_functionInitParamSet.FormTitle.IsNullOrEmpty())
                {
                    var text = "";
                    if (!_functionInitParamSet.SupportMultiCultures)
                    {
                        text = AnnexHelper.GetText("Abbrev", _functionInitParamSet.FunctionCode, _annexes, AnnexTextType.DisplayName, "", GetAnnexMode.FirstAnnex);
                    }
                    else
                    {
                        text = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "Abbrev", _functionInitParamSet.FunctionCode, _annexes,
                            _functionInitParamSet.ArchitectureCode + "/" + _functionInitParamSet.ApplicationCode + "/" + _functionInitParamSet.FunctionCode);
                    }
                    Text = text;
                }
                else
                {
                    Text = _functionInitParamSet.FormTitle;
                }

                var funcOrZoneDir = _functionsDir + "\\" + _functionInitParamSet.FunctionCode;
                if (formType == FunctionFormType.SingleView)
                    funcOrZoneDir = _functionInitParamSet.ZoneLocationForNonMultiViewForm;
                _hasTray = _functionFormStyle.HasTray;
                var isFormModal = this.Modal;
                if (isFormModal) _hasTray = false;

                Resizable = false;
                DrawCationBackground = false;
                DrawIcon = false;
                WindowState = FormWindowState.Normal;

                MaximizeBox = _functionFormStyle.MaximizeBox ? true : false;
                MinimizeBox = _functionFormStyle.MinimizeBox ? true : false;
                ControlBox = !_functionFormStyle.HasNoControlBox;

                var startWindowState = _functionFormStyle.StartWindowState;
                if (startWindowState == "maximized")
                {
                    WindowState = FormWindowState.Maximized;
                }
                else if (startWindowState == "minimized")
                {
                    WindowState = FormWindowState.Minimized;
                }
                else
                {
                    WindowState = FormWindowState.Normal;

                }
                var ordinaryWindowStatus = _functionFormStyle.OrdinaryWindowStatus;
                if (ordinaryWindowStatus.IsNullOrEmpty()) _ordinaryWindowStatus = WindowState;
                else _ordinaryWindowStatus = ordinaryWindowStatus == "maximized" ? FormWindowState.Maximized : FormWindowState.Normal;

                if (formType == FunctionFormType.MultipleView)
                {
                    if (WindowState != FormWindowState.Maximized)
                    {
                        Width = _functionFormStyle.Width > -1 ? _functionFormStyle.Width : 1024;
                        Height = _functionFormStyle.Height > -1 ? _functionFormStyle.Height : 768;
                    }
                }
                else
                {
                    Width = _zoneWidthForSingleViewForm + 4;
                    Height = _zoneHeightForSingleViewForm + 35 + (_hasRunningStatusSectionForNonMutiViewForm ? 26 : 0);
                }



                DrawIcon = _functionFormStyle.DrawIcon;
                if (DrawIcon)
                {
                    var iconUrl = _functionFormStyle.IconUrl;
                    var iconDir = funcOrZoneDir + "\\icons";
                    iconUrl = FileHelper.GetFilePath(iconUrl, iconDir);
                    if (!iconUrl.IsNullOrEmpty() && System.IO.File.Exists(iconUrl))
                    {
                        var strm = File.Open(iconUrl, FileMode.Open, FileAccess.Read, FileShare.Read);
                        Icon = new Icon(strm);
                    }
                }

                if (_functionFormStyle.WindowRadius != 0)
                {
                    Radius = _functionFormStyle.WindowRadius;
                    RoundStyle = RoundStyle.All;
                }

                TreatCloseBoxAsMinimizeBox = (_functionFormStyle.TreatCloseBoxAsMinimizeBox ? true : false);
                if (_hasTray)
                {
                    ShowInTaskbar = false;
                    TreatCloseBoxAsMinimizeBox = true;
                    InitTray(funcOrZoneDir);
                }

                if (isFormModal)
                {
                    //*ShowInTaskbar = false; //this will cause bug: dialog popups then disappears. move to pos before form.ShowDialog(), becomes OK
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
                        Top = rect.Height > Height ? (rect.Height / 2 - Height / 2) / 3 : 10;
                    }
                }
                else
                {
                    //var rect = new Rectangle();
                    //rect = Screen.GetWorkingArea(this);
                    //Left = rect.Width / 2 - Width / 2;
                    //Top = _functionFormStyle.TopLocationY == -1 ? 20 : _functionFormStyle.TopLocationY;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".InitLayout Error:" + ex.Message);
            }
        }

        //##SetLayoutTextByCulture
        private void SetLayoutTextByCulture()
        {
            try
            {
                //*title
                if (_functionInitParamSet.FormTitle.IsNullOrEmpty())
                {
                    Text = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "Abbrev", _functionInitParamSet.FunctionCode, _annexes, _functionInitParamSet.ArchitectureCode + "/" + _functionInitParamSet.ApplicationCode + "/" + _functionInitParamSet.FunctionCode);
                }
                else
                {
                    Text = _functionInitParamSet.FormTitle;
                }


                //*tray
                if (_hasTray)
                {
                    _tray.Text = Text;
                    _trayContextMenuStrip.SetTextByCulture();
                }

                //*layout
                var elmts = _layoutElements.FindAll(x => x.Type == (int)LayoutElementType.ViewMenuItem
                            | x.Type == (int)LayoutElementType.DisplayOnlyItem | x.Type == (int)LayoutElementType.DisplayAndTransactionItem
                            );
                foreach (var elmt in elmts)
                {
                    var ctnName = elmt.Container;
                    var area = _layoutElements.Find(x => (x.Type == (int)LayoutElementType.ViewMenuArea | x.Type == (int)LayoutElementType.ToolMenuArea) & x.Name.EndsWith(ctnName) & x.IsRendered);
                    if (area != null)
                    {
                        if (area.ControlTypeName == "ToolStrip")
                        {
                            var areaControl = GetControl(area.Name);
                            var areaToolStrip = areaControl as ToolStrip;
                            var elmtControls = areaToolStrip.Items.Find(elmt.Name, true);

                            if (elmtControls != null && elmtControls.Length > 0)
                            {
                                elmtControls[0].Text = LayoutHelper.GetControlDisplayName(FunctionInitParamSet.SupportMultiCultures, "MenuItem", elmt.Name, _annexes, elmt.DisplayName);
                            }

                            if (!string.IsNullOrEmpty(elmt.Remark) & (elmt.ControlTypeName.ToLower().StartsWith("ImageToolButton".ToLower())))
                            {
                                elmtControls[0].Text = LayoutHelper.GetControlRemark(FunctionInitParamSet.SupportMultiCultures, "MenuItem", elmt.Name, _annexes, elmt.Remark);
                            }

                            if ((elmt.ControlTypeName.ToLower().Contains("ToolSplitButton".ToLower())))
                            {
                                var ctrl = elmtControls[0] as ToolStripSplitButtonEx;
                                ctrl.SetTextByCulture();
                                //to be improved
                            }
                        }
                        else if (area.ControlTypeName == "MenuStrip")
                        {
                            var areaControl = GetControl(area.Name);
                            var areaMenuStrip = areaControl as MenuStrip;
                            var elmtControls = areaMenuStrip.Items.Find(elmt.Name, true);

                            if (elmtControls != null && elmtControls.Length > 0)
                            {
                                elmtControls[0].Text = LayoutHelper.GetControlDisplayName(FunctionInitParamSet.SupportMultiCultures, "MenuItem", elmt.Name, _annexes, elmt.DisplayName);
                            }
                        }
                        else
                        {
                            var elmtControl = GetControl(elmt.Name);
                            elmtControl.Text = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "MenuItem", elmt.Name, _annexes, elmt.DisplayName);
                            if (!string.IsNullOrEmpty(elmt.Remark) & (elmt.ControlTypeName.ToLower().StartsWith("ImageButton".ToLower())))
                            {
                                elmtControl.Tag = LayoutHelper.GetControlRemark(_functionInitParamSet.SupportMultiCultures, "MenuItem", elmt.Name, _annexes, elmt.Remark);
                            }
                        }
                    }
                }

                //*zone
                var zoneItems = _zonesItems.FindAll(x => x.Type == (int)LayoutElementType.DisplayOnlyItem
                    | x.Type == (int)LayoutElementType.DisplayAndTransactionItem);
                foreach (var zoneItem in zoneItems)
                {
                    var zoneItemControl = GetControl(zoneItem.Name);
                    if (zoneItem.ControlTypeName == "Radio" | zoneItem.ControlTypeName == "CheckBox" | zoneItem.ControlTypeName.Contains("Button")
                        | zoneItem.ControlTypeName == "Label" | zoneItem.ControlTypeName == "TitleLabel" | zoneItem.ControlTypeName == "CommandLabel")
                    {
                        if (string.IsNullOrEmpty(zoneItem.DisplayName))
                        {
                            zoneItemControl.Text = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "ZoneItem", zoneItem.Name, _annexes, zoneItem.DisplayName);
                        }
                        else
                        {
                            var txt = ResolveStringByRefProcedureVariablesAndControls(zoneItem.DisplayName);
                            if (zoneItem.DisplayName.StartsWith("="))
                            {
                                zoneItemControl.Text = GetText(txt);
                            }
                            else
                            {
                                zoneItemControl.Text = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "ZoneItem", zoneItem.Name, _annexes, zoneItem.DisplayName);
                            }
                        }

                    }
                    else if (zoneItem.ControlTypeName == "PictureBox")
                    {
                        if (!string.IsNullOrEmpty(zoneItem.DisplayName))
                        {
                            var txt = ResolveStringByRefProcedureVariablesAndControls(zoneItem.DisplayName);
                            if (zoneItem.DisplayName.StartsWith("="))
                            {
                                zoneItemControl.Tag = GetText(txt);
                            }
                            else
                            {
                                zoneItemControl.Tag = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "ZoneItem", zoneItem.Name, _annexes, zoneItem.DisplayName);
                            }
                        }
                        else
                        {
                            zoneItemControl.Text = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "ZoneItem", zoneItem.Name, _annexes, zoneItem.DisplayName);
                        }
                    }
                    else if (zoneItem.ControlTypeName == "StatusLight")
                    {
                        var zoneItemStatusLight = zoneItemControl as StatusLight;
                        if (string.IsNullOrEmpty(zoneItem.DisplayName))
                        {
                            zoneItemStatusLight.Text = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "ZoneItem", zoneItem.Name, _annexes, zoneItem.DisplayName);
                        }
                        else
                        {
                            var txt = ResolveStringByRefProcedureVariablesAndControls(zoneItem.DisplayName);
                            if (zoneItem.DisplayName.StartsWith("="))
                            {
                                zoneItemStatusLight.Text = GetText(txt);
                            }
                            else
                            {
                                zoneItemStatusLight.Text = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "ZoneItem", zoneItem.Name, _annexes, zoneItem.DisplayName);
                            }
                        }

                    }
                    else if (zoneItem.ControlTypeName == "ScoreLight")
                    {
                        var zoneItemScoreLight = zoneItemControl as ScoreLight;
                        if (string.IsNullOrEmpty(zoneItem.DisplayName))
                        {
                            zoneItemScoreLight.Text = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "ZoneItem", zoneItem.Name, _annexes, zoneItem.DisplayName);
                        }
                        else
                        {
                            var txt = ResolveStringByRefProcedureVariablesAndControls(zoneItem.DisplayName);
                            if (zoneItem.DisplayName.StartsWith("="))
                            {
                                zoneItemScoreLight.Text = GetText(txt);
                            }
                            else
                            {
                                zoneItemScoreLight.Text = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "ZoneItem", zoneItem.Name, _annexes, zoneItem.DisplayName);
                            }
                        }
                    }
                    else if (zoneItem.ControlTypeName == "ComboBox")
                    {
                        var zoneItemComboBox = zoneItemControl as ComboBox;
                        var selectedIndex = zoneItemComboBox.SelectedIndex;
                        if (!String.IsNullOrEmpty(zoneItem.DataSource) & zoneItem.DataSource.ToLower().Contains("bylang"))
                        {
                            var dataSrcStr = zoneItem.DataSource;
                            if (dataSrcStr.StartsWith("="))
                            {
                                if (dataSrcStr.Contains("{") & dataSrcStr.Contains("}"))
                                {
                                    var valTxts = JsonHelper.ConvertToObject<List<ValueText>>(dataSrcStr);
                                    zoneItemComboBox.DataSource = valTxts;
                                    zoneItemComboBox.ValueMember = "Value";
                                    zoneItemComboBox.DisplayMember = "Text";
                                }
                            }
                            zoneItemComboBox.SelectedIndex = selectedIndex;
                        }
                    }
 

                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".ResetLayoutTextByCulture Error: " + ex.Message);
            }
        }

        //##InitTray
        private void InitTray(string funcOrZoneDir)
        {
            try
            {
                _tray = new NotifyIcon();
                var trayMenuItemsCfgXmlPath = FileHelper.GetFilePath(_functionFormStyle.TrayDataSource, funcOrZoneDir);
                _trayContextMenuStrip = new ContextMenuStripEx(_functionInitParamSet.SupportMultiCultures, trayMenuItemsCfgXmlPath);
                _trayContextMenuStrip.Name = "trayContextMenuStrip";
                _trayContextMenuStrip.OnItemClick += new System.EventHandler(ContextMenuItemClickHandler);

                _tray.Text = Text;
                _tray.Visible = true;
                _tray.ContextMenuStrip = _trayContextMenuStrip;
                _tray.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tray_MouseUp);

                var trayIconUrl = "";
                var iconDir = funcOrZoneDir + "\\icons";
                trayIconUrl = FileHelper.GetFilePath(_functionFormStyle.TrayIconUrl, iconDir);

                if (trayIconUrl.IsNullOrEmpty() | !System.IO.File.Exists(trayIconUrl)) trayIconUrl = _functionFormStyle.IconUrl;
                if (System.IO.File.Exists(trayIconUrl))
                {
                    var strm = File.Open(trayIconUrl, FileMode.Open, FileAccess.Read, FileShare.Read);
                    _tray.Icon = new Icon(strm);
                }
                else if (DrawIcon)
                {
                    _tray.Icon = Icon;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".InitTray Error:" + ex.Message);
            }
        }

        //##InitPublicRegionComponent
        private void InitPublicRegionComponent()
        {
            try
            {
                var resources = new System.ComponentModel.ComponentResourceManager(typeof(FunctionForm));

                if (CultureHelper.Cultures.Count > 1)
                {
                    var toolBarSectionPublicRegionSplitButtonCulture = new System.Windows.Forms.ToolStripSplitButton();
                    toolBarSectionPublicRegionSplitButtonCulture.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
                    toolBarSectionPublicRegionSplitButtonCulture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
                    toolBarSectionPublicRegionSplitButtonCulture.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
                    toolBarSectionPublicRegionSplitButtonCulture.ForeColor = System.Drawing.Color.Azure;
                    toolBarSectionPublicRegionSplitButtonCulture.Image = ((System.Drawing.Image)(resources.GetObject("ChooseLanguagePng")));
                    //toolBarSectionPublicRegionSplitButtonCulture.Image = ((System.Drawing.Image)(resources.GetObject("InputPng")));
                    toolBarSectionPublicRegionSplitButtonCulture.ImageTransparentColor = System.Drawing.Color.Magenta;
                    toolBarSectionPublicRegionSplitButtonCulture.Name = "ToolBarSectionPublicRegionToolStripSplitButtonCulture";
                    toolBarSectionPublicRegionSplitButtonCulture.Size = new System.Drawing.Size(97, 56);
                    toolBarSectionPublicRegionSplitButtonCulture.Text = CultureHelper.CurrentLanguageName;
                    toolBarSectionPublicRegionSplitButtonCulture.ToolTipText = WinformRes.ChooseLanguage;
                    foreach (var culture in CultureHelper.Cultures)
                    {
                        var toolBarSectionPublicRegionSplitButtonCultureItem = new System.Windows.Forms.ToolStripMenuItem();
                        var imgUrl = FileHelper.GetFilePath(culture.ImageUrl, _appDir + "\\Cultures");
                        toolBarSectionPublicRegionSplitButtonCultureItem.Image = ControlHelper.GetImage(imgUrl);
                        toolBarSectionPublicRegionSplitButtonCultureItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
                        toolBarSectionPublicRegionSplitButtonCultureItem.Name = "ToolBarSectionPublicRegionToolStripSplitButtonCultureItem" + "_" + culture.Name;
                        toolBarSectionPublicRegionSplitButtonCultureItem.Size = new System.Drawing.Size(124, 22);
                        toolBarSectionPublicRegionSplitButtonCultureItem.Tag = culture.Name;
                        toolBarSectionPublicRegionSplitButtonCultureItem.Text = culture.LanguageName;
                        toolBarSectionPublicRegionSplitButtonCultureItem.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                        toolBarSectionPublicRegionSplitButtonCultureItem.Click += new System.EventHandler(ToolBarSectionPublicRegionToolStripSplitButtonCultureItemClickHandler);
                        toolBarSectionPublicRegionSplitButtonCulture.DropDownItems.Add(toolBarSectionPublicRegionSplitButtonCultureItem);
                    }
                    this.ToolBarSectionPublicRegionToolStrip.Items.Add(toolBarSectionPublicRegionSplitButtonCulture);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".InitPublicRegionComponent Error:" + ex.Message);
            }
        }


        //##ResizeRegion
        private void ResizeRegion(string resizeStrs)
        {
            try
            {
                if (!string.IsNullOrEmpty(resizeStrs))
                {
                    var resizeStr = resizeStrs.GetStyleValue("MainMenu");
                    if (!resizeStr.IsNullOrEmpty())
                    {
                        var resizeStrArry = resizeStr.Split(',');
                        if (Convert.ToInt16(resizeStrArry[0]) > -1)
                            MainMenuSectionHeight = Convert.ToInt16(resizeStrArry[0]);
                        if (resizeStrArry.Length > 1)
                        {
                            if (Convert.ToInt16(resizeStrArry[1]) > -1)
                                MainMenuSectionLeftRegionWidth = Convert.ToInt16(resizeStrArry[1]);
                        }
                        if (resizeStrArry.Length > 2)
                        {
                            if (Convert.ToInt16(resizeStrArry[2]) > -1)
                                MainMenuSectionRightRegionWidth = Convert.ToInt16(resizeStrArry[2]);
                        }
                    }


                    resizeStr = resizeStrs.GetStyleValue("ToolBarSection");
                    if (!resizeStr.IsNullOrEmpty())
                    {
                        var resizeStrArry = resizeStr.Split(',');
                        if (Convert.ToInt16(resizeStrArry[0]) > -1)
                            ToolBarSectionHeight = Convert.ToInt16(resizeStrArry[0]);

                        if (resizeStrArry.Length > 1)
                        {
                            if (Convert.ToInt16(resizeStrArry[1]) > -1)
                                ToolBarSectionLeftRegionWidth = Convert.ToInt16(resizeStrArry[1]);
                        }

                        if (resizeStrArry.Length > 2)
                        {
                            if (Convert.ToInt16(resizeStrArry[2]) > -1)
                                ToolBarSectionCenterRegionWidth = Convert.ToInt16(resizeStrArry[2]);
                        }

                        if (resizeStrArry.Length > 3)
                        {
                            if (Convert.ToInt16(resizeStrArry[3]) > -1)
                                ToolBarSectionPublicRegionWidth = Convert.ToInt16(resizeStrArry[3]);
                        }
                    }

                    resizeStr = resizeStrs.GetStyleValue("NavigationSection");
                    if (!resizeStr.IsNullOrEmpty())
                    {
                        var resizeStrArry = resizeStr.Split(',');
                        if (Convert.ToInt16(resizeStrArry[0]) > -1)
                            NavigationSectionHeight = Convert.ToInt16(resizeStrArry[0]);

                        if (resizeStrArry.Length > 1)
                        {
                            if (Convert.ToInt16(resizeStrArry[1]) > -1)
                                NavigationSectionLeftRegionWidth = Convert.ToInt16(resizeStrArry[1]);
                        }
                        if (resizeStrArry.Length > 2)
                        {
                            if (Convert.ToInt16(resizeStrArry[2]) > -1)
                                NavigationSectionRightRegionWidth = Convert.ToInt16(resizeStrArry[2]);
                        }
                    }


                    resizeStr = resizeStrs.GetStyleValue("ShortcutSection");
                    if (!resizeStr.IsNullOrEmpty())
                    {
                        var resizeStrArry = resizeStr.Split(',');
                        if (resizeStrArry[0] != "-1")
                            ShortcutSectionHeight = Convert.ToInt16(resizeStrArry[0]);

                        if (resizeStrArry.Length > 1)
                        {
                            if (Convert.ToInt16(resizeStrArry[1]) > -1)
                                ShortcutSectionLeftRegionWidth = Convert.ToInt16(resizeStrArry[1]);
                        }
                        if (resizeStrArry.Length > 2)
                        {
                            if (Convert.ToInt16(resizeStrArry[2]) > -1)
                                ShortcutSectionRightRegionWidth = Convert.ToInt16(resizeStrArry[2]);
                        }
                    }


                    this.MainSection.Padding = new Padding(0);
                    resizeStr = resizeStrs.GetStyleValue("MainSectionLeftNavDivision");
                    if (!resizeStr.IsNullOrEmpty())
                    {
                        var resizeStrArry = resizeStr.Split(',');
                        if (Convert.ToInt16(resizeStrArry[0]) > -1)
                            MainSectionLeftNavDivisionWidth = Convert.ToInt16(resizeStrArry[0]);
                        if (resizeStrArry.Length > 1)
                        {
                            if (Convert.ToInt16(resizeStrArry[1]) > -1)
                                MainSectionLeftNavDivisionUpRegionHeight = Convert.ToInt16(resizeStrArry[1]);
                        }
                        if (resizeStrArry.Length > 2)
                        {
                            if (Convert.ToInt16(resizeStrArry[2]) > -1)
                                MainSectionLeftNavDivisionDownRegionHeight = Convert.ToInt16(resizeStrArry[2]);
                        }
                    }

                    resizeStr = resizeStrs.GetStyleValue("MainSectionRightNavDivision");
                    if (!resizeStr.IsNullOrEmpty())
                    {
                        var resizeStrArry = resizeStr.Split(',');
                        if (Convert.ToInt16(resizeStrArry[0]) > -1)
                            MainSectionRightNavDivisionWidth = Convert.ToInt16(resizeStrArry[0]);
                        if (resizeStrArry.Length > 1)
                        {
                            if (Convert.ToInt16(resizeStrArry[1]) > -1)
                                MainSectionRightNavDivisionUpRegionHeight = Convert.ToInt16(resizeStrArry[1]);
                        }
                        if (resizeStrArry.Length > 2)
                        {
                            if (Convert.ToInt16(resizeStrArry[2]) > -1)
                                MainSectionRightNavDivisionDownRegionHeight = Convert.ToInt16(resizeStrArry[2]);
                        }
                    }

                    resizeStr = resizeStrs.GetStyleValue("MainSectionMainDivision");
                    if (!resizeStr.IsNullOrEmpty())
                    {
                        var resizeStrArry = resizeStr.Split(',');
                        if (Convert.ToInt16(resizeStrArry[0]) > -1)
                            MainSectionMainDivisionUpRegionHeight = Convert.ToInt16(resizeStrArry[0]);
                        if (resizeStrArry.Length > 1)
                        {
                            if (Convert.ToInt16(resizeStrArry[1]) > -1)
                                MainSectionMainDivisionDownRegionHeight = Convert.ToInt16(resizeStrArry[1]);
                        }
                    }


                    resizeStr = resizeStrs.GetStyleValue("MainSectionRightDivision");
                    if (!resizeStr.IsNullOrEmpty())
                    {
                        var resizeStrArry = resizeStr.Split(',');
                        if (Convert.ToInt16(resizeStrArry[0]) > -1)
                            MainSectionRightDivisionWidth = Convert.ToInt16(resizeStrArry[0]);
                        if (resizeStrArry.Length > 1)
                        {
                            if (Convert.ToInt16(resizeStrArry[1]) > -1)
                                MainSectionRightDivisionUpRegionHeight = Convert.ToInt16(resizeStrArry[1]);
                        }
                        if (resizeStrArry.Length > 2)
                        {
                            if (Convert.ToInt16(resizeStrArry[2]) > -1)
                                MainSectionRightDivisionDownRegionHeight = Convert.ToInt16(resizeStrArry[2]);
                        }
                    }


                    resizeStr = resizeStrs.GetStyleValue("RunningMessageSection");
                    if (!resizeStr.IsNullOrEmpty())
                    {
                        RunningMessageSectionHeight = Convert.ToInt16(resizeStr);
                    }


                    resizeStr = resizeStrs.GetStyleValue("RunningStatusSection");
                    if (!resizeStr.IsNullOrEmpty())
                    {
                        var resizeStrArry = resizeStr.Split(',');
                        if (Convert.ToInt16(resizeStrArry[0]) > -1)
                            RunningStatusSectionHeight = Convert.ToInt16(resizeStrArry[0]);
                        if (resizeStrArry.Length > 1)
                        {
                            if (Convert.ToInt16(resizeStrArry[1]) > -1)
                                RunningStatusSectionBackgroundTaskRegionWidth = Convert.ToInt16(resizeStrArry[1]);
                        }
                    }

                    resizeStr = resizeStrs.GetStyleValue("HorResizableDivisionStatus");
                    if (resizeStr == "show")
                    {
                        HorizontalResizableDivisionStatus = ResizableDivisionStatus.Shown;
                    }
                    else if (resizeStr == "hide")
                    {
                        HorizontalResizableDivisionStatus = ResizableDivisionStatus.Hidden;
                    }
                    else
                    {
                        HorizontalResizableDivisionStatus = ResizableDivisionStatus.None;
                    }

                    InitFrameHorizontalResizableDivisionStatus();
                    ResizeFrameComponent();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".ResizeRegion Error: " + ex.Message);
            }
        }

        //#subfunc
        //##Menu
        //##GetMenuItems
        private List<LayoutElement> GetMenuItems()
        {
            try
            {
                var xmlPath = _functionsDir + "\\" + _functionInitParamSet.FunctionCode + "\\menuItems";
                var xmlMgr = new XmlHandler(xmlPath);
                var tmpMenuItems = xmlMgr.ConvertToObject<List<LayoutElement>>().ToList();
                foreach (var elmt in tmpMenuItems)
                {
                    elmt.Location = _functionsDir + "\\" + _functionInitParamSet.FunctionCode;
                    if (elmt.DockOrder.IsNullOrEmpty()) elmt.DockOrder = elmt.Id.ToString();
                    if (elmt.InvalidFlag.IsNullOrEmpty()) elmt.InvalidFlag = "false";
                    if (elmt.InvalidFlag.StartsWith("="))
                    {
                        var txt = ResolveStringByRefProcedureVariablesAndControls((elmt.InvalidFlag));
                        elmt.InvalidFlag = GetText(txt).ToLower();
                    }
                }

                var menuItems = tmpMenuItems.Where(x =>
                  (x.InvalidFlag.ToLower() == "false" | x.InvalidFlag.ToLower() == "0") & (
                    x.TypeName == LayoutElementType.ViewMenuArea.ToString()
                  | x.TypeName == LayoutElementType.ViewMenuItem.ToString()
                  | x.TypeName == LayoutElementType.DisplayAndTransactionItem.ToString()
                  | x.TypeName == LayoutElementType.TransactionOnlyItem.ToString()
                  | x.TypeName == LayoutElementType.DisplayOnlyItem.ToString())
                  | x.TypeName == LayoutElementType.ToolMenuArea.ToString()
                    )
                  .ToList();


                if (menuItems.Count > 0)
                {
                    foreach (var elmt in menuItems)
                    {
                        LayoutHelper.SetLayoutElementType(elmt);
                    }

                    LayoutHelper.CheckMenuItems(_functionFormViewMenuMode, menuItems);
                    var annexXmlPath = _functionsDir + "\\" + _functionInitParamSet.FunctionCode + "\\menuItemsAnnexes";
                    var xmlToReadForAnnexes = new XmlHandler(annexXmlPath);
                    var annexList = xmlToReadForAnnexes.ConvertToObject<List<Annex>>();
                    var viewMenuAreaName = "";
                    if (_functionFormViewMenuMode == FunctionFormViewMenuMode.Simple)
                    {
                        var viewMenuArea = menuItems.Find(x => x.Type == (int)LayoutElementType.ViewMenuArea);
                        if (viewMenuArea == null) throw new ArgumentException("viewMenuArea can't be null!");
                        viewMenuArea.ParentViewMenuId = 0;
                        viewMenuAreaName = viewMenuArea.Name;
                    }

                    foreach (var elmt in menuItems)
                    {
                        if (elmt.Type == (int)LayoutElementType.ViewMenuItem)
                        {
                            if (_functionFormViewMenuMode == FunctionFormViewMenuMode.Simple
                                & elmt.Type == (int)LayoutElementType.ViewMenuItem)
                            {
                                if (elmt.Container == viewMenuAreaName) elmt.ParentViewMenuId = -1;
                            }

                            var tempAnnexes = annexList.FindAll(x => x.MasterName == elmt.Name);
                            if (tempAnnexes.Count > 0)
                            {
                                foreach (var annex in tempAnnexes)
                                {
                                    annex.ClassName = "MenuItem";
                                    annex.MasterName = elmt.Name;
                                    _annexes.Add(annex);
                                }
                            }

                            //*view
                            var txt = ResolveStringByRefProcedureVariables(elmt.View, _procedures);
                            if (!elmt.View.IsNullOrEmpty()) elmt.View = GetText(txt);
                        }

                        else if (elmt.Type == (int)LayoutElementType.DisplayAndTransactionItem | elmt.Type == (int)LayoutElementType.DisplayOnlyItem)
                        {
                            var tempAnnexes = annexList.FindAll(x => x.MasterName == elmt.Name);
                            if (tempAnnexes.Count > 0)
                            {
                                foreach (var annex in tempAnnexes)
                                {
                                    annex.ClassName = "MenuItem";
                                    annex.MasterName = elmt.Name;
                                    _annexes.Add(annex);
                                }
                            }
                        }
                        else if (elmt.Type == (int)LayoutElementType.TransactionOnlyItem)
                        {
                            var tempAnnexes = annexList.FindAll(x => x.MasterName == elmt.Name);
                            if (tempAnnexes.Count > 0)
                            {
                                foreach (var annex in tempAnnexes)
                                {
                                    annex.ClassName = "MenuItem";
                                    annex.MasterName = elmt.Name;
                                    _annexes.Add(annex);
                                }
                            }
                        }
                        elmt.IsRendered = false;
                        elmt.IsChecked = false;
                    }
                    return menuItems;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetMenuItems Error: " + ex.Message);
            }
        }

        //##UpdateCustomizedViewMenu
        protected void UpdateCustomizedViewMenu(int menuId)
        {
            try
            {
                if (_layoutElements == null) return;

                LayoutElement subMenuArea = _layoutElements.Find(x => x.ParentViewMenuId == menuId && x.Type == (int)LayoutElementType.ViewMenuArea);
                if (subMenuArea == null) //last level
                {
                    var menuItem = _layoutElements.Find(x => x.Id == menuId && x.Type == (int)LayoutElementType.ViewMenuItem);

                    //ResizeRegion
                    var lastCheckedMenuItem = _layoutElements.Find(x => x.Id == CurrentViewMenuId && x.Type == (int)LayoutElementType.ViewMenuItem);
                    if (lastCheckedMenuItem != null)
                    {
                        var resizeRegionParams = menuItem.ResizeRegionParams.IsNullOrEmpty()
                             ? _functionFormStyle.ResizeRegionParams
                             : menuItem.ResizeRegionParams;
                        var lastCheckedResizeRegionParams = lastCheckedMenuItem.ResizeRegionParams.IsNullOrEmpty()
                                                     ? _functionFormStyle.ResizeRegionParams
                                                     : lastCheckedMenuItem.ResizeRegionParams;
                        if (resizeRegionParams != lastCheckedResizeRegionParams)
                        {
                            ResizeRegion(resizeRegionParams);
                        }
                    }

                    CurrentViewMenuId = menuItem.Id;
                    CurrentViewMenuName = menuItem.Name;
                    CurrentViewName = menuItem.View;
                    var renderedView = _renderedViewStatuses.Find(x => x.Name == menuItem.View);
                    if (renderedView != null)//rendered
                    {
                        HideLastCheckedViewThenShowCurrentView(menuItem.View);
                        SetViewCheckedAndUncheckLastView(menuItem.View);
                    }
                    else if (!menuItem.View.IsNullOrEmpty())//not rendered and view is not empty
                    {
                        HideLastCheckedView();
                        MergeViewItems(menuItem.View, menuItem.DataSource);
                        RenderView(menuItem.View);
                        UncheckLastCheckedView();
                        AddViewStatus(menuItem.View, true);
                    }
                    else//not rendered but view is empty
                    {
                        throw new ArgumentException("View can't be empty! ");
                    }
                }
                else //not last level, and has sub
                {
                    var subMenuItems = _layoutElements.FindAll(x => x.Container == subMenuArea.Name && x.Type == (int)LayoutElementType.ViewMenuItem);
                    if (subMenuItems.Count < 1) return;
                    if (subMenuArea.IsRendered)
                    {
                        if (subMenuArea.ParentViewMenuId != 0)
                        {
                            ShowViewMenuArea(subMenuArea);
                        }
                    }
                    else
                    {
                        RenderCustomizedViewOrToolMenuAreaAndItems(subMenuArea);
                    }

                    var checkedSubMenuItem = subMenuItems.Find(x => x.IsChecked);
                    if (checkedSubMenuItem != null)
                    {
                        var checkedSubMenuItemId = checkedSubMenuItem.Id;
                        UpdateCustomizedViewMenu(checkedSubMenuItemId);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".UpdateCustomizedViewMenu Error: menuId=" + menuId + ", " + ex.Message);
            }
        }

        //##RenderToolMenuAreasAndItems
        protected void RenderToolMenuAreasAndItems()
        {
            try
            {
                var toolMenuAreas = _layoutElements.FindAll(x => x.Type == (int)LayoutElementType.ToolMenuArea && x.ParentViewMenuId == 0);
                foreach (var toolMenuArea in toolMenuAreas)
                {
                    var toolMenuItems = _layoutElements.FindAll(x => x.Container == toolMenuArea.Name
                                                             && (x.Type == (int)LayoutElementType.DisplayAndTransactionItem
                                                               | x.Type == (int)LayoutElementType.DisplayOnlyItem)
                                                               );
                    if (toolMenuItems.Count < 1) return;
                    RenderCustomizedViewOrToolMenuAreaAndItems(toolMenuArea);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RenderToolMenuAreaAndItems Error: " + ex.Message);
            }
        }

        //#RenderSimpleViewMenuAreaAndItems
        protected void RenderSimpleViewMenuAreaAndItems()
        {
            try
            {
                if (_layoutElements == null) return;
                var viewMenuArea = _layoutElements.Find(x => x.ParentViewMenuId == 0 && x.Type == (int)LayoutElementType.ViewMenuArea);
                if (viewMenuArea != null)
                {
                    //*top level menu items
                    var topLevelViewMenuItems = _layoutElements.FindAll(x =>
                        x.Container == viewMenuArea.Name && x.Type == (int)LayoutElementType.ViewMenuItem && x.ParentViewMenuId < 0);
                    if (topLevelViewMenuItems.Count < 1) return;

                    if ((viewMenuArea.Type == (int)LayoutElementType.ViewMenuArea) &&
                        viewMenuArea.ControlTypeName.ToLower() == "MenuStrip".ToLower())
                    {
                        var viewMenuAreaControl = new MenuStrip();
                        ControlHelper.SetControlBackColor(viewMenuAreaControl, viewMenuArea.StyleText);

                        var regionControl = new Control();
                        try
                        {
                            regionControl = this.Controls.Find(viewMenuArea.Container, true)[0];
                        }
                        catch (Exception ex)
                        {
                            throw new ArgumentException("viewMenuArea.Container doesn't exist! ctrlName=" + viewMenuArea.Container);
                        }

                        var viewMenuAreaWidth = viewMenuArea.Width == -1 ? regionControl.Width : viewMenuArea.Width;
                        var viewMenuAreaHeight = viewMenuArea.Height == -1 ? regionControl.Height : viewMenuArea.Height;
                        ControlHelper.SetControlDockStyleAndLocationAndSize(viewMenuAreaControl, viewMenuArea.DockType,
                            viewMenuAreaWidth, viewMenuAreaHeight, viewMenuArea.OffsetOrPositionX, viewMenuArea.OffsetOrPositionY);

                        viewMenuAreaControl.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
                        if (viewMenuArea.ImageWidth != -1 && viewMenuArea.ImageHeight != -1)
                            viewMenuAreaControl.ImageScalingSize = new Size(viewMenuArea.ImageWidth, viewMenuArea.ImageHeight);
                        viewMenuAreaControl.AutoSize = false;
                        var autoSizeStr = viewMenuArea.StyleText.GetStyleValue("AutoSize");
                        if (!string.IsNullOrEmpty(autoSizeStr) && autoSizeStr.ToLower() == "true")
                        {
                            viewMenuAreaControl.AutoSize = true;
                        }

                        viewMenuAreaControl.Name = viewMenuArea.Name;
                        viewMenuAreaControl.Tag = viewMenuArea.ParentViewMenuId == 0 ?
                            "$" + viewMenuArea.Name : viewMenuArea.Name;
                        regionControl.Controls.Add(viewMenuAreaControl);
                        //viewMenuArea.IsChecked = true;
                        viewMenuArea.IsRendered = true;
                        foreach (var topLevelViewMenuItem in topLevelViewMenuItems)
                        {
                            RenderSimpleViewMenuItem(viewMenuAreaControl, null, topLevelViewMenuItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RenderSimpleViewMenuAreaAndItems Error: " + ex.Message);
            }
        }

        //##RenderSimpleViewMenuItem
        protected void RenderSimpleViewMenuItem(MenuStrip parentAreaCtrl, ToolStripMenuItem parentItemCtrl, LayoutElement viewMenuItem)
        {
            var itemNameForEx = "";
            try
            {

                itemNameForEx = viewMenuItem.Name;

                //*Visible, Enabled
                bool isItemVisible = true;
                var itemInvisibleFlag = viewMenuItem.InvisibleFlag;
                if (string.IsNullOrEmpty(itemInvisibleFlag)) itemInvisibleFlag = "false";
                else
                {
                    viewMenuItem.InvisibleFlag = ResolveConstants(viewMenuItem.InvisibleFlag);
                    var txt = ResolveStringByRefProcedureVariables(viewMenuItem.InvisibleFlag);
                    itemInvisibleFlag = GetText(txt);
                }
                isItemVisible = (itemInvisibleFlag == "false" | itemInvisibleFlag == "0") ? true : false;

                bool isItemEnabled = true;
                var itemDisabledFlag = viewMenuItem.DisabledFlag;
                if (string.IsNullOrEmpty(itemDisabledFlag)) itemDisabledFlag = "false";
                else
                {
                    viewMenuItem.DisabledFlag = ResolveConstants(viewMenuItem.DisabledFlag);
                    var txt = ResolveStringByRefProcedureVariables(viewMenuItem.DisabledFlag);
                    itemDisabledFlag = GetText(txt);

                }
                isItemEnabled = (itemDisabledFlag == "false" | itemDisabledFlag == "0") ? true : false;

                if (viewMenuItem.ControlTypeName.ToLower().Contains("ToolStripMenuItem".ToLower()))
                {
                    var viewMenuItemControl = new ToolStripMenuItem();

                    //*image
                    if (viewMenuItem.ControlTypeName.ToLower().Contains("image") & !string.IsNullOrEmpty(viewMenuItem.ImageUrl))
                    {
                        var imgUrl = FileHelper.GetFilePath(viewMenuItem.ImageUrl, viewMenuItem.Location);
                        viewMenuItemControl.Image = ControlHelper.GetImage(imgUrl);
                    }

                    //*imagetext relation
                    if (viewMenuItem.ControlTypeName.EndsWith("V")) viewMenuItemControl.TextImageRelation = TextImageRelation.ImageAboveText;
                    else if (viewMenuItem.ControlTypeName.EndsWith("H"))
                    {
                        viewMenuItemControl.TextImageRelation = TextImageRelation.ImageBeforeText;
                    }
                    //*dock, size,offset
                    if (viewMenuItem.DockType == (int)ControlDockType.Right)
                    {
                        viewMenuItemControl.Alignment = ToolStripItemAlignment.Right;
                    }
                    if (viewMenuItem.Width != -1)
                    {
                        viewMenuItemControl.AutoSize = false;
                        viewMenuItemControl.Width = viewMenuItem.Width;
                    }
                    if (viewMenuItem.Height != -1)
                    {
                        viewMenuItemControl.AutoSize = false;
                        viewMenuItemControl.Height = viewMenuItem.Height;
                    }
                    if (viewMenuItem.ImageWidth != -1 && viewMenuItem.ImageHeight != -1) viewMenuItemControl.ImageScaling = ToolStripItemImageScaling.None;

                    //*name
                    viewMenuItemControl.Name = viewMenuItem.Name;
                    viewMenuItemControl.Text = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "MenuItem", viewMenuItem.Name, _annexes, viewMenuItem.DisplayName);
                    //*displayname, remark
                    viewMenuItemControl.AutoToolTip = false;
                    if (!string.IsNullOrEmpty(viewMenuItem.Remark))
                    {
                        viewMenuItemControl.AutoToolTip = true;
                        viewMenuItemControl.Text = LayoutHelper.GetControlRemark(_functionInitParamSet.SupportMultiCultures, "MenuItem", viewMenuItem.Name, _annexes, viewMenuItem.Remark);
                    }

                    //*font
                    if (!viewMenuItem.StyleText.IsNullOrEmpty())
                    {
                        ControlHelper.SetToolStripMenuItemStyleByText(viewMenuItemControl, viewMenuItem.StyleText);
                    }

                    //*enable, visible
                    if (!isItemEnabled)
                    {
                        viewMenuItemControl.Enabled = false;
                    }
                    if (!isItemVisible)
                    {
                        viewMenuItemControl.Visible = false;
                    }

                    if (parentAreaCtrl != null)
                    {
                        parentAreaCtrl.Items.Add(viewMenuItemControl);
                    }
                    else
                    {
                        parentItemCtrl.DropDownItems.Add(viewMenuItemControl);
                    }

                    //*event
                    if (viewMenuItem.Type == (int)LayoutElementType.DisplayAndTransactionItem)
                    {
                        viewMenuItemControl.Click += new System.EventHandler(ControlEventHandler);
                    }
                    else if (viewMenuItem.Type == (int)LayoutElementType.ViewMenuItem && !viewMenuItem.View.IsNullOrEmpty())
                    {
                        viewMenuItemControl.Click += new System.EventHandler(ViewMenuItemClickHandler);
                    }

                    var subViewMenuItems = _layoutElements.FindAll(x => x.ParentViewMenuId == viewMenuItem.Id);
                    foreach (var subViewMenuItem in subViewMenuItems)
                    {
                        RenderSimpleViewMenuItem(null, viewMenuItemControl, subViewMenuItem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RenderSimpleViewMenu Error: itemName=" + itemNameForEx + "; " + ex.Message);
            }
        }



        //##RenderToolOrCustomizedViewMenuAreaAndItems
        private void RenderCustomizedViewOrToolMenuAreaAndItems(LayoutElement viewMenuArea)
        {
            var areaNameForEx = viewMenuArea.Name;
            var itemNameForEx = "";
            try
            {
                if ((viewMenuArea.Type == (int)LayoutElementType.ViewMenuArea | viewMenuArea.Type == (int)LayoutElementType.ToolMenuArea)
                    && viewMenuArea.ControlTypeName.ToLower() == "ToolStrip".ToLower())
                {
                    var viewMenuAreaControl = new ToolStrip();
                    ControlHelper.SetControlBackColor(viewMenuAreaControl, viewMenuArea.StyleText);

                    var regionControl = new Control();
                    try
                    {
                        regionControl = this.Controls.Find(viewMenuArea.Container, true)[0];
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("viewMenuArea.Container doesn't exist! ctrlName=" + viewMenuArea.Container);
                    }

                    var viewMenuAreaWidth = viewMenuArea.Width == -1 ? regionControl.Width : viewMenuArea.Width;
                    var viewMenuAreaHeight = viewMenuArea.Height == -1 ? regionControl.Height : viewMenuArea.Height;
                    ControlHelper.SetControlDockStyleAndLocationAndSize(viewMenuAreaControl, viewMenuArea.DockType, viewMenuAreaWidth, viewMenuAreaHeight, viewMenuArea.OffsetOrPositionX, viewMenuArea.OffsetOrPositionY);
                    viewMenuAreaControl.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
                    if (viewMenuArea.ImageWidth != -1 && viewMenuArea.ImageHeight != -1)
                        viewMenuAreaControl.ImageScalingSize = new Size(viewMenuArea.ImageWidth, viewMenuArea.ImageHeight);
                    viewMenuAreaControl.AutoSize = false;
                    var autoSizeStr = viewMenuArea.StyleText.GetStyleValue("AutoSize");
                    if (!string.IsNullOrEmpty(autoSizeStr) && autoSizeStr.ToLower() == "true")
                    {
                        viewMenuAreaControl.AutoSize = true;
                    }

                    viewMenuAreaControl.Name = viewMenuArea.Name;
                    viewMenuAreaControl.Tag = viewMenuArea.ParentViewMenuId == 0 ? "$" + viewMenuArea.Name : viewMenuArea.Name;
                    regionControl.Controls.Add(viewMenuAreaControl);
                    viewMenuArea.IsChecked = true;
                    viewMenuArea.IsRendered = true;
                    var viewMenuItems = _layoutElements.Where(x => x.Container == viewMenuArea.Name && (x.Type == (int)LayoutElementType.ViewMenuItem
                          | x.Type == (int)LayoutElementType.DisplayAndTransactionItem | x.Type == (int)LayoutElementType.DisplayOnlyItem)).OrderBy(x => x.DockOrder).ToList();

                    var defMenuIdStr = viewMenuArea.DefaultViewMenuIdFlag;
                    if (string.IsNullOrEmpty(defMenuIdStr) && viewMenuItems.Count > 0)
                    {
                        defMenuIdStr = viewMenuItems.FirstOrDefault().Id.ToString();
                    }


                    foreach (var viewMenuItem in viewMenuItems)
                    {
                        itemNameForEx = viewMenuItem.Name;

                        //*Visible, Enabled
                        bool isItemVisible = true;
                        var itemInvisibleFlag = viewMenuItem.InvisibleFlag;
                        if (string.IsNullOrEmpty(itemInvisibleFlag)) itemInvisibleFlag = "false";
                        else
                        {
                            viewMenuItem.InvisibleFlag = ResolveConstants(viewMenuItem.InvisibleFlag);
                            var txt = ResolveStringByRefProcedureVariables(viewMenuItem.InvisibleFlag);
                            itemInvisibleFlag = GetText(txt);
                        }
                        isItemVisible = (itemInvisibleFlag == "false" | itemInvisibleFlag == "0") ? true : false;

                        bool isItemEnabled = true;
                        var itemDisabledFlag = viewMenuItem.DisabledFlag;
                        if (string.IsNullOrEmpty(itemDisabledFlag)) itemDisabledFlag = "false";
                        else
                        {
                            viewMenuItem.DisabledFlag = ResolveConstants(viewMenuItem.DisabledFlag);
                            var txt = ResolveStringByRefProcedureVariables(viewMenuItem.DisabledFlag);
                            itemDisabledFlag = GetText(txt);

                        }
                        isItemEnabled = (itemDisabledFlag == "false" | itemDisabledFlag == "0") ? true : false;

                        if (viewMenuItem.ControlTypeName.ToLower().Contains("ToolSplitButton".ToLower()))
                        {
                            var xmlDir = FileHelper.GetFilePath(viewMenuItem.DataSource, viewMenuItem.Location);
                            var xmlFile = xmlDir + "\\Ui";
                            var viewMenuItemControl = new ToolStripSplitButtonEx(_functionInitParamSet.SupportMultiCultures, xmlFile);

                            if (viewMenuItem.ControlTypeName.ToLower() == "ImageToolSplitButton".ToLower())
                            {
                                viewMenuItemControl.DisplayStyle = ToolStripItemDisplayStyle.Image;
                            }
                            //*image
                            if (viewMenuItem.ControlTypeName.ToLower().Contains("image") & !string.IsNullOrEmpty(viewMenuItem.ImageUrl))
                            {
                                var imgUrl = FileHelper.GetFilePath(viewMenuItem.ImageUrl, viewMenuItem.Location);
                                viewMenuItemControl.Image = ControlHelper.GetImage(imgUrl);
                            }
                            //*imagetext relation
                            if (viewMenuItem.ControlTypeName.EndsWith("V")) viewMenuItemControl.TextImageRelation = TextImageRelation.ImageAboveText;
                            else if (viewMenuItem.ControlTypeName.EndsWith("H"))
                            {
                                viewMenuItemControl.TextImageRelation = TextImageRelation.ImageBeforeText;
                            }
                            //*dock, size,offset
                            if (viewMenuItem.DockType == (int)ControlDockType.Right)
                            {
                                viewMenuItemControl.Alignment = ToolStripItemAlignment.Right;
                            }
                            if (viewMenuItem.Width != -1)
                            {
                                viewMenuItemControl.AutoSize = false;
                                viewMenuItemControl.Width = viewMenuItem.Width;
                            }
                            if (viewMenuItem.Height != -1)
                            {
                                viewMenuItemControl.AutoSize = false;
                                viewMenuItemControl.Height = viewMenuItem.Height;
                            }
                            if (viewMenuItem.ImageWidth != -1 && viewMenuItem.ImageHeight != -1) viewMenuItemControl.ImageScaling = ToolStripItemImageScaling.None;
                            if (viewMenuItem.OffsetOrPositionX != -1)
                            {
                                var subItemOffsetControl = new ToolStripLabel();
                                subItemOffsetControl.AutoSize = false;
                                subItemOffsetControl.Width = viewMenuItem.OffsetOrPositionX;
                                if (viewMenuItem.DockType == (int)ControlDockType.Right)
                                {
                                    viewMenuItemControl.Alignment = ToolStripItemAlignment.Right;
                                    subItemOffsetControl.Alignment = ToolStripItemAlignment.Right;
                                }
                                viewMenuAreaControl.Items.AddRange(new ToolStripItem[] { subItemOffsetControl });
                            }

                            //*name
                            viewMenuItemControl.Name = viewMenuItem.Name;
                            //*displayname, remark
                            viewMenuItemControl.Text = LayoutHelper.GetControlDisplayName(FunctionInitParamSet.SupportMultiCultures, "MenuItem", viewMenuItem.Name, _annexes, viewMenuItem.DisplayName);
                            viewMenuItemControl.AutoToolTip = false;
                            if (!string.IsNullOrEmpty(viewMenuItem.Remark))
                            {
                                viewMenuItemControl.AutoToolTip = true;
                                viewMenuItemControl.Text = LayoutHelper.GetControlRemark(FunctionInitParamSet.SupportMultiCultures, "MenuItem", viewMenuItem.Name, _annexes, viewMenuItem.Remark);
                            }
                            //*font
                            if (viewMenuArea.ParentViewMenuId == 0)
                            {
                                viewMenuItemControl.ForeColor = StyleSheet.CaptionTextColor;
                                viewMenuItemControl.Font = new Font("", 8, FontStyle.Bold);
                            }
                            //*enable, visible
                            if (!isItemEnabled)
                            {
                                viewMenuItemControl.Enabled = false;
                            }
                            if (!isItemVisible)
                            {
                                viewMenuItemControl.Visible = false;
                            }

                            viewMenuAreaControl.Items.AddRange(new ToolStripItem[] { viewMenuItemControl });

                            //*event
                            viewMenuItemControl.OnMenuItemClick += new System.EventHandler(ContextMenuItemClickHandler);
                        }
                        else if (viewMenuItem.ControlTypeName.ToLower().Contains("ToolButton".ToLower()))
                        {
                            var viewMenuItemControl = new ToolStripButton();
                            if (viewMenuItem.ControlTypeName.ToLower() == "ImageToolButton".ToLower())
                            {
                                viewMenuItemControl.DisplayStyle = ToolStripItemDisplayStyle.Image;
                            }
                            //*image
                            if (viewMenuItem.ControlTypeName.ToLower().Contains("image") & !string.IsNullOrEmpty(viewMenuItem.ImageUrl))
                            {
                                var imgUrl = FileHelper.GetFilePath(viewMenuItem.ImageUrl, viewMenuItem.Location);
                                viewMenuItemControl.Image = ControlHelper.GetImage(imgUrl);
                            }
                            //*imagetext relation
                            if (viewMenuItem.ControlTypeName.EndsWith("V")) viewMenuItemControl.TextImageRelation = TextImageRelation.ImageAboveText;
                            else if (viewMenuItem.ControlTypeName.EndsWith("H"))
                            {
                                viewMenuItemControl.TextImageRelation = TextImageRelation.ImageBeforeText;
                            }
                            //*dock, size,offset
                            if (viewMenuItem.DockType == (int)ControlDockType.Right)
                            {
                                viewMenuItemControl.Alignment = ToolStripItemAlignment.Right;
                            }
                            if (viewMenuItem.Width != -1)
                            {
                                viewMenuItemControl.AutoSize = false;
                                viewMenuItemControl.Width = viewMenuItem.Width;
                            }
                            if (viewMenuItem.Height != -1)
                            {
                                viewMenuItemControl.AutoSize = false;
                                viewMenuItemControl.Height = viewMenuItem.Height;
                            }
                            if (viewMenuItem.ImageWidth != -1 && viewMenuItem.ImageHeight != -1) viewMenuItemControl.ImageScaling = ToolStripItemImageScaling.None;
                            if (viewMenuItem.OffsetOrPositionX != -1)
                            {
                                var subItemOffsetControl = new ToolStripLabel();
                                subItemOffsetControl.AutoSize = false;
                                subItemOffsetControl.Width = viewMenuItem.OffsetOrPositionX;
                                if (viewMenuItem.DockType == (int)ControlDockType.Right)
                                {
                                    viewMenuItemControl.Alignment = ToolStripItemAlignment.Right;
                                    subItemOffsetControl.Alignment = ToolStripItemAlignment.Right;
                                }
                                viewMenuAreaControl.Items.AddRange(new ToolStripItem[] { subItemOffsetControl });
                            }

                            //*name
                            viewMenuItemControl.Name = viewMenuItem.Name;
                            viewMenuItemControl.Text = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "MenuItem", viewMenuItem.Name, _annexes, viewMenuItem.DisplayName);
                            //*displayname, remark
                            viewMenuItemControl.AutoToolTip = false;
                            if (!string.IsNullOrEmpty(viewMenuItem.Remark))
                            {
                                viewMenuItemControl.AutoToolTip = true;
                                viewMenuItemControl.Text = LayoutHelper.GetControlRemark(_functionInitParamSet.SupportMultiCultures, "MenuItem", viewMenuItem.Name, _annexes, viewMenuItem.Remark);
                            }
                            //*font
                            if (viewMenuArea.ParentViewMenuId == 0)
                            {
                                viewMenuItemControl.ForeColor = StyleSheet.CaptionTextColor;
                                viewMenuItemControl.Font = new Font("", 8, FontStyle.Bold);
                            }
                            //*enable, visible
                            if (!isItemEnabled)
                            {
                                viewMenuItemControl.Enabled = false;
                            }
                            if (!isItemVisible)
                            {
                                viewMenuItemControl.Visible = false;
                            }

                            viewMenuAreaControl.Items.AddRange(new ToolStripItem[] { viewMenuItemControl });
                            //*event
                            if (viewMenuItem.Type == (int)LayoutElementType.DisplayAndTransactionItem)
                            {
                                viewMenuItemControl.Click += new System.EventHandler(ControlEventHandler);
                            }
                            else if (viewMenuItem.Type == (int)LayoutElementType.ViewMenuItem)
                            {
                                viewMenuItemControl.Click += new System.EventHandler(ViewMenuItemClickHandler);
                                if (viewMenuItem.Id.ToString() == defMenuIdStr)
                                {
                                    viewMenuItem.IsChecked = true;
                                    viewMenuItemControl.Checked = true;
                                }
                            }
                        }
                        else if (viewMenuItem.ControlTypeName.ToLower() == "ToolLabel".ToLower())
                        {
                            var viewMenuItemControl = new ToolStripLabel();
                            //*dock, size,offset
                            if (viewMenuItem.DockType == (int)ControlDockType.Right)
                            {
                                viewMenuItemControl.Alignment = ToolStripItemAlignment.Right;
                            }
                            if (viewMenuItem.Width != -1)
                            {
                                viewMenuItemControl.AutoSize = false;
                                viewMenuItemControl.Width = viewMenuItem.Width;
                            }
                            if (viewMenuItem.Height != -1)
                            {
                                viewMenuItemControl.Height = viewMenuItem.Height;
                            }
                            if (viewMenuItem.OffsetOrPositionX != -1)
                            {
                                var subItemOffsetControl = new ToolStripLabel();
                                subItemOffsetControl.AutoSize = false;
                                subItemOffsetControl.Width = viewMenuItem.OffsetOrPositionX;
                                if (viewMenuItem.DockType == (int)ControlDockType.Right)
                                {
                                    viewMenuItemControl.Alignment = ToolStripItemAlignment.Right;
                                    subItemOffsetControl.Alignment = ToolStripItemAlignment.Right;
                                }
                                viewMenuAreaControl.Items.AddRange(new ToolStripItem[] { subItemOffsetControl });
                            }

                            viewMenuItem.Name = viewMenuArea.Name;

                            //*displayname
                            if (!string.IsNullOrEmpty(viewMenuItem.DisplayName))
                            {
                                viewMenuItem.DisplayName = ResolveConstants(viewMenuItem.DisplayName);
                                var txt = ResolveStringByRefProcedureVariables(viewMenuItem.DisplayName);
                                viewMenuItemControl.Text = txt;
                            }

                            //*visible, enabled
                            if (viewMenuItem.InvisibleFlag == "true")
                            {
                                viewMenuItemControl.Visible = false;
                            }
                            if (viewMenuItem.DisabledFlag == "true")
                            {
                                viewMenuItemControl.Enabled = false;
                            }
                            //*font
                            if (viewMenuArea.ParentViewMenuId == 0)
                            {
                                viewMenuItemControl.ForeColor = StyleSheet.CaptionTextColor;
                                viewMenuItemControl.Font = new Font("", 8, FontStyle.Regular);
                            }
                            //*enable, visible
                            if (!isItemEnabled)
                            {
                                viewMenuItemControl.Enabled = false;
                            }
                            if (!isItemVisible)
                            {
                                viewMenuItemControl.Visible = false;
                            }

                            viewMenuAreaControl.Items.AddRange(new ToolStripItem[] { viewMenuItemControl });
                        }

                    }
                }
                else if ((viewMenuArea.Type == (int)LayoutElementType.ViewMenuArea
                        | viewMenuArea.Type == (int)LayoutElementType.ToolMenuArea)
                         && viewMenuArea.ControlTypeName.ToLower() == "Panel".ToLower())
                {

                    //*area
                    var regionControl = this.GetControl(viewMenuArea.Container);

                    var areaWidth = viewMenuArea.Width == -1 ? regionControl.Width : viewMenuArea.Width;
                    var areaHeight = viewMenuArea.Height == -1 ? regionControl.Height : viewMenuArea.Height;
                    var areaControl = new ContainerPanel();
                    areaControl.AutoScroll = true;
                    ControlHelper.SetContainerPanelStyleByClass(areaControl, viewMenuArea.StyleClass);
                    ControlHelper.SetContainerPanelStyleByText(areaControl, viewMenuArea.StyleText);
                    ControlHelper.SetControlDockStyleAndLocationAndSize(areaControl, viewMenuArea.DockType, areaWidth, areaHeight, viewMenuArea.OffsetOrPositionX, viewMenuArea.OffsetOrPositionY);

                    areaControl.Name = viewMenuArea.Name;
                    //areaControl.Padding=new Padding(1,0,0,0);
                    regionControl.Controls.Add(areaControl);
                    viewMenuArea.IsChecked = true;
                    viewMenuArea.IsRendered = true;

                    var defMenuIdStr = viewMenuArea.DefaultViewMenuIdFlag;
                    var viewMenuItems = _layoutElements.Where(x => x.Container == viewMenuArea.Name
                        && (x.Type == (int)LayoutElementType.DisplayOnlyItem | x.Type == (int)LayoutElementType.DisplayAndTransactionItem | x.Type == (int)LayoutElementType.ViewMenuItem)
                        ).ToList();

                    if (string.IsNullOrEmpty(defMenuIdStr) && viewMenuItems.Count > 0)
                    {
                        defMenuIdStr = viewMenuItems.FirstOrDefault().Id.ToString();
                    }
                    viewMenuItems = viewMenuItems.OrderByDescending(x => x.DockOrder).ToList();

                    //*items
                    foreach (var viewMenuItem in viewMenuItems)
                    {
                        itemNameForEx = viewMenuItem.Name;
                        //Visible, Enabled
                        bool isItemVisible = true;
                        var itemInvisibleFlag = viewMenuItem.InvisibleFlag;
                        if (string.IsNullOrEmpty(itemInvisibleFlag)) itemInvisibleFlag = "false";
                        else
                        {
                            viewMenuItem.InvisibleFlag = ResolveConstants(viewMenuItem.InvisibleFlag);
                            var txt = ResolveStringByRefProcedureVariables(viewMenuItem.InvisibleFlag);
                            itemInvisibleFlag = GetText(txt);
                        }
                        isItemVisible = (itemInvisibleFlag == "false" | itemInvisibleFlag == "0") ? true : false;

                        bool isItemEnabled = true;
                        var itemDisabledFlag = viewMenuItem.DisabledFlag;
                        if (string.IsNullOrEmpty(itemDisabledFlag)) itemDisabledFlag = "false";
                        else
                        {
                            viewMenuItem.DisabledFlag = ResolveConstants(viewMenuItem.DisabledFlag);
                            var txt = ResolveStringByRefProcedureVariables(viewMenuItem.DisabledFlag);
                            itemDisabledFlag = GetText(txt);

                        }
                        isItemEnabled = (itemDisabledFlag == "false" | itemDisabledFlag == "0") ? true : false;

                        if (viewMenuItem.ControlTypeName.ToLower().StartsWith("ImageTextButton".ToLower()))
                        {
                            var itemControl = new ImageTextButton();
                            itemControl.Name = viewMenuItem.Name;
                            itemControl.Text = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "MenuItem", viewMenuItem.Name, _annexes, viewMenuItem.DisplayName);
                            //*location, size
                            ControlHelper.SetControlDockStyleAndLocationAndSize(itemControl, viewMenuItem.DockType, viewMenuItem.Width, viewMenuItem.Height, viewMenuItem.OffsetOrPositionX, viewMenuItem.OffsetOrPositionY);
                            if (viewMenuItem.DockType > 0 && viewMenuItem.DockType < 5)
                            {
                                if (viewMenuItem.OffsetOrPositionX != -1 || viewMenuItem.OffsetOrPositionY != -1)
                                {
                                    var offsetCrtl = new Label();
                                    ControlHelper.SetControlOffsetByDockStyle(offsetCrtl, viewMenuItem.DockType, viewMenuItem.OffsetOrPositionX, viewMenuItem.OffsetOrPositionY);
                                    areaControl.Controls.Add(offsetCrtl);
                                }
                            }
                            else
                            {
                                itemControl.Location = new Point(viewMenuItem.OffsetOrPositionX, viewMenuItem.OffsetOrPositionY);
                            }

                            //*image
                            itemControl.ImageWidth = viewMenuItem.ImageWidth;
                            itemControl.ImageHeight = viewMenuItem.ImageHeight;
                            //var imgDir = _functionDir + "\\Images";
                            itemControl.Image = ControlHelper.GetImage(FileHelper.GetFilePath(viewMenuItem.ImageUrl, viewMenuItem.Location));
                            if (viewMenuItem.ControlTypeName.EndsWith("V"))
                            {
                                itemControl.TextImageRelation = TextImageRelation.ImageAboveText;
                                ControlHelper.SetImageTextButtonStyleByClass(itemControl, "VerMenu");

                            }
                            else if (viewMenuItem.ControlTypeName.EndsWith("H"))
                            {
                                itemControl.TextImageRelation = TextImageRelation.ImageBeforeText;
                                itemControl.TextAlign = ContentAlignment.MiddleLeft;
                                ControlHelper.SetImageTextButtonStyleByClass(itemControl, "HorMenu");
                            }

                            ControlHelper.SetImageTextButtonStyleByText(itemControl, viewMenuItem.StyleText);

                            //*enable, visible
                            if (!isItemEnabled)
                            {
                                itemControl.Enabled = false;
                            }
                            if (!isItemVisible)
                            {
                                itemControl.Visible = false;
                            }

                            areaControl.Controls.Add(itemControl);

                            //*event
                            if (viewMenuItem.Type == (int)LayoutElementType.DisplayAndTransactionItem)
                            {
                                itemControl.Click += new System.EventHandler(ControlEventHandler);
                            }

                            else if (viewMenuItem.Type == (int)LayoutElementType.ViewMenuItem)
                            {
                                itemControl.SensitiveType = ControlSensitiveType.Check;
                                itemControl.Click += new System.EventHandler(ViewMenuItemClickHandler);
                                if (viewMenuItem.Id.ToString() == defMenuIdStr)
                                {
                                    viewMenuItem.IsChecked = true;
                                    itemControl.Checked = true;
                                }
                            }
                        }
                        else if (viewMenuItem.ControlTypeName.ToLower() == "TextButton".ToLower())
                        {
                            var itemControl = new TextButton();
                            itemControl.Name = viewMenuItem.Name;
                            itemControl.Text = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "MenuItem", viewMenuItem.Name, _annexes, viewMenuItem.DisplayName);
                            ControlHelper.SetTextButtonStyleByClass(itemControl, "Menu");
                            ControlHelper.SetTextButtonStyleByText(itemControl, viewMenuItem.StyleText);
                            //*location, size
                            ControlHelper.SetControlDockStyleAndLocationAndSize(itemControl, viewMenuItem.DockType, viewMenuItem.Width, viewMenuItem.Height, viewMenuItem.OffsetOrPositionX, viewMenuItem.OffsetOrPositionY);
                            if (viewMenuItem.DockType > 0 && viewMenuItem.DockType < 5)
                            {
                                if (viewMenuItem.OffsetOrPositionX != -1 || viewMenuItem.OffsetOrPositionY != -1)
                                {
                                    var offsetCrtl = new Label();
                                    ControlHelper.SetControlOffsetByDockStyle(offsetCrtl, viewMenuItem.DockType, viewMenuItem.OffsetOrPositionX, viewMenuItem.OffsetOrPositionY);
                                    areaControl.Controls.Add(offsetCrtl);
                                }
                            }
                            else
                            {
                                itemControl.Location = new Point(viewMenuItem.OffsetOrPositionX, viewMenuItem.OffsetOrPositionY);
                            }

                            areaControl.Controls.Add(itemControl);
                            //*enable, visible
                            if (!isItemEnabled)
                            {
                                itemControl.Enabled = false;
                            }
                            if (!isItemVisible)
                            {
                                itemControl.Visible = false;
                            }
                            //*event
                            if (viewMenuItem.Type == (int)LayoutElementType.DisplayAndTransactionItem)
                            {
                                itemControl.Click += new System.EventHandler(ControlEventHandler);
                            }
                            else if (viewMenuItem.Type == (int)LayoutElementType.ViewMenuItem)
                            {
                                itemControl.SensitiveType = ControlSensitiveType.Check;
                                itemControl.Click += new System.EventHandler(ViewMenuItemClickHandler);
                                if (viewMenuItem.Id.ToString() == defMenuIdStr)
                                {
                                    viewMenuItem.IsChecked = true;
                                    itemControl.Checked = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RenderToolOrCustomizedViewMenuAreaAndItems Error: areaName=" + areaNameForEx + "; itemName=" + itemNameForEx + "; " + ex.Message);
            }
        }


        //##ResetDefaultViewMenu
        private void ResetDefaultViewMenu(List<LayoutElement> layoutElements, int startingMenuId)
        {
            var menu = layoutElements.Find(x => x.Id == startingMenuId);
            if (menu != null)
            {
                var menuAreaName = menu.Container;
                var viewMenuArea = layoutElements.Find(x => x.Name == menuAreaName);
                if (viewMenuArea != null)
                {
                    viewMenuArea.DefaultViewMenuIdFlag = menu.Id.ToString();
                    if (viewMenuArea.ParentViewMenuId == 0) return;
                    else ResetDefaultViewMenu(layoutElements, viewMenuArea.ParentViewMenuId);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        //##HideViewMenuAreas
        private void HideViewMenuAreas(int viewMenuId)
        {
            var menuArea = _layoutElements.Find(x => x.Type == (int)LayoutElementType.ViewMenuArea && x.ParentViewMenuId == viewMenuId);
            if (menuArea != null)
            {
                var areaControl = GetControl(menuArea.Name);
                ControlHelper.HideControlByDockStyle(areaControl, menuArea.DockType);
                menuArea.IsChecked = false;
                var checkedMenuItem = _layoutElements.Find(x => x.Container == menuArea.Name && x.Type == (int)LayoutElementType.ViewMenuItem && x.IsChecked);
                if (checkedMenuItem != null)
                {
                    HideViewMenuAreas(checkedMenuItem.Id);
                }
            }
        }

        //##ShowViewMenuArea
        private void ShowViewMenuArea(LayoutElement viewMenuArea)
        {
            try
            {
                var containerControl = GetControl(viewMenuArea.Container);
                var viewMenuAreaControl = ControlHelper.GetControlByNameByParent(viewMenuArea.Name, containerControl);
                var viewMenuAreaWidth = viewMenuArea.Width == -1 ? containerControl.Width : viewMenuArea.Width;
                var viewMenuAreaHeight = viewMenuArea.Height == -1 ? containerControl.Height : viewMenuArea.Height;
                ControlHelper.SetControlDockStyleAndLocationAndSize(viewMenuAreaControl, viewMenuArea.DockType, viewMenuAreaWidth, viewMenuAreaHeight, viewMenuArea.OffsetOrPositionX, viewMenuArea.OffsetOrPositionY);
                viewMenuArea.IsChecked = true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".ShowViewMenuArea Error: " + ex.Message);
            }
        }

        //##CheckViewMenuItemAndUncheckParallelItems
        private void CheckViewMenuItemAndUncheckParallelItems(int viewMenuId)
        {
            try
            {
                var menuItem = _layoutElements.Find(x => x.Id == viewMenuId && x.Type == (int)LayoutElementType.ViewMenuItem);
                var viewMenuArea = _layoutElements.Find(x => x.Name == menuItem.Container && x.Type == (int)LayoutElementType.ViewMenuArea);
                var menuAreaControl = GetControl(viewMenuArea.Name);
                //uncheck
                var lastCheckedParallelmenuItem = _layoutElements.Find(x => x.Container == viewMenuArea.Name && x.Type == (int)LayoutElementType.ViewMenuItem && x.IsChecked);
                if (lastCheckedParallelmenuItem != null)
                {
                    if (lastCheckedParallelmenuItem.ControlTypeName.ToLower().Contains("ImageTextToolButton".ToLower()))
                    {
                        var menuAreaCpnt = menuAreaControl as ToolStrip;
                        var cpnt = menuAreaCpnt.Items.Find(lastCheckedParallelmenuItem.Name, true)[0] as ToolStripButton;
                        cpnt.Checked = false;
                    }
                    else if (lastCheckedParallelmenuItem.ControlTypeName.ToLower().Contains("ImageTextButton".ToLower()))
                    {
                        var menuItemControl = ControlHelper.GetControlByNameByParent(lastCheckedParallelmenuItem.Name, menuAreaControl);
                        var cpnt = menuItemControl as ImageTextButton;
                        cpnt.Checked = false;
                    }
                    else if (lastCheckedParallelmenuItem.ControlTypeName.ToLower().Contains("TextButton".ToLower()))
                    {
                        var menuItemControl = ControlHelper.GetControlByNameByParent(lastCheckedParallelmenuItem.Name, menuAreaControl);
                        var cpnt = menuItemControl as TextButton;
                        cpnt.Checked = false;
                    }
                    lastCheckedParallelmenuItem.IsChecked = false;
                }

                //*check
                if (menuItem.ControlTypeName.ToLower().Contains("ImageTextToolButton".ToLower()))
                {
                    var menuAreaCpnt = menuAreaControl as ToolStrip;
                    var cpnt = menuAreaCpnt.Items.Find(menuItem.Name, true)[0] as ToolStripButton;
                    cpnt.Checked = true;
                }
                else if (menuItem.ControlTypeName.ToLower().Contains("ImageTextButton".ToLower()))
                {
                    var menuItemControl = ControlHelper.GetControlByNameByParent(menuItem.Name, menuAreaControl);
                    var cpnt = menuItemControl as ImageTextButton;
                    cpnt.Checked = true;
                }
                else if (menuItem.ControlTypeName.ToLower().Contains("TextButton".ToLower()))
                {
                    var menuItemControl = ControlHelper.GetControlByNameByParent(menuItem.Name, menuAreaControl);
                    var cpnt = menuItemControl as TextButton;
                    cpnt.Checked = true;
                }
                menuItem.IsChecked = true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".CheckViewMenuItemAndUncheckedParallelItems Error: " + ex.Message);
            }
        }

        //##view
        //##MergeViewItems
        private void MergeViewItems(string viewName, string viewDataSource)
        {
            var viewNameEx = "";
            try
            {
                var xmlPath = _functionsDir + "\\" + _functionInitParamSet.FunctionCode + "\\Views\\" + viewName;
                viewNameEx = viewName;
                if (!viewDataSource.IsNullOrEmpty())
                {
                    xmlPath = FileHelper.GetFilePath(viewDataSource, _functionsDir + "\\" + _functionInitParamSet.FunctionCode + "\\Views");
                }

                var xmlMgr = new XmlHandler(xmlPath);
                var tmpViewItems = xmlMgr.ConvertToObject<List<LayoutElement>>();
                foreach (var viewItem in tmpViewItems)
                {
                    if (viewItem.DockOrder.IsNullOrEmpty()) viewItem.DockOrder = viewItem.Id.ToString();
                    if (viewItem.InvalidFlag.IsNullOrEmpty()) viewItem.InvalidFlag = "false";
                    if (viewItem.InvalidFlag.StartsWith("="))
                    {
                        var txt = ResolveStringByRefProcedureVariables((viewItem.InvalidFlag));
                        viewItem.InvalidFlag = GetText(txt).ToLower();
                    }
                }

                var viewItems = tmpViewItems.Where(x =>
                    (x.InvalidFlag == "false" | x.InvalidFlag == "0") &
                    (x.TypeName == LayoutElementType.ContentArea.ToString()
                    | x.TypeName == LayoutElementType.Zone.ToString())
                    | x.TypeName == LayoutElementType.FollowingTransactionItem.ToString()
                    | x.TypeName == LayoutElementType.TransactionOnlyItem.ToString()
                //| x.TypeName == LayoutElementType.ViewBeforeRenderHandler.ToString() | x.TypeName == LayoutElementType.ViewAfterRenderHandler.ToString()
                //| x.TypeName == LayoutElementType.ViewAfterShowHandler.ToString() | x.TypeName == LayoutElementType.ViewAfterHideHandler.ToString().ToString()
                ).ToList();

                if (viewItems.Count > 0)
                {
                    foreach (var item in viewItems)
                    {
                        LayoutHelper.SetLayoutElementType(item);
                    }
                    LayoutHelper.CheckViewItems(viewName, viewItems);
                    foreach (var viewItem in viewItems)
                    {
                        //item.Id = -1;
                        viewItem.IsRendered = false;
                        viewItem.IsChecked = false;
                        viewItem.View = viewName;
                        viewItem.Name = viewName + "_" + viewItem.Name;
                        _layoutElements.Add(viewItem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".MergeViewItems Error: ViewName=" + viewNameEx + "; " + ex.Message);
            }
        }

        //##RenderView
        protected void RenderView(string viewName)
        {
            try
            {
                var cttAreas = _layoutElements.FindAll(x => x.View == viewName && (x.Type == (int)LayoutElementType.ContentArea));
                if (cttAreas.Count == 0) throw new ArgumentException("View: '" + viewName + "' has no ContentArea! ");
                var regions = cttAreas.Select(x => x.Container).Distinct();

                //ViewEventHandler(viewName, LayoutElementType.ViewBeforeRenderHandler);
                foreach (var region in regions)
                {
                    var regionCttAreas = cttAreas.Where(x => x.Container == region).OrderByDescending(x => x.DockOrder);
                    foreach (var regionCttArea in regionCttAreas)
                    {
                        RenderContentAreaAndItems(regionCttArea);
                    }
                }
                //ViewEventHandler(viewName, LayoutElementType.ViewAfterRenderHandler);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RenderView Error: " + ex.Message);
            }
        }

        //##AddViewStatus
        private void AddViewStatus(string viewName, bool isChecked)
        {
            try
            {
                var view = _renderedViewStatuses.Find(x => x.Name == viewName);
                if (view == null)
                    _renderedViewStatuses.Add(new RenderedViewStatus(viewName, isChecked));
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".AddViewStatus Error: " + ex.Message);
            }
        }

        //##HideLastCheckedViewThenShowCurrentView
        protected void HideLastCheckedViewThenShowCurrentView(string viewName)
        {
            try
            {
                //*hide
                HideLastCheckedView();

                //*show
                var areas = _layoutElements.FindAll(x => x.View == viewName && x.Type == (int)LayoutElementType.ContentArea);
                foreach (var area in areas)
                {
                    var areaContainerControl = new Control();
                    try
                    {
                        areaContainerControl = this.Controls.Find(area.Container, true)[0];
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("area.Container doesn't exist! ctrlName=" + area.Container);
                    }

                    var areaControl = ControlHelper.GetControlByNameByParent(area.Name, areaContainerControl);
                    var areaWidth = area.Width == -1 ? areaContainerControl.Width : area.Width;
                    var areaHeight = area.Height == -1 ? areaContainerControl.Height : area.Height;
                    ControlHelper.SetControlDockStyleAndLocationAndSize(areaControl, area.DockType, areaWidth, areaHeight, area.OffsetOrPositionX, area.OffsetOrPositionY);
                }

                //ViewEventHandler(viewName, LayoutElementType.ViewAfterShowHandler);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".HideLastCheckedViewThenShowCurrentView Error: " + ex.Message);
            }
        }

        //##HideLastCheckedView
        private void HideLastCheckedView()
        {
            try
            {
                //*hide
                var lastCheckedView = _renderedViewStatuses.Find(x => x.IsChecked);
                if (lastCheckedView == null) return;
                var parallelAreas = _layoutElements.FindAll(x => x.View == lastCheckedView.Name && x.Type == (int)LayoutElementType.ContentArea);
                foreach (var parallelArea in parallelAreas)
                {
                    var parallelAreaContainerControl = new Control();
                    try
                    {
                        parallelAreaContainerControl = this.Controls.Find(parallelArea.Container, true)[0];
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("parallelArea.Container doesn't exist! ctrlName=" + parallelArea.Container);
                    }

                    var parallelAreaControl = ControlHelper.GetControlByNameByParent(parallelArea.Name, parallelAreaContainerControl);
                    ControlHelper.HideControlByDockStyle(parallelAreaControl, parallelArea.DockType);
                }

                //ViewEventHandler(lastCheckedView.Name, LayoutElementType.ViewAfterHideHandler);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".HideLastCheckedView Error: " + ex.Message);
            }
        }


        //##SetViewCheckedAndUncheckLastView
        private void SetViewCheckedAndUncheckLastView(string viewName)
        {
            try
            {
                var checkedView = _renderedViewStatuses.Find(x => x.IsChecked);
                if (checkedView != null) checkedView.IsChecked = false;
                var view = _renderedViewStatuses.Find(x => x.Name == viewName);
                view.IsChecked = true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".SetViewCheckedAndUncheckLastView Error: " + ex.Message);
            }
        }

        //##UncheckLastCheckedView
        private void UncheckLastCheckedView()
        {
            try
            {
                var view = _renderedViewStatuses.Find(x => x.IsChecked);
                if (view != null)
                    view.IsChecked = false;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".UncheckLastCheckedView Error: " + ex.Message);
            }
        }

        //##IsViewRendered
        private bool IsViewRendered(string viewName)
        {
            try
            {
                var view = _renderedViewStatuses.Find(x => x.Name == viewName);
                if (view != null) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".IsViewRendered Error: " + ex.Message);
            }
        }

        //##ContentArea
        //##RenderContentAreaAndItems
        private void RenderContentAreaAndItems(LayoutElement cttArea)
        {
            var areaNameForEx = cttArea.Name;
            var itemNameForEx = "";
            try
            {
                if (cttArea.Type == (int)LayoutElementType.ContentArea)
                {
                    //area
                    var regionControl = this.GetControl(cttArea.Container);
                    var cttAreaWidth = cttArea.Width == -1 ? regionControl.Width : cttArea.Width;
                    var cttAreaHeight = cttArea.Height == -1 ? regionControl.Height : cttArea.Height;
                    var cttAreaControl = new ContainerPanel();
                    cttAreaControl.AutoScroll = true;
                    ControlHelper.SetContainerPanelStyleByClass(cttAreaControl, cttArea.StyleClass);
                    ControlHelper.SetContainerPanelStyleByText(cttAreaControl, cttArea.StyleText);
                    ControlHelper.SetControlDockStyleAndLocationAndSize(cttAreaControl, cttArea.DockType, cttAreaWidth, cttAreaHeight, cttArea.OffsetOrPositionX, cttArea.OffsetOrPositionY);

                    cttAreaControl.Name = cttArea.Name;
                    regionControl.Controls.Add(cttAreaControl);
                    cttArea.IsChecked = true;
                    cttArea.IsRendered = true;
                    var zones = _layoutElements.Where(x => x.Type == (int)LayoutElementType.Zone && x.View == cttArea.View && x.Container == cttArea.Name.Split('_')[1]
                            ).ToList();

                    zones = zones.OrderByDescending(x => x.DockOrder).ToList();

                    //items
                    foreach (var zone in zones)
                    {
                        itemNameForEx = zone.Name;
                        InitZone(zone, cttAreaControl);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RenderContentAreaAndItems Error: areaName=" + areaNameForEx + "; itemName=" + itemNameForEx + "; " + ex.Message);
            }
        }

        //##zone
        //##MergeZoneItems
        //##GetAndMergeZoneProcedures
        private List<ProcedureItem> GetAndMergeZoneProcedures(LayoutElement zone)
        {
            try
            {
                var zoneProcedures = new List<ProcedureItem>();
                var zoneProceduresTmp = new List<ProcedureItem>();
                var inputTxt = zone.InputVariables;
                if (!inputTxt.IsNullOrEmpty())
                {
                    var separator = inputTxt.GetParamSeparator();
                    var inputTxtArry = inputTxt.Split(separator);
                    var zinputStr = "";
                    int ct = 0;
                    foreach (var str in inputTxtArry)
                    {
                        var val = str;
                        if (!str.IsNullOrEmpty())
                        {
                            var txt = ResolveConstants(str);
                            val = GetText(txt);
                        }

                        zinputStr = ct == 0 ? val : zinputStr + "^" + val;
                        ct = ct + 1;
                    }

                    var variableItem = new ProcedureItem();
                    variableItem.Name = zone.Name + "_" + "zinput";
                    variableItem.Value = zinputStr;
                    variableItem.ZoneName = zone.Name;
                    variableItem.Type = (int)ProcedureItemType.None;
                    zoneProcedures.Add(variableItem);
                }

                var location = FileHelper.GetFilePath(zone.Location, _formDir + "\\Zones");
                var xmlPath = location + "\\Procedures";
                if (File.Exists(xmlPath + ".xml") | File.Exists(xmlPath + ".exml"))
                {
                    var xmlMgr = new XmlHandler(xmlPath);
                    zoneProceduresTmp = xmlMgr.ConvertToObject<List<ProcedureItem>>();
                }
                zoneProceduresTmp = zoneProceduresTmp.FindAll(x => x.TypeName == ProcedureItemType.Variable.ToString()
                                                                   | x.TypeName == ProcedureItemType.Action.ToString()
                                                                   | x.TypeName == ProcedureItemType.Break.ToString()
                                                                   | x.TypeName == ProcedureItemType.Exit.ToString());
                if (zoneProceduresTmp.Count > 0)
                {
                    LayoutHelper.CheckProcedures(false, zone.Name, zoneProceduresTmp);
                    string annexesXmlPath = xmlPath + "Annexes";
                    var annexList = new List<Annex>();
                    if (File.Exists(annexesXmlPath + ".xml") | File.Exists(annexesXmlPath + ".exml"))
                    {
                        var xmlToReadForAnnexes = new XmlHandler(annexesXmlPath);
                        annexList = xmlToReadForAnnexes.ConvertToObject<List<Annex>>();
                    }

                    foreach (var proc in zoneProceduresTmp)
                    {
                        LayoutHelper.SetProcedureType(proc);
                        proc.Value = string.IsNullOrEmpty(proc.Value) ? "" : proc.Value;

                        if (proc.GroupId < 0) proc.GroupId = 0;

                        var tempAnnexes = annexList.FindAll(x => x.MasterName == proc.Name);

                        proc.Name = zone.Name + "_" + proc.Name;
                        proc.ZoneName = zone.Name;
                        proc.Formula = AddZoneIdentifer(proc.Formula, zone.Name);
                        proc.Condition = AddZoneIdentifer(proc.Condition, zone.Name);
                        if (tempAnnexes.Count > 0)
                        {
                            foreach (var annex in tempAnnexes)
                            {
                                annex.ClassName = "ProcedureItem";
                                annex.MasterName = proc.Name;
                                _annexes.Add(annex);
                            }
                        }
                    }
                    zoneProcedures.AddRange(zoneProceduresTmp);
                }
                if (zoneProcedures.Count > 0)
                    _procedures.AddRange(zoneProcedures);

                return zoneProcedures;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetAndMergeZoneProcedures Error: " + ex.Message);
            }
        }

        private void MergeZoneItems(LayoutElement zone)
        {
            try
            {
                var location = FileHelper.GetFilePath(zone.Location, _formDir + "\\Zones");
                zone.Location = location;
                var xmlMgr = new XmlHandler(location + "\\ui");
                var tmpZoneItems = xmlMgr.ConvertToObject<List<ZoneItem>>().ToList();
                tmpZoneItems = tmpZoneItems.FindAll(x => x.TypeName == "ZoneBeforeRenderHandler" | x.TypeName == "ZoneAfterRenderHandler"
                | x.TypeName == "DisplayAndTransactionItem" | x.TypeName == "DisplayOnlyItem" | x.TypeName == "TransactionOnlyItem" | x.TypeName == "VirtualItem");
                var zoneItems = new List<ZoneItem>();

                foreach (var item in tmpZoneItems)
                {
                    if (item.DockOrder.IsNullOrEmpty()) item.DockOrder = item.Id.ToString();
                    if (item.InvalidFlag.IsNullOrEmpty()) item.InvalidFlag = "false";
                    item.InvalidFlag = AddZoneIdentifer(item.InvalidFlag, zone.Name);
                    if (item.InvalidFlag.StartsWith("="))
                    {
                        var txt = ResolveConstants((item.InvalidFlag));
                        txt = ResolveStringByRefProcedureVariables((item.InvalidFlag));
                        item.InvalidFlag = GetText(txt).ToLower();
                    }

                    if (item.InvalidFlag.ToLower() == "false" | item.InvalidFlag.ToLower() == "0")
                    {
                        zoneItems.Add(item);
                    }
                }

                if (zoneItems.Count > 0)
                {
                    foreach (var item in zoneItems)
                    {
                        LayoutHelper.SetZoneItemType(item);
                    }
                    LayoutHelper.CheckZoneItems(zone.Name, zoneItems);
                    string annexesXmlPath = location + "\\Annexes";
                    var annexList = new List<Annex>();
                    if (File.Exists(annexesXmlPath + ".xml") | File.Exists(annexesXmlPath + ".exml"))
                    {
                        var xmlToReadForAnnexes = new XmlHandler(annexesXmlPath);
                        annexList = xmlToReadForAnnexes.ConvertToObject<List<Annex>>();
                    }

                    foreach (var item in zoneItems)
                    {
                        var tempAnnexes = annexList.FindAll(x => x.MasterName == item.Name);
                        item.Name = zone.Name + "_" + item.Name;
                        if (!item.RowName.IsNullOrEmpty())
                            item.RowName = zone.Name + "_" + item.RowName;
                        _zonesItems.Add(item);
                        if (tempAnnexes.Count > 0)
                        {
                            foreach (var annex in tempAnnexes)
                            {
                                annex.ClassName = "ZoneItem";
                                annex.MasterName = item.Name;
                                _annexes.Add(annex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new ArgumentException("\n>> " + GetType().FullName + ".MergeZoneItems Error: ZoneName=" + zone.Name + " " + ex.Message);
            }
        }

        //##InitZone
        private void InitZone(LayoutElement zone, ContainerPanel cttAreaControl)
        {
            try
            {
                var zoneProcedures = GetAndMergeZoneProcedures(zone);
                InitZoneProcedures(zone.Name, zoneProcedures);

                MergeZoneItems(zone);
                ZoneEventHandler(zone.Name, LayoutElementType.ZoneBeforeRenderHandler);

                var location = zone.Location;

                var xmlMgr = new XmlHandler(location + "\\Feature");
                var zoneFeature = xmlMgr.ConvertToObject<ZoneFeature>();
                if (zone.Width < 0) zone.Width = zoneFeature.Width;
                if (zone.Height < 0) zone.Height = zoneFeature.Height;
                zone.ZoneArrangementType = zoneFeature.ArrangementType;
                if (zone.StyleText.IsNullOrEmpty()) zone.StyleText = zoneFeature.StyleText;

                var zoneContainer = new ContainerPanel();
                zoneContainer.AutoScroll = true;
                zoneContainer.Name = zone.Name;
                //zoneContainer.Dock = DockStyle.Left;
                ControlHelper.SetContainerPanelStyleByClass(zoneContainer, zone.StyleClass);
                ControlHelper.SetContainerPanelStyleByText(zoneContainer, zone.StyleText);
                ControlHelper.SetPanelStyleByText(zoneContainer, zone.StyleText);
                var zoneWidth = zone.Width == -1 ? cttAreaControl.Width : zone.Width;
                var zoneHeight = zone.Height == -1 ? cttAreaControl.Height : zone.Height;

                var isPopup = zone.IsPopup;
                if (isPopup)
                {
                    zoneContainer.Location = new Point(3, 3);
                    zoneContainer.Size = new Size(zoneWidth, zoneHeight);
                    var popupContainer = new PopupContainer();
                    popupContainer.Name = zone.Name + "_" + "Container";
                    popupContainer.Width = zone.Width + 9;
                    popupContainer.Height = zone.Height + 9;
                    GroundPanel.Controls.Add(popupContainer);
                    var shadowPanelCtrl = ControlHelper.GetControlByNameByContainer(popupContainer, "shadowPanel");
                    shadowPanelCtrl.Controls.Add(zoneContainer);
                }
                else
                {
                    ControlHelper.SetControlDockStyleAndLocationAndSize(zoneContainer, zone.DockType, zoneWidth, zoneHeight, zone.OffsetOrPositionX, zone.OffsetOrPositionY);
                    cttAreaControl.Controls.Add(zoneContainer);
                }
                RenderZoneItems(zone, zoneContainer);

                ZoneEventHandler(zone.Name, LayoutElementType.ZoneAfterRenderHandler);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".InitZone Error" + "; " + ex.Message);
            }
        }

        //##RenderZoneItems
        private void RenderZoneItems(LayoutElement zone, Control zoneContainer)
        {
            var zoneNameForEx = zone.Name;
            var zoneItemNameForEx = "";
            var zoneItemIdForEx = "";
            try
            {
                var zonePanel = new Panel();
                zonePanel = zoneContainer as Panel;
                var zoneItems = _zonesItems.FindAll(x => x.Name.StartsWith(zone.Name + "_"));
                var zoneArrangement = zone.ZoneArrangementType == 0 ? ZoneArrangementType.Positioning : ZoneArrangementType.RowLining;

                foreach (var zoneItem in zoneItems.Where(x => x.Type == (int)LayoutElementType.TransactionOnlyItem
                | x.Type == (int)LayoutElementType.ZoneBeforeRenderHandler | x.Type == (int)LayoutElementType.ZoneAfterRenderHandler).OrderBy(x => x.DockOrder))
                {
                    if (!String.IsNullOrEmpty(zoneItem.Action))
                    {
                        zoneItem.Action = AddZoneIdentifer(zoneItem.Action, zone.Name);
                    }
                }
                foreach (var zoneItem in zoneItems.Where(x => x.Type == (int)LayoutElementType.DisplayAndTransactionItem | x.Type == (int)LayoutElementType.DisplayOnlyItem).OrderBy(x => x.DockOrder))
                {
                    zoneItemNameForEx = zoneItem.Name;
                    zoneItemIdForEx = zoneItem.Id.ToString();

                    //*Visible
                    bool isCpntVisible = true;
                    var invisibleFlag = zoneItem.InvisibleFlag;
                    if (string.IsNullOrEmpty(invisibleFlag)) invisibleFlag = "false";
                    else
                    {
                        zoneItem.InvisibleFlag = ResolveConstants(zoneItem.InvisibleFlag);
                        zoneItem.InvisibleFlag = AddZoneIdentifer(zoneItem.InvisibleFlag, zone.Name);
                        invisibleFlag = ResolveStringByRefProcedureVariablesAndControls(zoneItem.InvisibleFlag);
                        invisibleFlag = GetText(invisibleFlag);
                    }
                    isCpntVisible = (invisibleFlag.ToLower() == "false" | invisibleFlag.ToLower() == "0") ? true : false;

                    //*Enabled
                    bool isCpntEnabled = true;
                    var disabledFlag = zoneItem.DisabledFlag;
                    if (string.IsNullOrEmpty(disabledFlag)) disabledFlag = "false";
                    else
                    {
                        zoneItem.DisabledFlag = ResolveConstants(zoneItem.DisabledFlag);
                        zoneItem.DisabledFlag = AddZoneIdentifer(zoneItem.DisabledFlag, zone.Name);
                        disabledFlag = ResolveStringByRefProcedureVariablesAndControls(zoneItem.DisabledFlag);
                        disabledFlag = GetText(disabledFlag);
                    }
                    isCpntEnabled = (disabledFlag.ToLower() == "false" | disabledFlag.ToLower() == "0") ? true : false;

                    //*dataSrc
                    var dataSrc = "";
                    if (!String.IsNullOrEmpty(zoneItem.DataSource))
                    {
                        zoneItem.DataSource = ResolveConstants(zoneItem.DataSource);
                        zoneItem.DataSource = AddZoneIdentifer(zoneItem.DataSource, zone.Name);
                        dataSrc = ResolveStringByRefProcedureVariablesAndControls(zoneItem.DataSource);
                    }

                    //*defVal
                    var defVal = "";
                    if (!String.IsNullOrEmpty(zoneItem.DefaultValue))
                    {
                        zoneItem.DefaultValue = ResolveConstants(zoneItem.DefaultValue);
                        zoneItem.DefaultValue = AddZoneIdentifer(zoneItem.DefaultValue, zone.Name);
                        defVal = ResolveStringByRefProcedureVariablesAndControls(zoneItem.DefaultValue);
                        defVal = GetText(defVal);
                    }

                    //*diplayName
                    var displayName = "";
                    if (!String.IsNullOrEmpty(zoneItem.DisplayName))
                    {
                        zoneItem.DisplayName = ResolveConstants(zoneItem.DisplayName);
                        zoneItem.DisplayName = AddZoneIdentifer(zoneItem.DisplayName, zone.Name);
                        var txt = ResolveStringByRefProcedureVariablesAndControls(zoneItem.DisplayName);
                        if (zoneItem.DisplayName.StartsWith("="))
                        {
                            displayName = GetText(txt);
                        }
                        else
                        {
                            displayName = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "ZoneItem", zoneItem.Name, _annexes, zoneItem.DisplayName);
                        }
                    }
                    else
                    {
                        displayName = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "ZoneItem", zoneItem.Name, _annexes, "");
                    }

                    //*action
                    var action = "";
                    if (!String.IsNullOrEmpty(zoneItem.Action))
                    {
                        zoneItem.Action = AddZoneIdentifer(zoneItem.Action, zone.Name);
                    }

                    //*action1
                    var action1 = "";
                    if (!String.IsNullOrEmpty(zoneItem.Action1))
                    {
                        zoneItem.Action1 = AddZoneIdentifer(zoneItem.Action1, zone.Name);
                    }


                    //*styleClass
                    var styleClass = "";
                    if (!String.IsNullOrEmpty(zoneItem.StyleClass))
                    {
                        zoneItem.StyleClass = ResolveConstants(zoneItem.StyleClass);
                        zoneItem.StyleClass = AddZoneIdentifer(zoneItem.StyleClass, zone.Name);
                        styleClass = ResolveStringByRefProcedureVariablesAndControls(zoneItem.StyleClass);
                        styleClass = GetText(styleClass);
                    }

                    //*styleText
                    var styleText = "";
                    if (!String.IsNullOrEmpty(zoneItem.StyleText))
                    {
                        zoneItem.StyleText = ResolveConstants(zoneItem.StyleText);
                        zoneItem.StyleText = AddZoneIdentifer(zoneItem.StyleText, zone.Name);
                        styleText = ResolveStringByRefProcedureVariablesAndControls(zoneItem.StyleText);
                        styleText = GetText(styleText);
                    }

                    //*render item
                    if (zoneItem.ControlTypeName == "Panel")
                    {
                        var cpnt = new Panel();
                        cpnt.Name = zoneItem.Name;
                        cpnt.AutoScroll = false;
                        ControlHelper.SetPanelStyleByClass(cpnt, styleClass);
                        ControlHelper.SetPanelStyleByText(cpnt, styleText);
                        if (!isCpntVisible) cpnt.Visible = false;
                        if (!isCpntEnabled) cpnt.Enabled = false;
                        zonePanel.Controls.Add(cpnt);
                        SetZoneCpntDockStyleOrPositionOnZoneArrangementTypeIsPositioning(zoneArrangement, zonePanel, cpnt, zoneItem.DockType, zoneItem.Width, zoneItem.Height, zoneItem.OffsetOrPositionX, zoneItem.OffsetOrPositionY);
                    }
                    else if (zoneItem.ControlTypeName == "ContainerPanel")
                    {
                        var cpnt = new ContainerPanel();
                        cpnt.Name = zoneItem.Name;
                        cpnt.AutoScroll = false;
                        ControlHelper.SetContainerPanelStyleByClass(cpnt, styleClass);
                        ControlHelper.SetContainerPanelStyleByText(cpnt, styleText);
                        if (!isCpntVisible) cpnt.Visible = false;
                        if (!isCpntEnabled) cpnt.Enabled = false;
                        zonePanel.Controls.Add(cpnt);
                        SetZoneCpntDockStyleOrPositionOnZoneArrangementTypeIsPositioning(zoneArrangement, zonePanel, cpnt, zoneItem.DockType, zoneItem.Width, zoneItem.Height, zoneItem.OffsetOrPositionX, zoneItem.OffsetOrPositionY);
                    }
                    else if (zoneItem.ControlTypeName == "ShadowPanel")
                    {
                        var cpnt = new ShadowPanel();
                        cpnt.Name = zoneItem.Name;

                        ControlHelper.SetShadowPanelStyleByClass(cpnt, styleClass);
                        ControlHelper.SetShadowPanelStyleByText(cpnt, styleText);
                        if (!isCpntVisible) cpnt.Visible = false;
                        if (!isCpntEnabled) cpnt.Enabled = false;

                        zonePanel.Controls.Add(cpnt);
                        SetZoneCpntDockStyleOrPositionOnZoneArrangementTypeIsPositioning(zoneArrangement, zonePanel, cpnt, zoneItem.DockType, zoneItem.Width, zoneItem.Height, zoneItem.OffsetOrPositionX, zoneItem.OffsetOrPositionY);
                    }
                    else if (zoneItem.ControlTypeName == "SplitRectangle")
                    {
                        var cpnt = new SplitRectangle();
                        cpnt.Name = zoneItem.Name;

                        ControlHelper.SetSplitRectangleStyleByClass(cpnt, styleClass);
                        ControlHelper.SetSplitRectangleStyleByText(cpnt, styleText);
                        if (!isCpntVisible) cpnt.Visible = false;
                        if (!isCpntEnabled) cpnt.Enabled = false;

                        zonePanel.Controls.Add(cpnt);
                        SetZoneCpntDockStyleOrPositionOnZoneArrangementTypeIsPositioning(zoneArrangement, zonePanel, cpnt, zoneItem.DockType, zoneItem.Width, zoneItem.Height, zoneItem.OffsetOrPositionX, zoneItem.OffsetOrPositionY);
                    }
                    else if (zoneItem.ControlTypeName == "GroupBox")
                    {
                        var cpnt = new GroupBox();
                        cpnt.Name = zoneItem.Name;

                        if (!isCpntVisible) cpnt.Visible = false;
                        if (!isCpntEnabled) cpnt.Enabled = false;

                        zonePanel.Controls.Add(cpnt);
                        SetZoneCpntDockStyleOrPositionOnZoneArrangementTypeIsPositioning(zoneArrangement, zonePanel, cpnt, zoneItem.DockType, zoneItem.Width, zoneItem.Height, zoneItem.OffsetOrPositionX, zoneItem.OffsetOrPositionY);
                    }
                    else if (zoneItem.ControlTypeName == "Label")
                    {
                        var cpnt = new Label();
                        cpnt.TextAlign = ContentAlignment.MiddleLeft;
                        cpnt.Name = zoneItem.Name;
                        cpnt.Text = displayName;
                        cpnt.TextAlign = ContentAlignment.MiddleLeft;
                        ControlHelper.SetLabelStyleByClass(cpnt, styleClass);
                        ControlHelper.SetLabelStyleByText(cpnt, styleText);
                        if (zoneItem.Width != -1) cpnt.AutoSize = false;
                        else
                        {
                            cpnt.AutoSize = true;
                            cpnt.Width = 1;
                        }

                        if (zoneItem.Type == (int)LayoutElementType.DisplayAndTransactionItem)
                        {
                            cpnt.Click += new System.EventHandler(ControlEventHandler);
                        }

                        if (!isCpntVisible) cpnt.Visible = false;
                        if (!isCpntEnabled) cpnt.Enabled = false;

                        zonePanel.Controls.Add(cpnt);
                        SetZoneCpntDockStyleOrPositionOnZoneArrangementTypeIsPositioning(zoneArrangement, zonePanel, cpnt, zoneItem.DockType, zoneItem.Width, zoneItem.Height, zoneItem.OffsetOrPositionX, zoneItem.OffsetOrPositionY);

                    }
                    else if (zoneItem.ControlTypeName == "TitleLabel")
                    {
                        var cpnt = new TitleLabel();
                        cpnt.TextAlign = ContentAlignment.MiddleLeft;
                        cpnt.Name = zoneItem.Name;
                        cpnt.Text = displayName;
                        cpnt.TextAlign = ContentAlignment.MiddleLeft;
                        ControlHelper.SetTitleLabelStyleByClass(cpnt, styleClass);
                        ControlHelper.SetTitleLabelStyleByText(cpnt, styleText);

                        if (!isCpntVisible) cpnt.Visible = false;
                        if (!isCpntEnabled) cpnt.Enabled = false;

                        zonePanel.Controls.Add(cpnt);
                        SetZoneCpntDockStyleOrPositionOnZoneArrangementTypeIsPositioning(zoneArrangement, zonePanel, cpnt, zoneItem.DockType, zoneItem.Width, zoneItem.Height, zoneItem.OffsetOrPositionX, zoneItem.OffsetOrPositionY);

                    }
                    else if (zoneItem.ControlTypeName == "CommandLabel")
                    {
                        var cpnt = new CommandLabel();
                        cpnt.TextAlign = ContentAlignment.MiddleLeft;
                        cpnt.Name = zoneItem.Name;
                        cpnt.Text = displayName;
                        cpnt.TextAlign = ContentAlignment.MiddleLeft;
                        ControlHelper.SetCommandLabelStyleByClass(cpnt, styleClass);
                        ControlHelper.SetCommandLabelStyleByText(cpnt, styleText);

                        //*event
                        if (zoneItem.Type == (int)LayoutElementType.DisplayAndTransactionItem)
                        {
                            cpnt.Click += new System.EventHandler(ControlEventHandler);
                        }

                        if (!isCpntVisible) cpnt.Visible = false;
                        if (!isCpntEnabled) cpnt.Enabled = false;

                        zonePanel.Controls.Add(cpnt);
                        SetZoneCpntDockStyleOrPositionOnZoneArrangementTypeIsPositioning(zoneArrangement, zonePanel, cpnt, zoneItem.DockType, zoneItem.Width, zoneItem.Height, zoneItem.OffsetOrPositionX, zoneItem.OffsetOrPositionY);

                    }
                    else if (zoneItem.ControlTypeName == "StatusLight")
                    {
                        var cpnt = new StatusLight();
                        cpnt.Name = zoneItem.Name;
                        cpnt.Text = displayName;
                        ControlHelper.SetStatusLightStyleByClass(cpnt, styleClass);
                        ControlHelper.SetStatusLightStyleByText(cpnt, styleText);

                        if (!defVal.IsNullOrEmpty())
                        {
                            cpnt.Value = Convert.ToInt16(defVal);
                        }

                        if (!isCpntVisible) cpnt.Visible = false;
                        if (!isCpntEnabled) cpnt.Enabled = true;

                        //*event
                        if (zoneItem.Type == (int)LayoutElementType.DisplayAndTransactionItem)
                        {
                            cpnt.OnLightClick += new System.EventHandler(ControlEventHandler);
                        }
                        zonePanel.Controls.Add(cpnt);
                        SetZoneCpntDockStyleOrPositionOnZoneArrangementTypeIsPositioning(zoneArrangement, zonePanel, cpnt, zoneItem.DockType, zoneItem.Width, zoneItem.Height, zoneItem.OffsetOrPositionX, zoneItem.OffsetOrPositionY);

                    }
                    else if (zoneItem.ControlTypeName == "ScoreLight")
                    {
                        var cpnt = new ScoreLight();
                        cpnt.Name = zoneItem.Name;
                        cpnt.Text = displayName;
                        ControlHelper.SetScoreLightStyleByClass(cpnt, styleClass);
                        ControlHelper.SetScoreLightStyleByText(cpnt, styleText);

                        if (!defVal.IsNullOrEmpty())
                        {
                            cpnt.Value = Convert.ToSingle(defVal);
                        }

                        if (!isCpntVisible) cpnt.Visible = false;
                        if (!isCpntEnabled) cpnt.Enabled = true;

                        //*event
                        if (zoneItem.Type == (int)LayoutElementType.DisplayAndTransactionItem)
                        {
                            cpnt.OnLightClick += new System.EventHandler(ControlEventHandler);
                        }
                        zonePanel.Controls.Add(cpnt);
                        SetZoneCpntDockStyleOrPositionOnZoneArrangementTypeIsPositioning(zoneArrangement, zonePanel, cpnt, zoneItem.DockType, zoneItem.Width, zoneItem.Height, zoneItem.OffsetOrPositionX, zoneItem.OffsetOrPositionY);

                    }
                    else if (zoneItem.ControlTypeName == "PictureBox")
                    {
                        var cpnt = new PictureBox();
                        cpnt.Name = zoneItem.Name;
                        var location = zone.Location;
                        var imagUrl = FileHelper.GetFilePath(GetText(dataSrc), location);

                        ControlHelper.SetControlBackgroundImage(cpnt, imagUrl);

                        //*event
                        if (!String.IsNullOrEmpty(zoneItem.DisplayName))
                        {
                            cpnt.Tag = displayName;
                            cpnt.MouseHover += new System.EventHandler(ControlHoverHandler);
                        }
                        if (zoneItem.Type == (int)LayoutElementType.DisplayAndTransactionItem)
                        {
                            cpnt.Click += new System.EventHandler(ControlEventHandler);
                        }

                        if (!isCpntVisible) cpnt.Visible = false;
                        if (!isCpntEnabled) cpnt.Enabled = false;

                        zonePanel.Controls.Add(cpnt);
                        SetZoneCpntDockStyleOrPositionOnZoneArrangementTypeIsPositioning(zoneArrangement, zonePanel, cpnt, zoneItem.DockType, zoneItem.Width, zoneItem.Height, zoneItem.OffsetOrPositionX, zoneItem.OffsetOrPositionY);

                    }
                    else if (zoneItem.ControlTypeName == "TextButton")
                    {
                        var cpnt = new TextButton();
                        cpnt.Name = zoneItem.Name;
                        cpnt.Text = displayName;
                        ControlHelper.SetTextButtonStyleByClass(cpnt, styleClass);
                        ControlHelper.SetTextButtonStyleByText(cpnt, styleText);

                        //*event
                        if (zoneItem.Type == (int)LayoutElementType.DisplayAndTransactionItem)
                        {
                            cpnt.Click += new System.EventHandler(ControlEventHandler);
                        }

                        if (!isCpntVisible) cpnt.Visible = false;
                        if (!isCpntEnabled) cpnt.Enabled = false;

                        zonePanel.Controls.Add(cpnt);
                        SetZoneCpntDockStyleOrPositionOnZoneArrangementTypeIsPositioning(zoneArrangement, zonePanel, cpnt, zoneItem.DockType, zoneItem.Width, zoneItem.Height, zoneItem.OffsetOrPositionX, zoneItem.OffsetOrPositionY);

                    }
                    else if (zoneItem.ControlTypeName.StartsWith("ImageTextButton"))
                    {
                        var cpnt = new ImageTextButton();
                        cpnt.Name = zoneItem.Name;
                        cpnt.Text = displayName;

                        //*image
                        var imgDir = zone.Location;
                        cpnt.ImageWidth = zoneItem.Width;
                        cpnt.ImageHeight = zoneItem.Height;

                        if (!dataSrc.IsNullOrEmpty())
                        {
                            if (dataSrc.StartsWith("="))
                            {
                                dataSrc = GetText(dataSrc);
                            }
                            cpnt.Image = ControlHelper.GetImage(FileHelper.GetFilePath(GetText(dataSrc), imgDir));
                        }

                        if (zoneItem.ControlTypeName.EndsWith("V"))
                        {
                            cpnt.TextImageRelation = TextImageRelation.ImageAboveText;
                            cpnt.TextAlign = ContentAlignment.MiddleCenter;
                        }
                        else if (zoneItem.ControlTypeName.EndsWith("H"))
                        {
                            cpnt.TextImageRelation = TextImageRelation.ImageBeforeText;
                            cpnt.TextAlign = ContentAlignment.MiddleLeft;
                        }
                        ControlHelper.SetImageTextButtonStyleByClass(cpnt, styleClass);
                        ControlHelper.SetImageTextButtonStyleByText(cpnt, styleText);

                        //*event
                        if (zoneItem.Type == (int)LayoutElementType.DisplayAndTransactionItem)
                        {
                            cpnt.Click += new System.EventHandler(ControlEventHandler);
                        }

                        if (!isCpntVisible) cpnt.Visible = false;
                        if (!isCpntEnabled) cpnt.Enabled = false;
                        zonePanel.Controls.Add(cpnt);
                        SetZoneCpntDockStyleOrPositionOnZoneArrangementTypeIsPositioning(zoneArrangement, zonePanel, cpnt, zoneItem.DockType, zoneItem.Width, zoneItem.Height, zoneItem.OffsetOrPositionX, zoneItem.OffsetOrPositionY);

                    }
                    else if (zoneItem.ControlTypeName == "RadioButton")
                    {
                        if (zoneItem.ContainerName.IsNullOrEmpty()) throw new ArgumentException("RadioButton must have a container! ");
                        zoneItem.ContainerName = zone.Name + "_" + zoneItem.ContainerName;

                        var cpnt = new RadioButton();
                        cpnt.Name = zoneItem.Name;
                        cpnt.Text = displayName;
                        if (!defVal.IsNullOrEmpty())
                        {
                            if (defVal.ToLower() == "true") cpnt.Checked = true;
                            else cpnt.Checked = false;
                        }
                        ControlHelper.SetRadioButtonStyleByClass(cpnt, styleClass);
                        ControlHelper.SetRadioButtonStyleByText(cpnt, styleText);
                        //*event
                        if (zoneItem.Type == (int)LayoutElementType.DisplayAndTransactionItem)
                        {
                            cpnt.Click += new System.EventHandler(ControlEventHandler);
                        }

                        if (!isCpntVisible) cpnt.Visible = false;
                        if (!isCpntEnabled) cpnt.Enabled = false;
                        var container = GetControl(zoneItem.ContainerName);

                        container.Controls.Add(cpnt);
                        ControlHelper.SetControlDockStyleAndLocationAndSize(cpnt, zoneItem.DockType, zoneItem.Width, zoneItem.Height, zoneItem.OffsetOrPositionX, zoneItem.OffsetOrPositionY);
                    }
                    else if (zoneItem.ControlTypeName == "CheckBox")
                    {
                        var cpnt = new CheckBox();

                        cpnt.Name = zoneItem.Name;
                        cpnt.Text = displayName;
                        if (!defVal.IsNullOrEmpty())
                        {
                            if (defVal.ToLower() == "true") cpnt.Checked = true;
                            else cpnt.Checked = false;
                        }
                        ControlHelper.SetCheckBoxStyleByClass(cpnt, styleClass);
                        ControlHelper.SetCheckBoxStyleByText(cpnt, styleText);
                        //*event
                        if (zoneItem.Type == (int)LayoutElementType.DisplayAndTransactionItem)
                        {
                            cpnt.CheckedChanged += new System.EventHandler(ControlEventHandler);
                        }

                        if (!isCpntVisible) cpnt.Visible = false;
                        if (!isCpntEnabled) cpnt.Enabled = false;

                        zonePanel.Controls.Add(cpnt);
                        SetZoneCpntDockStyleOrPositionOnZoneArrangementTypeIsPositioning(zoneArrangement, zonePanel, cpnt, zoneItem.DockType, zoneItem.Width, zoneItem.Height, zoneItem.OffsetOrPositionX, zoneItem.OffsetOrPositionY);

                    }
                    else if (zoneItem.ControlTypeName == "TextBox")
                    {
                        var cpnt = new TextBox();
                        cpnt.Name = zoneItem.Name;
                        var type = zoneItem.StyleText.GetStyleValue("Type");
                        if (type.ToLower() == "password".ToLower())
                        {
                            cpnt.PasswordChar = '*';
                        }
                        if (!defVal.IsNullOrEmpty()) cpnt.Text = defVal;

                        //*event
                        if (zoneItem.Type == (int)LayoutElementType.DisplayAndTransactionItem)
                        {
                            cpnt.TextChanged += new System.EventHandler(ControlEventHandler);
                        }

                        if (!isCpntVisible) cpnt.Visible = false;
                        if (!isCpntEnabled) cpnt.ReadOnly = true;

                        zonePanel.Controls.Add(cpnt);
                        SetZoneCpntDockStyleOrPositionOnZoneArrangementTypeIsPositioning(zoneArrangement, zonePanel, cpnt, zoneItem.DockType, zoneItem.Width, zoneItem.Height, zoneItem.OffsetOrPositionX, zoneItem.OffsetOrPositionY);

                    }
                    else if (zoneItem.ControlTypeName == "RichTextBox")
                    {
                        var cpnt = new RichTextBox();
                        cpnt.Name = zoneItem.Name;
                        if (!defVal.IsNullOrEmpty()) cpnt.Text = defVal;

                        ControlHelper.SetRichTextBoxStyleByClass(cpnt, styleClass);
                        ControlHelper.SetRichTextBoxStyleByText(cpnt, styleText);
                        //*event
                        if (zoneItem.Type == (int)LayoutElementType.DisplayAndTransactionItem)
                        {
                            cpnt.TextChanged += new System.EventHandler(ControlEventHandler);
                        }

                        if (!isCpntVisible) cpnt.Visible = false;
                        if (!isCpntEnabled) cpnt.ReadOnly = true;

                        zonePanel.Controls.Add(cpnt);
                        SetZoneCpntDockStyleOrPositionOnZoneArrangementTypeIsPositioning(zoneArrangement, zonePanel, cpnt, zoneItem.DockType, zoneItem.Width, zoneItem.Height, zoneItem.OffsetOrPositionX, zoneItem.OffsetOrPositionY);

                    }
                    else if (zoneItem.ControlTypeName == "ProgressBar")
                    {
                        var cpnt = new ProgressBar();
                        cpnt.Name = zoneItem.Name;
                        if (!defVal.IsNullOrEmpty()) cpnt.Value = Convert.ToInt16(defVal);

                        ControlHelper.SetProgressBarStyleByClass(cpnt, styleClass);
                        ControlHelper.SetProgressBarStyleByText(cpnt, styleText);
                        if (!isCpntVisible) cpnt.Visible = false;
                        if (!isCpntEnabled) cpnt.Enabled = false;

                        zonePanel.Controls.Add(cpnt);
                        SetZoneCpntDockStyleOrPositionOnZoneArrangementTypeIsPositioning(zoneArrangement, zonePanel, cpnt, zoneItem.DockType, zoneItem.Width, zoneItem.Height, zoneItem.OffsetOrPositionX, zoneItem.OffsetOrPositionY);

                    }

                    else if (zoneItem.ControlTypeName == "ComboBox")
                    {
                        var cpnt = new ComboBox();
                        cpnt.Name = zoneItem.Name;
                        var style = zoneItem.StyleText.GetStyleValue("Style");
                        if (style.ToLower() == "DropDown".ToLower())
                        {
                            cpnt.DropDownStyle = ComboBoxStyle.DropDown;
                            cpnt.SelectionLength = 0;
                        }
                        else
                        {
                            cpnt.DropDownStyle = ComboBoxStyle.DropDownList;
                        }

                        //*DataSource
                        if (!dataSrc.IsNullOrEmpty())
                        {
                            var valTxts = new List<ValueText>();
                            if (dataSrc.StartsWith("="))
                            {
                                dataSrc = GetText(dataSrc);
                                if (dataSrc.Contains("{") & dataSrc.Contains("}"))
                                {
                                    valTxts = JsonHelper.ConvertToObject<List<ValueText>>(dataSrc);
                                }
                            }
                            else
                            {
                                var strArray = dataSrc.Split(dataSrc.GetSubParamSeparator());
                                if (dataSrc.Contains(":"))
                                {
                                    foreach (var v in strArray)
                                    {
                                        var arry = v.Split(':');
                                        var valTxt = new ValueText();
                                        valTxt.Value = arry[0];
                                        valTxt.Text = arry[1];
                                        valTxts.Add(valTxt);
                                    }
                                }
                                else
                                {
                                    var i = 0;
                                    foreach (var v in strArray)
                                    {
                                        var valTxt = new ValueText();
                                        valTxt.Value = i.ToString();
                                        valTxt.Text = v;
                                        valTxts.Add(valTxt);
                                        i++;
                                    }
                                }
                            }

                            cpnt.DataSource = valTxts;
                            cpnt.ValueMember = "Value";
                            cpnt.DisplayMember = "Text";
                        }

                        zonePanel.Controls.Add(cpnt); //for ComboBox, this sentence must be before setting default value! else the selected value will be first one
                        SetZoneCpntDockStyleOrPositionOnZoneArrangementTypeIsPositioning(zoneArrangement, zonePanel, cpnt, zoneItem.DockType, zoneItem.Width, zoneItem.Height, zoneItem.OffsetOrPositionX, zoneItem.OffsetOrPositionY);

                        //*DefaultValue
                        if (style.ToLower() == "DropDown".ToLower())
                        {
                            if (!defVal.IsNullOrEmpty())
                            {
                                cpnt.SelectedValue = defVal;
                            }
                            else
                                cpnt.SelectedIndex = -1;
                        }
                        else if (!defVal.IsNullOrEmpty())
                        {
                            cpnt.SelectedValue = defVal;
                        }

                        //*event
                        if (zoneItem.Type == (int)LayoutElementType.DisplayAndTransactionItem)
                        {
                            cpnt.SelectedIndexChanged += new System.EventHandler(ControlEventHandler);
                        }
                        //*Visible
                        if (!isCpntVisible) cpnt.Visible = false;
                        if (!isCpntEnabled) cpnt.Enabled = false;
                    }



                }

                //**Location and Size
                if (zoneArrangement == ZoneArrangementType.RowLining)
                {
                    int itemDefOffsetX = StyleSheet.DefaultOffsetX;
                    var rows = zoneItems.FindAll(x => x.Type == (int)LayoutElementType.VirtualItem & x.ControlTypeName.ToLower() == "row");
                    int itemDefOffsetY = StyleSheet.DefaultOffsetY;

                    int rowPosY = 0;
                    foreach (var row in rows)
                    {
                        var rowItems = zoneItems.Where(x => (x.Type == (int)LayoutElementType.DisplayOnlyItem | x.Type == (int)LayoutElementType.DisplayAndTransactionItem
                            ) & x.ControlTypeName.ToLower() != "row" && x.RowName == row.Name).ToList();
                        int posX = 0;

                        int lastItemW = 0;
                        rowPosY = rowPosY + row.OffsetOrPositionY;
                        foreach (var rowItem in rowItems)
                        {
                            zoneItemNameForEx = rowItem.Name;
                            if (rowItem.OffsetOrPositionX != -1)
                            {

                                posX = posX + lastItemW + rowItem.OffsetOrPositionX;
                            }
                            else
                            {
                                posX = posX + lastItemW + itemDefOffsetX;
                            }

                            var posY = rowPosY;
                            if (rowItem.OffsetOrPositionY != -1)
                            {
                                posY = rowPosY + rowItem.OffsetOrPositionY;
                            }
                            else
                            {
                                posY = rowPosY + itemDefOffsetY;
                            }

                            //*localize and set size for item
                            var ctrlWidth = 0;
                            var ctrl = ControlHelper.GetControlByNameByParentForRecursionSubCall(rowItem.Name, zonePanel);
                            ctrl.Location = new Point(posX, posY);
                            if (rowItem.Width != -1) ctrl.Width = rowItem.Width;
                            if (rowItem.Height != -1) ctrl.Height = rowItem.Height;
                            if (rowItem.ControlTypeName == "Label" | rowItem.ControlTypeName == "TitleLabel" | rowItem.ControlTypeName == "CommandLabel")
                            {
                                ctrl.Width = rowItem.Width < 0 ? 0 : rowItem.Width;
                                ctrl.Height = rowItem.Height < 0 ? 20 : rowItem.Height;
                            }
                            else if (rowItem.ControlTypeName == "StatusLight")
                            {
                                ctrl.Width = rowItem.Width < 0 ? 0 : rowItem.Width;
                                ctrl.Height = rowItem.Height < 0 ? 23 : rowItem.Height;
                            }
                            else if (rowItem.ControlTypeName == "Button" | rowItem.ControlTypeName == "TextButton" | rowItem.ControlTypeName.StartsWith("ImageTextButton"))
                            {
                                ctrl.Location = new Point(posX, posY - 2);
                                ctrl.Width = rowItem.Width < 0 ? 0 : rowItem.Width;
                                ctrl.Height = rowItem.Height < 0 ? 23 : rowItem.Height;
                            }
                            else if (rowItem.ControlTypeName == "CheckBox")
                            {
                                ctrl.Width = rowItem.Width < 0 ? 0 : rowItem.Width;
                                ctrl.Height = rowItem.Height < 0 ? 20 : rowItem.Height;
                            }
                            else if (rowItem.ControlTypeName == "RadioButton")
                            {
                                ctrl.Width = rowItem.Width < 0 ? 0 : rowItem.Width;
                                //ctrl.Height = rowItem.Height != -1 ? item.Height : 20;//doesn't work ,ctrl.Height always=24
                            }
                            else if (rowItem.ControlTypeName == "RichTextBox")
                            {
                                ctrl.Location = new Point(posX, posY);
                                ctrl.Width = rowItem.Width < 0 ? 0 : rowItem.Width;
                                ctrl.Height = rowItem.Height < 0 ? 20 : rowItem.Height;
                            }
                            else if (rowItem.ControlTypeName == "ComboBox")
                            {
                                ctrl.Location = new Point(posX, posY);
                                ctrl.Width = rowItem.Width < 0 ? 0 : rowItem.Width;
                            }
                            else
                            {
                                //*size
                                if (rowItem.Width != -1) ctrl.Width = rowItem.Width;
                                if (rowItem.Height != -1) ctrl.Height = rowItem.Height;
                            }

                            ctrlWidth = ctrl.Width;
                            if (rowItem.ControlTypeName == "RadioButton")
                            {
                                ctrlWidth = 0;
                            }

                            lastItemW = ctrlWidth;
                        }
                        rowPosY = rowPosY + row.Height;
                    }
                }
                else //ZoneArrangementType.Positioning
                {

                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RenderZoneItems Error: zoneName=" + zoneNameForEx + "; itemName=" + zoneItemNameForEx + "; itemId=" + zoneItemIdForEx + "; " + ex.Message);
            }
        }

        //##SetZoneCpntDockStyleOrPositionOnZoneArrangementTypeIsPositioning
        private void SetZoneCpntDockStyleOrPositionOnZoneArrangementTypeIsPositioning(ZoneArrangementType zoneArrangementType, Panel zonePanel, Control ctrl, int dockType, int width, int height, int offsetOrPositionX, int offsetOrPositionY)
        {
            if (zoneArrangementType == ZoneArrangementType.Positioning)
            {
                ControlHelper.SetControlDockStyleAndLocationAndSize(ctrl, dockType, width, height, offsetOrPositionX, offsetOrPositionY);
                if (dockType > 0 && dockType < 5)
                {
                    if (offsetOrPositionX > 0 || offsetOrPositionY > 0)
                    {
                        var offsetCrtl = new Label();
                        ControlHelper.SetControlOffsetByDockStyle(offsetCrtl, dockType, offsetOrPositionX, offsetOrPositionY);
                        zonePanel.Controls.Add(offsetCrtl);
                    }
                }
            }

        }


        //##AddZoneIdentifer
        private string AddZoneIdentifer(string str, string zoneName)
        {
            try
            {
                if (str.IsNullOrEmpty()) return string.Empty;
                var str1 = str;
                if (str.Contains("#"))
                {
                    var strArray = str.Split('#');
                    int n = strArray.Count();
                    if (n % 2 == 0)
                    {
                        throw new ArgumentException(" '#' no. in " + str + " is not a even! ");
                    }
                    else
                    {
                        for (int i = 0; i < n; i++)
                        {
                            if (i % 2 == 1)
                            {
                                if (strArray[i].IsNullOrEmpty() | strArray[i] == ".")
                                {
                                    strArray[i] = zoneName;
                                }
                                else
                                    strArray[i] = zoneName + "_" + strArray[i];
                            }
                        }
                        str1 = string.Join("#", strArray);
                    }
                }

                var str2 = str1;
                if (str2.Contains("$"))
                {
                    var strArray = str2.Split('$');
                    int n = strArray.Count();
                    if (n % 2 == 0)
                    {
                        throw new ArgumentException(" '$' no. in " + str + " is not a even! ");
                    }
                    else
                    {
                        for (int i = 0; i < n; i++)
                        {
                            if (i % 2 == 1)
                            {
                                if (strArray[i].IsNullOrEmpty())
                                {
                                    strArray[i] = zoneName;
                                }
                                else
                                    strArray[i] = zoneName + "_" + strArray[i];
                            }
                        }
                        str2 = string.Join("$", strArray);
                    }
                }
                return str2;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".AddZoneIdentifer Error: Text=" + str + "; " + ex.Message);
            }
        }

        //##DeleteControlZoneIdentifier
        private string DeleteControlZoneIdentifier(string str)
        {
            try
            {
                if (str.Contains("$"))
                {
                    str = str.Replace("$", "");
                }
                return str;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".DeleteControlZoneIdentifier Error: Text=" + str + "; " + ex.Message);
            }
        }

        //##DeleteProcedureZoneIdentifier
        private string DeleteProcedureZoneIdentifier(string str)
        {
            try
            {
                if (str.Contains("#"))
                {
                    return str.Replace("#", "");
                }
                return str;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".DeleteProcedureZoneIdentifier Error: Text=" + str + "; " + ex.Message);
            }
        }

        //##Procedure
        //##InitZoneProcedures
        private void InitZoneProcedures(string zoneName, List<ProcedureItem> zoneProcedures)
        {
            try
            {
                foreach (var proc in zoneProcedures.Where(x => x.Type != (int)ProcedureItemType.None))
                {
                    proc.Formula = ResolveConstants(proc.Formula);
                    proc.Condition = ResolveConstants(proc.Condition);
                }

                if (zoneProcedures.Count != 0)
                {
                    var grpIds = zoneProcedures.Select(x => x.GroupId).Distinct();
                    foreach (var groupId in grpIds)
                    {
                        var procListByGrp = zoneProcedures.FindAll(x => x.GroupId == groupId & x.Type != (int)ProcedureItemType.None);
                        if (!RefreshProcedures(procListByGrp, zoneProcedures)) return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".InitZoneProcedures Error: zoneName = " + zoneName + "; " + ex.Message);
            }
        }

        //##RefreshZoneProcedures
        private void RefreshZoneProcedures(string zoneName)
        {
            try
            {
                var procList = new List<ProcedureItem>();
                if (string.IsNullOrEmpty(zoneName))
                {
                    procList = _procedures.FindAll(x => string.IsNullOrEmpty(x.ZoneName)); //for function procedures
                }
                else
                {
                    procList = _procedures.FindAll(x => x.ZoneName == zoneName);
                }
                if (procList.Count == 0)
                {
                    return;
                }

                if (procList.Count != 0)
                {
                    var grpIds = procList.Select(x => x.GroupId).Distinct();
                    foreach (var groupId in grpIds)
                    {
                        var procListByGrp = procList.FindAll(x => x.GroupId == groupId & x.Type != (int)ProcedureItemType.None);
                        if (!RefreshProcedures(procListByGrp, procList)) return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshZoneProcedures Error: zoneName=" + zoneName + "; " + ex.Message);
            }
        }

        //##RefreshZoneProceduresByGroup
        private void RefreshZoneProceduresByGroup(string zoneName, int grpId)
        {
            try
            {
                var procList = new List<ProcedureItem>();
                if (string.IsNullOrEmpty(zoneName))
                {
                    procList = _procedures.FindAll(x => string.IsNullOrEmpty(x.ZoneName)); //for function procedures
                }
                else
                {
                    procList = _procedures.FindAll(x => x.ZoneName == zoneName);
                }

                if (procList.Count == 0)
                {
                    return;
                }

                var procListByGrp = new List<ProcedureItem>();
                if (grpId > -1) //<1, for all group
                {
                    procListByGrp = procList.FindAll(x => x.GroupId == grpId & x.Type != (int)ProcedureItemType.None);
                }
                else
                {
                    procListByGrp = procList;
                }

                if (procList.Count != 0)
                {
                    if (!RefreshProcedures(procListByGrp, procList)) return;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshZoneProceduresByGroup Error: zoneName=" + zoneName + "; " + ex.Message);
            }
        }

        //##RefreshProcedures
        private bool RefreshProcedures(List<ProcedureItem> procListByGrp, List<ProcedureItem> procListAll)
        {
            var procNameForEx = "";
            try
            {
                foreach (var proc in procListByGrp)
                {
                    procNameForEx = proc.Name;
                    var conTxt = ResolveStringByRefProcedureVariables(proc.Condition, procListAll);
                    var con = conTxt.IsNullOrEmpty() ? string.Empty : GetText(conTxt);
                    bool toDo = string.IsNullOrEmpty(con) || con.ToLower() == "true";

                    if (toDo)
                    {
                        if (proc.Type == (int)ProcedureItemType.Variable)
                        {
                            if (!string.IsNullOrEmpty(proc.Formula))
                            {
                                var formularTxt = ResolveStringByRefProcedureVariables(proc.Formula, procListAll);
                                proc.Value = GetText(formularTxt);
                            }
                        }
                        else if (proc.Type == (int)ProcedureItemType.Action)
                        {
                            if (!string.IsNullOrEmpty(proc.Formula))
                            {
                                var formularTxt = ResolveStringByRefProcedureVariables(proc.Formula, procListAll);
                                formularTxt = GetText(formularTxt);
                                var displayName = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "ProcedureItem", proc.Name, _annexes, proc.DisplayName);
                                Act("", formularTxt, displayName, true);
                            }
                        }
                        else if (proc.Type == (int)ProcedureItemType.Break) break;
                        else if (proc.Type == (int)ProcedureItemType.Exit) return false;
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshProcedures Error: " + "procName=" + procNameForEx + "; " + ex.Message);
            }
        }

        //##ClearZoneProceduresVariablesByGroup
        private void ClearZoneProceduresVariablesByGroup(string zoneName, int groupId)
        {
            try
            {
                var procList = new List<ProcedureItem>();
                if (zoneName.IsNullOrEmpty())
                {
                    procList = _procedures.FindAll(x => x.ZoneName.IsNullOrEmpty() & x.Type == (int)ProcedureItemType.Variable);
                }
                else procList = _procedures.FindAll(x => x.ZoneName == zoneName & x.Type == (int)ProcedureItemType.Variable);

                if (procList.Count > 0)
                {
                    if (groupId > -1)//<1, for all group
                    {
                        procList = procList.FindAll(x => x.GroupId == groupId);
                    }

                    var grpIds = procList.Select(x => x.GroupId).Distinct();
                    foreach (var grpId in grpIds)
                    {
                        foreach (var var in procList.Where(x => x.GroupId == grpId))
                        {
                            var.Value = string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".ClearZoneProceduresVariablesByGroup Error: " + ex.Message);
            }
        }

        //##RefreshProcedureVariable
        private void RefreshProcedureVariable(string varName)
        {
            try
            {
                var var = _procedures.Find(x => x.Name == varName & x.Type == (int)ProcedureItemType.Variable);
                if (var == null)
                {
                    throw new ArgumentException("Procedure Variable name does not exist! varName=" + varName);
                }

                var vars = _procedures.FindAll(x => x.ZoneName == var.ZoneName);
                {

                    var conTxt = ResolveStringByRefProcedureVariables(var.Condition, vars);
                    var con = GetText(conTxt);
                    if (string.IsNullOrEmpty(con) || con.ToLower() == "true")
                    {
                        var formularTxt = ResolveConstants(var.Formula);
                        formularTxt = ResolveStringByRefProcedureVariables(formularTxt, vars);
                        var.Value = GetText(formularTxt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshProcedureVariable Error: " + ex.Message);
            }
        }

        //##SetProcedureVariableValue
        private void SetProcedureVariableValue(string varName, string val)
        {
            try
            {
                var var = _procedures.Find(x => x.Name == varName & x.Type == (int)ProcedureItemType.Variable);
                if (var != null)
                {
                    var.Value = val;
                }
                else
                {
                    throw new ArgumentException("Procedure Variable: " + varName + " does not exist!");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".SetProcedureVariableValue Error:" + ex.Message);
            }
        }

        //##ctrl
        //##control
        //##RefreshControl
        protected void RefreshControl(string ctrlName)
        {
            try
            {
                var item = _zonesItems.Find(x => x.Name == ctrlName);
                if (item == null) throw new ArgumentException("ZoneItem doesn't exist! ZoneItem=" + ctrlName);

                var ctrl = this.GetControl(item.Name);

                if (item.ControlTypeName == "StatusLight")
                {
                    var cpnt = ctrl as StatusLight;
                    cpnt.Refresh();
                }
                else if (item.ControlTypeName == "ScoreLight")
                {
                    var cpnt = ctrl as ScoreLight;
                    cpnt.Refresh();
                }
                else if (item.ControlTypeName == "RadioButton")
                {
                    var cpnt = ctrl as RadioButton;
                    cpnt.Refresh();
                }
                else if (item.ControlTypeName == "CheckBox")
                {
                    var cpnt = ctrl as CheckBox;
                    cpnt.Refresh();
                }
                else if (item.ControlTypeName == "TextBox")
                {
                    var cpnt = ctrl as TextBox;
                    cpnt.Refresh();
                }
                else if (item.ControlTypeName == "RichTextBox")
                {
                    var cpnt = ctrl as RichTextBox;
                    cpnt.Refresh();
                }
                else if (item.ControlTypeName == "ComboBox")
                {
                    var cpnt = ctrl as ComboBox;
                    cpnt.Refresh();
                }
                else if (item.ControlTypeName == "ProgressBar")
                {
                    var cpnt = ctrl as ProgressBar;
                    cpnt.Refresh();
                }
               
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + this.GetType() + ".RefreshControl Error: " + ex.Message);
            }
        }
        //##RefreshZoneControlsValues
        private void RefreshZoneControlsValues(string zoneName)
        {
            try
            {
                var zoneItems = _zonesItems.FindAll(x => x.Name.StartsWith(zoneName + "_"));
                var items = zoneItems.Where(x => x.ControlTypeName.ToLower() != "row"
                                                 & (x.Type == (int)LayoutElementType.DisplayOnlyItem | x.Type == (int)LayoutElementType.DisplayAndTransactionItem)
                ).ToList();
                foreach (var item in items)
                {
                    RefreshControlValue(item.Name);
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshZoneValues Error: " + ex.Message);
            }
        }

        //##RefreshZoneControlsInvisibles
        private void RefreshZoneControlsInvisibles(string zoneName)
        {
            try
            {
                var zoneItems = _zonesItems.FindAll(x => x.Name.StartsWith(zoneName + "_"));
                var items = zoneItems.Where(x => x.ControlTypeName.ToLower() != "row"
                                                 & (x.Type == (int)LayoutElementType.DisplayOnlyItem | x.Type == (int)LayoutElementType.DisplayAndTransactionItem)
                ).ToList();
                foreach (var item in items)
                {
                    RefreshControlInvisible(item.Name);
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshZoneControlsInvisibles Error: " + ex.Message);
            }
        }

        //##RefreshZoneControlsDisableds
        private void RefreshZoneControlsDisableds(string zoneName)
        {
            try
            {
                //zoneName = ReplaceAbbreviatedCurrentZoneName(zoneName);
                var zoneItems = _zonesItems.FindAll(x => x.Name.StartsWith(zoneName + "_"));
                var items = zoneItems.Where(x => x.ControlTypeName.ToLower() != "row"
                                                 & (x.Type == (int)LayoutElementType.DisplayOnlyItem | x.Type == (int)LayoutElementType.DisplayAndTransactionItem)
                ).ToList();
                foreach (var item in items)
                {
                    RefreshControlDisabled(item.Name);
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshZoneControlsDisableds Error: " + ex.Message);
            }
        }

        //##RefreshControlValue
        private void RefreshControlValue(string ctrlName)
        {
            try
            {
                var item = _zonesItems.Find(x => x.Name == ctrlName);
                if (item == null) throw new ArgumentException("ZoneItem doesn't exist! ZoneItem=" + ctrlName);

                //defVal
                var defVal = "";
                if (!String.IsNullOrEmpty(item.DefaultValue))
                {
                    var txt = ResolveStringByRefProcedureVariablesAndControls(item.DefaultValue);
                    defVal = GetText(txt);
                }

                SetControlValue(ctrlName, defVal);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshControlValue Error: " + ex.Message);
            }
        }

        //##SetControlValue
        protected void SetControlValue(string ctrlName, string val)
        {
            try
            {
                var item = _zonesItems.Find(x => x.Name == ctrlName);
                if (item == null) throw new ArgumentException("ZoneItem doesn't exist! ZoneItem=" + ctrlName);

                //*refresh item
                var ctrl = this.GetControl(item.Name);

                if (item.ControlTypeName == "StatusLight")
                {
                    var cpnt = ctrl as StatusLight;
                    cpnt.Value = Convert.ToInt16(val);
                }
                else if (item.ControlTypeName == "ScoreLight")
                {
                    var cpnt = ctrl as ScoreLight;
                    cpnt.Value = Convert.ToSingle(val, CultureInfo.InvariantCulture);
                }
                else if (item.ControlTypeName == "RadioButton")
                {
                    var cpnt = ctrl as RadioButton;
                    if (val.ToLower() == "true")
                    {
                        cpnt.Checked = true;
                    }
                    else
                    {
                        cpnt.Checked = false;
                    }
                }
                else if (item.ControlTypeName == "CheckBox")
                {
                    var cpnt = ctrl as CheckBox;
                    if (val.ToLower() == "true")
                    {
                        cpnt.Checked = true;
                    }
                    else
                    {
                        cpnt.Checked = false;
                    }
                }
                else if (item.ControlTypeName == "TextBox")
                {
                    var cpnt = ctrl as TextBox;
                    cpnt.Text = val;
                }
                else if (item.ControlTypeName == "RichTextBox")
                {
                    var cpnt = ctrl as RichTextBox;
                    cpnt.Text = val;
                }
                else if (item.ControlTypeName == "ComboBox")
                {
                    var cpnt = ctrl as ComboBox;
                    cpnt.SelectedValue = val;
                }
                else if (item.ControlTypeName == "ProgressBar")
                {
                    var cpnt = ctrl as ProgressBar;
                    cpnt.Value = Convert.ToInt32(val);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + this.GetType() + ".SetControlValue Error: " + ex.Message);
            }
        }


        //##RefreshControlText
        private void RefreshControlText(string ctrlName)
        {
            try
            {
                var item = _zonesItems.Find(x => x.Name == ctrlName);
                if (item == null) throw new ArgumentException("ZoneItem doesn't exist! ZoneItem=" + ctrlName);
                var text = "";
                if (!item.DisplayName.IsNullOrEmpty())
                {
                    var txt = ResolveStringByRefProcedureVariablesAndControls(item.DisplayName);
                    text = GetText(txt);
                }

                if (item.ControlTypeName == "PictureBox")
                {
                    if (!item.DisplayName.IsNullOrEmpty())
                    {
                        if (item.DisplayName.StartsWith("="))
                        {
                            var txt = ResolveStringByRefProcedureVariablesAndControls(item.DisplayName);
                            text = GetText(txt);
                        }
                    }
                }
                SetControlText(item, text);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshControlText Error: " + ex.Message);
            }
        }


        //##SetControlText
        protected void SetControlText(string ctrlName, string text)
        {
            try
            {
                var item = _zonesItems.Find(x => x.Name == ctrlName);
                if (item == null) throw new ArgumentException("ZoneItem doesn't exist! ZoneItem=" + ctrlName);
                SetControlText(item, text);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + this.GetType() + ".SetControlText Error: " + ex.Message);
            }
        }

        //##SetControlText
        private void SetControlText(ZoneItem item, string text)
        {
            try
            {
                var ctrl = this.GetControl(item.Name);
                if (item.ControlTypeName.ToLower().Contains("label"))
                {
                    ctrl.Text = text;
                }
                else if (item.ControlTypeName == "StatusLight")
                {
                    var cpnt = ctrl as StatusLight;
                    cpnt.Text = text;
                }
                else if (item.ControlTypeName == "ScoreLight")
                {
                    var cpnt = ctrl as ScoreLight;
                    cpnt.Text = text;
                }
                else if (item.ControlTypeName == "RadioButton")
                {
                    ctrl.Text = text;
                }
                else if (item.ControlTypeName == "CheckBox")
                {
                    ctrl.Text = text;
                }
                else if (item.ControlTypeName == "ComboBox")
                {
                    var cpnt = ctrl as ComboBox;
                    cpnt.SelectedText = text;
                }
                else if (item.ControlTypeName == "PictureBox")
                {
                    ctrl.Tag = text;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshControlText Error: " + ex.Message);
            }
        }

        //##GetControlValueOrText
        protected string GetControlValueOrText(string txt)
        {
            try
            {
                var ctrlName = txt.Trim();
                if (txt.ToLower().EndsWith(".t") | txt.ToLower().EndsWith(".v"))
                {
                    ctrlName = txt.Split('.')[0].Trim();
                }
                else
                {
                    //return txt;
                    throw new ArgumentException("Control value or text should end with .t or .v! ");
                }

                var item = _zonesItems.Find(x => x.Name == ctrlName);
                if (item == null) throw new ArgumentException("Zone item doesn't exist! Item=" + txt);
                var typeName = item.ControlTypeName;

                var ctrl = this.GetControl(item.Name);

                var retStr = "";
                if (typeName == "ComboBox")
                {
                    var cpnt = ctrl as ComboBox;
                    if (cpnt != null)
                    {
                        if (txt.ToLower().EndsWith(".t"))
                        {
                            retStr = Convert.ToString(cpnt.Text);
                        }
                        else
                        {
                            retStr = Convert.ToString(cpnt.SelectedValue);
                        }
                    }
                }
                else if (typeName == "RadioButton")
                {
                    var cpnt = ctrl as RadioButton;
                    if (cpnt != null)
                    {
                        if (txt.ToLower().EndsWith(".t"))
                        {
                            throw new ArgumentException("Express error: " + txt);
                        }
                        else
                        {
                            retStr = (cpnt.Checked) ? "true" : "false";
                        }
                    }
                }
                else if (typeName == "CheckBox")
                {
                    var cpnt = ctrl as CheckBox;
                    if (cpnt != null)
                    {
                        if (txt.ToLower().EndsWith(".t"))
                        {
                            throw new ArgumentException("Express error: " + txt);
                        }
                        else
                        {
                            retStr = (cpnt.Checked) ? "true" : "false";
                        }
                    }
                }
                else if (typeName == "StatusLight")
                {
                    var cpnt = ctrl as StatusLight;
                    if (cpnt != null)
                    {
                        if (txt.ToLower().EndsWith(".t"))
                        {
                            retStr = Convert.ToString(cpnt.Text);
                        }
                        else
                        {
                            retStr = Convert.ToString(cpnt.Value);
                        }

                    }
                }
                else if (typeName == "ScoreLight")
                {
                    var cpnt = ctrl as ScoreLight;
                    if (cpnt != null)
                    {
                        if (txt.ToLower().EndsWith(".t"))
                        {
                            retStr = Convert.ToString(cpnt.Text);
                        }
                        else
                        {
                            retStr = Convert.ToString(cpnt.Value, CultureInfo.InvariantCulture);
                        }

                    }
                }
                else if (typeName.Contains("ProgressBar"))
                {
                    var cpnt = ctrl as ProgressBar;
                    if (txt.ToLower().EndsWith(".v"))
                    {
                        retStr = Convert.ToString(cpnt.Value);
                    }
                    else
                    {
                        throw new ArgumentException("Express error: " + txt);
                    }
                }


                else if (typeName.Contains("Label"))
                {
                    if (txt.ToLower().EndsWith(".t"))
                    {
                        retStr = ctrl.Text;
                    }
                    else
                    {
                        throw new ArgumentException("Express error: " + txt);
                    }

                }

                else //text, richtextbox
                {
                    if (txt.ToLower().EndsWith(".v"))
                    {
                        retStr = ctrl.Text;
                    }
                    else
                    {
                        throw new ArgumentException("Express error: " + txt);
                    }
                }
                return retStr;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetControlValue Error: " + ex.Message);
            }
        }

        //##RefreshControlDataSource
        private void RefreshControlDataSource(string ctrlName)
        {
            try
            {
                var item = _zonesItems.Find(x => x.Name == ctrlName);
                if (item == null) throw new ArgumentException("ZoneItem doesn't exist! ZoneItem=" + ctrlName);

                //refresh item
                var ctrl = GetControl(ctrlName);
                if (item.ControlTypeName == "ComboBox")
                {
                    var cpnt = ctrl as ComboBox;
                    if (!String.IsNullOrEmpty(item.DataSource))
                    {
                        var dataSrc = item.DataSource;
                        dataSrc = ResolveConstants(dataSrc);
                        dataSrc = ResolveStringByRefProcedureVariablesAndControls(dataSrc);

                        var valTxts = new List<ValueText>();
                        if (dataSrc.StartsWith("="))
                        {
                            dataSrc = GetText(dataSrc);
                            if (dataSrc.Contains("{") & dataSrc.Contains("}"))
                            {
                                valTxts = JsonHelper.ConvertToObject<List<ValueText>>(dataSrc);
                            }
                        }
                        else
                        {
                            var strArray = dataSrc.Split(dataSrc.GetSubParamSeparator());
                            if (dataSrc.Contains("|"))
                            {
                                foreach (var v in strArray)
                                {
                                    var arry = v.Split('|');
                                    var valTxt = new ValueText();
                                    valTxt.Value = arry[0];
                                    valTxt.Text = arry[1];
                                    valTxts.Add(valTxt);
                                }
                            }
                            else
                            {
                                var i = 0;
                                foreach (var v in strArray)
                                {
                                    var valTxt = new ValueText();
                                    valTxt.Value = i.ToString();
                                    valTxt.Text = v;
                                    valTxts.Add(valTxt);
                                    i++;
                                }
                            }
                        }

                        cpnt.DataSource = valTxts;
                        cpnt.ValueMember = "Value";
                        cpnt.DisplayMember = "Text";
                        //cpnt.SelectedIndex = selectedIndex;

                    }
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshControlValue Error: " + ex.Message);
            }
        }

        //##RefreshControlInvisible
        private void RefreshControlInvisible(string ctrlName)
        {
            try
            {
                var item = _zonesItems.Find(x => x.Name == ctrlName);
                if (item == null) throw new ArgumentException("ZoneItem doesn't exist! ZoneItem=" + ctrlName);
                //Visible
                bool isCpntVisible = true;
                var inVisibleFlag = item.InvisibleFlag;
                if (string.IsNullOrEmpty(inVisibleFlag)) inVisibleFlag = "false";
                else
                {
                    var txt = ResolveStringByRefProcedureVariablesAndControls(item.InvisibleFlag);
                    inVisibleFlag = GetText(txt);
                }
                isCpntVisible = (inVisibleFlag.ToLower() == "false" | inVisibleFlag.ToLower() == "0") ? true : false;

                //refresh item
                var ctrl = GetControl(item.Name);
                if (!isCpntVisible) ctrl.Visible = false; else ctrl.Visible = true;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshControlInvisible Error: " + ex.Message);
            }
        }

        //##SetControlVisible
        private void SetControlVisible(string ctrlName, bool isVisible)
        {
            try
            {
                var item = _zonesItems.Find(x => x.Name == ctrlName);
                if (item == null) throw new ArgumentException("ZoneItem doesn't exist! ZoneItem=" + ctrlName);

                //set item
                var ctrl = GetControl(item.Name);
                if (!isVisible) ctrl.Visible = false; else ctrl.Visible = true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".SetControlVisible Error: " + ex.Message);
            }
        }

        //##RefreshControlDisabled
        private void RefreshControlDisabled(string ctrlName)
        {
            try
            {
                var item = _zonesItems.Find(x => x.Name == ctrlName);
                if (item == null) throw new ArgumentException("ZoneItem doesn't exist! ZoneItem=" + ctrlName);

                //Enabled
                bool isCpntEnabled = true;
                var disabledFlag = item.DisabledFlag;
                if (string.IsNullOrEmpty(disabledFlag)) disabledFlag = "false";
                else
                {
                    var txt = ResolveStringByRefProcedureVariablesAndControls(item.DisabledFlag);
                    disabledFlag = GetText(txt);
                }
                isCpntEnabled = (disabledFlag.ToLower() == "false" | disabledFlag.ToLower() == "0") ? true : false;

                var ctrl = GetControl(item.Name);
                if (item.ControlTypeName == "TextBox" | item.ControlTypeName == "RichTextBox")
                {
                    var cpnt = ctrl as TextBox;
                    if (!isCpntEnabled) cpnt.ReadOnly = true; else cpnt.ReadOnly = false;
                }
                else
                {
                    if (!isCpntEnabled) ctrl.Enabled = false; else ctrl.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshControlDisabled Error: " + ex.Message);
            }
        }

        //##SetControlEnabled
        private void SetControlEnabled(string ctrlName, bool isEnabled)
        {
            try
            {
                var item = _zonesItems.Find(x => x.Name == ctrlName);
                if (item == null) throw new ArgumentException("ZoneItem doesn't exist! ZoneItem=" + ctrlName);

                //set item
                var ctrl = GetControl(item.Name);
                if (item.ControlTypeName == "TextBox" | item.ControlTypeName == "RichTextBox")
                {
                    var cpnt = ctrl as TextBox;
                    if (!isEnabled) cpnt.ReadOnly = true; else cpnt.ReadOnly = false;
                }
                else
                {
                    if (!isEnabled) ctrl.Enabled = false; else ctrl.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".SetControlEnabled Error: " + ex.Message);
            }
        }

        //#common
        //##resolve
        private string ResolveConstants(string text)
        {
            if (text.IsNullOrEmpty()) return string.Empty;
            if (!text.Contains("%")) return text;

            var toBeRplStr = "%AppCode%".ToLower();
            if (text.ToLower().Contains(toBeRplStr))
            {
                var rplStr = _functionInitParamSet.ApplicationCode;
                text = Regex.Replace(text, toBeRplStr, rplStr, RegexOptions.IgnoreCase);
            }

            toBeRplStr = "%AppVersion%".ToLower();
            if (text.ToLower().Contains(toBeRplStr))
            {
                var rplStr = _functionInitParamSet.ApplicationVersion;
                return Regex.Replace(text, toBeRplStr, rplStr, RegexOptions.IgnoreCase);
            }

            toBeRplStr = "%FuncCode%".ToLower();
            if (text.ToLower().Contains(toBeRplStr))
            {
                var rplStr = _functionInitParamSet.FunctionCode;
                text = Regex.Replace(text, toBeRplStr, rplStr, RegexOptions.IgnoreCase);
            }

            toBeRplStr = "%AppDir%".ToLower();
            if (text.ToLower().Contains(toBeRplStr))
            {
                var rplStr = _appDir;
                text = Regex.Replace(text, toBeRplStr, rplStr, RegexOptions.IgnoreCase);
            }

            toBeRplStr = "%FormDir%".ToLower();
            if (text.ToLower().Contains(toBeRplStr))
            {
                var rplStr = _formDir;
                text = Regex.Replace(text, toBeRplStr, rplStr, RegexOptions.IgnoreCase);
            }

            toBeRplStr = "%FuncsDir%".ToLower();
            if (text.ToLower().Contains(toBeRplStr))
            {
                var rplStr = _functionsDir;
                text = Regex.Replace(text, toBeRplStr, rplStr, RegexOptions.IgnoreCase);
            }

            toBeRplStr = "%ZonesDir%".ToLower();
            if (text.ToLower().Contains(toBeRplStr))
            {
                var rplStr = _zonesDir;
                text = Regex.Replace(text, toBeRplStr, rplStr, RegexOptions.IgnoreCase);
            }

            toBeRplStr = "%ImplDir%".ToLower();
            if (text.ToLower().Contains(toBeRplStr))
            {
                var rplStr = _implDir;
                text = Regex.Replace(text, toBeRplStr, rplStr, RegexOptions.IgnoreCase);
            }

            toBeRplStr = "%AppDataDir%".ToLower();
            if (text.ToLower().Contains(toBeRplStr))
            {
                var rplStr = _appDataDir;
                text = Regex.Replace(text, toBeRplStr, rplStr, RegexOptions.IgnoreCase);
            }

            if (!text.Contains("%"))
            {
                return text;
            }
            else
            {
                var retStr = FunctionHelper.ResolveConstants(text);
                if (!retStr.Contains("%"))
                {
                    return retStr;
                }
                else
                {
                    return ResolveConstantsEx(text);
                }
            }
        }



        //##ResolveStringByRefProcedureVariablesAndControls
        private string ResolveStringByRefProcedureVariablesAndControls(string str)
        {
            try
            {
                if (str.IsNullOrEmpty()) return "";
                if (str.Contains("#"))
                {
                    str = ResolveStringByRefProcedureVariables(str, _procedures.Where(x => x.Type == (int)ProcedureItemType.None | x.Type == (int)ProcedureItemType.Variable).ToList());
                }
                if (str.Contains("$"))
                {
                    str = ResolveStringByRefControls(str, _zonesItems);
                }
                return str;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".ResolveStringByRefProcedureVariablesAndControls Error: " + ex.Message);
            }
        }

        //##ResolveStringByRefProcedureVariables
        private string ResolveStringByRefProcedureVariables(string str)
        {
            try
            {
                if (str.IsNullOrEmpty()) return "";
                if (str.Contains("#"))
                {
                    str = ResolveStringByRefProcedureVariables(str, _procedures.Where(x => x.Type == (int)ProcedureItemType.None | x.Type == (int)ProcedureItemType.Variable).ToList());
                }
                return str;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".ResolveStringByRefProcedureVariables Error: " + ex.Message);
            }
        }

        private string ResolveStringByRefProcedureVariables(string str, List<ProcedureItem> varList)
        {
            try
            {
                var vars = varList.Where(x => x.Type == (int)ProcedureItemType.None | x.Type == (int)ProcedureItemType.Variable).ToList();
                if (str.IsNullOrEmpty()) return "";
                if (!str.Contains("#")) return str;
                var strArray = str.Split('#');
                int n = strArray.Count();
                if (n % 2 == 0)
                {
                    throw new ArgumentException(" '#' no. in " + str + " is not a even! ");
                }
                else
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (i % 2 == 1)
                        {
                            if (strArray[i].IsPlusIntegerOrZero()) //func start input var
                            {
                                var tempStr = strArray[i];
                                if (tempStr == "0" | tempStr.IsPlusInteger())
                                {
                                    var funcParamVar = vars.Find(x => x.Name == "finput");
                                    if (funcParamVar != null)
                                    {
                                        var funcParamArry = funcParamVar.Value.Split('^');
                                        var index = Convert.ToInt16(tempStr);
                                        strArray[i] = funcParamArry[index];
                                    }
                                }
                            }
                            else if (strArray[i].GetLastSeparatedString('_').IsPlusIntegerOrZero()) //zone input var
                            {
                                var arry = strArray[i].Split('_');
                                if (arry.Length > 1)
                                {
                                    var lastStr = arry[arry.Length - 1];
                                    if (lastStr == "0" | lastStr.IsPlusInteger())
                                    {
                                        var zoneName = "";
                                        for (int ii = 0; ii < arry.Length - 1; ii++)
                                        {
                                            zoneName = ii == 0 ? arry[ii] : zoneName + "_" + arry[ii];
                                        }

                                        var zoneParamVar = vars.Find(x => x.Name == zoneName + "_zinput");
                                        if (zoneParamVar != null)
                                        {
                                            var zoneParamArray = zoneParamVar.Value.Split('^');
                                            var index = Convert.ToInt16(lastStr);
                                            strArray[i] = zoneParamArray[index];
                                        }
                                    }
                                    else throw new ArgumentException("Procedure variable value: #" + strArray[i] + "# doesn't exsit! ");
                                }
                            }
                            else if (strArray[i].ToLower().EndsWith(".v"))
                            {
                                var varName = strArray[i].Split('.')[0].Trim();
                                var variable1 = vars.Find(x => x.Name == varName);
                                strArray[i] = variable1.Value;
                            }
                            else
                            {
                                //throw new ArgumentException("Procedure: #" + strArray[i] + "# doesn't exsit! ");
                                strArray[i] = "#" + strArray[i] + "#";
                            }
                        }

                    }
                    return string.Join("", strArray);
                }
            }

            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".ResolveStringByRefProcedureVariables Error: str=" + str + "; " + ex.Message);
            }
        }

        //##ResolveStringByRefControls
        private string ResolveStringByRefControls(string str, List<ZoneItem> zoneItems)
        {
            try
            {
                var strArray = str.Split('$');
                int n = strArray.Count();
                if (n % 2 == 0)
                {
                    throw new ArgumentException(" '$' no. in " + str + " is not an even! ");
                }
                else
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (i % 2 == 1)
                        {
                            var txt = strArray[i];
                            if (txt.ToLower().EndsWith(".v") | txt.ToLower().EndsWith(".t"))
                                strArray[i] = GetControlValueOrText(txt);
                            else strArray[i] = "$" + strArray[i] + "$";
                        }
                    }
                    return string.Join("", strArray);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".ResolveStringByRefControls Error: str='" + str + "'; " + ex.Message);
            }
        }

        //##GetText
        protected string GetText(string str)
        {
            var funcName = "";
            var funcParams = "";
            try
            {
                if (str.IsNullOrEmpty()) //return string.Empty;
                {
                    return string.Empty;
                }

                if (!str.StartsWith("="))
                {
                    return str;
                }
                else
                {
                    bool cleanAlienText = false;
                    if (str.EndsWith(";CAT"))
                    {
                        str = str.Substring(0, str.Length - 4);
                        cleanAlienText = true;
                    }
                    else if (str.EndsWith(";CLAT"))
                    {
                        str = str.Substring(0, str.Length - 4);
                        cleanAlienText = true;
                    }


                    var funcNameAndParamsStr = str.Substring(1, str.Length - 1).Trim();
                    var separator = funcNameAndParamsStr.GetParamSeparator();

                    var funNameAndParamsArray = funcNameAndParamsStr.Split(separator);
                    funcName = funNameAndParamsArray[0].Trim().ToLower();
                    var funcParamArray = new string[funNameAndParamsArray.Length - 1];
                    for (int i = 0; i < funNameAndParamsArray.Length - 1; i++)
                    {
                        funcParamArray[i] = funNameAndParamsArray[i + 1];
                    }
                    funcParams = StringExtension.JoinStringArrayBySeparator(funcParamArray, separator);

                    var returnText = "";

                    if (funcName == "Empty".ToLower())
                    {
                        return string.Empty;
                    }
                    if (funcName == "Equal".ToLower())
                    {
                        return funcParamArray[0];
                    }
                    else if (funcName == "GetUserCode".ToLower())
                    {
                        returnText = GetCurrentUserCode();
                    }

                    else if (funcName == "currentViewMenuName".ToLower())
                    {
                        returnText = CurrentViewMenuName;
                    }

                    else if (funcName == "GetPhraseAnnexText".ToLower())
                    {
                        var className = "Phrase";
                        var masterName = funcParamArray[0];
                        var annexType = AnnexTextType.DisplayName;
                        if (funcParamArray.Length > 1)
                        {
                            var annexTypeStr = funcParamArray[1];
                            annexType = AnnexHelper.GetTextType(annexTypeStr);
                        }
                        var tempStr = "";
                        if (_functionInitParamSet.SupportMultiCultures)
                        {
                            tempStr = AnnexHelper.GetText(className, masterName, _annexes, annexType, CultureHelper.CurrentLanguageCode, GetAnnexMode.OnlyByCurLang);
                            if (tempStr.IsNullOrEmpty()) tempStr = masterName;
                        }
                        else
                        {
                            tempStr = masterName;
                        }


                        returnText = tempStr;
                    }
                    else if (funcName == "GetAbbrevAnnexText".ToLower())
                    {
                        var className = "Abbrev";
                        var masterName = funcParamArray[0];
                        var annexType = AnnexTextType.DisplayName;
                        if (funcParamArray.Length > 1)
                        {
                            var annexTypeStr = funcParamArray[1];
                            annexType = AnnexHelper.GetTextType(annexTypeStr);
                        }
                        var tempStr = "";
                        if (_functionInitParamSet.SupportMultiCultures)
                        {
                            tempStr = AnnexHelper.GetText(className, masterName, _annexes, annexType, CultureHelper.CurrentLanguageCode, GetAnnexMode.StepByStep);
                        }
                        else
                        {
                            tempStr = AnnexHelper.GetText(className, masterName, _annexes, annexType, "", GetAnnexMode.FirstAnnex);
                        }
                        returnText = tempStr;
                    }
                    else if (funcName == "GetControlsValidationResult".ToLower())
                    {
                        var ctrlArray = funcParamArray[0].SplitThenTrim(funcParamArray[0].GetSubParamSeparator());
                        foreach (var ctrlName in ctrlArray)
                        {
                            var ctrlName1 = DeleteControlZoneIdentifier(ctrlName.Trim());
                            var displayName = LayoutHelper.GetControlDisplayName(_functionInitParamSet.SupportMultiCultures, "ZoneItem", ctrlName1, _annexes, (GetControl(ctrlName1).Name).GetLastSeparatedString('_'));
                            var ctrlValue = GetControlValueOrText(ctrlName1 + ".v");
                            var ctrlValidateRules = _zonesItems.Find(x => x.Name == ctrlName1).ValidationRules;
                            if (ctrlValidateRules.IsNullOrEmpty())
                            {
                                return "true";
                            }
                            else
                            {
                                var ruleArry = ctrlValidateRules.Split(ctrlValidateRules.GetParamSeparator());
                                foreach (var rule in ruleArry)
                                {
                                    var funcParamArray1 = new string[] { ctrlValue, rule };
                                    var validationResult = FunctionHelper.GetText("GetValidationResult", funcParamArray1);

                                    if (validationResult == "true") continue;
                                    else if (validationResult != "OutOfScope") //false
                                    {
                                        returnText = (displayName + ": " + validationResult);
                                        return cleanAlienText ? returnText.CleanAlienText() : returnText;
                                    }
                                    else
                                    {
                                        validationResult = GetTextEx("GetValidationResult", funcParamArray1);
                                        if (validationResult == "true") continue;
                                        returnText = (displayName + ": " + validationResult);
                                        return cleanAlienText ? returnText.CleanAlienText() : returnText;
                                    }
                                }
                            }
                        }
                        returnText = "true";

                    }
                    else if (funcName == "GetBool".ToLower())
                    {
                        if (funcParamArray[0] == "IsFormModal")
                        {
                            returnText = (this.Modal).ToString();
                        }
                        else
                        {
                            returnText = GetTextFromLayoutParser(funcName, funcParamArray);
                        }
                    }
                    else
                    {
                        returnText = GetTextFromLayoutParser(funcName, funcParamArray);
                    }

                    return cleanAlienText ? returnText.CleanAlienText() : returnText;
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".GetText Error: funcName=" + funcName + ", Parameters=" + funcParams + "; " + ex.Message);
            }
        }

        //##GetTextFromLayoutParser
        private string GetTextFromLayoutParser(string funcName, string[] funcParamArray)
        {
            var returnStr = FunctionHelper.GetText(funcName, funcParamArray);
            if (returnStr == "OutOfScope")
            {
                return GetTextEx(funcName, funcParamArray);
            }
            else
            {
                return returnStr;
            }
        }



        //##Act
        private void Act(string controlName, string action, string transactionDisplayName, bool isTopLevelAction)
        {
            if (action.ToLower() == "DoNothing".ToLower()) return;

            try
            {
                if (FunctionHelper.IsZoneInputProcedure(action))
                {
                    action = ResolveStringByRefProcedureVariables(action);
                }
                else if (action.StartsWith("="))
                {
                    action = ResolveStringByRefProcedureVariablesAndControls(action);
                    action = GetText(action);
                }
                action = ResolveConstants(action);

                var separatorChar = action.GetParamSeparator();
                var actionNameAndParamsArray = action.Split(separatorChar);
                var actionName = actionNameAndParamsArray[0].Trim().ToLower();
                var actionParamArray = new string[actionNameAndParamsArray.Length - 1];
                for (int i = 0; i < actionNameAndParamsArray.Length - 1; i++)
                {
                    actionParamArray[i] = GetText(ResolveStringByRefProcedureVariablesAndControls(actionNameAndParamsArray[i + 1].Trim()));
                }
                var actionParams = StringExtension.JoinStringArrayBySeparator(actionParamArray, separatorChar);

                var actionSubParamSeparator = actionParams.GetSubParamSeparator();
                if (actionName.IsNullOrEmpty()) return;

                var clearMsg = false;
                if (isTopLevelAction & actionName.ToLower() != "Implement".ToLower())
                {
                    var msg = WinformRes.Dispensing;
                    var txt = ResolveStringByRefProcedureVariablesAndControls(transactionDisplayName);
                    var msg1 = transactionDisplayName.IsNullOrEmpty() ? actionName : GetText(txt);
                    msg1 = "'" + msg1 + "'";
                    var msg2 = "";
                    var msg3 = ", " + WinformRes.PleaseWait + "...";
                    if (!transactionDisplayName.IsNullOrEmpty())
                    {
                        RefreshRunningStatusMessageComponent(msg, msg1, msg2, msg3);
                        clearMsg = true;
                    }
                }

                var returnStr = "";

                if (actionName.ToLower() == "Implement".ToLower())
                {
                    var actionName1 = actionParamArray[0];
                    var ctrlsNames = "";
                    if (actionParamArray.Length > 1)
                    {
                        ctrlsNames = actionParamArray[1];
                        ctrlsNames = DeleteControlZoneIdentifier(ctrlsNames);
                    }
                    var vals = "";
                    if (actionParamArray.Length > 2)
                    {
                        vals = GetText(actionParamArray[2]);
                    }
                    Implement(actionName1, controlName, ctrlsNames, vals);
                    return;
                }

                if (actionName.ToLower() == "Lrun".ToLower())
                {
                    var tempStrArry = actionParams.SplitByTwoDifferentStrings("[", "]", false);
                    if (tempStrArry.Length != 1)
                        throw new ArgumentException("Lrun should only contains a pair of separatorChar []");
                    var separatedStr = tempStrArry[0];
                    var separatedStrArray = separatedStr.Split(tempStrArry[0].GetSubParamSeparator());
                    var preStr = actionParams.Split('[')[0];
                    var postStr = actionParams.Split(']')[1];
                    var tempStrArry1 = preStr.Split(preStr.GetParamSeparator());
                    var actionName1 = tempStrArry1[0];
                    foreach (var lrunParam in separatedStrArray)
                    {
                        var actionParams1 = tempStrArry1[1] + lrunParam + postStr;
                        Act(controlName, actionName1 + separatorChar + actionParams1, "", false);
                    }
                }

                else if (actionName.ToLower() == "Xrun".ToLower())
                {
                    Xrun(actionParams);
                }
                else if (actionName.ToLower() == "Crun".ToLower())
                {
                    var conVal = actionParamArray[0];
                    if (conVal.ToLower() == "true")
                    {
                        Xrun(actionParamArray[1]);
                    }
                    else
                    {
                        if (actionParamArray.Length == 3)
                        {
                            Xrun(actionParamArray[2]);
                        }
                    }
                }
                else if (actionName.ToLower() == "MaximizeForm".ToLower())
                {
                    MaximizeForm();
                }
                else if (actionName.ToLower() == "MinimizeForm".ToLower())
                {
                    MinimizeForm();
                }
                else if (actionName.ToLower() == "ShowForm".ToLower())
                {
                    ShowForm();
                }
                else if (actionName.ToLower() == "CloseForm".ToLower())
                {
                    CloseForm();
                }
                else if (actionName.ToLower() == "ReturnFalse".ToLower())
                {
                    IsOk = false;
                }
                else if (actionName.ToLower() == "ReturnFalseAndClose".ToLower())
                {
                    IsOk = false;
                    CloseForm();
                }
                else if (actionName.ToLower() == "ExitApp".ToLower())
                {
                    ExitApplication();
                }
                else if (actionName.ToLower() == "FadeIn".ToLower())
                {
                    var duration = 1000;
                    if (actionParamArray.Length > 0) duration = Convert.ToInt16(actionParamArray[0]);
                    FadeIn(duration);
                }
                else if (actionName.ToLower() == "FadeOut".ToLower())
                {
                    var duration = 1000;
                    if (actionParamArray.Length > 0) duration = Convert.ToInt16(actionParamArray[0]);
                    FadeOut(duration);
                }
                else if (actionName.ToLower() == "Sleep".ToLower())
                {
                    var duration = 1000;
                    if (actionParamArray.Length > 0) duration = Convert.ToInt16(actionParamArray[0]);
                    this.Refresh();
                    Thread.Sleep(duration);
                    this.Refresh();

                }
                else if (actionName.ToLower() == "RefreshUi".ToLower())
                {
                    RefreshUi();
                }
                else if (actionName.ToLower() == "RefreshForm".ToLower())
                {
                    Act(controlName, "NewForm", transactionDisplayName, isTopLevelAction);
                    ExitApplication();
                    //RefreshForm();
                }

                else if (actionName.ToLower() == "NewForm".ToLower())
                {

                    bool copyCurrentForm = false; ;
                    bool closeCurrentForm = false;
                    if (actionParamArray.Length == 0)
                    {
                        copyCurrentForm = true;
                    }
                    else
                    {
                        if (actionParamArray[0].IsNullOrEmpty())
                        {
                            copyCurrentForm = true;
                        }
                        if (actionParamArray.Length > 1)
                        {
                            closeCurrentForm = actionParamArray[1].ToLower() == "true";
                        }
                    }
                    var passedArg0 = copyCurrentForm ? "" : actionParamArray[0];
                    var passedArg0Arry = passedArg0.Split('@');

                    if (!copyCurrentForm)
                    {
                        if (passedArg0Arry.Length < 5)
                        {
                            PopupMessage.PopupError("length of passed Arg0 cannot be less than 5! ");
                            return;
                        }
                    }

                    var invisibleStr = copyCurrentForm ? "" : passedArg0Arry[0];
                    var formTypeStr = copyCurrentForm ? "" : passedArg0Arry[1];
                    var startAppStr = copyCurrentForm ? "" : passedArg0Arry[2];
                    var startFuncOrZoneLocStr = copyCurrentForm ? "" : passedArg0Arry[3];
                    var startViewMenuIdOrInputZoneVarsStr = copyCurrentForm ? "" : passedArg0Arry[4];
                    var startParams = (passedArg0Arry.Length > 5) ? passedArg0Arry[5] : "";
                    var startActionStr = passedArg0Arry.Length > 6 ? passedArg0Arry[6] : "";
                    var startPassword = passedArg0Arry.Length > 7 ? passedArg0Arry[7] : "";
                    var formTitle = passedArg0Arry.Length > 8 ? passedArg0Arry[8] : "";

                    if (startAppStr.IsNullOrEmpty()) startAppStr = _functionInitParamSet.ApplicationCode;
                    if (startFuncOrZoneLocStr.IsNullOrEmpty()) startFuncOrZoneLocStr = _functionInitParamSet.FunctionCode;
                    if (startViewMenuIdOrInputZoneVarsStr.IsNullOrEmpty())
                    {
                        if (_functionInitParamSet.FormType == FunctionFormType.MultipleView)
                            startViewMenuIdOrInputZoneVarsStr = CurrentViewMenuId.ToString();
                        else startViewMenuIdOrInputZoneVarsStr = _functionInitParamSet.InputZoneVariablesForNonMutiViewForm;
                    }

                    if (startPassword.IsNullOrEmpty())
                    {
                        if (startAppStr == _functionInitParamSet.ApplicationCode&startFuncOrZoneLocStr==_functionInitParamSet.FunctionCode)
                            startPassword = _functionInitParamSet.StartPassword;
                    }

                    var usrIdStr = GetCurrentUserId().ToString();
                    var usrCode = GetCurrentUserCode();
                    var usrToken = GetCurrentUserToken();

                    var arg0 = invisibleStr + "@" + formTypeStr + "@" + startAppStr + "@" + startFuncOrZoneLocStr + "@" + startViewMenuIdOrInputZoneVarsStr + "@" + startParams
                               + "@" + startActionStr + "@" + startPassword + "@" + formTitle + "@" + usrIdStr + "@" + usrCode + "@" + usrToken + "@@true";

                    //so+1
                    //arg0= EncryptionHelper.SmEncrypt(arg0,GlobalConfiguration.GlobalKey1,GlobalConfiguration.GlobalKey2);
                    var passedCultureName = "";
                    if (_functionInitParamSet.SupportMultiCultures)
                        passedCultureName = CultureHelper.CurrentCultureName;
                    if (actionParamArray.Length > 1) passedCultureName = actionParamArray[1];

                    var arguments = arg0 + " " + passedCultureName;
                    var path = Application.ExecutablePath;
                    var process = new Process();
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.FileName = path;
                    if (!string.IsNullOrEmpty(arguments))
                    {
                        process.StartInfo.Arguments = arguments;
                    }

                    if (closeCurrentForm) ExitApplication();
                    else
                    {
                        if (Control.ModifierKeys == Keys.Control) ExitApplication();
                    }
                    process.Start();
                }
                else if (actionName.ToLower() == "PopupZoneDialog".ToLower())
                {
                    var formTypeStr = "0";
                    var passedArg0 = actionParamArray.Length == 0 ? "" : actionParamArray[0];
                    var passedArg0Arry = passedArg0.Split('@');


                    string startZoneLocation = "", startInputZoneVarsStr = "", startParams = "", startActionsStr = "", startPassword = "", formTitle = "";

                    startZoneLocation = passedArg0Arry[0];
                    if (passedArg0Arry.Length > 1) startInputZoneVarsStr = passedArg0Arry[1];
                    if (passedArg0Arry.Length > 2) startParams = passedArg0Arry[2];
                    if (passedArg0Arry.Length > 3) startActionsStr = passedArg0Arry[3];
                    if (passedArg0Arry.Length > 4) startPassword = passedArg0Arry[4];
                    if (passedArg0Arry.Length > 5) formTitle = passedArg0Arry[5];


                    var functionInitParamSet = new FunctionInitParamSet();
                    functionInitParamSet.FormType = FunctionFormType.SingleView;
                    functionInitParamSet.ArchitectureCode = _functionInitParamSet.ArchitectureCode;
                    functionInitParamSet.ApplicationCode = _functionInitParamSet.ApplicationCode;
                    var temArry = startZoneLocation.SplitByLastSeparator('\\');
                    functionInitParamSet.FunctionCode = temArry.Length == 0 ? temArry[0] : temArry[1];
                    functionInitParamSet.ZoneLocationForNonMultiViewForm = FileHelper.GetFilePath(startZoneLocation, _zonesDir);
                    functionInitParamSet.InputZoneVariablesForNonMutiViewForm = startInputZoneVarsStr;

                    functionInitParamSet.StartParams = startParams;
                    functionInitParamSet.StartActions = startActionsStr;
                    functionInitParamSet.StartPassword = startPassword;
                    functionInitParamSet.FormTitle = formTitle;
                    functionInitParamSet.HelpdeskEmail = _functionInitParamSet.HelpdeskEmail;
                    functionInitParamSet.ApplicationVersion = _functionInitParamSet.ApplicationVersion;

                    functionInitParamSet.ImplementationDir = _functionInitParamSet.ImplementationDir;
                    functionInitParamSet.SupportMultiCultures = _functionInitParamSet.SupportMultiCultures;

                    PopupZoneDialogEx(functionInitParamSet);
                }
                else if (actionName.ToLower() == "PopupMsg".ToLower())
                {
                    var title = actionParamArray[0];
                    var content = actionParamArray[1];
                    var format = PopupMessageFormFormat.Common;
                    if (actionParamArray.Length > 2)
                    {
                        var p3 = actionParamArray[2];
                        if (p3.ToLower() == "MessageViewer".ToLower())
                        {
                            format = PopupMessageFormFormat.MessageViewer;
                        }
                        else if (p3.ToLower() == "RichTextViewer".ToLower())
                        {
                            format = PopupMessageFormFormat.RichTextViewer;
                        }
                    }
                    var width = 0;
                    if (actionParamArray.Length > 3)
                    {
                        width = Convert.ToInt16(actionParamArray[3]);
                    }
                    content = FunctionHelper.FormatRichText(content);
                    PopupMessage.Popup(title, content, format, width);
                }

                else if (actionName.ToLower() == "PopupZone".ToLower())
                {
                    var popupZoneName = DeleteControlZoneIdentifier(actionParamArray[0]);
                    var popupItem = _layoutElements.Find(x => x.Name == popupZoneName);
                    if (popupItem != null)
                    {
                        var popupCtnCtrl = GetControl(popupZoneName + "_Container");
                        var posArray = actionParamArray[1].Split(',');
                        var baseCtrlName = DeleteControlZoneIdentifier(posArray[0]);
                        var baseCtrl = GetControl(baseCtrlName);

                        var alignType = Convert.ToInt16(posArray[1]);
                        var offSetX = Convert.ToInt16(posArray[2]);
                        var offSetY = Convert.ToInt16(posArray[3]);
                        var p = baseCtrl.PointToClient(new Point(0, 0));
                        var p1 = GroundPanel.PointToClient(new Point(0, 0));
                        Point pos;
                        if (alignType == (int)ControlAlignPointType.LeftTop)
                        {
                            pos = new Point(p1.X - p.X, p1.Y - p.Y);
                        }
                        else if (alignType == (int)ControlAlignPointType.RightTop)
                        {
                            pos = new Point(p1.X - p.X - popupCtnCtrl.Width, p1.Y - p.Y);
                        }
                        else if (alignType == (int)ControlAlignPointType.LeftBottom)
                        {
                            pos = new Point(p1.X - p.X, p1.Y - p.Y - popupCtnCtrl.Height);
                        }
                        else if (alignType == (int)ControlAlignPointType.RightBottom)
                        {
                            pos = new Point(p1.X - p.X - popupCtnCtrl.Width, p1.Y - p.Y - popupCtnCtrl.Height);
                        }
                        else
                        {
                            pos = new Point(p1.X - p.X, p1.Y - p.Y);
                        }
                        pos.X = pos.X + offSetX;
                        pos.Y = pos.Y + offSetY;

                        popupCtnCtrl.Location = pos;
                        popupCtnCtrl.Visible = true;
                        popupCtnCtrl.BringToFront();
                    }
                }

                //procedure
                else if (actionName.ToLower() == "RefreshZonesProcedures".ToLower())
                {
                    var zoneNameArry = actionParamArray[0].SplitThenTrim(actionSubParamSeparator);

                    foreach (var zoneName in zoneNameArry)
                    {
                        var zoneName1 = DeleteControlZoneIdentifier(zoneName.Trim());
                        RefreshZoneProcedures(zoneName1);
                    }
                }
                else if (actionName.ToLower() == "RefreshZonesProceduresByGroup".ToLower())
                {
                    var zoneNameArry = actionParamArray[0].SplitThenTrim(actionSubParamSeparator);

                    foreach (var zoneName in zoneNameArry)
                    {
                        var zoneName1 = DeleteControlZoneIdentifier(zoneName.Trim());
                        RefreshZoneProceduresByGroup(zoneName1, Convert.ToInt32(actionParamArray[1].Trim()));
                    }
                }

                else if (actionName.ToLower() == "ClearZonesProcedureVariables".ToLower())
                {
                    var zoneNameArry = actionParamArray[0].SplitThenTrim(actionSubParamSeparator);

                    foreach (var zoneName in zoneNameArry)
                    {
                        var zoneName1 = DeleteControlZoneIdentifier(zoneName.Trim());
                        ClearZoneProceduresVariablesByGroup(zoneName1, -1);
                    }
                }
                else if (actionName.ToLower() == "ClearZonesProcedureVariablesByGroup".ToLower())
                {
                    var zoneNameArry = actionParamArray[0].SplitThenTrim(actionSubParamSeparator);

                    foreach (var zoneName in zoneNameArry)
                    {
                        var zoneName1 = DeleteControlZoneIdentifier(zoneName.Trim());
                        ClearZoneProceduresVariablesByGroup(zoneName1, Convert.ToInt32(actionParamArray[1].Trim()));
                    }
                }
                else if (actionName.ToLower() == "RefreshProcedureVariables".ToLower())
                {

                    var varNameArry = actionParamArray[0].SplitThenTrim(actionSubParamSeparator);
                    foreach (var varName in varNameArry)
                    {
                        var varName1 = DeleteProcedureZoneIdentifier(varName.Trim());
                        RefreshProcedureVariable(varName1);
                    }
                }

                else if (actionName.ToLower() == "ClearProcedureVariables".ToLower())
                {

                    var varNameArry = actionParamArray[0].SplitThenTrim(actionSubParamSeparator);

                    foreach (var varName in varNameArry)
                    {
                        var varName1 = DeleteProcedureZoneIdentifier(varName.Trim());
                        SetProcedureVariableValue(varName1.Trim(), string.Empty);
                    }
                }

                else if (actionName.ToLower() == "SetProcedureVariableValue".ToLower())
                {
                    var varName = DeleteProcedureZoneIdentifier(actionParamArray[0]);
                    var varValue = actionParamArray[1].Trim();
                    SetProcedureVariableValue(varName.Trim(), varValue);
                }

                //Control
                else if (actionName.ToLower() == "RefreshZonesControlsValues".ToLower())
                {
                    var zoneNameArry = actionParams.SplitThenTrim(actionSubParamSeparator);
                    foreach (var zoneName in zoneNameArry)
                    {
                        var zoneName1 = DeleteControlZoneIdentifier(zoneName.Trim());
                        RefreshZoneControlsValues(zoneName1);
                    }
                }
                else if (actionName.ToLower() == "SetControlText".ToLower())
                {
                    var ctrlName = DeleteControlZoneIdentifier(actionParamArray[0]);
                    var ctrlValue = actionParamArray[1].Trim();
                    SetControlText(ctrlName.Trim(), ctrlValue);
                }
                else if (actionName.ToLower() == "SetControlValue".ToLower())
                {
                    var ctrlName = DeleteControlZoneIdentifier(actionParamArray[0]);
                    //var ctrlValue = GetText(actionParamArray[1].Trim());
                    var ctrlValue = actionParamArray[1];
                    SetControlValue(ctrlName.Trim(), ctrlValue);
                }
                else if (actionName.ToLower() == "RefreshControls".ToLower())
                {
                    actionParams = DeleteControlZoneIdentifier(actionParams);
                    var ctrlNameArry = actionParams.SplitThenTrim(actionSubParamSeparator);
                    foreach (var ctrlName in ctrlNameArry)
                    {
                        var ctrlName1 = DeleteControlZoneIdentifier(ctrlName.Trim());
                        RefreshControl(ctrlName1);

                    }
                }
                else if (actionName.ToLower() == "RefreshControlsValues".ToLower())
                {
                    actionParams = DeleteControlZoneIdentifier(actionParams);
                    var ctrlNameArry = actionParams.SplitThenTrim(actionSubParamSeparator);
                    foreach (var ctrlName in ctrlNameArry)
                    {
                        var ctrlName1 = DeleteControlZoneIdentifier(ctrlName.Trim());
                        RefreshControlValue(ctrlName1);
                    }
                }
                else if (actionName.ToLower() == "RefreshControlsTexts".ToLower())
                {
                    actionParams = DeleteControlZoneIdentifier(actionParams);
                    var ctrlNameArry = actionParams.SplitThenTrim(actionSubParamSeparator);
                    foreach (var ctrlName in ctrlNameArry)
                    {
                        var ctrlName1 = DeleteControlZoneIdentifier(ctrlName.Trim());
                        RefreshControlText(ctrlName1);
                    }
                }
                else if (actionName.ToLower() == "RefreshControlsDataSources".ToLower())
                {
                    actionParams = DeleteControlZoneIdentifier(actionParams);
                    var ctrlNameArry = actionParams.SplitThenTrim(actionSubParamSeparator);
                    foreach (var ctrlName in ctrlNameArry)
                    {
                        var ctrlName1 = DeleteControlZoneIdentifier(ctrlName.Trim());
                        RefreshControlDataSource(ctrlName1);
                    }
                }
                else if (actionName.ToLower() == "RefreshZonesControlsInvisibles".ToLower())
                {
                    var zoneNameArry = actionParams.SplitThenTrim(actionSubParamSeparator);
                    foreach (var zoneName in zoneNameArry)
                    {
                        var zoneName1 = DeleteControlZoneIdentifier(zoneName.Trim());
                        RefreshZoneControlsInvisibles(zoneName1);
                    }
                }
                else if (actionName.ToLower() == "RefreshControlsInvisibles".ToLower())
                {
                    var ctrlNameArry = actionParams.SplitThenTrim(actionSubParamSeparator);
                    foreach (var ctrlName in ctrlNameArry)
                    {
                        var ctrlName1 = DeleteControlZoneIdentifier(ctrlName.Trim());
                        RefreshControlInvisible(ctrlName1);
                    }
                }
                else if (actionName.ToLower() == "SetControlVisible".ToLower())
                {
                    var ctrlName = DeleteControlZoneIdentifier(actionParamArray[0]);
                    SetControlVisible(ctrlName.Trim(), Convert.ToBoolean(actionParamArray[1].Trim()));
                }
                else if (actionName.ToLower() == "RefreshZonesControlsDisableds".ToLower())
                {
                    var zoneNameArry = actionParams.SplitThenTrim(actionSubParamSeparator);
                    foreach (var zoneName in zoneNameArry)
                    {
                        var zoneName1 = DeleteControlZoneIdentifier(zoneName.Trim());
                        RefreshZoneControlsDisableds(zoneName1);
                    }
                }
                else if (actionName.ToLower() == "RefreshControlsDisableds".ToLower())
                {
                    actionParams = DeleteControlZoneIdentifier(actionParams);
                    var ctrlNameArry = actionParams.SplitThenTrim(actionSubParamSeparator);
                    foreach (var ctrlName in ctrlNameArry)
                    {
                        var ctrlName1 = DeleteControlZoneIdentifier(ctrlName.Trim());
                        RefreshControlDisabled(DeleteControlZoneIdentifier(ctrlName1));
                    }
                }
                else if (actionName.ToLower() == "SetControlEnabled".ToLower())
                {
                    var ctrlName = DeleteControlZoneIdentifier(actionParamArray[0]);
                    SetControlEnabled(ctrlName.Trim(), Convert.ToBoolean(actionParamArray[1].Trim()));
                }
                else if (actionName.ToLower() == "SetControlPadding".ToLower())
                {

                    var ctrlName = actionParamArray[0];
                    var ctrl = GetControl(ctrlName);
                    //var ctrlPaddingFlag = GetText(actionParamArray[1].Trim());
                    ControlHelper.SetControlPadding(ctrl, "Padding:" + actionParamArray[1].Trim());
                }

                else//to do by LayoutParser
                {
                    if (actionName.ToLower() == "run" | actionName.ToLower() == ("runasadmin")
                        | actionName.ToLower() == "runcmd" | actionName.ToLower() == "runcmdasadmin"
                        | actionName.ToLower() == "openfile" | actionName.ToLower().StartsWith("openfileasadmin")
                        | actionName.ToLower() == "opentempfile" | actionName.ToLower() == "opentempfileasadmin"
                        | actionName.ToLower() == "editfile" | actionName.ToLower() == "editfileasadmin"
                        )
                    {

                        var defLoc = "";
                        var qty = controlName.GetQtyOfIncludedChar('_');
                        if (qty < 2) //menuitem=0 or viewitem=1
                        {
                            var ctrl = _layoutElements.Find(x => x.Name == controlName);
                            defLoc = ctrl.Location;
                        }
                        else //zoneitem=2
                        {
                            var zoneName = controlName.SplitByLastSeparator('_')[0];
                            var zone = _layoutElements.Find(x => x.Name == zoneName);
                            defLoc = zone.Location;
                        }
                        var path = actionParamArray[0];
                        path = FileHelper.GetFilePath(path, defLoc);
                        actionParamArray[0] = path;
                    }
                    returnStr = FunctionHelper.Act(actionName, actionParamArray);
                    if (returnStr == "OutOfScope")
                    {
                        returnStr = ActEx(actionName, actionParamArray);
                    }
                }

                if (actionName.ToLower() == "ExecCmd".ToLower() | actionName.ToLower() == "ExecCmdAsAdmin".ToLower() | actionName.ToLower() == "RunCmd".ToLower() | actionName.ToLower() == "RunCmdAsAdmin".ToLower())
                {
                    var lastParam = actionParamArray[actionParamArray.Length - 1];
                    if (lastParam.ToLower() == "p")
                    {
                        var title = actionName + " Executing Result";
                        var content = FunctionHelper.FormatRichText(returnStr);
                        PopupMessage.Popup(title, content, PopupMessageFormFormat.RichTextViewer, 0);
                    }

                    if (lastParam.ToLower() == "s")
                    {
                        var title = actionName;
                        foreach (var actParam in actionParamArray)
                        {
                            title = title + "-" + actParam.ToLegalFileName();
                        }

                        title = title + "-Executing-Result";
                        var content = FunctionHelper.FormatRichText(returnStr);
                        var path = _appDataDir + "\\" + title.ToUniqueStringByNow() + ".txt";
                        File.WriteAllText(path, content);
                    }

                }

                if (clearMsg)
                {
                    InitRunningStatusMessageComponent();
                }
            }
            catch (Exception ex)
            {
                InitRunningStatusMessageComponent();
                throw new ArgumentException("\n>> " + GetType().FullName + ".Act Error: action='" + action + "'; " + "controlName='" + controlName + "'; " + ex.Message);
            }
        }


        //##Implement
        protected virtual void Implement(string ctrlName, string funcName, string controlsNames, string values)
        {
        }


        //##xrun
        protected void Xrun(string subCtrlNamesStr)
        {
            var subCtrlNameArry = subCtrlNamesStr.Split(subCtrlNamesStr.GetSubParamSeparator());
            foreach (var subCtrlName in subCtrlNameArry)
            {
                var xrunSubCtrlName = DeleteControlZoneIdentifier(subCtrlName.Trim());
                if (xrunSubCtrlName.IsNullOrEmpty()) continue;
                var xrunSubAction = "";
                if (xrunSubCtrlName.GetQtyOfIncludedChar('_') < 2) //menuitem==0 or viewitem==1
                {
                    var xrunSubtransactionItem = _layoutElements.Find(x => x.Name == xrunSubCtrlName);
                    if (xrunSubtransactionItem == null) throw new ArgumentException("Xrun\\transactionItem='" + xrunSubCtrlName + "' doesn't exist!");
                    xrunSubAction = xrunSubtransactionItem.Action;
                }
                else if (xrunSubCtrlName.GetQtyOfIncludedChar('_') == 2)//zone item
                {
                    var xrunSubtransactionItem = _zonesItems.Find(x => x.Name == xrunSubCtrlName);
                    if (xrunSubtransactionItem == null) throw new ArgumentException("Xrun\\transactionItem='" + xrunSubCtrlName + "' doesn't exist!");
                    xrunSubAction = xrunSubtransactionItem.Action;
                }
                else
                {
                    throw new ArgumentException("Xrun\\transactionItem='" + xrunSubCtrlName + "' doesn't exist!");
                }
                Act(xrunSubCtrlName, xrunSubAction, "", false);
            }
        }



        //#subcommon
        //##GetControl
        protected Control GetControl(string ctrlName)
        {
            try
            {
                var ctrl = this.Controls.Find(ctrlName, true)[0];
                return ctrl;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + " GetControl Error: " + "Control doesn't exist! ctrlName=" + ctrlName);
            }
        }

        //##ShowForm
        protected void ShowForm()
        {
            this.Visible = true;
            this.WindowState = _ordinaryWindowStatus;
            this.Show();
        }

        //##MinimizeForm
        protected void MinimizeForm()
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //##MaximizeForm
        protected void MaximizeForm()
        {
            this.WindowState = FormWindowState.Maximized;
        }

        //##CloseForm
        protected void CloseForm()
        {
            Close();
        }

        //##ExitApplication
        protected void ExitApplication()
        {
            //_tray.Visible = false;
            IsRealClosing = true;
            Close();
            Application.Exit();
        }

        //##FadeIn
        private void FadeIn(int duration)
        {
            int interval = duration / 100;
            this.Opacity = 0;
            do
            {
                this.Opacity += 0.01;
                this.Refresh();
                Thread.Sleep(interval);
            } while (this.Opacity < 1);

        }

        //##FadeOut
        private void FadeOut(int duration)
        {
            int interval = duration / 100;
            this.Opacity = 1;
            do
            {
                this.Opacity -= 0.01;
                this.Refresh();
                Thread.Sleep(interval);
            } while (this.Opacity > 0);

            Close();
        }

        //##RefreshForm
        //**for testing UI, refresh after logon
        protected void RefreshUi()
        {
            try
            {
                foreach (var elemt in _layoutElements)
                {
                    var isPopup = elemt.IsPopup;
                    if (isPopup)
                    {
                        var ctrl = new Control();

                        try
                        {
                            ctrl = GroundPanel.Controls.Find(elemt.Name + "_" + "Container", true)[0];
                            GroundPanel.Controls.Remove(ctrl);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                _functionFormStyle = null;
                _procedures.Clear();
                _layoutElements.Clear();
                _annexes.Clear();
                _renderedViewStatuses.Clear();
                _zonesItems.Clear();

                MainMenuSectionLeftRegion.Controls.Clear();
                MainMenuSectionCenterRegion.Controls.Clear();
                MainMenuSectionRightRegion.Controls.Clear();

                ToolBarSectionLeftRegion.Controls.Clear();
                ToolBarSectionCenterRegion.Controls.Clear();
                ToolBarSectionRightRegion.Controls.Clear();
                ToolBarSectionPublicRegionToolStrip.Items.Clear();
                //ToolBarSectionPublicRegionToolStrip.Controls.Clear(); //error:集合为只读。

                NavigationSectionLeftRegion.Controls.Clear();
                NavigationSectionCenterRegion.Controls.Clear();
                NavigationSectionRightRegion.Controls.Clear();

                ShortcutSectionLeftRegion.Controls.Clear();
                ShortcutSectionCenterRegion.Controls.Clear();
                ShortcutSectionRightRegion.Controls.Clear();

                MainSectionLeftNavDivisionUpRegion.Controls.Clear();
                MainSectionLeftNavDivisionMidRegion.Controls.Clear();
                MainSectionLeftNavDivisionDownRegion.Controls.Clear();

                MainSectionRightNavDivisionUpRegion.Controls.Clear();
                MainSectionRightNavDivisionMidRegion.Controls.Clear();
                MainSectionRightNavDivisionDownRegion.Controls.Clear();

                MainSectionMainDivisionUpRegion.Controls.Clear();
                MainSectionMainDivisionMidRegion.Controls.Clear();
                MainSectionMainDivisionDownRegion.Controls.Clear();

                MainSectionRightDivisionUpRegion.Controls.Clear();
                MainSectionRightDivisionMidRegion.Controls.Clear();
                MainSectionRightDivisionDownRegion.Controls.Clear();

                LoadForm();
                this.Refresh();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".RefreshForm Error: " + ex.Message);
            }
        }

        //##GetAdditionalInfoForException
        private string GetAdditionalInfoForException()
        {
            return _additionalInfoForException + GetCurrentUserCode();
        }

        //#virtual
        //##ResolveConstantsEx
        protected virtual string ResolveConstantsEx(string text)
        {
            return text;
        }

        //##GetTextEx
        protected virtual string GetTextEx(string funName, string[] paramArray)
        {
            return string.Empty;
        }

        //##ActEx
        protected virtual string ActEx(string funcName, string[] funcParamArray)
        {
            return string.Empty;
        }

        //##PopupZoneDialogEx
        protected virtual void PopupZoneDialogEx(FunctionInitParamSet functionInitParamSet)
        {
        }

        protected virtual Int64 GetCurrentUserId()
        {
            return 0;
        }

        protected virtual string GetCurrentUserCode()
        {
            return string.Empty;
        }

        protected virtual string GetCurrentUserToken()
        {
            return string.Empty;
        }

        protected virtual void OnCurrentLanguageChanged()
        {
        }


        //#end
    }
}
