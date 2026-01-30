using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace iPath.API.Services;

public class GroupCacheServer(IMemoryCache cache, IGroupService srv, ILogger<GroupCacheServer> logger) : IGroupCache
{
    private readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1);
    public async Task<GroupDto?> GetGroupAsync(Guid Id)
    {
        try
        {
            await _cacheLock.WaitAsync();
            var key = $"group_{Id}";
            if (!cache.TryGetValue<GroupDto>(key, out var group))
            {
                group = await srv.GetGroupByIdAsync(Id);
                var opts = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(15));
                cache.Set(key, group, opts);
            }
            return group;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }
        finally
        {
            _cacheLock.Release();
        }

        return null;
    }

    public async Task ClearGroup(Guid Id)
    {
        try
        {
            await _cacheLock.WaitAsync();
            var key = $"group_{Id}";
            cache.Remove(key);
        }
        finally
        {
            _cacheLock.Release();
        }
    }
}