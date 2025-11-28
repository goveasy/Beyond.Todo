using Beyond.Todo.Domain.Entities;

namespace Beyond.Todo.Domain.Aggregates;

public class TodoList : ITodoList
{
    private readonly List<TodoItem> _items = new();
    private readonly TextWriter _writer;

    public TodoList(TextWriter? writer = null)
    {
        _writer = writer ?? Console.Out;
    }

    public IReadOnlyCollection<TodoItem> Items => _items.AsReadOnly();

    public void AddItem(int id, string title, string description, string category)
    {
        if (_items.Any(i => i.Id == id))
        {
            throw new InvalidOperationException($"TodoItem with id {id} already exists.");
        }

        var item = new TodoItem(id, title, description, category);
        _items.Add(item);
    }

    public void UpdateItem(int id, string description)
    {
        var item = FindItem(id);
        item.UpdateDescription(description);
    }

    public void RemoveItem(int id)
    {
        var item = FindItem(id);
        item.RemoveGuard();
        _items.Remove(item);
    }

    public void RegisterProgression(int id, DateTime dateTime, decimal percent)
    {
        var item = FindItem(id);
        item.AddProgression(new Progression(dateTime, percent));
    }

    public void PrintItems()
    {
        foreach (var item in _items.OrderBy(i => i.Id))
        {
            _writer.WriteLine($"{item.Id}) {item.Title} - {item.Description} ({item.Category}) Completed:{item.IsCompleted}");
            var cumulative = 0m;
            foreach (var progression in item.Progressions.OrderBy(p => p.Date))
            {
                cumulative += progression.Percent;
                var bar = BuildBar((int)cumulative);
                _writer.WriteLine($"{progression.Date} - {cumulative}% {bar}");
            }
        }
    }

    private static string BuildBar(int percent)
    {
        var filled = new string('O', percent);
        return $"|{filled}|";
    }

    private TodoItem FindItem(int id)
    {
        var item = _items.SingleOrDefault(i => i.Id == id);
        if (item is null)
        {
            throw new KeyNotFoundException($"TodoItem with id {id} not found");
        }

        return item;
    }
}
