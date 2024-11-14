namespace Domain.Interfaces.JukeBox
{
    public interface IGlobalListService
    {
        List<string> GetStrings();
        void AddString(string value);
        bool ContainsString(string value);
    }
}
