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

        protected int TopNavSectionHeight = 30;
        protected int TopNavSectionLeftRegionWidth = 10;
        protected int TopNavSectionRightRegionWidth = 10;


        protected int ToolBarSectionHeight = 64;
        protected int ToolBarSectionPublicRegionWidth = 105;
        protected int ToolBarSectionLeftRegionWidth = 10;
        protected int ToolBarSectionCenterRegionWidth = 10;


        protected int MiddleNavSectionHeight = 28;
        protected int MiddleNavSectionLeftRegionWidth = 10;
        protected int MiddleNavSectionRightRegionWidth = 10;

        protected int DownNavSectionHeight = 28;
        protected int DownNavSectionLeftRegionWidth = 10;
        protected int DownNavSectionRightRegionWidth = 10;

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

            TopNavSection.BackColor = StyleSheet.GroundColor;
            TopNavSection.StyleType = Ligg.WinForm.Controls.ContainerPanel.ContainerPanelStyle.None;
            TopNavSection.RoundStyle = RoundStyle.None;
            TopNavSection.Radius = 0;
            TopNavSection.BorderWidthOnLeft = 0;
            TopNavSection.BorderWidthOnTop = 0;
            TopNavSection.BorderWidthOnRight = 0;
            TopNavSection.BorderWidthOnBottom = 1;
            TopNavSection.BorderColor = StyleSheet.ControlBorderColor;
            TopNavSection.Padding = new Padding(2);

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

            MiddleNavSection.BackColor = StyleSheet.NavigationSectionBackColor;
            MiddleNavSection.StyleType = Ligg.WinForm.Controls.ContainerPanel.ContainerPanelStyle.Borders;
            MiddleNavSection.RoundStyle = RoundStyle.None;
            MiddleNavSection.Radius = 0;
            MiddleNavSection.BorderWidthOnLeft = 0;
            MiddleNavSection.BorderWidthOnTop = 0;
            MiddleNavSection.BorderWidthOnRight = 0;
            MiddleNavSection.BorderWidthOnBottom = 1;
            MiddleNavSection.BorderColor = StyleSheet.ControlBorderColor;
            MiddleNavSection.Padding = new Padding(2);

            MiddleNavSectionLeftRegion.BackColor = StyleSheet.NavigationSectionBackColor;
            MiddleNavSectionRightRegion.BackColor = StyleSheet.NavigationSectionBackColor;

            DownNavSection.BackColor = StyleSheet.ShortcutSectionBackColor;
            DownNavSection.StyleType = Ligg.WinForm.Controls.ContainerPanel.ContainerPanelStyle.Borders;
            DownNavSection.RoundStyle = RoundStyle.None;
            DownNavSection.Radius = 0;
            DownNavSection.BorderWidthOnLeft = 0;
            DownNavSection.BorderWidthOnTop = 0;
            DownNavSection.BorderWidthOnRight = 0;
            DownNavSection.BorderWidthOnBottom = 1;
            DownNavSection.BorderColor = StyleSheet.ControlBorderColor;
            DownNavSection.Padding = new Padding(2);

            DownNavSectionLeftRegion.BackColor = StyleSheet.ShortcutSectionBackColor;
            DownNavSectionRightRegion.BackColor = StyleSheet.ShortcutSectionBackColor;

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
            TopNavSection.Height = TopNavSectionHeight;
            TopNavSectionLeftRegion.Width = TopNavSectionLeftRegionWidth;
            TopNavSectionRightRegion.Width = TopNavSectionRightRegionWidth;
            TopNavSectionCenterRegion.Width = TopNavSection.Width - TopNavSectionLeftRegionWidth - TopNavSectionRightRegionWidth - 4;

            ToolBarSection.Height = ToolBarSectionHeight;
            ToolBarSectionPublicRegion.Width = ToolBarSectionPublicRegionWidth;
            ToolBarSectionLeftRegion.Width = ToolBarSectionLeftRegionWidth;
            ToolBarSectionCenterRegion.Width = ToolBarSectionCenterRegionWidth;
            ToolBarSectionRightRegion.Width = ToolBarSection.Width - ToolBarSectionPublicRegionWidth- ToolBarSectionLeftRegionWidth - ToolBarSectionCenterRegionWidth-4;

            MiddleNavSection.Height = MiddleNavSectionHeight;
            MiddleNavSectionLeftRegion.Width = MiddleNavSectionLeftRegionWidth;
            MiddleNavSectionRightRegion.Width = MiddleNavSectionRightRegionWidth;
            MiddleNavSectionCenterRegion.Width = MiddleNavSection.Width - MiddleNavSectionLeftRegionWidth - MiddleNavSectionRightRegionWidth - 4;

            DownNavSection.Height = DownNavSectionHeight;
            DownNavSectionLeftRegion.Width = DownNavSectionLeftRegionWidth;
            DownNavSectionRightRegion.Width = DownNavSectionRightRegionWidth;
            DownNavSectionCenterRegion.Width = DownNavSection.Width - DownNavSectionLeftRegionWidth - DownNavSectionRightRegionWidth - 4;

            MainSection.Height = GroundPanel.Height - TopNavSectionHeight - ToolBarSectionHeight - MiddleNavSectionHeight - DownNavSectionHeight - RunningMessageSectionHeight - RunningStatusSectionHeight;

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

        protected void SetFrameTextByCulture(bool isOnLoad, bool supportMultiCulture)
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
                if (!supportMultiCulture) ToolBarSectionPublicRegionWidth = 0;
            }

            ResetBaseTextByCulture();
        }



    }
}
