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

        builder.Navigation(c=>c.Progressions).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany<Progression>(c => c.Progressions, tb =>
        {
            tb.WithOwner().HasForeignKey("TodoItemId");
            tb.HasKey(c => c.Id);
        });

    }
}
