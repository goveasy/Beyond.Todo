using Beyond.Todo.Application.Abstractions;
using Beyond.Todo.Application.UseCases;
using Beyond.Todo.Infraestructure.Caching;
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

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });

        services.AddScoped<ITodoItemRepository, TodoItemEFRepository>();
        services.AddScoped<ITodoListRepository, TodoListEFRepository>();
        services.AddScoped<ICategoryCacheService, CategoryCacheService>();
        services.AddScoped<CreateTodoItemHandler>();
        services.AddScoped<UpdateTodoItemDescriptionHandler>();
        services.AddScoped<RemoveTodoItemHandler>();
        services.AddScoped<RegisterTodoItemProgressionHandler>();
        services.AddScoped<GetTodoItemsHandler>();
        services.AddScoped<GetTodoItemsCategoriesHandler>();

        return services;
    }
}
