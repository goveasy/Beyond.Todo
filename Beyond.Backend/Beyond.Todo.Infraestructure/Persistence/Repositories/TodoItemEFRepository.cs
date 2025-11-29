using Beyond.Todo.Application.Abstractions;
using Beyond.Todo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Beyond.Todo.Infraestructure.Persistence.Repositories;

public sealed class TodoItemEFRepository: ITodoItemRepository
{
    private readonly TodoEFDbContext _context;

    public TodoItemEFRepository(TodoEFDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<TodoItem>> LoadAsync()
    {
        return  (await _context.TodoItems
            .Include(t => t.Progressions)
            .AsNoTracking()
            .ToListAsync()).AsReadOnly();
    }

    public async Task SaveAsync(TodoItem item)
    {
        var exists = await _context.TodoItems.AnyAsync(t => t.Id == item.Id);

        if (exists)
        {
            _context.TodoItems.Update(item);
        }
        else
        {
            await _context.TodoItems.AddAsync(item);
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.TodoItems.SingleOrDefaultAsync(t => t.Id == id);
        if (entity is not null)
        {
            _context.TodoItems.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
