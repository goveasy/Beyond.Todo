namespace Beyond.Todo.Domain.Entities;

public sealed class TodoItem
{
    private readonly List<Progression> _progressions = new();

    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Category { get; private set; }
    public IReadOnlyCollection<Progression> Progressions => _progressions;
    public bool IsCompleted => GetCumulativePercent() >= 100;

    private TodoItem() { }

    public TodoItem(int id, string title, string description, string category)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required", nameof(title));
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required", nameof(description));
        if (string.IsNullOrWhiteSpace(category)) throw new ArgumentException("Category is required", nameof(category));

        Id = id;
        Title = title;
        Description = description;
        Category = category;
    }

    public void UpdateDescription(string description)
    {
        if (IsLocked()) throw new InvalidOperationException("Cannot update completed or half-completed items.");
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required", nameof(description));
        Description = description;
    }

    public void RemoveGuard()
    {
        if (IsLocked()) throw new InvalidOperationException("Cannot remove completed or half-completed items.");
    }

    public void AddProgression(Progression progression)
    {
        if (progression is null) throw new ArgumentException(nameof(progression));

        var currentPercent = GetCumulativePercent();

        if (IsCompleted)
        {
            throw new InvalidOperationException("Item is already completed.");
        }

        if (_progressions.Any())
        {
            var lastDate = _progressions.Max(p => p.Date);
            if (progression.Date <= lastDate)
            {
                throw new InvalidOperationException("Progression date must be greater than existing ones.");
            }
        }

        if (currentPercent + progression.Percent > 100)
        {
            throw new InvalidOperationException("Total progression cannot exceed 100%.");
        }

        _progressions.Add(progression);
    }

    public decimal GetCumulativePercent() => _progressions.Sum(p => p.Percent);

    private bool IsLocked() => GetCumulativePercent() > 50;
}
