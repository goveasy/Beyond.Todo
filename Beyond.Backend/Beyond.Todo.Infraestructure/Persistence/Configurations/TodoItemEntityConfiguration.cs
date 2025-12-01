using Beyond.Todo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Beyond.Todo.Infraestructure.Persistence.Configurations;

public sealed class TodoItemEntityConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c=>c.Id).ValueGeneratedNever();

        var seedTodoItem = new TodoItem(
            1,
            "Construir el sistema Beyond Todo.",
            "Progreso de la construcion de la aplicacion beyond todo.",
            "Work");

        builder.HasData(seedTodoItem);


        builder.Navigation(c=>c.Progressions).UsePropertyAccessMode(PropertyAccessMode.Field);

        var today = DateTime.UtcNow.Date;

        builder.OwnsMany<Progression>(c => c.Progressions, tb =>
        {
            tb.WithOwner().HasForeignKey("TodoItemId");
            tb.HasKey(c => c.Id);

            tb.HasData(
               new { Id = 1, Date = today.AddDays(-2), Percent = 25m, TodoItemId = seedTodoItem.Id },
               new { Id = 2, Date = today.AddDays(-1), Percent = 35m, TodoItemId = seedTodoItem.Id },
               new { Id = 3, Date = today, Percent = 40m, TodoItemId = seedTodoItem.Id }
           );
        });

    }
}
