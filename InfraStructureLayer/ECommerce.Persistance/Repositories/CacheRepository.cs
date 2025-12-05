using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace ECommerce.Persistance.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDatabase _database;
        public CacheRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task<string?> GetAsync(string cacheKey)
        {
            var cacheValue = await _database.StringGetAsync(cacheKey);

            // ❗ Correct: check the VALUE, not the key
            if (cacheValue.IsNullOrEmpty)
                return null;

            return cacheValue.ToString();
        }

        public async Task SetAsync(string cacheKey, string cacheValue, TimeSpan timeToLive)
         => await _database.StringSetAsync(cacheKey, cacheValue, timeToLive);
    }
}
