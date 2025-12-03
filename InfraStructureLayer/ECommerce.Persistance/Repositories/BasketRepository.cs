using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModule;
using StackExchange.Redis;

namespace ECommerce.Persistance.Repositories
{
    internal class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan timeToLive = default)
        {
            var basketJson = JsonSerializer.Serialize(basket);

            var isCreatedOrUpdated = await _database.StringSetAsync(basket.Id, basketJson, (timeToLive == default) ? TimeSpan.FromDays(7) : timeToLive);

            if (isCreatedOrUpdated)
            {
                var basketToReturn = await _database.StringGetAsync(basket.Id);

                return JsonSerializer.Deserialize<CustomerBasket>(basketToReturn!);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
           return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var basket = await _database.StringGetAsync(basketId);

            if (basket.IsNullOrEmpty)
                return null;
            
            else
                return JsonSerializer.Deserialize<CustomerBasket>(basket!);
            
        }
    }
}
