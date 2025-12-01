using Beyond.Todo.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Beyond.Todo.WebApi;

public class DatabaseInitializer
{
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly ILogger<DatabaseInitializer> _logger;
    public DatabaseInitializer(ILogger<DatabaseInitializer> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        this.serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Initialize()
    {
        try
        {
            var scope = serviceScopeFactory.CreateScope();
            var contex = scope.ServiceProvider.GetRequiredService<TodoEFDbContext>();
            await contex.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while migrating the database");
        }
    }
}
