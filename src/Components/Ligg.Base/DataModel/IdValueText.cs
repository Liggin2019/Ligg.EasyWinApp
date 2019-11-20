namespace Ligg.Base.DataModel
{
    public class IdValueText
    {
        public IdValueText(int id, string val, string txt)
        {
            Id = id;Value = val; Text = txt; 
        }
        public IdValueText()
        {
            
        }
        public int Id { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public override string ToString() { return Text; }
    }
}
