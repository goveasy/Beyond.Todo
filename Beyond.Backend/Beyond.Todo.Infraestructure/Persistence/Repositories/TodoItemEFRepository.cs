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
        return  (await _context.TodoItems.AsNoTracking().ToListAsync()).AsReadOnly();
    }

    public async Task SaveAsync(TodoItem item)
    {
        var entity = await _context.TodoItems.SingleOrDefaultAsync(t => t.Id == item.Id);

        if (entity is null)
        {
            await _context.TodoItems.AddAsync(item);
        }
        else
        {
            _context.Entry(entity).CurrentValues.SetValues(item);
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
