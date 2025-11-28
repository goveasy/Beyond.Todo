using Beyond.Todo.Domain.Entities;

namespace Beyond.Todo.Application.Abstractions;

public interface ITodoItemRepository
{
    Task<IReadOnlyCollection<TodoItem>> LoadAsync();
    Task SaveAsync(TodoItem item);
    Task DeleteAsync(int id);
}
