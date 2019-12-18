namespace Ligg.WinForm.DataModel.Enums
{
    public enum LayoutElementType
    {
        Menu= 10,
        Tray = 20,

        Area = 100,
        ViewMenuArea = 110,
        ToolMenuArea = 111,

        ContentArea = 120,

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

}
