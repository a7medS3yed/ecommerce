using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModule;
using ECommerce.Service.Abstraction.Basket;
using ECommerce.Shared.Dtos.Baskets;

namespace ECommerce.Service.Baskets
{
    internal class BasketService(IBasketRepository basketRepository, IMapper mapper) : IBasketService
    {
        public async Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basketDto)
        {
            var basket = mapper.Map<CustomerBasket>(basketDto);
            var basketToCreate = await basketRepository.CreateOrUpdateBasketAsync(basket);
            return mapper.Map<BasketDto>(basketToCreate);
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await basketRepository.DeleteBasketAsync(basketId);
        }

        public async Task<BasketDto> GetBasketByIdAsync(string basketId)
        {
            var basket = await basketRepository.GetBasketAsync(basketId);
            return mapper.Map<BasketDto>(basket);
        }
    }

}
