using Beyond.Todo.Application.Abstractions;

namespace Beyond.Todo.Application.UseCases;

public class GetTodoItemsCategoriesHandler
{
    private readonly ITodoListRepository _todoListRepository;
    private readonly ICategoryCacheService _categoryCacheService;
    public GetTodoItemsCategoriesHandler(ITodoListRepository todoItemRepository, ICategoryCacheService categoryCacheService)
    {
        _todoListRepository = todoItemRepository;
        _categoryCacheService = categoryCacheService;
    }

    public async Task<List<string>> Handle()
    {
        var categories = await _categoryCacheService.GetCategoriesAsync();

        if (categories is null || categories.Count == 0)
        {
            categories = await _todoListRepository.GetAllCategories();
            await _categoryCacheService.SetCategoriesAsync(categories);
        }

        return categories;
    }
}
