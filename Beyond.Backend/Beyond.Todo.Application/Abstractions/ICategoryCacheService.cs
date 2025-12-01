namespace Beyond.Todo.Application.Abstractions;

public interface ICategoryCacheService
{
    Task<List<string>?> GetCategoriesAsync();
    Task SetCategoriesAsync(List<string> categories);
}
