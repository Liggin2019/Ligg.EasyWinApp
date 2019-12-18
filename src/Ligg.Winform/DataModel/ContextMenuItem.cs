namespace Ligg.WinForm.DataModel
{
    public class ContextMenuItem
    {
        public int Id;
        public int Type;//such as select one/many/none
        public string TypeName;//such as select one/many/none
        public string ControlTypeName;//Seperator
        public int ParentId;
        public string Name;
        public string DisplayName;
        public string Description;
        public string InvisibleFlag;
        public string DisabledFlag;
        public string Action;
        public string ImageUrl;
    }

}
