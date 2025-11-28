using Beyond.Todo.Application.Abstractions;
using Beyond.Todo.Application.UseCases;
using Beyond.Todo.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Beyond.Todo.WebApi.Controllers;

[ApiController]
[Route("TodoItems")]
public class TodoItemsController : ControllerBase
{
    private readonly CreateTodoItemHandler _createTodoItemHandler;
    private readonly UpdateTodoItemDescriptionHandler _updateTodoItemDescriptionHandler;
    private readonly RegisterTodoItemProgressionHandler _registerTodoItemProgressionHandler;
    private readonly RemoveTodoItemHandler _removeTodoItemHandler;
    private readonly ITodoItemRepository _todoItemRepository;

    public TodoItemsController(
        CreateTodoItemHandler createTodoItemHandler,
        UpdateTodoItemDescriptionHandler updateTodoItemDescriptionHandler,
        RegisterTodoItemProgressionHandler registerTodoItemProgressionHandler,
        RemoveTodoItemHandler removeTodoItemHandler,
        ITodoItemRepository todoItemRepository)
    {
        _createTodoItemHandler = createTodoItemHandler;
        _updateTodoItemDescriptionHandler = updateTodoItemDescriptionHandler;
        _registerTodoItemProgressionHandler = registerTodoItemProgressionHandler;
        _removeTodoItemHandler = removeTodoItemHandler;
        _todoItemRepository = todoItemRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodoItem([FromBody] CreateTodoItemCommand command)
    {
        return Ok(await _createTodoItemHandler.Handle(command));
    }

    [HttpPut("{id}/description")]
    public async Task<ActionResult<UpdateTodoItemDescriptionResult>> UpdateTodoItemDescription(int id, [FromBody] UpdateTodoItemDescriptionCommand command)
    {
        var result = await _updateTodoItemDescriptionHandler.Handle(new UpdateTodoItemDescriptionCommand(id, command.NewDescription));
        return Ok(result);
    }

    [HttpPost("{id}/progressions")]
    public async Task<ActionResult<RegisterTodoItemProgressionResult>> RegisterTodoItemProgression(int id, [FromBody] RegisterTodoItemProgressionCommand command)
    {
        var result = await _registerTodoItemProgressionHandler.Handle(new RegisterTodoItemProgressionCommand(id, command.Date, command.Percent));
        return CreatedAtAction(nameof(RegisterTodoItemProgression), new { id }, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveTodoItem(int id)
    {
        await _removeTodoItemHandler.Handle(new RemoveTodoItemCommand(id));
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetTodoItems()
    {
        var items = await _todoItemRepository.LoadAsync();

        return Ok(items.Select(MapToDto));
    }

    private static TodoItemDto MapToDto(TodoItem item) => new TodoItemDto(
        item.Id,
        item.Title,
        item.Description,
        item.Category,
        item.IsCompleted,
        item.Progressions.Select(p => new ProgressionDto(p.Date, p.Percent))
    );
}

public record TodoItemDto(int Id, string Title, string Description, string Category, bool IsCompleted, IEnumerable<ProgressionDto> Progressions);
public record ProgressionDto(DateTime Date, decimal Percent);
