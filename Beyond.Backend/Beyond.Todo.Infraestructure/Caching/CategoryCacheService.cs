using System.Text.Json;
using Beyond.Todo.Application.Abstractions;
using Microsoft.Extensions.Caching.Distributed;

namespace Beyond.Todo.Infraestructure.Caching;

public sealed class CategoryCacheService : ICategoryCacheService
{
    private const string CategoriesCacheKey = "todoitems:categories";
    private readonly IDistributedCache _distributedCache;
    private readonly DistributedCacheEntryOptions _cacheEntryOptions;

    public CategoryCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
        _cacheEntryOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        };
    }

    public async Task<List<string>?> GetCategoriesAsync()
    {
        var serializedCategories = await _distributedCache.GetStringAsync(CategoriesCacheKey);
        if (string.IsNullOrEmpty(serializedCategories))
        {
            return null;
        }

        return JsonSerializer.Deserialize<List<string>>(serializedCategories);
    }

    public async Task SetCategoriesAsync(List<string> categories)
    {
        var serializedCategories = JsonSerializer.Serialize(categories);
        await _distributedCache.SetStringAsync(CategoriesCacheKey, serializedCategories, _cacheEntryOptions);
    }
}
