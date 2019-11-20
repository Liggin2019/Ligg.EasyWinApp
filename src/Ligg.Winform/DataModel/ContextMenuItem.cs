namespace Ligg.Winform.DataModel
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
        public string VisibleFlag;
        public string EnabledFlag;
        public string Action;
        public string ImageUrl;
    }

}
