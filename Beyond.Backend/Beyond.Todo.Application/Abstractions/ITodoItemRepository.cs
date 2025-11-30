using Beyond.Todo.Domain.Entities;

namespace Beyond.Todo.Application.Abstractions;

public interface ITodoItemRepository
{
    Task<List<TodoItem>> LoadAsync();
    Task SaveAsync(TodoItem item);
    Task DeleteAsync(int id);
}
