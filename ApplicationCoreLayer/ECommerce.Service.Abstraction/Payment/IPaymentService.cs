using ECommerce.Shared.CommenResponse;
using ECommerce.Shared.Dtos.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Abstraction.Payment
{
    public interface IPaymentService
    {
        Task<Result<BasketDto>> CreateOrUpdatePaymentIntentAsync(string basketId);
    }
}
