using System;
using System.Drawing;
using System.Windows.Forms;
using Ligg.WinForm.DataModel.Enums;
using Ligg.WinForm.Resources;

namespace Ligg.WinForm.Forms
{
    public partial class FrameForm : BaseForm
    {
        private bool _isRightNavDivisionCurrentlyShown;
        private readonly ToolTip _horizontalResizeButtonToolTip = new ToolTip();
        private int _mainSectionRightNavDivisionWidth;

        protected int MainMenuSectionHeight = 30;
        protected int MainMenuSectionLeftRegionWidth = 10;
        protected int MainMenuSectionRightRegionWidth = 10;


        protected int ToolBarSectionHeight = 64;
        protected int ToolBarSectionPublicRegionWidth = 105;
        protected int ToolBarSectionLeftRegionWidth = 10;
        protected int ToolBarSectionCenterRegionWidth = 10;


        protected int NavigationSectionHeight = 28;
        protected int NavigationSectionLeftRegionWidth =10;
        protected int NavigationSectionRightRegionWidth = 10;

        protected int ShortcutSectionHeight = 28;
        protected int ShortcutSectionLeftRegionWidth = 10;
        protected int ShortcutSectionRightRegionWidth = 10;

        protected int MainSectionLeftNavDivisionWidth = 80;
        protected int MainSectionLeftNavDivisionUpRegionHeight = 24;
        protected int MainSectionLeftNavDivisionDownRegionHeight = 0;

        protected int MainSectionRightNavDivisionWidth = 100;
        protected int MainSectionRightNavDivisionUpRegionHeight = 24;
        protected int MainSectionRightNavDivisionDownRegionHeight = 0;

        protected int MainSectionHorizontalResizeDivisionWidth = 6;
        protected int HorizontalResizeButtonPosX = 0;
        protected int HorizontalResizeButtonPosY = 0;

        protected int MainSectionMainDivisionUpRegionHeight = 0;
        protected int MainSectionMainDivisionDownRegionHeight = 0;

        protected int MainSectionRightDivisionWidth = 80;
        protected int MainSectionRightDivisionUpRegionHeight = 0;
        protected int MainSectionRightDivisionDownRegionHeight = 0;

        protected ResizableDivisionStatus HorizontalResizableDivisionStatus = ResizableDivisionStatus.None;

        protected FrameForm()
        {
            InitializeComponent();
            InitFrameComponent();
        }

        private void HorizontalResizeButton_Click(object sender, EventArgs e)
        {
            if (_isRightNavDivisionCurrentlyShown)
            {
                MainSectionRightNavDivision.Width = 0;
                HorizontalResizeButton.BackgroundImage = PictureList.Images["go_right.png"];
                _horizontalResizeButtonToolTip.SetToolTip(HorizontalResizeButton, WinformRes.ShowLeftDivision);
                MainSectionSplitter.Visible = false;
                _isRightNavDivisionCurrentlyShown = false;
            }
            else
            {
                MainSectionRightNavDivision.Width = MainSectionRightNavDivisionWidth;
                HorizontalResizeButton.BackgroundImage = PictureList.Images["go_left.png"];
                _horizontalResizeButtonToolTip.SetToolTip(HorizontalResizeButton, WinformRes.HideLeftDivision);
                MainSectionSplitter.Visible = true;
                _isRightNavDivisionCurrentlyShown = true;
            }
        }

        //#proc
        private void InitFrameComponent()
        {
            GroundPanel.BackColor = StyleSheet.GroundColor;

            MainMenuSection.BackColor = StyleSheet.GroundColor;
            MainMenuSection.StyleType = Ligg.WinForm.Controls.ContainerPanel.ContainerPanelStyle.None;
            MainMenuSection.RoundStyle = RoundStyle.None;
            MainMenuSection.Radius = 0;
            MainMenuSection.BorderWidthOnLeft = 0;
            MainMenuSection.BorderWidthOnTop = 0;
            MainMenuSection.BorderWidthOnRight = 0;
            MainMenuSection.BorderWidthOnBottom = 1;
            MainMenuSection.BorderColor = StyleSheet.ControlBorderColor;
            MainMenuSection.Padding = new Padding(2);

            ToolBarSection.BackColor = StyleSheet.BaseColor;
            ToolBarSection.StyleType = Ligg.WinForm.Controls.ContainerPanel.ContainerPanelStyle.None;
            ToolBarSection.RoundStyle = RoundStyle.None;
            ToolBarSection.Radius = 0;
            ToolBarSection.BorderWidthOnLeft = 0;
            ToolBarSection.BorderWidthOnTop = 0;
            ToolBarSection.BorderWidthOnRight = 0;
            ToolBarSection.BorderWidthOnBottom = 0;
            ToolBarSection.BorderColor = StyleSheet.ControlBorderColor;
            ToolBarSection.Padding = new Padding(2);
            ToolBarSectionPublicRegionToolStrip.BackColor = StyleSheet.BaseColor;
            ToolBarSectionLeftRegion.BackColor = StyleSheet.BaseColor;

            NavigationSection.BackColor = StyleSheet.NavigationSectionBackColor;
            NavigationSection.StyleType = Ligg.WinForm.Controls.ContainerPanel.ContainerPanelStyle.Borders;
            NavigationSection.RoundStyle = RoundStyle.None;
            NavigationSection.Radius = 0;
            NavigationSection.BorderWidthOnLeft = 0;
            NavigationSection.BorderWidthOnTop = 0;
            NavigationSection.BorderWidthOnRight = 0;
            NavigationSection.BorderWidthOnBottom = 1;
            NavigationSection.BorderColor = StyleSheet.ControlBorderColor;
            NavigationSection.Padding = new Padding(2);

            NavigationSectionLeftRegion.BackColor = StyleSheet.NavigationSectionBackColor;
            NavigationSectionRightRegion.BackColor = StyleSheet.NavigationSectionBackColor;

            ShortcutSection.BackColor = StyleSheet.ShortcutSectionBackColor;
            ShortcutSection.StyleType = Ligg.WinForm.Controls.ContainerPanel.ContainerPanelStyle.Borders;
            ShortcutSection.RoundStyle = RoundStyle.None;
            ShortcutSection.Radius = 0;
            ShortcutSection.BorderWidthOnLeft = 0;
            ShortcutSection.BorderWidthOnTop = 0;
            ShortcutSection.BorderWidthOnRight = 0;
            ShortcutSection.BorderWidthOnBottom = 1;
            ShortcutSection.BorderColor = StyleSheet.ControlBorderColor;
            ShortcutSection.Padding = new Padding(2);

            ShortcutSectionLeftRegion.BackColor = StyleSheet.ShortcutSectionBackColor;
            ShortcutSectionRightRegion.BackColor = StyleSheet.ShortcutSectionBackColor;

            MainSection.BackColor = StyleSheet.GroundColor;
            MainSection.StyleType = Ligg.WinForm.Controls.ContainerPanel.ContainerPanelStyle.Borders;
            MainSection.RoundStyle = RoundStyle.None;
            MainSection.Radius = 0;
            MainSection.BorderWidthOnLeft = 0;
            MainSection.BorderWidthOnTop = 0;
            MainSection.BorderWidthOnRight = 0;
            MainSection.BorderWidthOnBottom = 1;
            MainSection.BorderColor = StyleSheet.ControlBorderColor;
            MainSection.Padding = new Padding(2);

            MainSectionLeftNavDivision.BackColor = StyleSheet.MainSectionLeftNavDivisionBackColor;
            MainSectionLeftNavDivisionUpRegion.BackColor = StyleSheet.MainSectionLeftNavDivisionBackColor;
            MainSectionLeftNavDivisionMidRegion.BackColor = StyleSheet.MainSectionLeftNavDivisionBackColor;

            MainSectionRightNavDivision.BackColor = StyleSheet.MainSectionRightNavDivisionBackColor;
            MainSectionRightNavDivisionUpRegion.BackColor = StyleSheet.MainSectionRightNavDivisionBackColor;
            MainSectionRightNavDivisionMidRegion.BackColor = StyleSheet.MainSectionRightNavDivisionBackColor;
            MainSectionSplitter.BackColor = StyleSheet.ControlBorderColor;

            MainSectionHorizontalResizeDivision.BackColor = StyleSheet.MainSectionHorizontalResizeDivisionBackColor;
            HorizontalResizeButton.Size = new Size(6, 50);
        }

        public void InitFrameHorizontalResizableDivisionStatus()
        {
            if (HorizontalResizableDivisionStatus == ResizableDivisionStatus.None)
            {
                HorizontalResizeButton.Visible = false;
                MainSectionSplitter.Visible = false;
                HorizontalResizeButton.Visible = false;
                MainSectionHorizontalResizeDivision.Width = 0;
                _mainSectionRightNavDivisionWidth = MainSectionRightNavDivisionWidth;
            }
            else if (HorizontalResizableDivisionStatus == ResizableDivisionStatus.Hidden)
            {
                HorizontalResizeButton.BackgroundImage = PictureList.Images["go_right.png"];
                _horizontalResizeButtonToolTip.SetToolTip(HorizontalResizeButton, WinformRes.ShowLeftDivision);
                HorizontalResizeButton.Visible = true;
                MainSectionHorizontalResizeDivision.Width = MainSectionHorizontalResizeDivisionWidth;
                MainSectionSplitter.Visible = false;
                _isRightNavDivisionCurrentlyShown = false;
                _mainSectionRightNavDivisionWidth = 0;

            }
            else if (HorizontalResizableDivisionStatus == ResizableDivisionStatus.Shown)
            {
                HorizontalResizeButton.BackgroundImage = PictureList.Images["go_left.png"];
                _horizontalResizeButtonToolTip.SetToolTip(HorizontalResizeButton, WinformRes.HideLeftDivision);
                HorizontalResizeButton.Visible = true;
                MainSectionHorizontalResizeDivision.Width = MainSectionHorizontalResizeDivisionWidth;
                MainSectionSplitter.Visible = true;
                _isRightNavDivisionCurrentlyShown = true;
                _mainSectionRightNavDivisionWidth = MainSectionRightNavDivisionWidth;

                var posY = Convert.ToInt16(MainSectionHorizontalResizeDivision.Height / 3.2 - HorizontalResizeButton.Height / 3.2);
                HorizontalResizeButton.Location = new Point(0, posY);
            }
            else
            {
                HorizontalResizeButton.Visible = false;
                MainSectionSplitter.Visible = false;
                HorizontalResizeButton.Visible = false;
                MainSectionHorizontalResizeDivision.Width = 0;
                _mainSectionRightNavDivisionWidth = MainSectionRightNavDivisionWidth;
            }

        }

        protected void ResizeFrameComponent()
        {
            MainMenuSection.Height = MainMenuSectionHeight;
            MainMenuSectionLeftRegion.Width = MainMenuSectionLeftRegionWidth;
            MainMenuSectionRightRegion.Width = MainMenuSectionRightRegionWidth;
            MainMenuSectionCenterRegion.Width = MainMenuSection.Width - MainMenuSectionLeftRegionWidth - MainMenuSectionRightRegionWidth-4;

            ToolBarSection.Height = ToolBarSectionHeight;
            ToolBarSectionPublicRegion.Width = ToolBarSectionPublicRegionWidth;
            ToolBarSectionLeftRegion.Width = ToolBarSectionLeftRegionWidth;
            ToolBarSectionCenterRegion.Width = ToolBarSectionCenterRegionWidth;
            ToolBarSectionRightRegion.Width = ToolBarSection.Width - ToolBarSectionPublicRegionWidth- ToolBarSectionLeftRegionWidth - ToolBarSectionCenterRegionWidth-4;

            NavigationSection.Height = NavigationSectionHeight;
            NavigationSectionLeftRegion.Width = NavigationSectionLeftRegionWidth;
            NavigationSectionRightRegion.Width = NavigationSectionRightRegionWidth;
            NavigationSectionCenterRegion.Width = NavigationSection.Width - NavigationSectionLeftRegionWidth - NavigationSectionRightRegionWidth - 4;

            ShortcutSection.Height = ShortcutSectionHeight;
            ShortcutSectionLeftRegion.Width = ShortcutSectionLeftRegionWidth;
            ShortcutSectionRightRegion.Width = ShortcutSectionRightRegionWidth;
            ShortcutSectionCenterRegion.Width = ShortcutSection.Width - ShortcutSectionLeftRegionWidth - ShortcutSectionRightRegionWidth - 4;

            MainSection.Height = GroundPanel.Height - MainMenuSectionHeight - ToolBarSectionHeight - NavigationSectionHeight - ShortcutSectionHeight - RunningMessageSectionHeight - RunningStatusSectionHeight;

            MainSectionLeftNavDivision.Width = MainSectionLeftNavDivisionWidth;
            MainSectionLeftNavDivisionUpRegion.Height = MainSectionLeftNavDivisionUpRegionHeight;
            MainSectionLeftNavDivisionDownRegion.Height = MainSectionLeftNavDivisionDownRegionHeight;
            MainSectionLeftNavDivisionMidRegion.Height = MainSectionLeftNavDivision.Height - MainSectionLeftNavDivisionUpRegionHeight - MainSectionLeftNavDivisionDownRegionHeight; ;

            MainSectionRightNavDivision.Width = _mainSectionRightNavDivisionWidth;
            MainSectionRightNavDivisionUpRegion.Height = MainSectionRightNavDivisionUpRegionHeight;
            MainSectionRightNavDivisionDownRegion.Height = MainSectionRightNavDivisionDownRegionHeight;
            MainSectionRightNavDivisionMidRegion.Height = MainSectionRightNavDivision.Height - MainSectionRightNavDivisionUpRegionHeight - MainSectionRightNavDivisionDownRegionHeight; ;

            MainSectionMainDivisionUpRegion.Height = MainSectionMainDivisionUpRegionHeight;
            MainSectionMainDivisionDownRegion.Height = MainSectionMainDivisionDownRegionHeight;
            MainSectionMainDivisionMidRegion.Height = MainSectionMainDivision.Height - MainSectionMainDivisionUpRegionHeight - MainSectionMainDivisionDownRegionHeight;

            MainSectionRightDivision.Width = MainSectionRightDivisionWidth;
            MainSectionRightDivisionUpRegion.Height = MainSectionRightDivisionUpRegionHeight;
            MainSectionRightDivisionDownRegion.Height = MainSectionRightDivisionDownRegionHeight;
            MainSectionRightDivisionMidRegion.Height = MainSectionRightDivision.Height - MainSectionRightDivisionUpRegionHeight - MainSectionRightDivisionDownRegionHeight; ;

            ResizeBaseComponent();
        }

        protected void SetFrameTextByCulture(bool isOnLoad, bool supportMutiLangs)
        {
            if (isOnLoad)
            {
                if (_isRightNavDivisionCurrentlyShown)
                {
                    _horizontalResizeButtonToolTip.SetToolTip(HorizontalResizeButton, WinformRes.HideLeftDivision);
                }
                else
                {
                    _horizontalResizeButtonToolTip.SetToolTip(HorizontalResizeButton, WinformRes.ShowLeftDivision);
                }
                if (!supportMutiLangs) ToolBarSectionPublicRegionWidth = 0;
            }
            ResetBaseTextByCulture();
        }



    }
}
