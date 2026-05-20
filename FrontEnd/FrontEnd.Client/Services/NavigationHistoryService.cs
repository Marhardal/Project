namespace FrontEnd.Client.Services
{
    // NavigationHistoryService.cs
    public class NavigationHistoryService
    {
        private readonly Stack<string> _history = new();

        public string? PreviousPage => _history.Count > 1 ? _history.Peek() : null;

        public void AddPage(string url)
        {
            if (_history.Count == 0 || _history.Peek() != url)
                _history.Push(url);
        }

        public string? GoBack()
        {
            if (_history.Count > 1)
            {
                _history.Pop(); // remove current
                return _history.Peek(); // return previous
            }
            return null;
        }
    }
}
