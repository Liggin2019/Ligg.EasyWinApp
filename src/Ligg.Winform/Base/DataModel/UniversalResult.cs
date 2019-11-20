namespace Ligg.Base.DataModel
{
    public class UniversalResult 
    {
        public UniversalResult(bool val, string txt) { IsOk = val; Text = txt; }
        public UniversalResult()
        {
            
        }

        public bool IsOk  { get; set; }
        public string Text { get; set; }
        public override string ToString() { return (IsOk+"; "+Text); }
    }
}
