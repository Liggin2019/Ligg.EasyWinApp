
namespace Ligg.Winform.DataModel
{
    public class RenderedViewStatus
    {
        public string Name { get; set; }
        public bool IsChecked;
        //public string LastTimeLanguageCode;
        // public RenderedViewStatus(string name, bool isChecked)//string lastTimeLanguageCode)
        public RenderedViewStatus(string name, bool isChecked)
        {
            Name = name;
            IsChecked = isChecked;
            // LastTimeLanguageCode = lastTimeLanguageCode;
        }
    }

}
