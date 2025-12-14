using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModule;
using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Service.Abstraction.Orders;
using ECommerce.Service.Specification.OrdersSpecification;
using ECommerce.Shared.CommenResponse;
using ECommerce.Shared.Dtos.Orders;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace ECommerce.Service.Orders
{
    internal class OrderService(
        IMapper mapper,
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork
        ) : IOrderService
    {
        public async Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string buyerEmail)
        {
            //1. Maps the provided shipping address to the order address entity.
            var orderAddress =  mapper.Map<ShippingAddress>(orderDto.AddressDto);

            //2. Retrieves the basket and validates its existence.
            var basket = await basketRepository.GetBasketAsync(orderDto.BasketId);
            if (basket is null)
                return Error.NotFound("Basket.NotFound", "The specified basket was not found.");

            //3. Creates a list of order items by fetching product details from the database and validating each product.
            List<ItemOrder> orderItems = new List<ItemOrder>();

            foreach(var item in basket.BasketItems)
            {
                var product = await unitOfWork.GenaricRepository<Product, int>().GetByIdAsync(item.Id);

                if (product is null)
                    return Error.NotFound("Product.NotFound", $"The product with ID {item.Id} was not found.");
                
                orderItems.Add(CreateOrderItem(item, product));
            }

            //4. Retrieves the selected delivery method and validates its existence.
            var deliveryMethod = await unitOfWork.GenaricRepository<DeliveryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId);

            if (deliveryMethod is null)
                return Error.NotFound("DeliveryMethod.NotFound", "The specified delivery method was not found.");

            //Calculates the subtotal of the order based on the items and their quantities.
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);

            //5. Creates a new Order with all relevant details.
            var order = new Order
            {
                DeliveryMethodId = deliveryMethod.Id,
                ShippingAddress = orderAddress,
                Items = orderItems,
                SubTotal = subtotal,
                UserEmail = buyerEmail
            };
            await unitOfWork.GenaricRepository<Order,Guid>().AddAsync(order);

            bool result =  await unitOfWork.SaveChangesAsync() > 0;

            if (!result)
                return Error.Faliure("Order.CreationFailed", "Failed to create the order.");

            //6. Returns a DTO containing the full order details to the client,
            //    including Id[OrderId], UserEmail,
            //    items[ProductName, PictureUrl, Price, Quantity],
            //    address, delivery method[ShortName],
            //    order status, OrderDate, subtotal, and total price

            var orderToReturn = mapper.Map<OrderToReturnDto>(order);

            return orderToReturn;

        }

        private static ItemOrder CreateOrderItem(BasketItem item, Product product)
        {
            var orderItem = new ItemOrder
            {
                Product = new ProductItemOrdered
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    PictureUrl = product.PictureUrl
                },
                Price = product.Price,
                Quantity = item.Quentity
            };

            return orderItem;
        }

        public async Task<Result<IEnumerable<DeliveryMethodDto>>> GetAllDeliveryMethodsAsync()
        {
            var deliveryMethods = await unitOfWork.GenaricRepository<DeliveryMethod, int>().GetAllAsync();

            if (deliveryMethods is null || !deliveryMethods.Any())
                return Error.NotFound("DeliveryMethods.NotFound", "No delivery methods were found.");

            var deliveryMethodDtos = mapper.Map<IEnumerable<DeliveryMethodDto>>(deliveryMethods);
            return Result<IEnumerable<DeliveryMethodDto>>.Ok(deliveryMethodDtos);
        }

        public async Task<Result<IEnumerable<OrderToReturnDto>>> GetAllOrderAsync(string buyerEmail)
        {
            var orderSpec = new OrderSpecification(buyerEmail);

            var orders = await unitOfWork.GenaricRepository<Order, Guid>().GetAllAsync(orderSpec);

            if (orders is null || !orders.Any())
                return Error.NotFound("Orders.NotFound", "No orders were found for the specified user.");

            var orderDtos = mapper.Map<IEnumerable<OrderToReturnDto>>(orders);

            return Result<IEnumerable<OrderToReturnDto>>.Ok(orderDtos);
        }

        public async Task<Result<OrderToReturnDto>> GetOrderByIdAsync(Guid orderId, string buyerEmail)
        {
            var orderSpec = new OrderSpecification(orderId, buyerEmail);

            var order = await unitOfWork.GenaricRepository<Order, Guid>().GetByIdAsync(orderSpec);

            if (order is null)
                return Error.NotFound("Order.NotFound", "The specified order was not found for the user.");

            var orderDto = mapper.Map<OrderToReturnDto>(order);

            return Result<OrderToReturnDto>.Ok(orderDto);
        }
    }
}
