
using Beyond.Todo.Application.Abstractions;
using Beyond.Todo.Domain.Aggregates;

namespace Beyond.Todo.Application.UseCases;

public sealed class UpdateTodoItemDescriptionHandler
{
    private readonly ITodoItemRepository _todoItemRepository;
    
    public UpdateTodoItemDescriptionHandler(
        ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public async Task<UpdateTodoItemDescriptionResult> Handle(UpdateTodoItemDescriptionCommand command)
    {
        var todoItems = await _todoItemRepository.LoadAsync();
        var aggregate = new TodoList(todoItems);

        aggregate.UpdateItem(command.Id, command.NewDescription);
        var item = aggregate.Items.First(item => item.Id == command.Id);
        await _todoItemRepository.SaveAsync(item);


        return new UpdateTodoItemDescriptionResult(item.Id, item.Description);
    }
}

public record UpdateTodoItemDescriptionCommand(int Id, string NewDescription);
public record UpdateTodoItemDescriptionResult(int Id, string UpdatedDescription);
