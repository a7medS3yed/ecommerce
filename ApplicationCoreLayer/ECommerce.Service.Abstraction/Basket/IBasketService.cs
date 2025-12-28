using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Shared.Dtos.Baskets;

namespace ECommerce.Service.Abstraction.Basket
{
    public interface IBasketService
    {
        Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basketDto);
        Task<BasketDto> GetBasketByIdAsync(string basketId);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
