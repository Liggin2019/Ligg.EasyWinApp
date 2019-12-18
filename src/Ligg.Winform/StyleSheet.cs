using System;
using System.Drawing;
using Ligg.WinForm.Skin;

namespace Ligg.WinForm
{
    public static class StyleSheet
    {
        public static void SetStyleSet(string cfgXmlPath)
        {

        }

        public static int DefaultOffsetX = 8;
        public static int DefaultOffsetY = 5;

        //#Skin set
        //##base
        public static Color BaseColor = Color.FromArgb(48, 145, 242);
        public static Color GroundColor = Color.FromArgb(255, 255, 255);

        public static Color ColorDefault = Color.Black;
        public static Color ColorCommand = Color.DarkBlue;
        public static Color ColorSucceeded = Color.ForestGreen;
        public static Color ColorError = Color.Red;

        //##form
        public static Color CaptionActiveColor = BaseColor;
        public static Color CaptionInactiveColor = Color.FromArgb(131, 209, 255);
        public static Color CaptionTextColor = Color.FromArgb(255, 255, 255);

        public static Color FormBorderColor = Color.FromArgb(55, 126, 168);
        public static Color FormInnerBorderColor = Color.Transparent;

        public static Color ControlBoxActiveColor = BaseColor;
        public static Color ControlBoxInActiveColor = Color.FromArgb(48, 145, 242);
        public static Color ControlBoxInnerBorderColor = Color.FromArgb(250, 250, 250);

        public static Color ControlBoxHoveringColor = Color.FromArgb(37, 114, 151);
        public static Color ControlBoxPressedColor = Color.FromArgb(27, 84, 111);

        public static Color CloseControlBoxHoveringColor = Color.FromArgb(213, 66, 22);
        public static Color CloseControlBoxPressedColor = Color.FromArgb(171, 53, 17);

        //#section
        public static Color NavigationSectionBackColor = Color.FromArgb(244, 245, 247);
        public static Color ShortcutSectionBackColor = Color.FromArgb(248, 248, 248);
        public static Color RunningStatusSectionBackColor = Color.FromArgb(246, 246, 246);


        //#division         
        public static Color MainSectionLeftNavDivisionBackColor = Color.FromArgb(246, 250, 243);
        public static Color MainSectionRightNavDivisionBackColor = Color.FromArgb(246, 250, 243);
        public static Color MainSectionHorizontalResizeDivisionBackColor = Color.FromArgb(251, 255, 245);

        //#head toolstrip
        public static Color HeadToolStripBackColor = BaseColor;
        public static Color HeadToolStripItemHoveringBackColor = Color.FromArgb(64, 134, 235);
        public static Color HeadToolStripItemPressedBackColor = GraphicsHelper.GetColor(HeadToolStripItemHoveringBackColor, 0, -2 * 13, -2 * 8, -2 * 3);
        public static Color HeadToolStripItemCheckedBorderColor = HeadToolStripItemPressedBackColor;
        public static Color HeadToolStripItemBorderColor = GraphicsHelper.GetColor(HeadToolStripItemPressedBackColor, 0, -2 * 13, -2 * 8, -2 * 3);
        public static Color HeadToolStripItemInnerBorderColor = FormInnerBorderColor;

        //ToolStrip DropDown including head toolstrip DropDown  
        public static Color ToolStripDropDownBackColor = Color.FromArgb(253, 253, 253);
        public static Color ToolStripDropDownBorderColor = Color.FromArgb(233, 238, 238);
        public static Color ToolStripDropDownPressedCenterColor = Color.FromArgb(233, 238, 238);
        public static Color ToolStripDropDownSeparatorColor = Color.FromArgb(197, 197, 197);

        public static Color ToolStripMenuItemHoveredBackColor = Color.FromArgb(152, 200, 249);
        public static Color ToolStripMenuItemPressedBackColor = Color.FromArgb(208, 227, 252);
        public static Color ToolStripMenuItemBorderColor = Color.FromArgb(197, 197, 197);
        public static Color ToolStripMenuItemInnerBorderColor = FormInnerBorderColor;

        //#Control
        public static string ControlBackColorRgbString = "222, 242, 252";
        public static string ViewMenuBackColorRgbString = "191,219,255";

        public static Color ControlBackColor = Color.FromArgb(222, 242, 252);
        public static Color ControlHoveringColor = GraphicsHelper.GetColor(ControlBackColor, 0, -2 * 13, -2 * 8, -2 * 3);
        public static Color ControlPressedColor = GraphicsHelper.GetColor(ControlHoveringBackColor, 0, -2 * 13, -2 * 8, -2 * 3);
        public static Color ControlFocusedColor = BaseColor;

        public static Color ControlHoveringBackColor = GraphicsHelper.GetColor(ControlBackColor, 0, -2 * 13, -2 * 8, -2 * 3);
        public static Color ControlPressedBackColor = GraphicsHelper.GetColor(ControlHoveringBackColor, 0, -2 * 13, -2 * 8, -2 * 3);
        public static Color ControlCheckedBorderColor = ControlPressedBackColor;
        public static Color ControlFocusedBackColor = BaseColor;

        public static Color ControlDisabledBaseColor = Color.FromArgb(204, 204, 204);

        public static Color ControlBorderColor = Color.FromArgb(197, 197, 197);
        public static Color ControlInnerBorderColor = FormInnerBorderColor;

        //##PopupContainer
        public static Color PopupContainerBackColor = Color.FromArgb(250, 250, 250);
        public static Color PopupContainerBorderColor = Color.FromArgb(180, 180, 180);

        //##pager
        public static Color PagerBackColor = Color.FromArgb(230, 230, 230);

        //#control Class
        //Font:Microsoft Sans Serif
        public static string PanelClass_Default= "BackColor:250,250,250;BorderColor:180, 180, 180";

        public static string ShadowPanelClass_Default= "BackColor:250,250,250;BorderColor:180, 180, 180";

        public static string ContainerPanelClass_Rect= "Style:Borders;BorderColor:218,218,218;BorderWidth:1";
        public static string ContainerPanelClass_Ellipse= "Style:Rounded;BorderColor:218,218,218;Radius:15";
        public static string ContainerPanelClass_RadioContainer= "Style:Rounded;BorderColor:235,236,235;Radius:5;padding:1";

        public static string SplitRectangleClass_Level1 = "BackColor:200,200,200";
        public static string SplitRectangleClass_Level2 = "BackColor:235,236,235";

        public static string LabelClass_Level1 = "FontStyle:Bold;FontSize:9.00";
        public static string LabelClass_Level2 = "FontStyle:Bold;FontSize:8.50";
        public static string LabelClass_FieldName = "FontStyle:Bold;FontSize:8.00;TextAlign:MiddleRight";

        public static string TitleLabelClass_Level1 = "FontStyle:Bold;FontSize:9.00;TextAlign:MiddleLeft;HasBottomLine:true;BottomLineColor:200,200,200";
        public static string TitleLabelClass_Level2 = "FontStyle:Bold;FontSize:8.50;TextAlign:MiddleLeft;HasBottomLine:true;BottomLineColor:235,236,235";
        public static string TitleLabelClass_Menu = "FontStyle:Bold;FontSize:8.50;TextAlign:MiddleLeft;HasBottomLine:true;BottomLineColor:180,180,180";

        public static string CommandLabelClass_CmdLabel = "ForeColor:0, 0, 0";
        public static string CommandLabelClass_LinkedLabel = "HoveringColor:140, 0, 0;FocusedColor:230, 0, 0";

        public static string CheckBoxClass_Level1 = "FontStyle:Bold;FontSize:8.00";

        public static string StatusLightClass_Level1 = "FontStyle:Bold;FontSize:8";
        public static string StatusLightClass_Level2 = "FontStyle:Bold;FontSize:8";

        public static string TextButtonClass_Menu = "SensitiveType:Check;BackColor:" + ViewMenuBackColorRgbString + ";Font:Arial;FontSize:8.7;TextAlign:MiddleCenter;HasBorder:false";
        public static string TextButtonClass_Button = "BackColor:" + ControlBackColorRgbString + ";Font:Arial;FontSize:8.7;TextAlign:MiddleCenter;HasBorder:true";

        public static string ImageTextButtonClass_VerMenu = "Font:Arial;FontSize:8.7;HasBorder:false;Offset:3,3";
        public static string ImageTextButtonClass_HorMenu = "Font:Arial;FontSize:8.7;HasBorder:false;TextAlign:MiddleLeft;Offset:3,3";

        public static string ImageTextButtonClass_VerButton = "BackColor:" + ControlBackColorRgbString + ";Font:Arial;FontSize:8.7;HasBorder:true";
        public static string ImageTextButtonClass_HorButton = "BackColor:" + ControlBackColorRgbString + ";FontStyle:bold;Font:Arial;FontSize:8.7;TextAlign:MiddleLeft;HasBorder:true";

        public static string RichTextBoxClass_Error = "ForeColor:255,0,0;BorderStyle:none";

        public static string TimerExTimingRunClass_Default = "BackColor:235,235,235";
        public static string TimerExLoopingRunClass_Default = "BackColor:235,235,235";

    }
}
