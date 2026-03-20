namespace Oasis.Services;

public class ActivitySuggestionHistory {
    private readonly Queue<string> _recent = new();
    private const int MaxHistory = 10;

    public void Add(string title) {
        if (_recent.Count >= MaxHistory)
            _recent.Dequeue();
        _recent.Enqueue(title);
    }

    public IEnumerable<string> GetRecent() => _recent;
}