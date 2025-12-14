using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Shared.CommenResponse;
using ECommerce.Shared.Dtos.Orders;

namespace ECommerce.Service.Abstraction.Orders
{
    public interface IOrderService
    {
        Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string buyerEmail);
        Task<Result<IEnumerable<DeliveryMethodDto>>> GetAllDeliveryMethodsAsync();
        Task<Result<IEnumerable<OrderToReturnDto>>> GetAllOrderAsync(string buyerEmail);
        Task<Result<OrderToReturnDto>> GetOrderByIdAsync(Guid orderId, string buyerEmail);
    }
}
    