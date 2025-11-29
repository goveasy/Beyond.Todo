

using Beyond.Todo.Application.Abstractions;

namespace Beyond.Todo.Application.UseCases;

public class GetTodoItemsCategoriesHandler
{
    private readonly ITodoListRepository _todoListRepository;
    public GetTodoItemsCategoriesHandler(ITodoListRepository todoItemRepository)
    {
        _todoListRepository = todoItemRepository;
    }

    public async Task<List<string>> Handle()
    {
        return  await _todoListRepository.GetAllCategories();
    }
}

 
