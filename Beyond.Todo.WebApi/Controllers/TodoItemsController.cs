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
    private readonly GetTodoItemsHandler _todoItemHandler;
    private readonly GetTodoItemsCategoriesHandler _getTodoItemsCategoriesHandler;

    public TodoItemsController(
        CreateTodoItemHandler createTodoItemHandler,
        UpdateTodoItemDescriptionHandler updateTodoItemDescriptionHandler,
        RegisterTodoItemProgressionHandler registerTodoItemProgressionHandler,
        RemoveTodoItemHandler removeTodoItemHandler,
        GetTodoItemsHandler getTodoItemsHandler,
        GetTodoItemsCategoriesHandler getTodoItemsCategoriesHandler)
    {
        _createTodoItemHandler = createTodoItemHandler;
        _updateTodoItemDescriptionHandler = updateTodoItemDescriptionHandler;
        _registerTodoItemProgressionHandler = registerTodoItemProgressionHandler;
        _removeTodoItemHandler = removeTodoItemHandler;
        _todoItemHandler = getTodoItemsHandler;
        _getTodoItemsCategoriesHandler = getTodoItemsCategoriesHandler;
    }

    [HttpPost]
    public async Task<ActionResult<CreateTodoItemResult>> CreateTodoItem([FromBody] CreateTodoItemCommand command)
    {
        return Ok(await _createTodoItemHandler.Handle(command));
    }

    [HttpPut("{id}/description")]
    public async Task<ActionResult<UpdateTodoItemDescriptionResult>> UpdateTodoItemDescription(int id, [FromBody] UpdateTodoItemDescriptionRequest command)
    {
        var result = await _updateTodoItemDescriptionHandler.Handle(new UpdateTodoItemDescriptionCommand(id, command.NewDescription));
        return Ok(result);
    }

    [HttpPost("{id}/progressions")]
    public async Task<ActionResult<RegisterTodoItemProgressionResult>> RegisterTodoItemProgression(int id, [FromBody] RegisterProgresionRequest command)
    {
        var result = await _registerTodoItemProgressionHandler.Handle(new RegisterTodoItemProgressionCommand(id, command.Date, command.Percent));
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<RemoveTodoItemResult>> RemoveTodoItem(int id)
    {
        var result = await _removeTodoItemHandler.Handle(new RemoveTodoItemCommand(id));
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetTodoItems()
    {
        var result = await _todoItemHandler.Handle();
        return Ok(result);
    }

    [HttpGet("categories")]
    public async Task<ActionResult<List<string>>> GetTodoItemsCategories()
    {
        var result = await _getTodoItemsCategoriesHandler.Handle();
        return Ok(result);
    }
}

public record UpdateTodoItemDescriptionRequest(string NewDescription);
public record RegisterProgresionRequest(DateTime Date, decimal Percent);

