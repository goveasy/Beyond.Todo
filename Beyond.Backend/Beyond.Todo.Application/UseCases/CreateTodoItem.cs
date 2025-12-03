
using Beyond.Todo.Application.Abstractions;
using Beyond.Todo.Domain.Aggregates;

namespace Beyond.Todo.Application.UseCases;

public sealed class CreateTodoItemHandler
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly ITodoListRepository _todoListRepository;
    private readonly IDataBaseDistributedLockService _distributedLockService;
    private const string LockKey = "CreateTodoItemLock";

    public CreateTodoItemHandler(
        ITodoItemRepository todoItemRepository,
        ITodoListRepository todoListRepository,
        IDataBaseDistributedLockService distributedLockService)
    {
        _todoItemRepository = todoItemRepository;
        _todoListRepository = todoListRepository;
        _distributedLockService = distributedLockService;
    }

    public async Task<CreateTodoItemResult> Handle(CreateTodoItemCommand command)
    {
        var categories = await _todoListRepository.GetAllCategories();
        
        if (!categories.Contains(command.Category))
        {
            throw new ArgumentException($"Category '{command.Category}' does not exist.");
        }

        var todoItems = await _todoItemRepository.LoadAsync();
        var aggregate = new TodoList(todoItems);

        // Acquire a distributed lock to ensure that Id generation is thread-safe across multiple instances
        var distributedLock = await _distributedLockService.AcquireLockAsync(LockKey, TimeSpan.FromSeconds(5));

        if (distributedLock is null)
        {
            throw new Exception("Due to high demand, we are currently unable to process your request. Please try again later.");
        }

        try
        {
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
        finally 
        {
            await distributedLock.DisposeAsync();
        }
    }

}

public record CreateTodoItemCommand(string Title, string Description, string Category);
public record CreateTodoItemResult(int Id, string Title, string Description, string Category);
