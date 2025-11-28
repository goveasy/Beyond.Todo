using Beyond.Todo.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Beyond.Todo.Infraestructure.Persistence.Repositories;

public class TodoListEFRepository : ITodoListRepository
{
    private readonly TodoEFDbContext _context;

    public TodoListEFRepository(TodoEFDbContext context)
    {
        _context = context;
    }
    public async Task<List<string>> GetAllCategories()
    {
        return await _context.TodoItemCategories.AsNoTracking()
            .Select(c => c.Category).ToListAsync();
    }

    public async Task<int> GetNextId()
    {
        var max = await _context.TodoItems.AsNoTracking().MaxAsync(t => (int?)t.Id) ?? 0;
        return max + 1;
    }
}
