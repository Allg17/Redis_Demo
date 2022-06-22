using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RedisDemo.Extensions
{
    public static class DistributeCacheExtensions
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache,
            string recordId, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTIme = null)
        {
            var option = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(absoluteExpireTime ?? TimeSpan.FromSeconds(60))
                .SetSlidingExpiration(unusedExpireTIme ?? TimeSpan.FromSeconds(50));

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData, option);
        }

        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache,
           string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);

            if (string.IsNullOrWhiteSpace(jsonData))
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }

}
