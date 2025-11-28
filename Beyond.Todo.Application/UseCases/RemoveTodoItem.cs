using Beyond.Todo.Application.Abstractions;
using Beyond.Todo.Application.Factories;

namespace Beyond.Todo.Application.UseCases;

public sealed class RemoveTodoItemHandler
{
    private readonly ITodoItemRepository _todoItemRepository;
    public RemoveTodoItemHandler(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public async Task<RemoveTodoItemResult> Handle(RemoveTodoItemCommand command)
    {
        var todoItems= await _todoItemRepository.LoadAsync();
        var aggregate = TodoListAggregateFactory.CreateFromItems(todoItems);

        aggregate.RemoveItem(command.Id);

        await _todoItemRepository.DeleteAsync(command.Id);
        return new RemoveTodoItemResult(command.Id);
    }
}

public record RemoveTodoItemCommand(int Id);
public record RemoveTodoItemResult(int Id);