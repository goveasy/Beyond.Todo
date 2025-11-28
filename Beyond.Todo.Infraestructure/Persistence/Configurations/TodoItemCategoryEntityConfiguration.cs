using Beyond.Todo.Infraestructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Beyond.Todo.Infraestructure.Persistence.Configurations;

public class TodoItemCategoryEntityConfiguration : IEntityTypeConfiguration<TodoItemCategory>
{
    public void Configure(EntityTypeBuilder<TodoItemCategory> builder)
    {
        builder.HasKey(c => c.Category);

        builder.Property(c => c.Category)
            .IsRequired().ValueGeneratedNever();

        builder.HasData(new TodoItemCategory() { Category="Work"}, new TodoItemCategory() { Category = "Personal" }, new TodoItemCategory() { Category = "Learning" });
    }
}
