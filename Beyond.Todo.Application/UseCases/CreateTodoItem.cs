
using Beyond.Todo.Application.Abstractions;
using Beyond.Todo.Application.Factories;

namespace Beyond.Todo.Application.UseCases;

public sealed class CreateTodoItemHandler
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly ITodoListRepository _todoListRepository;

    public CreateTodoItemHandler(
        ITodoItemRepository todoItemRepository,
        ITodoListRepository todoListRepository)
    {
        _todoItemRepository = todoItemRepository;
        _todoListRepository = todoListRepository;
    }

    public async Task<CreateTodoItemResult> Handle(CreateTodoItemCommand command)
    {
        var categories = await _todoListRepository.GetAllCategories();
        
        if (!categories.Contains(command.Category))
        {
            throw new ArgumentException($"Category '{command.Category}' does not exist.");
        }

        var todoItems = await _todoItemRepository.LoadAsync();
        var aggregate = TodoListAggregateFactory.CreateFromItems(todoItems);
        var newId = await _todoListRepository.GetNextId();

        aggregate.AddItem(newId, command.Title, command.Description, command.Category);

        var newItem = aggregate.Items.First(item => item.Id == newId);

        await _todoItemRepository.SaveAsync(newItem);

        return new CreateTodoItemResult(
            newItem.Id,
            newItem.Title,
            newItem.Description,
            newItem.Category);
    }

}

public record CreateTodoItemCommand(string Title, string Description, string Category);
public record CreateTodoItemResult(int Id, string Title, string Description, string Category);
