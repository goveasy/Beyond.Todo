using Beyond.Todo.Application.Abstractions;
using Beyond.Todo.Application.UseCases;
using Beyond.Todo.Infraestructure.Persistence;
using Beyond.Todo.Infraestructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Beyond.Todo.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddSystemServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("TodoDb");
        services.AddDbContext<TodoEFDbContext>(optionsBuilder =>
            optionsBuilder.UseNpgsql(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 2, maxRetryDelay: TimeSpan.FromSeconds(2), null);
            })
        );


        services.AddScoped<ITodoItemRepository, TodoItemEFRepository>();
        services.AddScoped<ITodoListRepository, TodoListEFRepository>();
        services.AddScoped<CreateTodoItemHandler>();
        services.AddScoped<UpdateTodoItemDescriptionHandler>();
        services.AddScoped<RemoveTodoItemHandler>();


        return services;
    }
}
