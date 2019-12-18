namespace Ligg.Base.DataModel
{
    public class ValueText
    {
        public ValueText(string val, string txt) { Value = val; Text = txt; }
        public ValueText()
        {
            
        }
        public string Value { get; set; }
        public string Text { get; set; }
        public override string ToString() { return Text; }
    }
}
