using ECommerce.Service.Abstraction.Orders;
using ECommerce.Shared.Dtos.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    public class OrderController(IOrderService orderService) : ApiBaseController
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await orderService.CreateOrderAsync(orderDto, email!);
            
            return HandleProblem(order);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetAllOrders()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await orderService.GetAllOrderAsync(email!);
            
            return HandleProblem(orders);
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(Guid id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await orderService.GetOrderByIdAsync(id, email!);
            
            return HandleProblem(order);
        }

        [AllowAnonymous]
        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> GetDeliveryMethods()
        {
            var deliveryMethods = await orderService.GetAllDeliveryMethodsAsync();
            
            return HandleProblem(deliveryMethods);
        }
    }
}
