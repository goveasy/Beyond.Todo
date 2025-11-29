using Beyond.Todo.Domain.Entities;
using Beyond.Todo.Infraestructure.Entities;
using Beyond.Todo.Infraestructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Beyond.Todo.Infraestructure.Persistence;

public sealed class TodoEFDbContext: DbContext
{
    public TodoEFDbContext(DbContextOptions<TodoEFDbContext> options) : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<TodoItemCategory> TodoItemCategories => Set<TodoItemCategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TodoItemEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TodoItemCategoryEntityConfiguration());
    }
}
