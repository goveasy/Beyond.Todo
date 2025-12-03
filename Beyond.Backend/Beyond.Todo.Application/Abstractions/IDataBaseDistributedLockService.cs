namespace Beyond.Todo.Application.Abstractions;

public interface IDataBaseDistributedLockService
{
    public Task<IAsyncDisposable?> AcquireLockAsync(string key, TimeSpan timeout);
}
