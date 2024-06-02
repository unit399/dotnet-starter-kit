﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace ROC.WebApi.Core.Cache;

public static class CacheServiceExtensions
{
    public static T? GetOrSet<T>(this ICacheService cache, string key, Func<T> getItemCallback,
        TimeSpan? slidingExpiration = null)
    {
        var value = cache.Get<T>(key);

        if (value is not null) return value;

        value = getItemCallback();

        if (value is not null) cache.Set(key, value, slidingExpiration);

        return value;
    }

    public static async Task<T> GetOrSetAsync<T>(this ICacheService cache, string key, Func<Task<T>> task,
        TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
    {
        var value = await cache.GetAsync<T>(key, cancellationToken);

        if (value is not null) return value;

        value = await task();

        if (value is not null) await cache.SetAsync(key, value, slidingExpiration, cancellationToken);

        return value;
    }
}