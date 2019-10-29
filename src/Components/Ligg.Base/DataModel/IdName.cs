namespace Ligg.Base.DataModel
{
    public class IdName
    {
        public IdName(int id, string name) { _id = id; _name = name; }
        public IdName()
        {
            
        }
        private int _id;
        private string _name;

        public int Id { get { return _id; } set { _id = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public override string ToString() { return Name; }
    }
}
