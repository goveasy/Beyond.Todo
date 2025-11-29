namespace Beyond.Todo.Application.Abstractions;

public interface ITodoListRepository
{
    Task<int> GetNextId();
    Task<List<string>> GetAllCategories();
}
