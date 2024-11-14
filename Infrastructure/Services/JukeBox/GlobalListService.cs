using Domain.Interfaces.JukeBox;

namespace Infrastructure.Services.JukeBox
{
    public class GlobalListService : IGlobalListService
    {
        private readonly List<string> _globalStringList;

        public GlobalListService()
        {
            _globalStringList = new List<string>();
        }

        public List<string> GetStrings()
        {
            return _globalStringList;
        }

        public void AddString(string value)
        {
            if (!ContainsString(value))
            {
                _globalStringList.Add(value);
            }
        }

        public bool ContainsString(string value)
        {
            return _globalStringList.Any(s => s.Equals(value, StringComparison.OrdinalIgnoreCase));
        }
    }
}
