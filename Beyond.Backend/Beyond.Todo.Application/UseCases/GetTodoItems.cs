using Beyond.Todo.Application.Abstractions;

namespace Beyond.Todo.Application.UseCases;

public class GetTodoItemsHandler
{
    private readonly ITodoItemRepository _todoItemRepository;
    public GetTodoItemsHandler(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public async Task<IReadOnlyCollection<TodoItemDto>> Handle()
    {
        var todoItems = await _todoItemRepository.LoadAsync();
        return todoItems.Select(item => new TodoItemDto(
            item.Id,
            item.Title,
            item.Description,
            item.Category,
            item.IsCompleted,
            item.GetCumulativePercent(),
            item.Progressions.Select(p => new ProgressionDto(p.Id, p.Date, p.Percent)).ToList()
        )).ToList();
    }
}

public record ProgressionDto(int Id, DateTime Date, decimal Percent);
public record TodoItemDto(int Id, string Title, string Description, string Category, bool IsCompleted, decimal CumulativePercent, IReadOnlyCollection<ProgressionDto> Progressions);

