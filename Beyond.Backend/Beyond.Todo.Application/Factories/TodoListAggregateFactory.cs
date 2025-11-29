using Beyond.Todo.Domain.Aggregates;
using Beyond.Todo.Domain.Entities;

namespace Beyond.Todo.Application.Factories;

public static class TodoListAggregateFactory
{
    public static TodoList CreateFromItems(IReadOnlyCollection<TodoItem> items)
    {
        var aggregate = new TodoList();
        foreach (var state in items.OrderBy(i => i.Id))
        {
            aggregate.AddItem(state.Id, state.Title, state.Description, state.Category);
            foreach (var progression in state.Progressions.OrderBy(p => p.Date))
            {
                aggregate.RegisterProgression(state.Id, progression.Date, progression.Percent);
            }
        }

        return aggregate;
    }
}
