namespace Ligg.Base.DataModel
{
    public class ValueTextBool
    {
        public ValueTextBool(string val, string txt, bool isOk)
        {
            Value = val; Text = txt; IsOk = isOk;
        }
        public ValueTextBool()
        {

        }
        public string Value { get; set; }
        public string Text { get; set; }
        public bool IsOk { get; set; }
    }
}
