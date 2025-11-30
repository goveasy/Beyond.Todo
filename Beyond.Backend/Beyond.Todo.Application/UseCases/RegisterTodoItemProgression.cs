

using Beyond.Todo.Application.Abstractions;
using Beyond.Todo.Application.Factories;
using Beyond.Todo.Domain.Aggregates;

namespace Beyond.Todo.Application.UseCases;

public sealed class RegisterTodoItemProgressionHandler
{
    private readonly ITodoItemRepository _todoItemRepository;

    public RegisterTodoItemProgressionHandler(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public async Task<RegisterTodoItemProgressionResult> Handle(RegisterTodoItemProgressionCommand command)
    {
        await Task.Delay(TimeSpan.FromSeconds(5)); // Simular asincronía
        var todoItems = await _todoItemRepository.LoadAsync();

        // el problema esta al recrear las progresiones se pierde el ID;
        var aggregate = new TodoList(todoItems);
       
        aggregate.RegisterProgression(command.Id, command.Date, command.Percent);
        var item = aggregate.Items.First(i => i.Id == command.Id);

        
        await _todoItemRepository.SaveAsync(item);
        
        return new RegisterTodoItemProgressionResult(command.Id, command.Date, command.Percent);
    }
}

public record RegisterTodoItemProgressionCommand(int Id, DateTime Date, decimal Percent);
public record RegisterTodoItemProgressionResult(int Id, DateTime Date, decimal Percent);