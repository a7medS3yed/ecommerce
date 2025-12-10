using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts;
using ECommerce.Service.Abstraction.Caches;

namespace ECommerce.Service.Caches
{
    public class CacheService(ICacheRepository cacheRepository) : ICacheService
    {
        public async Task<string?> GetAsync(string key)
        {
            return await cacheRepository.GetAsync(key);
        }

        public async Task SetAsync(string key, object value, TimeSpan timeToLive)
        {
            var valueResult = JsonSerializer.Serialize(value);
            await cacheRepository.SetAsync(key, valueResult, timeToLive); 
        }
    }
}
