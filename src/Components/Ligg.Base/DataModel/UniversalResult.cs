namespace Ligg.Base.DataModel
{
    public class UniversalResult 
    {
        public UniversalResult(bool isOk, string msg)
        {
            IsOk = isOk; Message = msg;
        }
        public UniversalResult()
        {
            
        }

        public bool IsOk  { get; set; }
        public string Message { get; set; }
        public override string ToString() { return (IsOk+"; "+ Message); }
    }
}
