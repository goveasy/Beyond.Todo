using Beyond.Todo.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Beyond.Todo.WebApi.Controllers;

[ApiController]
[Route("TodoItems")]
public class TodoItemsController: ControllerBase
{
    private readonly CreateTodoItemHandler _createTodoItemHandler;

    public TodoItemsController(CreateTodoItemHandler createTodoItemHandler)
    {
        _createTodoItemHandler = createTodoItemHandler;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodoItem([FromBody] CreateTodoItemCommand command)
    {
        return Ok( await _createTodoItemHandler.Handle(command));
    }
}
