﻿using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using ROC.WebApi.Core.Cache;

namespace ROC.Core.Infrastructure.Cache;

public class DistributedCacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<DistributedCacheService> _logger;

    public DistributedCacheService(IDistributedCache cache, ILogger<DistributedCacheService> logger)
    {
        (_cache, _logger) = (cache, logger);
    }

    public T? Get<T>(string key)
    {
        return Get(key) is { } data
            ? Deserialize<T>(data)
            : default;
    }

    public async Task<T> GetAsync<T>(string key, CancellationToken token = default)
    {
        return (await GetAsync(key, token) is { } data
            ? Deserialize<T>(data)
            : default) ?? throw new InvalidOperationException();
    }

    public void Refresh(string key)
    {
        try
        {
            _cache.Refresh(key);
        }
        catch
        {
            // can be ignored
        }
    }

    public async Task RefreshAsync(string key, CancellationToken token = default)
    {
        try
        {
            await _cache.RefreshAsync(key, token);
            _logger.LogDebug("refreshed cache with key : {Key}", key);
        }
        catch
        {
            // can be ignored
        }
    }

    public void Remove(string key)
    {
        try
        {
            _cache.Remove(key);
        }
        catch
        {
            // can be ignored
        }
    }

    public async Task RemoveAsync(string key, CancellationToken token = default)
    {
        try
        {
            await _cache.RemoveAsync(key, token);
        }
        catch
        {
            // can be ignored
        }
    }

    public void Set<T>(string key, T value, TimeSpan? slidingExpiration = null)
    {
        Set(key, Serialize(value), slidingExpiration);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null,
        CancellationToken cancellationToken = default)
    {
        return SetAsync(key, Serialize(value), slidingExpiration, cancellationToken);
    }

    private byte[]? Get(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        try
        {
            return _cache.Get(key);
        }
        catch
        {
            return null;
        }
    }

    private async Task<byte[]?> GetAsync(string key, CancellationToken token = default)
    {
        try
        {
            return await _cache.GetAsync(key, token);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    private void Set(string key, byte[] value, TimeSpan? slidingExpiration = null)
    {
        try
        {
            _cache.Set(key, value, GetOptions(slidingExpiration));
            _logger.LogDebug("cached data with key : {Key}", key);
        }
        catch
        {
            // can be ignored
        }
    }

    private async Task SetAsync(string key, byte[] value, TimeSpan? slidingExpiration = null,
        CancellationToken token = default)
    {
        try
        {
            await _cache.SetAsync(key, value, GetOptions(slidingExpiration), token);
            _logger.LogDebug("cached data with key : {Key}", key);
        }
        catch
        {
            // can be ignored
        }
    }

    private static byte[] Serialize<T>(T item)
    {
        return Encoding.Default.GetBytes(JsonSerializer.Serialize(item));
    }

    private static T Deserialize<T>(byte[] cachedData)
    {
        return JsonSerializer.Deserialize<T>(Encoding.Default.GetString(cachedData))!;
    }

    private static DistributedCacheEntryOptions GetOptions(TimeSpan? slidingExpiration)
    {
        var options = new DistributedCacheEntryOptions();
        if (slidingExpiration.HasValue)
            options.SetSlidingExpiration(slidingExpiration.Value);
        else
            options.SetSlidingExpiration(TimeSpan.FromMinutes(5));
        options.SetAbsoluteExpiration(TimeSpan.FromMinutes(15));
        return options;
    }
}