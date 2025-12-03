

using Beyond.Todo.Application.Abstractions;
using Medallion.Threading;

namespace Beyond.Todo.Infraestructure.Persistence.Services;

public class DatabaseDistributedLockService : IDataBaseDistributedLockService
{
    IDistributedLockProvider _lockProvider;
    public DatabaseDistributedLockService(IDistributedLockProvider lockProvider)
    {
        _lockProvider = lockProvider;
    }
    public async Task<IAsyncDisposable?> AcquireLockAsync(string key, TimeSpan timeout)
    {
        return await _lockProvider.TryAcquireLockAsync(key, timeout);
    }
}
