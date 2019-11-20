namespace Ligg.Winform.DataModel.Enums
{
    public enum FunctionFormViewMenuMode
    {
        Simple = 0,
        Customized = 1,
    }

    public enum FunctionFormType
    {
        MutiView = 0,
        SingleView = 1,
        Dialogue = 2,
    }

    public enum LayoutElementType
    {
        MainMenu = 10,
        Tray = 20,

        Area = 100,
        ViewMenuArea = 110,
        MainMenuArea = 111,
        ToolMenuArea = 112,

        ContentArea = 120,
        PublicContentArea = 121,

        Item = 200,
        ViewMenuItem = 210,
        DisplayOnlyItem = 220,
        TransactionOnlyItem = 230,

        //*ViewEventHandler = 231,
        //no use yet
        //ViewBeforeRenderHandler = 2311,
        //ViewAfterRenderHandler = 2312,
        //ViewAfterShowHandler = 2313,
        //ViewAfterHideHandler = 2314,

        //*ZoneEventHandler = 232, 
        ZoneBeforeRenderHandler = 2321,
        //ZoneBeforeInitiateVariablesHandler = 2322,
        ZoneAfterRenderHandler = 2322,

        FollowingTransactionItem = 233,
        DisplayAndTransactionItem = 240,

        VirtualItem = 250,

        //)zone 260
        Zone = 260,
    }

    public enum ZoneArrangementType
    {
        Positioning = 0,
        RowLining = 1,
    }



    public enum DropDownStripAlignType
    {
        Left = 1,
        Right = 2,
        Center = 3,
    }

    public enum ResizableDivisionStatus
    {
        None = 0,
        Hidden = 1,
        Shown = 2,
    }

    public enum ControlDockType
    {
        None = 0,
        Top = 1,
        Right = 2,
        Bottom = 3,
        Left = 4,
        Fill = 5
    }
    public enum ControlAlignPointType
    {
        LeftTop = 1,
        RightTop = 2,
        LeftBottom = 3,
        RightBottom = 4,
    }

    public enum ControlSensitiveType
    {
        None = 0,
        Focus = 1,
        Check = 2,
    }

    public enum ProgressCircleStyle
    {
        Beam = 0,
        Circle
    }

    public enum ListViewSortType
    {
        String = 0,
        Numeral = 1,
        FileSize = 2,
    }

    public enum PopupMessageFormFormat
    {
        Common = 0,
        MessageViewer = 1,
        RichTextViewer = 2,
    }

}
